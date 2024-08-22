using System;
using System.Runtime.CompilerServices;
using StardewModdingAPI;
using StardewValley;

namespace BETAS.Helpers;

public static class ArgUtilityExtensions
{
    private static string GetValueParseError(string[] array, int index, bool required, string typeSummary)
    {
        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 4);
        defaultInterpolatedStringHandler.AppendFormatted(required ? "required" : "optional");
        defaultInterpolatedStringHandler.AppendLiteral(" index ");
        defaultInterpolatedStringHandler.AppendFormatted(index);
        defaultInterpolatedStringHandler.AppendLiteral(" has value '");
        defaultInterpolatedStringHandler.AppendFormatted(array[index]);
        defaultInterpolatedStringHandler.AppendLiteral("', which can't be parsed as ");
        defaultInterpolatedStringHandler.AppendFormatted(typeSummary);
        return defaultInterpolatedStringHandler.ToStringAndClear();
    }

    private static string GetMissingRequiredIndexError(string[] array, int index)
    {
        switch (array.Length)
        {
            case 0:
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler =
                    new DefaultInterpolatedStringHandler(41, 1);
                defaultInterpolatedStringHandler.AppendLiteral("required index ");
                defaultInterpolatedStringHandler.AppendFormatted(index);
                defaultInterpolatedStringHandler.AppendLiteral(" not found (list is empty)");
                return defaultInterpolatedStringHandler.ToStringAndClear();
            }
            case 1:
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler =
                    new DefaultInterpolatedStringHandler(62, 1);
                defaultInterpolatedStringHandler.AppendLiteral("required index ");
                defaultInterpolatedStringHandler.AppendFormatted(index);
                defaultInterpolatedStringHandler.AppendLiteral(" not found (list has a single value at index 0)");
                return defaultInterpolatedStringHandler.ToStringAndClear();
            }
            default:
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler =
                    new DefaultInterpolatedStringHandler(55, 2);
                defaultInterpolatedStringHandler.AppendLiteral("required index ");
                defaultInterpolatedStringHandler.AppendFormatted(index);
                defaultInterpolatedStringHandler.AppendLiteral(" not found (list has indexes 0 through ");
                defaultInterpolatedStringHandler.AppendFormatted(array.Length - 1);
                defaultInterpolatedStringHandler.AppendLiteral(")");
                return defaultInterpolatedStringHandler.ToStringAndClear();
            }
        }
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