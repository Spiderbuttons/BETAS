#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;
using StardewValley.Triggers;

namespace BETAS
{
    internal sealed class BETAS : Mod
    {
        internal static IModHelper ModHelper { get; set; } = null!;
        internal static IMonitor ModMonitor { get; set; } = null!;
        internal static Harmony Harmony { get; set; } = null!;
        internal static IManifest Manifest { get; set; } = null!;
        internal static HashSet<string> LoadedMods { get; set; } = [];

        internal static MultiplayerNpcCache? Cache { get; set; }

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = Monitor;
            Harmony = new Harmony(ModManifest.UniqueID);
            Manifest = ModManifest;
            
            var types = Assembly.GetExecutingAssembly().GetTypes();
            var methods = types.SelectMany(t => t.GetMethods())
                .Where(m => !m.IsDefined(typeof(CompilerGeneratedAttribute), false));
            
            RegisterTriggers(ref types);
            RegisterActions(ref methods);
            RegisterQueries(ref methods);
            RegisterTokenizableStrings(ref methods);

            Harmony.PatchAll();

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Multiplayer.ModMessageReceived += this.OnModMessageReceived;
            helper.Events.Multiplayer.PeerConnected += this.OnPeerConnected;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            foreach (var mod in Helper.ModRegistry.GetAll())
            {
                if (Helper.ModRegistry.IsLoaded(mod.Manifest.UniqueID)) LoadedMods.Add(mod.Manifest.UniqueID);
            }
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
                var name = type.Name;
                Log.Trace($"Registering BETAS Trigger: {name}");
                TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_{name}");
            }
        }

        private static void RegisterActions(ref IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                if (!method.IsDefined(typeof(ActionAttribute), false)) continue;
                var name = method.GetCustomAttribute<ActionAttribute>()?.Name;
                if (name is null) continue;
                Log.Trace($"Registering BETAS Action: {name}");
                TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_{name}", method.CreateDelegate<TriggerActionDelegate>());
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

            if (e.Button == SButton.F5)
            {
                //
            }
        }
    }
}