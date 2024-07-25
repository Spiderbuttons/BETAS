using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Patches
{
    [HarmonyPatch]
    static class experiencePatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.gainExperience))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            
            try
            {
                var matcher = new CodeMatcher(code, il);
                
                matcher.MatchEndForward(
                        new CodeMatch(OpCodes.Ldloc_0),
                        new CodeMatch(OpCodes.Ldloc_1),
                        new CodeMatch(OpCodes.Ble_S)
                        ).ThrowIfNotMatch("Could not find proper entry point for Farmer_gainExperience_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Sub),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BETAS), nameof(BETAS.Trigger_ExperienceGained))),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldloc_1)
                );
                
                Log.ILCode(matcher.InstructionEnumeration(), code);

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.Experience_Farmer_gainExperience_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}