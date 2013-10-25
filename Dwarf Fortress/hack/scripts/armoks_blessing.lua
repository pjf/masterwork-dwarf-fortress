-- Adjust all attributes, personality, age and skills of all dwarves in play
-- place in /hack/scripts/ for ease of use
-- without arguments, all attributes, age & personalities are adjusted
-- arguments allow for skills to be adjusted as well
-- WARNING: USING THIS SCRIPT WILL ADJUST ALL DWARVES IN PLAY!
-- by vjek, version 3, 20130123, for DF(hack) 34.11 r2
-- Praise Armok!
-- ---------------------------------------------------------------------------
function rejuvenate(v)
local current_year,newbirthyear
unit=v
	if unit==nil then
		print ("No unit available!  Aborting with extreme prejudice.")
		return
	end

current_year=df.global.cur_year
newbirthyear=current_year - 20
	if unit.relations.birth_year < newbirthyear then
		unit.relations.birth_year=newbirthyear
	end
end
-- ---------------------------------------------------------------------------
function brainwash_unit(v)
unit=v
	if unit==nil then
		print ("No unit available!  Aborting with extreme prejudice.")
		return
	end

unit.status.current_soul.traits.ANXIETY=1
unit.status.current_soul.traits.ANGER=1
unit.status.current_soul.traits.DEPRESSION=1
unit.status.current_soul.traits.SELF_CONSCIOUSNESS=1
unit.status.current_soul.traits.IMMODERATION=1
unit.status.current_soul.traits.VULNERABILITY=1
unit.status.current_soul.traits.FRIENDLINESS=99
unit.status.current_soul.traits.GREGARIOUSNESS=99
unit.status.current_soul.traits.ASSERTIVENESS=99
unit.status.current_soul.traits.ACTIVITY_LEVEL=99
unit.status.current_soul.traits.EXCITEMENT_SEEKING=99
unit.status.current_soul.traits.CHEERFULNESS=99
unit.status.current_soul.traits.IMAGINATION=99
unit.status.current_soul.traits.ARTISTIC_INTEREST=99
unit.status.current_soul.traits.EMOTIONALITY=99
unit.status.current_soul.traits.ADVENTUROUSNESS=99
unit.status.current_soul.traits.INTELLECTUAL_CURIOSITY=99
unit.status.current_soul.traits.LIBERALISM=1
unit.status.current_soul.traits.TRUST=99
unit.status.current_soul.traits.STRAIGHTFORWARDNESS=99
unit.status.current_soul.traits.ALTRUISM=99
unit.status.current_soul.traits.COOPERATION=99
unit.status.current_soul.traits.MODESTY=99
unit.status.current_soul.traits.SYMPATHY=99
unit.status.current_soul.traits.SELF_EFFICACY=99
unit.status.current_soul.traits.ORDERLINESS=99
unit.status.current_soul.traits.DUTIFULNESS=99
unit.status.current_soul.traits.ACHIEVEMENT_STRIVING=99
unit.status.current_soul.traits.SELF_DISCIPLINE=99
unit.status.current_soul.traits.CAUTIOUSNESS=99

end
-- ---------------------------------------------------------------------------
function elevate_attributes(v)
local demigod = 2600
unit=v
	if unit==nil then
		print ("No unit available!  Aborting with extreme prejudice.")
		return
	end

unit.body.physical_attrs.STRENGTH.value=demigod
unit.body.physical_attrs.AGILITY.value=demigod
unit.body.physical_attrs.TOUGHNESS.value=demigod
unit.body.physical_attrs.ENDURANCE.value=demigod
unit.body.physical_attrs.RECUPERATION.value=demigod
unit.body.physical_attrs.DISEASE_RESISTANCE.value=demigod

unit.status.current_soul.mental_attrs.ANALYTICAL_ABILITY.value=demigod
unit.status.current_soul.mental_attrs.FOCUS.value=demigod
unit.status.current_soul.mental_attrs.WILLPOWER.value=demigod
unit.status.current_soul.mental_attrs.CREATIVITY.value=demigod
unit.status.current_soul.mental_attrs.INTUITION.value=demigod
unit.status.current_soul.mental_attrs.PATIENCE.value=demigod
unit.status.current_soul.mental_attrs.MEMORY.value=demigod
unit.status.current_soul.mental_attrs.LINGUISTIC_ABILITY.value=demigod
unit.status.current_soul.mental_attrs.SPATIAL_SENSE.value=demigod
unit.status.current_soul.mental_attrs.MUSICALITY.value=demigod
unit.status.current_soul.mental_attrs.KINESTHETIC_SENSE.value=demigod
unit.status.current_soul.mental_attrs.EMPATHY.value=demigod
unit.status.current_soul.mental_attrs.SOCIAL_AWARENESS.value=demigod

end
-- ---------------------------------------------------------------------------
function make_legendary(skillname,v)
local skillnamenoun,skillnum
unit=v
	if unit==nil then
		print ("No unit available!  Aborting with extreme prejudice.")
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
-- ---------------------------------------------------------------------------
function BreathOfArmok(v)
unit=v
	if unit==nil then
		print ("No unit available!  Aborting with extreme prejudice.")
		return
	end
local i
utils = require 'utils'
	for i=0, 115 do
		utils.insert_or_update(unit.status.current_soul.skills, { new = true, id = i, rating = 20 }, 'id')
	end
print ("The breath of Armok has engulfed "..unit.name.first_name)
end
-- ---------------------------------------------------------------------------
function LegendaryByClass(skilltype,v)
unit=v
	if unit==nil then
		print ("No unit available!  Aborting with extreme prejudice.")
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
-- ---------------------------------------------------------------------------
function PrintSkillList()
local i
	for i=0, 115 do
		print("'"..df.job_skill.attrs[i].caption.."' "..df.job_skill[i].." Type: "..df.job_skill_class[df.job_skill.attrs[i].type])
	end
print ("Provide the UPPER CASE argument, for example: PROCESSPLANTS rather than Threshing")
end
-- ---------------------------------------------------------------------------
function PrintSkillClassList()
local i
	for i=0, 8 do
		print(df.job_skill_class[i])
	end
print ("Provide one of these arguments, and all skills of that type will be made Legendary")
print ("For example: Medical will make all medical skills legendary")
end
-- ---------------------------------------------------------------------------
function adjust_all_dwarves(skillname)
	for _,v in ipairs(df.global.world.units.all) do
		if v.race == df.global.ui.race_id then
			print("Adjusting "..dfhack.TranslateName(dfhack.units.getVisibleName(v)))
			brainwash_unit(v)
			elevate_attributes(v)
			rejuvenate(v)
			if skillname then
				if skillname=="Normal" or skillname=="Medical" or skillname=="Personal" or skillname=="Social" or skillname=="Cultural" or skillname=="MilitaryWeapon" or skillname=="MilitaryAttack" or skillname=="MilitaryDefense" or skillname=="MilitaryMisc" then
					LegendaryByClass(skillname,v)
				elseif skillname=="all" then
					BreathOfArmok(v)
				else
					make_legendary(skillname,v)
				end
			end
		end
	end
end
-- ---------------------------------------------------------------------------
-- main script operation starts here
-- ---------------------------------------------------------------------------
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
	skillname = opt
else
	print ("No skillname supplied, no skills will be adjusted.  Pass argument 'list' to see a skill list, 'classes' to show skill classes, or use 'all' if you want all skills legendary.")
	end

adjust_all_dwarves(skillname)