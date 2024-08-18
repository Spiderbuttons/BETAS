# GAME STATE QUERIES

BETAS adds 21 new game state queries for you to use. Some were made with the intent to use them alongside the new triggers, but they can all be used in whatever `Condition` fields you see fit.

### List of Game State Queries:
- [ITEM_MOD_DATA](#itemmoddata)
- [PLAYER_MOD_DATA](#playermoddata)
- [LOCATION_MOD_DATA](#locationmoddata)
- [HAS_MOD](#hasmod)
- [NPC_LOCATION](#npclocation)
- [NPC_NEAR_PLAYER](#npcnearplayer)
- [NPC_NEAR_NPC](#npcnearnpc)
- [NPC_NEAR_AREA](#npcneararea)
- [PLAYER_DAYS_MARRIED](#playerdaysmarried)
- [PLAYER_QI_GEMS](#playerqigems)
- [PLAYER_SPEED](#playerspeed)
- [PLAYER_SPOUSE_GENDER](#playerspousegender)
- [PLAYER_MOUNTED](#playermounted)
- [PLAYER_STARDROPS_FOUND](#playerstardropsfound)
- [LOCATION_HAS_NPC](#locationhasnpc)

## ITEM_MOD_DATA <a name="itemmoddata"></a>

There are three different queries that all fall under the `ITEM_MOD_DATA` category. They all check the mod data of an item but do so in different ways. The three queries are `ITEM_MOD_DATA`, `ITEM_MOD_DATA_RANGE`, and `ITEM_MOD_DATA_CONTAINS`. The `<Type>` parameter will be either `Input` or `Target`. If you are using these conditions with BETAS triggers, you likely want to use `Target`.

|                             CONDITION                              |                                                                                  EFFECT                                                                                  |
|:------------------------------------------------------------------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
|      `Spiderbuttons.BETAS_ITEM_MOD_DATA <Type> <Key> [Value]`      |             Whether or not the item has a mod data entry with the given `Key` and `Value`. If no `Value` is given, it checks whether the key exists at all.              |
| `Spiderbuttons.BETAS_ITEM_MOD_DATA_RANGE <Type> <Key> <Min> [Max]` | If the item has a mod data entry with the given `Key and the `Value` is an integer, this checks whether that `Value` is between the `Min` and `Max` (default unlimited). |
| `Spiderbuttons.BETAS_ITEM_MOD_DATA_CONTAINS <Type> <Key> <Value>+` |       If the item has a mod data entry with the given `Key` and the `Value` is a comma or space separated list, this checks whether that list contains any `Value`       |
<br>

* * *
## PLAYER_MOD_DATA <a name="playermoddata"></a>

There are three different queries that all fall under the `PLAYER_MOD_DATA` category. They all check the mod data of the player but do so in different ways. The three queries are `PLAYER_MOD_DATA`, `PLAYER_MOD_DATA_RANGE`, and `PLAYER_MOD_DATA_CONTAINS`. The `<Player>` parameter will be a [specified player](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_player). If you are using these conditions with BETAS triggers, you likely want to use `Current`.

|                             CONDITION                              |                                                                                   EFFECT                                                                                    |
|:------------------------------------------------------------------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
|      `Spiderbuttons.BETAS_PLAYER_MOD_DATA <Player> <Key> [Value]`      |              Whether or not the player has a mod data entry with the given `Key` and `Value`. If no `Value` is given, it checks whether the key exists at all.              |
| `Spiderbuttons.BETAS_PLAYER_MOD_DATA_RANGE <Player> <Key> <Min> [Max]` | If the player has a mod data entry with the given `Key` and the `Value` is an integer, this checks whether that `Value` is between the `Min` and `Max` (default unlimited). |
| `Spiderbuttons.BETAS_PLAYER_MOD_DATA_CONTAINS <Player> <Key> <Value>+` |       If the player has a mod data entry with the given `Key` and the `Value` is a comma or space separated list, this checks whether that list contains any `Value`        |
<br>

* * *
## LOCATION_MOD_DATA <a name="locationmoddata"></a>

There are three different queries that all fall under the `LOCATION_MOD_DATA` category. They all check the mod data of a location but do so in different ways. The three queries are `LOCATION_MOD_DATA`, `LOCATION_MOD_DATA_RANGE`, and `LOCATION_MOD_DATA_CONTAINS`. The `<Location>` parameter will be a [specified location](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_location).

|                             CONDITION                              |                                                                                    EFFECT                                                                                     |
|:------------------------------------------------------------------:|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
|      `Spiderbuttons.BETAS_LOCATION_MOD_DATA <Location> <Key> [Value]`      |              Whether or not the location has a mod data entry with the given `Key` and `Value`. If no `Value` is given, it checks whether the key exists at all.              |
| `Spiderbuttons.BETAS_LOCATION_MOD_DATA_RANGE <Location> <Key> <Min> [Max]` | If the location has a mod data entry with the given `Key` and the `Value` is an integer, this checks whether that `Value` is between the `Min` and `Max` (default unlimited). |
| `Spiderbuttons.BETAS_LOCATION_MOD_DATA_CONTAINS <Location> <Key> <Value>+` |       If the location has a mod data entry with the given `Key` and the `Value` is a comma or space separated list, this checks whether that list contains any `Value`        |
<br>

* * *
## HAS_MOD <a name="hasmod"></a>

|               CONDITION                |                                                  EFFECT                                                   |
|:--------------------------------------:|:---------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_HAS_MOD <ModId>+` |                    Whether or not a mod that matches any `ModId` is found and loaded.                     |
<br>

* * *
## NPC_LOCATION <a name="npclocation"></a>

|                      CONDITION                       |                                                                                  EFFECT                                                                                  |
|:----------------------------------------------------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_NPC_LOCATION <NPC> <Location>+` | Whether or not the given `NPC` is currently in any `Location`. If you put `Any` instead of an NPC name, this will check whether any `Location` has any NPC in it at all. |
<br>

* * *
## NPC_NEAR_PLAYER <a name="npcnearplayer"></a>

|                           CONDITION                            |                                                                                                                               EFFECT                                                                                                                                |
|:--------------------------------------------------------------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_NPC_NEAR_PLAYER <Player> <Radius> [NPC]+` | Whether or not any `NPC` is within a certain integer radius of the [specified player](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_player). If no `NPC` is given, this checks whether there is any `NPC` at all within the radius of the player. |
<br>

* * *
## NPC_NEAR_NPC <a name="npcnearnpc"></a>

|                           CONDITION                            |                                                                                          EFFECT                                                                                           |
|:--------------------------------------------------------------:|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_NPC_NEAR_NPC <TargetNPC> <Radius> [NPC]+` | Whether or not any `NPC` is within a certain integer radius of the `TargetNPC`. If no `NPC` is given, this checks whether there is any `NPC` at all within the radius of the `TargetNPC`. |
<br>

* * *
## NPC_NEAR_AREA <a name="npcneararea"></a>

|                               CONDITION                                |                                                                                                                                                       EFFECT                                                                                                                                                        |
|:----------------------------------------------------------------------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_NPC_NEAR_AREA <Location> <X> <Y> <Radius> [NPC]+` | Whether or not any `NPC` is within a certain integer radius of the `X` and `Y` tile coordinates of the given `Location`. If no `NPC` is given, this checks whether there is any `NPC` at all within that radius. The `Location` parameter accepts [relative location names](RelativeQueries.md#relativelocations]). |
<br>

* * *
## PLAYER_DAYS_MARRIED <a name="playerdaysmarried"></a>

|                           CONDITION                            |                                                                                               EFFECT                                                                                               |
|:--------------------------------------------------------------:|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_PLAYER_DAYS_MARRIED <Player> <Min> [Max]` | Whether or not the [specified player](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_player) has been married for at least `Min` days and at most `Max` (default unlimited) days. |
<br>

* * *
## PLAYER_QI_GEMS <a name="playerqigems"></a>

|                         CONDITION                         |                                                                 EFFECT                                                                 |
|:---------------------------------------------------------:|:-----------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_PLAYER_QI_GEMS <Player> <Min> [Max]` | Whether or not the [specified player](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_player) has at least `Min` and at most `Max` (default unlimited) Qi Gems. |
<br>

* * *
## PLAYER_SPEED <a name="playerspeed"></a>

|                        CONDITION                        |                                                                                                             EFFECT                                                                                                             |
|:-------------------------------------------------------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_PLAYER_SPEED <Player> <Min> [Max]` | Whether or not the [specified player](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_player) has a speed of at least `Min` and at most `Max` (default unlimited). For this query, `Min` and `Max` are floats. |
<br>

* * *
## PLAYER_SPOUSE_GENDER <a name="playerspousegender"></a>

|                          CONDITION                           |                                                                            EFFECT                                                                             |
|:------------------------------------------------------------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_PLAYER_SPOUSE_GENDER <Player> <Gender>` | Whether or not the [specified player](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_player) has a spouse that is either `Female` or `Male`. |
<br>

* * *
## PLAYER_MOUNTED <a name="playermounted"></a>

|                        CONDITION                        |                                                                          EFFECT                                                                           |
|:-------------------------------------------------------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_PLAYER_MOUNTED <Player>` | Whether or not the [specified player](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_player) is currently riding a horse or other mount. |
<br>

* * *
## PLAYER_STARDROPS_FOUND <a name="playerstardropsfound"></a>

|                               CONDITION                                |                                                                 EFFECT                                                                  |
|:----------------------------------------------------------------------:|:----------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_PLAYER_STARDROPS_FOUND <Player> <Min> [Max]` | Whether or not the [specified player](https://stardewvalleywiki.com/Modding:Game_state_queries#Target_player) has found at least `Min` and at most `Max` (default unlimited) Stardrops. |
<br>

* * *
## LOCATION_HAS_NPC <a name="locationhasnpc"></a>

|                           CONDITION                            |                                                                          EFFECT                                                                           |
|:--------------------------------------------------------------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------:|
| `Spiderbuttons.BETAS_LOCATION_HAS_NPC <Location> [NPC]+` | Whether or not the given `Location` has any specified `NPC` in it. If no `NPC` is given, this checks whether there is any `NPC` at all in the `Location`. |
<br>

* * *