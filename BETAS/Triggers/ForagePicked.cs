using System;
using System.Collections.Generic;
using System.Linq;
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
    static class ForagePicked
    {
        public static void Trigger(Item forage, GameLocation loc)
        {
            var newItem = ItemRegistry.Create(forage.QualifiedItemId, forage.Stack, forage.Quality);
            
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ForagePicked", targetItem: newItem, inputItem: newItem, location: loc);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.OnHarvestedForage))]
        public static void OnHarvestedForage_Postfix(GameLocation __instance, Farmer who, StardewValley.Object forage)
        {
            try
            {
                Trigger(forage, __instance);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ForagePicked_OnHarvestedForage_Postfix: \n" + ex);
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Crop), nameof(Crop.harvest))]
        [HarmonyPriority(Priority.First)] // This is so BETAS doesn't conflict with BETAS.
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Call, AccessTools.PropertyGetter(typeof(Game1), nameof(Game1.player))),
                    new CodeMatch(OpCodes.Ldc_I4_2),
                    new CodeMatch(OpCodes.Ldloc_2),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Farmer), nameof(Farmer.gainExperience)))
                ).Repeat(codeMatcher =>
                {
                    codeMatcher.InsertAndAdvance(
                        new CodeInstruction(OpCodes.Ldloc_1),
                        new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Game1), nameof(Game1.player))),
                        new CodeInstruction(OpCodes.Callvirt,
                            AccessTools.PropertyGetter(typeof(Farmer), nameof(Farmer.currentLocation))),
                        new CodeInstruction(OpCodes.Call,
                            AccessTools.Method(typeof(ForagePicked), nameof(Trigger)))
                    );
                    codeMatcher.Advance(4);
                });

                Log.ILCode(matcher.InstructionEnumeration(), code);

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ForagePicked_Crop_harvest_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}