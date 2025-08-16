using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class ChangeMusicTrack
{
    // Change the current music to the requested music track.
    [Action("ChangeMusicTrack")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(args, 1, out var track, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_ChangeMusicTrack <Track>";
            return false;
        }

        Game1.changeMusicTrack(track);
        return true;
    }
}