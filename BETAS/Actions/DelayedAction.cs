﻿using System;
using System.Text;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Triggers;

namespace BETAS.Actions;

public static class DelayedAction
{
    // Run a trigger action action after a delay.
    [Action("DelayedAction")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var actionString, out error, allowBlank: false) || !ArgUtilityExtensions.TryGetTokenizableInt(args, 2, out var delay, out error, allowBlank: false))
        {
            error = "Usage: DelayedAction <Action String> <Delay>";
            return false;
        }
        
        StardewValley.DelayedAction.functionAfterDelay(() =>
        {
            if (!TriggerActionManager.TryRunAction(actionString, out var error, out Exception ex))
            {
                Log.Error($"Error running action '{actionString}': {error}");
            }
        }, delay);
        
        return true;
    }
}