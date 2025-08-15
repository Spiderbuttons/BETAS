using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcKissedToday
{
    // Check whether a given NPC has been kissed today by any farmer.
    [GSQ("NPC_KISSED_TODAY")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var _, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return ArgUtilityExtensions.AnyArgMatches(query, 1, (name) =>
            {
                var npc = Game1.getCharacterFromName(name);
                if (npc != null) return npc.hasBeenKissedToday.Value;
                
                Log.Warn($"No NPC found with name '{name}'");
                return false;
            });
    }
}