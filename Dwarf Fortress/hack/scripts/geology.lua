-- Do stuff with the earth

local eventful = require 'plugins.eventful'
local mo = require 'makeown'
local fov = require 'fov'
local utils = require 'utils'

args={...}
	
	
function scanArea(reaction,unit,job,input_items,input_reagents,output_items,call_native)
    local pos = unit.pos
	for i=10,0,-1 do
		local x = 0
		local y = 0
		local z = 0
		local block = dfhack.maps.getTileBlock(pos.x,pos.y,pos.z)
		local theta = math.rad(math.random(360))
		local phi = math.rad(math.random(360))
		local radius = math.random(100)
		local xc = math.floor(radius * math.sin(theta) * math.cos(phi))
		local yc = math.floor(radius * math.sin(theta) * math.sin(phi))
		local zc = math.floor(radius * math.cos(theta))
		local xt = pos.x+xc
		local yt = pos.y+yc
		local zt = pos.z+zc
		block_target = nil
		block_target = dfhack.maps.ensureTileBlock(xt,yt,zt)
		if block_target then
			tiletype = block_target.tiletype[pos.x%16][pos.y%16]
			if tiletype == 219 or tiletype == 440 or tiletype == 265 then
				block_target.designation[xt%16][yt%16].hidden = false
			end
		end
	end
end


function block_events(pos, dir)
	local block = dfhack.maps.ensureTileBlock(pos.x,pos.y,pos.z)
	local found = false
	local uniqueFinds = {}
	if block and #block.block_events > 0 then
		for index,event in ipairs(block.block_events) do
			eventType = event:getType()
			if eventType == 0 then -- mineral
				if event.flags.discovered == false then -- only print if you haven't found it yet
					found = true
					local mineralName = ""
					inorganic = df.inorganic_raw.find(event.inorganic_mat)
					if inorganic.material.gem_name1 ~= "" then
						mineralName = inorganic.material.gem_name1
					else
						mineralName = inorganic.material.state_name.Solid
					end
					line = ""
					if uniqueFinds[mineralName] == nil then
						if dir == "here" then
							if event.flags.cluster == true then
								dfhack.gui.showAnnouncement( "This kind of rock often bears " .. mineralName .. "." , COLOR_WHITE, true)
							elseif event.flags.vein == true then
								dfhack.gui.showAnnouncement( "There are signs of " .. mineralName .. " nearby." , COLOR_WHITE, true)
							elseif event.flags.cluster_small == true then
								dfhack.gui.showAnnouncement( "There are hints of " .. mineralName .. " nearby." , COLOR_WHITE, true)
							elseif event.flags.cluster_one == true then
								dfhack.gui.showAnnouncement( "There may be " .. mineralName .. " nearby." , COLOR_WHITE, true)
							else
								dfhack.gui.showAnnouncement( "There is " .. mineralName .. " nearby." , COLOR_WHITE, true)
							end
						else
							
							if event.flags.cluster == true then
								dfhack.gui.showAnnouncement( "This stone to the "..dir.." is likely to have " .. mineralName .. "." , COLOR_WHITE, true)
							elseif event.flags.vein == true then
								dfhack.gui.showAnnouncement( "There are signs of " .. mineralName .. " in the stone to the " .. dir , COLOR_WHITE, true)
							elseif event.flags.cluster_small == true then
								dfhack.gui.showAnnouncement( "There are hints of " .. mineralName .. " in the stone to the " .. dir , COLOR_WHITE, true)
							elseif event.flags.cluster_one == true then
								dfhack.gui.showAnnouncement( "There may be " .. mineralName .. " in the stone to the " .. dir , COLOR_WHITE, true)
							else
								dfhack.gui.showAnnouncement( "There is " .. mineralName .. " nearby." , COLOR_WHITE, true)
							end
						end
						uniqueFinds[mineralName] = 1
					end
				end
			end
		end
	end
	return found
end

function prospect(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." examined the surrounding area.", COLOR_WHITE, true)
    local pos = {}
	pos.x = unit.pos.x
	pos.y = unit.pos.y
	pos.z = unit.pos.z
	found = false
	if block_events(pos, "here") == true then found = true end
	pos.y = pos.y-16
	if block_events(pos, "north") == true then found = true end
	pos.x = pos.x+16
	if block_events(pos, "northeast") == true then found = true end
	pos.y = pos.y+16
	if block_events(pos, "east") == true then found = true end
	pos.y = pos.y+16
	if block_events(pos, "southeast") == true then found = true end
	pos.x = pos.x-16
	if block_events(pos, "south") == true then found = true end
	pos.x = pos.x-16
	if block_events(pos, "southwest") == true then found = true end
	pos.y = pos.y-16
	if block_events(pos, "west") == true then found = true end
	pos.y = pos.y-16
	if block_events(pos, "northwest") == true then found = true end
	if found == false then
		dfhack.gui.showAnnouncement( "Nothing unexpected was found.", COLOR_WHITE, true)
	end
end

function featureList(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	local ret={}
	local unitBlock = dfhack.maps.getTileBlock(unit.pos.x,unit.pos.y,unit.pos.z)
	local unitLayer = unitBlock.designation[pos.x%16][pos.y%16].geolayer_index
	dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." struck the ground hard.", COLOR_WHITE, true)
	for index,feature in ipairs(df.global.world.cur_savegame.map_features) do
		local featureType=feature:getType()
		--[[
		river = 0
		cave = 1
		pit = 2
		magmaPool = 4
		volcano = 3
		tube = 5
		portal = 6
		cavern = 7
		magmaCore = 8
		underworld = 9
		]]--
		
		if feature.flags.Discovered == false then
			if featureType == 1 then
				dfhack.gui.showAnnouncement( "You hear a complex reverberating echo." , COLOR_WHITE, true)
			elseif featureType == 2 then
				dfhack.gui.showAnnouncement( "You hear a deep, fading echo." , COLOR_WHITE, true)
			elseif featureType == 4 then
				dfhack.gui.showAnnouncement( "You hear a soft, muffled echo." , COLOR_WHITE, true)
			elseif featureType == 5 then
				dfhack.gui.showAnnouncement( "You hear a small silent area." , COLOR_WHITE, true)
			elseif featureType == 6 then
				dfhack.gui.showAnnouncement( "You hear an unnaturally silent area." , COLOR_WHITE, true)
			elseif featureType == 7 then
				dfhack.gui.showAnnouncement( "You hear a wide, refracting echo." , COLOR_WHITE, true)
			end
		end
	end 
end

	--block.designation[pos.x%16][pos.y%16].feature_global = true
	--printall(block)
	--print(block.tiletype[pos.x%16][pos.y%16])
	--block.tiletype[pos.x%16][pos.y%16]=346
	
	--df.global.world.cur_savegame.map_features[0].
		--flags (only Discovered)
		--start_x, start_y, end_x, end_y (-1 for caverns, these are not normal map tiles.  Each feature is about 1-2 wide/tall)
		--start_depth, end_depth (also in large chunks.  Adamantine is typically 2 - 4)
		--layer
		--mat_type (for tubes)
		--mat_index (for tubes)
	--printall(df.global.world.cur_savegame.map_features)
	
	--df.global.world.cur_savegame.map_features[0].flags.Discovered = false
	local ret={}
	for index,value in ipairs(df.global.world.cur_savegame.map_features) do
		local featureType=value:getType()
		river = 0
		cave = 1
		pit = 2
		magmaPool = 4
		volcano = 3
		tube = 5
		portal = 6
		cavern = 7
		magmaCore = 8
		underworld = 9
	end 
	--df.global.world.cur_savegame.map_features[3].layer == block.global_feature, if the feature is global
	--print(block.global_feature)
	
	--Get the layer of the underworld
	for index,value in ipairs(df.global.world.cur_savegame.map_features) do
		local featureType=value:getType()
		if featureType==9 then --Underworld
			underworldLayer = value.layer
		end
	end
		
	--printall(df.global.world.cur_savegame.map_features[0].flags.Discovered) -- sets a map feature
	--printall(df.global.world.cur_savegame.map_features[0].alterations) -- ???
	--printall(df.global.world.cur_savegame.map_features[0].feature.population[0].race) -- species living on feature
		--type, plant, owner, count_min, count_max
	--printall(df.global.world.cur_savegame.map_features[0].feature.min_map_z) --max_map_z
	--block.local_feature
	--df.global.world.deep_vein_hollows[0].tiles.x -- an array of tiles found in the feature.  Also y and z, each index pointing to one tile.
	-- block.designation[pos.x%16][pos.y%16].geolayer_index --The general layer of earth  
	-- block.designation[pos.x%16][pos.y%16].feature_global --caverns, magma sea, hfs.  also block.global_feature
	-- block.designation[pos.x%16][pos.y%16].feature_local --adamantine, structures, magma pools
	-- hidden, smooth, light, subterranean, outside, biome, rained, traffic, water_stagnant, water_salt, water_table, liquid_type, pile, flow_size
--[[
    local pos=position or copyall(df.global.cursor)
    if pos.x==-30000 then
        qerror("Select a location")
    end
	local x = 0
	local y = 0
	for x=pos.x-1,pos.x+1,1 do
		for y=pos.y-1,pos.y+1,1 do
			for z=pos.z-1,pos.z+1,1 do
				local block = dfhack.maps.getTileBlock(x,y,z)
					if block then
					print(x..","..y..","..z)
					block.tiletype[x%16][y%16]=32
				end
			end
		end
	end
	for x=pos.x-2,pos.x+2,1 do
		for y=pos.y-2,pos.y+2,1 do
			for z=pos.z-2,pos.z+1,1 do
				local block = dfhack.maps.getTileBlock(x,y,z)
					if block then
					block.designation[x%16][y%16].hidden = false
				end
			end
		end
	end
	]]--


function getTileType(x,y,z)
    local block = dfhack.maps.getTileBlock(x,y,z)
    if block then
        return block.tiletype[x%16][y%16]
    else
        return 0
    end
end

dfhack.onStateChange.loadGeology = function(code)
	local registered_reactions
	if code==SC_MAP_LOADED then
		--registered_reactions = {}
		for i,reaction in ipairs(df.global.world.raws.reactions) do
			if string.starts(reaction.code,'LUA_HOOK_GEOLOGY_SCAN') then
				eventful.registerReaction(reaction.code,scanArea)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_GEOLOGY_PROSPECT') then
				eventful.registerReaction(reaction.code,prospect)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_GEOLOGY_FEATURE') then
				eventful.registerReaction(reaction.code,featureList)
				registered_reactions = true
			end
		end
		--if #registered_reactions > 0 then print('Construct Creature: Loaded') end
		if registered_reactions then
			print('Geology: Loaded.')
			--dfhack.timeout(1,"frames",function() update() end)
		end
	elseif code==SC_MAP_UNLOADED then
	end
end

-- if dfhack.init has already been run, force it to think SC_WORLD_LOADED to that reactions get refreshed
if dfhack.isMapLoaded() then dfhack.onStateChange.loadGeology(SC_MAP_LOADED) end


