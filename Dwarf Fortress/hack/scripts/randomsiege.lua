-- Invoke a random siege on the fort
--[[

	This script requires the force script found here :
	http://dffd.wimbli.com/file.php?id=7471

	Usage : fooccubus-callsiege x
	x = A number between 1 to 100, representing the probability for the siege to happen.

	Don't forget to edit the entities array below to your liking.
	@author Boltgun

]]

-- Config
-- Chances of attack for each entity, they do not need to sum up to 100
local entities = {
	["MOUNTAIN"] = 50, -- dwarves
	["FOREST"] = 50, -- elves
	["EVIL"] = 50, -- goblins
	["SKULKING"] = 10, -- kobolds
	["PLAINS"] = 50, -- humans
	["DROW"] = 50, -- humans
	["KOBOLD_CAMP"] = 10, -- humans
	["GNOME_CIV"] = 10, -- humans
	["FROST_GIANT"] = 10, -- humans
}

-- Checks
if not dfhack.isMapLoaded() then qerror('Map is not loaded.') end
if not ... then qerror('Missing parameter, please enter a probability between 1 and 100.') end
local proba = tonumber(...)

-- Methods
-- Make sure the number entered is correct
function validateProba(proba)
	return proba > 0 and proba < 101
end

-- Pick the race that will lay siege
function getRace()
	local total = 0
	local cursor = 0
	local random, entity, score

	for entity, score in pairs(entities) do
		total = total + score
	end

	random = math.random(0, total)

	for entity, score in pairs(entities) do
		cursor = cursor + score
		if cursor >= random then return entity end
	end

	return entity
end

-- Make the race selection then call for the siege, provides feedback
function provokeSiege()
	local entity = getRace()

	dfhack.run_script('force', 'siege', entity)
	dfhack.gui.showAnnouncement("You have attracted the attention of your enemies !", COLOR_LIGHTRED, true)
end

-- Execution
if not validateProba(proba) then qerror('Please enter a probability between 1 and 100.') end

if math.random(0, 100) <= proba then
	provokeSiege()
end
