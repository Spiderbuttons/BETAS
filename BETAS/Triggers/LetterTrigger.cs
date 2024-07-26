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

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class LetterTrigger
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.mailbox))]
        public static void mailbox_Postfix()
        {
            try
            {
                if (Game1.activeClickableMenu is LetterViewerMenu letter)
                {
                    var letterItem = ItemRegistry.Create(letter.mailTitle);
                    if (letter.moneyIncluded is not 0) letterItem.modData["BETAS/LetterRead/Money"] = $"{letter.moneyIncluded}";
                    letterItem.modData["BETAS/LetterRead/IsRecipe"] = $"{letter.learnedRecipe != ""}";
                    letterItem.modData["BETAS/LetterRead/IsQuestOrSpecialOrder"] = $"{letter.HasQuestOrSpecialOrder}";
                    letterItem.modData["BETAS/LetterRead/IsWithItem"] = $"{letter.itemsLeftToGrab()}";
                    if (letter.questID is not null) letterItem.modData["BETAS/LetterRead/Quest"] = $"{letter.questID}";
                    else if (letter.specialOrderId is not null) letterItem.modData["BETAS/LetterRead/SpecialOrder"] = $"{letter.questID}";
                    TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_LetterRead", targetItem: letterItem);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.LetterTrigger_GameLocation_mailbox_Postfix: \n" + ex);
            }
        }
    }
}