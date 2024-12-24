#nullable enable
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class MenuChanged
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.Display.MenuChanged += OnMenuChanged;
        }
        
        private static void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            if (!Context.IsWorldReady) return;
            var newMenuItem = ItemRegistry.Create(e.NewMenu?.GetType().Name ?? string.Empty);
            var oldMenuItem = ItemRegistry.Create(e.OldMenu?.GetType().Name ?? string.Empty);
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MenuChanged", targetItem: newMenuItem, inputItem: oldMenuItem);
        }
    }
}