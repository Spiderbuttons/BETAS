﻿using System;
using System.Globalization;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKPlayerModData
{
    /// <summary>When given a key, returns the value of that key in the current player's mod data.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    [TKString("PlayerModData")]
    public static bool Parse(string[] query, out string replacement, Random random, Farmer player)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var key, out var error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        if (!Game1.player.modData.TryGetValue(key, out var value))
        {
            return TokenParser.LogTokenError(query, $"Key not found in player mod data: {key}", out replacement);
        }
        
        replacement = value;
        return true;
    }
}