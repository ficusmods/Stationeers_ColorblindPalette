## What
Mod for Stationeers: https://store.steampowered.com/app/544550/Stationeers<br>
Some colors are hard to distinguish for colorblind people. This mod aims to help with that by replacing colors based on a configuration file on some items (e.g. the weather station).
The default configuration is based on the Bang Wong scheme and replaces red, green and yellow colors but you can create your own by editing config.xml and creating your own color palette based on the files in the palette/ folder.

* Nexus page: https://www.nexusmods.com/stationeers/mods/9
* News: https://ficus.cafe

## Installation
You will need the BepInEx which is an universal plugin loader for Unity games. There is an installation guide written by another modder here: https://steamcommunity.com/sharedfiles/filedetails/?id=2948870051.<br>
If you don't want BepInEx to work with the workshop you can do up to step 8 and then install mods manually by copying the files into your plugins folder ("c:\Program Files (x86)\Steam\steamapps\common\Stationeers\BepInEx\plugins" by default).

## Limitations
At the moment the mod replaces all instances of the color for all items that use the same material.<br>
From what I can tell the game uses animation clips to set the materials of objects based on the interactable states. However, the current game engine version (2021.2.3) doesn't support editing the animation bindings at runtime and setting the material in animation events don't work from my experiment. If the game ever updates it's engine version I will revisit the mod to add support for coloring individual items.

## Resources
* [Bang Wong color scheme](https://twitter.com/bangwong/status/492662880760655873?lang=en)
* [Coloring for Colorblindness](https://davidmathlogic.com/colorblind/#%23000000-%23E69F00-%2356B4E9-%23009E73-%23F0E442-%230072B2-%23D55E00-%23CC79A7)
