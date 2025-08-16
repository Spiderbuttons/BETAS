using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcLocation
{
    // Check whether a given NPC is currently in a specific map.
    [GSQ("NPC_LOCATION")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out string? npcName, out var error, name: "NPC Name") ||
            !TokenizableArgUtility.TryGetLocationName(query, 2, context.Location, out _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (npcName.Equals("Any"))
        {
            return TokenizableArgUtility.AnyArgMatches(query, 2, (rawName) => Game1.getLocationFromName(rawName)?.CachedCharacters().Any());
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no NPC found with name '" + npcName + "'");
        }

        return TokenizableArgUtility.AnyArgMatches(query, 2,
            (rawName) => string.Equals(rawName, npc.CachedLocation().Name, StringComparison.OrdinalIgnoreCase));
    }
}