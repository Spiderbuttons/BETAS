using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcNearLocation
{
    // Check whether a given NPC is currently within a specific radius of the player.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetPossiblyRelativeLocationName(query, 1, out var locationName, out var error) ||
            !ArgUtility.TryGetInt(query, 2, out var x, out error) ||
            !ArgUtility.TryGetInt(query, 3, out var y, out error) ||
            !ArgUtility.TryGetInt(query, 4, out var radius, out error) ||
            !ArgUtility.TryGetOptional(query, 5, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        var target = Game1.getLocationFromName(locationName);
        if (target == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no location found with name '" + locationName + "'");
        }

        var targetPosition = new Point(x * Game1.tileSize + Game1.tileSize / 2, y * Game1.tileSize + Game1.tileSize / 2);
        Rectangle rect = new Rectangle(targetPosition.X - radius * 64, targetPosition.Y - radius * 64, (radius * 2 + 1) * 64, (radius * 2 + 1) * 64);
        if (!ArgUtility.HasIndex(query, 5))
        {
            return target.characters.Any(i => rect.Contains(Utility.Vector2ToPoint(i.Position)));
        }

        return GameStateQuery.Helpers.AnyArgMatches(query, 5, (rawName) =>
        {
            return target.characters.Any(i =>
                i.Name.Equals(rawName) && rect.Contains(Utility.Vector2ToPoint(i.Position)));
        });
    }
}