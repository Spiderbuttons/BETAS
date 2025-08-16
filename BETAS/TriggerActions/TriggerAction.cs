using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley.Delegates;
using StardewValley.Extensions;
using StardewValley.Triggers;

namespace BETAS.TriggerActions;

public static class TriggerAction
{
    // Manually run a specific trigger action.
    [Action("TriggerAction")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out var triggerActionId, out error, allowBlank: false) ||
            !TokenizableArgUtility.TryGetOptional(args, 2, out var trigger, out error, defaultValue: "Manual"))
        {
            error = "Usage: Spiderbuttons.BETAS_TriggerAction <Id> [Trigger]";
            return false;
        }

        foreach (CachedTriggerAction item in TriggerActionManager.GetActionsForTrigger(trigger))
        {
            if (!item.Data.Id.EqualsIgnoreCase(triggerActionId)) continue;
            
            if (TriggerActionManager.TryRunActions(item, trigger, context.TriggerArgs))
            {
                error = null;
                return true;
            }
            error = $"Failed to run trigger action '{triggerActionId}' with trigger '{trigger}'";
            return false;
        }
        
        error = $"Could not find trigger action with ID '{triggerActionId}' and trigger '{trigger}'";
        return false;
    }
}