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
        if (!TokenizableArgUtility.TryGet(args, 1, out var itemId, out error, name: "string Item ID") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 2, out var count, out error, defaultValue: 1, name: "int #Count") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 3, out var giveItem, out error, defaultValue: false, name: "bool Give Item?"))
        {
            return false;
        }

        Item item = ItemRegistry.Create(itemId);
        item.Stack = count;
        item.FixStackSize();
        
        // Player isn't considered "free" if they're in a festival.
        if (Game1.eventUp && Game1.CurrentEvent.isFestival)
        {
            Game1.player.holdUpItemThenMessage(item, item.Stack);
        }
        else Game1.PerformActionWhenPlayerFree(() =>
        {
            Game1.player.holdUpItemThenMessage(item, item.Stack);
        });
        
        if (giveItem)
        {
            Game1.PerformActionWhenPlayerFree(() =>
            { 
                Game1.player.addItemByMenuIfNecessary(item);
            });
        }
        return true;
    }
}