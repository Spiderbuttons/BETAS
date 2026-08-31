using BETAS.Attributes;
using HarmonyLib;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class PhoneRang
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Phone), nameof(Phone.Ring))]
        public static void Ring_Postfix(Phone __instance, string callId)
        {
            if (string.IsNullOrWhiteSpace(callId)) return; // Means it actually STOPPED ringing.

            Item ringItem = ItemRegistry.Create(callId);
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_PhoneRang", targetItem: ringItem, inputItem: __instance, location: __instance.Location);
        }
    }
}