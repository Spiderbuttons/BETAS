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
    static class ItemCrafted
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.craftingRecipes.OnValueAdded += OnItemCrafted;
        }

        public static void OnItemCrafted(string itemId, int count)
        {
            var item = ItemRegistry.Create(itemId);
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ItemCrafted", inputItem: item, targetItem: item);
        }
    }
}