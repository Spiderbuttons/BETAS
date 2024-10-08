﻿using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerStardropsFound
{
    // Checks how many stardrops a player has found. May or may not catch stardrops added through mods if the mod does not patch numStardropsFound.
    [GSQ("PLAYER_STARDROPS_FOUND")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out var minDrops, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 3, out var maxDrops, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            var num = Utility.numStardropsFound(target);
            return num >= minDrops && num <= maxDrops;
        });
    }
}