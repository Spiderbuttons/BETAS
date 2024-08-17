using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcNearNpc
{
    // Check whether a given NPC is currently within a specific radius of another Npc
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var npcName, out var error) ||
            !ArgUtility.TryGetInt(query, 2, out var radius, out error) ||
            !ArgUtility.TryGetOptional(query, 3, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no NPC found with name '" + npcName + "'");
        }

        var npcPosition = Utility.Vector2ToPoint(npc.Position);
        Rectangle rect = new Rectangle(npcPosition.X - radius * 64, npcPosition.Y - radius * 64,
            (radius * 2 + 1) * 64, (radius * 2 + 1) * 64);
        if (!ArgUtility.HasIndex(query, 3))
        {
            return npc.currentLocation.characters.Any(i => rect.Contains(Utility.Vector2ToPoint(i.Position)));
        }

        return GameStateQuery.Helpers.AnyArgMatches(query, 3, (rawName) =>
        {
            return npc.currentLocation.characters.Any(i =>
                i.Name.Equals(rawName) && rect.Contains(Utility.Vector2ToPoint(i.Position)));
        });
    }
}