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
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizable(query, 2, out var genderName, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var genderEnum = genderName switch
        {
            "Male" => Gender.Male,
            "Female" => Gender.Female,
            _ => Gender.Undefined
        };

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, (Farmer target) =>
        {
            var spouse = target.getSpouse();
            if (spouse == null)
                return false;
            return spouse.Gender == genderEnum;
        });
    }
}