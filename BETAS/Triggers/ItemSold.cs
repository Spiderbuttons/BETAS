using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class ItemSold
    {
        public static void Trigger(ISalable item, string shopId)
        {
            if (item is null) return;

            var soldItem = ItemRegistry.Create(item.QualifiedItemId, item.Stack, item.Quality);
            if (shopId is not null) soldItem.modData["BETAS/ItemSold/ShopId"] = shopId;
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ItemSold", targetItem: soldItem, triggerArgs: [soldItem]);
        }
        
        static IEnumerable<MethodBase> TargetMethods()
        {
            return new[]
            {
                AccessTools.Method(typeof(ShopMenu), nameof(ShopMenu.receiveLeftClick)),
                AccessTools.Method(typeof(ShopMenu), nameof(ShopMenu.receiveRightClick))
            };
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(op => op.IsLdarg()),
                    new CodeMatch(OpCodes.Ldfld,
                        AccessTools.Field(typeof(ShopMenu), nameof(ShopMenu.inventory))),
                    new CodeMatch(op => op.IsLdarg()),
                    new CodeMatch(op => op.IsLdarg()),
                    new CodeMatch(OpCodes.Ldnull),
                    new CodeMatch(OpCodes.Ldc_I4_0)
                ).ThrowIfNotMatch(
                    "Could not find first entry point for ShopMenu_receiveLeftClick/receiveRightClick_Transpiler");
                
                matcher.MatchEndForward(
                    new CodeMatch(op => op.opcode == OpCodes.Callvirt &&
                                        (op.OperandIs(AccessTools.Method(typeof(InventoryMenu),
                                             nameof(InventoryMenu.rightClick))) ||
                                         op.OperandIs(AccessTools.Method(typeof(InventoryMenu),
                                             nameof(InventoryMenu.leftClick))))),
                    new CodeMatch(op => op.IsStloc()),
                    new CodeMatch(op => op.IsLdloc())
                ).ThrowIfNotMatch(
                    "Could not find second entry point for ShopMenu_receiveLeftClick/receiveRightClick_Transpiler");

                matcher.Advance(1);

                matcher.Insert(
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ShopMenu), nameof(ShopMenu.ShopId))),
                    new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(ItemSold), nameof(Trigger)))
                );

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.ItemSold_ShopMenu_receiveLeftClick/receiveRightClick_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}