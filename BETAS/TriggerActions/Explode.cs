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
        if (!TokenizableArgUtility.TryGetOptionalInt(args, 1, out int x, out error,
                defaultValue: Game1.player.TilePoint.X, name: "int #X Coordinate") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 2, out int y, out error,
                defaultValue: Game1.player.TilePoint.Y, name: "int #Y Coordinate") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 3, out int radius, out error, defaultValue: 3,
                name: "int #Radius") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 4, out bool damageFarmers, out error, defaultValue: false,
                name: "bool Damage Farmers?") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 5, out int damageAmount, out error, defaultValue: -1,
                name: "int #Damage Amount") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 6, out bool destroyObjects, out error, defaultValue: false,
                name: "bool Destroy Objects?"))
        {
            return false;
        }

        Game1.currentLocation.playSound("explosion");
        Game1.currentLocation.explode(new Vector2(x, y), radius, Game1.player, damageFarmers, damageAmount,
            destroyObjects);
        return true;
    }
}