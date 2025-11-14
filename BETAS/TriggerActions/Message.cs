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
        if (!TokenizableArgUtility.TryGet(args, 1, out var message, out error, allowBlank: false, name: "string Text") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 2, out var typing, out error, defaultValue: false, name: "bool Typing?"))
        {
            return false;
        }
        
        if (!string.IsNullOrWhiteSpace(message))
        {
            try
            {
                var msg = Game1.content.LoadString(message);
                if (!typing) Game1.drawDialogueNoTyping(msg);
                else Game1.drawObjectDialogue(msg);
            }
            catch (Exception)
            {
                if (!typing) Game1.drawDialogueNoTyping(message);
                else Game1.drawObjectDialogue(message);
            }

            return true;
        }

        return false;
    }
}