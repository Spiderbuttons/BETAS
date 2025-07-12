using System.Diagnostics.CodeAnalysis;
using StardewModdingAPI;
using StardewModdingAPI.Framework;

namespace BETAS.AdvancedPermissions;

public static class Logging
{
    public static bool TryLogAsMod(IModMetadata mod, string message, LogLevel level, [NotNullWhen(false)] out string? error)
    {
        error = null;
        if (!mod.HasPermission(Permissions.Logging))
        {
            error = $"Mod with UniqueID '{mod.Manifest.UniqueID}' has not enabled the 'Logging' permission in its manifest.";
            return false;
        }
        
        mod.Monitor?.Log(message, level);
        return true;
    }
}