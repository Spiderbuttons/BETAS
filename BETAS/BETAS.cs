﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BETAS.AdvancedPermissions;
using BETAS.APIs;
using BETAS.Attributes;
using BETAS.Helpers;
using BETAS.Models;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Framework;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;
using StardewValley.Triggers;

namespace BETAS
{
    internal sealed class BETAS : Mod
    {
        internal static IModHelper ModHelper { get; private set; } = null!;
        internal static IMonitor ModMonitor { get; private set; } = null!;
        internal static Harmony Harmony { get; private set; } = null!;
        internal static IManifest Manifest { get; private set; } = null!;
        internal static HashSet<string> LoadedMods { get; } = [];
        internal static ModRegistry ModRegistry { get; private set; } = null!;
        internal static ISpaceCoreApi? SCAPI { get; private set; }

        internal static MultiplayerNpcCache? Cache { get; private set; }

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = Monitor;
            Harmony = new Harmony(ModManifest.UniqueID);
            Manifest = ModManifest;

            ModRegistry = SCore.Instance.ModRegistry;

            var types = Assembly.GetExecutingAssembly().GetTypes();
            var methods = types.SelectMany(t => t.GetMethods())
                .Where(m => !m.IsDefined(typeof(CompilerGeneratedAttribute), false));

            RegisterTriggers(ref types);
            RegisterTriggerActions(ref methods);
            RegisterMapActions(ref types);
            RegisterQueries(ref methods);
            RegisterTokenizableStrings(ref methods);

            Harmony.PatchAll();

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
            helper.Events.Content.AssetsInvalidated += this.OnAssetsInvalidated;
            helper.Events.GameLoop.UpdateTicked += this.InitializePatcher;
            helper.Events.Multiplayer.ModMessageReceived += this.OnModMessageReceived;
            helper.Events.Multiplayer.ModMessageReceived += MultiplayerSupport.ReceiveTrigger;
            helper.Events.Multiplayer.PeerConnected += this.OnPeerConnected;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            Log.Trace("Step into my parlour...");
            foreach (var mod in ModRegistry.GetAll())
            {
                if (Helper.ModRegistry.IsLoaded(mod.Manifest.UniqueID))
                {
                    LoadedMods.Add(mod.Manifest.UniqueID);
                    var perms = PermissionsManager.ParsePermissionsFromManifest(mod);
                    if (perms == Permissions.None) continue;
                    Log.Info($"{mod.Manifest.Name} ({mod.Manifest.UniqueID}) enabled permissions: {perms}");
                }
            }

            SCAPI = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            if (!Context.IsMainPlayer) Helper.Multiplayer.SendMessage("PeerConnected", "BETAS.NpcCacheRequest");
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            if (!Context.IsMainPlayer || !Context.IsMultiplayer) return;
            Cache ??= new MultiplayerNpcCache();
            Cache.UpdateCharacterList();
        }

        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Spiderbuttons.BETAS/HarmonyPatches"))
            {
                e.LoadFrom(() => new List<DynamicPatch>(), AssetLoadPriority.Medium);
            }
        }

        private void OnAssetsInvalidated(object? sender, AssetsInvalidatedEventArgs e)
        {
            if (e.NamesWithoutLocale.Any(asset => asset.IsEquivalentTo("Spiderbuttons.BETAS/HarmonyPatches")))
            {
                DynamicPatcher.Reset(ModManifest);
            }
        }

        private void InitializePatcher(object? sender, UpdateTickedEventArgs e)
        {
            if (e.Ticks < 4 || DynamicPatcher.IsInitialized) return;
            Log.Trace("Initializing patcher...");
            DynamicPatcher.Initialize(ModManifest);
            Helper.Events.GameLoop.UpdateTicked -= InitializePatcher;
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || Cache is null || !Context.IsMainPlayer || !Context.HasRemotePlayers)
                return;

            List<MultiplayerNpcCache.NpcCacheData> npcChanges = [];

            if (e.IsMultipleOf(60))
            {
                npcChanges.AddRange(Cache.CheckL1Cache());
            }

            if (e.IsMultipleOf(180))
            {
                npcChanges.AddRange(Cache.CheckL2Cache());
            }

            if (e.IsMultipleOf(600))
            {
                npcChanges.AddRange(Cache.CheckL3Cache());
            }

            if (npcChanges.Count > 0)
            {
                Helper.Multiplayer.SendMessage(npcChanges, "BETAS.NpcCache");
            }
        }

        private void OnPeerConnected(object? sender, PeerConnectedEventArgs e)
        {
            Cache ??= new MultiplayerNpcCache();
            Cache.CheckL1Cache();
            Cache.CheckL2Cache();
            Cache.CheckL3Cache();
        }

        private void OnModMessageReceived(object? sender, ModMessageReceivedEventArgs e)
        {
            switch (e.Type)
            {
                case "BETAS.NpcCache" when !Context.IsMainPlayer:
                {
                    Cache ??= new MultiplayerNpcCache();

                    foreach (var npcData in e.ReadAs<List<MultiplayerNpcCache.NpcCacheData>>()
                                 .Where(npcData => !Cache.L1Cache.TryAdd(npcData.NpcName, npcData)))
                    {
                        Cache.L1Cache[npcData.NpcName].CopyFrom(npcData);
                    }

                    break;
                }
                case "BETAS.NpcCacheRequest" when Context.IsMainPlayer && Cache is not null:
                    Log.Trace($"Received Npc Cache request from {e.FromPlayerID}");
                    List<MultiplayerNpcCache.NpcCacheData> npcCache = Cache.L1Cache.Values.ToList();
                    npcCache.AddRange(Cache.L2Cache.Values);
                    npcCache.AddRange(Cache.L3Cache.Values);
                    Helper.Multiplayer.SendMessage(npcCache, "BETAS.NpcCache");
                    break;
            }
        }

        private static void RegisterQueries(ref IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                if (!method.IsDefined(typeof(GSQAttribute), false)) continue;
                var name = method.GetCustomAttribute<GSQAttribute>()?.Name;
                if (name is null) continue;
                Log.Trace($"Registering BETAS Game State Query: {name}");
                GameStateQuery.Register($"{Manifest.UniqueID}_{name}", method.CreateDelegate<GameStateQueryDelegate>());
            }
        }

        private static void RegisterTriggers(ref Type[] types)
        {
            foreach (var type in types)
            {
                if (!type.IsDefined(typeof(TriggerAttribute), false)) continue;
                var initializeMethod = type.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static);
                initializeMethod?.Invoke(null, null);

                var name = type.Name;
                Log.Trace($"Registering BETAS Trigger: {name}");
                TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_{name}");
            }
        }

        private static void RegisterTriggerActions(ref IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                if (!method.IsDefined(typeof(ActionAttribute), false)) continue;
                var name = method.GetCustomAttribute<ActionAttribute>()?.Name;
                if (name is null) continue;
                Log.Trace($"Registering BETAS Action: {name}");
                TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_{name}",
                    method.CreateDelegate<TriggerActionDelegate>());
                GameLocation.RegisterTileAction($"{Manifest.UniqueID}_{name}", (_, args, _, _) =>
                {
                    if (TriggerActionManager.TryRunAction(args.Join(null, " "), out _, out Exception ex))
                        return true;
                    Log.Error($"Error in BETAS TileAction '{name}': {ex}");
                    return false;
                });
                GameLocation.RegisterTouchAction($"{Manifest.UniqueID}_{name}", (_, args, _, _) =>
                {
                    if (TriggerActionManager.TryRunAction(args.Join(null, " "), out _, out Exception ex))
                        return;
                    Log.Error($"Error in BETAS TouchAction '{name}': {ex}");
                });
            }
        }

        private static void RegisterMapActions(ref Type[] types)
        {
            foreach (var type in types)
            {
                if (!type.IsDefined(typeof(MapActionAttribute), false)) continue;
                var tileActionMethod = type.GetMethod("TileAction", BindingFlags.Public | BindingFlags.Static);
                var touchActionMethod = type.GetMethod("TouchAction", BindingFlags.Public | BindingFlags.Static);

                if (tileActionMethod is not null)
                {
                    var tileAction =
                        (Func<GameLocation, string[], Farmer, Point, bool>)Delegate.CreateDelegate(
                            typeof(Func<GameLocation, string[], Farmer, Point, bool>), tileActionMethod);
                    Log.Trace($"Registering BETAS TileAction: {type.Name}");
                    GameLocation.RegisterTileAction($"{Manifest.UniqueID}_{type.Name}", tileAction);
                }

                if (touchActionMethod is not null)
                {
                    var touchAction =
                        (Action<GameLocation, string[], Farmer, Vector2>)Delegate.CreateDelegate(
                            typeof(Action<GameLocation, string[], Farmer, Vector2>), touchActionMethod);
                    Log.Trace($"Registering BETAS TouchAction: {type.Name}");
                    GameLocation.RegisterTouchAction($"{Manifest.UniqueID}_{type.Name}", touchAction);
                }
            }
        }

        private static void RegisterTokenizableStrings(ref IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                if (!method.IsDefined(typeof(TKStringAttribute), false)) continue;
                var name = method.GetCustomAttribute<TKStringAttribute>()?.Name;
                if (name is null) continue;
                Log.Trace($"Registering BETAS Tokenizable String: {name}");
                TokenParser.RegisterParser($"{Manifest.UniqueID}_{name}", method.CreateDelegate<TokenParserDelegate>());
            }
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button is SButton.F2)
            {
                //
            }
        }
    }
}