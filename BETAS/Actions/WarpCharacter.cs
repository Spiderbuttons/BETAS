using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.GSQs;

public static class WarpCharacter
{
    // Warp an NPC to a specific map and X/Y coordinate, with optional facing direction.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtility.TryGet(args, 1, out string npcName, out error, allowBlank: false) || !ArgUtility.TryGet(args, 2, out string locationName, out error, allowBlank: false) || !ArgUtility.TryGetInt(args, 3, out int x, out error) || !ArgUtility.TryGetInt(args, 4, out int y, out error) || !ArgUtility.TryGetOptionalInt(args, 5, out int facingDirection, out error, 2))
        {
            error = "Usage: WarpCharacter <NPC Name> <Location Name> <X> <Y> [Facing Direction]";
            return false;
        }
        
        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        var location = Game1.RequireLocation(locationName);
        if (location == null)
        {
            error = "no location found with name '" + locationName + "'";
            return false;
        }

        Game1.warpCharacter(npc, location, new Vector2(x, y));
        npc.faceDirection(facingDirection);
        npc.controller = null;
        npc.Halt();
        return true;
    }
}