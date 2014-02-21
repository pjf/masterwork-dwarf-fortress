args={...}

local unit = df.unit.find(tonumber(args[1]))
local dir = tonumber(args[2])

pers,status = dfhack.persistent.get('teleport')
if pers.ints[7] == 1 then
	if dir == 1 then
		local unitoccupancy = dfhack.maps.getTileBlock(unit.pos).occupancy[unit.pos.x%16][unit.pos.y%16]
		unit.pos.x = pers.ints[1]
		unit.pos.y = pers.ints[2]
		unit.pos.z = pers.ints[3]
		if not unit.flags1.on_ground then unitoccupancy.unit = false else unitoccupancy.unit_grounded = false end
	elseif dir == 2 then
		local unitoccupancy = dfhack.maps.getTileBlock(unit.pos).occupancy[unit.pos.x%16][unit.pos.y%16]
		unit.pos.x = pers.ints[4]
		unit.pos.y = pers.ints[5]
		unit.pos.z = pers.ints[6]
		if not unit.flags1.on_ground then unitoccupancy.unit = false else unitoccupancy.unit_grounded = false end
	else
		print('not a valid portal imput')
	end
else
	print('no valid portals')
end