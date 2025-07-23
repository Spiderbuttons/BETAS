using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class AnimateNpc
{
    // Make an NPC perform animation.
    [Action("AnimateNpc")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out string? npcName, out error) ||
            !ArgUtilityExtensions.TryGetTokenizableBool(args, 2, out bool flip, out error) ||
            !ArgUtilityExtensions.TryGetTokenizableBool(args, 3, out bool loop, out error) ||
            !ArgUtilityExtensions.TryGetTokenizableInt(args, 4, out int frameDuration, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_AnimateNpc <Name> <Flip?> <Loop?> <#FrameDuration> <#AnimationFrame>+";
            return false;
        }

        List<NpcSprite.AnimationFrame> animationFrames = new List<NpcSprite.AnimationFrame>();
        for (int i = 5; i < args.Length; i++)
        {
            if (!ArgUtility.TryGetInt(args, i, out var frame, out error))
            {
                error = "Usage: Spiderbuttons.BETAS_AnimateNpc <Name> <Flip?> <Loop?> <#FrameDuration> <#AnimationFrame>+";
                return;
            }
            animationFrames.Add(new NpcSprite.AnimationFrame(frame, frameDuration, secondaryArm: false, flip));
        }

        var npc = Game1.getCharacterFromName(npcName);
        if (npc == null)
        {
            error = "no NPC found with name '" + npcName + "'";
            return false;
        }
        npc.setCurrentAnimation(animationFrames);
        npc.loop = loop;

        return true;
    }
}