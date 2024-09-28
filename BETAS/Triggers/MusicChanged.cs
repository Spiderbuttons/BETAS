using System;
using System.Reflection;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class MusicChanged
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Game1), nameof(Game1.changeMusicTrack))]
        public static void changeMusicTrack_Postfix(string newTrackName)
        {
            try
            {
                if (newTrackName is null || (Game1.IsGreenRainingHere() && !Game1.currentLocation.InIslandContext() && Game1.IsRainingHere(Game1.currentLocation) && !newTrackName.Equals("rain"))) return;
                
                var musicItem = ItemRegistry.Create(newTrackName);

                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MusicChanged", targetItem: musicItem);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.MusicChanged_Game1_changeMusicTrack_Postfix: \n" + ex);
            }
        }
    }
}