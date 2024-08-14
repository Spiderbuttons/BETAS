using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.Actions;

public static class Lightning
{
    // Make lightning strike at the specified coordinates, or on the current farmer if no coordinates are specified.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalPossiblyOffsetCoordinate(args, 1, out int x, out error, defaultValue: Game1.player.TilePoint.X) ||
            !ArgUtilityExtensions.TryGetOptionalPossiblyOffsetCoordinate(args, 2, out int y, out error, defaultValue: Game1.player.TilePoint.Y))
        {
            error = "Usage: Lightning <X> <Y>";
            return false;
        }
        
        Log.Debug($"Lightning at {x}, {y}");

        Game1.flashAlpha = (float)(1.5);
        Utility.drawLightningBolt(new Vector2(x * Game1.tileSize + Game1.tileSize / 2, y * Game1.tileSize + Game1.tileSize / 2), Game1.player.currentLocation);
        Game1.playSound("thunder");
        return true;
    }
}