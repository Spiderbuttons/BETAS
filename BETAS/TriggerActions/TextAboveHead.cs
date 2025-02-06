using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class TextAboveHead
{
    // Show a speech bubble of text above the head of an NPC.
    [Action("TextAboveHead")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string npcName, out error, allowBlank: false) ||
            !ArgUtilityExtensions.TryGetTokenizable(args, 2, out string dialogue, out error, allowBlank: false) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 3, out int duration, out error, 3000) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 4, out int delay, out error, 0) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableBool(args, 5, out bool jitter, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(args, 6, out string colour, out error, defaultValue: null)
            )
        {
            error = "Usage: Spiderbuttons.BETAS_TextAboveHead <NPC Name> <Dialogue> [Duration] [Delay] [Jitter?] [Colour]";
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
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