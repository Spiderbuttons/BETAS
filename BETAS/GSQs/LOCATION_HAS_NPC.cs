using System;
using System.Linq;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class LocationHasNpc
{
    // Check whether a given location has any of the given NPCs inside of it.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var locationName, out var error) || !ArgUtility.TryGetOptional(query, 2, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var location = Game1.getLocationFromName(locationName);
        if (location == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no location found with name '" + locationName + "'");
        }
        
        return query.Length == 2 ? location.characters.Any() : GameStateQuery.Helpers.AnyArgMatches(query, 2, (rawName) => string.Equals(location.Name, Game1.getCharacterFromName(rawName)?.currentLocation?.Name));
    }
}