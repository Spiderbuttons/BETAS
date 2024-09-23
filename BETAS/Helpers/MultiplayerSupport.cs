using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Mods;
using StardewValley.Triggers;

namespace BETAS.Helpers;

public class MultiplayerSupport
{
    public class TriggerPackage
    {
        public string TriggerName { get; set; }
        
        public string TargetItemID { get; set; } 
        public SerializableDictionary<string, string> TargetItemData { get; set; } = [];
        
        public string InputItemID { get; set; }
        public SerializableDictionary<string, string> InputItemData { get; set; } = [];
        
        public string Location { get; set; }
        public long PlayerID { get; set; }
        
        public TriggerPackage(string triggerName, Item targetItem, Item inputItem, string location, long player)
        {
            TriggerName = triggerName;
            TargetItemID = targetItem?.ItemId;
            InputItemID = inputItem?.ItemId;
            Location = location;
            PlayerID = player;
            
            if (targetItem != null)
            {
                foreach (var data in targetItem.modData.Pairs)
                {
                    TargetItemData[data.Key] = data.Value;
                }
            }
            
            if (inputItem != null)
            {
                foreach (var data in inputItem.modData.Pairs)
                {
                    InputItemData[data.Key] = data.Value;
                }
            }
        }
    }
    
    public static void BroadcastTrigger(TriggerPackage triggerPackage)
    {
        if (Context.IsMainPlayer)
        {
            BETAS.ModHelper.Multiplayer.SendMessage(triggerPackage, "TriggerPackage", new[] { BETAS.Manifest.UniqueID });
        }
    }

    public static void ReceiveTrigger(object? sender, ModMessageReceivedEventArgs e)
    {
        if (e.FromModID != BETAS.Manifest.UniqueID || Context.IsMainPlayer || e.Type != "TriggerPackage") return;

        var triggerPackage = e.ReadAs<TriggerPackage>();
        if (triggerPackage == null) return;

        Item targetItem = triggerPackage.TargetItemID != null ? ItemRegistry.Create(triggerPackage.TargetItemID) : null;
        Item inputItem = triggerPackage.InputItemID != null ? ItemRegistry.Create(triggerPackage.InputItemID) : null;
        Log.Debug("Got here");
        GameLocation location = triggerPackage.Location != null ? Game1.getLocationFromName(triggerPackage.Location) : Game1.currentLocation;
        Farmer player = triggerPackage.PlayerID != 0 ? Game1.getFarmer(triggerPackage.PlayerID) : Game1.player;
        
        if (targetItem != null)
        {
            foreach (var data in triggerPackage.TargetItemData)
            {
                targetItem.modData[data.Key] = data.Value;
            }
        }
        
        if (inputItem != null)
        {
            foreach (var data in triggerPackage.InputItemData)
            {
                inputItem.modData[data.Key] = data.Value;
            }
        }

        Log.Alert($"Received Trigger: {triggerPackage.TriggerName}");
        TriggerActionManager.Raise(triggerPackage.TriggerName, targetItem: targetItem, inputItem: inputItem, location: location, player: player);
    }
}