using BETAS.Attributes;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    static class SaveLoaded
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        private static void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            var saveItem = ItemRegistry.Create(Game1.GetSaveGameName() ?? "SaveLoaded");
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_SaveLoaded", targetItem: saveItem);
        }
    }
}