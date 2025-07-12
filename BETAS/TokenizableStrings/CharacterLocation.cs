using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.TokenizableStrings;

namespace BETAS.TokenizableStrings;

public static class TKCharacterLocation
{
    /// <summary>The name of the location an NPC is in, given their internal name.</summary>
    /// <inheritdoc cref="T:StardewValley.TokenizableStrings.TokenParserDelegate" />
    [TKString("CharacterLocation")]
    public static bool Parse(string[] query, out string replacement, Random random, Farmer player)
    {
        if (!ArgUtility.TryGet(query, 1, out var characterName, out var error) || !ArgUtility.TryGetOptionalBool(query, 2, out var displayName, out error))
        {
            return TokenParser.LogTokenError(query, error, out replacement);
        }
        
        if (characterName.Equals("Farmer", StringComparison.OrdinalIgnoreCase))
        {
            replacement = displayName ? Game1.player.currentLocation.DisplayName : Game1.player.currentLocation.Name;
            return true;
        }

        GameLocation? location = ArgUtilityExtensions.GetCharacterLocationFromNameOrCache(characterName);
        if (location == null)
        {
            return TokenParser.LogTokenError(query, "no location found for character with name '" + characterName + "'", out replacement);
        }
            
        replacement = displayName ? location.DisplayName : location.Name;
        return true;
    }
}