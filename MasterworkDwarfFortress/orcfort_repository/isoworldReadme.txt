====Introduction====
Iso world is a little program I made that opens the worldmaps from df, but you knew that already.

To use it, export the elevation, elevation with water, biome (NOT that standard biome+elevation map), and structure maps from DF, then open one of them. the rest will get loaded automatically. if they don't, you can manually put the path isoworld.ini.

====Key bindings====

space				Change the current tileset.
g					Toggle the grid.
d					Toggle debug mode.
q					Decrease ligting quality.
w					Increase ligting quality.
arrow keys			Move around.
shift				Inncrease scroll speed.
left click (on minimap)	Jump to area on map.
right click drag	Move around.

====How to make a tileset====
Tilesets are stored in three main groups of files, tilesets.ini, which contains a list of all the tilesets that should be loaded, a main tileset config, and a number of tile configs. the structure is as follows:

===tilesets.ini===
This is a rather simple file containg a list of all the tilesets that should be loaded by isoworld. sample follows:

[TILESETS]
num_tilesets=2 					#This is the number of tilesets that Isoworld will try to load.
tileset_0=path/to/tileset.ini
tileset_1=path/to/tileset2.ini	#These are paths to the various tileset configs. they will all be loaded, and can changed by pressing the spacebar while running the program.

===Main tileset configs===
these files give the global properties of a tileset. Sample follows:

[TILESET_PROPERTIES]
tile_width=32					#The width of the main portion of the tiles. used by the tiling algorithm
tile_height=16					#The height of the main portion of the tiles. used by the tiling algorithm
snap_height						#Optional. If set, this 
grid_tile						#A tile containing a single diagonal line, for use in the grid.
tile_dir						#Directory containing all the tiles. if the tiles are in the same folder as the tileset config, simply put './' without the quotes.
palette_file					#image file containing color gradients. Each pixel column is one gradient, can be any size, but only 280 pixels tall is of any use.

===Individual tile configs===
These are the tiles that appear on the map. Isoworld will load all of them that are in a directory, and draw the first one in a list that it finds that matches all the given paramiters. If no match is found, no tile is drawn, so it's best to have a default tile that can always get drawn.

[SPRITE]
height_max=280 					#the maximum allowable height for the tile. if the height on the terrain is above this, the tile will be skipped, and the next one on the list will be tested for drawing.
height_min=0					#the minimum height that the tile will be drawn.
priority=0						#The priority of the tile. if two tiles are found that match the given terrain, the one with the lower number will be drawn.
special_terrain=any				#The biome for which this tile should be drawn. see the later section for a full list.
special_object=none				#additional objects on the terrain. This includes rivers, and all manmade objects.
cap_layers=1					#Number of sprite laters that make up the top of the tile. The sprites themselves are listed later.
column_layers=1					#Number of sprites that make up the side of the column. these are tiled from the top downwards, with the bottom one shifted upwards to make it line up right. it's better to have a few larger ones than a lot of smaller ones, speed-wise.
surface_layers=0				#Similar to the cap layers, but used for the ocean and lake surfaces.
object_layers=0					#This is for objects that sit on top of the terrain. Anything in these layers will be not be drawn if there is any special objects on the terrain, such as rivers, roads, and towns.
intermediate_layers=0			#Similar to the column layers, but they get drawn between the surface and the bottom of oceans and lakes.
[OBJECT_IMAGE_0][CAP_IMAGE_0][SURFACE_IMAGE_0][COLUMN_IMAGE_0][INTERMEDIATE_IMAGE_0]
x=0								#X location on the tileset image of the top-left corner of the sprite.
y=0								#Y location.
width=32						#width of the sprite.
height=32						#height of the sprite.
origin_x=15						#draw origin on the sprite. should be the bottom center, but can be anywhere as long as it's consistant.
origin_y=31
column_height=0					#Height of the column, in pixels, if it's a column sprite. otherwise is not used.
color_source=none				#Where the sprite should get it's color from. see the later section for possible values.
color_html=#FFFFFF				#HTML color code, used if color_source is set to html
offset_type=none				#The sprite can be offset by a number of different methods, moving the source rectangle of the sprite across the tileset image horizontally. this is mainly used for random variation, or tile borders.
border_terrain=none				#The terrain that border offsets should search for. if set to none, or not set, it will use the current terrain that the tile is on. uses the same tags as 
offset_amount=0					#for offset types that have a variable amount, this sets the total number of possible tiles.

===Values of various tags===
==color_source==
none							#The sprite is not colored, other than by lighting.
html							#The sprite is colored by an HTML code given in another tag.
biome_map						#The color is taken from the biome map that is exported from DF.

==special_terrain==
any								#This tile will be used for any terrain, unless excluded by a different tag.
none							#This tile will only show up if the biome map has a biome that's not recognised by Isoworld.
mountain						#the rest are pretty self explanatory.
temperate_freshwater_lake
temperate_brackish_lake
temperate_saltwater_lake
tropical_freshwater_lake
tropical_brackish_lake
tropical_saltwater_lake
arctic_ocean
tropical_ocean
temperate_ocean
glacier
tundra
temperate_freshwater_swamp
temperate_saltwater_swamp
temperate_freshwater_marsh
temperate_saltwater_marsh
tropical_freshwater_swamp
tropical_saltwater_swamp
mangrove_swamp
tropical_freshwater_marsh
tropical_saltwater_marsh
taiga_forest
temperate_conifer_forest
temperate_broadleaf_forest
tropical_conifer_forest
tropical_dry_broadleaf_forest
tropical_moist_broadleaf_forest
temperate_grassland
temperate_savanna
temperate_shrubland
tropical_grassland
tropical_savanna
tropical_shrubland
badland_desert
sand_desert
rock_desert

==special_object==
any								#This tile will be used for any objects on the terrain. Good for default, but make sure it has a low priority.
none							#This tile will only show up on unbuilt terrain
castle							#the rest are pretty self explanatory.
village
crops_1
crops_2
crops_3
pasture
meadow
woodland
orchard
tunnel
stone_bridge
other_bridge
stone_road
other_road
stone_wall
other_wall
river
brook

==offset_type==
none							#There is no tile offset.
sixteen							#Sprite is chosen from a list of sixteen possible neighbor boundaries.
path							#The sprite is chosen from a list of 10 directions. good for rivers, walls, and roads.
pair							#A pair of two sprites are chosen betwen, one in the / direction, the other in the \ direction.
six								#similar to the path one, but with no T or X sections
four							#There are 4 sprites to chose from, one for each cardinal direction
random							#randomly chose between the number of sprites given in offset_amount