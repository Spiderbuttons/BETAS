﻿using System;
using System.Linq;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcNearPlayer
{
    // Check whether a given NPC is currently within a specific radius of the player.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var playerKey, out var error) || !ArgUtility.TryGetInt(query, 2, out var radius, out error) || !ArgUtility.TryGetOptional(query, 3, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            var playerPosition = Utility.Vector2ToPoint(target.Position);
            Rectangle rect = new Rectangle(playerPosition.X - radius * 64, playerPosition.Y - radius * 64, (radius * 2 + 1) * 64, (radius * 2 + 1) * 64);
            Log.Debug(query.Length);
            if (!ArgUtility.HasIndex(query, 3))
            {
                return target.currentLocation.characters.Any(i => rect.Contains(Utility.Vector2ToPoint(i.Position)));
            }
            return GameStateQuery.Helpers.AnyArgMatches(query, 3, (rawName) =>
            {
                Log.Debug(rawName);
                return target.currentLocation.characters.Any(i => i.Name.Equals(rawName) && rect.Contains(Utility.Vector2ToPoint(i.Position)));
            });
        });
    }
}