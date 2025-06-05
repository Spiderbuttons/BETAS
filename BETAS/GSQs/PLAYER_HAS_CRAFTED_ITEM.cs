using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerHasCraftedItem
{
    // Check whether the player has ever crafted a specific item.
    [GSQ("PLAYER_HAS_CRAFTED_ITEM")]
    public static bool Current(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizable(query, 2, out var recipeId, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 3, out var min, out error, defaultValue: 1) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 4, out var max, out error, defaultValue: int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, target =>
        {
            if (!target.craftingRecipes.TryGetValue(recipeId, out int count))
                return false;
            
            return count >= min && count <= max;
        });
    }
}