using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class EventStarted
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.startEvent))]
        public static void GameLocation_startEvent_Postfix(GameLocation __instance, Event evt)
        {
            Item eventItem = ItemRegistry.Create(evt.id ?? "Unknown");
            eventItem.modData["BETAS/EventStarted/WasFestival"] = (!string.IsNullOrWhiteSpace(evt.FestivalName)).ToString();
            eventItem.modData["BETAS/EventStarted/IsFestival"] = (!string.IsNullOrWhiteSpace(evt.FestivalName)).ToString();
            eventItem.modData["BETAS/EventStarted/WasWedding"] = evt.isWedding.ToString();
            eventItem.modData["BETAS/EventStarted/IsWedding"] = evt.isWedding.ToString();
            eventItem.modData["BETAS/EventStarted/WasMemory"] = evt.isMemory.ToString();
            eventItem.modData["BETAS/EventStarted/IsMemory"] = evt.isMemory.ToString();
                
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_EventStarted", targetItem: eventItem, inputItem: eventItem, location: __instance);
            ;
        }
    }
}