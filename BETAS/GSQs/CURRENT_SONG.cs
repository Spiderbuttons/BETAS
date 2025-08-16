using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class CurrentSong
{
    // Check whether the volume of a specific category is set between a min and max value. Volume is a float between 0 and 1.
    [GSQ("CURRENT_SONG")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGetOptional(query, 1, out _, out var error, name: "string Song ID"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (!ArgUtility.HasIndex(query, 1)) return (Game1.currentSong != null && Game1.currentSong.IsPlaying);
        return Game1.currentSong != null && TokenizableArgUtility.AnyArgMatches(query, 1, (songName) => Game1.currentSong.Name.Equals(songName, StringComparison.OrdinalIgnoreCase));
    }
}