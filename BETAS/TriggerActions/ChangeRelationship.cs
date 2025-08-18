using System;
using BETAS.Attributes;
using BETAS.Helpers;
using BETAS.Triggers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class ChangeRelationship
{
    // Change the current farmer's relationship with an NPC.
    [Action("ChangeRelationship")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out string? npcName, out error, name: "string NPC") ||
            !TokenizableArgUtility.TryGetOptionalEnum(args, 2, out FriendshipStatus relation, out error, name: "RelationshipEnum Relationship") ||
            !TokenizableArgUtility.TryGetOptionalBool(args, 3, out bool roommates, out error, defaultValue: false, name: "bool Roommates?") ||
            !TokenizableArgUtility.TryGetOptionalInt(args, 4, out int daysAway, out error, defaultValue: 3, name: "int #Wedding Delay"))
        {
            return false;
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }
        
        if (!Game1.player.friendshipData.TryGetValue(npc.Name, out var friendship))
        {
            friendship = Game1.player.friendshipData[npc.Name] = new Friendship();
        }

        if (friendship.Status == relation) return true;

        var oldFriendship = RelationshipChanged.FriendlyCloner(friendship);
        
        switch (relation) 
        {
            case FriendshipStatus.Friendly:
                break;
            case FriendshipStatus.Dating:
                Game1.Multiplayer.globalChatInfoMessage("Dating", Game1.player.Name, npc.GetTokenizedDisplayName());
                Game1.player.autoGenerateActiveDialogueEvent("dating_" + npc.Name);
                Game1.player.autoGenerateActiveDialogueEvent("dating");
                Game1.player.changeFriendship(25, npc);
                break;
            case FriendshipStatus.Engaged:
                if (Game1.player.HouseUpgradeLevel < 1)
                {
                    error = "Cannot set relationship to 'Engaged' without a house upgrade.";
                    return false;
                }
                Game1.player.spouse = npc.Name;
                if (!roommates) Game1.Multiplayer.globalChatInfoMessage("Engaged", Game1.player.Name, npc.GetTokenizedDisplayName());
                friendship.RoommateMarriage = roommates;
                WorldDate weddingDate = new WorldDate(Game1.Date);
                weddingDate.TotalDays += Math.Max(daysAway, 1);
                while (!Game1.canHaveWeddingOnDay(weddingDate.DayOfMonth, weddingDate.Season)) weddingDate.TotalDays++;
                friendship.WeddingDate = weddingDate;
                Game1.player.changeFriendship(1, npc);
                break;
            case FriendshipStatus.Married:
                if (Game1.player.HouseUpgradeLevel < 1)
                {
                    error = "Cannot set relationship to 'Married' without a house upgrade.";
                    return false;
                }
                Game1.player.spouse = npc.Name;
                friendship.WeddingDate = new WorldDate(Game1.Date);
                if (!roommates) Game1.Multiplayer.globalChatInfoMessage("Married", Game1.player.Name, npc.GetTokenizedDisplayName());
                friendship.RoommateMarriage = roommates;
                Game1.prepareSpouseForWedding(Game1.player);
                if (Game1.player.getSpouse() is { } spouse && !spouse.isRoommate())
                {
                    Game1.player.autoGenerateActiveDialogueEvent("married_" + Game1.player.spouse);
                    if (!Game1.player.autoGenerateActiveDialogueEvent("married")) Game1.player.autoGenerateActiveDialogueEvent("married_twice");
                }
                else Game1.player.autoGenerateActiveDialogueEvent("roommates_" + Game1.player.spouse);
                break;
            case FriendshipStatus.Divorced:
                if (Game1.player.spouse != npc.Name) break;
                Game1.player.doDivorce();
                npc.PerformDivorce();
                break;
            default:
                error = $"Somehow received an invalid relationship status '{relation}'";
                return false;
        }
        
        if (relation != FriendshipStatus.Dating) Game1.player.removeDatingActiveDialogueEvents(npc.Name);
        if (relation is not FriendshipStatus.Engaged and not FriendshipStatus.Married && Game1.player.spouse == npc.Name)
        {
            Game1.player.spouse = null;
        }
        
        friendship.Status = relation;
        
        RelationshipChanged.Trigger(friendship, oldFriendship!, npc, Game1.player);
        return true;
    }
}