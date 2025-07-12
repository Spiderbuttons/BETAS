using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class GlobalModData
{
    // GSQ for checking whether a mod's global mod data has a specific key with a specific value. If the value is omitted, it just checks if the key exists at all.
    [GSQ("GLOBAL_MOD_DATA")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var uniqueId, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizable(query, 2, out var key, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(query, 3, out var value, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        var mod = BETAS.ModRegistry.Get(uniqueId);
        if (mod == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, $"No mod found with unique ID '{uniqueId}'");
        }

        var ignoreValue = !ArgUtility.HasIndex(query, 3);

        return AdvancedPermissions.GlobalModData.TryReadGlobalModData(mod, key, out var data, out error) &&
               (string.Equals(data, value, StringComparison.OrdinalIgnoreCase) || ignoreValue);
    }

    // GSQ for checking whether a mod's global mod data has a specific mod data key with a value within a specific range. Values are parsed as ints.
    [GSQ("GLOBAL_MOD_DATA_RANGE")]
    public static bool Query_Range(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var uniqueId, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizable(query, 2, out var key, out error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 3, out var minRange, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 4, out var maxRange, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        var mod = BETAS.ModRegistry.Get(uniqueId);
        if (mod == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, $"No mod found with unique ID '{uniqueId}'");
        }

        return AdvancedPermissions.GlobalModData.TryReadGlobalModData(mod, key, out var data, out error) && int.TryParse(data, out var dataInt) &&
               dataInt >= minRange && dataInt <= maxRange;
    }

    // GSQ for checking whether a comma- or space-delimited list of values in a mod's global mod data contains a specific value.
    [GSQ("GLOBAL_MOD_DATA_CONTAINS")]
    public static bool Query_Contains(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var uniqueId, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizable(query, 2, out var key, out error) ||
            !ArgUtilityExtensions.TryGetTokenizable(query, 3, out var value, out error, false))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        var mod = BETAS.ModRegistry.Get(uniqueId);
        if (mod == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, $"No mod found with unique ID '{uniqueId}'");
        }

        if (!AdvancedPermissions.GlobalModData.TryReadGlobalModData(mod, key, out var data, out error))
        {
            return false;
        }

        var list = data.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
        return GameStateQuery.Helpers.AnyArgMatches(query, 3,
            (rawValue) => list.Contains(rawValue, StringComparer.OrdinalIgnoreCase));
    }
}