using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class RelationshipTrigger
    {
        public static void Trigger_RelationshipChanged(Friendship oldData, Friendship newData, Farmer who)
        {
            Log.Debug($"Old status: {oldData.Status}, New status: {newData.Status}");
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MonsterKilled");
        }
        
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.doDivorce))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_0),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Points))),
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_0)
                ).ThrowIfNotMatch("Could not find proper entry point #1 for Farmer_doDivorce_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1)
                );
                
                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_4),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Status)))
                ).ThrowIfNotMatch("Could not find proper entry point #2 for Farmer_doDivorce_Transpiler");

                matcher.Advance(1);
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RelationshipTrigger), nameof(Trigger_RelationshipChanged)))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.EnemyTrigger_GameLocation_monsterDrop_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}