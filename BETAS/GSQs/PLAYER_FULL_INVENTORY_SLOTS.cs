using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerFullInventorySlots
{
    // Checks the number of backpack slots that a player has with an item in it
    [GSQ("PLAYER_FULL_INVENTORY_SLOTS")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGetInt(query, 2, out var min, out error, name: "int #Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 3, out var max, out error, int.MaxValue, name: "int #Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, target => target.MaxItems - target.freeSpotsInInventory() >= min && target.MaxItems - target.freeSpotsInInventory() <= max);
    }
}