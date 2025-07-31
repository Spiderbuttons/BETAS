using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class HoldUpItem
{
    // Give the player an item and make them hold it up.
    [Action("HoldUpItem")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var itemId, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 2, out var count, out error, defaultValue: 1))
        {
            error = "Usage: Spiderbuttons.BETAS_HoldUpItem <ItemId> [Count]";
            return false;
        }

        if (Game1.eventUp && Game1.CurrentEvent.isFestival)
        {
            Game1.player.holdUpItemThenMessage(ItemRegistry.Create(itemId), count);
        } else Game1.PerformActionWhenPlayerFree(() => Game1.player.holdUpItemThenMessage(ItemRegistry.Create(itemId), count));
        
        return true;
    }
}