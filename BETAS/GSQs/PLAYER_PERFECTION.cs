using System;
using System.Collections.Generic;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;
using StardewValley.GameData.Characters;
using StardewValley.GameData.Objects;
using StardewValley.ItemTypeDefinitions;
using Object = StardewValley.Object;

namespace BETAS.GSQs;

public static class PlayerPerfection
{
    // Check the number of (distinct) items shipped.
    [GSQ("PLAYER_PERFECTION_ITEMS_SHIPPED")]
    public static bool Items(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGet(query, 2, out var calculation, out error, name: "string Calculation") ||
            !TokenizableArgUtility.TryGetInt(query, 3, out var min, out error, name: "int #Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 4, out var max, out error, int.MaxValue, name: "int #Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        IEnumerable<ParsedItemData> itemRegData = ItemRegistry.GetObjectTypeDefinition().GetAllData();

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            int farmerShipped = 0;
            int total = 0;
            
            foreach (ParsedItemData data in itemRegData)
            {
                int category = data.Category;
                if (category == -7 || category == -2 || !Object.isPotentialBasicShipped(data.ItemId, data.Category, data.ObjectType)) continue;
                
                total++;
                if (target.basicShipped.ContainsKey(data.ItemId))
                {
                    farmerShipped++;
                }
            }
            
            if (calculation.Equals("Raw", StringComparison.OrdinalIgnoreCase))
            {
                return farmerShipped >= min && farmerShipped <= max;
            }

            var percent = farmerShipped / (float)total * 100;
            return percent >= min && percent <= max;
        });
    }
    
    // Check the number of (distinct) fish caught.
    [GSQ("PLAYER_PERFECTION_FISH_CAUGHT")]
    public static bool Fish(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGet(query, 2, out var calculation, out error, name: "string Calculation") ||
            !TokenizableArgUtility.TryGetInt(query, 3, out var min, out error, name: "int #Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 4, out var max, out error, int.MaxValue, name: "int #Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        IEnumerable<ParsedItemData> itemRegData = ItemRegistry.GetObjectTypeDefinition().GetAllData();

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            int fishCaught = 0;
            int totalFish = 0;
            
            foreach (ParsedItemData data in itemRegData)
            {
                if (data.ObjectType != "Fish" || (data.RawData is ObjectData objData && objData.ExcludeFromFishingCollection)) continue;
                
                totalFish++;
                if (target.fishCaught.ContainsKey(data.QualifiedItemId))
                {
                    fishCaught++;
                }
            }
            
            if (calculation.Equals("Raw", StringComparison.OrdinalIgnoreCase))
            {
                return fishCaught >= min && fishCaught <= max;
            }
            
            var percent = fishCaught / (float)totalFish * 100;
            return percent >= min && percent <= max;
        });
    }
    
    // Check the number of (distinct) recipes cooked.
    [GSQ("PLAYER_PERFECTION_RECIPES_COOKED")]
    public static bool Cooking(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGet(query, 2, out var calculation, out error, name: "string Calculation") ||
            !TokenizableArgUtility.TryGetInt(query, 3, out var min, out error, name: "int #Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 4, out var max, out error, int.MaxValue, name: "int #Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        Dictionary<string, string> recipes = DataLoader.CookingRecipes(Game1.content);

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            int numberOfRecipesCooked = 0;
            
            foreach (var r in recipes)
            {
                string recipeKey = r.Key;
                if (!target.cookingRecipes.ContainsKey(recipeKey)) continue;
                
                string recipe = ArgUtility.SplitBySpaceAndGet(ArgUtility.Get(r.Value.Split('/'), 2), 0);
                if (target.recipesCooked.ContainsKey(recipe))
                {
                    numberOfRecipesCooked++;
                }
            }
            
            if (calculation.Equals("Raw", StringComparison.OrdinalIgnoreCase))
            {
                return numberOfRecipesCooked >= min && numberOfRecipesCooked <= max;
            }
            
            var percent = numberOfRecipesCooked / (float)recipes.Count * 100;
            return percent >= min && percent <= max;
        });
    }
    
    // Check the number of (distinct) recipes crafted (not cooked!).
    [GSQ("PLAYER_PERFECTION_RECIPES_CRAFTED")]
    public static bool Crafting(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGet(query, 2, out var calculation, out error, name: "string Calculation") ||
            !TokenizableArgUtility.TryGetInt(query, 3, out var min, out error, name: "int #Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 4, out var max, out error, int.MaxValue, name: "int #Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        Dictionary<string, string> recipes = DataLoader.CraftingRecipes(Game1.content);

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            int numberOfRecipesCrafted = 0;
            
            foreach (var r in recipes.Keys)
            {
                if (r != "Wedding Ring" && target.craftingRecipes.TryGetValue(r, out var timesCrafted) &&
                    timesCrafted > 0)
                {
                    numberOfRecipesCrafted++;
                }
            }
            
            if (calculation.Equals("Raw", StringComparison.OrdinalIgnoreCase))
            {
                return numberOfRecipesCrafted >= min && numberOfRecipesCrafted <= max;
            }
            
            var percent = numberOfRecipesCrafted / (float)(recipes.Count - 1) * 100;
            return percent >= min && percent <= max;
        });
    }
    
    // Check the number of NPCs the player has maxed friendship with.
    [GSQ("PLAYER_PERFECTION_FRIENDSHIP")]
    public static bool Friendships(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var playerKey, out var error, name: "string Player") ||
            !TokenizableArgUtility.TryGet(query, 2, out var calculation, out error, name: "string Calculation") ||
            !TokenizableArgUtility.TryGetInt(query, 3, out var min, out error, name: "int #Minimum") ||
            !TokenizableArgUtility.TryGetOptionalInt(query, 4, out var max, out error, int.MaxValue, name: "int #Maximum"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, delegate(Farmer target)
        {
            int maxedFriends = 0;
            int totalFriends = 0;
            
            foreach (var pair in Game1.characterData)
            {
                string npcName = pair.Key;
                CharacterData data = pair.Value;
                if (!data.PerfectionScore || GameStateQuery.IsImmutablyFalse(data.CanSocialize)) continue;

                totalFriends++;
                if (!target.friendshipData.TryGetValue(npcName, out var friendship)) continue;
                
                int maxPoints = (data.CanBeRomanced ? 8 : 10) * 250;
                if (friendship != null && friendship.Points >= maxPoints)
                {
                    maxedFriends++;
                }
            }
            
            if (calculation.Equals("Raw", StringComparison.OrdinalIgnoreCase))
            {
                return maxedFriends >= min && maxedFriends <= max;
            }
            
            var percent = maxedFriends / (float)totalFriends * 100;
            return percent >= min && percent <= max;
        });
    }
}