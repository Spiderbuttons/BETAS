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
    static class GenderChanged
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.changeGender))]
        public static void changeGender_Postfix()
        {
            try
            {
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_GenderChanged");
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.GenderChanged_Farmer_changeGender_Postfix: \n" + ex);
            }
        }
    }
}