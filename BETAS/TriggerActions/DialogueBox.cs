﻿using System;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.TriggerActions;

public static class DialogueBox
{
    // Make the current farmer perform an emote.
    [Action("DialogueBox")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var name, out error) ||
            !ArgUtilityExtensions.TryGetTokenizable(args, 2, out var message, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(args, 3, out var portrait, out error, defaultValue: "null") ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(args, 4, out var displayName, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_DialogueBox <Name> <Message> [Portrait] [DisplayName]";
            return false;
        }

        var NPC = Game1.getCharacterFromName(name);
        Texture2D? portraitTexture = null;

        if (ArgUtility.HasIndex(args, 3) && !portrait.EqualsIgnoreCase("null"))
        {
            if (!Game1.content.DoesAssetExist<Texture2D>(portrait))
            {
                Log.Warn("No asset found with name '" + portrait + "'");
            }
            else
            {
                portraitTexture = Game1.content.Load<Texture2D>(portrait);
            }
        }

        if (portraitTexture is not null)
        {
            if (NPC is not null)
            {
                NPC = new NPC(NPC.Sprite, Vector2.Zero, "", 0, NPC.Name, portrait.EqualsIgnoreCase("null") ? NPC.Portrait : portraitTexture,
                    eventActor: false);
                NPC.displayName = displayName ?? NPC.displayName;
            }
            else
            {
                NPC = new NPC(new AnimatedSprite("Characters\\Abigail", 0, 16, 16),
                    Vector2.Zero, "", 0, "???", portraitTexture, eventActor: false)
                {
                    displayName = displayName ?? name
                };
            }
        }

        try
        {
            Game1.DrawDialogue(NPC, message);
        }
        catch (Exception)
        {
            var dialogue = new Dialogue(NPC, null, message);
            Game1.DrawDialogue(dialogue);
        }

        return true;
    }
}