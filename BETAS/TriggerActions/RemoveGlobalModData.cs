using BETAS.AdvancedPermissions;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class RemoveGlobalModData
{
    // Remove a mod's global mod data with a given key, optionally only if the current value matches a given value.
    [Action("RemoveGlobalModData")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string? uniqueId, out error) ||
            !ArgUtilityExtensions.TryGetTokenizable(args, 2, out string? key, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(args, 3, out string? value, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_RemoveGlobalModData <UniqueId> <Key> [Value]";
            return false;
        }

        var mod = BETAS.ModRegistry.Get(uniqueId);
        if (mod == null)
        {
            error = $"No mod found with unique ID '{uniqueId}'.";
            return false;
        }

        if (!GlobalModData.TryReadGlobalModData(mod, key, out var currentValue, out error))
        {
            return false;
        }
        
        if (!ArgUtility.HasIndex(args, 3))
        {
            return GlobalModData.TryWriteGlobalModData(mod, key, value, out error);
        }

        return currentValue != value || GlobalModData.TryWriteGlobalModData(mod, key, null, out error);
    }
}