﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.GameData.GarbageCans;
using StardewValley.Monsters;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class GarbageTrigger
    {
        public static void Trigger_GarbageChecked(string trashId, Item result, GarbageCanItemData data, bool caught, Farmer farmer, Vector2 tile, GameLocation location)
        {
            var trashItem = result ?? ItemRegistry.Create(trashId);
            trashItem.modData["BETAS/GarbageChecked/GarbageCanId"] = trashId;
            if (data != null)
            {
                trashItem.modData["BETAS/GarbageChecked/IsMegaSuccess"] = data.IsMegaSuccess ? "true" : "false";
                trashItem.modData["BETAS/GarbageChecked/IsDoubleMegaSuccess"] = data.IsDoubleMegaSuccess ? "true" : "false";
            }
            if (caught)
            {
                var witnesses = (from npc in Utility.GetNpcsWithinDistance(tile, 7, location)
                    where npc is not Horse
                    select npc.Name).ToList();
                if (witnesses.Count > 0) trashItem.modData["BETAS/GarbageChecked/Witnesses"] = string.Join(",", witnesses);
            }
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_GarbageChecked", targetItem: trashItem, location: location, player: farmer);
        }
        
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.CheckGarbage))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il).End().Advance(-1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldarg_1).MoveLabelsFrom(matcher.Instruction),
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Ldarg_S, 5),
                    new CodeInstruction(OpCodes.Ldarg_3),
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GarbageTrigger), nameof(Trigger_GarbageChecked)))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.GarbageTrigger_GameLocation_CheckGarbage_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}