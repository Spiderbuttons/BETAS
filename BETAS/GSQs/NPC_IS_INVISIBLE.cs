using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcIsInvisible
{
    // Check whether any given NPC is invisible or not.
    [GSQ("NPC_IS_INVISIBLE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out _, out var error, name: "string NPC"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return TokenizableArgUtility.AnyArgMatches(query, 1, (name) => {
            var npc = Game1.getCharacterFromName(name);
            if (npc != null) return npc.IsInvisible;
            
            Log.Warn($"No NPC found with name '{name}'");
            return false;
        });
    }
}