using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class AddQiCoins
{
    // Add an amount of Qi Coins to the farmer's casino wallet.
    [Action("AddQiCoins")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 1, out int amount, out error, defaultValue: 1))
        {
            error = "Usage: Spiderbuttons.BETAS_AddQiCoins [Amount]";
            return false;
        }

        Game1.player.clubCoins += amount;
        return true;
    }
}