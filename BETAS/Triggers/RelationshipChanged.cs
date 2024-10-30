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
    static class RelationshipChanged
    {
        public static void Trigger(Friendship newData, Friendship oldData, NPC npc, Farmer who)
        {
            var oldFriendship = ItemRegistry.Create(npc.Name);
            var newFriendship = ItemRegistry.Create(npc.Name);

            oldFriendship.modData["BETAS/RelationshipChanged/Status"] =
                oldData.IsRoommate() ? "Roommate" : oldData.Status.ToString();
            oldFriendship.modData["BETAS/RelationshipChanged/Friendship"] = oldData.Points.ToString();
            if (oldData.GiftsToday > 0)
                oldFriendship.modData["BETAS/RelationshipChanged/GiftsToday"] = oldData.GiftsToday.ToString();
            if (oldData.GiftsThisWeek > 0)
                oldFriendship.modData["BETAS/RelationshipChanged/GiftsThisWeek"] = oldData.GiftsThisWeek.ToString();
            if (oldData.LastGiftDate != null)
                oldFriendship.modData["BETAS/RelationshipChanged/DaysSinceLastGift"] =
                    (Game1.Date.TotalDays - oldData.LastGiftDate.TotalDays).ToString();
            oldFriendship.modData["BETAS/RelationshipChanged/WasTalkedToToday"] = oldData.TalkedToToday.ToString();
            oldFriendship.modData["BETAS/RelationshipChanged/WasRoommate"] = oldData.RoommateMarriage.ToString();
            oldFriendship.modData["BETAS/RelationshipChanged/IsRoommate"] = oldData.RoommateMarriage.ToString();
            if (oldData.WeddingDate != null)
            {
                oldFriendship.modData["BETAS/RelationshipChanged/WeddingSeason"] = oldData.WeddingDate.SeasonKey;
                oldFriendship.modData["BETAS/RelationshipChanged/WeddingDay"] =
                    oldData.WeddingDate.DayOfMonth.ToString();
                oldFriendship.modData["BETAS/RelationshipChanged/WeddingYear"] = oldData.WeddingDate.Year.ToString();
                oldFriendship.modData["BETAS/RelationshipChanged/DaysMarried"] =
                    (Game1.Date.TotalDays - oldData.WeddingDate.TotalDays).ToString();
            }

            if (oldData.NextBirthingDate != null)
                oldFriendship.modData["BETAS/RelationshipChanged/NextBirthingDate"] =
                    oldData.NextBirthingDate.ToString();

            newFriendship.modData["BETAS/RelationshipChanged/Status"] =
                newData.IsRoommate() ? "Roommate" : newData.Status.ToString();
            newFriendship.modData["BETAS/RelationshipChanged/Friendship"] = newData.Points.ToString();
            if (newData.GiftsToday > 0)
                newFriendship.modData["BETAS/RelationshipChanged/GiftsToday"] = newData.GiftsToday.ToString();
            if (newData.GiftsThisWeek > 0)
                newFriendship.modData["BETAS/RelationshipChanged/GiftsThisWeek"] = newData.GiftsThisWeek.ToString();
            if (newData.LastGiftDate != null)
                newFriendship.modData["BETAS/RelationshipChanged/DaysSinceLastGift"] =
                    (Game1.Date.TotalDays - newData.LastGiftDate.TotalDays).ToString();
            newFriendship.modData["BETAS/RelationshipChanged/WasTalkedToToday"] = newData.TalkedToToday.ToString();
            newFriendship.modData["BETAS/RelationshipChanged/WasRoommate"] = newData.RoommateMarriage.ToString();
            newFriendship.modData["BETAS/RelationshipChanged/IsRoommate"] =
                newData.RoommateMarriage
                    .ToString(); // Since this is the only edge case where this shouldn't actually be past tense, people might get confused, so I'm making both "Is" and "Was" work here.
            if (newData.WeddingDate != null)
            {
                newFriendship.modData["BETAS/RelationshipChanged/WeddingSeason"] = newData.WeddingDate.SeasonKey;
                newFriendship.modData["BETAS/RelationshipChanged/WeddingDay"] =
                    newData.WeddingDate.DayOfMonth.ToString();
                newFriendship.modData["BETAS/RelationshipChanged/WeddingYear"] = newData.WeddingDate.Year.ToString();
                newFriendship.modData["BETAS/RelationshipChanged/DaysMarried"] =
                    (Game1.Date.TotalDays - newData.WeddingDate.TotalDays).ToString();
            }

            if (newData.NextBirthingDate != null)
                newFriendship.modData["BETAS/RelationshipChanged/NextBirthingDate"] =
                    newData.NextBirthingDate.ToString();

            newFriendship.modData["BETAS/RelationshipChanged/WasMemoryWiped"] =
                oldData.Status == FriendshipStatus.Divorced && newData.Status == FriendshipStatus.Friendly
                    ? "true"
                    : "false";

            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_RelationshipChanged", inputItem: oldFriendship,
                targetItem: newFriendship, location: npc.currentLocation, player: who);
        }

        public static void Trigger(Friendship newData, Friendship oldData, string npc, Farmer who)
        {
            var character = Game1.getCharacterFromName(npc);
            if (character != null)
            {
                Trigger(newData, oldData, character, who);
            }
        }

        public static Friendship FriendlyCloner(Friendship data)
        {
            if (data is null) return null;
            return new Friendship()
            {
                Points = data.Points,
                GiftsToday = data.GiftsToday,
                GiftsThisWeek = data.GiftsThisWeek,
                LastGiftDate = data.LastGiftDate,
                TalkedToToday = data.TalkedToToday,
                RoommateMarriage = data.RoommateMarriage,
                WeddingDate = data.WeddingDate,
                NextBirthingDate = data.NextBirthingDate,
                Status = data.Status,
            };
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(NPC), nameof(NPC.tryToReceiveActiveObject))]
        public static IEnumerable<CodeInstruction> tryToReceiveActiveObject_Transpiler(
            IEnumerable<CodeInstruction> instructions,
            ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                var oldFriendship = il.DeclareLocal(typeof(Friendship));

                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(NPC), nameof(NPC.CanReceiveGifts)))
                ).ThrowIfNotMatch("Could not find proper entry point #1 for NPC_tryToReceiveActiveObject_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipChanged), nameof(FriendlyCloner))),
                    new CodeInstruction(OpCodes.Stloc, oldFriendship)
                );

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Callvirt,
                        AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Status)))
                ).Repeat((codeMatcher) =>
                {
                    codeMatcher.Advance(1).Insert(
                        new CodeInstruction(OpCodes.Ldloc_1),
                        new CodeInstruction(OpCodes.Ldloc, oldFriendship),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Call,
                            AccessTools.Method(typeof(RelationshipChanged), nameof(Trigger),
                                [typeof(Friendship), typeof(Friendship), typeof(NPC), typeof(Farmer)]))
                    );
                });

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.RelationshipChanged_NPC_tryToReceiveActiveObject_Transpiler: \n" + ex);
                return code;
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Game1), "_newDayAfterFade", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> newDayAfterFade_Transpiler(IEnumerable<CodeInstruction> instructions,
            ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                var oldFriendship = il.DeclareLocal(typeof(Friendship));

                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Dup),
                    new CodeMatch(OpCodes.Ldc_I4_3),
                    new CodeMatch(OpCodes.Callvirt,
                        AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Status)))
                ).ThrowIfNotMatch("Could not find proper entry point #1 for Game1_newDayAfterFade_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipChanged), nameof(FriendlyCloner))),
                    new CodeInstruction(OpCodes.Stloc, oldFriendship),
                    new CodeInstruction(OpCodes.Dup)
                );

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Callvirt,
                        AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.WeddingDate)))
                ).ThrowIfNotMatch("Could not find proper entry point #2 for Game1_newDayAfterFade_Transpiler");

                matcher.Advance(1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc, oldFriendship),
                    new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Game1), nameof(Game1.player))),
                    new CodeInstruction(OpCodes.Callvirt,
                        AccessTools.PropertyGetter(typeof(Farmer), nameof(Farmer.spouse))),
                    new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Game1), nameof(Game1.player))),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipChanged), nameof(Trigger),
                            [typeof(Friendship), typeof(Friendship), typeof(string), typeof(Farmer)]))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.RelationshipChanged_Game1_newDayAfterFade_Transpiler: \n" + ex);
                return code;
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(NPC), "engagementResponse")]
        public static IEnumerable<CodeInstruction> engagementResponse_Transpiler(
            IEnumerable<CodeInstruction> instructions,
            ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                var oldFriendship = il.DeclareLocal(typeof(Friendship));

                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_2),
                    new CodeMatch(OpCodes.Callvirt,
                        AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Status)))
                ).ThrowIfNotMatch("Could not find proper entry point #1 for NPC_engagementResponse_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipChanged), nameof(FriendlyCloner))),
                    new CodeInstruction(OpCodes.Stloc, oldFriendship)
                );

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldloc_2),
                    new CodeMatch(OpCodes.Callvirt,
                        AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.WeddingDate)))
                ).ThrowIfNotMatch("Could not find proper entry point #2 for NPC_engagementResponse_Transpiler");

                matcher.Advance(1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldloc, oldFriendship),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipChanged), nameof(Trigger),
                            [typeof(Friendship), typeof(Friendship), typeof(NPC), typeof(Farmer)]))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.RelationshipChanged_NPC_engagementResponse_Transpiler: \n" + ex);
                return code;
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.wipeExMemories))]
        public static IEnumerable<CodeInstruction> wipeExMemories_Transpiler(IEnumerable<CodeInstruction> instructions,
            ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                var oldFriendship = il.DeclareLocal(typeof(Friendship));

                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Brfalse_S),
                    new CodeMatch(OpCodes.Ldloc_3),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Friendship), nameof(Friendship.Clear)))
                ).ThrowIfNotMatch("Could not find proper entry point #1 for Farmer_wipeExMemories_Transpiler");

                matcher.Advance(1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipChanged), nameof(FriendlyCloner))),
                    new CodeInstruction(OpCodes.Stloc, oldFriendship)
                );

                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloca_S),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Brtrue_S),
                    new CodeMatch(OpCodes.Leave_S)
                ).ThrowIfNotMatch("Could not find proper entry point #2 for Farmer_wipeExMemories_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Ldloc, oldFriendship),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipChanged), nameof(Trigger),
                            new Type[] { typeof(Friendship), typeof(Friendship), typeof(string), typeof(Farmer) }))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.RelationshipChanged_Farmer_wipeExMemories_Transpiler: \n" + ex);
                return code;
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.doDivorce))]
        public static IEnumerable<CodeInstruction> doDivorve_Transpiler(IEnumerable<CodeInstruction> instructions,
            ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);
                var oldData = il.DeclareLocal(typeof(Friendship));

                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_0),
                    new CodeMatch(OpCodes.Callvirt,
                        AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Points))),
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_0)
                ).ThrowIfNotMatch("Could not find proper entry point #1 for Farmer_doDivorce_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipChanged), nameof(FriendlyCloner))),
                    new CodeInstruction(OpCodes.Stloc_S, oldData)
                );

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_4),
                    new CodeMatch(OpCodes.Callvirt,
                        AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Status)))
                ).ThrowIfNotMatch("Could not find proper entry point #2 for Farmer_doDivorce_Transpiler");

                matcher.Advance(1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_S, oldData),
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RelationshipChanged), nameof(Trigger),
                        [typeof(Friendship), typeof(Friendship), typeof(NPC), typeof(Farmer)]))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.RelationshipChanged_Farmer_doDivorce_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}