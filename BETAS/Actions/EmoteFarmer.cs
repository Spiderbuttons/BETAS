using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.Actions;

public static class EmoteFarmer
{
    // Make the current farmer perform an emote.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtility.TryGetInt(args, 1, out int emote, out error))
        {
            error = "Usage: EmoteFarmer <EmoteId>";
            return false;
        }
        
        Game1.player.doEmote(emote, 24);
        return true;
    }
}