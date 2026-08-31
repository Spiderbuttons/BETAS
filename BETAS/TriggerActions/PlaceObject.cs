using BETAS.Attributes;
using BETAS.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.TriggerActions;

public static class PlaceObject
{
    // Place an object with the given ID at the given tile coordinates in the given location, optionally overwriting any existing object.
    [Action("PlaceObject")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        GameLocation? location = Game1.player.currentLocation;
        if (!TokenizableArgUtility.TryGet(args, 1, out string? itemId, out error, name: "string Item ID") ||
            !TokenizableArgUtility.TryGetOptionalLocation(args, 2, ref location, out error, defaultValue: Game1.player.currentLocation, name: "Location") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 3, out int tileX, out error, defaultValue: (int)Game1.player.Tile.X, name: "int X") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 4, out int tileY, out error, defaultValue: (int)Game1.player.Tile.Y, name: "int Y") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 5, out var overwrite, out error, defaultValue: false, name: "bool Overwrite?"))
        {
            return false;
        }

        var item = ItemRegistry.Create<Object>(ItemRegistry.QualifyItemId(itemId));
        item.IsSpawnedObject = !item.HasTypeBigCraftable();
        Vector2 tile = new Vector2(tileX, tileY);
        if (!location.tryPlaceObject(tile, item))
        {
            if (!overwrite) return false;
            
            location.Objects[tile] = item;

        }

        return true;
    }
}