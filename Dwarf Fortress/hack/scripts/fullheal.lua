-- attempt to fully heal a selected unit, option -r to attempt to resurrect the unit
local unit=dfhack.gui.getSelectedUnit()
for _,arg in ipairs({...}) do
    if arg == "-r" or arg == "-R" then resurrect  = true end
end

if unit then
	if resurrect then
		if unit.flags1.dead then
			print("Resurrecting...")
			unit.flags2.slaughter = false
			unit.flags3.scuttle = false
		end
		unit.flags1.dead = false
		unit.flags2.killed = false
		unit.flags3.ghostly = false
		--unit.unk_100 = 3
	end
	
	print("Erasing wounds...")
	while #unit.body.wounds > 0 do
		unit.body.wounds:erase(#unit.body.wounds-1)
	end
	unit.body.wound_next_id=1

	print("Refilling blood...")
	unit.body.blood_count=unit.body.blood_max

	print("Resetting grasp/stand status...")
	unit.status2.limbs_stand_count=unit.status2.limbs_stand_max
	unit.status2.limbs_grasp_count=unit.status2.limbs_grasp_max

	print("Resetting status flags...")
	unit.flags2.has_breaks=false
	unit.flags2.gutted=false
	unit.flags2.circulatory_spray=false
	unit.flags2.vision_good=true
	unit.flags2.vision_damaged=false
	unit.flags2.vision_missing=false
	unit.flags2.breathing_good=true
	unit.flags2.breathing_problem=false

	unit.flags2.calculated_nerves=false
	unit.flags2.calculated_bodyparts=false
	unit.flags2.calculated_insulation=false
	unit.flags3.compute_health=true

	print("Resetting counters...")
	unit.counters.winded=0
	unit.counters.stunned=0
	unit.counters.unconscious=0
	unit.counters.webbed=0
	unit.counters.pain=0
	unit.counters.nausea=0
	unit.counters.dizziness=0

	unit.counters2.paralysis=0
	unit.counters2.fever=0
	unit.counters2.exhaustion=0
	unit.counters2.hunger_timer=0
	unit.counters2.thirst_timer=0
	unit.counters2.sleepiness_timer=0
	unit.counters2.vomit_timeout=0
	
	print("Resetting body part status...")
	v=unit.body.components
	for i=0,#v.body_layer_328 - 1,1 do
		v.body_layer_328[i] = 100	-- percent remaining of fluid layers (Urist Da Vinci)
	end

	v=unit.body.components
	for i=0,#v.body_layer_338 - 1,1 do
		v.body_layer_338[i] = 0		-- severed, leaking layers (Urist Da Vinci)
		v.body_layer_348[i] = 0		-- wound contact areas (Urist Da Vinci)
		v.body_layer_358[i] = 0		-- 100*surface percentage of cuts/fractures on the body part layer (Urist Da Vinci)
		v.body_layer_368[i] = 0		-- 100*surface percentage of dents on the body part layer (Urist Da Vinci)
		v.body_layer_378[i] = 0		-- 100*surface percentage of "effects" on the body part layer (Urist Da Vinci)
	end
	
	v=unit.body.components.body_part_status
	for i=0,#v-1,1 do
		v[i].on_fire = false
		v[i].missing = false
		v[i].organ_loss = false
		v[i].organ_damage = false
		v[i].muscle_loss = false
		v[i].muscle_damage = false
		v[i].bone_loss = false
		v[i].bone_damage = false
		v[i].skin_damage = false
		v[i].motor_nerve_severed = false
		v[i].sensory_nerve_severed = false
	end
	
	if unit.job.current_job and unit.job.current_job.job_type == df.job_type.Rest then
		print("Wake from rest -> clean self...")
		unit.job.current_job = df.job_type.CleanSelf
	end
end