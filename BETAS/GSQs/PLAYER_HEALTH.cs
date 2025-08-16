using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerHealth
{
    // Check whether the players current health is within a given range.
    [GSQ("PLAYER_HEALTH")]
    public static bool Current(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error) ||
            !TokenizableArgUtility.TryGetInt(query, 2, out var min, out error) ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 3, out var max, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey,
            target => target.health >= min && target.health <= max);
    }
    
    // Check whether the players maximum health is within a given range.
    [GSQ("PLAYER_MAX_HEALTH")]
    public static bool Max(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error) ||
            !TokenizableArgUtility.TryGetInt(query, 2, out var min, out error) ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 3, out var max, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey,
            target => target.maxHealth >= min && target.maxHealth <= max);
    }
}