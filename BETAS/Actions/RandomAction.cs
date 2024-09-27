using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Triggers;

namespace BETAS.Actions;

public static class RandomAction
{
    // Choose a random action string from a list of given actions to run.
    [Action("RandomAction")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var _, out error, allowBlank: false))
        {
            error = "Usage: RandomAction <Action String>+";
            return false;
        }
        
        var r = Utility.CreateRandom(Game1.currentGameTime.TotalGameTime.TotalMilliseconds);
        
        var actions = new string[args.Length - 1];
        Array.Copy(args, 1, actions, 0, actions.Length);
        var action = actions[r.Next(actions.Length)];
        
        return TriggerActionManager.TryRunAction(action, out error, out Exception ex);
    }
}