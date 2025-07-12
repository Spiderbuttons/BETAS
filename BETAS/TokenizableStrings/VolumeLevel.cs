using System;
using System.Globalization;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKVolumeLevel
{
    /// <summary>When given a volume category, return the level that volume option is set to.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    [TKString("VolumeLevel")]
    public static bool Parse(string[] query, out string replacement, Random random, Farmer player)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var category, out var error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        float? vol = category.ToLower() switch {
            "music" => Game1.options.musicVolumeLevel,
            "sound" => Game1.options.soundVolumeLevel,
            "ambient" => Game1.options.ambientVolumeLevel,
            "footstep" => Game1.options.footstepVolumeLevel,
            _ => null
        };
        
        if (vol is null)
        {
            return TokenParser.LogTokenError(query, $"Invalid volume category: {category}", out replacement);
        }
        
        replacement = vol.Value.ToString(CultureInfo.InvariantCulture);
        return true;
    }
}