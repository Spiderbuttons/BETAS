﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class FishCaught
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.pullFishFromWater))]
        public static void pullFishFromWater_Postfix(FishingRod __instance, string fishId, int fishSize, int numCaught, int fishQuality, int fishDifficulty, bool fromFishPond, bool wasPerfect, bool isBossFish, bool treasureCaught)
        {
            try
            {
                var fishItem = ItemRegistry.Create(fishId, numCaught, fishQuality);
                if (fishItem.Category == -20 || fromFishPond) return;
                fishItem.modData["BETAS/FishCaught/Size"] = $"{fishSize}";
                fishItem.modData["BETAS/FishCaught/Difficulty"] = $"{fishDifficulty}";
                fishItem.modData["BETAS/FishCaught/IsPerfect"] = wasPerfect ? "true" : "false";
                fishItem.modData["BETAS/FishCaught/IsLegendary"] = isBossFish ? "true" : "false";
                fishItem.modData["BETAS/FishCaught/IsWithTreasure"] = treasureCaught ? "true" : "false";
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_FishCaught", targetItem: fishItem, location: __instance.lastUser.currentLocation);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.FishTrigger_FishingRod_pullFishFromWater_Postfix: \n" + ex);
            }
        }
    }
}