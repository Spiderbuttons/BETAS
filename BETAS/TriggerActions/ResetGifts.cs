using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class ResetGifts
{
    // Reset the weekly/daily gift giving limit for a specific NPC.
    [Action("ResetGifts")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out var npcName, out error, allowBlank: true, name: "string NPC") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 2, out var gifts, out error, defaultValue: 0, name: "int #Amount") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 3, out var today, out error, defaultValue: false, name: "bool Today?"))
        {
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        if (Game1.player.friendshipData.TryGetValue(npc.Name, out var friendship))
        {
            if (!today) friendship.GiftsThisWeek = gifts;
            else friendship.GiftsToday = gifts;
            return true;
        }

        Log.Warn("Tried to reset the gift limit for a friend that wasn't there.");
        return false;
    }
}