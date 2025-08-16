using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerWeddingToday
{
    // Check whether there is a wedding scheduled today and optionally check which player is getting married.
    [GSQ("PLAYER_WEDDING_TODAY")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return Game1.weddingToday && GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, target => target.GetSpouseFriendship() is not null && target.GetSpouseFriendship().WeddingDate.Equals(Game1.Date));
    }
}