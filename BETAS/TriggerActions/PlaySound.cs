using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class PlaySound
{
    // Play a sound.
    [Action("PlaySound")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var cueName, out error, allowBlank: true) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableBool(args, 2, out var local, out error, defaultValue: false))
        {
            error = "Usage: Spiderbuttons.BETAS_PlaySound <Sound> [Everyone?]";
            return false;
        }

        if (local) Game1.player.playNearbySoundLocal(cueName);
        else Game1.player.playNearbySoundAll(cueName);

        return true;
    }
}