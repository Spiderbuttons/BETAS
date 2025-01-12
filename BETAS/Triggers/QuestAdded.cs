using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using Netcode;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Quests;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class QuestAdded
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.questLog.OnElementChanged += OnQuestAdded;
        }

        public static void OnQuestAdded(NetList<Quest, NetRef<Quest>> list, int index, Quest oldValue, Quest newValue)
        {
            if (newValue is null || list.Contains(newValue)) return;
            
            var rawQuest = Quest.GetRawQuestFields(newValue.id.Value);
            if (!ArgUtility.TryGet(rawQuest, 0, out var questType, out var error, allowBlank: false))
            {
                Log.Trace($"Could not get raw quest type from quest id {newValue.id.Value}. It's probably a daily quest. Error: {error}");
            }
            
            var questItem = ItemRegistry.Create(newValue.id.Value ?? newValue.questTitle);
            questItem.modData["BETAS/QuestAdded/Title"] = newValue.questTitle;
            questItem.modData["BETAS/QuestAdded/Description"] = newValue.questDescription;
            questItem.modData["BETAS/QuestAdded/Type"] = questType ?? newValue.questType.Value.ToString();
            questItem.modData["BETAS/QuestAdded/NextQuests"] = string.Join(",", newValue.nextQuests);
            if (newValue.GetDaysLeft() > 0) questItem.modData["BETAS/QuestAdded/DaysLeft"] = newValue.GetDaysLeft().ToString();
            if (newValue.moneyReward.Value > 0) questItem.modData["BETAS/QuestAdded/MoneyReward"] = newValue.moneyReward.Value.ToString();
            questItem.modData["BETAS/QuestAdded/WasDailyQuest"] = newValue.dailyQuest.Value.ToString();
            questItem.modData["BETAS/QuestAdded/WasSecretQuest"] = newValue.isSecretQuest().ToString();
            questItem.modData["BETAS/QuestAdded/WasTimedQuest"] = newValue.IsTimedQuest().ToString();
            
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_QuestAdded", targetItem: questItem);
        }
    }
}