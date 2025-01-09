using System;
using System.Numerics;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.Actions;

public static class DialogueBox
{
    // Make the current farmer perform an emote.
    [Action("DialogueBox")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var name, out error) ||
            !ArgUtilityExtensions.TryGetTokenizable(args, 2, out var message, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(args, 3, out var portrait, out error))
        {
            error = "Usage: DialogueBox <Name> <Message> [Portrait]";
            return false;
        }

        var NPC = Game1.getCharacterFromName(name);
        Texture2D portraitTexture = null;

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

        if (portraitTexture is not null || name is not null)
        {
            if (NPC is not null)
            {
                NPC = new NPC(NPC.Sprite, Microsoft.Xna.Framework.Vector2.Zero, "", 0, NPC.Name, portrait.EqualsIgnoreCase("null") ? null : portraitTexture ?? NPC.Portrait,
                    eventActor: false);
                NPC.displayName = name ?? NPC.displayName;
            }
            else if (portraitTexture is not null)
            {
                NPC = new NPC(new AnimatedSprite("Characters\\Abigail", 0, 16, 16),
                    Microsoft.Xna.Framework.Vector2.Zero, "", 0, "???", portraitTexture, eventActor: false)
                {
                    displayName = name ?? "???"
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