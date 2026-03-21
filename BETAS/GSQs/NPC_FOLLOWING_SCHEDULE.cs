using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.GSQs;

public static class NpcFollowingSchedule
{
    // Check whether a given NPC is following any of the given schedules today.
    [GSQ("NPC_FOLLOWING_SCHEDULE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var npcName, out var error, name: "string NPC") ||
            !TokenizableArgUtility.TryGet(query, 2, out _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            Log.Warn($"No NPC found with name '{npcName}'");
            return false;
        }

        return TokenizableArgUtility.AnyArgMatches(query, 2, (schedule) => npc.ScheduleKey.EqualsIgnoreCase(schedule));
    }
}