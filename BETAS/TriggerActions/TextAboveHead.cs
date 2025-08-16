using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class TextAboveHead
{
    // Show a speech bubble of text above the head of an NPC.
    [Action("TextAboveHead")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? npcName, out error, allowBlank: false) ||
            !TokenizableArgUtility.TryGet(args, 2, out string? dialogue, out error, allowBlank: false) ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 3, out int duration, out error, 3000) ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 4, out int delay, out error) ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 5, out bool jitter, out error) ||
            !TokenizableArgUtility.TryGetOptional(args, 6, out string? colour, out error, defaultValue: null)
            )
        {
            error = "Usage: Spiderbuttons.BETAS_TextAboveHead <NPC Name> <Dialogue> [Duration] [Delay] [Jitter?] [Colour]";
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName)?.EventActorIfPossible();
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        // try
        // {
        //     dialogue = Game1.content.LoadString(dialogue);
        // }
        // catch
        // {
        //     Log.Trace("BETAS.TriggerActions.TextAboveHead: Argument is not a translation key.");
        // }

        npc.showTextAboveHead(dialogue, Utility.StringToColor(colour), jitter ? 0 : 2, duration, delay);
        return true;
    }
}