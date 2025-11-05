using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class AddHealth
{
    // Add a given value to the health of the current player.
    [Action("AddHealth")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetInt(args, 1, out var health, out error, name: "int #Health") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 2, out var overrideBool, out error, defaultValue: false, name: "bool Override Max?"))
        {
            return false;
        }

        Game1.player.health += health;
        if (!overrideBool) Game1.player.health = Math.Min(Game1.player.health, Game1.player.maxHealth);
        return true;
    }
}