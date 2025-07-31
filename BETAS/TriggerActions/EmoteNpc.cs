using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class EmoteNpc
{
    // Make an NPC perform an emote.
    [Action("EmoteNpc")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string? npcName, out error, allowBlank: false) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(args, 2, out int emote, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_EmoteNpc <NPC Name> <EmoteId>";
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName)?.EventActorIfPossible();
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        npc.doEmote(emote, 24);
        return true;
    }
}