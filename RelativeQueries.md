# RELATIVE QUERIES

Some Game State Queries or Actions require you to specify a certain location or certain tile coordinates in order to work properly. However, you may not always know exactly where you want to check in these cases. Take the [Lightning Action](Actions.md) for instance. You may want to strike lightning 5 tiles above the Farmer or 3 blocks to the left of $`{\textsf{\color{Rhodamine}Haley}}`$, no matter where they might be on the map. Also consider the [WarpFarmer Action](Actions.md). You may want to warp the Farmer to the same map as Abigail, no matter where Abigail actually is. This is where these **Relative Queries** come into play.

<br>

* * *

## RELATIVE COORDINATES <a name="Coordinates"></a>

| Format                     | Effect                                                                                                                                                                                                                                                                                       | Example                                                                           |
|:---------------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:----------------------------------------------------------------------------------|
| $`{\textsf{\color{White}RelativeX:\color{Rhodamine}<Target>\color{BlueGreen}:<Value>}}`$ | The X coordinate will be calculated as `Value` tiles to the left (negative values) or right (positive values) of the tile the `Target` is on. Possible `Target` values are `Farmer` or any NPC name.                                                                                         | $`{\textsf{\color{White}RelativeX:\color{Rhodamine}Haley\color{BlueGreen}:-5}}`$ |
| $`{\textsf{\color{White}RelativeY:\color{Rhodamine}<Target>\color{BlueGreen}:<Value>}}`$ | The Y coordinate will be calculated as `Value` tiles above (negative values) or below (positive values) of the tile the `Target` is on. Possible `Target` values are `Farmer` or any NPC name.                                                                                               | $`{\textsf{\color{White}RelativeY:\color{Rhodamine}Farmer\color{BlueGreen}:2}}`$ |
<br>

Please keep in mind that these coordinates will still be calculated _regardless of what location the `Target` is in._ That is, if you intend to warp the Farmer to the IslandWest location but use coordinates relative to $`{\textsf{\color{Rhodamine}Haley}}`$ who is currently at X:25, Y:13 in the Beach location, the resulting coordinates will be relative to X:25, Y:13 in the IslandWest location.

<br>

* * *

## RELATIVE LOCATIONS <a name="Locations"></a>

| Format                                                       | Effect                                                                                                                                            | Example                                                     |
|:-------------------------------------------------------------|:--------------------------------------------------------------------------------------------------------------------------------------------------|:------------------------------------------------------------|
| $`{\textsf{\color{White}NPC:\color{Rhodamine}<Target>}}`$    | The location will be set to the location the `Target` is currently in. The `Target` value must be an NPC name.                                    | $`{\textsf{\color{White}NPC:\color{Rhodamine}Abigail}}`$    |
| $`{\textsf{\color{White}Farmer:\color{Rhodamine}<Target>}}`$ | The location will be set to a specific location related to the Farmer. Currently, only `Current` is supported, for the Farmer's current location. | $`{\textsf{\color{White}Farmer:\color{Rhodamine}Current}}`$ |