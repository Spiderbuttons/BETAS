using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class WriteFarmModData
{
    // Write a value to the current farm's mod data with a given key.
    [Action("WriteFarmModData")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? key, out error) ||
            !TokenizableArgUtility.TryGet(args, 2, out string? value, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_WriteFarmModData <Key> <Value>";
            return false;
        }

        Game1.getFarm().modData[key] = value;
        return true;
    }
}