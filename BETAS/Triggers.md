# TRIGGERS

Triggers are `Raised` whenever a specific event occurs in the game. Unmodded Stardew Valley includes three triggers: DayStarted, DayEnding, and LocationChanged. BETAS adds 17 new triggers to the game that your mod can react to.

In order to provide more information about the event that occured, most of the triggers will also create an item to send along with the trigger as the `Target` item. This item will sometimes be an ordinary Stardew Valley Item. Other times, it will be a fake item that only exists for the purpose of the trigger, which I will simply call a `Trigger Item`. The documentation for each trigger will specify whether the `Target` item is just a `Trigger Item` or if it is an actual item. If it is a `Trigger Item`, it may still contain useful data in its ordinary fields, which will be shown as necessary. You will be able to use [the standard item Game State Queries](https://stardewvalleywiki.com/Modding:Game_state_queries#For_items_only) to get information from the `Target` item along with [the new item Game State Queries added by BETAS](GameStateQueries.md).

Most often, the `Target` item will contain information about the trigger event stored in its mod data and will require the use of the new item Game State Queries added by BETAS to access said information.

The local player will most often be the `Target` of the trigger as well. Finally, the `Target` location of the trigger will be wherever the trigger occured or the event was witnessed (which, you guessed it, is most often the current location of the local player).

> The `Target` item will often include boolean values in specific mod data entries. These are the ones that start with words like 'Was' or 'Did,' like they are asking a yes or no question. These entries will _always_ contain a value of either `true` or `false` no matter what, unlike non-boolean values that may or may not exist at all if they are not relevant.

Please keep in mind that all triggers will look for these events from the perspective of the **current, local player.** This means that if you are playing multiplayer, some of the triggers will not apply if anyone other than yourself causes the event to happen.

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

## ANIMAL PETTED <a name="animalpetted"></a>

Raised whenever the local player pets an animal, whether it is a farm animal or a pet.

|     TRIGGER     |    Spiderbuttons.BETAS_AnimalPetted     |
|:---------------:|:---------------------------------------:|
|  Target Player  |     Player that petted the animal.      |
| Target Location | Location of the animal that was petted. |
|   Target Item   |              Trigger Item               |
<br>

#### TARGET ITEM:
| Field  |                            Value                             |                                                Usage Notes |
|:-------|:------------------------------------------------------------:|-----------------------------------------------------------:|
| ItemId | The type of the animal that was petted (i.e. `Cat` or `Cow`) |                                                            |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                      |                                                                Value                                                                 |                                                Usage Notes |
|:----------------------------------|:------------------------------------------------------------------------------------------------------------------------------------:|-----------------------------------------------------------:|
| BETAS/AnimalPetted/Name           |                                                       The name of the animal.                                                        |                                                            |
| BETAS/AnimalPetted/Friendship     |                                   The friendship of the animal towards the player that petted it.                                    |                                             Integer value. |
| BETAS/AnimalPetted/Breed          |                                                    What breed of animal this is.                                                     |         This value will only exist if the animal is a pet. |
| BETAS/AnimalPetted/Happiness      |                                                     The happiness of the animal.                                                     | This value will only exist if the animal is a farm animal. |
| BETAS/AnimalPetted/ProduceQuality | The quality level of the produce this animal creates. Possible values are `0` (normal), `1` (silver), `2` (gold), and `4` (iridium). | This value will only exist if the animal is a farm animal. |
| BETAS/AnimalPetted/WasPet         |                                      Whether or not the animal was a pet and not a farm animal.                                      |                                                            |
| BETAS/AnimalPetted/WasFarmAnimal  |                                      Whether or not the animal was a farm animal and not a pet.                                      |                                                            |
| BETAS/AnimalPetted/WasBaby        |                                             Whether or not the animal was a baby or not.                                             |                       This value is always false for pets. |
<br>

* * *
## BOMB EXPLODED <a name="bombexploded"></a>

Raised whenever a bomb explodes, including when that bomb is a Hot Head.

|     TRIGGER     |    Spiderbuttons.BETAS_BombExploded    |
|:---------------:|:--------------------------------------:|
|  Target Player  | Player that caused the bomb explosion. |
| Target Location |   Location where the bomb exploded.    |
|   Target Item   |              Trigger Item              |
<br>

#### TARGET ITEM:
| Field  | Value  | Usage Notes |
|:-------|:------:|------------:|
| ItemId | (O)287 |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key              |            Value             |    Usage Notes |
|:--------------------------|:----------------------------:|---------------:|
| BETAS/BombExploded/Radius | The radius of the explosion. | Integer value. |
<br>

* * *
## CROP HARVESTED <a name="cropharvested"></a>

Raised whenever a crop is harvested, whether by a Farmer or by a Junimo.

|     TRIGGER     |    Spiderbuttons.BETAS_CropHarvested     |
|:---------------:|:----------------------------------------:|
|  Target Player  |            The local player.             |
| Target Location | Location of the crop that was harvested. |
|   Target Item   |       The crop that was harvested.       |
<br>

#### TARGET ITEM:
The `Target` item in this case is the crop that was dug up. Therefore, you can use ordinary item game state queries to check things like its item ID, quality, stack size (if multiple crops were dug up from e.g. potatoes), etc.
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                             |                          Value                           | Usage Notes |
|:-----------------------------------------|:--------------------------------------------------------:|------------:|
| BETAS/CropHarvested/WasHarvestedByJunimo | Whether or not it was a Junimo that harvested this crop. |             |
<br>

* * *
## DAMAGE TAKEN <a name="damagetaken"></a>

Raised whenever the local player takes damage.

|     TRIGGER     |    Spiderbuttons.BETAS_DamageTaken    |
|:---------------:|:-------------------------------------:|
|  Target Player  |           The local player.           |
| Target Location | Location where the damage was taken.  |
|   Target Item   |              Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |                    Value                     |                                                                        Usage Notes |
|:-------|:--------------------------------------------:|-----------------------------------------------------------------------------------:|
| ItemId | The name of the monster that did the damage. | If the damage came from a source other than a monster, the value will be `Unknown` |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                 |                             Value                             |                                                                Usage Notes |
|:-----------------------------|:-------------------------------------------------------------:|---------------------------------------------------------------------------:|
| BETAS/DamageTaken/Damage     | The amount of damage that the damage source tried to inflict. | Integer value. This does not take defense or other modifiers into account. |
| BETAS/DamageTaken/WasParried |         Whether or not the player parried the damage.         |                                                                            |
<br>

* * *
## DIALOGUE OPENED <a name="dialogueopened"></a>

Raised whenever the local player opens a dialogue box/starts a conversation with an NPC.

|     TRIGGER     |    Spiderbuttons.BETAS_DialogueOpened    |
|:---------------:|:----------------------------------------:|
|  Target Player  |           The local player.              |
| Target Location | Location of the NPC that was spoken to.  |
|   Target Item   |              Trigger Item                |
<br>

#### TARGET ITEM:
| Field  |                       Value                        | Usage Notes |
|:-------|:--------------------------------------------------:|------------:|
| ItemId |    The name of the NPC that is being spoken to.    |             |
| Stack  | How many lines of dialogue the speaker has to say. |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                         |                                            Value                                            |                                                                                                                               Usage Notes |
|:-------------------------------------|:-------------------------------------------------------------------------------------------:|------------------------------------------------------------------------------------------------------------------------------------------:|
| BETAS/DialogueOpened/Age             |    The age of the NPC being spoken to. Possible values are `Adult`, `Teen`, or `Child`.     |                                                                                                                                           |
| BETAS/DialogueOpened/Gender          | The gender of the NPC being spoken to. Possible values are `Female`, `Male`, or `Undefined` |                                                                                                                                           |
| BETAS/DialogueOpened/Friendship      |                     The current friendship for the NPC being spoken to.                     |                                                                                                                            Integer value. |
| BETAS/DialogueOpened/WasDatingFarmer |       Whether or not the player was dating this NPC when the dialogue box was opened.       | This may not behave as expected when giving a relationship-changing item (e.g. bouquet). See [RelationshipChanged](#relationshipchanged). |
<br>

* * *
## EXPERIENCE GAINED <a name="experiencegained"></a>

Raised whenever the local player gains experience in a skill.

|     TRIGGER     | Spiderbuttons.BETAS_ExperienceGained |
|:---------------:|:------------------------------------:|
|  Target Player  |          The local player.           |
| Target Location |    Location of the local player.     |
|   Target Item   |             Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |                                                              Value                                                               |                     Usage Notes |
|:-------|:--------------------------------------------------------------------------------------------------------------------------------:|--------------------------------:|
| ItemId | The name of the skill that the experience was for. Possible values are `Farming`, `Fishing`, `Foraging`, `Mining`, and `Combat`. | SpaceCore skills not supported. |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                      |                            Value                            |    Usage Notes |
|:----------------------------------|:-----------------------------------------------------------:|---------------:|
| BETAS/ExperienceGained/Amount     |               How much experience was gained.               | Integer value. |
| BETAS/ExperienceGained/WasLevelUp | Whether or not this experience gain resulted in a level up. |                |
<br>

* * *
## FISH CAUGHT <a name="fishcaught"></a>

Raised whenever the local player catches a fish. This does _not_ include trash.

|     TRIGGER     |   Spiderbuttons.BETAS_FishCaught    |
|:---------------:|:-----------------------------------:|
|  Target Player  |          The local player.          |
| Target Location | Location where the fish was caught. |
|   Target Item   |      The fish that was caught.      |
<br>

#### TARGET ITEM:
The `Target` item in this case is the fish that was caught. Therefore, you can use ordinary item game state queries to check things like its item ID, quality, stack size (if multiple fish were caught), etc.
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                     |                           Value                            |    Usage Notes |
|:---------------------------------|:----------------------------------------------------------:|---------------:|
| BETAS/FishCaught/Size            |           The size of the fish that was caught.            | Integer value. |
| BETAS/FishCaught/Difficulty      |     The difficulty value for the fish that was caught.     | Integer value. |
| BETAS/FishCaught/WasPerfect      |       Whether or not this fish was caught perfectly.       |                |
| BETAS/FishCaught/WasLegendary    |         Whether or not this was a legendary fish.          |                |
| BETAS/FishCaught/WasWithTreasure | Whether or not the player also fished up a treasure chest. |                |
<br>

* * *
## FLORA SHAKEN <a name="florashaken"></a>

Raised whenever the local player shakes a tree or bush.

|     TRIGGER     |    Spiderbuttons.BETAS_FloraShaken    |
|:---------------:|:-------------------------------------:|
|  Target Player  |           The local player.           |
| Target Location | Location of the flora that was shaken. |
|   Target Item   |              Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |                                              Value                                              |                                                                                                      Usage Notes |
|:-------|:-----------------------------------------------------------------------------------------------:|-----------------------------------------------------------------------------------------------------------------:|
| ItemId | If it's a tree, it will be the type or ID of the tree. If it's a bush, the value will be `Bush` |                                                         Fruit tree IDs are the ID of the seed used to grow them. |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                      |                                                             Value                                                              |                                                                                                                                                                                                                                      Usage Notes |
|:----------------------------------|:------------------------------------------------------------------------------------------------------------------------------:|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| BETAS/FloraShaken/Stage           |                                     The current growth stage of the flora that was shaken.                                     |                Integer value between 0 and 15.                                                                                                                                                             This value will not exist for bushes. |
| BETAS/FloraShaken/Seed            |                                        The item ID of the seed used to grow this flora.                                        |                                                                                                                                                                                                            This value will not exist for bushes. |
| BETAS/FloraShaken/Quality         | The quality of the produce this flora produces. Possible values are `0` (normal), `1` (silver), `2` (gold), and `4` (iridium). |                                                                                                                                                                                                      This value will only exist for fruit trees. |
| BETAS/FloraShaken/Produce         |                                A list of the produce that was on this flora when it was shaken.                                |                                                                                                                                                                                                   This value will not exist for non-fruit trees. |
| BETAS/FloraShaken/ProduceCount    |                                The number of produce that was on this flora when it was shaken.                                | Integer value.                                                                                                                                                                                       This value will only exist for fruit trees. |
| BETAS/FloraShaken/PossibleProduce |                                  A list of produce that this flora can ever possibly produce.                                  |                                                                                                                                                                                                      This value will only exist for fruit trees. |
| BETAS/FloraShaken/Size            |                            The size of this flora. Possible values are `0`, `1`, `2`, `3`, and `4`.                            |                                                                 This value will only exist for bushes. Sizes `0`, `1`, and `2` are small, medium, and large bushes respectively. Size `3` is a green tea bush. Size `4` is a golden walnut bush. |
| BETAS/FloraShaken/WasInSeason     |                                            Whether or not this flora is in season.                                             |                                              For non-fruit trees, this checks whether the tree can grow. For fruit trees, this checks whether or not the tree can currently produce fruit. For bushes, this checks whether the bush is in bloom. |
| BETAS/FloraShaken/WasMossy        |                                              Whether or not this flora was mossy.                                              |                                                                                                                                                                                                                                                  |
| BETAS/FloraShaken/WasSeedy        |                                 Whether or not this flora had a seed that fell out (I think).                                  |                                                                                                                                                                                                                                                  |
| BETAS/FloraShaken/WasFertilized   |                                        Whether or not this flora was fertilized or not.                                        |                                                                                                                                                                                                                                                  |
| BETAS/FloraShaken/WasTapped       |                                         Whether or not this flora had a tapper on it.                                          |                                                                                                                                                                                                                                                  |
| BETAS/FloraShaken/WasTree         |                                             Whether or not this flora was a tree.                                              |                                                                                                                                                                                                                                                  |
| BETAS/FloraShaken/WasFruitTree    |                                          Whether or not this flora was a fruit tree.                                           |                                                                                                                                                                                                                                                  |
| BETAS/FloraShaken/WasBush         |                                             Whether or not this flora was a bush.                                              |                                                                                                                                                                                                                                                  |
<br>

* * *
## GARBAGE CHECKED <a name="garbagechecked"></a>

Raised whenever the local player rummages through a garbage can.

|     TRIGGER     |                Spiderbuttons.BETAS_GarbageChecked                 |
|:---------------:|:-----------------------------------------------------------------:|
|  Target Player  |                         The local player.                         |
| Target Location |           Location of the garbage can that was checked.           |
|   Target Item   | The item the player found in the trash can OR just a Trigger Item |
<br>

#### TARGET ITEM:
| Field  |                 Value                 |                                                                                               Usage Notes |
|:-------|:-------------------------------------:|----------------------------------------------------------------------------------------------------------:|
| ItemId | ID of the trash can that was checked. | Only if the player did not find an item in the trash. If they did, this will be the item ID of that item. |
As said above, if the player found an item in the trash can, the `Target` item for this trigger will be the item that was found. Therefore, you can use ordinary item game state queries to check things like its item ID, quality, stack size, etc.

<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                              |                              Value                               |                                           Usage Notes |
|:------------------------------------------|:----------------------------------------------------------------:|------------------------------------------------------:|
| BETAS/GarbageChecked/GarbageCanId         |           The ID of the garbage can that was checked.            |                                                       |
| BETAS/GarbageChecked/Witnesses            | A list of NPCs that caught the player checking the garbage can.  | If there are no witnesses, this value will not exist. |
| BETAS/GarbageChecked/WasMegaSuccess       |    Whether or not this garbage can check was a mega success.     |                                                       |
| BETAS/GarbageChecked/WasDoubleMegaSuccess | Whether or not this garbage can check was a double mega success. |                                                       |
<br>

* * *
## GIFT GIVEN <a name="giftgiven"></a>

Raised whenever the local player gives a gift to an NPC.

|     TRIGGER     |    Spiderbuttons.BETAS_GiftGiven     |
|:---------------:|:------------------------------------:|
|  Target Player  |          The local player.           |
| Target Location | Location of the NPC that was gifted. |
|   Target Item   |             Trigger Item             |
|   Input Item    |      The item that was gifted.       |
<br>

#### TARGET ITEM:
| Field  |                       Value                        | Usage Notes |
|:-------|:--------------------------------------------------:|------------:|
| ItemId |    The name of the NPC that was given the gift.    |             |

#### INPUT ITEM:
The `Input` item in this case is the item that was given as a gift. Therefore, you can use ordinary item game state queries to check things like its item ID, quality, stack size, etc. It will not include any mod data in it.

<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                |                                                             Value                                                              |                         Usage Notes |
|:----------------------------|:------------------------------------------------------------------------------------------------------------------------------:|------------------------------------:|
| BETAS/GiftGiven/Friendship  |                                    The friendship level of the NPC that was given the gift.                                    |                      Integer value. |
| BETAS/GiftGiven/Taste       | The gift taste that the NPC had for the item. Possible values are `Love`, `Hate`, `Like`, `Dislike`, `Neutral`, and `Special`. | `Special` is used for Stardrop Tea. |
| BETAS/GiftGiven/WasBirthday |                               Whether or not it was the NPC's birthday when the gift was given.                                |                                     |
<br>

* * *

## LETTER READ <a name="letterread"></a>

Raised whenever the local player opens their mailbox to read a letter. This trigger is _not_ raised if the player looks at a letter again in their Collections menu.

|     TRIGGER     |             Spiderbuttons.BETAS_LetterRead             |
|:---------------:|:------------------------------------------------------:|
|  Target Player  |                   The local player.                    |
| Target Location | Location of the mailbox that the letter was read from. |
|   Target Item   |                      Trigger Item                      |
<br>

#### TARGET ITEM:
| Field  |               Value                | Usage Notes |
|:-------|:----------------------------------:|------------:|
| ItemId | The `Data/mail` key of the letter. |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                            |                                 Value                                  |                                                                   Usage Notes |
|:----------------------------------------|:----------------------------------------------------------------------:|------------------------------------------------------------------------------:|
| BETAS/LetterRead/Money                  |               How much money was included in the letter.               | Integer value. This value will not exist if there was no money in the letter. |
| BETAS/LetterRead/Quest                  |             The quest ID that was attached to the letter.              |                This value will not exist if there was no quest in the letter. |
| BETAS/LetterRead/SpecialOrder           |         The special order ID that was attached to the letter.          |        This value will not exist if there was no special order in the letter. |
| BETAS/LetterRead/WasRecipe              |   Whether or not the letter contained a cooking or crafting recipe.    |                                                                               |
| BETAS/LetterRead/WasQuestOrSpecialOrder | Whether or not the letter contained either a quest or a special order. |                                                                               |
| BETAS/LetterRead/WasWithItem            |           Whether or not the letter had an attached item(s).           |                                                                               |
<br>

* * *
## LIGHTNING STRUCK <a name="lightningstruck"></a>

Raised whenever the player witnesses a lightning strike.

|     TRIGGER     | Spiderbuttons.BETAS_LightningStruck |
|:---------------:|:-----------------------------------:|
|  Target Player  |          The local player.          |
| Target Location |    Location of the local player.    |
|   Target Item   |            Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |       Value        | Usage Notes |
|:-------|:------------------:|------------:|
| ItemId | `Lightning Strike` |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                               |                                                      Value                                                      |                                                                  Usage Notes |
|:-------------------------------------------|:---------------------------------------------------------------------------------------------------------------:|-----------------------------------------------------------------------------:|
| BETAS/LightningStruck/Size                 |                    The size of the lightning strike. Possible values are `Big` and `Small`.                     |                                                                              |
| BETAS/LightningStruck/StruckTerrainFeature | The type of terrain feature that was struck by lightning. Possible values are `FruitTree`, `Crop`, and `Grass`. | This value will not exist if lightning did not strike one of these features. |
| BETAS/LightningStruck/StruckCrop           |                        The item ID of the crop that was struck by this lightning strike.                        |                This value will not exist if lightning did not strike a crop. |
| BETAS/LightningStruck/StruckTree           |                     The tree ID of the fruit tree that was struck by this lightning strike.                     |          This value will not exist if lightning did not strike a fruit tree. |
| BETAS/LightningStruck/WasLightningRod      |                          Whether or not this lightning strike struck a lightning rod.                           |                                                                              |
<br>

* * *
## MINECART USED <a name="minecartused"></a>

Raised whenever the local player uses the minecart to travel somewhere.

|     TRIGGER     | Spiderbuttons.BETAS_MinecartUsed |
|:---------------:|:--------------------------------:|
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
|:---------------:|:---------------------------------:|
|  Target Player  |         The local player.         |
| Target Location | Location of the monster killed.   |
|   Target Item   |           Trigger Item            |
<br>

#### TARGET ITEM:
| Field  |                  Value                   |                                                           Usage Notes |
|:-------|:----------------------------------------:|----------------------------------------------------------------------:|
| ItemId | The name of the monster that was killed. |                                                                       |
| Stack  | The number of items the monster dropped. | This does not take into account extra drops like books, Qi gems, etc. |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                           |                                Value                                |    Usage Notes |
|:---------------------------------------|:-------------------------------------------------------------------:|---------------:|
| BETAS/MonsterKilled/MaxHealth          |         The maximum health of the monster that was killed.          | Integer value. |
| BETAS/MonsterKilled/Damage             | How much damage the monster that was killed would do to the player. | Integer value. |
| BETAS/MonsterKilled/Drops              |   A list of item IDs that the monster dropped when it was killed.   |                |
| BETAS/MonsterKilled/WasHardmodeMonster |             Whether or not this was a hardmode monster.             |                |
<br>

* * *
## NPC KISSED <a name="npckissed"></a>

Raised whenever the local player kisses an NPC.

|     TRIGGER     |    Spiderbuttons.BETAS_NpcKissed     |
|:---------------:|:------------------------------------:|
|  Target Player  |          The local player.           |
| Target Location | Location of the NPC that was kissed. |
|   Target Item   |             Trigger Item             |
<br>

#### TARGET ITEM:
| Field  |                Value                 | Usage Notes |
|:-------|:------------------------------------:|------------:|
| ItemId | The name of the NPC that was kissed. |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                    |                                             Value                                             |    Usage Notes |
|:--------------------------------|:---------------------------------------------------------------------------------------------:|---------------:|
| BETAS/NpcKissed/Age             |     The age of the NPC that was kissed. Possible values are `Adult`, `Teen`, and `Child`.     |                |
| BETAS/NpcKissed/Gender          | The gender of the NPC that was kissed. Possible values are `Female`, `Male`, and `Undefined`. |                |
| BETAS/NpcKissed/Friendship      |                      The friendship value for the player with this NPC.                       | Integer value. |
| BETAS/NpcKissed/WasDatingFarmer |              Whether or not the NPC was dating the farmer when they were kissed.              |                |
<br>

* * *
## PASSED OUT <a name="passedout"></a>

Raised whenever the local player passes out either from exhaustion or from being up too late.

|     TRIGGER     |         Spiderbuttons.BETAS_PassedOut         |
|:---------------:|:---------------------------------------------:|
|  Target Player  |               The local player.               |
| Target Location | Location that the local player passed out in. |
|   Target Item   |                 Trigger Item                  |
<br>

#### TARGET ITEM:
| Field  |                       Value                        | Usage Notes |
|:-------|:--------------------------------------------------:|------------:|
| ItemId | The name of the location the player passed out in. |             |
<br>

#### TARGET ITEM MOD DATA:
| Mod Data Key                 |                                          Value                                          |                                                                             Usage Notes |
|:-----------------------------|:---------------------------------------------------------------------------------------:|----------------------------------------------------------------------------------------:|
| BETAS/PassedOut/Time         |                       The time of day when the player passed out.                       |                                                              Standard `0600–2600` form. |
| BETAS/PassedOut/Tool         |           The item ID of the tool that the player exhausted themselves with.            | This value will only exist if the player passed out from exhaustion through tool usage. |
| BETAS/PassedOut/WasUpTooLate |        Whether or not the player passed out because they stayed awake too late.         |                                                                                         |
| BETAS/PassedOut/WasExhausted |         Whether or not the player passed out because they exhausted themselves.         |                                                                                         |
| BETAS/PassedOut/WasSafe      | Whether or not the player passed out in a safe location (e.g. FarmHouse or IslandWest). |                                                                                         |
<br>

* * *
## RELATIONSHIP CHANGED <a name="relationshipchanged"></a>

Raised whenever the local player's relationship status (_not_ friendship value) with an NPC changes.

|     TRIGGER     |             Spiderbuttons.BETAS_RelationshipChanged             |
|:---------------:|:---------------------------------------------------------------:|
|  Target Player  |                        The local player.                        |
| Target Location | Location of the NPC whose relationship with the farmer changed. |
|   Target Item   |               Trigger Item (New Friendship Data)                |
|   Input Item    |               Trigger Item (Old Friendship Data)                |
<br>

#### TARGET ITEM:
| Field  |                      Value                      | Usage Notes |
|:-------|:-----------------------------------------------:|------------:|
| ItemId | The name of the NPC whose relationship changed. |             |
<br>

#### INPUT ITEM:
| Field  |                      Value                      | Usage Notes |
|:-------|:-----------------------------------------------:|------------:|
| ItemId | The name of the NPC whose relationship changed. |             |
<br>

In this trigger, the `Trigger Item` that is given as the `Target` will contain the friendship data for the NPC _after_ the relationship changes have already occured. The `Trigger Item` that is given as the `Input` will contain the friendship data for the NPC as it was _before_ the relationship change occured. Please keep in mind which one you are using when you are checking the friendship data.

#### TARGET ITEM MOD DATA:
| Mod Data Key                                |                                                             Value                                                             |                                                                                         Usage Notes |
|:--------------------------------------------|:-----------------------------------------------------------------------------------------------------------------------------:|----------------------------------------------------------------------------------------------------:|
| BETAS/RelationshipChanged/Status            |     The relationship status. Possible values are `Friendly`, `Dating`, `Engaged`, `Married`, `Roommate`, and `Divorced`.      |                                                                                                     |
| BETAS/RelationshipChanged/Friendship        |                                               The friendship level for the NPC.                                               |                                                                                      Integer value. |
| BETAS/RelationshipChanged/GiftsToday        |                                       The number of gifts the NPC has been given today.                                       |                                                                                      Integer value. |
| BETAS/RelationshipChanged/GiftsThisWeek     |                                     The number of gifts the NPC has been given this week.                                     |                                                                                      Integer value. |
| BETAS/RelationshipChanged/DaysSinceLastGift |                            The number of days since the last time this NPC has been given a gift.                             |                                                                                      Integer value. |
| BETAS/RelationshipChanged/WeddingSeason     | The name of the season the wedding with this NPC took place in. Possible values are `Spring`, `Summer`, `Fall`, and `Winter`. |                  This value will only exist if the player had or will have a wedding with this NPC. |
| BETAS/RelationshipChanged/WeddingDate       |                              The day of the month that the wedding with this NPC took place on.                               |   Integer value. This value will only exist if the player had or will have a wedding with this NPC. |
| BETAS/RelationshipChanged/WeddingYear       |                                The year number that the wedding with this place took place in.                                |   Integer value. This value will only exist if the player had or will have a wedding with this NPC. |
| BETAS/RelationshipChanged/DaysMarried       |                             The number of days that the player has been married to this NPC for.                              |   Integer value. This value will only exist if the player had or will have a wedding with this NPC. |
| BETAS/RelationshipChanged/WasTalkedToToday  |                                         Whether or not this NPC was talked to today.                                          |                                                                                                     |
| BETAS/RelationshipChanged/IsRoommate        |                                       Whether or not this NPC is the player's roommate.                                       | `WasRoommate` will also work here, but remember this will be the _current_ status, not past status. |
| BETAS/RelationshipChanged/WasMemoryWiped    |               Whether or not this relationship change was the result of the player wiping their ex's memories.                |                                                                                                     |
<br>

#### INPUT ITEM MOD DATA:
The input item shares the same mod data as the target item **with the exception of** `WasMemoryWiped`, which will not exist in the input item. Remember that the mod data in the `Input` item describes the friendship data with the NPC _before_ the relationship change occured.