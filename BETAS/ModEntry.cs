using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using BETAS.Helpers;
using StardewValley.Triggers;

namespace BETAS
{
    internal sealed class ModEntry : Mod
    {
        internal static IMonitor ModMonitor { get; set; } = null!;
        internal static Harmony Harmony { get; set; } = null!;
        
        internal static IManifest Manifest { get; set; } = null!;

        public override void Entry(IModHelper helper)
        {
            ModMonitor = Monitor;
            Harmony = new Harmony(ModManifest.UniqueID);
            Manifest = ModManifest;

            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_LevelIncreased");
            TriggerActionManager.RegisterTrigger($"{Manifest.UniqueID}_ExperienceGained");
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

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
        }
    }
}