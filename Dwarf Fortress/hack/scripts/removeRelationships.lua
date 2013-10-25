-- Removes all relationships (not including deities) from a unit.

local arg = ...

function removeRelationship(unit)
	for _,relationship in ipairs(unit.status.acquintances) do --i don't think that's how you spell "acquaintances", but oh well, that's how it spells it
		relationship.unit_id = -1 --a bit hackish, but works AFAIK
	end
end

function acceptRelationshipArg(arg)
	if tonumber(arg) then removeRelationship(df.global.world.units.all[tonumber(arg)])
	else removeRelationship(dfhack.gui.getSelectedUnit()) end
end

acceptRelationshipArg(arg)