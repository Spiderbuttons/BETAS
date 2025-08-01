﻿using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.GSQs;

public static class GrandpaScore
{
    // Check whether the grandpa evaluation score is within a given range.
    [GSQ("GRANDPA_SCORE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var scoreType, out var error) || !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out var min, out error) || !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 3, out var max, out error, defaultValue: int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (!scoreType.EqualsIgnoreCase("Score") && !scoreType.EqualsIgnoreCase("Candles"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, "invalid score type '" + scoreType + "'");
        }

        int rawScore = Utility.getGrandpaScore();
        int candleScore = Utility.getGrandpaCandlesFromScore(rawScore);
        
        if (scoreType.EqualsIgnoreCase("Score"))
        {
            return rawScore >= min && (max == 0 || rawScore <= max);
        }
        
        return candleScore >= min && (max == 0 || candleScore <= max);
    }
}