using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;
using StardewValley.TerrainFeatures;

namespace BETAS.Actions;

public static class ClearTerrainFeatures
{
    // Clear the terrain feature on a specific tile or all terrain features in a rectangle in a location according to class.
    [Action("ClearTerrainFeatures")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var location, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(args, 2, out var type, out error, defaultValue: "All") ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 3, out var topLeftX, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 4, out var topLeftY, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 5, out var bottomRightX, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 6, out var bottomRightY, out error))
        {
            error = "Usage: ClearTerrainFeatures <Location> [Type] [TopLeft] [TopLeft] [BottomRightX] [BottomRightY]";
            return false;
        }

        var loc = Game1.RequireLocation(location);
        
        if (!ArgUtility.HasIndex(args, 3))
        {
            loc.terrainFeatures.RemoveWhere(pair => pair.Value.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All"));
            loc.resourceClumps.RemoveWhere(clump => clump.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All"));
            loc.largeTerrainFeatures.RemoveWhere(feature => feature.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All"));
            return true;
        }

        if (!ArgUtility.HasIndex(args, 5))
        {
            loc.terrainFeatures.Remove(new Vector2(topLeftX, topLeftY));
            loc.resourceClumps.RemoveWhere(clump => clump.Tile == new Vector2(topLeftX, topLeftY));
            loc.largeTerrainFeatures.RemoveWhere(feature => feature.Tile == new Vector2(topLeftX, topLeftY));
            return true;
        }
        
        loc.terrainFeatures.RemoveWhere(pair => pair.Key.X >= topLeftX && pair.Key.Y >= topLeftY &&
                                               pair.Key.X <= bottomRightX && pair.Key.Y <= bottomRightY &&
                                               (pair.Value.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All")));
        loc.resourceClumps.RemoveWhere(clump => clump.Tile.X >= topLeftX && clump.Tile.Y >= topLeftY &&
                                                  clump.Tile.X <= bottomRightX && clump.Tile.Y <= bottomRightY &&
                                                  (clump.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All")));
        loc.largeTerrainFeatures.RemoveWhere(feature => feature.Tile.X >= topLeftX && feature.Tile.Y >= topLeftY &&
                                                       feature.Tile.X <= bottomRightX && feature.Tile.Y <= bottomRightY &&
                                                       (feature.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All")));
        
        return true;
    }
}