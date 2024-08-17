using System;
using System.Linq;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerQiGems
{
    // Checks how many Qi gems a player has.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var playerKey, out var error) || !ArgUtility.TryGetFloat(query, 2, out var minGems, out error) || !ArgUtility.TryGetOptionalFloat(query, 3, out var maxGems, out error, float.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            var gems = target.QiGems;
            return gems >= minGems && gems <= maxGems;
        });
    }
}