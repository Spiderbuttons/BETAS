using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.TriggerActions;

public static class RegionalFriendship
{
    // Add or remove friendship points for the current player with every NPC in a specified region.
    [Action("RegionalFriendship")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? region, out error,
                allowBlank: false) || !TokenizableArgUtility.TryGetInt(args, 2, out var amount, out error) ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 3, out var set, out error, defaultValue: false))
        {
            error = "Usage: Spiderbuttons.BETAS_RegionalFriendship <Region> <Amount> [Set?]";
            return false;
        }

        if (amount < 0 && set)
        {
            error = "Cannot set friendship points below 0.";
            return false;
        }

        Utility.ForEachLocation(delegate(GameLocation l)
        {
            foreach (NPC current in l.characters)
            {
                if (current.GetData() is not null && current.GetData().HomeRegion.EqualsIgnoreCase(region) &&
                    Game1.player.friendshipData.TryGetValue(current.Name, out var friendship))
                {
                    if (set) friendship.Points = amount;
                    else Game1.player.changeFriendship(amount, current);
                }
            }
            return true;
        });
        
        return true;
    }
}