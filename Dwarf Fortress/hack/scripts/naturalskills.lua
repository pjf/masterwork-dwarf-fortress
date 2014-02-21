-- Check for a set of creatures and see if they have their naturals skills
--[[

	There is cases in the game where a creature does not have their natural skills, for example  
	tranformations does not provide the skills as defined in the raws.

	This script goes through the list of creatures and apply natural skills on them.

	It will not lower a creature's skill if its level is above their natural one.

	You add creature IDs to the validUnit table below to narrow down the search.

	Usage
	-----

	naturalSkills [delay]
	- The optional delay will set the execution after x ingame ticks.

	Using autosyndrome
	------------------

	Using autosyndrome, add this to a rock's syndrome :
	[SYN_CLASS:\COMMAND]
	[SYN_CLASS:naturalSkills]
	[SYN_CLASS:100]

	This will apply natural skills after 100 game ticks.

	Checking everytime a save is loaded
	-----------------------------------

	Putting this line inside the raw/init.lua file, inside your saves will allow
	you to check everytime a save is loaded :

	dfhack.run_script('naturalSkills')

]]
 
if not dfhack.isMapLoaded() then qerror('Map is not loaded.') end
 
local utils = require 'utils'
local delay

-- The list of creatures supported by this script, ignored if left empty
local validUnit = {
}  

-- Scan the units table for targeted creatures
function naturalSkills()
	local unitList = df.global.world.units.active
	local i
 
	-- Check through the list for the right units
	for i = #unitList - 1, 0, -1 do
		if #validUnit == 0 or isCreature(unitList[i]) then
			fixSkills(unitList[i])
		end
	end
end
 
-- Returns true is the creature belong to our set
function isCreature(unit)
	local k, id
	local raw = df.global.world.raws.creatures.all[unit.race]
 
	for k, id in ipairs(validUnit) do
		if raw.creature_id == id then
			return true
		end
	end
 
end

-- Check if the unit has the skill and its rating high enough
function hasSkill(unit, skillId, rating)
	local k, skill, currentSkill

	for k, skill in ipairs(unit.status.current_soul.skills) do

		if skill.id == skillId then
			currentSkill = skill
			break
		end

	end

	if currentSkill then
		return currentSkill.rating >= rating
	end

	return false

end
 
-- Search the creatures for a select set, ensure those have their natural skills
function fixSkills(unit)
	local raw = df.global.world.raws.creatures.all[unit.race].caste[unit.caste]
	local k, skill, rating
 
	for k, skill in ipairs(raw.natural_skill_id) do
		rating = raw.natural_skill_lvl[k]
 
		if hasSkill(unit, skill, rating) == false then
			newSkill = df.unit_skill:new()
			newSkill['id'] = skill
			newSkill['experience'] = raw.natural_skill_exp[k]
			newSkill['rating'] = rating
 
			utils.insert_or_update(unit.status.current_soul.skills, newSkill, 'id')
		end
	end
end

if not ... then
	naturalSkills()
else
	dfhack.timeout(tonumber(...), 'ticks', function() naturalSkills() end)
end
