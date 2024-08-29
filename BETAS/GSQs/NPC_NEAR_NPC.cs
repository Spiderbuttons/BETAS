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
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var npcName, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out var radius, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(query, 3, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no NPC found with name '" + npcName + "'");
        }

        Point npcPosition;
        string npcLocation;

        if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            !(BETAS.Cache is not null && BETAS.Cache.L1Cache.TryGetValue(npc.Name, out var cacheNpc)))
        {
            npcPosition = Utility.Vector2ToPoint(npc.Position);
            npcLocation = npc.currentLocation.Name;
        }
        else
        {
            npcPosition = Utility.Vector2ToPoint(cacheNpc.Position);
            npcLocation = cacheNpc.LocationName;
        }

        Rectangle rect = new Rectangle(npcPosition.X - radius * 64, npcPosition.Y - radius * 64,
            (radius * 2 + 1) * 64, (radius * 2 + 1) * 64);
        
        if (npc.currentLocation.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            BETAS.Cache is null || !BETAS.Cache.L1Cache.Any())
        {
            if (!ArgUtility.HasIndex(query, 3))
            {
                return npc.currentLocation.characters.Any(i => i.Name != npc.Name && rect.Contains(Utility.Vector2ToPoint(i.Position)));
            }
            
            return GameStateQuery.Helpers.AnyArgMatches(query, 3, (rawName) =>
            {
                return npc.currentLocation.characters.Any(i =>
                    i.Name.Equals(rawName) && rect.Contains(Utility.Vector2ToPoint(i.Position)));
            });
        }
        
        Dictionary<string, Vector2> npcPositionsFromCache = [];
        foreach (var cachedNpc in BETAS.Cache.L1Cache.Values.Where(cachedNpc => cachedNpc.LocationName == npcLocation))
        {
            npcPositionsFromCache.Add(cachedNpc.NpcName, cachedNpc.Position);
        }
        
        if (!ArgUtility.HasIndex(query, 3))
        {
            return npcPositionsFromCache.Any(i => i.Key != npc.Name && rect.Contains(Utility.Vector2ToPoint(i.Value)));
        }
        
        return GameStateQuery.Helpers.AnyArgMatches(query, 3, (rawName) =>
        {
            return npcPositionsFromCache.Any(i => i.Key.Equals(rawName) && rect.Contains(Utility.Vector2ToPoint(i.Value)));
        });
    }
}