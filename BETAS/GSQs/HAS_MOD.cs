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
        if (!TokenizableArgUtility.TryGet(query, 1, out var _, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return TokenizableArgUtility.AnyArgMatches(query, 1, (modID) => BETAS.LoadedMods.Contains(modID));
    }
}