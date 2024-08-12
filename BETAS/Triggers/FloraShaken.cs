using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.FruitTrees;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class FloraShaken
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Tree), nameof(Tree.shake))]
        public static void Tree_shake_Prefix(Tree __instance, bool doEvenIfStillShaking)
        {
            try
            {
                if ((__instance.maxShake != 0f && !doEvenIfStillShaking) || __instance.growthStage.Value < 3 ||
                    __instance.stump.Value) return;
                
                var treeItem = ItemRegistry.Create(__instance.treeType.Value);
                treeItem.modData["BETAS/FloraShaken/Stage"] = $"{__instance.growthStage.Value}";
                treeItem.modData["BETAS/FloraShaken/Seed"] = $"{__instance.GetData().SeedItemId}";
                treeItem.modData["BETAS/FloraShaken/IsInSeason"] = $"{__instance.IsInSeason()}";
                treeItem.modData["BETAS/FloraShaken/IsMossy"] = $"{__instance.hasMoss.Value}";
                treeItem.modData["BETAS/FloraShaken/IsSeedy"] = $"{__instance.hasSeed.Value}";
                treeItem.modData["BETAS/FloraShaken/IsFertilized"] = $"{__instance.fertilized.Value}";
                treeItem.modData["BETAS/FloraShaken/IsTapped"] = $"{__instance.tapped.Value}";
                treeItem.modData["BETAS/FloraShaken/IsTree"] = "true";
                treeItem.modData["BETAS/FloraShaken/IsFruitTree"] = "false";
                treeItem.modData["BETAS/FloraShaken/IsBush"] = "false";
                
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_FloraShaken", targetItem: treeItem, location: __instance.Location);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.FloraShaken_Tree_shake_Prefix: \n" + ex);
            }
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FruitTree), nameof(FruitTree.shake))]
        public static void FruitTree_shake_Prefix(FruitTree __instance, bool doEvenIfStillShaking)
        {
            try
            {
                if ((__instance.maxShake != 0f && !doEvenIfStillShaking) || __instance.growthStage.Value < 3 ||
                    __instance.stump.Value) return;
                
                var treeItem = ItemRegistry.Create(__instance.treeId.Value);

                var fruits = __instance.fruit.Select(fruit => fruit.QualifiedItemId).ToList();
                var possibleFruits = __instance.GetData().Fruit.Select(fruit => ItemRegistry.QualifyItemId(fruit.ItemId)).ToList();

                treeItem.modData["BETAS/FloraShaken/Stage"] = $"{__instance.growthStage.Value}";
                treeItem.modData["BETAS/FloraShaken/Seed"] = $"{ItemRegistry.QualifyItemId(__instance.treeId.Value)}";
                treeItem.modData["BETAS/FloraShaken/Quality"] = $"{__instance.GetQuality()}";
                if (fruits.Count != 0) treeItem.modData["BETAS/FloraShaken/Produce"] = $"{string.Join(",", fruits)}";
                treeItem.modData["BETAS/FloraShaken/ProduceCount"] = $"{__instance.fruit.Count}";
                treeItem.modData["BETAS/FloraShaken/PossibleProduce"] = $"{string.Join(",", possibleFruits)}";
                treeItem.modData["BETAS/FloraShaken/IsInSeason"] = $"{__instance.IsInSeasonHere()}";
                treeItem.modData["BETAS/FloraShaken/IsMossy"] = "false";
                treeItem.modData["BETAS/FloraShaken/IsSeedy"] = "false";
                treeItem.modData["BETAS/FloraShaken/IsFertilized"] = "false";
                treeItem.modData["BETAS/FloraShaken/IsTapped"] = "false";
                treeItem.modData["BETAS/FloraShaken/IsTree"] = "true";
                treeItem.modData["BETAS/FloraShaken/IsFruitTree"] = "true";
                treeItem.modData["BETAS/FloraShaken/IsBush"] = "false";
                
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_FloraShaken", targetItem: treeItem, location: __instance.Location);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.FloraShaken_FruitTree_shake_Prefix: \n" + ex);
            }
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Bush), nameof(Bush.shake))]
        public static void Bush_shake_Prefix(Bush __instance, bool doEvenIfStillShaking, float ___maxShake)
        {
            try
            {
                if (!(___maxShake == 0f || doEvenIfStillShaking)) return;
                
                var bushItem = ItemRegistry.Create("Bush");

                if (!__instance.townBush.Value && __instance.tileSheetOffset.Value == 1 && __instance.inBloom())
                {
                    var item = ItemRegistry.Create(__instance.GetShakeOffItem());
                    if (item is not null)
                    {
                        bushItem.ItemId = $"{item.Name} Bush";
                        bushItem.modData["BETAS/FloraShaken/Produce"] = $"{item.QualifiedItemId}";
                    }
                }

                bushItem.modData["BETAS/FloraShaken/Size"] = $"{__instance.size.Value}";
                bushItem.modData["BETAS/FloraShaken/IsInSeason"] = $"{__instance.inBloom()}";
                bushItem.modData["BETAS/FloraShaken/IsMossy"] = "false";
                bushItem.modData["BETAS/FloraShaken/IsSeedy"] = "false";
                bushItem.modData["BETAS/FloraShaken/IsFertilized"] = "false";
                bushItem.modData["BETAS/FloraShaken/IsTapped"] = "false";
                bushItem.modData["BETAS/FloraShaken/IsTree"] = "false";
                bushItem.modData["BETAS/FloraShaken/IsFruitTree"] = "false";
                bushItem.modData["BETAS/FloraShaken/IsBush"] = "true";
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_FloraShaken", targetItem: bushItem, location: __instance.Location);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.FloraShaken_Bush_shake_Prefix: \n" + ex);
            }
        }
    }
}