-- Will execute a command over the field of view for the selected unit in a radius of 10.
--[[
	
	This script requires the fov plugin from the Hire guard reaction mods :
	http://dffd.wimbli.com/file.php?id=7785
 
	It will calculate the field of view of the source unit and will run a dfhack command for every
	unit found within this field of view. This is used, for example, as an effect from a workshop
	reaction that will affect nearby creatures.
 
	If no source unit is provided
 
	This will unfortunatly only support lua scripts.
 
	Usage : fovcommand <source_unit> <command> [<target_raw>] [<arg>]
	- command : The name of the script that will run on the targets.
	- source_unit : The id of the unit whose field of view will be checked.
	- target_raw (optional) : A creature raw's ID, this will limit the effect to this creature only.
	- arg (optional) : An additional argument to give to the command.
 
	The target script will be run as "myscript <target> <arg>".
 
	Usage with autosyndrome :
	This script works with autosyndrome, here's an example.
 
	[SYN_CLASS:\COMMAND]
	[SYN_CLASS:fovcommand]
	[SYN_CLASS:\WORKER_ID]
	[SYN_CLASS:myscript]
	[SYN_CLASS:DOG]
	[SYN_CLASS:BITING]
 
	This will run "myscript <id> BITING" for every dog in the vinicity of the worker.
	@version 1.0
	@author Boltgun
 
]]
local fov = require 'fov'
 
-- Checks
if not dfhack.isMapLoaded() then qerror('Map is not loaded.') end
if not ... then qerror('No arguments provided, please provide the source\'s id and a script name') end
 
local args = {...}
local radius = 3
local unitList = df.global.world.units.active
local unit = nil
local target_raw = nil
local view, i, unitTarget
 
-- Checking args
if not args[2] then qerror('Please enter the command you wish to run on the target units') end
 
-- Finding the source unit
function findUnit(searchId)
	local k, _
 
	for k, _ in ipairs(unitList) do
		if unitList[k].id == searchId then
			return unitList[k]
		end
	end
 
	return nil
end
 
-- Check if the unit is seen and valid
function isSelected(unit, view)
	local unitRaw = df.global.world.raws.creatures.all[unitTarget.race]
 
	if not dfhack.units.isDead(unit) and not dfhack.units.isOpposedToLife(unit) then
		if not target_raw or unitRaw.creature_id == target_raw then 
			return validateCoords(unit.pos, view) 
		end
	end
 
	return false
end
 
-- Check boundaries and field of view
function validateCoords(pos, view)
 
	if pos.x < view.xmin or pos.x > view.xmax then
		return false
	end
 
	if pos.y < view.ymin or pos.y > view.ymax then
		return false
	end
 
	return view.z == pos.z and view[pos.y][pos.x] > 0
 
end
 
 
-- action
unit = findUnit(tonumber(args[1]))
if not unit then qerror('Source unit not found') end
 
if args[3] then target_raw = args[3] end
view = fov.get_fov(radius, unit.pos)
 
-- Check through the list for the right units
for i = #unitList - 1, 0, -1 do
	unitTarget = unitList[i]
	if isSelected(unitTarget, view) then
 
		if args[4] then
			dfhack.run_script(args[2], unitTarget.id, args[4])
		else
			dfhack.run_script(args[2], unitTarget.id)
		end
 
	end
end