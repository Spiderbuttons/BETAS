using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerAnniversaryToday
{
    // Check whether a player has a wedding anniversary today.
    [GSQ("PLAYER_ANNIVERSARY_TODAY")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var playerKey, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, target => target.GetSpouseFriendship() != null && target.GetSpouseFriendship().WeddingDate.DayOfMonth == Game1.Date.DayOfMonth && target.GetSpouseFriendship().WeddingDate.Season == Game1.Date.Season && target.GetSpouseFriendship().WeddingDate.Year != Game1.Date.Year);
    }
}