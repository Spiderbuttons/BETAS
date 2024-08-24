using System;
using System.Linq;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcLocation
{
    // Check whether a given NPC is currently in a specific map.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var npcName, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizable(query, 2, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (npcName.Equals("Any"))
        {
            if (Context.IsMainPlayer || BETAS.Cache is null)
            {
                return GameStateQuery.Helpers.AnyArgMatches(query, 2,
                    (rawName) => Game1.getLocationFromName(rawName)?.characters.Any());
            }
            
            return GameStateQuery.Helpers.AnyArgMatches(query, 2,
                (rawName) =>
                {
                    return BETAS.Cache.L1Cache.Values.Any(npc => string.Equals(npc.LocationName, rawName, StringComparison.OrdinalIgnoreCase));
                });
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no NPC found with name '" + npcName + "'");
        }
        
        if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
        {
            return GameStateQuery.Helpers.AnyArgMatches(query, 2,
                (rawName) => string.Equals(rawName, npc.currentLocation?.Name, StringComparison.OrdinalIgnoreCase));
        }

        return GameStateQuery.Helpers.AnyArgMatches(query, 2,
            (rawName) => string.Equals(rawName, cache.LocationName, StringComparison.OrdinalIgnoreCase));
    }
}