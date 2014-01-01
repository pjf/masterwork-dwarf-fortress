-- Sets the units within line of sight of the unit
--[[

	This script is called by the conversion dens
	@author Boltgun

	@todo support for semi megabeasts such as minotaur
]]
local fov = require 'fov'
local mo = require 'makeown'

if not dfhack.isMapLoaded() then qerror('Map is not loaded.') end
if not ... then qerror('Please enter a creature ID.') end

local unit, creatureSet
local args = {...}

-- Check if the unit is seen and valid
function isSelected(unit, view)
	local creatureId = df.global.world.raws.creatures.all[unit.race].creature_id

	if creatureSet[creatureId] and
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

-- Find targets within the LOS of the creature
function findLos(unitSource)
	local view = fov.get_fov(10, unitSource.pos)
	local i
	local unitList = df.global.world.units.active

	-- Check through the list for the right units
	for i = #unitList - 1, 0, -1 do
		unitTarget = unitList[i]
		if isSelected(unitTarget, view) then
			mo.make_own(unitTarget)

			if makeCitizen then
				mo.make_citizen(unitTarget)
			end

			-- Taking down all the hostility flags
			unitTarget.flags1.marauder = false
			unitTarget.flags1.active_invader = false
			unitTarget.flags1.hidden_in_ambush = false
			unitTarget.flags1.hidden_ambusher = false
			unitTarget.flags1.invades = false
			unitTarget.flags1.coward = false
			unitTarget.flags1.invader_origin = false
			unitTarget.flags2.underworld = false
			unitTarget.flags2.visitor_uninvited = false
			unitTarget.invasion_id = -1
		end
	end
end

-- Action
unit = df.unit.find(tonumber(args[1]))
if not unit then qerror('Unit not found.') end

-- Return the set of affected units
makeCitizen = false
if not args[2] then qerror('Please enter a creature set.') end
if args[2] == 'invaders-deep' then
	creatureSet = {['HUMAN'] = true, ['KOBOLD'] = true, ['ELF'] = true, ['DWARF'] = true, ['GOBLIN'] = true, ['FOOCCUBUS'] = true}
elseif args[2] == 'invaders' then
	creatureSet = {['HUMAN'] = true, ['KOBOLD'] = true, ['ELF'] = true, ['DWARF'] = true, ['GOBLIN'] = true, ['FOOCCUBUS_DEEP'] = true}
	makeCitizen = true
elseif args[2] == 'minotaur' then
	creatureSet = {['MINOTAUR'] = true}
	makeCitizen = true
else
	qerror('Unsupported creature set.')
end

findLos(unit)
