using System;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class LetterRead
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.mailbox))]
        public static void mailbox_Postfix(GameLocation __instance)
        {
            try
            {
                if (Game1.activeClickableMenu is LetterViewerMenu letter)
                {
                    var letterItem = ItemRegistry.Create(letter.mailTitle);
                    if (letter.moneyIncluded is not 0)
                        letterItem.modData["BETAS/LetterRead/Money"] = $"{letter.moneyIncluded}";
                    letterItem.modData["BETAS/LetterRead/WasRecipe"] = $"{letter.learnedRecipe != ""}";
                    letterItem.modData["BETAS/LetterRead/WasQuestOrSpecialOrder"] = $"{letter.HasQuestOrSpecialOrder}";
                    letterItem.modData["BETAS/LetterRead/WasWithItem"] = $"{letter.itemsLeftToGrab()}";
                    if (letter.questID is not null) letterItem.modData["BETAS/LetterRead/Quest"] = $"{letter.questID}";
                    else if (letter.specialOrderId is not null)
                        letterItem.modData["BETAS/LetterRead/SpecialOrder"] = $"{letter.questID}";
                    TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_LetterRead", targetItem: letterItem,
                        location: __instance);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.LetterRead_GameLocation_mailbox_Postfix: \n" + ex);
            }
        }
    }
}