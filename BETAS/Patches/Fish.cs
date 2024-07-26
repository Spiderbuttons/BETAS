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
    static class fishPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FishingRod), nameof(FishingRod.pullFishFromWater))]
        public static void pullFishFromWater_Postfix(string fishId, int fishSize, int fishQuality, int fishDifficulty, bool wasPerfect, bool isBossFish)
        {
            try
            {
                var fishItem = ItemRegistry.Create(fishId);
                fishItem.Stack = fishSize;
                fishItem.Quality = fishQuality;
                TriggerActionManager.Raise("BETAS_FishCaught", targetItem: fishItem);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.Fish_FishingRod_pullFishFromWater_Postfix: \n" + ex);
            }
        }
    }
}