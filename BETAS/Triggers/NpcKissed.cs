using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class NpcKissed
    {
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
    }
}