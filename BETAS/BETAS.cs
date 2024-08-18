#nullable enable
using System;
using System.Collections.Generic;
using BETAS.Actions;
using BETAS.GSQs;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using BETAS.Helpers;
using StardewValley.Triggers;

namespace BETAS
{
    internal sealed class BETAS : Mod
    {
        internal static IMonitor ModMonitor { get; set; } = null!;
        internal static Harmony Harmony { get; set; } = null!;
        internal static IManifest Manifest { get; set; } = null!;
        internal static HashSet<string> LoadedMods { get; set; } = [];

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
            Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }
        
        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            foreach (var mod in Helper.ModRegistry.GetAll())
            {
                if (Helper.ModRegistry.IsLoaded(mod.Manifest.UniqueID)) LoadedMods.Add(mod.Manifest.UniqueID);
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
            GameStateQuery.Register($"{Manifest.UniqueID}_PLAYER_QI_GEMS", PlayerQiGems.Query);
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
                return;
            }
        }
    }
}