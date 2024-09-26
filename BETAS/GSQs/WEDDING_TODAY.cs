using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class WeddingToday
{
    // Check whether or not there is a wedding scheduled today and optionally check which player is getting married.
    [GSQ("WEDDING_TODAY")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizable(query, 1, out var playerKey, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (!ArgUtility.HasIndex(query, 1)) return Game1.weddingToday;
        
        return Game1.weddingToday && GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, (Farmer target) => target?.getSpouse() != null && target.GetSpouseFriendship().WeddingDate.Equals(Game1.Date));
    }
}