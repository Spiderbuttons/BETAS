﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class MonsterKilled
    {
        public static void Trigger(Monster mon, GameLocation loc, List<Debris> drops, Farmer killer)
        {
            var monsterItem = ItemRegistry.Create(mon.Name, drops.Count);
            if (drops.Count > 0)
                monsterItem.modData["BETAS/MonsterKilled/Drops"] =
                    drops.Join(d => ItemRegistry.QualifyItemId(d.itemId.Value), ",");
            monsterItem.modData["BETAS/MonsterKilled/MaxHealth"] = mon.MaxHealth.ToString();
            monsterItem.modData["BETAS/MonsterKilled/Damage"] = mon.DamageToFarmer.ToString();
            monsterItem.modData["BETAS/MonsterKilled/WasHardmodeMonster"] = mon.isHardModeMonster.Value.ToString();
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_MonsterKilled", targetItem: monsterItem,
                location: loc, player: killer);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.monsterDrop))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il).End();

                matcher.SetOpcodeAndAdvance(OpCodes.Ldarg_1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Ldarg_S, 4),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MonsterKilled), nameof(Trigger))),
                    new CodeInstruction(OpCodes.Ret)
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.MonsterKilled_GameLocation_monsterDrop_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}