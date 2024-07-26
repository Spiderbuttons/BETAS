using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;
using StardewValley.Triggers;

namespace BETAS.Patches
{
    [HarmonyPatch]
    static class FishTrigger
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.pullFishFromWater))]
        public static void pullFishFromWater_Postfix(string fishId, int fishSize, int numCaught, int fishQuality, int fishDifficulty, bool fromFishPond, bool wasPerfect, bool isBossFish, bool treasureCaught)
        {
            try
            {
                var fishItem = ItemRegistry.Create(fishId, numCaught, fishQuality);
                if (fishItem.Category == -20 || fromFishPond) return;
                fishItem.modData["BETAS/FishCaught/Size"] = $"{fishSize}";
                fishItem.modData["BETAS/FishCaught/Difficulty"] = $"{fishDifficulty}";
                fishItem.modData["BETAS/FishCaught/Perfect"] = wasPerfect ? "true" : "false";
                fishItem.modData["BETAS/FishCaught/Legendary"] = isBossFish ? "true" : "false";
                fishItem.modData["BETAS/FishCaught/Treasure"] = treasureCaught ? "true" : "false";
                Log.Debug(fishItem.ItemId);
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_FishCaught", inputItem: fishItem);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.Fish_FishingRod_pullFishFromWater_Postfix: \n" + ex);
            }
        }
    }
}