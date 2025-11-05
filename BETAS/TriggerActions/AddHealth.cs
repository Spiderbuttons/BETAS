using System;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Objects.Trinkets;

namespace BETAS.TriggerActions;

public static class AddHealth
{
    // Add a given value to the health of the current player.
    [Action("AddHealth")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetInt(args, 1, out var health, out error, name: "int #Health") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 2, out var overrideBool, out error, defaultValue: false, name: "bool Override Max?"))
        {
            return false;
        }
        
        if (health >= 0)
        {
            Game1.player.health += health;
            if (!overrideBool) Game1.player.health = Math.Min(Game1.player.health, Game1.player.maxHealth);
            return true;
        }
        
        // Basically recreating Farmer.takeDamage because I figure this traction should ignore defense and parrying and whatnot, but we still want the game to react to it like damage otherwise.

        health = Math.Abs(health);
        Rumble.rumble(0.75f, 150f);
        Game1.player.health = Math.Max(0, Game1.player.health - health);
        
        foreach (Trinket trinket in Game1.player.trinketItems)
        {
            trinket?.OnReceiveDamage(Game1.player, health);
        }
        
        if (Game1.player.health <= 0 && Game1.player.GetEffectsOfRingMultiplier("863") > 0 && !Game1.player.hasUsedDailyRevive.Value)
        {
            Game1.player.startGlowing(new Color(255, 255, 0), border: false, 0.25f);
            StardewValley.DelayedAction.functionAfterDelay(Game1.player.stopGlowing, 500);
            Game1.playSound("yoba");
            for (int i = 0; i < 13; i++)
            {
                float xPos = Game1.random.Next(-32, 33);
                Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors2", new Rectangle(114, 46, 2, 2), 200f, 5, 1, new Vector2(xPos + 32f, -96f), flicker: false, flipped: false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f)
                {
                    attachedCharacter = Game1.player,
                    positionFollowsAttachedCharacter = true,
                    motion = new Vector2(xPos / 32f, -3f),
                    delayBeforeAnimationStart = i * 50,
                    alphaFade = 0.001f,
                    acceleration = new Vector2(0f, 0.1f)
                });
            }
            Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors2", new Rectangle(157, 280, 28, 19), 2000f, 1, 1, new Vector2(-20f, -16f), flicker: false, flipped: false, 1E-06f, 0f, Color.White, 4f, 0f, 0f, 0f)
            {
                attachedCharacter = Game1.player,
                positionFollowsAttachedCharacter = true,
                alpha = 0.1f,
                alphaFade = -0.01f,
                alphaFadeFade = -0.00025f
            });
            Game1.player.health = (int)Math.Min(Game1.player.maxHealth, (float)Game1.player.maxHealth * 0.5f + (float)Game1.player.GetEffectsOfRingMultiplier("863"));
            Game1.player.hasUsedDailyRevive.Value = true;
        }
        
        Game1.player.temporarilyInvincible = true;
        Game1.player.flashDuringThisTemporaryInvincibility = true;
        Game1.player.temporaryInvincibilityTimer = 0;
        Game1.player.currentTemporaryInvincibilityDuration = 1200 + Game1.player.GetEffectsOfRingMultiplier("861") * 400;
        Point standingPixel = Game1.player.StandingPixel;
        Game1.player.currentLocation.debris.Add(new Debris(health, new Vector2(standingPixel.X + 8, standingPixel.Y), Color.Red, 1f, Game1.player));
        Game1.player.playNearbySoundAll("ow");
        Game1.hitShakeTimer = 100 * health;
        
        return true;
    }
}