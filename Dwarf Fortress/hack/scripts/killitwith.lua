--causes targeted unit to die in several possible manners
unit=dfhack.gui.getSelectedUnit()
if unit==nil then
	 print ("No unit under cursor!  Aborting!")
	 return
	 end

if unit.flags1.dead==true then
	 print ("Dead unit!  Aborting!")
	 return
	 end

--fire heat cold gravity thirst hunger time(age)
--blood violence(berserk) water(drown) sabotage(scuttle)

args={...}

if #args ~= 1 then
	 print ("Use one of the following arguments when calling the script (all lowercase!):")
	 print ("fire")
	 print ("heat")
	 print ("cold")
	 print ("gravity")
	 print ("thirst")
	 print ("hunger")
	 print ("time")
	 print ("blood")
	 print ("violence")
	 print ("water")
	 print ("sabotage")
	 return
	 end

if args[1]=="fire" then
	for k,v in pairs(unit.body.components.body_part_status) do
		unit.body.components.body_part_status[k].on_fire=true
	end
	unit.flags3.body_temp_in_range=false
	return
end


if args[1]=="heat" then
	for k,v in pairs(unit.status2.body_part_temperature) do
		unit.status2.body_part_temperature[k].whole=12000
	end
	return
end


if args[1]=="cold" then
	for k,v in pairs(unit.status2.body_part_temperature) do
		unit.status2.body_part_temperature[k].whole=0
	end
	return
end


if args[1]=="gravity" then
if unit.flags1.projectile==true then
	 print ("Already a projectile!  Aborting!")
	 return
	 end
if unit.flags1.rider==true or unit.flags1.caged==true or unit.flags2.swimming==true or unit.flags3.exit_vehicle1==true or 

unit.flags3.exit_vehicle2==true then
	 print ("Not eligible (rider, caged, swimming, minecart)!  Aborting!")
	 return
	 end
local count=0
local l = df.global.world.proj_list
local lastlist=l
l=l.next
    while l do
      count=count+1
	if l.next==nil then
		lastlist=l
	end
      l = l.next
    end
newlist = df.proj_list_link:new()
lastlist.next=newlist
newlist.prev=lastlist
proj = df.proj_unitst:new()
newlist.item=proj
proj.link=newlist
proj.id=df.global.proj_next_id
df.global.proj_next_id=df.global.proj_next_id+1
proj.unit=unit
proj.origin_pos.x=unit.pos.x
proj.origin_pos.y=unit.pos.y
proj.origin_pos.z=unit.pos.z
proj.prev_pos.x=unit.pos.x
proj.prev_pos.y=unit.pos.y
proj.prev_pos.z=unit.pos.z
proj.cur_pos.x=unit.pos.x
proj.cur_pos.y=unit.pos.y
proj.cur_pos.z=unit.pos.z
proj.flags.no_impact_destroy=true
proj.flags.piercing=true
proj.flags.parabolic=true
proj.flags.unk9=true
proj.speed_z=-200000
unitoccupancy = dfhack.maps.getTileBlock(unit.pos).occupancy[unit.pos.x%16][unit.pos.y%16]
if not unit.flags1.on_ground then 
	unitoccupancy.unit = false 
else 
	unitoccupancy.unit_grounded = false 
end
unit.flags1.projectile=true
unit.flags1.on_ground=false
	return
end


if args[1]=="thirst" then
	unit.counters2.thirst_timer=6000000
	return
end


if args[1]=="hunger" then
	unit.counters2.hunger_timer=9000000
	unit.counters2.stored_fat=0
	unit.counters2.stomach_food=0
	return
end


if args[1]=="time" then
	print("old age placeholder")
	return
end


if args[1]=="blood" then
	unit.body.blood_count=0
	return
end


if args[1]=="violence" then
if unit.status.current_soul==nil then
	 print ("Soulless unit!  Aborting!")
	 return
	 end
	unit.status.happiness=0
	unit.status.insanity_chance=100
	unit.status.current_soul.traits.ANGER=100
	unit.status.current_soul.traits.VULNERABILITY=80
	--eventually makes the unit go berserk
	return
end


if args[1]=="water" then
	print("drowning placeholder")
	return
end


if args[1]=="sabotage" then
	unit.flags3.scuttle=true
	return
end


--none of the above
	 print ("Use one of the following arguments when calling the script (all lowercase!):")
	 print ("fire")
	 print ("heat")
	 print ("cold")
	 print ("gravity")
	 print ("thirst")
	 print ("hunger")
	 print ("time")
	 print ("blood")
	 print ("violence")
	 print ("water")
	 print ("sabotage")
--done