#nullable enable
using System;
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

        public override void Entry(IModHelper helper)
        {
            ModMonitor = Monitor;
            Harmony = new Harmony(ModManifest.UniqueID);
            Manifest = ModManifest;
            
            RegisterQueries();
            RegisterTriggers();
            RegisterActions();

            Harmony.PatchAll();

            Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
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
        }

        private static void RegisterActions()
        {
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_SetNewDialogue", SetNewDialogue.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_WarpCharacter", WarpCharacter.Action);
            TriggerActionManager.RegisterAction($"{Manifest.UniqueID}_WarpFarmer", WarpFarmer.Action);
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F5)
            {
                for (int i = 0; i < 100; i++) Log.Debug($"\x1b[{i}mTEST\x1b[0m");
            }
            
            if (e.Button == SButton.F6)
            {
                string action = "Spiderbuttons.BETAS_WarpFarmer BusStop 22 15 1";
                if (!TriggerActionManager.TryRunAction(action, out string error, out Exception ex))
                    Log.Error($"Failed running action '{action}': {error}\n{ex}");
            }

            if (e.Button == SButton.F8)
            {
                Harmony.UnpatchAll(ModManifest.UniqueID);
                Harmony.PatchAll();
            }
        }
    }
}