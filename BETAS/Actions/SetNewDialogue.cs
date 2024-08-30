using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Menus;

namespace BETAS.Actions;

public static class SetNewDialogue
{
    // Add a new line of dialogue to an NPC, optionally adding on to their current dialogue instead of replacing it entirely.
    [Action("SetNewDialogue")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string npcName, out error, allowBlank: false) ||
            !ArgUtilityExtensions.TryGetTokenizable(args, 2, out string dialogue, out error, allowBlank: false) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableBool(args, 3, out bool append, out error, false))
        {
            error = "Usage: SetNewDialogue <NPC Name> <Dialogue> [Append]";
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        if (Game1.activeClickableMenu is not null && Game1.activeClickableMenu is DialogueBox dialogueBox &&
            dialogueBox.characterDialogue.speaker.Name.Equals(npcName))
        {
            dialogueBox.characterDialogue.dialogues.AddRange(new Dialogue(npc, null, dialogue).dialogues);
        }
        else
        {
            npc.setNewDialogue(new Dialogue(npc, null, dialogue), append);
        }

        return true;
    }
}