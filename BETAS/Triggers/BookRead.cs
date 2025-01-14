using System;
using System.Reflection;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.GameData.Minecarts;
using StardewValley.Triggers;
using Object = StardewValley.Object;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class BookRead
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Object), "readBook")]
        public static void readBook_Postfix(Object __instance, GameLocation location)
        {
            try
            {
                DelayedAction.functionAfterDelay(() =>
                {
                    TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_BookRead",
                        location: location, targetItem: ItemRegistry.Create(__instance.QualifiedItemId));
                }, 1000);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.BookRead_Object_readBook_Postfix: \n" + ex);
            }
        }
    }
}