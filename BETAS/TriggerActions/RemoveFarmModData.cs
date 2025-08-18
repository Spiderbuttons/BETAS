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
        if (!TokenizableArgUtility.TryGet(args, 1, out string? key, out error, name: "string Key") ||
            !TokenizableArgUtility.TryGetOptional(args, 2, out string? value, out error, name: "string Value"))
        {
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