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
    static class CropHarvested
    {
        public static void Trigger(Item crop, GameLocation loc, JunimoHarvester junimo = null, int numToHarvest = 1)
        {
            crop.modData["BETAS/CropHarvested/WasHarvestedByJunimo"] = $"{junimo is not null}";
            crop.Stack = numToHarvest;
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_CropHarvested", targetItem: crop, location: loc);
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
                // Edit from future me: Apparently forageCrop just never happens? I wasted half my time here? Oh well...
                // Edit from even more future me: Apparently it's for spring onions. I really don't care about those...
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
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Crop), nameof(Crop.currentLocation))),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropHarvested), nameof(Trigger)))
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
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Crop), nameof(Crop.currentLocation))),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropHarvested), nameof(Trigger)))
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
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Crop), nameof(Crop.currentLocation))),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropHarvested), nameof(Trigger)))
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
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Crop), nameof(Crop.currentLocation))),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropHarvested), nameof(Trigger)))
                );
                
                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Stloc_0),
                    new CodeMatch(OpCodes.Br_S)
                ).ThrowIfNotMatch("Could not find proper entry point #7 for Crop_harvest_Transpiler");
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_S, harvestItem.LocalIndex),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Crop), nameof(Crop.currentLocation))),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, numToHarvestLocal.LocalIndex),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CropHarvested), nameof(Trigger)))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.CropHarvested_Crop_harvest_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}