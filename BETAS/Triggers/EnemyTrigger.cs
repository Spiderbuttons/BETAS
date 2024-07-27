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
    static class EnemyTrigger
    {
        public static void Trigger_EnemyKilled(Monster mon, GameLocation loc, List<Debris> drops)
        {
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_CropHarvested");
        }
        
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.monsterDrop))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);


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