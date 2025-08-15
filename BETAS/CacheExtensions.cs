using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;

namespace BETAS;

public static class CacheExtensions
{
    public static Point CachedTilePoint(this NPC npc)
    {
        if (Context.IsMainPlayer || npc.currentLocation.Name == Game1.player.currentLocation.Name || npc.currentLocation.isAlwaysActive.Value ||
            !(BETAS.Cache is not null && BETAS.Cache.TryGetCachedCharacter(npc.Name, out var cache)))
        {
            return npc.TilePoint;
        }

        return cache.TilePoint;
    }
    
    public static Vector2 CachedPosition(this NPC npc)
    {
        if (Context.IsMainPlayer || npc.currentLocation.Name == Game1.player.currentLocation.Name || npc.currentLocation.isAlwaysActive.Value ||
            !(BETAS.Cache is not null && BETAS.Cache.TryGetCachedCharacter(npc.Name, out var cache)))
        {
            return npc.Position;
        }

        return cache.Position;
    }
    
    public static GameLocation CachedLocation(this NPC npc)
    {
        if (Context.IsMainPlayer || npc.currentLocation.Name == Game1.player.currentLocation.Name || npc.currentLocation.isAlwaysActive.Value ||
            !(BETAS.Cache is not null && BETAS.Cache.TryGetCachedCharacter(npc.Name, out var cache)))
        {
            return npc.currentLocation;
        }

        return Game1.getLocationFromName(cache.LocationName) ?? npc.currentLocation;
    }
    
    public static List<NPC> CachedCharacters(this GameLocation location)
    {
        if (Context.IsMainPlayer || location.isAlwaysActive.Value || BETAS.Cache is null)
        {
            return location.characters.ToList();
        }

        return BETAS.Cache.GetAllCachedCharacters().Where(npc => npc.LocationName == location.Name)
            .Select(npc => Game1.getCharacterFromName(npc.NpcName))
            .ToList();
    }
}