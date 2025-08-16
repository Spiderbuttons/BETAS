using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class LocationModData
{
    // GSQ for checking whether a location has a specific mod data key with a specific value. If the value is omitted, it just checks if the key exists at all.
    [GSQ("LOCATION_MOD_DATA")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        GameLocation? location = context.Location;
        if (!TokenizableArgUtility.TryGetLocation(query, 1, ref location, out var error) ||
            !TokenizableArgUtility.TryGet(query, 2, out var key, out error) ||
            !TokenizableArgUtility.TryGetOptional(query, 3, out var value, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (location == null)
        {
            return false;
        }

        bool ignoreValue = !ArgUtility.HasIndex(query, 3);

        return location.modData.TryGetValue(key, out var data) &&
               (string.Equals(data, value, StringComparison.OrdinalIgnoreCase) || ignoreValue);
    }

    // GSQ for checking whether a location has a specific mod data key with a value within a specific range. Values are parsed as ints.
    [GSQ("LOCATION_MOD_DATA_RANGE")]
    public static bool Query_Range(string[] query, GameStateQueryContext context)
    {
        GameLocation? location = context.Location;
        if (!TokenizableArgUtility.TryGetLocation(query, 1, ref location, out var error) ||
            !TokenizableArgUtility.TryGet(query, 2, out var key, out error) ||
            !TokenizableArgUtility.TryGetInt(query, 3, out var minRange, out error) ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 4, out var maxRange, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (location == null)
        {
            return false;
        }

        return location.modData.TryGetValue(key, out var data) && int.TryParse(data, out var dataInt) &&
               dataInt >= minRange && dataInt <= maxRange;
    }

    // GSQ for checking whether a comma- or space-delimited list of values in item mod data contains a specific value.
    [GSQ("LOCATION_MOD_DATA_CONTAINS")]
    public static bool Query_Contains(string[] query, GameStateQueryContext context)
    {
        GameLocation? location = context.Location;
        if (!TokenizableArgUtility.TryGetLocation(query, 1, ref location, out var error) ||
            !TokenizableArgUtility.TryGet(query, 2, out var key, out error) ||
            !TokenizableArgUtility.TryGet(query, 3, out _, out error, false))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (location == null)
        {
            return false;
        }

        if (!location.modData.TryGetValue(key, out var data))
        {
            return false;
        }

        var list = data.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
        return TokenizableArgUtility.AnyArgMatches(query, 3,
            (rawValue) => list.Contains(rawValue, StringComparer.OrdinalIgnoreCase));
    }
}