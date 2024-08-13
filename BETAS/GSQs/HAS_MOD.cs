using System;
using System.Linq;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class HasMod
{
    // Check whether a specific mod is installed.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var _, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return GameStateQuery.Helpers.AnyArgMatches(query, 1, (modID) => BETAS.LoadedMods.Contains(modID));
    }
}