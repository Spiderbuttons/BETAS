﻿using System;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Pathfinding;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class NpcArrived
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PathFindController), "moveCharacter")]
        public static void moveCharacter_Postfix(Character ___character, PathFindController __instance)
        {
            try
            {
                if (!Context.IsMainPlayer) return;
                if (__instance.pathToEndPoint.Count != 0 || !__instance.NPCSchedule) return;
                
                var npcItem = ItemRegistry.Create(___character.Name);
                npcItem.modData["BETAS/NpcArrived/X"] = $"{___character.TilePoint.X}";
                npcItem.modData["BETAS/NpcArrived/Y"] = $"{___character.TilePoint.Y}";
                //Log.Debug($"NpcArrived: {___character.Name} at {___character.TilePoint.X}, {___character.TilePoint.Y}");
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_NpcArrived", targetItem: npcItem,
                    location: ___character.currentLocation);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.NpcArrived_PathFindController_moveCharacter_Postfix: \n" + ex);
            }
        }
    }
}