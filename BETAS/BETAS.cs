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

        // Raised whenever the player gains experience. ItemId is the name of the skill, ItemStack is the amount of experience gained, and ItemQuality is whether the experience gain resulted in a level up (0 if not, 1 if it did).
        // SpaceCore custom skills are not supported.
        public static void Trigger_ExperienceGained( int levelUp, int skillID, int howMuch)
        {
            var skill = skillID switch
            {
                0 => "Farming",
                1 => "Fishing",
                2 => "Foraging",
                3 => "Mining",
                4 => "Combat",
                _ => null
            };
            var skillItem = ItemRegistry.Create(skill);
            skillItem.Stack = howMuch;
            skillItem.Quality = levelUp;
            TriggerActionManager.Raise($"{Manifest.UniqueID}_LevelIncreased", targetItem: skillItem);
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