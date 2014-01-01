-- Wrapper script for commands used in summoning reactions
--[[

	Requires SpawnUnit, for the current version of dfhack.
	This will spawn the unit using a random caste and call a fov tame flag removal if needed.

	usage fooccubus-summoning <sourceUnit> <creatureRaw>
	* sourceUnit : The unit's id in the current site (ie: 128)
	* creatureRaw : The creature raw id (ie: DOG)

	First parameter is the worker id, then a serie of commands
	@author Boltgun

	todo: add untame

]]

if not dfhack.isMapLoaded() then qerror('Map is not loaded.') end
if not ... then qerror('Missing parameters.') end

-- Variables
local args = {...}
local tame = false
local casteMax = 0

if not args[2] then qerror('Please enter a source unit ID number.') end
local unitId = args[1]
if not args[2] then qerror('Please enter a creature raw ID.') end
local creature = args[2]

-- Getting the summoner
unit = df.unit.find(unitId)
if not unit then qerror('Unit not found.') end

-- Setting things up for the creature
if
	creature == 'NAHASH' or
	creature == 'HELLHOUND2' or
	creature == 'NIGHTMARE2' or
	creature == 'BASILISK2'
then
	casteMax = 1
end

if
	creature == 'NAHASH' or
	creature == 'SHOTHOTH_SPAWN'
then
	tame = true
end

-- Picking a caste or gender at random
if casteMax > 0 then
	caste = math.random(0, casteMax)
else
	caste = 0
end

-- Execution
dfhack.run_script('spawnunit', creature, caste, nil, dfhack.units.getPosition(unit))

if tame then dfhack.run_script('fovtame', unitId, creature) end