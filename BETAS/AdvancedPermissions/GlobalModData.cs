using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Framework;
using StardewModdingAPI.Framework.ModHelpers;

namespace BETAS.AdvancedPermissions;

public static class GlobalModData
{
    public static bool TryWriteGlobalModData(IModMetadata mod, string key, string? value, [NotNullWhen(false)] out string? error)
    {
        error = null;
        if (!mod.HasPermission(Permissions.GlobalModData))
        {
            error = $"Mod with UniqueID '{mod.Manifest.UniqueID}' has not enabled the 'GlobalModData' permission in its manifest";
            return false;
        }

        try
        {
            if (mod.IsContentPack)
            {
                string path = Path.Combine(Constants.DataPath, ".smapi", "mod-data", mod.ContentPack!.Manifest.UniqueID.ToLower(), "betas.json");
                DataHelper helper = (BETAS.ModHelper.Data as DataHelper)!;
                if (!helper.JsonHelper.ReadJsonFileIfExists(path, out Dictionary<string, object>? data))
                {
                    data = new Dictionary<string, object>();
                }

                if (value is null)
                {
                    data.Remove(key);
                } else data[key] = value;
                helper.JsonHelper.WriteJsonFile(path, data);
            }
            else
            {
                var data = mod.Mod!.Helper.Data.ReadGlobalData<Dictionary<string, object>>("betas");
                data ??= new Dictionary<string, object>();
                if (value is null)
                {
                    data.Remove(key);
                }
                else data[key] = value;
                mod.Mod!.Helper.Data.WriteGlobalData("betas", data);
            }
        }
        catch (Exception ex)
        {
            error = $"Failed to write global mod data: {ex}";
            return false;
        }
        
        return true;
    }
    
    public static bool TryReadGlobalModData(IModMetadata mod, string key, [NotNullWhen(true)] out string? value, [NotNullWhen(false)] out string? error)
    {
        value = null;
        error = null;

        if (!mod.HasPermission(Permissions.GlobalModData))
        {
            error = $"Mod with UniqueID '{mod.Manifest.UniqueID}' has not enabled the 'GlobalModData' permission in its manifest";
            return false;
        }

        try
        {
            Dictionary<string, object?>? data;
            if (mod.IsContentPack)
            {
                string path = Path.Combine(Constants.DataPath, ".smapi", "mod-data", mod.ContentPack!.Manifest.UniqueID.ToLower(), "betas.json");
                DataHelper helper = (BETAS.ModHelper.Data as DataHelper)!;
                helper.JsonHelper.ReadJsonFileIfExists(path, out data);
            }
            else data = mod.Mod!.Helper.Data.ReadGlobalData<Dictionary<string, object?>>("betas");

            if (data is null)
            {
                error = $"No global mod data found for mod '{mod.Manifest.UniqueID}'";
                return false;
            }
            
            if (!data.TryGetValue(key, out var objValue))
            {
                error = $"Global mod data for mod '{mod.Manifest.UniqueID}' does not contain the key '{key}'";
                return false;
            }

            if (objValue is null)
            {
                error = $"Global mod data for mod '{mod.Manifest.UniqueID}' does not contain a valid value for the key '{key}'";
                return false;
            }
            
            value = objValue.ToString()!;
            return true;
        }
        catch (Exception ex)
        {
            error = $"Failed to read global mod data: {ex}";
            return false;
        }
    }
}