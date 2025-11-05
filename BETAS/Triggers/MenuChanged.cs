using BETAS.Attributes;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    static class MenuChanged
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.Display.MenuChanged += OnMenuChanged;
        }
        
        private static void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            var newMenuItem = ItemRegistry.Create(e.NewMenu?.GetType().Name ?? "null");
            var oldMenuItem = ItemRegistry.Create(e.OldMenu?.GetType().Name ?? "null");
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MenuChanged", targetItem: newMenuItem, inputItem: oldMenuItem);
        }
    }
}