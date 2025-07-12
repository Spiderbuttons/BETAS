using System;
using BETAS.Attributes;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKNumberOffset
{
    /// <summary>When given a number and an offset, returns that number plus the offset value supplied.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    [TKString("NumberOffset")]
    public static bool Parse(string[] query, out string replacement, Random random, Farmer player)
    {
        if (!ArgUtility.TryGetInt(query, 1, out var number, out var error) || !ArgUtility.TryGetInt(query, 2, out var offset, out error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        replacement = (number + offset).ToString();
        return true;
    }
}