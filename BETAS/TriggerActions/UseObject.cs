using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class UseObject
{
    // Make the current farmer use an object with the given ID, optionally not requiring it to be in the inventory.
    [Action("UseObject")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? itemId, out error, name: "string Item ID") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 2, out var fromInventory, out error, defaultValue: true, name: "bool From Inventory?"))
        {
            return false;
        }

        var item = ItemRegistry.Create<Object>(ItemRegistry.QualifyItemId(itemId));
        if (fromInventory && Game1.player.Items.CountId(itemId) == 0)
        {
            return false;
        }

        if (item.performUseAction(Game1.player.currentLocation) && fromInventory)
        {
            Game1.player.Items.ReduceId(itemId, 1);
        }
        return true;
    }
}