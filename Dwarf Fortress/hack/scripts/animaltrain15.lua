-- This script will modify skills, or a single skill, of a unit
-- usage is:  target a unit in DF, and execute this script in dfhack
-- via ' lua /path/to/script skillname '
-- the skill will be increased to 20 (Legendary +5)
-- arguments 'list', 'classes' and 'all' added
-- original by vjek
-- version 3.1, 20130827, for DF(hack) 34.11 r3
-- Praise Armok!

-- Finding the unit
function findUnit(searchId)
	local k, _
	local unitList = df.global.world.units.active
 
	for k, _ in ipairs(unitList) do
		if unitList[k].id == searchId then
			return unitList[k]
		end
	end
 
	return nil
end

function make_legendary(skillname,unit)
local skillnamenoun,skillnum

if unit==nil then
	print ("No unit selected!  Aborting with extreme prejudice.")
	return
	end

if (df.job_skill[skillname]) then
	skillnamenoun = df.job_skill.attrs[df.job_skill[skillname]].caption_noun
else
	print ("The skill name provided is not in the list.")
	return
	end

if skillnamenoun ~= nil then
	utils = require 'utils'
	skillnum = df.job_skill[skillname]
	utils.insert_or_update(unit.status.current_soul.skills, { new = true, id = skillnum, rating = rating or 15 }, 'id')
	--print (unit.name.first_name.." is now a Level ".. rating .." ".. skillnamenoun)
else
	print ("Empty skill name noun, bailing out!")
	return
	end
end

--main script operation starts here
----
local opt = {...}
local skillname

local unit      = findUnit(opt[1])
local skillname = opt[2]
	  
if not opt[1] or not opt[2] then
	print("Your syntax is wrong. To be exact...")
	if not opt[1] then
		print("You forgot to declare a unit!")
	end
	if not opt[2] then
		print("You forgot to say what skill you wanted!")
	end
end
make_legendary(skillname,unit)