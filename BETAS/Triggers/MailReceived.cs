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
    static class MailReceived
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.mailReceived.OnValueAdded += OnMailReceived;
        }

        public static void OnMailReceived(string value)
        {
            var mailItem = ItemRegistry.Create(value);
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MailReceived", inputItem: mailItem, targetItem: mailItem);
        }
    }
}