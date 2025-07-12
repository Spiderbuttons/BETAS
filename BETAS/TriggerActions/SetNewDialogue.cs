using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class SetNewDialogue
{
    // Add a new line of dialogue to an NPC, optionally adding on to their current dialogue instead of replacing it entirely.
    [Action("SetNewDialogue")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string? npcName, out error, allowBlank: false) ||
            !ArgUtilityExtensions.TryGetTokenizable(args, 2, out string? dialogue, out error, allowBlank: false) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableBool(args, 3, out bool append, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_SetNewDialogue <NPC Name> <Dialogue> [Add?]";
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }
        
        try
        {
            var dialogueText = Game1.content.LoadString(dialogue);
            if (Game1.activeClickableMenu is not null && Game1.activeClickableMenu is StardewValley.Menus.DialogueBox dialogueBox &&
                dialogueBox.characterDialogue.speaker.Name.Equals(npcName))
            {
                dialogueBox.characterDialogue.dialogues.AddRange(new Dialogue(npc, dialogue, dialogueText).dialogues);
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
                dialogueBox2.characterDialogue.dialogues.AddRange(new Dialogue(npc, null, dialogue).dialogues);
            }
            else
            {
                npc.setNewDialogue(new Dialogue(npc, null, dialogue), append);
            }
        }

        return true;
    }
}