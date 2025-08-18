using System.Collections.Generic;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcNearArea
{
    // Check whether a given NPC is currently within a specific radius of an area.
    [GSQ("NPC_NEAR_AREA")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        GameLocation? location = context.Location;
        if (!TokenizableArgUtility.TryGetLocation(query, 1, ref location, out var error) ||
            !TokenizableArgUtility.TryGetInt(query, 2, out int x, out error, name: "int #X Coordinate") ||
            !TokenizableArgUtility.TryGetInt(query, 3, out int y, out error, name: "int #Y Coordinate") ||
            !TokenizableArgUtility.TryGetInt(query, 4, out int radius, out error, name: "int #Radius") ||
            !TokenizableArgUtility.TryGetOptional(query, 5, out _, out error, name: "string NPC"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var targetRect = new Rectangle(x, y, 1, 1);
        targetRect.Inflate(radius, radius);
        
        if (!ArgUtility.HasIndex(query, 5))
        {
            return location.CachedCharacters().Any(i => targetRect.Contains(i.TilePoint));
        }
        
        return TokenizableArgUtility.AnyArgMatches(query, 5, (rawName) =>
        {
            return location.CachedCharacters().Any(i => i.Name.Equals(rawName) && targetRect.Contains(i.TilePoint));
        });
    }
}