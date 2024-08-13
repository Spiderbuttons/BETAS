using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.Actions;

public static class WarpFarmer
{
    // Warp the current player to a specific map and X/Y coordinate, with optional facing direction.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtility.TryGet(args, 1, out string locationName, out error, allowBlank: false) || !ArgUtility.TryGetInt(args, 2, out int x, out error) || !ArgUtility.TryGetInt(args, 3, out int y, out error) || !ArgUtility.TryGetOptionalInt(args, 4, out int facingDirection, out error, 2))
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