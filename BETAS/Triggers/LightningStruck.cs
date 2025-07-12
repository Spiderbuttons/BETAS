using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    static class LightningStruck
    {
        public static void Trigger(Farm.LightningStrikeEvent strike, bool hitRod, TerrainFeature? hitFeature)
        {
            var strikeItem = ItemRegistry.Create("Lightning Strike");
            strikeItem.modData["BETAS/LightningStruck/Size"] = strike is { bigFlash: true } ? "Big" : "Small";
            strikeItem.modData["BETAS/LightningStruck/WasLightningRod"] = hitRod ? "true" : "false";
            if (hitFeature is not null)
                strikeItem.modData["BETAS/LightningStruck/StruckTerrainFeature"] = hitFeature switch
                {
                    FruitTree => "FruitTree", HoeDirt => "Crop", _ => hitFeature.GetType().Name
                };
            switch (hitFeature)
            {
                case HoeDirt { crop: not null } dirt:
                    strikeItem.modData["BETAS/LightningStruck/StruckCrop"] =
                        ItemRegistry.QualifyItemId(dirt.crop.indexOfHarvest.Value);
                    break;
                case FruitTree tree:
                    strikeItem.modData["BETAS/LightningStruck/StruckTree"] =
                        ItemRegistry.QualifyItemId(tree.treeId.Value);
                    break;
            }

            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_LightningStruck", targetItem: strikeItem);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Utility), nameof(Utility.performLightningUpdate))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldfld,
                        AccessTools.Field(typeof(GameLocation), nameof(GameLocation.terrainFeatures))),
                    new CodeMatch(OpCodes.Ldloca_S),
                    new CodeMatch(OpCodes.Ldloca_S),
                    new CodeMatch(OpCodes.Ldnull)
                ).ThrowIfNotMatch(
                    "Could not find TerrainFeature LocalBuilder for Utility_performLightningUpdate_Transpiler");

                var terrainFeatureLocal = matcher.InstructionAt(-1).operand;

                matcher.Start();

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(m =>
                        m.opcode == OpCodes.Ldfld && m.operand is not null &&
                        m.operand.ToString()!.Contains("lightningStrikeEvent")),
                    new CodeMatch(m => m.opcode == OpCodes.Ldloc_S || m.opcode == OpCodes.Ldloc_2),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Ret)
                ).ThrowIfNotMatch("Could not find proper entry point #1 for Utility_performLightningUpdate_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Ldloc_S, terrainFeatureLocal),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LightningStruck), nameof(Trigger)))
                );

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(m =>
                        m.opcode == OpCodes.Ldfld && m.operand is not null &&
                        m.operand.ToString()!.Contains("lightningStrikeEvent")),
                    new CodeMatch(m => m.opcode == OpCodes.Ldloc_S || m.opcode == OpCodes.Ldloc_2),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Ret)
                ).Repeat((codeMatcher) =>
                {
                    codeMatcher.Insert(
                        new CodeInstruction(OpCodes.Ldloc_2),
                        new CodeInstruction(OpCodes.Ldc_I4_0),
                        new CodeInstruction(OpCodes.Ldloc_S, terrainFeatureLocal),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LightningStruck), nameof(Trigger)))
                    );
                });

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.LightningStruck_Utility_performLightningUpdate_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}