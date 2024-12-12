using System;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class GoldClockActive
{
    // Check whether the gold clock is a) built and b) active i.e. not turned off.
    [GSQ("GOLD_CLOCK_ACTIVE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        return Game1.IsBuildingConstructed("Gold Clock") && !Game1.netWorldState.Value.goldenClocksTurnedOff.Value;
    }
}