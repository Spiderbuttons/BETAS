using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerHeldItem
{
    // Checks if the item or tool a player is holding matches any given item ID.
    [GSQ("PLAYER_HELD_ITEM")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !TokenizableArgUtility.TryGetTokenizable(query, 2, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, target =>
        {
            return TokenizableArgUtility.AnyArgMatches(query, 2,
                (itemID) => target.ActiveObject?.QualifiedItemId.Equals(ItemRegistry.QualifyItemId(itemID)) == true ||
                           target.CurrentTool?.QualifiedItemId.Equals(ItemRegistry.QualifyItemId(itemID)) == true
                );
        });
    }
}