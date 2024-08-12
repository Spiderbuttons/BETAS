using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.FruitTrees;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
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
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_BombExploded", location: __instance, player: who);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.BombExploded_GameLocation_explode_Postfix: \n" + ex);
            }
        }
    }
}