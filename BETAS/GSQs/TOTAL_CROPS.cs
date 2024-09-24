using System.Data;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class TotalCrops
{
    // Checks how many grown crops there are in all the locations a farmer has visited.
    [GSQ("TOTAL_CROPS")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizableInt(query, 1, out var min, out var error) || 
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 2, out var max, out error, int.MaxValue) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(query, 3, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        if (max == -1) max = int.MaxValue;
        var cropCount = 0;

        Utility.ForEachCrop((crop) =>
        {
            if (Game1.getAllFarmers().All(farmer => !farmer.locationsVisited.Contains(crop.currentLocation.Name)))
            {
                return true;
            }
            var harvestable = crop.currentPhase.Value >= crop.phaseDays.Count - 1 && (!crop.fullyGrown.Value || crop.dayOfCurrentPhase.Value <= 0);
            if (!harvestable) return true;
            if (!ArgUtility.HasIndex(query, 3))
            {
                cropCount++;
            } else if (GameStateQuery.Helpers.AnyArgMatches(query, 3, (cropID) =>
                       {
                           if (crop.indexOfHarvest.Value is null) return ItemRegistry.QualifyItemId(crop.netSeedIndex.Value).Equals(ItemRegistry.QualifyItemId(cropID));
                           return ItemRegistry.QualifyItemId(crop.indexOfHarvest.Value)
                               .Equals(ItemRegistry.QualifyItemId(cropID));
                       }))
            {
                cropCount++;
            }
            return true;
        });
        
        return cropCount >= min && cropCount <= max;
    }
}