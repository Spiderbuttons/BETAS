using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerDaysUntilHouseUpgrade
{
    // Check whether the player has a house upgrade pending and/or how long until it's done.
    [GSQ("PLAYER_DAYS_UNTIL_HOUSE_UPGRADE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !TokenizableArgUtility.TryGetTokenizableInt(query, 2, out var minDays, out error) ||
            !TokenizableArgUtility.TryGetOptionalTokenizableInt(query, 3, out var maxDays, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey,
            target => target.daysUntilHouseUpgrade.Value >= minDays && target.daysUntilHouseUpgrade.Value <= maxDays);
    }
}