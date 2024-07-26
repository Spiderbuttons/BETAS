#nullable enable
using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using BETAS.Helpers;
using StardewValley.Delegates;
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

            GameStateQuery.Register($"{Manifest.UniqueID}_ITEM_MOD_DATA", ITEM_MOD_DATA);
            GameStateQuery.Register($"{Manifest.UniqueID}_ITEM_MOD_DATA_RANGE", ITEM_MOD_DATA_RANGE);
            
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_ExperienceGained");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_FishCaught");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_LetterRead");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_CropHarvested");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_CropHarvestedByJunimo");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_CropDied");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_MachineOutputCollected");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_EnemyKilled");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_BedEntered");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_SpouseKissed");

            Harmony.PatchAll();

            Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }
        
        // GSQ for checking whether an item has a specific mod data key with a specific value.
        public static bool ITEM_MOD_DATA(string[] query, GameStateQueryContext context)
        {
            if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGet(query, 3, out var value, out error))
            {
                return GameStateQuery.Helpers.ErrorResult(query, error);
            }
            if (item == null)
            {
                return false;
            }

            return item.modData.TryGetValue(key, out var data) &&
                   string.Equals(data, value, StringComparison.OrdinalIgnoreCase);
        }
        
        // GSQ for checking whether an item has a specific mod data key with a value within a specific range. Values are parsed as ints.
        public static bool ITEM_MOD_DATA_RANGE(string[] query, GameStateQueryContext context)
        {
            if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGet(query, 3, out var value, out error) || !ArgUtility.TryGetInt(query, 4, out var minRange, out error) || !ArgUtility.TryGetOptionalInt(query, 5, out var maxRange, out error, int.MaxValue))
            {
                return GameStateQuery.Helpers.ErrorResult(query, error);
            }
            if (item == null)
            {
                return false;
            }

            return item.modData.TryGetValue(key, out var data) && int.TryParse(data, out var dataInt) &&
                   dataInt >= minRange && dataInt <= maxRange;
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F5)
            {
                Game1.player.gainExperience(1, 500);
            }

            if (e.Button == SButton.F8)
            {
                Harmony.UnpatchAll(ModManifest.UniqueID);
                Harmony.PatchAll();
            }
        }
    }
}