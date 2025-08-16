using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class ItemEnchantments
{
    // Check whether an item has enchantments on it. Only useful for tools.
    [GSQ("ITEM_ENCHANTMENTS")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item, out var error) || !TokenizableArgUtility.TryGetOptionalTokenizable(query, 2, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (item is not Tool tool) return false;

        var enchants = tool.enchantments;
        
        if (query.Length == 2)
        {
            return enchants.Any();
        }
        
        return TokenizableArgUtility.AnyArgMatches(query, 2, (enchant) =>
        {
            return enchants.Any(e =>
            {
                var name = e.GetName();
                return name.Equals(enchant, StringComparison.OrdinalIgnoreCase);
            });
        });
    }
}