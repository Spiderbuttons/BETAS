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
using Object = System.Object;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class RelationshipTrigger
    {
        public static void Trigger_RelationshipChanged(Friendship oldData, Friendship newData, NPC npc, Farmer who)
        {
            var oldFriendship = ItemRegistry.Create(npc.Name);
            var newFriendship = ItemRegistry.Create(npc.Name);
            oldFriendship.modData["BETAS/RelationshipChanged/Status"] = oldData.IsRoommate() ? "Roommate" : oldData.Status.ToString();
            oldFriendship.modData["BETAS/RelationshipChanged/Points"] = oldData.Points.ToString();
            if (oldData.GiftsToday > 0) oldFriendship.modData["BETAS/RelationshipChanged/GiftsToday"] = oldData.GiftsToday.ToString();
            if (oldData.GiftsThisWeek > 0) oldFriendship.modData["BETAS/RelationshipChanged/GiftsThisWeek"] = oldData.GiftsThisWeek.ToString();
            if (oldData.LastGiftDate != null) oldFriendship.modData["BETAS/RelationshipChanged/DaysSinceLastGift"] = (Game1.Date.TotalDays - oldData.LastGiftDate.TotalDays).ToString();
            oldFriendship.modData["BETAS/RelationshipChanged/IsTalkedToToday"] = oldData.TalkedToToday.ToString();
            oldFriendship.modData["BETAS/RelationshipChanged/IsRoommate"] = oldData.RoommateMarriage.ToString();
            if (oldData.WeddingDate != null)
            {
                oldFriendship.modData["BETAS/RelationshipChanged/WeddingSeason"] = oldData.WeddingDate.SeasonKey;
                oldFriendship.modData["BETAS/RelationshipChanged/WeddingDay"] = oldData.WeddingDate.DayOfMonth.ToString();
                oldFriendship.modData["BETAS/RelationshipChanged/WeddingYear"] = oldData.WeddingDate.Year.ToString();
                oldFriendship.modData["BETAS/RelationshipChanged/DaysMarried"] = (Game1.Date.TotalDays - oldData.WeddingDate.TotalDays).ToString();
            }
            if (oldData.NextBirthingDate != null) oldFriendship.modData["BETAS/RelationshipChanged/NextBirthingDate"] = oldData.NextBirthingDate.ToString();
            
            newFriendship.modData["BETAS/RelationshipChanged/Status"] = newData.IsRoommate() ? "Roommate" : newData.Status.ToString();
            newFriendship.modData["BETAS/RelationshipChanged/Points"] = newData.Points.ToString();
            if (newData.GiftsToday > 0) newFriendship.modData["BETAS/RelationshipChanged/GiftsToday"] = newData.GiftsToday.ToString();
            if (newData.GiftsThisWeek > 0) newFriendship.modData["BETAS/RelationshipChanged/GiftsThisWeek"] = newData.GiftsThisWeek.ToString();
            if (newData.LastGiftDate != null) newFriendship.modData["BETAS/RelationshipChanged/DaysSinceLastGift"] = (Game1.Date.TotalDays - newData.LastGiftDate.TotalDays).ToString();
            newFriendship.modData["BETAS/RelationshipChanged/IsTalkedToToday"] = newData.TalkedToToday.ToString();
            newFriendship.modData["BETAS/RelationshipChanged/IsRoommate"] = newData.RoommateMarriage.ToString();
            if (newData.WeddingDate != null)
            {
                newFriendship.modData["BETAS/RelationshipChanged/WeddingSeason"] = newData.WeddingDate.SeasonKey;
                newFriendship.modData["BETAS/RelationshipChanged/WeddingDay"] = newData.WeddingDate.DayOfMonth.ToString();
                newFriendship.modData["BETAS/RelationshipChanged/WeddingYear"] = newData.WeddingDate.Year.ToString();
                newFriendship.modData["BETAS/RelationshipChanged/DaysMarried"] = (Game1.Date.TotalDays - newData.WeddingDate.TotalDays).ToString();
            }
            if (newData.NextBirthingDate != null) newFriendship.modData["BETAS/RelationshipChanged/NextBirthingDate"] = newData.NextBirthingDate.ToString();
            
            newFriendship.modData["BETAS/RelationshipChanged/IsMemoryWiped"] = oldData.Status == FriendshipStatus.Divorced && newData.Status == FriendshipStatus.Friendly ? "true" : "false";
            
            Log.Debug($"Old Status: {(oldData.IsRoommate() ? "Roommate" : oldData.Status.ToString())}, New Status: {(newData.IsRoommate() ? "Roommate" : newData.Status.ToString())}");
            Log.Debug($"Old Points: {oldData.Points.ToString()}, New Points: {newData.Points.ToString()}");
            Log.Debug($"Old GiftsToday: {oldData.GiftsToday.ToString()}, New GiftsToday: {newData.GiftsToday.ToString()}");
            Log.Debug($"Old GiftsThisWeek: {oldData.GiftsThisWeek.ToString()}, New GiftsThisWeek: {newData.GiftsThisWeek.ToString()}");
            Log.Debug($"Old LastGiftDate: {oldData.LastGiftDate?.ToString()}, New LastGiftDate: {newData.LastGiftDate?.ToString()}");
            Log.Debug($"Old TalkedToToday: {oldData.TalkedToToday.ToString()}, New TalkedToToday: {newData.TalkedToToday.ToString()}");
            Log.Debug($"Old RoommateMarriage: {oldData.RoommateMarriage.ToString()}, New RoommateMarriage: {newData.RoommateMarriage.ToString()}");
            if (oldData.WeddingDate != null)
            {
                Log.Debug($"Old WeddingDate: {oldData.WeddingDate.SeasonKey} {oldData.WeddingDate.DayOfMonth.ToString()} {oldData.WeddingDate.Year.ToString()}, New WeddingDate: {newData.WeddingDate?.SeasonKey} {newData.WeddingDate?.DayOfMonth.ToString()} {newData.WeddingDate?.Year.ToString()}");
            }
            else
            {
                Log.Debug($"Old WeddingDate: null, New WeddingDate: null");
            }
            Log.Debug($"Old NextBirthingDate: {oldData.NextBirthingDate?.ToString()}, New NextBirthingDate: {newData.NextBirthingDate?.ToString()}");
            Log.Debug($"Was Memory Wiped: {newFriendship.modData["BETAS/RelationshipChanged/IsMemoryWiped"]}");
            
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_RelationshipChanged", inputItem: oldFriendship, targetItem: newFriendship, location: npc.currentLocation, player: who);
        }

        public static void Trigger_RelationshipChanged(Friendship oldData, Friendship newData, string npc, Farmer who)
        {
            var character = Game1.getCharacterFromName(npc);
            if (character != null)
            {
                Trigger_RelationshipChanged(oldData, newData, character, who);
            }
        }
        
        public static Friendship FriendlyCloner(Friendship data)
        {
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
        [HarmonyPatch(typeof(NPC), "engagementResponse")]
        public static IEnumerable<CodeInstruction> engagementResponse_Transpiler(IEnumerable<CodeInstruction> instructions,
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
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Status)))
                ).ThrowIfNotMatch("Could not find proper entry point #1 for NPC_engagementResponse_Transpiler");

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipTrigger), nameof(FriendlyCloner))),
                    new CodeInstruction(OpCodes.Stloc, oldFriendship)
                );

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldloc_2),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.WeddingDate)))
                ).ThrowIfNotMatch("Could not find proper entry point #2 for NPC_engagementResponse_Transpiler");

                matcher.Advance(1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc, oldFriendship),
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(RelationshipTrigger), nameof(Trigger_RelationshipChanged),
                            new Type[] { typeof(Friendship), typeof(Friendship), typeof(NPC), typeof(Farmer) }))
                );
                
                Log.ILCode(matcher.InstructionEnumeration(), code);

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.RelationshipTrigger_NPC_engagementResponse_Transpiler: \n" + ex);
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
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RelationshipTrigger), nameof(FriendlyCloner))),
                    new CodeInstruction(OpCodes.Stloc, oldFriendship)
                );
                
                matcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloca_S),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Brtrue_S),
                    new CodeMatch(OpCodes.Leave_S)
                ).ThrowIfNotMatch("Could not find proper entry point #2 for Farmer_wipeExMemories_Transpiler");
                
                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc, oldFriendship),
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RelationshipTrigger), nameof(Trigger_RelationshipChanged), new Type[] {typeof(Friendship), typeof(Friendship), typeof(string), typeof(Farmer)}))
                );
                
                return matcher.InstructionEnumeration();    
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.RelationshipTrigger_Farmer_wipeExMemories_Transpiler: \n" + ex);
                return code;
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.doDivorce))]
        public static IEnumerable<CodeInstruction> doDivorve_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
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
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RelationshipTrigger), nameof(FriendlyCloner)))
                );
                
                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_1),
                    new CodeMatch(OpCodes.Ldc_I4_4),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(Friendship), nameof(Friendship.Status)))
                ).ThrowIfNotMatch("Could not find proper entry point #2 for Farmer_doDivorce_Transpiler");

                matcher.Advance(1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RelationshipTrigger), nameof(Trigger_RelationshipChanged), new Type[] {typeof(Friendship), typeof(Friendship), typeof(NPC), typeof(Farmer)}))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.RelationshipTrigger_Farmer_doDivorce_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}