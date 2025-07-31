using System.Collections.Generic;
using Netcode;
using StardewValley;

namespace BETAS.Helpers;

public static class GameLocationExtensions
{
    public static IList<NPC> EventCharactersIfPossible(this GameLocation location)
    {
        if (!Game1.eventUp) return location.characters;
        return location.currentEvent.actors;
    }
}