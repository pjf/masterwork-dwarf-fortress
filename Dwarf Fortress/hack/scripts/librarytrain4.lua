-- This script will increase one skill level of a unit
--[[
	Usage : trainAnimal <unit_id> <skill> [<max_skill>]
	unit_id : the unit's number
	skill : One of the skills as defined in the raws
	max_skill : (optionnal) The highest level allowed
 
	The skill will be increased by 1 level
 
	based on makeLegendary by vjek
	version 3.2, 20130910, for DF(hack) 34.11 r3
]]
 
local opt = {...}
local skillname, unit, max_skill
 
-- Increase he skill by one
function trainSkill(skillname, unit)
	local skillnamenoun, skill
 
	if unit==nil then
		print ("Unit not found!  Aborting with extreme prejudice.")
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
		skill = getUnitSkill(df.job_skill[skillname], unit)
 
		if skill then
			utils.insert_or_update(unit.status.current_soul.skills, skill, 'id')
		end
	else
		print ("Empty skill name noun, bailing out!")
		return
	end
end
 
-- Fetch the unit's skill and increment it, if the creature doesn't have the skill, create a lvl 1 one
function getUnitSkill(skillId, unit)
	local skill = df.unit_skill:new()
	local foundSkill = false
 
	for k, soulSkill in ipairs(unit.status.current_soul.skills) do
		if soulSkill.id == skillId then
			skill = soulSkill
			foundSkill = true
			break
		end
	end
 
	if foundSkill then
		-- Let's not train beyond the max skill
		if skill.rating >= max_skill then
			return false
		end
 
		skill.experience = 100 * skill.rating + 500
		skill.rating = skill.rating + 5
	else
		skill.id = skillId
		skill.experience = 500
		skill.rating = 5
	end
 
	return skill
end
 
-- Main script operation starts here
if not opt[1] or not opt[2] then
	if not opt[1] then
		qerror("You forgot to declare a unit!")
	end
	if not opt[2] then
		qerror("You forgot to say what skill you wanted!")
	end
end
 
unit = df.unit.find(tonumber(opt[1]))
skillname = opt[2]
max_skill = tonumber(opt[3])
 
-- By default, the highest skill will be legendary+5
if not max_skill then 
	max_skill = 5
elseif max_skill < 2 or max_skill > 5 then
	qerror("The maximum skill is out of bounds.")
end
 
trainSkill(skillname, unit)