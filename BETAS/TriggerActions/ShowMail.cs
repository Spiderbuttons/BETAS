using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Menus;

namespace BETAS.TriggerActions;

public static class ShowMail
{
    // Open a LetterViewerMenu
    [Action("ShowMail")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(args, 1, out string? mailId, out error) ||
            !TokenizableArgUtility.TryGetOptionalTokenizableBool(args, 2, out bool flag, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_ShowMail <Mail ID> [Flag?]";
            return false;
        }

        var mails = DataLoader.Mail(Game1.content);
        if (!mails.TryGetValue(mailId, out var mailText))
        {
            error = $"No mail with ID '{mailId}' found in Data/mail";
            return false;
        }

        try
        {
            Game1.activeClickableMenu = new LetterViewerMenu(mailText, mailId);
            if (flag) Game1.addMail(mailId, noLetter: true, sendToEveryone: false);
            return true;
        } catch (Exception ex)
        {
            error = $"Failed to open mail with ID '{mailId}': {ex.Message}";
            return false;
        }
    }
}