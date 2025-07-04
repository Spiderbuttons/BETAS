using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley.Delegates;
using StardewValley.Triggers;

namespace BETAS.TriggerActions;

public static class ActionList
{
    // Run every action in an action list, one after the other.
    [Action("ActionList")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var _, out error, allowBlank: false))
        {
            error = "Usage: Spiderbuttons.BETAS_ActionList <Action String>+";
            return false;
        }

        for (var i = 1; i < args.Length; i++)
        {
            if (!TriggerActionManager.TryRunAction(args[i], out error, out _))
            {
                return false;
            }
        }
        
        return true;
    }
}