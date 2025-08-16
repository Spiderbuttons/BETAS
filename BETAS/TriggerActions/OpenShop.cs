using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class OpenShop
{
    // Open a shop menu.
    [Action("OpenShop")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? shopId, out error) ||
            !TokenizableArgUtility.TryGetOptional(args, 2, out string? ownerName, out error, defaultValue: null))
        {
            error = "Usage: Spiderbuttons.BETAS_OpenShop <ShopId> [Owner]";
            return false;
        }

        if (Utility.TryOpenShopMenu(shopId, ownerName)) return true;
        
        error = $"Failed to open shop with ID '{shopId}'" + (ownerName != null ? $" for owner '{ownerName}'" : string.Empty);
        return false;
    }
}