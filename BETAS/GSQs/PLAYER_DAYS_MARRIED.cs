﻿using System;
using System.Linq;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerDaysMarried
{
    // GSQ for checking how long a Target Player has been married for, or 0 if they have no spouse in the first place.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var playerKey, out var error) || !ArgUtility.TryGetInt(query, 2, out var minDays, out error) || !ArgUtility.TryGetOptionalInt(query, 3, out var maxDays, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        Log.Debug($"PlayerDaysMarried.Query: playerKey={playerKey}, minDays={minDays}, maxDays={maxDays}");
        
        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            var daysMarried = target.GetDaysMarried();
            return daysMarried >= minDays && daysMarried <= maxDays;
        });
    }
}