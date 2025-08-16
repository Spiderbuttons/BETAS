using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class NpcNearPlayer
{
    // Check whether a given NPC is currently within a specific radius of the player.
    [GSQ("NPC_NEAR_PLAYER")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGetInt(query, 2, out var radius, out error, name: "int Radius") ||
            !TokenizableArgUtility.TryGetOptional(query, 3, out _, out error, name: "string NPC"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, (farmer) =>
        {
            var playerTile = farmer.TilePoint;
            Rectangle rect = new Rectangle(playerTile.X, playerTile.Y, 1, 1);
            rect.Inflate(radius, radius);

            if (!ArgUtility.HasIndex(query, 3))
            {
                return farmer.currentLocation.characters.Any(i => rect.Contains(i.TilePoint));
            }

            return TokenizableArgUtility.AnyArgMatches(query, 3,
                (rawName) =>
                {
                    return farmer.currentLocation.characters.Any(i =>
                        i.Name.Equals(rawName) && rect.Contains(i.TilePoint));
                });
        });
    }
}