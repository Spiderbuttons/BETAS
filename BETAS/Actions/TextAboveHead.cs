using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.Actions;

public static class TextAboveHead
{
    // Show a speech bubble of text above the head of an NPC.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtility.TryGet(args, 1, out string npcName, out error, allowBlank: false) || !ArgUtility.TryGet(args, 2, out string dialogue, out error, allowBlank: false) || !ArgUtility.TryGetOptionalInt(args, 3, out int duration, out error, 3000) || !ArgUtility.TryGetOptionalInt(args, 4, out int delay, out error, 0) || !ArgUtility.TryGetOptional(args, 5, out string colour, out error))
        {
            error = "Usage: TextAboveHead <NPC Name> <Dialogue> [Duration] [Delay] [Colour]";
            return false;
        }

        if (!ArgUtility.HasIndex(args, 5)) colour = null;
        
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
        catch
        {
            Log.Trace("BETAS.Actions.TextAboveHead: Argument is not a translation key.");
        }
        
        npc.showTextAboveHead(dialogue, Utility.StringToColor(colour), 2, duration, delay);
        return true;
    }
}