using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcIsSingle
{
    // Check whether a given NPC is currently in a relationship.
    [GSQ("NPC_IS_SINGLE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var _, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.AnyArgMatches(query, 1, (name) =>
            {
                var npc = Game1.getCharacterFromName(name);
                if (npc == null)
                {
                    Log.Warn($"No NPC found with name '{name}'");
                    return false;
                }

                foreach (var farmer in Game1.getAllFarmers())
                {
                    if (!farmer.friendshipData.TryGetValue(npc.Name, out var friendship)) continue;
                    if (friendship.Status is FriendshipStatus.Dating or FriendshipStatus.Engaged or FriendshipStatus.Married)
                    {
                        return false;
                    }
                }

                return true;
            });
    }
}