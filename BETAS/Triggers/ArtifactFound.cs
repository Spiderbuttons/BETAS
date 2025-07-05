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
    static class ArtifactFound
    {
        public static void Initialize()
        {
            BETAS.ModHelper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }
        
        public static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.archaeologyFound.OnValueAdded += OnArtifactFound;
        }

        public static void OnArtifactFound(string artifact, int[] count)
        {
            var item = ItemRegistry.Create(artifact);
            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_ArtifactFound", inputItem: item, targetItem: item);
        }
    }
}