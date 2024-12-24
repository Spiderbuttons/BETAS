using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.Actions;

public static class FadeToBlack
{
    // Fade the screen to black, keep it black for a certain duration, and then fade to clear.
    [Action("FadeToBlack")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 1, out var duration, out error, defaultValue: 1000))
        {
            error = "Usage: FadeToBlack <Duration>";
            return false;
        }

        Game1.globalFadeToBlack(() => {
            Game1.freezeControls = true;
            Game1.viewportFreeze = true;
            Game1.viewport.X = -10000;
            Game1.pauseThenDoFunction(duration, () => {
                Game1.globalFadeToClear();
                Game1.viewportFreeze = false;
                Game1.freezeControls = false;
            });
        });
        
        return true;
    }
}