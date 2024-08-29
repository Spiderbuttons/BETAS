using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class ExperienceGained
    {
        // Raised whenever the player gains experience. ItemId is the name of the skill, ItemStack is the amount of experience gained, and ItemQuality is whether the experience gain resulted in a level up (0 if not, 1 if it did).
        // SpaceCore custom skills are not supported. Maybe eventually!
        public static void Trigger(int levelUp, int skillID, int howMuch)
        {
            var skill = skillID switch
            {
                0 => "Farming",
                1 => "Fishing",
                2 => "Foraging",
                3 => "Mining",
                4 => "Combat",
                _ => null
            };
            var skillItem = ItemRegistry.Create(skill);
            skillItem.modData["BETAS/ExperienceGained/Amount"] = $"{howMuch}";
            skillItem.modData["BETAS/ExperienceGained/WasLevelUp"] = levelUp == 1 ? "true" : "false";
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ExperienceGained", targetItem: skillItem);
        }

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
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ExperienceGained), nameof(Trigger))),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldloc_1)
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ExperienceGained_Farmer_gainExperience_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}