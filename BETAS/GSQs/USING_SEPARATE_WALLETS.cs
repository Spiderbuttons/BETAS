﻿using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class UsingSeparateWallets
{
    // Check whether the farmers in this save file are using separate wallets or not.
    [GSQ("USING_SEPARATE_WALLETS")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        return Game1.player.useSeparateWallets;
    }
}