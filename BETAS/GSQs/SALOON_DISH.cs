using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class SaloonDish
{
    // Check whether the saloons dish of the day matches any given item ID.
    [GSQ("SALOON_DISH")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var _, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return TokenizableArgUtility.AnyArgMatches(query, 1, (dishName) => Game1.dishOfTheDay.QualifiedItemId.Equals(ItemRegistry.QualifyItemId(dishName), StringComparison.OrdinalIgnoreCase));
    }
}