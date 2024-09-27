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
    static class SwimmingChanged
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.swimming.fieldChangeEvent += OnSwimmingChanged;
        }

        public static void OnSwimmingChanged(NetBool field, bool oldValue, bool newValue)
        {
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_SwimmingChanged");
        }
    }
}