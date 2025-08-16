using System.Collections.Generic;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;
using Program = StardewModdingAPI.Program;

namespace BETAS.GSQs;

public static class LocationHasNpc
{
    // Check whether a given location has any of the given NPCs inside of it.
    [GSQ("LOCATION_HAS_NPC")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        GameLocation? location = context.Location;
        if (!TokenizableArgUtility.TryGetLocation(query, 1, ref location, out var error) ||
            !TokenizableArgUtility.TryGetOptional(query, 2, out _, out error, name: "string NPC"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (!ArgUtility.HasIndex(query, 2))
        {
            return location.CachedCharacters().Any();
        }
        
        return TokenizableArgUtility.AnyArgMatches(query, 2, (rawName) => location.CachedCharacters().Any(
            i => i.Name.Equals(rawName)));
    }
}