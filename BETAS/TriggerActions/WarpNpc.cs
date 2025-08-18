using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class WarpNpc
{
    // Warp an NPC to a specific map and X/Y coordinate, with optional facing direction.
    [Action("WarpNpc")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        GameLocation? location = Game1.player.currentLocation;
        if (!TokenizableArgUtility.TryGet(args, 1, out string? npcName, out error, allowBlank: false, name: "string NPC") ||
            !TokenizableArgUtility.TryGetLocation(args, 2, ref location, out error) ||
            !TokenizableArgUtility.TryGetInt(args, 3, out int x, out error, name: "int #X Coordinate") ||
            !TokenizableArgUtility.TryGetInt(args, 4, out int y, out error, name: "int #Y Coordinate") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 5, out int facingDirection, out error, 2, name: "int #Facing Direction"))
        {
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        Game1.warpCharacter(npc, location, new Vector2(x, y));
        npc.faceDirection(facingDirection);
        npc.controller = null;
        npc.Halt();
        return true;
    }
}