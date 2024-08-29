using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BETAS.Attributes;
using BETAS.Helpers;
using HarmonyLib;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Triggers;

namespace BETAS.Triggers
{
    [Trigger]
    [HarmonyPatch]
    static class AnimalPetted
    {
        public static void Trigger(Pet pet, Farmer petter)
        {
            var petItem = ItemRegistry.Create(pet.petType.Value);
            petItem.modData["BETAS/AnimalPetted/Name"] = pet.Name;
            petItem.modData["BETAS/AnimalPetted/Breed"] = pet.whichBreed.Value;
            petItem.modData["BETAS/AnimalPetted/Friendship"] = pet.friendshipTowardFarmer.Value.ToString();
            petItem.modData["BETAS/AnimalPetted/WasPet"] = "true";
            petItem.modData["BETAS/AnimalPetted/WasFarmAnimal"] = "false";
            petItem.modData["BETAS/AnimalPetted/WasBaby"] = "false";

            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_AnimalPetted", targetItem: petItem,
                location: pet.currentLocation, player: petter);
        }

        public static void Trigger(FarmAnimal animal, Farmer petter)
        {
            var animalItem = ItemRegistry.Create(animal.type.Value);
            animalItem.modData["BETAS/AnimalPetted/Name"] = animal.Name;
            animalItem.modData["BETAS/AnimalPetted/Friendship"] = animal.friendshipTowardFarmer.Value.ToString();
            animalItem.modData["BETAS/AnimalPetted/Happiness"] = animal.happiness.Value.ToString();
            animalItem.modData["BETAS/AnimalPetted/ProduceQuality"] = animal.produceQuality.ToString();
            animalItem.modData["BETAS/AnimalPetted/WasPet"] = "false";
            animalItem.modData["BETAS/AnimalPetted/WasFarmAnimal"] = "true";
            animalItem.modData["BETAS/AnimalPetted/WasBaby"] = animal.isBaby() ? "true" : "false";

            TriggerActionManager.Raise($"{BETAS.Manifest.UniqueID}_AnimalPetted", targetItem: animalItem,
                location: animal.currentLocation, player: petter);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Pet), nameof(Pet.checkAction))]
        public static IEnumerable<CodeInstruction> Transpiler_Pets(IEnumerable<CodeInstruction> instructions,
            ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Pet), nameof(Pet.playContentSound)))
                ).Repeat((codeMatcher) =>
                {
                    codeMatcher.Advance(1).Insert(
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Call,
                            AccessTools.Method(typeof(AnimalPetted), nameof(Trigger), [typeof(Pet), typeof(Farmer)])));
                });

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.AnimalPetted_Pet_checkAction_Transpiler: \n" + ex);
                return code;
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(FarmAnimal), nameof(FarmAnimal.pet))]
        public static IEnumerable<CodeInstruction> Transpiler_FarmAnimals(IEnumerable<CodeInstruction> instructions,
            ILGenerator il)
        {
            var code = instructions.ToList();
            try
            {
                var matcher = new CodeMatcher(code, il);

                matcher.MatchEndForward(
                    new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.makeSound)))
                ).Repeat((codeMatcher) =>
                {
                    codeMatcher.Advance(1).Insert(
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Call,
                            AccessTools.Method(typeof(AnimalPetted), nameof(Trigger),
                                [typeof(FarmAnimal), typeof(Farmer)])));
                });

                return matcher.InstructionEnumeration();
            }
            catch (Exception ex)
            {
                Log.Error("Error in BETAS.AnimalPetted_FarmAnimal_pet_Transpiler: \n" + ex);
                return code;
            }
        }
    }
}