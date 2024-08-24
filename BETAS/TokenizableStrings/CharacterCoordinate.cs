using System;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKCharacterCoordinate
{
    /// <summary>The X or Y coordinate of an NPC, given their internal name.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    public static bool Parse(string[] query, out string replacement, Random random, Farmer player)
    {
        if (!ArgUtility.TryGet(query, 1, out var characterName, out var error) || !ArgUtility.TryGet(query, 2, out var axis, out error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        if (characterName.Equals("Farmer", StringComparison.OrdinalIgnoreCase))
        {
            if (axis.Equals("X", StringComparison.OrdinalIgnoreCase))
            {
                replacement = Game1.player.TilePoint.X.ToString();
            }
            else if (axis.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                replacement = Game1.player.TilePoint.Y.ToString();
            } else {
                return TokenParser.LogTokenError(query, "invalid axis '" + axis + "'", out replacement);
            }

            return true;
        }

        Point? tilePoint = ArgUtilityExtensions.GetCharacterTilePointFromNameOrCache(characterName);
        if (tilePoint is null)
        {
            return TokenParser.LogTokenError(query, "no coordinates found for character with name '" + characterName + "'", out replacement);
        }
            
        if (axis.Equals("X", StringComparison.OrdinalIgnoreCase))
        {
            replacement = tilePoint.Value.X.ToString();
        }
        else if (axis.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            replacement = tilePoint.Value.Y.ToString();
        } else {
            return TokenParser.LogTokenError(query, "invalid axis '" + axis + "'", out replacement);
        }
        return true;
    }
}