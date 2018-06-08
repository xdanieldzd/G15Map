G1.5Map
=======

A rudimentary viewer for the Pokemon Gold/Silver Spaceworld 1997 demos/prototypes (specifically, the debug builds) found in May 2018. Expect bugs, just like in the games themselves.

Acknowledgements
================

* Some information was gleaned from...
  * The [pret pokegold-spaceworld disassembly project](https://github.com/pret/pokegold-spaceworld/)
  * The [Elfs2World map viewer by 2Tie](https://github.com/2Tie/Elfs2World/)
* The overlay features use the Smallest Pixel-7 font v1.0 by [Sizenko Alexander/Style-7](http://www.styleseven.com/)
  * See also G15Map/Data/PixelFont/readme.txt

Usage
=====

Open one of the debug versions of Spaceworld Gold/Silver by opening the __File__ menu, then clicking __Open ROM__. Now you can either select a map to view from the tree on the left, or open the __Tileset Viewer__ from the __Viewer__ menu to look at the tileset graphics and blocks.

The map view will display the selected map, a list of all blocks available in its tileset, and all events it has defined; __Warps__, __Signs__ and __NPCs__, depending on the map. Events can be selected using the left mouse button, which will also show the __Event Information__ dialog. They can also be selected from the tree view, in which case the dialog will not appear. It can also be shown, if an event is selected, by choosing it in the __Viewer__ menu. Warp events are __blue__, signs are __red__, and NPCs are __green__. Some of the stats visible from the Event Information dialog are also drawn directly onto the colored rectangles. An image of the loaded map can be saved via the __Save Map Image__ option in the File menu.

Warp events can also be followed by right-clicking them; this will automatically load the connected map and select the warp's counterpart. Any orange tiles, either on the map or in the blocks list, point to outside the tileset and are invalid.

The __Options__ menu allows for enabling and disabling the __Event Overlay__, i.e. the blue, red and green event rectangles, the __Grid Overlay__ outlining the blocks, the use of __Nighttime Palettes__, and the __Zoom__ functionality.

The aforementioned __Tileset Viewer__ can display all tilesets available in the game, using any palette available. The __Show Overlays__ option will display toggle a tile number overlay on top of the tileset image, as well as a collision type overlay on the blocks list. By default, the viewer will attempt to display collision types the way they actually work in-game, in the available demo areas. On the other hand, the __Assume Early Collisions__ function will try to approximate how collision used to work at an earlier stage of the game's development, as still evident by the maps left unused and with broken collision by the demo (e.g. Old City and onwards). __Show Grids__ works just like the related option for maps in the Options menu.

Do note that the collision display in the viewer is incomplete, especially for the early collision types, and it is not guaranteed that the known types are fully accurate, either.

Images of the tilesets and blocks, without the collision overlays, can be saved by clicking the __Save Tileset Image__ and __Save Block Image__ buttons.

Screenshots
===========

![Screenshot](https://raw.githubusercontent.com/xdanieldzd/G15Map/master/Screenshots/Shot1.png)<br><br>
