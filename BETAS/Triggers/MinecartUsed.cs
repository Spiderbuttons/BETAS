using System;
using HarmonyLib;
using BETAS.Helpers;
using StardewValley;
using StardewValley.GameData.Minecarts;
using StardewValley.Locations;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class MinecartUsed
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.MinecartWarp))]
        public static void MinecartWarp_Postfix(GameLocation __instance, MinecartDestinationData destination)
        {
            try
            {
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MinecartUsed", location: Game1.RequireLocation(destination.TargetLocation));
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.MinecartTrigger_GameLocation_MinecartWarp_Postfix: \n" + ex);
            }
        }
    }
}