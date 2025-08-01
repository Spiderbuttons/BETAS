﻿using System.Linq;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.TriggerActions;

public static class ShakeNpc
{
    // Make an NPC perform an emote.
    [Action("ShakeNpc")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetOptionalTokenizable(args, 1, out string? npcName, out error,
                defaultValue: "All") ||
            !ArgUtilityExtensions.TryGetOptionalTokenizableInt(args, 2, out int duration, out error,
                defaultValue: 1000))
        {
            error = "Usage: Spiderbuttons.BETAS_ShakeNpc [NPC Name] [Duration]";
            return false;
        }

        if (npcName.EqualsIgnoreCase("All"))
        {
            foreach (var chara in Game1.currentLocation.EventCharactersIfPossible().Where(chara => chara.IsVillager))
            {
                chara.EventActorIfPossible().shake(duration);
            }

            return true;
        }

        var npc = Game1.getCharacterFromName(npcName)?.EventActorIfPossible();
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }

        npc.shake(duration);
        return true;
    }
}