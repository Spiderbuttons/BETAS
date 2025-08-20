using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class RegeneratePathsLayer
{
    // Regenerate the features from the Paths layer in a location in the specified area
    [Action("RegeneratePathsLayer")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        GameLocation? location = Game1.player.currentLocation;
        if (!TokenizableArgUtility.TryGetOptionalLocation(args, 1, ref location, out error) ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 2, out int x, out error, defaultValue: 0, name: "int #Top Left X Coordinate") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 3, out int y, out error, defaultValue: 0, name: "int #Top Left Y Coordinate") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 4, out int width, out error, defaultValue: -1, name: "int #Width") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 5, out int height, out error, defaultValue: -1, name: "int #Height"))
        {
            return false;
        }

        if (width is -1) width = location.Map.DisplayWidth / Game1.tileSize - 1 - x;
        if (height is -1) height = location.Map.DisplayHeight / Game1.tileSize - 1 - y;

        location.loadPathsLayerObjectsInArea(x, y, width, height);
        return true;
    }
}