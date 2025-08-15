using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.MapActions;

[MapAction]
// Run a Trigger Action action (e.g. AddMail or Spiderbuttons.BETAS_DialogueBox)
public static class Action
{
    public static bool TileAction(GameLocation location, string[] args, Farmer player, Point point)
    {
        string action = string.Join(" ", args[1..]);
        if (!TriggerActionManager.TryRunAction(action, out string error, out _))
        {
            Log.Error(error);
            return false;
        }
        
        return true;
    }

    public static void TouchAction(GameLocation location, string[] args, Farmer player, Vector2 position)
    {
        TileAction(location, args, player, position.ToPoint());
    }
}