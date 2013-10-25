-- This script will make any "old" dwarf 20 years old
-- usage is:  target a unit in DF, and execute this script in dfhack
-- via ' lua /path/to/script '
-- the target will be changed to 20 years old
-- by vjek, version 3, 20130123, for DF(hack) 34.11 r2
-- Paise Armok!


function rejuvenate()
local current_year,newbirthyear
unit=dfhack.gui.getSelectedUnit()

if unit==nil then
	print ("No unit under cursor!  Aborting with extreme prejudice.")
	return
	end

current_year=df.global.cur_year
newbirthyear=current_year - 20
if unit.relations.birth_year < newbirthyear then
	unit.relations.birth_year=newbirthyear
end
print (unit.name.first_name.." is now 20 years old")

end

rejuvenate()