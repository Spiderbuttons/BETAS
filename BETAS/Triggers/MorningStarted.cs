using System;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class MorningStarted
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Game1), nameof(Game1.doMorningStuff))]
        public static void doMorningStuff_Postfix()
        {
            try
            {
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MorningStarted");
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.MorningStarted_Game1_doMorningStuff_Postfix: \n" + ex);
            }
        }
    }
}