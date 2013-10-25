A collection of utility plugins for DFHack version 0.34.11 r3 that focus on improving the usability of some of the interface screens. None of these plugins change the game's behaviour in any way, they only add to the interface. To install, copy the * files for the plugins you want into "<your df folder>\hack\plugins" and they will be automatically loaded when you next start Dwarf Fortress. If you are installing the zone plugin, I recommend you make a backup of the existing versions in your plugins directory first.

Detailed information for each plugin, including screenshots, can be found at the forum threads linked to in each descriptions below. Please make sure you have the correct version of DFHack installed (if yours came bundled with the LNP it is likely to be out of date).


Plugin File: search.plug
Fixes a crash condition in the military screen that exists in the search plugin included with DFHack r3


Plugin File: buildingplan.plug
Version: 0.9
Thread: http://www.bay12forums.com/smf/index.php?topic=121858.0
Allows you to place furniture (beds, chairs, etc) before they are built. The building will remain in "suspended construction" state until a suitable item is available, at which point the closest available item will be allocated to it and construction will resume automatically. You can set filters for quality, materials and decorations.


Plugin File: automaterial.plug
Version: 0.10
Thread: http://www.bay12forums.com/smf/index.php?topic=119369
A plugin that makes building constructions a bit easier by adding the following features:
* Moves the last used material to the top of the material list
* Allows you to assign certain materials for "auto-selected" in future construction
* Enables rectangular selection for placing constructions, the way designations are done
* Allows the designations if "future" constructions, i.e. allocating new constructions in open space or adjacent to constructions not yet built. This allows you to designate an entire structure in one sitting, instead of having to wait till each section is built before 


Plugin File: stocks.plug
Version: 0.4
Thread: http://www.bay12forums.com/smf/index.php?topic=125164.0
Adds an alternative stocks interface, which should make it a bit faster to find items. It includes a single searchable list with multiple indicators and filters. Add a keybinding to dfhack.init similar to:
keybinding add Ctrl-K@dwarfmode/Default "stocks show"
and use that keybinding to open the screen.


Plugin File: zone.plug
Version: 0.2
Thread: http://www.bay12forums.com/smf/index.php?topic=119575.0
This plugin adds search and filtering functionality to the list that you get when you go to assign creatures to a Pen/Pasture/Pit/Pond. It adds a free text search option, as well as the ability to filter out non grazing animals and animals already in a pasture.


Plugin File: autotrade.plug
Version: 0.1
Thread: http://www.bay12forums.com/smf/index.php?topic=125164.0
Allows you to tag selected stockpiles, so that whenever caravans arrives, items in those stockpiles will automatically be marked for hauling to the trade depot. Checking will be done every half a day and you can also do a one off marking of all items in a specific stockpile.


Plugin File: mousequery.plug
Version: 0.14
Thread: http://www.bay12forums.com/smf/index.php?topic=119575.0
A plugin that allows you to use the mouse to click on items, creatures, buildings, etc and have the game open the appropriate query menu for it, in a context sensitive fashion. Allows moving the cursor with the mouse and left clicking to simulate an Enter keypress. Further, hovering over a tile with the mouse cursor displays a live query of objects at that location in the menu pane.



Plugin File: resume.plug
Version: 0.2
Thread: http://www.bay12forums.com/smf/index.php?topic=122395.0
When enabled, displays a colour coded "X" over buildings and constructions that are suspended, making them easier to spot. The plugin also provides a command to resume all suspended buildings. Type "resume" in the DFHack console for usage instructions.


Plugin File: dwarfmonitor.plug
Version: 0.7
Thread: http://www.bay12forums.com/smf/index.php?topic=123279.0
When enabled, tracks all work and leisure activity in the fortress every 100 ticks over a rolling window of 7 days (by default). This data can be displayed ingame, to show how much time is spent in the fort on each activity, as a percentage. The data can also be displayed per dwarf, telling you what they've been doing and how much time they have spent on each activity.

Additionally, the plugin can:
 * show a live overlay in the game margin showing happiness levels of your citizens.
 * show a summary of the preferences of the dwarves in your fort.

Type "dwarfmonitor" in the DFHack console for full usage instructions.
