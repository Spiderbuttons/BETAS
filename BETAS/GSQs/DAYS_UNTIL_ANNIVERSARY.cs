using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class DaysUntilAnniversary
{
    // Check whether or not the days remaining until the player's anniversary is between min and max.
    [GSQ("DAYS_UNTIL_ANNIVERSARY")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGetInt(query, 2, out var minDays, out error, name: "int #Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 3, out var maxDays, out error, int.MaxValue, name: "int #Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            Friendship? spouseFriendship = target.GetSpouseFriendship();
            if (spouseFriendship is null) return false;
            
            WorldDate wedding = new WorldDate(spouseFriendship.WeddingDate);
            WorldDate today = WorldDate.Now();
            wedding.Year = today.Year;
            if (today > wedding) wedding.Year += 1;
            int daysUntilAnniversary = wedding.TotalDays - today.TotalDays;
            
            return daysUntilAnniversary >= minDays && daysUntilAnniversary <= maxDays;
        });
    }
}