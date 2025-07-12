using BETAS.Attributes;
using HarmonyLib;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class MineralFound
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.mineralsFound.OnValueAdded += OnMineralFound;
        }

        public static void OnMineralFound(string mineral, int count)
        {
            var item = ItemRegistry.Create(mineral);
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MineralFound", inputItem: item, targetItem: item);
        }
    }
}