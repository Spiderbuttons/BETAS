using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class WritePlayerModData
{
    // Write a value to the current player's mod data with a given key.
    [Action("WritePlayerModData")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string key, out error) || !ArgUtilityExtensions.TryGetTokenizable(args, 2, out string value, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_WritePlayerModData <Key> <Value>";
            return false;
        }

        Game1.player.modData[key] = value;
        return true;
    }
}