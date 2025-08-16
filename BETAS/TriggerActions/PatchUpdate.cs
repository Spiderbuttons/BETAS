using BETAS.Attributes;
using BETAS.Helpers;
using ContentPatcher;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class PatchUpdate
{
    // Make the current farmer perform an emote.
    [Action("PatchUpdate")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetOptionalTokenizable(args, 1, out string? modId, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_PatchUpdate [UniqueId]";
            return false;
        }

        if (BETAS.ModRegistry.Get("Pathoschild.ContentPatcher")?.Mod is not ModEntry cpEntry)
        {
            error = "Content Patcher is not installed.";
            return false;
        }
        
        cpEntry.CommandHandler.Handle($"update{(modId is not null ? $" {modId}" : "")}".Split(" "));
        return true;
    }
}