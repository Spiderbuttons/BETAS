using StardewValley;

namespace BETAS.Helpers;

public static class NpcExtensions
{
    public static NPC EventActorIfPossible(this NPC npc)
    {
        if (!Game1.eventUp) return npc;
        return Game1.CurrentEvent?.getActorByName(npc.Name) ?? npc;
    }
}