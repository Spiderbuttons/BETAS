using System;
using BETAS.AdvancedPermissions;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKGlboalModData
{
    /// <summary>When given a key, returns the value of that key in the farm's mod data.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    [TKString("GlobalModData")]
    public static bool Parse(string[] query, out string replacement, Random random, Farmer player)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var uniqueId, out var error) ||
            !TokenizableArgUtility.TryGetTokenizable(query, 2, out var key, out error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        var mod = BETAS.ModRegistry.Get(uniqueId);
        if (mod == null)
        {
            return TokenParser.LogTokenError(query, $"No mod found with unique ID '{uniqueId}'", out replacement);    
        }
        
        if (!GlobalModData.TryReadGlobalModData(mod, key, out var value, out error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        replacement = value;
        return true;
    }
}