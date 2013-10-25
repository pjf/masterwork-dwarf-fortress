-- This script will modify skills, or a single skill, of a unit
--[[
	Usage : trainAnimal <unit_id> <skill>
	unit_id : the unit's number
	skill : One of the skills as defined in the raws
 
	The skill will be increased by 1 level
 
 
-- based on makeLegendary by vjek
-- version 3.1, 20130827, for DF(hack) 34.11 r3
]]
 
-- Finding the unit
function findUnit(searchId)
	local k, _
	local unitList = df.global.world.units.active
	searchId = tonumber(searchId)
 
	for k, _ in ipairs(unitList) do
		if unitList[k].id == searchId then
			return unitList[k]
		end
	end
 
	return nil
end
 
-- increase he skill by one
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
		-- Let's not train beyond legendary +5
		if skill.rating >= 20 then
			return false
		end
 
		skill.experience = 100 * skill.rating + 500
		skill.rating = skill.rating + 1
	else
		skill.id = skillId
		skill.experience = 500
		skill.rating = 1
	end
 
	return skill
end
 
--main script operation starts here
local opt = {...}
local skillname
 
local unit = findUnit(opt[1])
local skillname = opt[2]
 
	  
if not opt[1] or not opt[2] then
	if not opt[1] then
		qerror("You forgot to declare a unit!")
	end
	if not opt[2] then
		qerror("You forgot to say what skill you wanted!")
	end
end
 
trainSkill(skillname, unit)