-- Shows/resets hunger and thirst counters

function run(cmd)
	local unit=dfhack.gui.getSelectedUnit()
	if unit then
		local name = dfhack.units.getVisibleName(unit)
		if name and name.has_name then
			unitname = dfhack.TranslateName(name)
		else
			unitname = "unit"
		end
		

		if cmd == 'status' then
			print("Status for "..unitname..":")
			print("Hunger:", unit.counters2.hunger_timer)
		elseif cmd == 'reset' then
			print("Resetting hunger and thirst counters for "..unitname)
			unit.counters2.hunger_timer=0
		end
	end
end

local cmd = ...
if not cmd then
	qerror('Usage: hunger status/reset')
end
run(cmd)