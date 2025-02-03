using System.Runtime.CompilerServices;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.GameData.Characters;

namespace BETAS.Actions;

public static class UpdateAppearance
{
    public static bool ForceAppearance(NPC npc, string appearanceId = null)
    {
        if (appearanceId == null)
        {
            npc.ChooseAppearance();
            return true;
        }

        CharacterData data = npc.GetData();
        CharacterAppearanceData appearance = data?.Appearance.Find(appearanceEntry => appearanceEntry.Id == appearanceId);
        if (appearance == null) return false;

        if (!npc.TryLoadPortraits(appearance.Portrait, out var error1, Game1.content))
        {
            Log.Warn($"NPC {npc.Name} can't load portraits from '{appearance.Portrait}' (per appearance entry '{appearance.Id}' in Data/Characters): {error1}. Falling back to default portraits.");
        }
        
        if (!npc.TryLoadSprites(appearance.Sprite, out var error2, Game1.content))
        {
            Log.Warn($"NPC {npc.Name} can't load sprites from '{appearance.Sprite}' (per appearance entry '{appearance.Id}' in Data/Characters): {error2}. Falling back to default sprites.");
        }
        
        if (npc.Sprite is null) return false;
        if (error1 is null && error2 is null) npc.LastAppearanceId = appearanceId;
        
        npc.Sprite.SpriteWidth = data.Size.X;
        npc.Sprite.SpriteHeight = data.Size.Y;
        npc.Sprite.ignoreSourceRectUpdates = false;

        return true;
    }
    
    // Force an update to an NPC's appearance, optionally specifying which appearance to force the change to.
    [Action("UpdateAppearance")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string npcName, out error, allowBlank: false) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(args, 2, out var appearanceId, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_UpdateAppearance <NPC> [AppearanceId]";
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }
        
        return ForceAppearance(npc, appearanceId);
    }
}