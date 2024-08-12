using System;
using System.Linq;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class LocationModData
{
    // GSQ for checking whether a location has a specific mod data key with a specific value. If the value is omitted, it just checks if the key exists at all.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        GameLocation location = context.Location;
        if (!GameStateQuery.Helpers.TryGetLocationArg(query, 1, ref location, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGetOptional(query, 3, out var value, out error))
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
    public static bool Query_Range(string[] query, GameStateQueryContext context)
    {
        GameLocation location = context.Location;
        if (!GameStateQuery.Helpers.TryGetLocationArg(query, 1, ref location, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGetInt(query, 3, out var minRange, out error) || !ArgUtility.TryGetOptionalInt(query, 4, out var maxRange, out error, int.MaxValue))
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
    public static bool Query_Contains(string[] query, GameStateQueryContext context)
    {
        GameLocation location = context.Location;
        if (!GameStateQuery.Helpers.TryGetLocationArg(query, 1, ref location, out var error) || !ArgUtility.TryGet(query, 2, out var key, out error) || !ArgUtility.TryGet(query, 3, out var value, out error, false))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        if (location == null)
        {
            return false;
        }
            
        return location.modData.TryGetValue(key, out var data) && data.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList().Contains(value, StringComparer.OrdinalIgnoreCase);
    }
}