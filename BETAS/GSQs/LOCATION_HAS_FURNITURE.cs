using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class LocationHasFurniture
{
    // Check whether a given location has any of the given furnitures placed inside of it.
    [GSQ("LOCATION_HAS_FURNITURE")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        GameLocation contextualLocation = context.Location;
        if (!TokenizableArgUtility.TryGetLocationName(query, 1, contextualLocation, out var locationName, out var error) ||
            !TokenizableArgUtility.TryGet(query, 2, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        if (locationName.Equals("Any", StringComparison.OrdinalIgnoreCase))
        {
            var foundFurniture = false;
            Utility.ForEachLocation((gameLocation) =>
            {
                if (TokenizableArgUtility.AnyArgMatches(query, 2,
                        (furnitureID) => gameLocation.furniture.Any(furniture => furniture.QualifiedItemId == furnitureID || furniture.ItemId == furnitureID)))
                {
                    foundFurniture = true;
                }
        
                return true;
            });
            return foundFurniture;
        }
        
        if (locationName.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            var foundFurniture = true;
            Utility.ForEachLocation((gameLocation) =>
            {
                if (!TokenizableArgUtility.AnyArgMatches(query, 2,
                        (furnitureID) => gameLocation.furniture.Any(furniture => furniture.QualifiedItemId == furnitureID || furniture.ItemId == furnitureID)))
                {
                    foundFurniture = false;
                }
        
                return true;
            });
            return foundFurniture;
        }
        
        var location = Game1.getLocationFromName(locationName);
        if (location == null)
        {
            return GameStateQuery.Helpers.ErrorResult(query, "no location found with name '" + locationName + "'");
        }
        
        return TokenizableArgUtility.AnyArgMatches(query, 2,
                (furnitureID) => location.furniture.Any(furniture => furniture.QualifiedItemId == furnitureID || furniture.ItemId == furnitureID));
    }
}