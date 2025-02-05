using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BETAS.Attributes;
using BETAS.Helpers;
using ContentPatcher;
using HarmonyLib;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Tools;
using StardewValley.Menus;
using StardewValley.Tools;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class ItemBought
    {
        public static void Trigger(ISalable item, string shopId)
        {
            if (item is null) return;

            /*
                Returning early if it's a GenericTool is a temporary fix to work around a SpaceCore bug.
                SpaceCore.Patches.ToolDataDefinitionPatcher.After_CreateToolInstance(Tool, ToolData)
                No null check in the Where(), null reference exception when checking toolData.ClassName.
            */
            if (item is GenericTool) return;

            var boughtItem = ItemRegistry.Create(item.QualifiedItemId, item.Stack, item.Quality);
            if (shopId is not null) boughtItem.modData["BETAS/ItemBought/ShopId"] = shopId;
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ItemBought", targetItem: boughtItem);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ShopMenu), "tryToPurchaseItem")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(ShopMenu), nameof(ShopMenu.chargePlayer)))
                ).Repeat(codeMatcher =>
                {
                    codeMatcher.Advance(1).Insert(
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldfld,
                            AccessTools.Field(typeof(ShopMenu), nameof(ShopMenu.ShopId))),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ItemBought), nameof(Trigger)))
                    );
                });

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ItemBought_ShopMenu_tryToPurchaseItem_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}