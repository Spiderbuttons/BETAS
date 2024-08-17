using System;
using System.Linq;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.TokenizableStrings;

namespace BETAS.Actions;

public static class MakeMachineReady
{
    // Make a number of machines instantly ready their output in a given location.
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtility.TryGetOptional(args, 1, out string machineID, out error, defaultValue: "-1") || !ArgUtility.TryGetOptional(args, 2, out string outputID, out error, defaultValue: "-1") || !ArgUtilityExtensions.TryGetOptionalPossiblyRelativeLocationName(args, 3, out string locationName, out error, defaultValue: "-1") || !ArgUtility.TryGetOptionalInt(args, 4, out int count, out error, defaultValue: -1))
        {
            error = "Usage: MakeMachineReady [Machine ID] [Output ID] [Location Name] [Count]";
            return false;
        }
        
        Utility.ForEachLocation(delegate(GameLocation location)
        {
            if (locationName != "-1" && !string.Equals(location.Name, locationName, StringComparison.OrdinalIgnoreCase))
                return false;
            
            foreach (var building in location.buildings)
            {
                var interior = building.GetIndoors();
                if (interior == null)
                    continue;
                foreach (var machine in building.GetIndoors().objects.Values)
                {
                    if (count == 0)
                        break;
                    
                    if (machineID != "-1" && machine.QualifiedItemId != machineID)
                        continue;
                    
                    if (machine.heldObject.Value == null || (outputID != "-1" && machine.heldObject.Value.QualifiedItemId != outputID))
                        continue;
                    
                    machine.MinutesUntilReady = 10;
                    DelayedAction.functionAfterDelay(delegate
                    {
                        machine.minutesElapsed(10);
                    }, 50);
                    count--;
                }
            }
            
            if (count == 0)
                return true;
            
            foreach (var machine in location.Objects.Values)
            {
                if (count == 0)
                    break;
                
                if (machineID != "-1" && machine.QualifiedItemId != machineID)
                    continue;
                
                if (machine.heldObject.Value == null || (outputID != "-1" && machine.heldObject.Value.QualifiedItemId != outputID))
                    continue;
                
                machine.MinutesUntilReady = 10;
                DelayedAction.functionAfterDelay(delegate
                {
                    machine.minutesElapsed(10);
                }, 50);
                count--;
            }

            return true;
        });
        
        return true;
    }
}