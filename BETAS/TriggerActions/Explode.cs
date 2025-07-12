using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class Explode
{
    // Make an explosion happen in the current location at the specified coordinates.
    [Action("Explode")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 1, out int x, out error,
                defaultValue: Game1.player.TilePoint.X) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 2, out int y, out error,
                defaultValue: Game1.player.TilePoint.Y) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 3, out int radius, out error,
                defaultValue: 3) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableBool(args, 4, out bool damageFarmers, out error,
                defaultValue: false) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 5, out int damageAmount, out error,
                defaultValue: -1) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableBool(args, 6, out bool destroyObjects, out error,
                defaultValue: false))
        {
            error = "Usage: Spiderbuttons.BETAS_Explode [Tile X] [Tile Y] [Radius] [Damage Farmers?] [Damage Amount] [Destroy Objects?]";
            return false;
        }

        Game1.currentLocation.playSound("explosion");
        Game1.currentLocation.explode(new Vector2(x, y), radius, Game1.player, damageFarmers, damageAmount, destroyObjects);
        return true;
    }
}