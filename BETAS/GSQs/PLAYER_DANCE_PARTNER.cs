using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.GSQs;

public static class PlayerDancePartner
{
    // Check whether the player has a dance partner, and optionally whether it matches a given NPC name.
    [GSQ("PLAYER_DANCE_PARTNER")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!TokenizableArgUtility.TryGetTokenizable(query, 1, out var playerKey, out var error) ||
            !TokenizableArgUtility.TryGetOptionalTokenizable(query, 2, out var npc, out error, defaultValue: null))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        return GameStateQuery.Helpers.WithPlayer(context.Player, playerKey, farmer => {
            var partner = farmer.dancePartner.GetCharacter();
        
            if (partner == null) return false;
            if (npc is null or "Any") return true;   
            if (npc.EqualsIgnoreCase("Farmer")) return partner is Farmer;
            return partner.Name.EqualsIgnoreCase(npc);
        });
        
    }
}