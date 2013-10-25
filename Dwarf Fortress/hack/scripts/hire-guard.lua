-- Hires same race caravan guards.  Hooks reactions that start with LUA_HOOK_HIRE_GUARD
--[[

sample reaction
[REACTION:LUA_HOOK_HIRE_GUARD_DWARVEN]
[NAME:Hire dwarven caravan guard]
[BUILDING:TANNER:NONE]
[REAGENT:A:10000:COIN:NONE:INORGANIC:GOLD]
[PRODUCT:10:0:BOULDER:NONE:INORGANIC:CLAY][PRODUCT_DIMENSION:10]
[SKILL:TANNER]

product field must exist, using quantity 0 because it 
	doesn'tneed to actually produce anything.
	
product dimension determines sight radius in a circle with shadow cast visibility (default 10).

product probability + effective skill level determines chance to convert 
	each valid in range target, limit 1.  If the first found target fails, 
	the second found target has the same chance etc.  If all fail, reagents
	are consumed as normal (respects PRESERVE_REAGENTS if present)
	
if no valid targets in range are found, forces PRESERVE_REAGENTS
	
Announcements:

success:    "[guard name] has joined our cause!" in bright green

no targets: "[Worker Name], [Worker Profession] cancels [reaction name]: No visible targets in range." in yellow

failed:     "[Reaction name] failed." in dark red

--]]


local eventful = require 'plugins.eventful'
local mo = require 'makeown'
local fov = require 'fov'
local utils = require 'utils'

--------------------------------------------------
--------------------------------------------------
--http://lua-users.org/wiki/StringRecipes  (removed indents since I am not using them)
function wrap(str, limit)--, indent, indent1)
	--indent = indent or ""
	--indent1 = indent1 or indent
	local limit = limit or 72
	local here = 1 ---#indent1
	return str:gsub("(%s+)()(%S+)()",	--indent1..str:gsub(
							function(sp, st, word, fi)
								if fi-here > limit then
									here = st -- - #indent
									return "\n"..word --..indent..word
								end
							end)
end
--------------------------------------------------
--------------------------------------------------


function isSeen(view,p)
	return p.z == view.z and view[p.y][p.x] > 0
end

function cbHireGuard(reaction,unit,input_items,input_reagents,output_items,call_native)
	local probability = reaction.products[0].probability + dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end
	local view = fov.get_fov(radius,unit.pos)
	local i,v,u, found, success
	
	--find applicable unit
	v = df.global.world.units.active
	for i=#v-1,0,-1 do	-- search list in reverse, guards are probably nearer the end of the list
		u = v[i]
		if u.flags1.forest and not dfhack.units.isDead(u) and dfhack.units.isDwarf(u) and 
					not u.flags1.caged and isSeen(view,u.pos) and not dfhack.units.isOpposedToLife(u) and
					dfhack.units.isSane(u) and not u.flags1.merchant and not u.flags1.diplomat then
			--found one, check success
			found = true
			if math.random(100) < probability then
				mo.make_own(u)
				success = true
				-- override presearve reagents to consume? or let the reaction def control it?
				break
			end
		end
	end
	if not found then -- no valid targets
		local lines = utils.split_string( wrap( string.format("%s, %s cancels %s: No visible targets in range.",
				dfhack.TranslateName(dfhack.units.getVisibleName(unit)), dfhack.units.getProfessionName(unit), reaction.name) ) , NEWLINE )
		for _,v in ipairs(lines) do
			dfhack.gui.showAnnouncement( v, COLOR_YELLOW)
		end
		for _,v in ipairs(input_reagents or {}) do
			v.flags.PRESERVE_REAGENT = true
		end
		--unit.job.current_job.flags.suspend = true
	elseif success then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(u.name).." has joined our cause!" , COLOR_GREEN, true)--color[,is_bright]
	else
		--if no successful targets, force reagents consumed? (same probability chance to recover reagent? for each item/stack?)
		--currently let the reaction's presearve reagents determine result here.
		dfhack.gui.showAnnouncement( reaction.name.." failed.", COLOR_RED)
	end
	call_native.value = false
end



--------------------------------------------------
--------------------------------------------------
--http://lua-users.org/wiki/StringRecipes
function string.starts(String,Start)
   return string.sub(String,1,string.len(Start))==Start
end
--------------------------------------------------
--------------------------------------------------

dfhack.onStateChange.loadHireGuard = function(code)
	local registered_reactions
	if code==SC_MAP_LOADED then
		--registered_reactions = {}
		for i,reaction in ipairs(df.global.world.raws.reactions) do
			-- register each applicable reaction (to avoid doing string check
			-- for every lua hook reaction (not just ours), this way uses identity check
			if string.starts(reaction.code,'LUA_HOOK_HIRE_GUARD') then
			-- register reaction.code
				eventful.registerReaction(reaction.code,cbHireGuard)
				-- save reaction.code
				--table.insert(registered_reactions,reaction.code)
				registered_reactions = true
			end
		end
		--if #registered_reactions > 0 then print('HireGuard: Loaded') end
		if registered_reactions then print('HireGuard: Loaded') end
	elseif code==SC_MAP_UNLOADED then
		--[[ doesn't seem to be working, and probably not needed
		registered_reactions = registered_reactions or {}
		if #registered_reactions > 0 then print('HireGuard: Unloaded') end
		for i,reaction in ipairs(registered_reactions) do
			-- un register each registered reaction (to prevent persistance between
			-- differing worlds (probably irrelavant, but doesn't hurt)
			-- un register reaction.code
			eventful.registerReaction(reaction.code,nil)
		end
		registered_reactions = nil -- clear registered_reactions
		--]]
	end
end


-- if dfhack.init has already been run, force it to think SC_WORLD_LOADED to that reactions get refreshed
if dfhack.isMapLoaded() then dfhack.onStateChange.loadHireGuard(SC_MAP_LOADED) end


