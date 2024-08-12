#nullable enable
using System;
using System.Linq;
using BETAS.GSQs;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using BETAS.Helpers;
using BETAS.Triggers;
using Netcode;
using StardewValley.Delegates;
using StardewValley.Monsters;
using StardewValley.Network;
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
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_TreeShook");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_BushHarvested");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_BombExploded");

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
                return;
            }

            if (e.Button == SButton.F8)
            {
                Harmony.UnpatchAll(ModManifest.UniqueID);
                Harmony.PatchAll();
            }
        }
    }
}