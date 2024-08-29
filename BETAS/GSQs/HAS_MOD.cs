using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class HasMod
{
    // Check whether a specific mod is installed.
    [GSQ("HAS_MOD")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var _, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.AnyArgMatches(query, 1, (modID) => BETAS.LoadedMods.Contains(modID));
    }
}