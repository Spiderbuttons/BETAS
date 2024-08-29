using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.Actions;

public static class Lightning
{
    // Make lightning strike at the specified coordinates, or on the current farmer if no coordinates are specified.
    [Action("Lightning")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 1, out int x, out error,
                defaultValue: Game1.player.TilePoint.X) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 2, out int y, out error,
                defaultValue: Game1.player.TilePoint.Y))
        {
            error = "Usage: Lightning <X> <Y>";
            return false;
        }

        Game1.flashAlpha = (float)(1.5);
        Utility.drawLightningBolt(
            new Vector2(x * Game1.tileSize + Game1.tileSize / 2, y * Game1.tileSize + Game1.tileSize / 2),
            Game1.player.currentLocation);
        Game1.playSound("thunder");
        return true;
    }
}