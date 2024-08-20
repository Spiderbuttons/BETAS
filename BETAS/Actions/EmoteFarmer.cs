using StardewValley;
using StardewValley.Delegates;

namespace BETAS.Actions;

public static class EmoteFarmer
{
    // Make the current farmer perform an emote.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtility.TryGetInt(args, 1, out int emote, out error))
        {
            error = "Usage: EmoteFarmer <EmoteId>";
            return false;
        }

        Game1.player.doEmote(emote, 24);
        return true;
    }
}