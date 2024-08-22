#nullable enable

using System.Collections.Generic;
using System.Linq;
using BETAS.Actions;
using BETAS.GSQs;
using BETAS.Helpers;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS
{
    internal sealed class BETAS : Mod
    {
        internal static IMonitor ModMonitor { get; set; } = null!;
        internal static Harmony Harmony { get; set; } = null!;
        internal static IManifest Manifest { get; set; } = null!;
        internal static HashSet<string> LoadedMods { get; set; } = [];

        internal static MultiplayerNpcCache? Cache { get; set; }

        public override void Entry(IModHelper helper)
        {
            ModMonitor = Monitor;
            Harmony = new Harmony(ModManifest.UniqueID);
            Manifest = ModManifest;

            RegisterQueries();
            RegisterTriggers();
            RegisterActions();

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

        private static void RegisterQueries()
        {
            GameStateQuery.Register($"{Manifest.UniqueID}_ITEM_MOD_DATA", ItemModData.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_ITEM_MOD_DATA_RANGE", ItemModData.Query_Range);
            GameStateQuery.Register($"{Manifest.UniqueID}_ITEM_MOD_DATA_CONTAINS", ItemModData.Query_Contains);
            GameStateQuery.Register($"{Manifest.UniqueID}_LOCATION_MOD_DATA", LocationModData.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_LOCATION_MOD_DATA_RANGE", LocationModData.Query_Range);
            GameStateQuery.Register($"{Manifest.UniqueID}_LOCATION_MOD_DATA_CONTAINS", LocationModData.Query_Contains);
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_MOD_DATA", PlayerModData.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_MOD_DATA_RANGE", PlayerModData.Query_Range);
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_MOD_DATA_CONTAINS", PlayerModData.Query_Contains);
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_DAYS_MARRIED", PlayerDaysMarried.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_SPEED", PlayerSpeed.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_MOUNTED", PlayerMounted.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_SPOUSE_GENDER", PlayerSpouseGender.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_STARDROPS_FOUND", PlayerStardropsFound.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_NPC_LOCATION", NpcLocation.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_LOCATION_HAS_NPC", LocationHasNpc.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_NPC_NEAR_PLAYER", NpcNearPlayer.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_NPC_NEAR_NPC", NpcNearNpc.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_NPC_NEAR_AREA", NpcNearArea.Query);
            GameStateQuery.Register($"{Manifest.UniqueID}_HAS_MOD", HasMod.Query);
        }

        private static void RegisterTriggers()
        {
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_ExperienceGained"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_FishCaught"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_LetterRead"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_CropHarvested"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_MonsterKilled"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_NpcKissed"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_GiftGiven"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_AnimalPetted"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_GarbageChecked"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_PassedOut"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_MinecartUsed"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_RelationshipChanged"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_FloraShaken"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_BombExploded"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_LightningStruck"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_DialogueOpened"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_DamageTaken"); // Done!
        }

        private static void RegisterActions()
        {
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_SetNewDialogue", SetNewDialogue.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_WarpNpc", WarpNpc.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_WarpFarmer", WarpFarmer.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_MakeMachineReady", MakeMachineReady.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_TextAboveHead", TextAboveHead.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_EmoteNpc", EmoteNpc.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_EmoteFarmer", EmoteFarmer.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_Lightning", Lightning.Action);
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