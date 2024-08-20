using System;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class BombExploded
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.explode))]
        public static void explode_Postfix(GameLocation __instance, int radius, Farmer who)
        {
            try
            {
                var bombItem = ItemRegistry.Create("(O)287");
                bombItem.modData["BETAS/BombExploded/Radius"] = radius.ToString();

                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_BombExploded", location: __instance,
                    player: who);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.BombExploded_GameLocation_explode_Postfix: \n" + ex);
            }
        }
    }
}