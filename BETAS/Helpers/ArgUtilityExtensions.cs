using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using StardewModdingAPI;
using HarmonyLib;
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
    
    public static bool TryGetOptionalPossiblyRelativeCoordinate(string[] array, int index, out int value, out string error, int defaultValue = 0)
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

        if (!(split.Length == 3 && (split[0].Contains("RelativeX") || split[0].Contains("RelativeY"))  && int.TryParse(split[2], out value)))
        {
            error = GetValueParseError(array, index, required: false, "an int");
            value = defaultValue;
            return false;
        }
        
        if (Game1.getCharacterFromName(split[1]) != null)
        {
            if (split[0].Contains("RelativeX"))
            {
                value += Game1.getCharacterFromName(split[1]).TilePoint.X;
            }
            else if (split[0].Contains("RelativeY"))
            {
                value += Game1.getCharacterFromName(split[1]).TilePoint.Y;
            }
            error = null;
            return true;
        } 
        if (split[1].Equals("Farmer")) {
            if (split[0].Contains("RelativeX"))
            {
                value += Game1.player.TilePoint.X;
            }
            else if (split[0].Contains("RelativeY"))
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