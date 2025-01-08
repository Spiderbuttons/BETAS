using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.Actions;

public static class EatObject
{
    // Make the current farmer eat an object with the given ID, optionally not requiring it to be in the inventory.
    [Action("EatObject")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string itemId, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableBool(args, 2, out var fromInventory, out error, defaultValue: true))
        {
            error = "Usage: EatObject <ItemId> [From Inventory?]";
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