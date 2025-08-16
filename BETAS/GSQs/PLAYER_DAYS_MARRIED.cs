using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerDaysMarried
{
    // GSQ for checking how long a Target Player has been married for, or 0 if they have no spouse in the first place.
    [GSQ("PLAYER_DAYS_MARRIED")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGetInt(query, 2, out var minDays, out error, name: "int Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 3, out var maxDays, out error, int.MaxValue, name: "int Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            var daysMarried = target.GetDaysMarried();
            return daysMarried >= minDays && daysMarried <= maxDays;
        });
    }
}