#nullable enable
using System;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using BETAS.Helpers;
using BETAS.Triggers;
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
            GameStateQuery.Register($"{Manifest.UniqueID}_ITEM_MOD_DATA_CONTAINS", ITEM_MOD_DATA_CONTAINS);
            GameStateQuery.Register($"{Manifest.UniqueID}_LOCATION_MOD_DATA", LOCATION_MOD_DATA);
            GameStateQuery.Register($"{Manifest.UniqueID}_LOCATION_MOD_DATA_RANGE", LOCATION_MOD_DATA_RANGE);
            GameStateQuery.Register($"{Manifest.UniqueID}_LOCATION_MOD_DATA_CONTAINS", LOCATION_MOD_DATA_CONTAINS);
            
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_ExperienceGained"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_FishCaught"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_LetterRead"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_CropHarvested"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_MonsterKilled"); // Done!
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_SpouseKissed");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_GiftGiven");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_PetPet");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_TrashcanChecked");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_MinecartUsed");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_PassedOut");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_BombExploded");

            Harmony.PatchAll();

            Helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }
        
        // GSQ for checking whether an item has a specific mod data key with a specific value. If the value is omitted, it just checks if the key exists at all.
        public static bool ITEM_MOD_DATA(string[] query, GameStateQueryContext context)
        {
            if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGetOptional(query, 3, out var value, out error))
            {
                return GameStateQuery.Helpers.ErrorResult(query, error);
            }
            if (item == null)
            {
                return false;
            }

            bool ignoreValue = !ArgUtility.HasIndex(query, 3);

            return item.modData.TryGetValue(key, out var data) &&
                   (string.Equals(data, value, StringComparison.OrdinalIgnoreCase) || ignoreValue);
        }
        
        // GSQ for checking whether an item has a specific mod data key with a value within a specific range. Values are parsed as ints.
        public static bool ITEM_MOD_DATA_RANGE(string[] query, GameStateQueryContext context)
        {
            if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGetInt(query, 3, out var minRange, out error) || !ArgUtility.TryGetOptionalInt(query, 4, out var maxRange, out error, int.MaxValue))
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
        
        // GSQ for checking whether a comma- or space-delimited list of values in item mod data contains a specific value.
        public static bool ITEM_MOD_DATA_CONTAINS(string[] query, GameStateQueryContext context)
        {
            if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGet(query, 3, out var value, out error, false))
            {
                return GameStateQuery.Helpers.ErrorResult(query, error);
            }
            if (item == null)
            {
                return false;
            }
            
            return item.modData.TryGetValue(key, out var data) && data.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList().Contains(value, StringComparer.OrdinalIgnoreCase);
        }
        
        // GSQ for checking whether a location has a specific mod data key with a specific value. If the value is omitted, it just checks if the key exists at all.
        public static bool LOCATION_MOD_DATA(string[] query, GameStateQueryContext context)
        {
            GameLocation location = context.Location;
            if (!GameStateQuery.Helpers.TryGetLocationArg(query, 1, ref location, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGetOptional(query, 3, out var value, out error))
            {
                return GameStateQuery.Helpers.ErrorResult(query, error);
            }
            if (location == null)
            {
                return false;
            }
            bool ignoreValue = !ArgUtility.HasIndex(query, 3);

            return location.modData.TryGetValue(key, out var data) &&
                   (string.Equals(data, value, StringComparison.OrdinalIgnoreCase) || ignoreValue);
        }
        
        // GSQ for checking whether a location has a specific mod data key with a value within a specific range. Values are parsed as ints.
        public static bool LOCATION_MOD_DATA_RANGE(string[] query, GameStateQueryContext context)
        {
            GameLocation location = context.Location;
            if (!GameStateQuery.Helpers.TryGetLocationArg(query, 1, ref location, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGetInt(query, 3, out var minRange, out error) || !ArgUtility.TryGetOptionalInt(query, 4, out var maxRange, out error, int.MaxValue))
            {
                return GameStateQuery.Helpers.ErrorResult(query, error);
            }
            if (location == null)
            {
                return false;
            }

            return location.modData.TryGetValue(key, out var data) && int.TryParse(data, out var dataInt) &&
                   dataInt >= minRange && dataInt <= maxRange;
        }
        
        // GSQ for checking whether a comma- or space-delimited list of values in item mod data contains a specific value.
        public static bool LOCATION_MOD_DATA_CONTAINS(string[] query, GameStateQueryContext context)
        {
            GameLocation location = context.Location;
            if (!GameStateQuery.Helpers.TryGetLocationArg(query, 1, ref location, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGet(query, 3, out var value, out error, false))
            {
                return GameStateQuery.Helpers.ErrorResult(query, error);
            }
            if (location == null)
            {
                return false;
            }
            
            return location.modData.TryGetValue(key, out var data) && data.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList().Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F5)
            {
                Game1.player.gainExperience(1, 100);
            }

            if (e.Button == SButton.F8)
            {
                Harmony.UnpatchAll(ModManifest.UniqueID);
                Harmony.PatchAll();
            }
        }
    }
}