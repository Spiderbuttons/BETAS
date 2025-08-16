using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class AddFestivalScore
{
    // Add an amount of points to the current farmer's festival score.
    [Action("AddFestivalScore")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetOptionalTokenizableInt(args, 1, out int amount, out error, defaultValue: 1))
        {
            error = "Usage: Spiderbuttons.BETAS_AddFestivalScore [Amount]";
            return false;
        }
        
        Log.Warn(amount);

        Game1.player.festivalScore += amount;
        return true;
    }
}