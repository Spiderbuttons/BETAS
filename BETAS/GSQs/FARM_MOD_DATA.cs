﻿using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class FarmModData
{
    // GSQ for checking whether the farm has a specific mod data key with a specific value. If the value is omitted, it just checks if the key exists at all.
    [GSQ("FARM_MOD_DATA")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var key, out var error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(query, 2, out var value, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var ignoreValue = !ArgUtility.HasIndex(query, 2);

        return Game1.getFarm().modData.TryGetValue(key, out var data) &&
               (string.Equals(data, value, StringComparison.OrdinalIgnoreCase) || ignoreValue);
    }

    // GSQ for checking whether the farm has a specific mod data key with a value within a specific range. Values are parsed as ints.
    [GSQ("FARM_MOD_DATA_RANGE")]
    public static bool Query_Range(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var key, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out var minRange, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 3, out var maxRange, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return Game1.getFarm().modData.TryGetValue(key, out var data) && int.TryParse(data, out var dataInt) &&
               dataInt >= minRange && dataInt <= maxRange;
    }

    // GSQ for checking whether a comma- or space-delimited list of values in the farm's mod data contains a specific value.
    [GSQ("FARM_MOD_DATA_CONTAINS")]
    public static bool Query_Contains(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var key, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizable(query, 2, out _, out error, false))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }


        if (!Game1.getFarm().modData.TryGetValue(key, out var data))
        {
            return false;
        }

        var list = data.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
        return GameStateQuery.Helpers.AnyArgMatches(query, 2,
            (rawValue) => list.Contains(rawValue, StringComparer.OrdinalIgnoreCase));
    }
}