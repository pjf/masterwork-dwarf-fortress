-- This script will modify skills, or a single skill, of a unit
-- usage is:  target a unit in DF, and execute this script in dfhack
-- via ' lua /path/to/script skillname '
-- the skill will be increased to 20 (Legendary +5)
-- arguments 'list', 'classes' and 'all' added
-- by vjek, version 3, 20130123, for DF(hack) 34.11 r2
-- Praise Armok!

function make_legendary(skillname)
local skillnamenoun,skillnum
unit=dfhack.gui.getSelectedUnit()

if unit==nil then
	print ("No unit under cursor!  Aborting with extreme prejudice.")
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
	utils.insert_or_update(unit.status.current_soul.skills, { new = true, id = skillnum, rating = 20 }, 'id')
	print (unit.name.first_name.." is now a Legendary "..skillnamenoun)
else
	print ("Empty skill name noun, bailing out!")
	return
	end
end

function PrintSkillList()
local i
for i=0, 115 do
	print("'"..df.job_skill.attrs[i].caption.."' "..df.job_skill[i].." Type: "..df.job_skill_class[df.job_skill.attrs[i].type])
	
	end
print ("Provide the UPPER CASE argument, for example: PROCESSPLANTS rather than Threshing")
end

function BreathOfArmok()
unit=dfhack.gui.getSelectedUnit()
if unit==nil then
	print ("No unit under cursor!  Aborting with extreme prejudice.")
	return
	end
local i
utils = require 'utils'
for i=0, 115 do
	utils.insert_or_update(unit.status.current_soul.skills, { new = true, id = i, rating = 20 }, 'id')
	end
print ("The breath of Armok has engulfed "..unit.name.first_name)
end

function LegendaryByClass(skilltype)
unit=dfhack.gui.getSelectedUnit()
if unit==nil then
	print ("No unit under cursor!  Aborting with extreme prejudice.")
	return
	end

utils = require 'utils'
local i
local skillclass
for i=0, 115 do
	skillclass = df.job_skill_class[df.job_skill.attrs[i].type]
	if skilltype == skillclass then
		print ("Skill "..df.job_skill.attrs[i].caption.." is type: "..skillclass.." and is now Legendary for "..unit.name.first_name)
		utils.insert_or_update(unit.status.current_soul.skills, { new = true, id = i, rating = 20 }, 'id')
		end
	end
end

function PrintSkillClassList()
local i
for i=0, 8 do
	print(df.job_skill_class[i])
	end
print ("Provide one of these arguments, and all skills of that type will be made Legendary")
print ("For example: Medical will make all medical skills legendary")
end

--main script operation starts here
----
local opt = ...
local skillname

if opt then
	if opt=="list" then
		PrintSkillList()
		return
		end
	if opt=="classes" then
		PrintSkillClassList()
		return
		end
	if opt=="all" then
		BreathOfArmok()
		return
		end
	if opt=="Normal" or opt=="Medical" or opt=="Personal" or opt=="Social" or opt=="Cultural" or opt=="MilitaryWeapon" or opt=="MilitaryAttack" or opt=="MilitaryDefense" or opt=="MilitaryMisc" then
		LegendaryByClass(opt)
		return
		end
	skillname = opt
else
	print ("No skillname supplied.  Pass argument 'list' to see a list, 'classes' to show skill classes, or use 'all' if you want it all!")
	return
	end

make_legendary(skillname)