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
        if (!TokenizableArgUtility.TryGetOptionalInt(args, 1, out int amount, out error, defaultValue: 1, name: "int #Amount"))
        {
            return false;
        }

        Game1.player.clubCoins += amount;
        return true;
    }
}