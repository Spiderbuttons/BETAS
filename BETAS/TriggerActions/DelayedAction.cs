using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley.Delegates;
using StardewValley.Triggers;

namespace BETAS.TriggerActions;

public static class DelayedAction
{
    // Run a trigger action action after a delay.
    [Action("DelayedAction")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out var actionString, out error, allowBlank: false) || !TokenizableArgUtility.TryGetInt(args, 2, out var delay, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_DelayedAction <Action String> <Delay>";
            return false;
        }
        
        StardewValley.DelayedAction.functionAfterDelay(() =>
        {
            if (!TriggerActionManager.TryRunAction(actionString, out var error, out _))
            {
                Log.Error($"Error running action '{actionString}': {error}");
            }
        }, delay);
        
        return true;
    }
}