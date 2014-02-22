-- Do stuff with the earth

local eventful = require 'plugins.eventful'
local mo = require 'makeown'
local fov = require 'fov'
local utils = require 'utils'

args={...}
	
	
function drillDown(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	pos = {}
	pos.x = unit.pos.x
	pos.y = unit.pos.y
	pos.z = unit.pos.z
	baseBlock = dfhack.maps.ensureTileBlock(pos.x,pos.y,pos.z)
	for depth=pos.z-1,0,-1 do
		local block = dfhack.maps.ensureTileBlock(pos.x,pos.y,depth)
		if block then
			if block.tiletype[pos.x%16][pos.y%16] ~= 495 then
				block.tiletype[pos.x%16][pos.y%16] = 495
				block.designation[pos.x%16][pos.y%16].hidden = false
				block.designation[(pos.x)%16][(pos.y-1)%16].hidden = false
				block.designation[(pos.x+1)%16][(pos.y-1)%16].hidden = false
				block.designation[(pos.x+1)%16][(pos.y)%16].hidden = false
				block.designation[(pos.x+1)%16][(pos.y+1)%16].hidden = false
				block.designation[(pos.x)%16][(pos.y+1)%16].hidden = false
				block.designation[(pos.x-1)%16][(pos.y+1)%16].hidden = false
				block.designation[(pos.x-1)%16][(pos.y)%16].hidden = false
				block.designation[(pos.x-1)%16][(pos.y-1)%16].hidden = false
				block.designation[pos.x%16][pos.y%16].flow_size = 0
				break
			end
		end
	end
end

function drillUp(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	pos = {}
	pos.x = unit.pos.x
	pos.y = unit.pos.y
	pos.z = unit.pos.z
	baseBlock = dfhack.maps.ensureTileBlock(pos.x,pos.y,pos.z)
	if (dfhack.maps.ensureTileBlock(pos.x,pos.y,pos.z-1)).tiletype[pos.x%16][pos.y%16] ~= 495 then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." cannot retract the drill any further.", COLOR_RED, true)
		reaction.products[0].probability = 0
	else
		for depth=pos.z-1,0,-1 do
			local block = dfhack.maps.ensureTileBlock(pos.x,pos.y,depth)
			if block then
				if block.tiletype[pos.x%16][pos.y%16] ~= 495 then
					drillBit = dfhack.maps.ensureTileBlock(pos.x,pos.y,depth+1)
					if block.tiletype[pos.x%16][pos.y%16] == 440 then -- solid regular rock below
						drillBit.tiletype[pos.x%16][pos.y%16] = 237 -- regular rock upward slope
					elseif block.tiletype[pos.x%16][pos.y%16] == 219 then -- solid event rock below
						drillBit.tiletype[pos.x%16][pos.y%16] = 240 -- event rock upward slope
					elseif block.tiletype[pos.x%16][pos.y%16] == 237 or block.tiletype[pos.x%16][pos.y%16] == 240 then -- upward slope below
						drillBit.tiletype[pos.x%16][pos.y%16] = 1 -- downward slope
					else
						drillBit.tiletype[pos.x%16][pos.y%16] = 32 -- open space
					end
					dfhack.maps.enableBlockUpdates(drillBit,true,true)
					break
				end
			end
		end
	end
end

function drillPump(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	pos = {}
	pos.x = unit.pos.x
	pos.y = unit.pos.y
	pos.z = unit.pos.z
	baseBlock = dfhack.maps.ensureTileBlock(pos.x,pos.y,pos.z)
	underBaseBlock = dfhack.maps.ensureTileBlock(pos.x,pos.y,pos.z-1)
	for depth=pos.z-1,0,-1 do
		local block = dfhack.maps.ensureTileBlock(pos.x,pos.y,depth)
		if block then
			if block.tiletype[pos.x%16][pos.y%16] ~= 495 then
				drillBit = dfhack.maps.ensureTileBlock(pos.x,pos.y,depth)
				if drillBit.designation[pos.x%16][pos.y%16].flow_size > 3 then
					drillBit.designation[pos.x%16][pos.y%16].flow_size = drillBit.designation[pos.x%16][pos.y%16].flow_size - 1
					if underBaseBlock.designation[pos.x%16][pos.y%16].flow_size == 7 then
						baseBlock.designation[pos.x%16][pos.y%16].flow_size = baseBlock.designation[pos.x%16][pos.y%16].flow_size + 1
						baseBlock.designation[pos.x%16][pos.y%16].liquid_type = drillBit.designation[pos.x%16][pos.y%16].liquid_type
						dfhack.maps.enableBlockUpdates(baseBlock,true,true)
					else
						underBaseBlock.designation[pos.x%16][pos.y%16].flow_size = underBaseBlock.designation[pos.x%16][pos.y%16].flow_size + 1
						underBaseBlock.designation[pos.x%16][pos.y%16].liquid_type = drillBit.designation[pos.x%16][pos.y%16].liquid_type
						dfhack.maps.enableBlockUpdates(baseBlock,true,true)
					end
						
					dfhack.maps.enableBlockUpdates(drillBit,true,true)
				else
					dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to bring up any liquids.", COLOR_RED, true)
				end
				break
			end
		end
	end
end


dfhack.onStateChange.loadDrillRig = function(code)
	local registered_reactions
	if code==SC_MAP_LOADED then
		--registered_reactions = {}
		for i,reaction in ipairs(df.global.world.raws.reactions) do
			if string.starts(reaction.code,'LUA_HOOK_DRILLING_RIG_DOWN') then
				eventful.registerReaction(reaction.code,drillDown)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRILLING_RIG_UP') then
				eventful.registerReaction(reaction.code,drillUp)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRILLING_RIG_PUMP') then
				eventful.registerReaction(reaction.code,drillPump)
				registered_reactions = true
			end
		end
		if registered_reactions then
			print('Drilling Rig: Loaded.')
			--dfhack.timeout(1,"frames",function() update() end)
		end
	elseif code==SC_MAP_UNLOADED then
	end
end

-- if dfhack.init has already been run, force it to think SC_WORLD_LOADED to that reactions get refreshed
if dfhack.isMapLoaded() then dfhack.onStateChange.loadDrillRig(SC_MAP_LOADED) end


