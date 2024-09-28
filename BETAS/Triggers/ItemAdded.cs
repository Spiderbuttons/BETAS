using System;
using System.Reflection;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class ItemAdded
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.OnItemReceived))]
        public static void OnItemReceived_Postfix(Farmer __instance, Item item, Item mergedIntoStack, int countAdded)
        {
            try
            {
                if (!__instance.IsLocalPlayer) return;
                
                var actualItem = mergedIntoStack ?? item;
                actualItem.Stack = countAdded;

                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ItemAdded", targetItem: actualItem);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ItemAdded_Farmer_OnItemReceived_Postfix: \n" + ex);
            }
        }
    }
}