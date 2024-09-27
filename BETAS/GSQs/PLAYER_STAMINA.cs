using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerStamina
{
    // Check whether the players current stamina is within a given range.
    [GSQ("PLAYER_STAMINA")]
    public static bool Current(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out var min, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 3, out var max, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey,
            target => target.Stamina >= min && target.Stamina <= max);
    }
    
    // Check whether the players maximum stamina is within a given range.
    [GSQ("PLAYER_MAX_STAMINA")]
    public static bool Max(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out var min, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 3, out var max, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey,
            target => target.MaxStamina >= min && target.MaxStamina <= max);
    }
}