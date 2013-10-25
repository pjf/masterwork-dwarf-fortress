-- This script will brainwash a dwarf, modifying their personality
-- usage is:  target a unit in DF, and execute this script in dfhack
-- via ' lua /path/to/script '
-- all personality traits will be set to an ideal
-- by vjek, version 3, 20130123, for DF(hack) 34.11 r2
-- Praise Armok!

function brainwash_unit()
unit=dfhack.gui.getSelectedUnit()
if unit==nil then
	print ("No unit under cursor!  Aborting with extreme prejudice.")
	return
	end

print("Previous personality values for "..unit.name.first_name)
printall(unit.status.current_soul.traits)

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

print("New personality values for "..unit.name.first_name)
printall(unit.status.current_soul.traits)

print(unit.name.first_name.." has been brainwashed, happy happy, joy joy!")
end

brainwash_unit()