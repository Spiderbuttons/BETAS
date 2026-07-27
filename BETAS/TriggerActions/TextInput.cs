using BETAS.AdvancedPermissions;
using BETAS.Attributes;
using BETAS.Helpers;
using BETAS.Menus;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class TextInput
{
    // Open a generic text input menu that stores the input in mod data.
    [Action("TextInput")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? modDataKey, out error, name: "string Key") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 2, out bool local, out error, defaultValue: true, name: "bool Local?") ||
            !TokenizableArgUtility.TryGetOptional(args, 3, out string? title, out error, allowBlank: true, name: "string Title") ||
            !TokenizableArgUtility.TryGetOptional(args, 4, out string? defaultText, out error, allowBlank: true, name: "string Default Text") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 5, out int minChars, out error, defaultValue: -1, name: "int #Min") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 6, out int maxChars, out error, defaultValue: -1, name: "int #Max") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 7, out bool numbersOnly, out error, defaultValue: false, name: "bool Numbers Only?"))
        {
            return false;
        }
        
        GenericTextInputMenu menu = new GenericTextInputMenu(
            afterInput: text =>
            {
                if (local)
                    Game1.player.modData[modDataKey] = text;
                else
                    Game1.getFarm().modData[modDataKey] = text;
            },
            title: title,
            defaultText: defaultText,
            modDataKey: modDataKey,
            minChars: minChars,
            maxChars: maxChars,
            numbersOnly: numbersOnly,
            prevMenu: Game1.activeClickableMenu
        );
        
        Game1.activeClickableMenu = menu;
        return true;
    }
    
    // Open a generic text input menu that stores the input in GLOBAL mod data, if possible.
    [Action("GlobalTextInput")]
    public static bool Action_Global(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? uniqueId, out error, name: "string UniqueID") ||
            !TokenizableArgUtility.TryGet(args, 2, out string? modDataKey, out error, name: "string Key") ||
            !TokenizableArgUtility.TryGetOptional(args, 3, out string? title, out error, allowBlank: true, name: "string Title") ||
            !TokenizableArgUtility.TryGetOptional(args, 4, out string? defaultText, out error, allowBlank: true, name: "string Default Text") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 5, out int minChars, out error, defaultValue: -1, name: "int #Min") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 6, out int maxChars, out error, defaultValue: -1, name: "int #Max") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 7, out bool numbersOnly, out error, defaultValue: false, name: "bool Numbers Only?"))
        {
            return false;
        }
        
        var mod = BETAS.ModRegistry.Get(uniqueId);
        if (mod == null)
        {
            error = $"No mod found with unique ID '{uniqueId}'";
            return false;
        }
        
        if (!mod.HasPermission(Permissions.GlobalModData))
        {
            error = $"Mod with UniqueID '{mod.Manifest.UniqueID}' has not enabled the 'GlobalModData' permission in its manifest";
            return false;
        }
        
        GenericTextInputMenu menu = new GenericTextInputMenu(
            afterInput: text =>
            {
                if (!GlobalModData.TryWriteGlobalModData(mod, modDataKey, text, out var writeError))
                {
                    Log.Error($"Failed to write global mod data: {writeError}");
                }
            },
            title: title,
            defaultText: defaultText,
            modDataKey: modDataKey,
            minChars: minChars,
            maxChars: maxChars,
            numbersOnly: numbersOnly,
            prevMenu: Game1.activeClickableMenu
        );
        
        Game1.activeClickableMenu = menu;
        return true;
    }
}