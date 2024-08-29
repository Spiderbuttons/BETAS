using System.Collections.Generic;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class LocationHasNpc
{
    // Check whether a given location has any of the given NPCs inside of it.
    [GSQ("LOCATION_HAS_NPC")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var locationName, out var error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(query, 2, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var location = Game1.getLocationFromName(locationName);
        if (location == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no location found with name '" + locationName + "'");
        }
        
        if (location.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            BETAS.Cache is null || !BETAS.Cache.L1Cache.Any())
        {
            return query.Length == 2
                ? location.characters.Any()
                : GameStateQuery.Helpers.AnyArgMatches(query, 2,
                    (rawName) => string.Equals(location.Name, Game1.getCharacterFromName(rawName)?.currentLocation?.Name));
        }

        List<string> npcInLocationFromCache = [];
        npcInLocationFromCache.AddRange(BETAS.Cache.L1Cache.Values.Where(npc => npc.LocationName == location.Name).Select(npc => npc.NpcName.ToLower()));

        return query.Length == 2
            ? npcInLocationFromCache.Any()
            : GameStateQuery.Helpers.AnyArgMatches(query, 2,
                (rawName) => npcInLocationFromCache.Contains(rawName.ToLower()));
    }
}