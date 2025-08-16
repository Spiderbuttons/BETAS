using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.TriggerActions;

public static class ClearTerrainFeatures
{
    // Clear the terrain feature on a specific tile or all terrain features in a rectangle in a location according to class.
    [Action("ClearTerrainFeatures")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetOptional(args, 1, out var location, out error, defaultValue: "Here") ||
            !TokenizableArgUtility.TryGetOptional(args, 2, out var type, out error, defaultValue: "All") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 3, out var topLeftX, out error) ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 4, out var topLeftY, out error) ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 5, out var bottomRightX, out error) ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 6, out var bottomRightY, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_ClearTerrainFeatures [Location] [Type] [TopLeftX] [TopLeftY] [BottomRightX] [BottomRightY]";
            return false;
        }

        var loc = location.EqualsIgnoreCase("Here") ? Game1.player.currentLocation : Game1.RequireLocation(location);

        if (!ArgUtility.HasIndex(args, 1) || !ArgUtility.HasIndex(args, 2))
        {
            loc.terrainFeatures.Clear();
            loc.resourceClumps.Clear();
            loc.largeTerrainFeatures.Clear();
        }
        
        if (type.EqualsIgnoreCase("!All")) return false;
        var negate = type is not null && type.StartsWith("!");

        if (!ArgUtility.HasIndex(args, 3))
        {
            if (!negate)
            {
                loc.terrainFeatures.RemoveWhere(pair =>
                    pair.Value.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All"));
                loc.resourceClumps.RemoveWhere(clump =>
                    clump.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All"));
                loc.largeTerrainFeatures.RemoveWhere(feature =>
                    feature.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All"));
                return true;
            }
            type = type?.Substring(1);
            loc.terrainFeatures.RemoveWhere(pair => !pair.Value.GetType().Name.EqualsIgnoreCase(type));
            loc.resourceClumps.RemoveWhere(clump => !clump.GetType().Name.EqualsIgnoreCase(type));
            loc.largeTerrainFeatures.RemoveWhere(feature => !feature.GetType().Name.EqualsIgnoreCase(type));
            return true;
        }

        if (!ArgUtility.HasIndex(args, 5))
        {
            if (!negate)
            {
                loc.terrainFeatures.RemoveWhere(pair => pair.Key == new Vector2(topLeftX, topLeftY) &&
                                                        (pair.Value.GetType().Name.EqualsIgnoreCase(type) ||
                                                         type.EqualsIgnoreCase("All")));
                loc.resourceClumps.RemoveWhere(clump => clump.Tile == new Vector2(topLeftX, topLeftY) &&
                                                        (clump.GetType().Name.EqualsIgnoreCase(type) ||
                                                         type.EqualsIgnoreCase("All")));
                loc.largeTerrainFeatures.RemoveWhere(feature => feature.Tile == new Vector2(topLeftX, topLeftY) &&
                                                            (feature.GetType().Name.EqualsIgnoreCase(type) ||
                                                             type.EqualsIgnoreCase("All")));
                return true;
            }
            type = type?.Substring(1);
            loc.terrainFeatures.RemoveWhere(pair => pair.Key == new Vector2(topLeftX, topLeftY) &&
                                                    !pair.Value.GetType().Name.EqualsIgnoreCase(type));
            loc.resourceClumps.RemoveWhere(clump => clump.Tile == new Vector2(topLeftX, topLeftY) &&
                                                    !clump.GetType().Name.EqualsIgnoreCase(type));
            loc.largeTerrainFeatures.RemoveWhere(feature => feature.Tile == new Vector2(topLeftX, topLeftY) &&
                                                        !feature.GetType().Name.EqualsIgnoreCase(type));
            return true;
        }

        if (!negate)
        {
            loc.terrainFeatures.RemoveWhere(pair => pair.Key.X >= topLeftX && pair.Key.Y >= topLeftY &&
                                                    pair.Key.X <= bottomRightX && pair.Key.Y <= bottomRightY &&
                                                    (pair.Value.GetType().Name.EqualsIgnoreCase(type) ||
                                                     type.EqualsIgnoreCase("All")));
            loc.resourceClumps.RemoveWhere(clump => clump.Tile.X >= topLeftX && clump.Tile.Y >= topLeftY &&
                                                    clump.Tile.X <= bottomRightX && clump.Tile.Y <= bottomRightY &&
                                                    (clump.GetType().Name.EqualsIgnoreCase(type) ||
                                                     type.EqualsIgnoreCase("All")));
            loc.largeTerrainFeatures.RemoveWhere(feature => feature.Tile.X >= topLeftX && feature.Tile.Y >= topLeftY &&
                                                            feature.Tile.X <= bottomRightX &&
                                                            feature.Tile.Y <= bottomRightY &&
                                                            (feature.GetType().Name.EqualsIgnoreCase(type) ||
                                                             type.EqualsIgnoreCase("All")));
            return true;
        }
        
        type = type?.Substring(1);
        loc.terrainFeatures.RemoveWhere(pair => pair.Key.X >= topLeftX && pair.Key.Y >= topLeftY &&
                                                pair.Key.X <= bottomRightX && pair.Key.Y <= bottomRightY &&
                                                !pair.Value.GetType().Name.EqualsIgnoreCase(type));
        loc.resourceClumps.RemoveWhere(clump => clump.Tile.X >= topLeftX && clump.Tile.Y >= topLeftY &&
                                                clump.Tile.X <= bottomRightX && clump.Tile.Y <= bottomRightY &&
                                                !clump.GetType().Name.EqualsIgnoreCase(type));
        loc.largeTerrainFeatures.RemoveWhere(feature => feature.Tile.X >= topLeftX && feature.Tile.Y >= topLeftY &&
                                                        feature.Tile.X <= bottomRightX && feature.Tile.Y <= bottomRightY &&
                                                        !feature.GetType().Name.EqualsIgnoreCase(type));
        
        return true;
    }
}