using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class ItemModData
{
    // GSQ for checking whether an item has a specific mod data key with a specific value. If the value is omitted, it just checks if the key exists at all.
    [GSQ("ITEM_MOD_DATA")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item,
                out var error) || !TokenizableArgUtility.TryGet(query, 2, out var key, out error, name: "string Key") ||
            !TokenizableArgUtility.TryGetOptional(query, 3, out var value, out error, name: "string Value"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (item == null)
        {
            return false;
        }

        bool ignoreValue = !ArgUtility.HasIndex(query, 3);

        return item.modData.TryGetValue(key, out var data) &&
               (string.Equals(data, value, StringComparison.OrdinalIgnoreCase) || ignoreValue);
    }

    // GSQ for checking whether an item has a specific mod data key with a value within a specific range. Values are parsed as ints.
    [GSQ("ITEM_MOD_DATA_RANGE")]
    public static bool Query_Range(string[] query, GameStateQueryContext context)
    {
        if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item,
                out var error) || !TokenizableArgUtility.TryGet(query, 2, out var key, out error, name: "string Key") ||
            !TokenizableArgUtility.TryGetInt(query, 3, out var minRange, out error, name: "int Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 4, out var maxRange, out error, int.MaxValue, name: "int Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (item == null)
        {
            return false;
        }

        return item.modData.TryGetValue(key, out var data) && int.TryParse(data, out var dataInt) &&
               dataInt >= minRange && dataInt <= maxRange;
    }

    // GSQ for checking whether a comma- or space-delimited list of values in item mod data contains a specific value.
    [GSQ("ITEM_MOD_DATA_CONTAINS")]
    public static bool Query_Contains(string[] query, GameStateQueryContext context)
    {
        if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item,
                out var error) || !TokenizableArgUtility.TryGet(query, 2, out var key, out error, name: "string Key") ||
            !TokenizableArgUtility.TryGet(query, 3, out _, out error, false, name: "string Value"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (item == null)
        {
            return false;
        }

        if (!item.modData.TryGetValue(key, out var data))
        {
            return false;
        }

        var list = data.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
        return TokenizableArgUtility.AnyArgMatches(query, 3,
            (rawValue) => list.Contains(rawValue, StringComparer.OrdinalIgnoreCase));
    }
}