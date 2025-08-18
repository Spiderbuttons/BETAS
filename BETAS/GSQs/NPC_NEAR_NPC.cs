using System.Collections.Generic;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcNearNpc
{
    // Check whether a given NPC is currently within a specific radius of another NPC
    [GSQ("NPC_NEAR_NPC")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var npcName, out var error, name: "string Target NPC Name") ||
            !TokenizableArgUtility.TryGetInt(query, 2, out var radius, out error, name: "int #Radius") ||
            !TokenizableArgUtility.TryGetOptional(query, 3, out _, out error, name: "string Other NPC Name"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no NPC found with name '" + npcName + "'");
        }

        Rectangle rect = new Rectangle(npc.CachedTilePoint().X - radius, npc.CachedTilePoint().Y - radius,
            radius * 2 + 1, radius * 2 + 1);

        if (!ArgUtility.HasIndex(query, 3))
        {
            return npc.CachedLocation().CachedCharacters().Any(i => i.Name != npc.Name && rect.Contains(i.CachedTilePoint()));
        }
        
        return TokenizableArgUtility.AnyArgMatches(query, 3, (rawName) =>
        {
            return npc.CachedLocation().CachedCharacters().Any(i => i.Name.Equals(rawName) && rect.Contains(i.CachedTilePoint()));
        });
    }
}