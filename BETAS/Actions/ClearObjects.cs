using System;
using System.Collections.Generic;
using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;
using StardewValley.TerrainFeatures;

namespace BETAS.Actions;

public static class ClearObjects
{
    // Clear the objects (furniture, chests, etc) on a specific tile or all objects in a rectangle in a location according to class.
    [Action("ClearObjects")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizable(args, 1, out var location, out error, defaultValue: "Here") ||
            !ArgUtilityExtensions.TryGetOptionalTokenizable(args, 2, out var type, out error, defaultValue: "All") ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 3, out var topLeftX, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 4, out var topLeftY, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 5, out var bottomRightX, out error) ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 6, out var bottomRightY, out error))
        {
            error = "Usage: ClearObjects [Location] [Type] [TopLeftX] [TopLeftY] [BottomRightX] [BottomRightY]";
            return false;
        }

        var loc = location.EqualsIgnoreCase("Here") ? Game1.player.currentLocation : Game1.RequireLocation(location);
        
        if (type.EqualsIgnoreCase("!All")) return false;
        var negate = type.StartsWith("!");

        if (!ArgUtility.HasIndex(args, 3))
        {
            if (!negate)
            {
                foreach (var obj in loc.Objects.Pairs)
                {
                    if (obj.Value.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All"))
                    {
                        loc.Objects.Remove(obj.Key);
                    }
                }
                loc.furniture.RemoveWhere(furn => furn.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All"));
                return true;
            }
            type = type.Substring(1);
            foreach (var obj in loc.Objects.Pairs)
            {
                if (!obj.Value.GetType().Name.EqualsIgnoreCase(type))
                {
                    loc.Objects.Remove(obj.Key);
                }
            }
            loc.furniture.RemoveWhere(furn => !furn.GetType().Name.EqualsIgnoreCase(type));
            return true;
        }

        if (!ArgUtility.HasIndex(args, 5))
        {
            if (!negate)
            {
                foreach (var obj in loc.Objects.Pairs)
                {
                    if (obj.Key == new Vector2(topLeftX, topLeftY) &&
                        (obj.Value.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All")))
                    {
                        loc.Objects.Remove(obj.Key);
                    }
                }
                loc.furniture.RemoveWhere(furn => furn.TileLocation == new Vector2(topLeftX, topLeftY) &&
                                                  (furn.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All")));
                return true;
            }
            type = type.Substring(1);
            foreach (var obj in loc.Objects.Pairs)
            {
                if (obj.Key == new Vector2(topLeftX, topLeftY) &&
                    !obj.Value.GetType().Name.EqualsIgnoreCase(type))
                {
                    loc.Objects.Remove(obj.Key);
                }
            }
            loc.furniture.RemoveWhere(furn => furn.TileLocation == new Vector2(topLeftX, topLeftY) &&
                                              !furn.GetType().Name.EqualsIgnoreCase(type));
            return true;
        }

        if (!negate)
        {
            foreach (var obj in loc.Objects.Pairs)
            {
                if (obj.Key.X >= topLeftX && obj.Key.Y >= topLeftY &&
                    obj.Key.X <= bottomRightX && obj.Key.Y <= bottomRightY &&
                    (obj.Value.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All")))
                {
                    loc.Objects.Remove(obj.Key);
                }   
            }
            loc.furniture.RemoveWhere(furn => furn.TileLocation.X >= topLeftX && furn.TileLocation.Y >= topLeftY &&
                                              furn.TileLocation.X <= bottomRightX && furn.TileLocation.Y <= bottomRightY &&
                                              (furn.GetType().Name.EqualsIgnoreCase(type) || type.EqualsIgnoreCase("All")));
            return true;
        }
        
        type = type.Substring(1);
        foreach (var obj in loc.Objects.Pairs)
        {
            if (obj.Key.X >= topLeftX && obj.Key.Y >= topLeftY &&
                obj.Key.X <= bottomRightX && obj.Key.Y <= bottomRightY &&
                !obj.Value.GetType().Name.EqualsIgnoreCase(type))
            {
                loc.Objects.Remove(obj.Key);
            }
        }
        loc.furniture.RemoveWhere(furn => furn.TileLocation.X >= topLeftX && furn.TileLocation.Y >= topLeftY &&
                                          furn.TileLocation.X <= bottomRightX && furn.TileLocation.Y <= bottomRightY &&
                                          !furn.GetType().Name.EqualsIgnoreCase(type));
        
        return true;
    }
}