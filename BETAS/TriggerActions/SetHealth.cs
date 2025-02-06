using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class SetHealth
{
    // Set the health of the current player to a given value.
    [Action("SetHealth")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizableInt(args, 1, out var health, out error, allowBlank: false) || !ArgUtilityExtensions.TryGetOptionalTokenizableBool(args, 2, out var overrideBool, out error, defaultValue: false))
        {
            error = "Usage: Spiderbuttons.BETAS_SetHealth <Value> [Override Max]";
            return false;
        }
        
        Game1.player.health = overrideBool ? health : Math.Min(health, Game1.player.maxHealth);
        return true;
    }
}