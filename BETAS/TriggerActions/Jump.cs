using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.TriggerActions;

public static class Jump
{
    // Make an NPC or the Farmer jump up into the air with a specified velocity.
    [Action("Jump")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizable(args, 1, out string? npcName, out error, defaultValue: "All") ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableFloat(args, 2, out float jumpVelocity, out error, defaultValue: 4f) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 3, out int yVelocity, out error, defaultValue: 0) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 4, out int xVelocity, out error, defaultValue: 0))
        {
            error = "Usage: Spiderbuttons.BETAS_Jump [NPC/Farmer] [Velocity] [Vertical Speed] [Horizontal Speed]";
            return false;
        }

        if (npcName.EqualsIgnoreCase("Farmer"))
        {
            Game1.player.jump(jumpVelocity);
            Game1.player.setTrajectory(xVelocity, yVelocity);
            return true;
        }

        if (npcName.EqualsIgnoreCase("All"))
        {
            foreach (var chara in Game1.currentLocation.EventCharactersIfPossible().Where(chara => chara.IsVillager))
            {
                chara.jump(jumpVelocity);
            }
            return true;
        }

        var npc = Game1.getCharacterFromName(npcName)?.EventActorIfPossible();
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        npc.jump(jumpVelocity);
        return true;
    }
}