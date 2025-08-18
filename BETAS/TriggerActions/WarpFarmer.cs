using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class WarpFarmer
{
    // Warp the current player to a specific map and X/Y coordinate, with optional facing direction.
    [Action("WarpFarmer")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        GameLocation? location = Game1.player.currentLocation;
        if (!TokenizableArgUtility.TryGetLocation(args, 1, ref location, out error) ||
            !TokenizableArgUtility.TryGetInt(args, 2, out int x, out error, name: "int #X Coordinate") ||
            !TokenizableArgUtility.TryGetInt(args, 3, out int y, out error, name: "int #Y Coordinate") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 4, out int facingDirection, out error, 2, name: "int #Facing Direction"))
        {
            return false;
        }

        Game1.warpFarmer(Game1.getLocationRequest(location.NameOrUniqueName), x, y, facingDirection);
        return true;
    }
}