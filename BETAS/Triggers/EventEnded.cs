using BETAS.Attributes;
using HarmonyLib;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class EventEnded
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.startEvent))]
        public static void GameLocation_startEvent_Postfix(GameLocation __instance, Event evt)
        {
            evt.onEventFinished += () =>
            {
                Item eventItem = ItemRegistry.Create(evt.id ?? "Unknown");
                eventItem.modData["BETAS/EventEnded/WasFestival"] = (!string.IsNullOrWhiteSpace(evt.FestivalName)).ToString(); // evt.isFestival is already changed to false by the time the onEventFinished delegate fires.
                eventItem.modData["BETAS/EventEnded/WasWedding"] = evt.isWedding.ToString();
                eventItem.modData["BETAS/EventEnded/WasMemory"] = evt.isMemory.ToString();
                eventItem.modData["BETAS/EventEnded/WasSkipped"] = evt.skipped.ToString();
                
                TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_EventEnded", targetItem: eventItem, inputItem: eventItem, location: __instance);
            };
        }
    }
}