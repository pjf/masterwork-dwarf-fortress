-- Slam a unit into the ground
args={...}
--local unit=dfhack.gui.getSelectedUnit()
local unit = df.unit.find(tonumber(args[1]))

local strength=args[2]
 if unit == nil then
	print('No unit selected')
	return
end
if strength == nil then
	print('No argument for velocity found.  Try inputting "slam 100000"')
	return
end
 
if unit then
	local l = df.global.world.proj_list
	local lastlist=l
	l=l.next
	count = 0
	  while l do
		  count=count+1
			if l.next==nil then
					lastlist=l
			end
		  l = l.next
		end
 
	unitTarget=unit
 
	newlist = df.proj_list_link:new()
	lastlist.next=newlist
	newlist.prev=lastlist
	proj = df.proj_unitst:new()
	newlist.item=proj
	proj.link=newlist
	proj.id=df.global.proj_next_id
	df.global.proj_next_id=df.global.proj_next_id+1
	proj.unit=unitTarget
	proj.origin_pos.x=unitTarget.pos.x
	proj.origin_pos.y=unitTarget.pos.y
	proj.origin_pos.z=unitTarget.pos.z
	proj.prev_pos.x=unitTarget.pos.x
	proj.prev_pos.y=unitTarget.pos.y
	proj.prev_pos.z=unitTarget.pos.z
	proj.cur_pos.x=unitTarget.pos.x
	proj.cur_pos.y=unitTarget.pos.y
	proj.cur_pos.z=unitTarget.pos.z
	proj.flags.no_impact_destroy=true
	proj.flags.piercing=true
	proj.flags.parabolic=true
	proj.flags.unk9=true
	proj.speed_x=0
	proj.speed_y=0
	proj.speed_z=strength
	unitoccupancy = dfhack.maps.getTileBlock(unitTarget.pos).occupancy[unitTarget.pos.x%16][unitTarget.pos.y%16]
	if not unitTarget.flags1.on_ground then 
			unitoccupancy.unit = false 
	else 
			unitoccupancy.unit_grounded = false 
	end
	unitTarget.flags1.projectile=true
	unitTarget.flags1.on_ground=false
end