using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class AddStamina
{
    // Add a given value to the health of the current player.
    [Action("AddStamina")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetInt(args, 1, out var stamina, out error, name: "int #Stamina") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 2, out var overrideBool, out error, defaultValue: false, name: "bool Override Max?"))
        {
            return false;
        }

        Game1.player.stamina += stamina;
        if (!overrideBool) Game1.player.stamina = Math.Min(Game1.player.stamina, Game1.player.MaxStamina);
        return true;
    }
}