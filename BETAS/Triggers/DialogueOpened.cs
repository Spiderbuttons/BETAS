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
    static class DialogueOpened
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Game1), nameof(Game1.drawDialogue))]
        public static void drawDialogue_Postfix(Game1 __instance, NPC speaker)
        {
            try
            {
                var npcItem = ItemRegistry.Create(speaker.Name, speaker.CurrentDialogue.Count);
                npcItem.modData["BETAS/DialogueOpened/Age"] = speaker.Age switch
                {
                    0 => "Adult",
                    1 => "Teen",
                    _ => "Child",
                };
                npcItem.modData["BETAS/DialogueOpened/Gender"] = speaker.Gender switch
                {
                    Gender.Female => "Female",
                    Gender.Male => "Male",
                    _ => "Undefined"
                };
                npcItem.modData["BETAS/DialogueOpened/Friendship"] =
                    Game1.player.getFriendshipLevelForNPC(speaker.Name).ToString();
                npcItem.modData["BETAS/DialogueOpened/WasDatingFarmer"] =
                    Game1.player.friendshipData.ContainsKey(speaker.Name) &&
                    Game1.player.friendshipData[speaker.Name].IsDating()
                        ? "true"
                        : "false";
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_DialogueOpened", targetItem: npcItem,
                    location: speaker.currentLocation);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.DialogueOpened_Game1_drawDialogue_Postfix: \n" + ex);
            }
        }
    }
}