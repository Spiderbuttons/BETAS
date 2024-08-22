using System;
using System.Collections.Generic;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;

namespace BETAS;

public class MultiplayerNpcCache
{
    public class NpcCacheData(string npcName, string locationName, Point tilePoint, Vector2 position)
    {
        public readonly string NpcName = npcName;
        public string LocationName = locationName;
        public Point TilePoint = tilePoint;
        public Vector2 Position = position;
        public int skippedFrames = 0;
        
        public bool HasChanged(NPC npc)
        {
            return !LocationName.Equals(npc.currentLocation.Name) || !TilePoint.Equals(npc.TilePoint) || !Position.Equals(npc.Position);
        }

        public void Update(string locationName, Point tilePoint, Vector2 position)
        {
            LocationName = locationName;
            TilePoint = tilePoint;
            Position = position;
        }
        
        public void CopyFrom(NpcCacheData other)
        {
            LocationName = other.LocationName;
            TilePoint = other.TilePoint;
            Position = other.Position;
        }
    }

    public Dictionary<string, NpcCacheData> L1Cache = new();
    public Dictionary<string, NpcCacheData> L2Cache = new();
    public Dictionary<string, NpcCacheData> L3Cache = new();

    public void UpdateCharacterList()
    {
        Utility.ForEachCharacter(delegate(NPC rawNpc)
        {
            if (L1Cache.TryGetValue(rawNpc.Name, out var L1))
            {
                L1.skippedFrames = 0;
            }
            else if (L2Cache.TryGetValue(rawNpc.Name, out var L2))
            {
                L2.skippedFrames = 0;
            }
            else if (L3Cache.TryGetValue(rawNpc.Name, out var L3))
            {
                L3.skippedFrames = 0;
            }
            else
            {
                L1Cache.Add(rawNpc.Name, new NpcCacheData(rawNpc.Name, rawNpc.currentLocation.Name,rawNpc.TilePoint, rawNpc.Position));
            }

            return true;
        });
    }

    public List<NpcCacheData> CheckL1Cache()
    {
        List<NpcCacheData> toBroadcast = [];
        foreach (var (name, data) in L1Cache)
        {
            var npc = Game1.getCharacterFromName(name);
            if (npc == null)
            {
                L1Cache.Remove(name);
                continue;
            }

            if (data.HasChanged(npc))
            {
                data.skippedFrames = 0;
                data.Update(npc.currentLocation.Name, npc.TilePoint, npc.Position);
                toBroadcast.Add(data);
            } 
            else
            {
                data.skippedFrames++;
                if (data.skippedFrames < 3) continue;
                L1Cache.Remove(name);
                L2Cache.Add(name, data);
            }
        }
        
        return toBroadcast;
    }
    
    public List<NpcCacheData> CheckL2Cache()
    {
        List<NpcCacheData> toBroadcast = [];
        foreach (var (name, data) in L2Cache)
        {
            var npc = Game1.getCharacterFromName(name);
            if (npc == null)
            {
                L2Cache.Remove(name);
                continue;
            }

            if (data.HasChanged(npc))
            {
                data.skippedFrames = 0;
                data.Update(npc.currentLocation.Name, npc.TilePoint, npc.Position);
                
                L2Cache.Remove(name);
                L1Cache.Add(name, data);
                toBroadcast.Add(data);
            } 
            else
            {
                data.skippedFrames++;
                if (data.skippedFrames < 3) continue;
                L2Cache.Remove(name);
                L3Cache.Add(name, data);
            }
        }
        
        return toBroadcast;
    }
    
    public List<NpcCacheData> CheckL3Cache()
    {
        List<NpcCacheData> toBroadcast = [];
        foreach (var (name, data) in L3Cache)
        {
            var npc = Game1.getCharacterFromName(name);
            if (npc == null)
            {
                L3Cache.Remove(name);
                continue;
            }

            if (data.HasChanged(npc))
            {
                data.skippedFrames = 0;
                data.Update(npc.currentLocation.Name, npc.TilePoint, npc.Position);
                
                L3Cache.Remove(name);
                L1Cache.Add(name, data);
                toBroadcast.Add(data);
            } 
            else
            {
                data.skippedFrames++;
            }
        }
        
        return toBroadcast;
    }
}