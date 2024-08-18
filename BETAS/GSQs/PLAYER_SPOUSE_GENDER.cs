using System;
using System.Linq;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Minigames;

namespace BETAS.GSQs;

public static class PlayerSpouseGender
{
    // Checks whether a player's spouse is Male or Female.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var playerKey, out var error) || !ArgUtility.TryGet(query, 2, out var genderName, out error))
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