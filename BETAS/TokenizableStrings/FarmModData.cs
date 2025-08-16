using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKFarmModData
{
    /// <summary>When given a key, returns the value of that key in the farm's mod data.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    [TKString("FarmModData")]
    public static bool Parse(string[] query, out string replacement, Random random, Farmer player)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var key, out var error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        if (!Game1.getFarm().modData.TryGetValue(key, out var value))
        {
            return TokenParser.LogTokenError(query, $"Key not found in farm mod data: {key}", out replacement);
        }
        
        replacement = value;
        return true;
    }
}