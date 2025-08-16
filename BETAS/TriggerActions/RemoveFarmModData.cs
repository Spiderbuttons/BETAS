using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class RemoveFarmModData
{
    // Remove an entry from the current farm's mod data dictionary, optionally only if the current value matches a given value.
    [Action("RemoveFarmModData")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(args, 1, out string? key, out error) ||
            !TokenizableArgUtility.TryGetOptionalTokenizable(args, 2, out string? value, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_RemoveFarmModData <Key> [Value]";
            return false;
        }

        if (!ArgUtility.HasIndex(args, 2))
        {
            Game1.getFarm().modData.Remove(key);
        }
        else
        {
            if (Game1.getFarm().modData.TryGetValue(key, out var existingValue) && existingValue == value)
            {
                Game1.getFarm().modData.Remove(key);
            }
        }
        return true;
    }
}