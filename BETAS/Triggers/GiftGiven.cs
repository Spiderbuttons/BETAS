using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using StardewValley.Triggers;
using Object = StardewValley.Object;

namespace BETAS.Triggers
{
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
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_GiftGiven", targetItem: npcItem, inputItem: o, location: __instance.currentLocation, player: giver);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.GiftGiven_NPC_receiveGift_Postfix: \n" + ex);
            }
        }
    }
}