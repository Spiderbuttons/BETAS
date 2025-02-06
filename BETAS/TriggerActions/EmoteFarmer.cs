using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class EmoteFarmer
{
    // Make the current farmer perform an emote.
    [Action("EmoteFarmer")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizableInt(args, 1, out int emote, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_EmoteFarmer <EmoteId>";
            return false;
        }

        Game1.player.doEmote(emote, 24);
        return true;
    }
}