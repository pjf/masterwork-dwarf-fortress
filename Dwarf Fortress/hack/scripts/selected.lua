args={...}

function split(str, pat)
   local t = {}  -- NOTE: use {n = 0} in Lua-5.0
   local fpat = "(.-)" .. pat
   local last_end = 1
   local s, e, cap = str:find(fpat, 1)
   while s do
      if s ~= 1 or cap ~= "" then
	 table.insert(t,cap)
      end
      last_end = e+1
      s, e, cap = str:find(fpat, last_end)
   end
   if last_end <= #str then
      cap = str:sub(last_end)
      table.insert(t, cap)
   end
   return t
end

function getAttrValue(unit,attr,mental)
	if unit.curse.attr_change then
		if mental then
			return (unit.status.current_soul.mental_attrs[attr].value+unit.curse.attr_change.ment_att_add[attr])*unit.curse.attr_change.ment_att_perc[attr]/100
		else
			return (unit.body.physical_attrs[attr].value+unit.curse.attr_change.phys_att_add[attr])*unit.curse.attr_change.phys_att_perc[attr]/100
		end
	else
		if mental then
			return unit.status.current_soul[attr].value
		else
			return unit.body.physical_attrs[attr].value
		end
	end
end

function isSelected(unit,unitTarget,args)

	local s1 = '@'
	local s2 = '/'
	local s3 = ','
	local s4 = ';'

	local value = {
	radius = '0,0,0',
	target = 'all',
	aclass = 'NONE',
	acreature = 'NONE',
	asyndrome = 'NONE',
	atoken = 'NONE',
	iclass = 'NONE',
	icreature = 'NONE',
	isyndrome = 'NONE',
	itoken = 'NONE',
	physical = 'NONE',
	mental = 'NONE',
	skills = 'NONE',
	traits = 'NONE',
	age = 'NONE',
	noble = 'NONE',
	speed = 'NONE',
	profession = 'NONE'
	}

-- Arguments Analysis	
	local temp = 'NONE'
	for i = 1, #args, 1 do
		if string.match(args[i],s1) ~= nil then
			temp = split(args[i],s1)
			value[temp[1]] = temp[2]
		end
	end

-- Value Determination
	local rx = tonumber(split(value['radius'],',')[1])
	local ry = tonumber(split(value['radius'],',')[2])
	local rz = tonumber(split(value['radius'],',')[3])
	local target = value['target']
	local aclass = value['aclass']
	local acreature = value['acreature']
	local asyndrome = value['asyndrome']
	local atoken = value['atoken']
	local iclass = value['iclass']
	local icreature = value['icreature']
	local isyndrome = value['isyndrome']
	local itoken = value['itoken']
	local physical = value['physical']
	local mental = value['mental']
	local skills = value['skills']
	local traits = value['traits']
	local age = value['age']
	local noble = value['noble']
	local speed = value['speed']
	local profession = value['profession']

-- Raws Analysis	
	local unitraws = df.creature_raw.find(unitTarget.race)
	local casteraws = unitraws.caste[unitTarget.caste]
	local unitracename = unitraws.creature_id
	local castename = casteraws.caste_id
	local unitclasses = casteraws.creature_class
	local actives = unitTarget.syndromes.active
	local syndromes = df.global.world.raws.syndromes.all
	local flags1 = unitraws.flags
	local flags2 = casteraws.flags
	local tokens = {}
	for k,v in pairs(flags1) do
		tokens[k] = v
	end
	for k,v in pairs(flags2) do
		tokens[k] = v
	end

-- Distance Check
	local pos = {dfhack.units.getPosition(unitTarget)}

	local mapx, mapy, mapz = dfhack.maps.getTileSize()
	local xmin = unit.pos.x - rx
	local xmax = unit.pos.x + rx
	local ymin = unit.pos.y - ry
	local ymax = unit.pos.y + ry
	local zmin = unit.pos.z - rz
	local zmax = unit.pos.z + rz
	if xmin < 1 then xmin = 1 end
	if ymin < 1 then ymin = 1 end
	if zmin < 1 then zmin = 1 end
	if xmax > mapx then xmax = mapx-1 end
	if ymax > mapy then ymax = mapy-1 end
	if zmax > mapz then zmax = mapz-1 end

	if pos[1] < xmin or pos[1] > xmax then return false end
	if pos[2] < ymin or pos[2] > ymax then return false end
	if pos[3] < zmin or pos[3] > zmax then return false end

-- Target Check	
	if target == 'enemy' then
		if unit.civ_id == unitTarget.civ_id then return false end
	elseif target == 'civ' then
		if unit.civ_id ~= unitTarget.civ_id then return false end
	end

-- Age Check
	if age ~= 'NONE' then
		local uage = dfhack.units.getAge(unitTarget)
		local agea = split(age,s3)
		for _,x in ipairs(agea) do
			if split(x,s2)[1] == 'min' then
				if tonumber(split(x,s2)[2]) < uage then return false end
			elseif split(x,s2)[1] == 'max' then
				if tonumber(split(x,s2)[2]) > uage then return false end
			end
		end
	end

-- Speed Check
	if speed ~= 'NONE' then
		local uspeed = dfhack.units.computeMovementSpeed(unitTarget)
		local speeda = split(speed,s3)
		for _,x in ipairs(speeda) do
			if split(x,s2)[1] == 'min' then
				if tonumber(split(x,s2)[2]) < uspeed then return false end
			elseif split(x,s2)[1] == 'max' then
				if tonumber(split(x,s2)[2]) > uspeed then return false end
			end
		end
	end

-- Physical Attributes Check
	if physical ~= 'NONE' then
		local physicala = split(physical,s3)
		for _,x in ipairs(physicala) do
			local uphysical = getAttrValue(unitTarget,split(x,s2)[2])
			if split(x,s2)[1] == 'min' then
				if tonumber(split(age,s2)[3]) < uphysical then return false end
			elseif split(x,s2)[1] == 'max' then
				if tonumber(split(age,s2)[3]) > uphysical then return false end
			end
		end
	end

-- Mental Attributes Check
	if mental ~= 'NONE' then
		local mentala = split(mental,s3)
		for _,x in ipairs(mentala) do
			local umental = getAttrValue(unitTarget,split(x,s2)[2],true)
			if split(x,s2)[1] == 'min' then
				if tonumber(split(age,s2)[3]) < umental then return false end
			elseif split(x,s2)[1] == 'max' then
				if tonumber(split(age,s2)[3]) > umental then return false end
			end
		end
	end

-- Skill Level Check
	if skills ~= 'NONE' then
		local skilla = split(skills,s3)
		for _,x in ipairs(skilla) do
			local uskill = dfhack.units.getEffectiveSkill(unitTarget,df.job_skill[split(x,'/')[2]])
			if split(x,s2)[1] == 'min' then
				if tonumber(split(age,s2)[3]) < uskill then return false end
			elseif split(x,s2)[1] == 'max' then
				if tonumber(split(age,s2)[3]) > uskill then return false end
			end
		end
	end

-- Noble Check
	if noble ~= 'NONE' then
		local noblea = split(noble,s3)
		local unoblea = dfhack.units.getNoblePositions(unitTarget)
		for _,x in ipairs(noblea) do
			for _,y in ipairs(unoblea) do
				if split(x,s2)[1] == 'required' then
					if split(x,s2)[2] ~= y.position.code then return false end
				elseif split(x,s2)[1] == 'immune' then
					if split(x,s2)[2] == y.position.code then return false end
				end
			end
		end
	end

-- Profession Check
	if profession ~= 'NONE' then
		local professiona = split(profession,s3)
		local uprofession = unitTarget.profession
		for _,x in ipairs(professiona) do
			print(split(x,s2)[2],uprofession)
			if split(x,s2)[1] == 'required' then
				if df.profession[split(x,s2)[2]] ~= uprofession then return false end
			elseif split(x,s2)[1] == 'immune' then
				if df.profession[split(x,s2)[2]] == uprofession then return false end
			end
		end
	end

-- Trait Check
	if traits ~= 'NONE' then
		local traita = split(traits,s3)
		local utraita = unitTarget.status.current_soul.traits
		for _,x in ipairs(traita) do
			for z,y in pairs(utraita) do
				if split(x,s2)[2] == z then
					if split(x,s2)[1] == 'min' then
						if tonumber(split(x,s2)[3]) < y then return false end
					elseif split(x,s2)[1] == 'max' then
						if tonumber(split(x,s2)[3]) > y then return false end
					end
				end
			end
		end
	end

-- Immune Creature Class Check
	if iclass ~= 'NONE' then
		local iclassa = split(iclass,s3)
		for _,unitclass in ipairs(unitclasses) do
			for _,x in ipairs(iclassa) do
				if x == unitclass.value then return false end
			end
		end
	end

-- Immune Creature/Caste Check	
	if icreature ~= 'NONE' then
		local icreaturea = split(icreature,s3)
		for _,x in ipairs(icreaturea) do
			local xsplit = split(x,s4)
			if xsplit[1] == unitracename and xsplit[2] == castename then return false end
		end
	end

-- Immune Syndrome Class Check	
	if isyndrome ~= 'NONE' then
		local isyndromea = split(isyndrome,s3)
		for _,x in ipairs(actives) do
			local synclass=syndromes[x.type].syn_class
			for _,y in ipairs(synclass) do
				for _,z in ipairs(isyndromea) do
					if z == y.value then return false end
				end
			end
		end
	end

-- Immune Creature Tokens Check	
	if itoken ~= 'NONE' then
		local itokena = split(itoken,s3)
		for _,x in ipairs(itokena) do
			if tokens[x] then return false end
		end
	end

	if aclass == 'NONE' and acreature == 'NONE' and asyndrome == 'NONE' and atoken == 'NONE' then return true end

-- Required Creature Class Check	
	if aclass ~= 'NONE' then
		local aclassa = split(aclass,s3)
		for _,unitclass in ipairs(unitclasses) do
			for _,x in ipairs(aclassa) do
				if x == unitclass.value then return true end
			end
		end
	end

-- Required Creature/Caste Check	
	if acreature ~= 'NONE' then
		local acreaturea = split(acreature,s3)
		for _,x in ipairs(acreaturea) do
			local xsplit = split(x,s4)
			if xsplit[1] == unitracename and xsplit[2] == castename then return true end
		end
	end

-- Required Syndrome Class Check	
	if asyndrome ~= 'NONE' then
		local asyndromea = split(asyndrome,s3)
		for _,x in ipairs(actives) do
			local synclass=syndromes[x.type].syn_class
			for _,y in ipairs(synclass) do
				for _,z in ipairs(asyndromea) do
					if z == y.value then return true end
				end
			end
		end
	end

-- Required Creature Tokens Check
	if atoken ~= 'NONE' then
		local atokena = split(atoken,s3)
		for _,x in ipairs(atokena) do
			if not tokens[x] then return false end
		end
	end

	return false
end

local unit = df.unit.find(args[1])

isSelected(unit,unit,args)