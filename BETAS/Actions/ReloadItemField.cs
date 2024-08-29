using System;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Menus;
using Object = StardewValley.Object;

namespace BETAS.Actions;

public static class ReloadItemField
{
    // Reload one or more fields on every item ID in the game (i.e. price) to overwrite cached values.
    [Action("ReloadItemField")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string itemId, out error,
                allowBlank: false) ||
            !ArgUtilityExtensions.TryGetTokenizable(args, 2, out var _, out error))
        {
            error = "Usage: ReloadItemField <ItemId> <Field>+";
            return false;
        }
        
        var refItem = ItemRegistry.Create(itemId);
        if (refItem == null)
        {
            error = "No item found with ID '" + itemId + "'";
            return false;
        }
        
        Utility.ForEachItem((item) =>
        {
            if (item.QualifiedItemId == refItem.QualifiedItemId)
            {
                ArgUtilityExtensions.ForEachArg(args, 2, (field) =>
                {
                    var type = refItem.GetType();
                    var value = type.GetProperty(field)?.GetValue(refItem);
                    type.GetProperty(field)?.SetValue(item, value);
                    if (field.Equals("SpriteIndex")) item.ResetParentSheetIndex();
                });
            }
            return true;
        });
        
        return true;
    }
}