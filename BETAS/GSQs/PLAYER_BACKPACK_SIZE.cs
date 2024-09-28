using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerBackpackSize
{
    // Checks the number of backpack slots that a player has available to them
    [GSQ("PLAYER_BACKPACK_SIZE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out var min, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 3, out var max, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, target => target.MaxItems >= min && target.MaxItems <= max);
    }
}