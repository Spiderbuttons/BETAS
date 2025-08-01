﻿using System;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Network;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class DancePartnered
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(NetDancePartner), nameof(NetDancePartner.SetCharacter))]
        public static void SetCharacter_Postfix(Character? value)
        {
            try
            {
                if (value is null) return;
                
                var danceItem = ItemRegistry.Create(value.Name);
                danceItem.modData["BETAS/DancePartnered/WasNPC"] = (value is NPC).ToString();
                danceItem.modData["BETAS/DancePartnered/WasFarmer"] = (value is Farmer).ToString();
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_DancePartnered", inputItem: danceItem, targetItem: danceItem);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.DancePartnered_NetDancePartner_SetCharacter_Postfix: \n" + ex);
            }
        }
    }
}