using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class SetStamina
{
    // Set the stamina of the current player to a given value.
    [Action("SetStamina")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetInt(args, 1, out var stamina, out error, name: "int #Stamina") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 2, out var overrideBool, out error, defaultValue: false, name: "bool Override Max?"))
        {
            return false;
        }

        Game1.player.stamina = overrideBool ? stamina : Math.Min(stamina, Game1.player.MaxStamina);
        return true;
    }
}