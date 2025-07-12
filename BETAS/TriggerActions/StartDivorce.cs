using BETAS.Attributes;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class StartDivorce
{
    // Set the player to be divorced tonight.
    [Action("StartDivorce")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        error = null;
        Game1.player.divorceTonight.Value = true;
        return true;
    }
}