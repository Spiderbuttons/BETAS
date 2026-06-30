using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class SetNewDialogue
{
    // Add a new line of dialogue to an NPC, optionally adding on to their current dialogue instead of replacing it entirely.
    [Action("SetNewDialogue")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? npcName, out error, allowBlank: false, name: "string NPC") ||
            !TokenizableArgUtility.TryGet(args, 2, out string? dialogue, out error, allowBlank: false, name: "string Dialogue") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 3, out bool append, out error, name: "bool Add?"))
        {
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }
        
        // TODO: This may need to be run only on the host?
        
        try
        {
            var dialogueText = Game1.content.LoadString(dialogue);
            if (Game1.activeClickableMenu is not null && Game1.activeClickableMenu is StardewValley.Menus.DialogueBox dialogueBox &&
                dialogueBox.characterDialogue.speaker.Name.Equals(npcName))
            {
                dialogueText = $"Dummy text.#" + dialogueText; // https://discord.com/channels/137344473976799233/1275188689152114708/1503962597681004576
                dialogueBox.characterDialogue.dialogues.AddRange(new Dialogue(npc, dialogue, dialogueText).dialogues.Skip(1));
            }
            else
            {
                npc.setNewDialogue(new Dialogue(npc, dialogue, dialogueText), append);
            }
        }
        catch (Exception)
        {
            if (Game1.activeClickableMenu is not null && Game1.activeClickableMenu is StardewValley.Menus.DialogueBox dialogueBox2 &&
                dialogueBox2.characterDialogue.speaker.Name.Equals(npcName))
            {
                dialogue = $"Dummy text.#" + dialogue;
                dialogueBox2.characterDialogue.dialogues.AddRange(new Dialogue(npc, null, dialogue).dialogues.Skip(1));
            }
            else
            {
                npc.setNewDialogue(new Dialogue(npc, null, dialogue), append);
            }
        }

        return true;
    }
}