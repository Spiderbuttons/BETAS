using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class EatObject
{
    // Make the current farmer eat an object with the given ID, optionally not requiring it to be in the inventory.
    [Action("EatObject")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? itemId, out error) ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 2, out var fromInventory, out error, defaultValue: true))
        {
            error = "Usage: Spiderbuttons.BETAS_EatObject <ItemId> [From Inventory?]";
            return false;
        }

        var item = ItemRegistry.Create<Object>(ItemRegistry.QualifyItemId(itemId));

        if ((fromInventory && Game1.player.Items.CountId(itemId) == 0) || item.Edibility == -300)
        {
            return false;
        }

        Game1.player.eatObject(item);
        if (fromInventory)
        {
            Game1.player.Items.ReduceId(itemId, 1);
        }
        return true;
    }
}