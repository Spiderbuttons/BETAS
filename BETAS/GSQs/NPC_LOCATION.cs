using System;
using System.Linq;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcLocation
{
    // Check whether a given NPC is currently in a specific map.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var npcName, out var error) ||
            !ArgUtility.TryGet(query, 2, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (npcName.Equals("Any"))
        {
            return GameStateQuery.Helpers.AnyArgMatches(query, 2,
                (rawName) => Game1.getLocationFromName(rawName)?.characters.Any());
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no NPC found with name '" + npcName + "'");
        }

        return GameStateQuery.Helpers.AnyArgMatches(query, 2,
            (rawName) => string.Equals(rawName, npc.currentLocation?.Name, StringComparison.OrdinalIgnoreCase));
    }
}