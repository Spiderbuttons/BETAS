using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.Actions;

public static class WarpFarmer
{
    // Warp the current player to a specific map and X/Y coordinate, with optional facing direction.
    [Action("WarpFarmer")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string locationName, out error,
                allowBlank: false) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(args, 2, out int x, out error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(args, 3, out int y, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 4, out int facingDirection, out error, 2))
        {
            error = "Usage: WarpFarmer <Location Name> <X> <Y> [Facing Direction]";
            return false;
        }
        
        

        var location = Game1.RequireLocation(locationName);
        if (location == null)
        {
            error = "no location found with name '" + locationName + "'";
            return false;
        }

        Game1.warpFarmer(Game1.getLocationRequest(locationName), x, y, facingDirection);
        return true;
    }
}