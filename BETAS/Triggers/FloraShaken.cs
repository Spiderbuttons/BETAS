using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
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
                treeItem.modData["BETAS/FloraShaken/Seed"] = $"{__instance.GetData()?.SeedItemId ?? "Unknown"}";
                treeItem.modData["BETAS/FloraShaken/WasInSeason"] = $"{__instance.IsInSeason()}";
                treeItem.modData["BETAS/FloraShaken/WasMossy"] = $"{__instance.hasMoss.Value}";
                treeItem.modData["BETAS/FloraShaken/WasSeedy"] = $"{__instance.hasSeed.Value}";
                treeItem.modData["BETAS/FloraShaken/WasFertilized"] = $"{__instance.fertilized.Value}";
                treeItem.modData["BETAS/FloraShaken/WasTapped"] = $"{__instance.tapped.Value}";
                treeItem.modData["BETAS/FloraShaken/WasTree"] = "true";
                treeItem.modData["BETAS/FloraShaken/WasFruitTree"] = "false";
                treeItem.modData["BETAS/FloraShaken/WasBush"] = "false";

                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_FloraShaken", targetItem: treeItem,
                    location: __instance.Location);
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
                if (__instance.struckByLightningCountdown.Value > 0) fruits.Add("(O)382");
                var possibleFruits = __instance.GetData().Fruit
                    .Select(fruit => ItemRegistry.QualifyItemId(fruit.ItemId)).ToList();

                treeItem.modData["BETAS/FloraShaken/Stage"] = $"{__instance.growthStage.Value}";
                treeItem.modData["BETAS/FloraShaken/Seed"] = $"{ItemRegistry.QualifyItemId(__instance.treeId.Value)}";
                treeItem.modData["BETAS/FloraShaken/Quality"] = $"{__instance.GetQuality()}";
                if (fruits.Count != 0) treeItem.modData["BETAS/FloraShaken/Produce"] = $"{string.Join(",", fruits)}";
                treeItem.modData["BETAS/FloraShaken/ProduceCount"] = $"{__instance.fruit.Count}";
                treeItem.modData["BETAS/FloraShaken/PossibleProduce"] = $"{string.Join(",", possibleFruits)}";
                treeItem.modData["BETAS/FloraShaken/WasInSeason"] = $"{__instance.IsInSeasonHere()}";
                treeItem.modData["BETAS/FloraShaken/WasMossy"] = "false";
                treeItem.modData["BETAS/FloraShaken/WasSeedy"] = "false";
                treeItem.modData["BETAS/FloraShaken/WasFertilized"] = "false";
                treeItem.modData["BETAS/FloraShaken/WasTapped"] = "false";
                treeItem.modData["BETAS/FloraShaken/WasTree"] = "true";
                treeItem.modData["BETAS/FloraShaken/WasFruitTree"] = "true";
                treeItem.modData["BETAS/FloraShaken/WasBush"] = "false";

                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_FloraShaken", targetItem: treeItem,
                    location: __instance.Location);
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

                if (!__instance.townBush.Value && __instance.tileSheetOffset.Value == 1 && __instance.inBloom() && __instance.GetShakeOffItem() is { } itemId)
                {
                    var item = ItemRegistry.Create(itemId);
                    if (item is not null)
                    {
                        bushItem.ItemId = $"{item.Name} Bush";
                        bushItem.modData["BETAS/FloraShaken/Produce"] = $"{item.QualifiedItemId}";
                    }
                }

                bushItem.modData["BETAS/FloraShaken/Size"] = $"{__instance.size.Value}";
                bushItem.modData["BETAS/FloraShaken/WasInSeason"] = $"{__instance.inBloom()}";
                bushItem.modData["BETAS/FloraShaken/WasMossy"] = "false";
                bushItem.modData["BETAS/FloraShaken/WasSeedy"] = "false";
                bushItem.modData["BETAS/FloraShaken/WasFertilized"] = "false";
                bushItem.modData["BETAS/FloraShaken/WasTapped"] = "false";
                bushItem.modData["BETAS/FloraShaken/WasTree"] = "false";
                bushItem.modData["BETAS/FloraShaken/WasFruitTree"] = "false";
                bushItem.modData["BETAS/FloraShaken/WasBush"] = "true";
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_FloraShaken", targetItem: bushItem,
                    location: __instance.Location);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.FloraShaken_Bush_shake_Prefix: \n" + ex);
            }
        }
    }
}