﻿using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class Volume
{
    // Check whether the volume of a specific category is set between a min and max value. Volume is a float between 0 and 1.
    [GSQ("VOLUME")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var category, out var error) || !ArgUtilityExtensions.TryGetTokenizableFloat(query, 2, out var min, out error) || !ArgUtilityExtensions.TryGetOptionalTokenizableFloat(query, 3, out var max, out error, int.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        float? volumeLevel = category.ToLower() switch {
            "music" => Game1.options.musicVolumeLevel,
            "sound" => Game1.options.soundVolumeLevel,
            "ambient" => Game1.options.ambientVolumeLevel,
            "footstep" => Game1.options.footstepVolumeLevel,
            _ => null
        };
        
        if (volumeLevel is null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, $"Invalid volume category: {category}");
        }
        
        return volumeLevel >= min && volumeLevel <= max;
    }
}