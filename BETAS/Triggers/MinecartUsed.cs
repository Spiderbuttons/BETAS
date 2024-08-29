using System;
using System.Reflection;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.GameData.Minecarts;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class MinecartUsed
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.MinecartWarp))]
        public static void MinecartWarp_Postfix(GameLocation __instance, MinecartDestinationData destination)
        {
            try
            {
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MinecartUsed",
                    location: Game1.RequireLocation(destination.TargetLocation));
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.MinecartUsed_GameLocation_MinecartWarp_Postfix: \n" + ex);
            }
        }
    }
}