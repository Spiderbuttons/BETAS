using StardewValley;

namespace BETAS.APIs
{
    public interface ISpaceCoreApi
    {
        /// <summary>
        /// Add EXP to a custom skill
        /// </summary>
        /// <param name="farmer"> The farmer who you want to give exp to</param>
        /// <param name="skill"> The string ID of the custom skill</param>
        /// <param name="amt"> The int Amount you want to give</param>
        void AddExperienceForCustomSkill(Farmer farmer, string skill, int amt);
    }
}