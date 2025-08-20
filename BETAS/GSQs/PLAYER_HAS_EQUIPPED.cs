using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerHasEquipped
{
    // GSQ for checking whether a player has any of the given item IDs equipped
    [GSQ("PLAYER_HAS_EQUIPPED")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGet(query, 2, out _, out error, name: "string Item ID"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            var equipment = target.GetEquippedItems().Concat(target.trinketItems).Select(item => item.QualifiedItemId).ToHashSet();
            equipment.LogPairs();
            return TokenizableArgUtility.AnyArgMatches(query, 2, itemId => equipment.Contains(ItemRegistry.QualifyItemId(itemId)));
        });
    }
}