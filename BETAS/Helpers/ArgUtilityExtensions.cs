using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.Helpers;

public static class ArgUtilityExtensions
{
    private static string GetValueParseError(string[] array, int index, bool required, string typeSummary)
    {
        return $"{(required ? "required" : "optional")} index {index} has value '{array[index]}', which can't be parsed as {typeSummary}";
    }

    private static string GetMissingRequiredIndexError(string[] array, int index)
    {
        return array.Length switch
        {
            0 => $"required index {index} not found (list is empty)",
            1 => $"required index {index} not found (list has a single value at index 0)",
            _ => $"required index {index} not found (list has indexes 0 through {array.Length - 1})"
        };
    }

    public static GameLocation GetCharacterLocationFromNameOrCache(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var npc = Game1.getCharacterFromName(name);
        if (npc is null) return null;

        if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
        {
            return npc.currentLocation;
        }

        return Game1.getLocationFromName(cache.LocationName);
    }
    
    public static void ForEachArg(string[] args, int startIndex, Action<string> action)
    {
        for (var i = startIndex; i < args.Length; i++)
        {
            action(args[i]);
        }
    }
    
    public static Vector2? GetCharacterPositionFromNameOrCache(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }
        
        var npc = Game1.getCharacterFromName(name);
        if (npc is null) return null;
        
        if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
        {
            return npc.Position;
        }
        
        return cache.Position;
    }
    
    public static Point? GetCharacterTilePointFromNameOrCache(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }
        
        var npc = Game1.getCharacterFromName(name);
        if (npc is null) return null;
        
        if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
        {
            return npc.TilePoint;
        }
        
        return cache.TilePoint;
    }

    private static string[] CombineTokenizableIndices(string[] array)
    {
        if (array == null) return null;

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

    public static bool TryGetTokenizable(string[] array, int index, out string value, out string error,
        bool allowBlank = true)
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
            error = GetMissingRequiredIndexError(array, index);
            return false;
        }

        value = TokenParser.ParseText(array[index]);
        if (!allowBlank && string.IsNullOrWhiteSpace(value))
        {
            value = null;
            error = $"required index {index} has a blank value";
            return false;
        }

        error = null;
        return true;
    }

    public static bool TryGetOptionalTokenizable(string[] array, int index, out string value,
        out string error, string defaultValue = null, bool allowBlank = true)
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
            error = $"optional index {index} can't have a blank value";
            return false;
        }

        error = null;
        return true;
    }

    public static bool TryGetTokenizableLocationName(string[] array, int index, GameLocation contextualLocation, out string value, out string error,
        bool allowBlank = true)
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
    
    public static bool TryGetTokenizableLocation(string[] query, int index, ref GameLocation location, out string error)
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

    public static bool TryGetTokenizableInt(string[] array, int index, out int value, out string error,
        bool allowBlank = true)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(array, index, out var raw, out error, allowBlank: false))
        {
            value = 0;
            return false;
        }
        
        if (!int.TryParse(raw, out value))
        {
            value = 0;
            error = ArgUtilityExtensions.GetValueParseError(array, index, required: true, "an integer");
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool TryGetOptionalTokenizableInt(string[] array, int index, out int value, out string error,
        int defaultValue = 0)
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
            error = ArgUtilityExtensions.GetValueParseError(array, index, required: false, "an integer");
            return false;
        }
        
        error = null;
        return true;
    }
    
    public static bool TryGetTokenizableFloat(string[] array, int index, out float value, out string error,
        bool allowBlank = true)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(array, index, out var raw, out error, allowBlank: false))
        {
            value = 0f;
            return false;
        }
        
        if (!float.TryParse(raw, out value))
        {
            value = 0f;
            error = ArgUtilityExtensions.GetValueParseError(array, index, required: true, "a float");
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool TryGetOptionalTokenizableFloat(string[] array, int index, out float value, out string error,
        float defaultValue = 0f)
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
            error = ArgUtilityExtensions.GetValueParseError(array, index, required: false, "a float");
            return false;
        }
        
        error = null;
        return true;
    }
    
    public static bool TryGetTokenizableBool(string[] array, int index, out bool value, out string error,
        bool allowBlank = true)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(array, index, out var raw, out error, allowBlank: false))
        {
            value = false;
            return false;
        }
        
        if (!bool.TryParse(raw, out value))
        {
            value = false;
            error = ArgUtilityExtensions.GetValueParseError(array, index, required: true, "a boolean");
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool TryGetOptionalTokenizableBool(string[] array, int index, out bool value, out string error,
        bool defaultValue = false)
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
            error = ArgUtilityExtensions.GetValueParseError(array, index, required: false, "a boolean");
            return false;
        }
        
        error = null;
        return true;
    }

    public static bool TryGetTokenizableEnum<TEnum>(string[] array, int index, out TEnum value, out string error)
        where TEnum : struct
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(array, index, out var raw, out error, allowBlank: false))
        {
            value = default(TEnum);
            return false;
        }
        
        if (!Utility.TryParseEnum<TEnum>(raw, out value))
        {
            Type type = typeof(TEnum);
            value = default(TEnum);
            error = ArgUtilityExtensions.GetValueParseError(array, index, required: true, $"an enum of type '{type.FullName ?? type.Name}' (should be one of {string.Join(", ", Enum.GetNames(type))})");
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool TryGetOptionalTokenizableEnum<TEnum>(string[] array, int index, out TEnum value, out string error,
        TEnum defaultValue = default(TEnum)) where TEnum : struct
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
        
        if (!Utility.TryParseEnum<TEnum>(TokenParser.ParseText(array[index]), out value))
        {
            Type type = typeof(TEnum);
            value = defaultValue;
            error = ArgUtilityExtensions.GetValueParseError(array, index, required: false, $"an enum of type '{type.FullName ?? type.Name}' (should be one of {string.Join(", ", Enum.GetNames(type))})");
            return false;
        }
        
        error = null;
        return true;
    }
}