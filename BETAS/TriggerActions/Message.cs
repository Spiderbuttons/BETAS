using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class Message
{
    // Makes a message box appear on the screen to display some text.
    [Action("Message")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(args, 1, out var message, out error, allowBlank: false))
        {
            error = "Usage: Spiderbuttons.BETAS_Message <Text>";
            return false;
        }
        
        if (!string.IsNullOrWhiteSpace(message))
        {
            try
            {
                var msg = Game1.content.LoadString(message);
                Game1.drawDialogueNoTyping(msg);
            }
            catch (Exception)
            {
                Game1.drawDialogueNoTyping(message);
            }

            return true;
        }

        return false;
    }
}