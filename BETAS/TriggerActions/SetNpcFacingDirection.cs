using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class SetNpcFacingDirection
{
    // Make an NPC faces certain direction.
    [Action("SetNpcFacingDirection")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string? npcName, out error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(args, 2, out var facingDirection, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_SetNpcFacingDirection <Name> <int>";
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        npc.faceDirection(facingDirection)

        return true;
    }
}