using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using Netcode;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Quests;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class AchievementUnlocked
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.achievements.OnValueAdded += OnAchievementUnlocked;
        }

        public static void OnAchievementUnlocked(int value)
        {
            Log.Alert($"Achievement Unlocked: {value}");
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_AchievementUnlocked");
        }
    }
}