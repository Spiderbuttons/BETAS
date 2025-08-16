using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerSpouseGender
{
    // Checks whether a player's spouse is Male or Female or Undefined.
    [GSQ("PLAYER_SPOUSE_GENDER")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGetEnum(query, 2, out Gender gender, out error, name: "Gender Spouse Gender"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, target =>
        {
            var spouse = target.getSpouse();
            if (spouse == null)
                return false;
            return spouse.Gender == gender;
        });
    }
}