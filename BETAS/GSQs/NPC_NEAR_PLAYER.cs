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
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !TokenizableArgUtility.TryGetTokenizableInt(query, 2, out var radius, out error) ||
            !TokenizableArgUtility.TryGetOptionalTokenizable(query, 3, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            var playerPosition = Utility.Vector2ToPoint(target.Position);
            Rectangle rect = new Rectangle(playerPosition.X - radius * 64, playerPosition.Y - radius * 64,
                (radius * 2 + 1) * 64, (radius * 2 + 1) * 64);
            if (!ArgUtility.HasIndex(query, 3))
            {
                return target.currentLocation.characters.Any(i => rect.Contains(Utility.Vector2ToPoint(i.Position)));
            }

            return TokenizableArgUtility.AnyArgMatches(query, 3,
                (rawName) =>
                {
                    return target.currentLocation.characters.Any(i =>
                        i.Name.Equals(rawName) && rect.Contains(Utility.Vector2ToPoint(i.Position)));
                });
        });
    }
}