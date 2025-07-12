using BETAS.Attributes;
using HarmonyLib;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class MailRemoved
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.mailReceived.OnValueRemoved += OnMailRemoved;
        }

        public static void OnMailRemoved(string value)
        {
            var mailItem = ItemRegistry.Create(value);
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MailRemoved", inputItem: mailItem, targetItem: mailItem);
        }
    }
}