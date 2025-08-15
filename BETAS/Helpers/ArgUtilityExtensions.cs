using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.Helpers;

public static class ArgUtilityExtensions
{
    private static string GetFieldLabel(int index, string? name)
    {
        return name != null ? $"index {index} ({name})" : $"index {index}";
    }
    
    private static string GetValueParseError(string[] array, int index, string? name, bool required, string typeSummary)
    {
        return $"{(required ? "required" : "optional")} {GetFieldLabel(index, name)} has value '{array[index]}', which can't be parsed as {typeSummary}";
    }

    private static string GetMissingRequiredIndexError(string[] array, int index, string? name)
    {
        return array.Length switch
        {
            0 => "required " + GetFieldLabel(index, name) + " not found (list is empty)", 
            1 => "required " + GetFieldLabel(index, name) + " not found (list has a single value at index 0)", 
            _ => $"required {GetFieldLabel(index, name)} not found (list has indexes 0 through {array.Length - 1})", 
        };
    }

    private static string[] CombineTokenizableIndices(string[]? array)
    {
        if (array == null) return [];

        List<string> newArray = [];
        int open = 0;
        foreach (var item in array)
        {
            if (open > 0) newArray[^1] += $" {item}";
            else newArray.Add(item);
            if (item.StartsWith('[')) open++;
            if (item.EndsWith(']')) open--;
        }
        
        return newArray.ToArray();
    }
    
    public static bool AnyArgMatches(string[] query, int startAt, Func<string, bool?> check)
    {
        query = CombineTokenizableIndices(query);
        for (int i = startAt; i < query.Length; i++)
        {
            bool? flag = check(TokenParser.ParseText(query[i]));
            if (flag.HasValue)
            {
                if (flag.GetValueOrDefault())
                {
                    return true;
                }
                continue;
            }
            return false;
        }
        return false;
    }
    
    public static void ForEachArg(string[] args, int startIndex, Action<string> action)
    {
        args = CombineTokenizableIndices(args);
        for (var i = startIndex; i < args.Length; i++)
        {
            action(TokenParser.ParseText(args[i]));
        }
    }

    public static bool TryGetTokenizable(string[]? array, int index, [NotNullWhen(true)] out string? value, out string? error,
        bool allowBlank = true, [CallerArgumentExpression("value")] string? name = null)
    {
        if (array == null)
        {
            value = null;
            error = "argument list is null";
            return false;
        }
        
        array = CombineTokenizableIndices(array);
        
        if (index < 0 || index >= array.Length)
        {
            value = null;
            error = GetMissingRequiredIndexError(array, index, name);
            return false;
        }

        value = TokenParser.ParseText(array[index]);
        if (!allowBlank && string.IsNullOrWhiteSpace(value))
        {
            value = null;
            error = $"required {GetFieldLabel(index, name)} has a {(!string.IsNullOrWhiteSpace(array[index]) ? "blank value after parsing" : "blank value")}";
            return false;
        }

        error = null;
        return true;
    }

    public static bool TryGetOptionalTokenizable(string[]? array, int index, out string? value,
        out string? error, string? defaultValue = null, bool allowBlank = true, [CallerArgumentExpression("value")] string? name = null)
    {
        if (array == null)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        array = CombineTokenizableIndices(array);
        
        if (index < 0 || index >= array.Length || (!allowBlank && array[index] == string.Empty))
        {
            value = defaultValue;
            error = null;
            return true;
        }

        value = TokenParser.ParseText(array[index]);
        if (!allowBlank && string.IsNullOrWhiteSpace(value))
        {
            value = null;
            error = $"optional {GetFieldLabel(index, name)} can't have a {(!string.IsNullOrWhiteSpace(array[index]) ? "blank value after parsing" : "blank value")}";
            return false;
        }

        error = null;
        return true;
    }

    public static bool TryGetTokenizableLocationName(string[] array, int index, GameLocation contextualLocation, [NotNullWhen(true)] out string? value, out string? error)
    {
        if (!TryGetTokenizable(array, index, out value, out error, allowBlank: false))
        {
            return false;
        }
        
        if (string.Equals(value, "Here", StringComparison.OrdinalIgnoreCase))
        {
            value = Game1.player.currentLocation.Name;
            return true;
        }
        
        if (string.Equals(value, "Target", StringComparison.OrdinalIgnoreCase))
        {
            value = contextualLocation.Name ?? Game1.currentLocation.Name;
            return true;
        }

        return true;
    }
    
    public static bool TryGetTokenizableLocation(string[] query, int index, [NotNullWhen(true)] ref GameLocation? location, out string? error)
    {
        if (!TryGetTokenizable(query, index, out var locationTarget, out error))
        {
            location = null;
            return false;
        }
        
        GameLocation loaded = GameStateQuery.Helpers.GetLocation(locationTarget, location);
        if (loaded == null)
        {
            error = "no location found matching '" + locationTarget + "'";
            return false;
        }
        location = loaded;
        return true;
    }

    public static bool TryGetTokenizableInt(string[] array, int index, out int value, out string? error, [CallerArgumentExpression("value")] string? name = null)
    {
        if (!TryGetTokenizable(array, index, out var raw, out error, allowBlank: false, name))
        {
            value = 0;
            return false;
        }
        
        if (!int.TryParse(raw, out value))
        {
            value = 0;
            error = GetValueParseError(array, index, name, required: true, "an integer");
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool TryGetOptionalTokenizableInt(string[]? array, int index, out int value, out string? error,
        int defaultValue = 0, [CallerArgumentExpression("value")] string? name = null)
    {
        if (array == null)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        array = CombineTokenizableIndices(array);
        
        if (index < 0 || index >= array.Length || array[index] == string.Empty)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        if (!int.TryParse(TokenParser.ParseText(array[index]), out value))
        {
            value = defaultValue;
            error = GetValueParseError(array, index, name, required: false, "an integer");
            return false;
        }
        
        error = null;
        return true;
    }
    
    public static bool TryGetTokenizableFloat(string[] array, int index, out float value, out string? error, [CallerArgumentExpression("value")] string? name = null)
    {
        if (!TryGetTokenizable(array, index, out var raw, out error, allowBlank: false, name))
        {
            value = 0f;
            return false;
        }
        
        if (!float.TryParse(raw, out value))
        {
            value = 0f;
            error = GetValueParseError(array, index, name, required: true, "a float");
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool TryGetOptionalTokenizableFloat(string[]? array, int index, out float value, out string? error,
        float defaultValue = 0f, [CallerArgumentExpression("value")] string? name = null)
    {
        if (array == null)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        array = CombineTokenizableIndices(array);
        
        if (index < 0 || index >= array.Length || array[index] == string.Empty)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        if (!float.TryParse(TokenParser.ParseText(array[index]), out value))
        {
            value = defaultValue;
            error = GetValueParseError(array, index, name, required: false, "a float");
            return false;
        }
        
        error = null;
        return true;
    }
    
    public static bool TryGetTokenizableBool(string[] array, int index, out bool value, out string? error, [CallerArgumentExpression("value")] string? name = null)
    {
        if (!TryGetTokenizable(array, index, out var raw, out error, allowBlank: false, name))
        {
            value = false;
            return false;
        }
        
        if (!bool.TryParse(raw, out value))
        {
            value = false;
            error = GetValueParseError(array, index, name, required: true, "a boolean");
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool TryGetOptionalTokenizableBool(string[]? array, int index, out bool value, out string? error,
        bool defaultValue = false, [CallerArgumentExpression("value")] string? name = null)
    {
        if (array == null)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        array = CombineTokenizableIndices(array);
        
        if (index < 0 || index >= array.Length || array[index] == string.Empty)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        if (!bool.TryParse(TokenParser.ParseText(array[index]), out value))
        {
            value = defaultValue;
            error = GetValueParseError(array, index, name, required: false, "a boolean");
            return false;
        }
        
        error = null;
        return true;
    }

    public static bool TryGetTokenizableEnum<TEnum>(string[] array, int index, out TEnum value, out string? error, [CallerArgumentExpression("value")] string? name = null)
        where TEnum : struct
    {
        if (!TryGetTokenizable(array, index, out var raw, out error, allowBlank: false, name))
        {
            value = default(TEnum);
            return false;
        }
        
        if (!Utility.TryParseEnum(raw, out value))
        {
            Type type = typeof(TEnum);
            value = default;
            error = GetValueParseError(array, index, name, required: true, $"an enum of type '{type.FullName ?? type.Name}' (should be one of {string.Join(", ", Enum.GetNames(type))})");
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool TryGetOptionalTokenizableEnum<TEnum>(string[]? array, int index, out TEnum value, out string? error,
        TEnum defaultValue = default, [CallerArgumentExpression("value")] string? name = null) where TEnum : struct
    {
        if (array == null)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        array = CombineTokenizableIndices(array);
        
        if (index < 0 || index >= array.Length || array[index] == string.Empty)
        {
            value = defaultValue;
            error = null;
            return true;
        }
        
        if (!Utility.TryParseEnum(TokenParser.ParseText(array[index]), out value))
        {
            Type type = typeof(TEnum);
            value = defaultValue;
            error = GetValueParseError(array, index, name, required: false, $"an enum of type '{type.FullName ?? type.Name}' (should be one of {string.Join(", ", Enum.GetNames(type))})");
            return false;
        }
        
        error = null;
        return true;
    }
}