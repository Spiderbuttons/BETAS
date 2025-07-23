using System.Collections.Generic;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcFacingDirection
{
    // Check whether a given NPC is facing certain direction.
    [GSQ("NPC_FACING_DIRECTION")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var npcName, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out int direction, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (direction > 3 || direction < 0)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "direction set is not valid, 0 = up, 1 = right, 2 = down, 3 = left");
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no NPC found with name '" + npcName + "'");
            return false;
        }

        if ( npc.FacingDirection != direction )
        {
            return false;
        }
        return true;
    }
}