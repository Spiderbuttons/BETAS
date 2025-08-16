using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class RemovePlayerModData
{
    // Remove an entry from the current player's mod data dictionary, optionally only if the current value matches a given value.
    [Action("RemovePlayerModData")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? key, out error) ||
            !TokenizableArgUtility.TryGetOptional(args, 2, out string? value, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_RemovePlayerModData <Key> [Value]";
            return false;
        }

        if (!ArgUtility.HasIndex(args, 2))
        {
            Game1.player.modData.Remove(key);
        }
        else
        {
            if (Game1.player.modData.TryGetValue(key, out var existingValue) && existingValue == value)
            {
                Game1.player.modData.Remove(key);
            }
        }
        return true;
    }
}