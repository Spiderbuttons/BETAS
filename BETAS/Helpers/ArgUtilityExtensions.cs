using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public static bool TryGetPossiblyRelativeLocationName(string[] array, int index, out string value, out string error,
        bool allowBlank = true)
    {
        if (array == null)
        {
            value = null;
            error = "argument list is null";
            return false;
        }

        if (index < 0 || index >= array.Length)
        {
            value = null;
            error = GetMissingRequiredIndexError(array, index);
            return false;
        }

        string[] split = array[index].Split(':');
        value = array[index];

        if (split.Length == 2)
        {
            if (string.Equals(split[0], "NPC", StringComparison.OrdinalIgnoreCase))
            {
                var npc = Game1.getCharacterFromName(split[1]);
                if (npc != null)
                {
                    if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
                        !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
                    {
                        value = npc.currentLocation.Name;
                    }
                    else
                    {
                        value = cache.LocationName;
                    }
                }
            } else if (string.Equals(split[0], "Farmer", StringComparison.OrdinalIgnoreCase))
            {
                value = split[1].ToLower() switch
                {
                    "current" => Game1.player.currentLocation.Name,
                    _ => Game1.player.currentLocation.Name
                };
            }
        }

        if (!allowBlank && string.IsNullOrWhiteSpace(value))
        {
            value = null;
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler =
                new DefaultInterpolatedStringHandler(33, 1);
            defaultInterpolatedStringHandler.AppendLiteral("required index ");
            defaultInterpolatedStringHandler.AppendFormatted(index);
            defaultInterpolatedStringHandler.AppendLiteral(" has a blank value");
            error = defaultInterpolatedStringHandler.ToStringAndClear();
            return false;
        }

        error = null;
        return true;
    }

    public static bool TryGetOptionalPossiblyRelativeLocationName(string[] array, int index, out string value,
        out string error, string defaultValue = null)
    {
        if (array == null || index < 0 || index >= array.Length || array[index] == string.Empty)
        {
            error = null;
            value = defaultValue;
            return true;
        }

        string[] split = array[index].Split(':');
        if (split.Length == 1)
        {
            value = array[index];
            error = null;
            return true;
        }

        if (split.Length == 2 && string.Equals(split[0], "NPC", StringComparison.OrdinalIgnoreCase))
        {
            var npc = Game1.getCharacterFromName(split[1]);
            if (npc != null)
            {
                if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
                    !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
                {
                    value = npc.currentLocation.Name;
                }
                else
                {
                    value = cache.LocationName;
                }
                error = null;
                return true;
            }
        }
        
        if (split.Length == 2 && string.Equals(split[0], "Farmer", StringComparison.OrdinalIgnoreCase))
        {
            value = split[1].ToLower() switch
            {
                "current" => Game1.player.currentLocation.Name,
                _ => Game1.player.currentLocation.Name
            };
            error = null;
            return true;
        }

        error = GetValueParseError(array, index, required: false, "a location name");
        value = defaultValue;
        return false;
    }

    public static bool TryGetOptionalPossiblyRelativeCoordinate(string[] array, int index, out int value,
        out string error, int defaultValue = 0)
    {
        if (array == null || index < 0 || index >= array.Length || array[index] == string.Empty)
        {
            error = null;
            value = defaultValue;
            return true;
        }

        string[] split = array[index].Split(':');
        if (split.Length == 1 && int.TryParse(split[0], out value))
        {
            error = null;
            return true;
        }

        if (!(split.Length == 3 &&
              (string.Equals(split[0], "RelativeX", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(split[0], "RelativeY", StringComparison.OrdinalIgnoreCase)) &&
              int.TryParse(split[2], out value)))
        {
            error = GetValueParseError(array, index, required: false, "an int");
            value = defaultValue;
            return false;
        }

        var npc = Game1.getCharacterFromName(split[1]);
        if (npc != null)
        {
            if (string.Equals(split[0], "RelativeX", StringComparison.OrdinalIgnoreCase))
            {
                if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
                    !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
                {
                    value += npc.TilePoint.X;
                }
                else
                {
                    value += cache.TilePoint.X;
                } 
            }
            else if (string.Equals(split[0], "RelativeY", StringComparison.OrdinalIgnoreCase))
            {
                if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
                    !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cache)))
                {
                    value += npc.TilePoint.Y;
                }
                else
                {
                    value += cache.TilePoint.Y;
                }
            }

            error = null;
            return true;
        }

        if (split[1].Equals("Farmer"))
        {
            if (string.Equals(split[0], "RelativeX", StringComparison.OrdinalIgnoreCase))
            {
                value += Game1.player.TilePoint.X;
            }
            else if (string.Equals(split[0], "RelativeY", StringComparison.OrdinalIgnoreCase))
            {
                value += Game1.player.TilePoint.Y;
            }

            error = null;
            return true;
        }

        error = GetValueParseError(array, index, required: false, "an int");
        value = defaultValue;
        return false;
    }
}