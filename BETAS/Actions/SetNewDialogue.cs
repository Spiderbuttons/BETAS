using System;
using System.Linq;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Menus;
using StardewValley.TokenizableStrings;

namespace BETAS.Actions;

public static class SetNewDialogue
{
    // Add a new line of dialogue to an NPC, optionally adding on to their current dialogue instead of replacing it entirely.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtility.TryGet(args, 1, out string npcName, out error, allowBlank: false) || !ArgUtility.TryGet(args, 2, out string dialogue, out error, allowBlank: false) || !ArgUtility.TryGetOptionalBool(args, 3, out bool append, out error, false) || !ArgUtility.TryGetOptionalInt(args, 4, out int delay, out error, 0))
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

        try
        {
            dialogue = Game1.content.LoadString(dialogue);
        }
        catch (Exception ex)
        {
            Log.Trace("BETAS.Actions.SetNewDialogue: Argument is not a translation key.");
        }

        if (Game1.activeClickableMenu is not null && Game1.activeClickableMenu is DialogueBox dialogueBox && dialogueBox.characterDialogue.speaker.Name.Equals(npcName))
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