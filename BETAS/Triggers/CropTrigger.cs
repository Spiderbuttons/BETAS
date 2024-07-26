using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class CropTrigger
    {
        // Raised whenever the player gains experience. ItemId is the name of the skill, ItemStack is the amount of experience gained, and ItemQuality is whether the experience gain resulted in a level up (0 if not, 1 if it did).
        // SpaceCore custom skills are not supported. Maybe eventually!
        public static void Trigger_CropHarvested(Item crop, JunimoHarvester junimo = null, int numToHarvest = 1)
        {
            Log.Debug($"Crop harvested: {crop.Name}, junimo exists: {junimo is not null}, numToHarvest: {numToHarvest}");
            crop.modData["BETAS/ExperienceGained/IsHarvestedByJunimo"] = $"{junimo is not null}";
            Log.Debug(crop.Stack);
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_CropHarvested", targetItem: crop);
            var x = new Crop();
        }
        
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Crop), nameof(Crop.harvest))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);
                
                // Need to find the numToHarvest localBuilder. The crop, if it's a forageCrop, is just Ldloc.1 which is nice.
                //  Edit from future me: Apparently forageCrop just never happens? I wasted half my time here? Oh well...
                matcher.MatchEndForward(
                    new CodeMatch(static i => i.opcode == OpCodes.Call && i.operand.ToString()!.Contains("Clamp")),
                    new CodeMatch(OpCodes.Stloc_S),
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Stloc_S)
                ).ThrowIfNotMatch("Could not find proper entry point #1 for Crop_harvest_Transpiler");

                var numToHarvestLocal = (LocalBuilder)matcher.Operand;
                matcher.Start();

                matcher.MatchStartForward(
                        new CodeMatch(OpCodes.Ldarg_S),
                        new CodeMatch(OpCodes.Ldloc_1),
                        new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(JunimoHarvester), nameof(JunimoHarvester.tryToAddItemToHut))),
                        new CodeMatch(OpCodes.Ldc_I4_1),
                        new CodeMatch(OpCodes.Ret)
                        ).ThrowIfNotMatch("Could not find proper entry point #2 for Crop_harvest_Transpiler");
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropTrigger), nameof(Trigger_CropHarvested)))
                );
                
                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Farmer), nameof(Farmer.gainExperience))),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Ldstr, "moss_cut"),
                    new CodeMatch(OpCodes.Ldloca_S),
                    new CodeMatch(OpCodes.Initobj)
                    ).ThrowIfNotMatch("Could not find proper entry point #3 for Crop_harvest_Transpiler");
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropTrigger), nameof(Trigger_CropHarvested)))
                );
                
                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Call, AccessTools.PropertyGetter(typeof(Game1), nameof(Game1.player))),
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_0),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Farmer), nameof(Farmer.addItemToInventoryBool))),
                    new CodeMatch(OpCodes.Brfalse),
                    new CodeMatch(OpCodes.Ldloca_S)
                ).ThrowIfNotMatch("Could not find proper entry point #4 for Crop_harvest_Transpiler");
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropTrigger), nameof(Trigger_CropHarvested)))
                );
                
                // Now we need the harvestItem LocalBuilder if it's not a forageCrop.
                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Item), nameof(Item.getOne))),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(JunimoHarvester), nameof(JunimoHarvester.tryToAddItemToHut))),
                    new CodeMatch(OpCodes.Br_S)
                ).ThrowIfNotMatch("Could not find proper entry point #5 for Crop_harvest_Transpiler");
                
                var harvestItem = (LocalBuilder)matcher.Operand;
                
                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Stloc_0),
                    new CodeMatch(OpCodes.Br)
                ).ThrowIfNotMatch("Could not find proper entry point #6 for Crop_harvest_Transpiler");
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_S, harvestItem.LocalIndex),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropTrigger), nameof(Trigger_CropHarvested)))
                );
                
                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Stloc_0),
                    new CodeMatch(OpCodes.Br_S)
                ).ThrowIfNotMatch("Could not find proper entry point #7 for Crop_harvest_Transpiler");
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_S, harvestItem.LocalIndex),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropTrigger), nameof(Trigger_CropHarvested)))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ExperienceTrigger_Farmer_gainExperience_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}