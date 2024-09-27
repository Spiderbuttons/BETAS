using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class AnniversaryToday
{
    // Check whether a player has a wedding anniversary today.
    [GSQ("ANNIVERSARY_TODAY")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var playerKey, out var error) || !ArgUtilityExtensions.TryGetOptionalTokenizable(query, 2, out var type, out error, defaultValue: "year") || !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 3, out var interval, out error, defaultValue: 1))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, (Farmer target) => target.GetSpouseFriendship() != null && target.GetSpouseFriendship().WeddingDate.DayOfMonth == Game1.Date.DayOfMonth && target.GetSpouseFriendship().WeddingDate.Season == Game1.Date.Season);
    }
}