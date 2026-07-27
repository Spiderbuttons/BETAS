using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKPlayerModData
{
    /// <summary>When given a key, returns the value of that key in the current player's mod data.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    [TKString("PlayerModData")]
    public static bool Parse(string[] query, out string replacement, Random random, Farmer player)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var key, out var error) ||
            !TokenizableArgUtility.TryGetOptional(query, 2, out var defaultValue, out error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        if (!Game1.player.modData.TryGetValue(key, out var value))
        {
            if (defaultValue is null)
            {
                return TokenParser.LogTokenError(query, $"Key not found in player mod data: {key}", out replacement);
            }

            value = defaultValue;
        }
        
        replacement = value;
        return true;
    }
}