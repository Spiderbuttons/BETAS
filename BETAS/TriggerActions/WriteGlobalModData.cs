using BETAS.AdvancedPermissions;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class WriteGlobalModData
{
    // Write a value to global mod data with a given key.
    [Action("WriteGlobalModData")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? uniqueId, out error) ||
            !TokenizableArgUtility.TryGet(args, 2, out string? key, out error) ||
            !TokenizableArgUtility.TryGet(args, 3, out string? value, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_WriteGlobalModData <UniqueId> <Key> <Value>";
            return false;
        }

        var mod = BETAS.ModRegistry.Get(uniqueId);
        if (mod == null)
        {
            error = $"No mod found with unique ID '{uniqueId}'.";
            return false;
        }

        return GlobalModData.TryWriteGlobalModData(mod, key, value, out error);
    }
}