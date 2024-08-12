using System;
using HarmonyLib;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class PassedOut
    {
        public static void Trigger(Farmer who, GameLocation location)
        {
            var passoutItem = ItemRegistry.Create(location.Name);
            passoutItem.modData["BETAS/PassedOut/Time"] = Game1.timeOfDay.ToString();
            passoutItem.modData["BETAS/PassedOut/IsUpTooLate"] = Game1.timeOfDay >= 2600 ? "true" : "false";
            if (who.stamina <= -15f && who.CurrentTool is not null) passoutItem.modData["BETAS/PassedOut/Tool"] = who.CurrentTool.QualifiedItemId;
            passoutItem.modData["BETAS/PassedOut/IsExhausted"] = who.stamina <= -15f ? "true" : "false";
            passoutItem.modData["BETAS/PassedOut/IsSafe"] = (location is FarmHouse) || (location is IslandFarmHouse) || (location is Cellar) || location.HasMapPropertyWithValue("PassOutSafe") ? "true" : "false";
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_PassedOut", targetItem: passoutItem, location: location, player: who);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.passOutFromTired))]
        public static void passOutFromTired_Postfix(Farmer who)
        {
            try
            {
                if (!who.IsLocalPlayer) return;
                Trigger(who, who.currentLocation);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.PassedOut_Farmer_passOutFromTired_Postfix: \n" + ex);
            }
        }
    }
}