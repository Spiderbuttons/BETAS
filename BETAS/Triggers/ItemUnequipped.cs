using System;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class ItemUnequipped
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Item), nameof(Item.onUnequip))]
        public static void onUnequip_Postfix(Item __instance, Farmer who)
        {
            try
            {
                var itemItem = __instance.getOne();
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ItemUnequipped", targetItem: itemItem,
                    location: who.currentLocation);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ItemUnequipped_Item_onUnequip_Postfix: \n" + ex);
            }
        }
    }
}