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
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var npcName, out var error) ||
            !TokenizableArgUtility.TryGetTokenizableLocationName(query, 2, context.Location, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (npcName.Equals("Any"))
        {
            return TokenizableArgUtility.AnyArgMatches(query, 2, (rawName) => Game1.getLocationFromName(rawName)?.characters.Any());
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no NPC found with name '" + npcName + "'");
        }
        
        if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
        {
            return TokenizableArgUtility.AnyArgMatches(query, 2,
                (rawName) => string.Equals(rawName, npc.currentLocation?.Name, StringComparison.OrdinalIgnoreCase));
        }

        return TokenizableArgUtility.AnyArgMatches(query, 2,
            (rawName) => string.Equals(rawName, cache.LocationName, StringComparison.OrdinalIgnoreCase));
    }
}