-- This script will elevate all the mental attributes of a unit
-- to 2600 or whatever value is specified
-- usage is:  target a unit in DF, and execute this script in dfhack
-- via ' lua /path/to/script value '
-- all mental attributes will be set to whatever 'value' is.
-- if a value is omitted, 2600 will be used.
-- by vjek, version 3, 20130123, for DF(hack) 34.11 r2
-- Praise Armok!

function elevate_unit(demigod)
unit=dfhack.gui.getSelectedUnit()
if unit==nil then
	print ("No unit under cursor!  Aborting with extreme prejudice.")
	return
	end

local old_analytical_ability,old_focus,old_willpower,old_creativity,old_intuition,old_patience,old_memory,old_linguistic_ability,old_spatial_sense,old_musicality,old_kinesthetic_sense,old_empathy,old_social_awareness
local new_analytical_ability,new_focus,new_willpower,new_creativity,new_intuition,new_patience,new_memory,new_linguistic_ability,new_spatial_sense,new_musicality,new_kinesthetic_sense,new_empathy,new_social_awareness

old_analytical_ability=unit.status.current_soul.mental_attrs.ANALYTICAL_ABILITY.value
old_focus=unit.status.current_soul.mental_attrs.FOCUS.value
old_willpower=unit.status.current_soul.mental_attrs.WILLPOWER.value
old_creativity=unit.status.current_soul.mental_attrs.CREATIVITY.value
old_intuition=unit.status.current_soul.mental_attrs.INTUITION.value
old_patience=unit.status.current_soul.mental_attrs.PATIENCE.value
old_memory=unit.status.current_soul.mental_attrs.MEMORY.value
old_linguistic_ability=unit.status.current_soul.mental_attrs.LINGUISTIC_ABILITY.value
old_spatial_sense=unit.status.current_soul.mental_attrs.SPATIAL_SENSE.value
old_musicality=unit.status.current_soul.mental_attrs.MUSICALITY.value
old_kinesthetic_sense=unit.status.current_soul.mental_attrs.KINESTHETIC_SENSE.value
old_empathy=unit.status.current_soul.mental_attrs.EMPATHY.value
old_social_awareness=unit.status.current_soul.mental_attrs.SOCIAL_AWARENESS.value

print ("Old Mental Attributes for "..unit.name.first_name)
print ("Analytical_Ability: "..old_analytical_ability.." | Focus: "..old_focus.." | Willpower: "..old_willpower.." | Creativity: "..old_creativity.." | Intuition: "..old_intuition.." | Patience: "..old_patience.." | Memory: "..old_memory.." | Linguistic_Ability: "..old_linguistic_ability.." | Spatial_Sense: "..old_spatial_sense.." | Musicality: "..old_musicality.." | Kinesthetic_Sense: "..old_kinesthetic_sense.." | Empathy: "..old_empathy.." | Social_Awareness: "..old_social_awareness)

print ("Updating Mental Attributes for "..unit.name.first_name)
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

new_analytical_ability=unit.status.current_soul.mental_attrs.ANALYTICAL_ABILITY.value
new_focus=unit.status.current_soul.mental_attrs.FOCUS.value
new_willpower=unit.status.current_soul.mental_attrs.WILLPOWER.value
new_creativity=unit.status.current_soul.mental_attrs.CREATIVITY.value
new_intuition=unit.status.current_soul.mental_attrs.INTUITION.value
new_patience=unit.status.current_soul.mental_attrs.PATIENCE.value
new_memory=unit.status.current_soul.mental_attrs.MEMORY.value
new_linguistic_ability=unit.status.current_soul.mental_attrs.LINGUISTIC_ABILITY.value
new_spatial_sense=unit.status.current_soul.mental_attrs.SPATIAL_SENSE.value
new_musicality=unit.status.current_soul.mental_attrs.MUSICALITY.value
new_kinesthetic_sense=unit.status.current_soul.mental_attrs.KINESTHETIC_SENSE.value
new_empathy=unit.status.current_soul.mental_attrs.EMPATHY.value
new_social_awareness=unit.status.current_soul.mental_attrs.SOCIAL_AWARENESS.value

print ("New Mental Attributes for "..unit.name.first_name)
print ("Analytical_Ability: "..new_analytical_ability.." | Focus: "..new_focus.." | Willpower: "..new_willpower.." | Creativity: "..new_creativity.." | Intuition: "..new_intuition.." | Patience: "..new_patience.." | Memory: "..new_memory.." | Linguistic_Ability: "..new_linguistic_ability.." | Spatial_Sense: "..new_spatial_sense.." | Musicality: "..new_musicality.." | Kinesthetic_Sense: "..new_kinesthetic_sense.." | Empathy: "..new_empathy.." | Social_Awareness: "..new_social_awareness)
end

local opt = ...
local demigod = 2600

if opt then
	demigod = opt
end

elevate_unit(demigod)