using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKToken
{
    /// <summary>When given a key, returns the value of that key in the farm's mod data.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    [TKString("Token")]
    public static bool Parse(string[] query, out string? replacement, Random random, Farmer player)
    {
        if (!ArgUtility.TryGet(query, 1, out var token, out var error) ||
            !ArgUtility.TryGetOptionalRemainder(query, 2, out var args))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        replacement = $"[{token} {args}]";
        return true;
    }
}