using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class ItemInWorld
{
    // Checks if any given item ID exists somewhere in the world (in the inventory, chests, machines, etc)
    [GSQ("ITEM_IN_WORLD")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out _, out var error, name: "string Item ID"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var found = false;
        Utility.ForEachItem((item) =>
        {
            if (!TokenizableArgUtility.AnyArgMatches(query, 1,
                    (itemID) => item.QualifiedItemId.Equals(ItemRegistry.QualifyItemId(itemID)))) return true;
            found = true;
            return false;
        });
        
        return found;
    }
}