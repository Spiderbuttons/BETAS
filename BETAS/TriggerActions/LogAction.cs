using BETAS.AdvancedPermissions;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class LogAction
{
    // Log to the console as a specific mod instead of as BETAS.
    [Action("Log")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var uniqueId, out error) ||
            !ArgUtilityExtensions.TryGetTokenizable(args, 2, out var message, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableEnum(args, 3, out var logLevel, out error, defaultValue: LogLevel.Info))
        {
            return false;
        }

        var mod = BETAS.ModRegistry.Get(uniqueId);
        if (mod == null)
        {
            error = $"No mod found with unique ID '{uniqueId}'.";
            return false;
        }

        return Logging.TryLogAsMod(mod, message, logLevel, out error);
    }
}