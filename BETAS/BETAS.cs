#nullable enable
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

            Harmony.PatchAll();

            Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F5)
            {
                if (Game1.player.friendshipData.TryGetValue("Haley", out var friendship))
                {
                    Log.Debug(friendship.Status);
                }
            }
            
            if (e.Button == SButton.F6)
            {
                Log.Debug(Game1.player.GetDaysMarried());
            }

            if (e.Button == SButton.F8)
            {
                Harmony.UnpatchAll(ModManifest.UniqueID);
                Harmony.PatchAll();
            }
        }
    }
}