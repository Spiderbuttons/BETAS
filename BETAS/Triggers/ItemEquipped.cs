using System;
using System.Reflection;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Tools;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class ItemEquipped
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Item), nameof(Item.onEquip))]
        public static void onEquip_Postfix(Item __instance, Farmer who)
        {
            try
            {
                var itemItem = __instance.getOne();
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ItemEquipped", targetItem: itemItem,
                    location: who.currentLocation);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ItemEquipped_Item_onEquip_Postfix: \n" + ex);
            }
        }
    }
}