using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.Actions;

public static class Message
{
    // Make the current farmer perform an emote.
    [Action("Message")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var message, out error, allowBlank: true))
        {
            Log.Error(error);
            return false;
        }
        if (!string.IsNullOrWhiteSpace(message))
        {
            Game1.drawDialogueNoTyping(message);
            return true;
        }

        return false;
    }
}