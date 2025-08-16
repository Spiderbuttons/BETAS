using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerFestivalScore
{
    // Check whether the players current festival score is within a given range.
    [GSQ("PLAYER_FESTIVAL_SCORE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !TokenizableArgUtility.TryGetTokenizableInt(query, 2, out var min, out error) ||
            !TokenizableArgUtility.TryGetOptionalTokenizableInt(query, 3, out var max, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey,
            target => target.festivalScore >= min && target.festivalScore <= max);
    }
}