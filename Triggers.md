# TRIGGERS

Triggers are `Raised` whenever a specific event occurs in the game. Unmodded Stardew Valley includes three triggers: DayStarted, DayEnding, and LocationChanged. BETAS adds 17 new Triggers to the game that your mod can react to.

In order to provide more information about the event that occured, most of the triggers will also create an item to send along with the trigger as the `Target` item. This item will sometimes be an ordinary Stardew Valley Item. Other times, it will be a fake item that only exists for the purpose of the trigger, which I will simply call a `Trigger Item`. The documentation for each trigger will specify whether the `Target` item is just a `Trigger Item` or if it is an actual item. If it is a `Trigger Item`, it may still contain useful data in its ordinary fields, which will be shown as necessary. You will be able to use [the standard item Game State Queries](https://stardewvalleywiki.com/Modding:Game_state_queries#For_items_only) to get information from the `Target` item along with [the new item Game State Queries added by BETAS](GameStateQueries.md).

Most often, the `Target` item will contain information about the trigger event stored in its mod data and will require the use of the new item Game State Queries added by BETAS to access said information. The `Target` item will often include boolean values in specific mod data entries. These are the ones that start with words like 'Was' or 'Is' like they are asking a yes or no question. These entries will **_always_** contain a value of either `true` or `false` no matter what, unlike non-boolean values that may or may not exist at all if they are not relevant.

The `Target` location of the trigger will be wherever the trigger occured or the event was witnessed, usually the current location of the local player.

Please keep in mind that all triggers will look for these events from the perspective of the **current, local player.** This means that if you are playing multiplayer, the triggers will not apply if anyone other than yourself witnesses or causes the event in question.

All Triggers, both vanilla or otherwise, can be used to perform the [new Actions added by BETAS](Actions.md).

### List of Triggers:
- [AnimalPetted](#animalpetted)
- [BombExploded](#bombexploded)
- [CropHarvested](#cropharvested)
- [DamageTaken](#damagetaken)
- [DialogueOpened](#dialogueopened)
- [ExperienceGained](#experiencegained)
- [FishCaught](#fishcaught)
- [FloraShaken](#florashaken)
- [GarbageChecked](#garbagechecked)
- [GiftGiven](#giftgiven)
- [LetterRead](#letterread)
- [LightningStruck](#lightningstruck)
- [MinecartUsed](#minecartused)
- [MonsterKilled](#monsterkilled)
- [NpcKissed](#npckissed)
- [PassedOut](#passedout)
- [RelationshipChanged](#relationshipchanged)

<br>

### Example Usage:
```json
{
  "Format": "2.3.0",
  "Changes": [
    {
      "Action": "EditData",
      "Target": "Data/TriggerActions",
      "Entries": {
        "ExampleTrigger_One": {
          "Id": "ExampleTrigger_One",
          "Trigger": "Spiderbuttons.BETAS_CropHarvested",
          "Condition": "ITEM_QUALITY Target 2, ITEM_CATEGORY Target -75 -79",
          "Action": "Spiderbuttons.BETAS_MakeMachineReady -1 -1 IslandWest 4"
        },
        
        "ExampleTrigger_Two": {
          "Id": "ExampleTrigger_Two",
          "Trigger": "Spiderbuttons.BETAS_MonsterKilled",
          "Condition": "ITEM_ID Target \"Green Slime\", Spiderbuttons.BETAS_ITEM_MOD_DATA_CONTAINS Target BETAS/MonsterKilled/Drops (O)766",
          "Action": "Spiderbuttons.BETAS_EmoteFarmer 32",
          "MarkActionApplied": false
        },
        
        "ExampleTrigger_Three": {
          "Id": "ExampleTrigger_Three",
          "Trigger": "Spiderbuttons.BETAS_LightningStruck",
          "Condition": null,
          "Actions": [
            "Spiderbuttons.BETAS_SetNewDialogue Haley \"I hate lightning...\" false",
            "Spiderbuttons.BETAS_WarpNpc Abigail NPC:Emily 25 24 2"
          ]
        }
      }
    }
  ]
}
```

<br>

* * *

## ANIMAL PETTED <a name="animalpetted"></a>

Raised whenever the local player pets an animal, whether it is a farm animal or a pet.

|     TRIGGER     |    Spiderbuttons.BETAS_AnimalPetted     |
|:---------------|:---------------------------------------|
|  Target Player  |     Player that petted the animal.      |
| Target Location | Location of the animal that was petted. |
|   Target Item   |              Trigger Item               |
<br>

#### TARGET ITEM:
| Field  |                            Value                             |                                                Usage Notes |
|:-------|:------------------------------------------------------------|:-----------------------------------------------------------|
| ItemId | The type of the animal that was petted (i.e. `Cat` or `Cow`) |                                                            |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                      |                                                                Value                                                                 | Type           |                                                Usage Notes |
|:----------------------------------|:------------------------------------------------------------------------------------------------------------------------------------|:---------------|:-----------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}AnimalPetted\color{Cerulean}/Name}}`$           |                                                       The name of the animal.                                                        | String         |                                                            |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}AnimalPetted\color{Cerulean}/Friendship}}`$     |                                   The friendship of the animal towards the player that petted it.                                    | Integer        |                                             |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}AnimalPetted\color{Cerulean}/Breed}}`$          |                                                    What breed of animal this is.                                                     | String         |         This value will only exist if the animal is a pet. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}AnimalPetted\color{Cerulean}/Happiness}}`$      |                                                     The happiness of the animal.                                                     | Integer        | This value will only exist if the animal is a farm animal. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}AnimalPetted\color{Cerulean}/ProduceQuality}}`$ | The quality level of the produce this animal creates. Possible values are `0` (normal), `1` (silver), `2` (gold), and `4` (iridium). | Integer        | This value will only exist if the animal is a farm animal. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}AnimalPetted\color{JungleGreen}/WasPet}}`$         |                                      Whether or not the animal was a pet and not a farm animal.                                      | Boolean        |                                                            |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}AnimalPetted\color{JungleGreen}/WasFarmAnimal}}`$  |                                      Whether or not the animal was a farm animal and not a pet.                                      | Boolean        |                                                            |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}AnimalPetted\color{JungleGreen}/WasBaby}}`$        |                                             Whether or not the animal was a baby or not.                                             | Boolean        |                       This value is always false for pets. |
<br>

* * *
## BOMB EXPLODED <a name="bombexploded"></a>

Raised whenever a bomb explodes, including when that bomb is a Hot Head.

|     TRIGGER     |    Spiderbuttons.BETAS_BombExploded    |
|:---------------|:--------------------------------------|
|  Target Player  | Player that caused the bomb explosion. |
| Target Location |   Location where the bomb exploded.    |
|   Target Item   |              Trigger Item              |
<br>

#### TARGET ITEM:
| Field  | Value  | Usage Notes                                                                  |
|:-------|:------|:------------------------------------------------------------------------------|
| ItemId | `(O)287` | This value will always be the same regardless of what type of bomb exploded. |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key              |            Value             | Type     |    Usage Notes |
|:--------------------------|:----------------------------|:---------|:---------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}BombExploded\color{Cerulean}/Radius}}`$ | The radius of the explosion. | Integer  | |
<br>

* * *
## CROP HARVESTED <a name="cropharvested"></a>

Raised whenever a crop is harvested, whether by a Farmer or by a Junimo.

|     TRIGGER     |    Spiderbuttons.BETAS_CropHarvested     |
|:---------------|:----------------------------------------|
|  Target Player  |            The local player.             |
| Target Location | Location of the crop that was harvested. |
|   Target Item   |       The crop that was harvested.       |
<br>

#### TARGET ITEM:
The `Target` item in this case is the crop that was dug up. Therefore, you can use ordinary item game state queries to check things like its item ID, quality, stack size (if multiple crops were dug up from e.g. potatoes), etc.
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                             |                          Value                           | Type     | Usage Notes |
|:-----------------------------------------|:--------------------------------------------------------|:---------|:------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}CropHarvested\color{JungleGreen}/WasHarvestedByJunimo}}`$ | Whether or not it was a Junimo that harvested this crop. | Boolean  |             |
<br>

* * *
## DAMAGE TAKEN <a name="damagetaken"></a>

Raised whenever the local player takes damage.

|     TRIGGER     |    Spiderbuttons.BETAS_DamageTaken    |
|:---------------|:-------------------------------------|
|  Target Player  |           The local player.           |
| Target Location | Location where the damage was taken.  |
|   Target Item   |              Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |                    Value                     |                                                                        Usage Notes |
|:-------|:--------------------------------------------|:-----------------------------------------------------------------------------------|
| ItemId | The name of the monster that did the damage. | If the damage came from a source other than a monster, the value will be `Unknown` |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                                                                                  | Value                                                         | Type     |                                                                Usage Notes |
|:----------------------------------------------------------------------------------------------|:--------------------------------------------------------------|:---------|:---------------------------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}DamageTaken\color{Cerulean}/Damage}}`$      | The amount of damage that the damage source tried to inflict. | Integer  | This does not take defense or other modifiers into account. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}DamageTaken\color{JungleGreen}/WasParried}}`$ | Whether or not the player parried the damage.                 | Boolean  |                                                                            |
<br>

* * *
## DIALOGUE OPENED <a name="dialogueopened"></a>

Raised whenever the local player opens a dialogue box/starts a conversation with an NPC.

|     TRIGGER     |    Spiderbuttons.BETAS_DialogueOpened    |
|:---------------|:----------------------------------------|
|  Target Player  |           The local player.              |
| Target Location | Location of the NPC that was spoken to.  |
|   Target Item   |              Trigger Item                |
<br>

#### TARGET ITEM:
| Field  |                       Value                        | Usage Notes |
|:-------|:--------------------------------------------------|:------------|
| ItemId |    The name of the NPC that is being spoken to.    |             |
| Stack  | How many lines of dialogue the speaker has to say. |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                         |                                            Value                                            | Type    |                                                                                                                               Usage Notes |
|:-------------------------------------|:-------------------------------------------------------------------------------------------|:---------|:------------------------------------------------------------------------------------------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}DialogueOpened\color{Cerulean}/Age}}`$             |    The age of the NPC being spoken to. Possible values are `Adult`, `Teen`, or `Child`.     | String  |                                                                                                                                           |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}DialogueOpened\color{Cerulean}/Gender}}`$          | The gender of the NPC being spoken to. Possible values are `Female`, `Male`, or `Undefined` | String  |                                                                                                                                           |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}DialogueOpened\color{Cerulean}/Friendship}}`$      |                     The current friendship for the NPC being spoken to.                     | Integer |                                                                                                                             |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}DialogueOpened\color{JungleGreen}/WasDatingFarmer}}`$ |       Whether or not the player was dating this NPC when the dialogue box was opened.       | Boolean | This may not behave as expected when giving a relationship-changing item (e.g. bouquet). See [RelationshipChanged](#relationshipchanged). |
<br>

* * *
## EXPERIENCE GAINED <a name="experiencegained"></a>

Raised whenever the local player gains experience in a skill.

|     TRIGGER     | Spiderbuttons.BETAS_ExperienceGained |
|:---------------|:------------------------------------|
|  Target Player  |          The local player.           |
| Target Location |    Location of the local player.     |
|   Target Item   |             Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |                                                              Value                                                               |                     Usage Notes |
|:-------|:--------------------------------------------------------------------------------------------------------------------------------|:--------------------------------|
| ItemId | The name of the skill that the experience was for. Possible values are `Farming`, `Fishing`, `Foraging`, `Mining`, and `Combat`. | SpaceCore skills not supported. |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                      |                            Value                            | Type    |    Usage Notes |
|:----------------------------------|:-----------------------------------------------------------|:---------|:---------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}ExperienceGained\color{Cerulean}/Amount}}`$     |               How much experience was gained.               | Integer |  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}ExperienceGained\color{JungleGreen}/WasLevelUp}}`$ | Whether or not this experience gain resulted in a level up. | Boolean |                |
<br>

* * *
## FISH CAUGHT <a name="fishcaught"></a>

Raised whenever the local player catches a fish. This does _not_ include trash.

|     TRIGGER     |   Spiderbuttons.BETAS_FishCaught    |
|:---------------|:-----------------------------------|
|  Target Player  |          The local player.          |
| Target Location | Location where the fish was caught. |
|   Target Item   |      The fish that was caught.      |
<br>

#### TARGET ITEM:
The `Target` item in this case is the fish that was caught. Therefore, you can use ordinary item game state queries to check things like its item ID, quality, stack size (if multiple fish were caught), etc.
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                     |                           Value                            | Type    |    Usage Notes |
|:---------------------------------|:----------------------------------------------------------|:---------|:---------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FishCaught\color{Cerulean}/Size}}`$            |           The size of the fish that was caught.            | Integer |  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FishCaught\color{Cerulean}/Difficulty}}`$      |     The difficulty value for the fish that was caught.     | Integer |  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FishCaught\color{JungleGreen}/WasPerfect}}`$      |       Whether or not this fish was caught perfectly.       | Boolean |                |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FishCaught\color{JungleGreen}/WasLegendary}}`$    |         Whether or not this was a legendary fish.          | Boolean |                |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FishCaught\color{JungleGreen}/WasWithTreasure}}`$ | Whether or not the player also fished up a treasure chest. | Boolean |                |
<br>

* * *
## FLORA SHAKEN <a name="florashaken"></a>

Raised whenever the local player shakes a tree or bush.

|     TRIGGER     |    Spiderbuttons.BETAS_FloraShaken    |
|:---------------|:-------------------------------------|
|  Target Player  |           The local player.           |
| Target Location | Location of the flora that was shaken. |
|   Target Item   |              Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |                                              Value                                              |                                                                                                      Usage Notes |
|:-------|:-----------------------------------------------------------------------------------------------|:-----------------------------------------------------------------------------------------------------------------|
| ItemId | If it's a tree, it will be the type or ID of the tree. If it's a bush, the value will be `Bush` |                                                         Fruit tree IDs are the ID of the seed used to grow them. |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                      |                                                             Value                                                              | Type           |                                                                                                                                                                                                                                      Usage Notes |
|:----------------------------------|:------------------------------------------------------------------------------------------------------------------------------|:----------------|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{Cerulean}/Stage}}`$           |                                     The current growth stage of the flora that was shaken.                                     | Integer (0-15) |                                                                                                                                                                             This value will not exist for bushes. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{Cerulean}/Seed}}`$            |                                        The item ID of the seed used to grow this flora.                                        | String         |                                                                                                                                                                                                            This value will not exist for bushes. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{Cerulean}/Quality}}`$         | The quality of the produce this flora produces. Possible values are `0` (normal), `1` (silver), `2` (gold), and `4` (iridium). | Integer        |                                                                                                                                                                                                      This value will only exist for fruit trees. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{Cerulean}/Produce}}`$         |                                A list of the produce that was on this flora when it was shaken.                                | String         |                                                                                                                                                                                                   This value will not exist for non-fruit trees. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{Cerulean}/ProduceCount}}`$    |                                The number of produce that was on this flora when it was shaken.                                | Integer        |                                                                                                                                                                                        This value will only exist for fruit trees. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{Cerulean}/PossibleProduce}}`$ |                                  A list of produce that this flora can ever possibly produce.                                  | String         |                                                                                                                                                                                                      This value will only exist for fruit trees. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{Cerulean}/Size}}`$            |                            The size of this flora. Possible values are `0`, `1`, `2`, `3`, and `4`.                            | Integer        |                                                                 This value will only exist for bushes. Sizes `0`, `1`, and `2` are small, medium, and large bushes respectively. Size `3` is a green tea bush. Size `4` is a golden walnut bush. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{JungleGreen}/WasInSeason}}`$     |                                            Whether or not this flora is in season.                                             | Boolean        |                                              For non-fruit trees, this checks whether the tree can grow. For fruit trees, this checks whether or not the tree can currently produce fruit. For bushes, this checks whether the bush is in bloom. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{JungleGreen}/WasMossy}}`$        |                                              Whether or not this flora was mossy.                                              | Boolean        |                                                                                                                                                                                                                                                  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{JungleGreen}/WasSeedy}}`$        |                                 Whether or not this flora had a seed that fell out (I think).                                  | Boolean        |                                                                                                                                                                                                                                                  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{JungleGreen}/WasFertilized}}`$   |                                        Whether or not this flora was fertilized or not.                                        | Boolean        |                                                                                                                                                                                                                                                  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{JungleGreen}/WasTapped}}`$       |                                         Whether or not this flora had a tapper on it.                                          | Boolean        |                                                                                                                                                                                                                                                  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{JungleGreen}/WasTree}}`$         |                                             Whether or not this flora was a tree.                                              | Boolean        |                                                                                                                                                                                                                                                  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{JungleGreen}/WasFruitTree}}`$    |                                          Whether or not this flora was a fruit tree.                                           | Boolean        |                                                                                                                                                                                                                                                  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}FloraShaken\color{JungleGreen}/WasBush}}`$         |                                             Whether or not this flora was a bush.                                              | Boolean        |                                                                                                                                                                                                                                                  |
<br>

* * *
## GARBAGE CHECKED <a name="garbagechecked"></a>

Raised whenever the local player rummages through a garbage can.

|     TRIGGER     |                Spiderbuttons.BETAS_GarbageChecked                 |
|:---------------|:-----------------------------------------------------------------|
|  Target Player  |                         The local player.                         |
| Target Location |           Location of the garbage can that was checked.           |
|   Target Item   | The item the player found in the trash can OR just a Trigger Item |
<br>

#### TARGET ITEM:
| Field  |                 Value                 |                                                                                               Usage Notes |
|:-------|:-------------------------------------|:----------------------------------------------------------------------------------------------------------|
| ItemId | ID of the trash can that was checked. | Only if the player did not find an item in the trash. If they did, this will be the item ID of that item. |

As said above, if the player found an item in the trash can, the `Target` item for this trigger will be the item that was found. Therefore, you can use ordinary item game state queries to check things like its item ID, quality, stack size, etc.

<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                              |                              Value                               | Type    |                                           Usage Notes |
|:------------------------------------------|:----------------------------------------------------------------|:---------|:------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}GarbageChecked\color{Cerulean}/GarbageCanId}}`$         |           The ID of the garbage can that was checked.            | String  |                                                       |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}GarbageChecked\color{Cerulean}/Witnesses}}`$            | A list of NPCs that caught the player checking the garbage can.  | String  | If there are no witnesses, this value will not exist. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}GarbageChecked\color{JungleGreen}/WasMegaSuccess}}`$       |    Whether or not this garbage can check was a mega success.     | Boolean |                                                       |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}GarbageChecked\color{JungleGreen}/WasDoubleMegaSuccess}}`$ | Whether or not this garbage can check was a double mega success. | Boolean |                                                       |
<br>

* * *
## GIFT GIVEN <a name="giftgiven"></a>

Raised whenever the local player gives a gift to an NPC.

|     TRIGGER     |    Spiderbuttons.BETAS_GiftGiven     |
|:---------------|:------------------------------------|
|  Target Player  |          The local player.           |
| Target Location | Location of the NPC that was gifted. |
|   Target Item   |             Trigger Item             |
|   Input Item    |      The item that was gifted.       |
<br>

#### TARGET ITEM:
| Field  |                       Value                        | Usage Notes |
|:-------|:--------------------------------------------------|:------------|
| ItemId |    The name of the NPC that was given the gift.    |             |

#### INPUT ITEM:
The `Input` item in this case is the item that was given as a gift. Therefore, you can use ordinary item game state queries to check things like its item ID, quality, stack size, etc. It will not include any mod data in it.

<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                                                                                 |                                                             Value                                                              | Type    |                         Usage Notes |
|:---------------------------------------------------------------------------------------------|:------------------------------------------------------------------------------------------------------------------------------|:---------|:------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}GiftGiven\color{Cerulean}/Friendship}}`$   |                                    The friendship level of the NPC that was given the gift.                                    | Integer |                       |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}GiftGiven\color{Cerulean}/Taste}}`$        | The gift taste that the NPC had for the item. Possible values are `Love`, `Hate`, `Like`, `Dislike`, `Neutral`, and `Special`. | String  | `Special` is used for Stardrop Tea. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}GiftGiven\color{JungleGreen}/WasBirthday}}`$ |                               Whether or not it was the NPC's birthday when the gift was given.                                | Boolean |                                     |
<br>

* * *

## LETTER READ <a name="letterread"></a>

Raised whenever the local player opens their mailbox to read a letter. This trigger is _not_ raised if the player looks at a letter again in their Collections menu.

|     TRIGGER     |             Spiderbuttons.BETAS_LetterRead             |
|:---------------|:------------------------------------------------------|
|  Target Player  |                   The local player.                    |
| Target Location | Location of the mailbox that the letter was read from. |
|   Target Item   |                      Trigger Item                      |
<br>

#### TARGET ITEM:
| Field  |               Value                | Usage Notes |
|:-------|:----------------------------------|:------------|
| ItemId | The `Data/mail` key of the letter. |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                            |                                 Value                                  | Type    |                                                                   Usage Notes |
|:----------------------------------------|:----------------------------------------------------------------------|:---------|:------------------------------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LetterRead\color{Cerulean}/Money}}`$                  |               How much money was included in the letter.               | Integer | This value will not exist if there was no money in the letter. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LetterRead\color{Cerulean}/Quest}}`$                  |             The quest ID that was attached to the letter.              | String  |                This value will not exist if there was no quest in the letter. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LetterRead\color{Cerulean}/SpecialOrder}}`$           |         The special order ID that was attached to the letter.          | String  |        This value will not exist if there was no special order in the letter. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LetterRead\color{JungleGreen}/WasRecipe}}`$              |   Whether or not the letter contained a cooking or crafting recipe.    | Boolean |                                                                               |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LetterRead\color{JungleGreen}/WasQuestOrSpecialOrder}}`$ | Whether or not the letter contained either a quest or a special order. | Boolean |                                                                               |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LetterRead\color{JungleGreen}/WasWithItem}}`$            |           Whether or not the letter had an attached item(s).           | Boolean |                                                                               |
<br>

* * *
## LIGHTNING STRUCK <a name="lightningstruck"></a>

Raised whenever the player witnesses a lightning strike.

|     TRIGGER     | Spiderbuttons.BETAS_LightningStruck |
|:---------------|:-----------------------------------|
|  Target Player  |          The local player.          |
| Target Location |    Location of the local player.    |
|   Target Item   |            Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |       Value        | Usage Notes |
|:-------|:------------------|:------------|
| ItemId | `Lightning Strike` |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                               |                                                      Value                                                      | Type    |                                                                  Usage Notes |
|:-------------------------------------------|:---------------------------------------------------------------------------------------------------------------|:---------|:-----------------------------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LightningStruck\color{Cerulean}/Size}}`$                 |                    The size of the lightning strike. Possible values are `Big` and `Small`.                     | String  |                                                                              |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LightningStruck\color{Cerulean}/StruckTerrainFeature}}`$ | The type of terrain feature that was struck by lightning. Possible values are `FruitTree`, `Crop`, and `Grass`. | String  | This value will not exist if lightning did not strike one of these features. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LightningStruck\color{Cerulean}/StruckCrop}}`$           |                        The item ID of the crop that was struck by this lightning strike.                        | String  |                This value will not exist if lightning did not strike a crop. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LightningStruck\color{Cerulean}/StruckTree}}`$           |                     The tree ID of the fruit tree that was struck by this lightning strike.                     | String  |          This value will not exist if lightning did not strike a fruit tree. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}LightningStruck\color{JungleGreen}/WasLightningRod}}`$      |                          Whether or not this lightning strike struck a lightning rod.                           | Boolean |                                                                              |
<br>

* * *
## MINECART USED <a name="minecartused"></a>

Raised whenever the local player uses the minecart to travel somewhere.

|     TRIGGER     | Spiderbuttons.BETAS_MinecartUsed |
|:---------------|:--------------------------------|
|  Target Player  |        The local player.         |
| Target Location |    The destination location.     |
|   Target Item   |              None.               |
<br>

This trigger does not pass any item, fake or otherwise, to the trigger context. The only value it passes is the destination location that the player is traveling to, which is the `Target` location. Check the `Here` location to see where the player is leaving from.

<br>

* * *
## MONSTER KILLED <a name="monsterkilled"></a>

Raised whenever the local player kills a monster.

|     TRIGGER     | Spiderbuttons.BETAS_MonsterKilled |
|:---------------|:---------------------------------|
|  Target Player  |         The local player.         |
| Target Location | Location of the monster killed.   |
|   Target Item   |           Trigger Item            |
<br>

#### TARGET ITEM:
| Field  |                  Value                   |                                                           Usage Notes |
|:-------|:----------------------------------------|:----------------------------------------------------------------------|
| ItemId | The name of the monster that was killed. |                                                                       |
| Stack  | The number of items the monster dropped. | This does not take into account extra drops like books, Qi gems, etc. |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                           |                                Value                                | Type    |    Usage Notes |
|:---------------------------------------|:-------------------------------------------------------------------|:---------|:---------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}MonsterKilled\color{Cerulean}/MaxHealth}}`$          |         The maximum health of the monster that was killed.          | Integer |  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}MonsterKilled\color{Cerulean}/Damage}}`$             | How much damage the monster that was killed would do to the player. | Integer |  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}MonsterKilled\color{Cerulean}/Drops}}`$              |   A list of item IDs that the monster dropped when it was killed.   | String  |                |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}MonsterKilled\color{JungleGreen}/WasHardmodeMonster}}`$ |             Whether or not this was a hardmode monster.             | Boolean |                |
<br>

* * *
## NPC KISSED <a name="npckissed"></a>

Raised whenever the local player kisses an NPC.

|     TRIGGER     |    Spiderbuttons.BETAS_NpcKissed     |
|:---------------|:------------------------------------|
|  Target Player  |          The local player.           |
| Target Location | Location of the NPC that was kissed. |
|   Target Item   |             Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |                Value                 | Usage Notes |
|:-------|:------------------------------------|:------------|
| ItemId | The name of the NPC that was kissed. |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                    |                                             Value                                             | Type    |    Usage Notes |
|:--------------------------------|:---------------------------------------------------------------------------------------------|:---------|:---------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}NpcKissed\color{Cerulean}/Age}}`$             |     The age of the NPC that was kissed. Possible values are `Adult`, `Teen`, and `Child`.     | String  |                |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}NpcKissed\color{Cerulean}/Gender}}`$          | The gender of the NPC that was kissed. Possible values are `Female`, `Male`, and `Undefined`. | String  |                |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}NpcKissed\color{Cerulean}/Friendship}}`$      |                      The friendship value for the player with this NPC.                       | Integer |  |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}NpcKissed\color{JungleGreen}/WasDatingFarmer}}`$ |              Whether or not the NPC was dating the farmer when they were kissed.              | Boolean |                |
<br>

* * *
## PASSED OUT <a name="passedout"></a>

Raised whenever the local player passes out either from exhaustion or from being up too late.

|     TRIGGER     |         Spiderbuttons.BETAS_PassedOut         |
|:---------------|:---------------------------------------------|
|  Target Player  |               The local player.               |
| Target Location | Location that the local player passed out in. |
|   Target Item   |                 Trigger Item                  |
<br>

#### TARGET ITEM:
| Field  |                       Value                        | Usage Notes |
|:-------|:--------------------------------------------------|:------------|
| ItemId | The name of the location the player passed out in. |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                 |                                          Value                                          | Type    |                                                                             Usage Notes |
|:-----------------------------|:---------------------------------------------------------------------------------------|:---------|:----------------------------------------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}PassedOut\color{Cerulean}/Time}}`$         |                       The time of day when the player passed out.                       | Integer |                                                              Standard `0600–2600` form. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}PassedOut\color{Cerulean}/Tool}}`$         |           The item ID of the tool that the player exhausted themselves with.            | String  | This value will only exist if the player passed out from exhaustion through tool usage. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}PassedOut\color{JungleGreen}/WasUpTooLate}}`$ |        Whether or not the player passed out because they stayed awake too late.         | Boolean |                                                                                         |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}PassedOut\color{JungleGreen}/WasExhausted}}`$ |         Whether or not the player passed out because they exhausted themselves.         | Boolean |                                                                                         |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}PassedOut\color{JungleGreen}/WasSafe}}`$      | Whether or not the player passed out in a safe location (e.g. FarmHouse or IslandWest). | Boolean |                                                                                         |
<br>

* * *
## RELATIONSHIP CHANGED <a name="relationshipchanged"></a>

Raised whenever the local player's relationship status (_not_ friendship value) with an NPC changes.

|     TRIGGER     |             Spiderbuttons.BETAS_RelationshipChanged             |
|:---------------|:---------------------------------------------------------------|
|  Target Player  |                        The local player.                        |
| Target Location | Location of the NPC whose relationship with the farmer changed. |
|   Target Item   |               Trigger Item (New Friendship Data)                |
|   Input Item    |               Trigger Item (Old Friendship Data)                |
<br>

#### TARGET ITEM:
| Field  |                      Value                      | Usage Notes |
|:-------|:-----------------------------------------------|:------------|
| ItemId | The name of the NPC whose relationship changed. |             |
<br>

#### INPUT ITEM:
| Field  |                      Value                      | Usage Notes |
|:-------|:-----------------------------------------------|:------------|
| ItemId | The name of the NPC whose relationship changed. |             |
<br>

In this trigger, the `Trigger Item` that is given as the `Target` will contain the friendship data for the NPC _after_ the relationship changes have already occured. The `Trigger Item` that is given as the `Input` will contain the friendship data for the NPC as it was _before_ the relationship change occured. Please keep in mind which one you are using when you are checking the friendship data.

#### TARGET ITEM MOD DATA:
| Mod Data Key                                                                                                |                                                             Value                                                             | Type    |                                                                                         Usage Notes |
|:------------------------------------------------------------------------------------------------------------|:-----------------------------------------------------------------------------------------------------------------------------|:---------|:----------------------------------------------------------------------------------------------------|
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/Status}}`$            |     The relationship status. Possible values are `Friendly`, `Dating`, `Engaged`, `Married`, `Roommate`, and `Divorced`.      | String  |                                                                                                     |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/Friendship}}`$        |                                               The friendship level for the NPC.                                               | Integer |                                                                                      |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/GiftsToday}}`$        |                                       The number of gifts the NPC has been given today.                                       | Integer |                                                                                       |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/GiftsThisWeek}}`$     |                                     The number of gifts the NPC has been given this week.                                     | Integer |                                                                                      |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/DaysSinceLastGift}}`$ |                            The number of days since the last time this NPC has been given a gift.                             | Integer |                                                                                       |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/WeddingSeason}}`$     | The name of the season the wedding with this NPC took place in. Possible values are `Spring`, `Summer`, `Fall`, and `Winter`. | String  |                  This value will only exist if the player had or will have a wedding with this NPC. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/WeddingDate}}`$       |                              The day of the month that the wedding with this NPC took place on.                               | Integer |   This value will only exist if the player had or will have a wedding with this NPC. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/WeddingYear}}`$       |                                The year number that the wedding with this place took place in.                                | Integer |   This value will only exist if the player had or will have a wedding with this NPC. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{Cerulean}/DaysMarried}}`$       |                             The number of days that the player has been married to this NPC for.                              | Integer |   This value will only exist if the player had or will have a wedding with this NPC. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{JungleGreen}/WasTalkedToToday}}`$ |                                         Whether or not this NPC was talked to today.                                          | Boolean |                                                                                                     |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{JungleGreen}/IsRoommate}}`$       |                                       Whether or not this NPC is the player's roommate.                                       | Boolean | `WasRoommate` will also work here, but remember this will be the _current_ status, not past status. |
| $`{\textsf{\color{White}BETAS/\color{RedOrange}RelationshipChanged\color{JungleGreen}/WasMemoryWiped}}`$   |               Whether or not this relationship change was the result of the player wiping their ex's memories.                | Boolean |                                                                                                     |
<br>

#### INPUT ITEM MOD DATA:
The input item shares the same mod data keys as the target item **with the exception of** `WasMemoryWiped`, which will not exist in the input item. Remember that the mod data in the `Input` item describes the friendship data with the NPC _before_ the relationship change occured.