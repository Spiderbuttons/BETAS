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
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGetInt(query, 2, out var min, out error, name: "int #Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 3, out var max, out error, int.MaxValue, name: "int #Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey,
            target => target.festivalScore >= min && target.festivalScore <= max);
    }
}