using System;
using System.Collections.Generic;
using System.Reflection;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class ItemShipped
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShippingMenu), nameof(ShippingMenu.parseItems))]
        public static void parseItems_Postfix(IList<Item> items)
        {
            try
            {
                foreach (var item in items)
                {
                    var itemCopy = item.getOne();
                    itemCopy.Stack = item.Stack;
                    TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ItemShipped", inputItem: itemCopy, targetItem: itemCopy);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ItemShipped_ShippingMenu_parseItems_Postfix: \n" + ex);
            }
        }
    }
}