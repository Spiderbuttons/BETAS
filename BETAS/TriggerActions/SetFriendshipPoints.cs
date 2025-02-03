using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.Actions;

public static class SetFriendshipPoints
{
    // Set the friendship points the player has with an NPC.
    [Action("SetFriendshipPoints")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var npcName, out error, allowBlank: true) || !ArgUtilityExtensions.TryGetTokenizableInt(args, 2, out var points, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_SetFriendshipPoints <NPC Name> <Points>";
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
            friendship.Points = points < 0 ? 0 : points;
        }
        else
        {
            Game1.debugOutput = "Tried to change friendship for a friend that wasn't there.";
        }
        
        return true;
    }
}