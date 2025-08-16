using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;
using StardewValley.TerrainFeatures;
using StardewValley.TokenizableStrings;

namespace BETAS.GSQs;

public static class GiantCropGrown
{
    // Check whether any of the given giant crops have been grown and currently exist in the world.
    [GSQ("GIANT_CROP_GROWN")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var cropId, out var error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        // Might as well search the farm first, since it's the most likely place to find giant crops.
        if (Game1.getFarm().resourceClumps.Any(clump => clump is GiantCrop crop && crop.Id.EqualsIgnoreCase(cropId)))
        {
            return true;
        }

        bool found = false;
        Utility.ForEachLocation(location =>
        {
            if (location is Farm) return true;
            if (location.resourceClumps.Any(clump => clump is GiantCrop crop && crop.Id.EqualsIgnoreCase(cropId)))
            {
                found = true;
                return false;
            }

            return true;
        });

        return found;
    }
}