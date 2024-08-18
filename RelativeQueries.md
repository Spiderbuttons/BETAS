# ACTIONS

BETAS adds 21 new game state queries for you to use. Some were made with the intent to use them alongside the new triggers, but they can all be used in whatever `Condition` fields you see fit. If a parameter is wrapped in `< >` then it is a _required_ parameter. If the parameter is wrapped in `[ ]` then it is optional. If the parameter has a `+` after it, it means you can provide multiple of that parameter separated by spaces.

<br>

* * *

## UTILITY

| Condition                                                                                           | Effect |
|:----------------------------------------------------------------------------------------------------|:------|
| $`{\textsf{\color{Orchid}Spiderbuttons.BETAS\_HAS\_MOD}}`$ \$`{\textsf{\color{RedOrange}<ModId>}}`$+ | Whether or not a mod that matches any `ModId` is found and loaded. |
<br>

* * *