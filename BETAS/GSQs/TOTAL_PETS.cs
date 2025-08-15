using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Delegates;

namespace BETAS.GSQs;

public static class TotalPets
{
    // Checks how many pets there are on the farm.
    [GSQ("TOTAL_PETS")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizableInt(query, 1, out var min, out var error) || 
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(query, 2, out var max, out error, int.MaxValue) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(query, 3, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        if (max == -1) max = int.MaxValue;
        var petCount = 0;

        foreach (var farmer in Game1.getAllFarmers())
        {
            foreach (var character in Utility.getHomeOfFarmer(farmer).characters)
            {
                if (character is not Pet pet) continue;
                if (!ArgUtility.HasIndex(query, 3))
                {
                    petCount++;
                }
                else if (ArgUtilityExtensions.AnyArgMatches(query, 3, (petID) => pet.petType.Value.Equals(petID)))
                {
                    petCount++;
                }
            }
        }

        foreach (var character in Game1.getFarm().characters)
        {
            if (character is not Pet pet) continue;
            if (!ArgUtility.HasIndex(query, 3))
            {
                petCount++;
            }
            else if (ArgUtilityExtensions.AnyArgMatches(query, 3, (petID) => pet.petType.Value.Equals(petID)))
            {
                petCount++;
            }
        }
        
        return petCount >= min && petCount <= max;
    }
}