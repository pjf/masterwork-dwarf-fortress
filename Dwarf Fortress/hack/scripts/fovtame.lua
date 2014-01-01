-- Add the tame flag from a specified creature within LOS of the source unit
--[[

	usage fovuntame <sourceUnit> <creatureRaw>
	* sourceUnit : The unit's id in the current site (ie: 128)
	* creatureRaw : The creature raw id (ie: DOG)

	@author Boltgun

]]
local fov = require 'fov'

if not dfhack.isMapLoaded() then qerror('Map is not loaded.') end
if not ... then qerror('Please enter a creature ID.') end

local args = {...}
local unit = nil
local unitList = df.global.world.units.active
local unitId = args[1]

-- Check if the unit is seen and valid
function isSelected(unit, view)
	local unitRaw = df.global.world.raws.creatures.all[unitTarget.race]

	if unitRaw.creature_id == args[2] and
		not dfhack.units.isDead(unit) and
		not dfhack.units.isOpposedToLife(unit) then
			return validateCoords(unit, view)
	end

	return false
end

-- Check boundaries and field of view
function validateCoords(unit, view)
	local pos = {dfhack.units.getPosition(unit)}

	if pos[1] < view.xmin or pos[1] > view.xmax then
		return false
	end

	if pos[2] < view.ymin or pos[2] > view.ymax then
		return false
	end

	return view.z == pos[3] and view[pos[2]][pos[1]] > 0
end



-- Find soul wisps within the LOS of the creature
function findLos(unitSource)
	local view = fov.get_fov(5, unitSource.pos)
	local i

	-- Check through the list for the right units
	for i = #unitList - 1, 0, -1 do
		unitTarget = unitList[i]
		if isSelected(unitTarget, view) then
			untame(unitTarget)
		end
	end
end

function untame(unit)
	unit.flags1.tame = true
	unit.training_level = df.animal_training_level.Domesticated
end

unit = df.unit.find(unitId)
if not unit then qerror('Unit not found.') end

findLos(unit)
