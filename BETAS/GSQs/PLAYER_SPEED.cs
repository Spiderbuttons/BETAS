using System;
using System.Linq;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class PlayerSpeed
{
    // GSQ for checking whether a players speed is between a minimum and optional maximum value.
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtility.TryGet(query, 1, out var playerKey, out var error) || !ArgUtility.TryGetFloat(query, 2, out var minSpeed, out error) || !ArgUtility.TryGetOptionalFloat(query, 3, out var maxSpeed, out error, float.MaxValue))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            var speed = target.getMovementSpeed();
            Log.Debug(speed);
            return speed >= minSpeed && speed <= maxSpeed;
        });
    }
}