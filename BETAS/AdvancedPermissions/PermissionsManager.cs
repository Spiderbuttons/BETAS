using System;
using System.Collections.Generic;
using BETAS.Helpers;
using BETAS.Models;
using Newtonsoft.Json.Linq;
using StardewModdingAPI.Framework;

namespace BETAS.AdvancedPermissions;

[Flags]
public enum Permissions
{
    None = 0,
    GlobalModData = 1 << 0,
    Logging = 1 << 1,
}

public static class PermissionsManager
{
    private static readonly Dictionary<IModMetadata, Permissions> _permissionsCache = new();
    
    public static Permissions ParsePermissionsFromManifest(IModMetadata? mod)
    {
        if (mod is null)
        {
            throw new ArgumentNullException(nameof(mod), "Mod metadata cannot be null.");
        }
        
        if (_permissionsCache.TryGetValue(mod, out Permissions cachedPermissions))
        {
            return cachedPermissions;
        }

        if (!mod.Manifest.ExtraFields.TryGetValue("Spiderbuttons.BETAS", out object? raw))
        {
            return Permissions.None;
        }
        
        if ((raw as JObject)?.ToObject<ManifestData>() is not { } manifestData) return Permissions.None;

        Permissions modPermissions = Permissions.None;
        foreach (var permission in manifestData.AdvancedPermissions)
        {
            if (Enum.TryParse(permission, true, out Permissions parsedPermission))
            {
                modPermissions |= parsedPermission;
            }
            else Log.Warn($"Unknown advanced permission '{permission}' in mod '{mod.Manifest.UniqueID}' (should be one of {string.Join(", ", Enum.GetNames(typeof(Permissions)))})");
        }
        _permissionsCache[mod] = modPermissions;
        return modPermissions;
    }
    
    public static bool HasPermission(this IModMetadata? mod, Permissions permission)
    {
        Permissions modPermissions = ParsePermissionsFromManifest(mod);
        return modPermissions != Permissions.None && (modPermissions & permission) != 0;
    }
    
    public static Permissions GetPermissions(this IModMetadata? mod)
    {
        return ParsePermissionsFromManifest(mod);
    }
}