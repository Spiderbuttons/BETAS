using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using ContentPatcher;
using HarmonyLib;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.Actions;

public static class PatchUpdate
{
    // Make the current farmer perform an emote.
    [Action("PatchUpdate")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizable(args, 1, out string modId, out error))
        {
            error = "Usage: PatchUpdate [UniqueId]";
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