using System.Collections.Generic;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcNearArea
{
    // Check whether a given NPC is currently within a specific radius of the player.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var locationName, out var error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 2, out var x, out error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 3, out var y, out error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(query, 4, out var radius, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(query, 5, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var target = Game1.getLocationFromName(locationName);
        if (target == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no location found with name '" + locationName + "'");
        }

        var targetPosition =
            new Point(x * Game1.tileSize + Game1.tileSize / 2, y * Game1.tileSize + Game1.tileSize / 2);
        
        Rectangle rect = new Rectangle(targetPosition.X - radius * 64, targetPosition.Y - radius * 64,
            (radius * 2 + 1) * 64, (radius * 2 + 1) * 64);


        if (target.Name == Game1.player.currentLocation.Name || Context.IsMainPlayer ||
            BETAS.Cache is null || !BETAS.Cache.L1Cache.Any())
        {
            if (!ArgUtility.HasIndex(query, 5))
            {
                return target.characters.Any(i => rect.Contains(Utility.Vector2ToPoint(i.Position)));
            }
            return GameStateQuery.Helpers.AnyArgMatches(query, 5, (rawName) =>
            {
                return target.characters.Any(i =>
                    i.Name.Equals(rawName) && rect.Contains(Utility.Vector2ToPoint(i.Position)));
            });
        }
        
        Dictionary<string, Vector2> npcPositionsFromCache = [];
        foreach (var npc in BETAS.Cache.L1Cache.Values.Where(npc => npc.LocationName == target.Name))
        {
            npcPositionsFromCache.Add(npc.NpcName, npc.Position);
        }
        
        if (!ArgUtility.HasIndex(query, 5))
        {
            return npcPositionsFromCache.Any(i => rect.Contains(Utility.Vector2ToPoint(i.Value)));
        }
        
        return GameStateQuery.Helpers.AnyArgMatches(query, 5, (rawName) =>
        {
            return npcPositionsFromCache.Any(i => i.Key.Equals(rawName) && rect.Contains(Utility.Vector2ToPoint(i.Value)));
        });
    }
}