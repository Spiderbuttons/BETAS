﻿using System;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Triggers;
using Object = StardewValley.Object;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class GiftGiven
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(NPC), nameof(NPC.receiveGift))]
        public static void receiveGift_Postfix(NPC __instance, Object o, Farmer giver)
        {
            try
            {
                var npcItem = ItemRegistry.Create(__instance.Name);
                npcItem.modData["BETAS/GiftGiven/WasBirthday"] = __instance.isBirthday() ? "true" : "false";
                npcItem.modData["BETAS/GiftGiven/Taste"] = __instance.getGiftTasteForThisItem(o) switch
                {
                    0 => "Love",
                    6 => "Hate",
                    2 => "Like",
                    4 => "Dislike",
                    7 => "Special",
                    _ => "Neutral"
                };
                npcItem.modData["BETAS/GiftGiven/Friendship"] =
                    giver.getFriendshipLevelForNPC(__instance.Name).ToString();
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_GiftGiven", targetItem: npcItem, inputItem: o,
                    location: __instance.currentLocation, player: giver);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.GiftGiven_NPC_receiveGift_Postfix: \n" + ex);
            }
        }
    }
}