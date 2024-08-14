using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.Actions;

public static class EmoteNpc
{
    // Make an NPC perform an emote.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtility.TryGet(args, 1, out string npcName, out error, allowBlank: false) || !ArgUtility.TryGetInt(args, 2, out int emote, out error))
        {
            error = "Usage: EmoteNpc <NPC Name> <Emote>";
            return false;
        }
        
        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        npc.doEmote(emote, 24);
        return true;
    }
}