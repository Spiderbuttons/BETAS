using System;
using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;

namespace BETAS.TriggerActions;

public static class MakeMachineReady
{
    // Make a number of machines instantly ready their output in a given location.
    [Action("MakeMachineReady")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGetOptionalTokenizable(args, 1, out string? machineID, out error, defaultValue: "Any") ||
            !TokenizableArgUtility.TryGetOptionalTokenizable(args, 2, out string? outputID, out error, defaultValue: "Any") ||
            !TokenizableArgUtility.TryGetOptionalTokenizable(args, 3, out string? locationName,
                out error, defaultValue: "All") ||
            !TokenizableArgUtility.TryGetOptionalTokenizableInt(args, 4, out int count, out error, defaultValue: -1))
        {
            error = "Usage: Spiderbuttons.BETAS_MakeMachineReady [Machine ID] [Output ID] [Location Name] [Count]";
            return false;
        }

        Utility.ForEachLocation(delegate(GameLocation location)
        {
            if (locationName != "All" && !string.Equals(location.Name, locationName, StringComparison.OrdinalIgnoreCase))
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

                    if (machineID != "Any" && machine.QualifiedItemId != machineID)
                        continue;

                    if (machine.heldObject.Value == null ||
                        (outputID != "Any" && machine.heldObject.Value.QualifiedItemId != outputID))
                        continue;

                    machine.MinutesUntilReady = 10;
                    StardewValley.DelayedAction.functionAfterDelay(delegate { machine.minutesElapsed(10); }, 50);
                    count--;
                }
            }

            if (count == 0)
                return true;

            foreach (var machine in location.Objects.Values)
            {
                if (count == 0)
                    break;

                if (machineID != "Any" && machine.QualifiedItemId != machineID)
                    continue;

                if (machine.heldObject.Value == null ||
                    (outputID != "Any" && machine.heldObject.Value.QualifiedItemId != outputID))
                    continue;

                machine.MinutesUntilReady = 10;
                StardewValley.DelayedAction.functionAfterDelay(delegate { machine.minutesElapsed(10); }, 50);
                count--;
            }

            return true;
        });

        return true;
    }
}