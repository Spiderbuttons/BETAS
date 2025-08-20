using System.Data;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Network.NetEvents;

namespace BETAS.GSQs;

public static class CheckAndSetMail
{
    // Check whether the current player has a specific mail flag and set it after the check.
    [GSQ("CHECK_AND_SET_MAIL")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGet(query, 1, out var flag, out var error, name: "string Mail ID") ||
            !TokenizableArgUtility.TryGetOptional(query, 2, out var mailbox, out error, defaultValue: "any", allowBlank: true, name: "string Mailbox Type"))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }
        
        bool hasFlag;

        switch (mailbox.ToLower())
        {
            case "mailbox":
                hasFlag = Game1.player.mailbox.Contains(flag);
                Game1.player.mailbox.Add(flag);
                break;
            case "tomorrow":
                hasFlag = Game1.player.mailForTomorrow.Contains(flag);
                Game1.player.mailForTomorrow.Add(flag);
                break;
            case "received":
                hasFlag = Game1.player.mailReceived.Contains(flag);
                Game1.player.mailReceived.Add(flag);
                break;
            case "any":
                hasFlag = Game1.player.mailbox.Contains(flag) ||
                          Game1.player.mailForTomorrow.Contains(flag) ||
                          Game1.player.mailReceived.Contains(flag);
                Game1.player.mailbox.Add(flag);
                Game1.player.mailForTomorrow.Add(flag);
                Game1.player.mailReceived.Add(flag);
                break;
            default:
                return GameStateQuery.Helpers.ErrorResult(query, "unknown mail type '" + mailbox + "'; expected 'Mailbox', 'Tomorrow', 'Received', or 'Any'");
        }

        return hasFlag;
    }
}