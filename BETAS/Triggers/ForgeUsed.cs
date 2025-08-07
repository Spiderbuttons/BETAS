using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Toolkit.Utilities;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class ForgeUsed
    {
        public static void Trigger(Item? target, Item? input = null, bool unforge = false)
        {
            if (target is null) return;
            
            if (target is CombinedRing comboRing)
            {
                target = comboRing.combinedRings[0];
                input = comboRing.combinedRings[1];
            }
            
            target.modData["BETAS/ForgeUsed/WasUnforge"] = unforge.ToString();
            if (input is not null) target.modData["BETAS/ForgeUsed/WasUnforge"] = unforge.ToString();
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ForgeUsed", targetItem: target, inputItem: input);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ForgeMenu), nameof(ForgeMenu.CraftItem))]
        public static void CraftItem_Postfix(Item? __result, Item left_item, Item right_item, bool forReal)
        {
            try
            {
                if (__result is null || !forReal) return;
                Trigger(__result, right_item);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ForgeUsed_ForgeMenu_CraftItem_Postfix: \n" + ex);
            }
        }

        public static void RingGrabber(List<Ring> rings)
        {
            Trigger(rings[0], rings[1], true);
        }
        
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ForgeMenu), nameof(ForgeMenu.update))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            if (Constants.Platform is Platform.Android) return ForgeMenu_update_AndroidTranspiler(instructions, il);
            
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Call,
                        AccessTools.PropertySetter(typeof(MenuWithInventory), nameof(MenuWithInventory.heldItem)))
                ).ThrowIfNotMatch("Could not find proper entry point #1 for ForgeMenu_update_Transpiler");

                matcher.Advance(1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.PropertyGetter(typeof(MenuWithInventory), nameof(MenuWithInventory.heldItem))),
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ForgeUsed), nameof(Trigger)))
                );
                
                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(CombinedRing), nameof(CombinedRing.combinedRings))),
                    new CodeMatch(OpCodes.Newobj),
                    new CodeMatch(OpCodes.Stloc_S)
                ).ThrowIfNotMatch("Could not find proper entry point #2 for ForgeMenu_update_Transpiler");
                
                var listLocal = matcher.Instruction.operand;
                
                matcher.Advance(1);
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_S, listLocal),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ForgeUsed), nameof(RingGrabber)))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ForgeUsed_ForgeMenu_update_Transpiler: \n" + ex);
                return code;
            }
        }

        private static IEnumerable<CodeInstruction> ForgeMenu_update_AndroidTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Call,
                        AccessTools.Method(typeof(Utility), "CollectOrDrop", [typeof(Item)])),
                    new CodeMatch(OpCodes.Pop),
                    new CodeMatch(OpCodes.Ldstr, "(O)848")
                ).ThrowIfNotMatch("Could not find proper entry point #1 for ForgeMenu_update_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld,
                        AccessTools.Field(typeof(MenuWithInventory), "heldItem")),
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ForgeUsed), nameof(Trigger)))
                );
                
                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(CombinedRing), nameof(CombinedRing.combinedRings))),
                    new CodeMatch(OpCodes.Newobj),
                    new CodeMatch(OpCodes.Stloc_S)
                ).ThrowIfNotMatch("Could not find proper entry point #2 for ForgeMenu_update_Transpiler");
                
                var listLocal = matcher.Instruction.operand;
                
                matcher.Advance(1);
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_S, listLocal),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ForgeUsed), nameof(RingGrabber)))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ForgeUsed_ForgeMenu_update_AndroidTranspiler: \n" + ex);
                return code;
            }
        }
    }
}