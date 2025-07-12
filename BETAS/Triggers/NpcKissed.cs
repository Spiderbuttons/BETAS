using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class NpcKissed
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.GameLaunched += OnGameLaunched;
        }
        
        public static void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            if (BETAS.ModHelper.ModRegistry.IsLoaded("ApryllForever.PolyamorySweetKiss"))
            {
                Log.Trace("Adding PolyamorySweetKiss compatibility patch for NpcKissed...");
                try
                {
                    BETAS.Harmony.Patch(original: AccessTools.Method("PolyamorySweetKiss.Kissing:PlayerNPCKiss"),
                        transpiler: new HarmonyMethod(typeof(NpcKissed), nameof(Transpiler_Compatibility_PolySweetHugsAndKisses)));
                }
                catch (Exception ex)
                {
                    Log.Error($"Error adding compatibility patch for Polyamory Sweet Kiss: {ex}");
                    Log.Error("Report this error to BETAS, not to Polyamory Sweet Kiss!");
                }
            }

            if (BETAS.ModHelper.ModRegistry.IsLoaded("JoXW.HugsAndKisses"))
            {
                Log.Trace("Adding Hugs and Kisses compatibility patch for NpcKissed...");
                try
                {
                    BETAS.Harmony.Patch(original: AccessTools.Method("HugsAndKisses.Framework.Kissing:PlayerNPCKiss"),
                        transpiler: new HarmonyMethod(typeof(NpcKissed), nameof(Transpiler_Compatibility_PolySweetHugsAndKisses)));
                }
                catch (Exception ex)
                {
                    Log.Error($"Error adding compatibility patch for Hugs and Kisses: {ex}");
                    Log.Error("Report this error to BETAS, not to Hugs and Kisses!");
                }
            }
            
            if (BETAS.ModHelper.ModRegistry.IsLoaded("EnderTedi.Polyamory"))
            {
                Log.Trace("Adding EnderTedi's Polyamory compatibility patch for NpcKissed...");
                try
                {
                    BETAS.Harmony.Patch(original: AccessTools.Method("Polyamory.Patchers.NPCPatcher+NPCPatch_checkAction:Prefix"),
                        transpiler: new HarmonyMethod(typeof(NpcKissed), nameof(Transpiler_Compatibility_EnderTedi)));
                }
                catch (Exception ex)
                {
                    Log.Error($"Error adding compatibility patch for EnderTedi's Polyamory: {ex}");
                    Log.Error("Report this error to BETAS, not to EnderTedi's Polyamory!");
                }
            }
        }
        
        public static void Trigger(NPC kissee, Farmer kisser, GameLocation location)
        {
            var npcItem = ItemRegistry.Create(kissee.Name);
            npcItem.modData["BETAS/NpcKissed/Age"] = kissee.Age switch
            {
                0 => "Adult",
                1 => "Teen",
                _ => "Child",
            };
            npcItem.modData["BETAS/NpcKissed/Gender"] = kissee.Gender switch
            {
                Gender.Female => "Female",
                Gender.Male => "Male",
                _ => "Undefined"
            };
            npcItem.modData["BETAS/NpcKissed/Friendship"] = kisser.getFriendshipLevelForNPC(kissee.Name).ToString();
            npcItem.modData["BETAS/NpcKissed/WasDatingFarmer"] = kisser.friendshipData.ContainsKey(kissee.Name) &&
                                                                 kisser.friendshipData[kissee.Name].IsDating()
                ? "True"
                : "False";
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_NpcKissed", targetItem: npcItem, location: location);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(NPC), nameof(NPC.checkAction))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Farmer), nameof(Farmer.PerformKiss)))
                ).Repeat((codeMatcher) =>
                {
                    codeMatcher.Advance(1).Insert(
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Ldarg_2),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(NpcKissed), nameof(Trigger))));
                });

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.NpcKissed_NPC_checkAction_Transpiler: \n" + ex);
                return code;
            }
        }

        public static IEnumerable<CodeInstruction> Transpiler_Compatibility_PolySweetHugsAndKisses(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Farmer), nameof(Farmer.PerformKiss)))
                ).Repeat((codeMatcher) =>
                {
                    codeMatcher.Advance(1).Insert(
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Character), nameof(Character.currentLocation))),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(NpcKissed), nameof(Trigger))));
                });

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.NpcKissed_NPC_checkAction_Transpiler_Compatibility_PolySweet/HugsAndKisses: \n" + ex);
                return code;
            }
        }
        
        public static IEnumerable<CodeInstruction> Transpiler_Compatibility_EnderTedi(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Farmer), nameof(Farmer.PerformKiss)))
                ).Repeat((codeMatcher) =>
                {
                    codeMatcher.Advance(1).Insert(
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldarg_2),
                        new CodeInstruction(OpCodes.Ldarg_3),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(NpcKissed), nameof(Trigger))));
                });

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.NpcKissed_NPC_checkAction_Transpiler_Compatibility_EnderTedi: \n" + ex);
                return code;
            }
        }
    }
}