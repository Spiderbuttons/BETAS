using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class AddExperience
{
    // Add an amount of experience to the current player for the given skill, or all skills if none specified.
    [Action("AddExperience")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetOptionalInt(args, 1, out var amount, out error, defaultValue: 1, name: "int #Amount") ||
            !TokenizableArgUtility.TryGetOptional(args, 2, out var skill, out error, name: "string Skill"))
        {
            return false;
        }

        if (skill is not null) skill = skill switch
        {
            "Farming" => "0",
            "Fishing" => "1",
            "Foraging" => "2",
            "Mining" => "3",
            "Combat" => "4",
            "Luck" => "5",
            _ => skill
        };

        if (skill is null)
        {
            Game1.player.gainExperience(0, amount);
            Game1.player.gainExperience(1, amount);
            Game1.player.gainExperience(2, amount);
            Game1.player.gainExperience(3, amount);
            Game1.player.gainExperience(4, amount);
            Game1.player.gainExperience(5, amount);

            if (BETAS.SCAPI is null) return true;
            foreach (var skillString in BETAS.SCAPI.GetCustomSkills())
            {
                BETAS.SCAPI.AddExperienceForCustomSkill(Game1.player, skillString, amount);
            }
            return true;
        }
        
        if (int.TryParse(skill, out var skillID))
        {
            Game1.player.gainExperience(skillID, amount);
            return true;
        }

        if (BETAS.SCAPI is not null)
        {
            BETAS.SCAPI.AddExperienceForCustomSkill(Game1.player, skill, amount);
            return true;
        }
        
        Log.Error($"Attempted to add experience for non-existent skill '{skill}.' If this is a SpaceCore skill, please make sure SpaceCore is installed.");
        return false;
    }
}