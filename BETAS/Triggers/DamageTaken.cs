using System;
using HarmonyLib;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Tools;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [HarmonyPatch]
    static class DamageTaken
    {
        public static void Trigger(Farmer who, int damage, Monster damager, bool parried)
        {
            var damageItem = ItemRegistry.Create(damager.Name);
            damageItem.modData["BETAS/DamageTaken/Damage"] = damage.ToString();
            damageItem.modData["BETAS/DamageTaken/WasParried"] = parried ? "true" : "false";
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_DamageTaken", targetItem: damageItem, location: who.currentLocation, player: who);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Farmer), nameof(Farmer.takeDamage))]
        public static void takeDamage_Postfix(Farmer __instance, int damage, bool overrideParry, Monster damager)
        {
            try
            {
                if (Game1.eventUp || __instance.FarmerSprite.isPassingOut() || (__instance.isInBed.Value && Game1.activeClickableMenu != null && Game1.activeClickableMenu is ReadyCheckDialog))
                {
                    return;
                }
                bool num = damager != null && !damager.isInvincible() && !overrideParry;
                bool monsterDamageCapable = (damager == null || !damager.isInvincible()) && (damager == null || (!(damager is GreenSlime) && !(damager is BigSlime)) || !__instance.isWearingRing("520"));
                bool playerParryable = __instance.CurrentTool is MeleeWeapon && ((MeleeWeapon)__instance.CurrentTool).isOnSpecial && ((MeleeWeapon)__instance.CurrentTool).type.Value == 3;
                bool playerDamageable = __instance.CanBeDamaged();
                bool parried = num && playerParryable;
                
                if (!(monsterDamageCapable && playerDamageable))
                {
                    return;
                }
                
                Trigger(__instance, damage, damager, parried);
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.DamageTaken_Farmer_takeDamage_Postfix: \n" + ex);
            }
        }
    }
}