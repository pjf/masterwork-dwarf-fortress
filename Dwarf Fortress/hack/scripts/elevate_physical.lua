-- This script will elevate all the physical attributes of a unit
-- to 2600 or whatever value is specified
-- usage is:  target a unit in DF, and execute this script in dfhack
-- via ' lua /path/to/script value '
-- all physical attributes will be set to whatever 'value' is.
-- if a value is omitted, 2600 will be used.
-- by vjek, version 3, 20130123, for DF(hack) 34.11 r2
-- Praise Armok!

function elevate_unit(demigod)
unit=dfhack.gui.getSelectedUnit()
if unit==nil then
	print ("No unit under cursor!  Aborting with extreme prejudice.")
	return
	end

local old_strength,old_agility,old_toughness,old_endurance,old_recuperation,old_disease_resistance
local new_strength,new_agility,new_toughness,new_endurance,new_recuperation,new_disease_resistance

old_strength=unit.body.physical_attrs.STRENGTH.value
old_agility=unit.body.physical_attrs.AGILITY.value
old_toughness=unit.body.physical_attrs.TOUGHNESS.value
old_endurance=unit.body.physical_attrs.ENDURANCE.value
old_recuperation=unit.body.physical_attrs.RECUPERATION.value
old_disease_resistance=unit.body.physical_attrs.DISEASE_RESISTANCE.value
print ("Old Physical Attributes for "..unit.name.first_name)
print ("Strength: "..old_strength.." | Agility: "..old_agility.." | Toughness: "..old_toughness.." | Endurance: "..old_endurance.." | Recuperation: "..old_recuperation.." | Disease Resistance: "..old_disease_resistance)

print ("Updating Physical Attributes for "..unit.name.first_name)
unit.body.physical_attrs.STRENGTH.value=demigod
unit.body.physical_attrs.AGILITY.value=demigod
unit.body.physical_attrs.TOUGHNESS.value=demigod
unit.body.physical_attrs.ENDURANCE.value=demigod
unit.body.physical_attrs.RECUPERATION.value=demigod
unit.body.physical_attrs.DISEASE_RESISTANCE.value=demigod

new_strength=unit.body.physical_attrs.STRENGTH.value
new_agility=unit.body.physical_attrs.AGILITY.value
new_toughness=unit.body.physical_attrs.TOUGHNESS.value
new_endurance=unit.body.physical_attrs.ENDURANCE.value
new_recuperation=unit.body.physical_attrs.RECUPERATION.value
new_disease_resistance=unit.body.physical_attrs.DISEASE_RESISTANCE.value

print ("New Physical Attributes for "..unit.name.first_name)
print ("Strength: "..new_strength.." | Agility: "..new_agility.." | Toughness: "..new_toughness.." | Endurance: "..new_endurance.." | Recuperation: "..new_recuperation.." | Disease Resistance: "..new_disease_resistance)
end

local opt = ...
local demigod = 2600

if opt then
	demigod = opt
end

elevate_unit(demigod)