using System;
using System.Collections.Generic;
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
    static class ShopOpened
    {
        // ReSharper disable once UnusedMember.Local
        static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(ShopMenu).GetConstructors();
        }
        
        public static void Postfix(ShopMenu __instance)
        {
            try
            {
                var shopItem = ItemRegistry.Create(__instance.ShopId);
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ShopOpened", targetItem: shopItem);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ShopOpened_ShopMenu_ctors_Postfix: \n" + ex);
            }
        }
    }
}