-- Adds extra functionality to projectiles. Use the argument "disable" (minus quotes) to disable.

flowtypes = {
miasma = 0,
mist = 1,
mist2 = 2,
dust = 3,
lavaMist = 4,
smoke = 5,
dragonFire = 6,
fireBreath = 7,
web = 8,
undirectedGas = 9,
undirectedVapor = 10,
oceanWave = 11,
seaFoam = 12
}

local function posIsEqual(pos1,pos2)
	if pos1.x ~= pos2.x or pos1.y ~= pos2.y or pos1.z ~= pos2.z then return false end
	return true
end

local function getMaterial(item)
	if not dfhack.matinfo.decode(item) then return nil end
	return dfhack.matinfo.decode(item).material
end

local function getSyndrome(material)
	if material==nil then return nil end
	if #material.syndrome>0 then return material.syndrome[0]
	else return nil end
end

local function removeItem(item)
	item.flags.garbage_collect = true
end

local function findInorganicWithName(matString)
	for inorganicID,material in ipairs(df.global.world.raws.inorganics) do
		if material.id == matString then return inorganicID end
	end
	return nil
end

local function getScriptFromMaterial(material)
	local commandStart
	local commandEnd
	local reactionClasses = material.reaction_class
	for classNumber,reactionClass in ipairs(reactionClasses) do
		if reactionClass.value == "\\COMMAND" then commandStart = classNumber end
		if reactionClass.value == "\\ENDCOMMAND" then commandEnd = classNumber break end
	end
	local script = {}
	if commandStart and commandEnd then
		for i = commandStart+1, commandEnd-1, 1 do
			if reactionClasses[i].value ~= "\\UNIT_HIT_ID" then table.insert(script,reactionClasses[i].value)
			else table.insert(script,"unit")
			end
		end
	end
	return script
end

local function getUnitHitByProjectile(projectile)
	for uid,unit in ipairs(df.global.world.units.active) do
		if posIsEqual(unit.pos,projectile.cur_pos) then return uid end
	end
	return nil
end

local function matCausesSyndrome(material)
	for _,reactionClass in ipairs(material.reaction_class) do
		if reactionClass.value == "DFHACK_CAUSES_SYNDROME" then return true end --the syndrome is the syndrome local to the projectile material
	end
	return false
end

local function alreadyHasSyndrome(unit,syn_id)
	for _,syndrome in ipairs(unit.syndromes.active) do
		if syndrome.type == syn_id then return true end
	end
	return false
end

local function assignSyndrome(target,syn_id) --taken straight from here, but edited so I can understand it better: https://gist.github.com/warmist/4061959/
    if target==nil then
        return nil
    end
	if alreadyHasSyndrome(target,syn_id) then
		local syndrome
		for k,v in ipairs(target.syndromes.active) do
			if v.type == syn_id then syndrome = v end
		end
		if not syndrome then return nil end
		syndrome.ticks=1
		return true
	end
    local newSyndrome=df.unit_syndrome:new()
    local target_syndrome=df.syndrome.find(syn_id)
    newSyndrome.type=target_syndrome.id
    --newSyndrome.year=
    --newSyndrome.year_time=
    newSyndrome.ticks=1
    newSyndrome.unk1=1
    for k,v in ipairs(target_syndrome.ce) do
        local sympt=df.unit_syndrome.T_symptoms:new()
        sympt.ticks=1
        sympt.flags=2
        newSyndrome.symptoms:insert("#",sympt)
    end
    target.syndromes.active:insert("#",newSyndrome)
	return true
end

function getProjectileExpansionFlags(material)
	local projectileExpansionFlags = {
	matWantsSpecificInorganic = 0,
	matWantsSpecificSize      = 50000,
	matCausesDragonFire       = false,
	matCausesMiasma           = false,
	matCausesMist             = false,
	matCausesMist2            = false,
	matCausesDust             = false,
	matCausesLavaMist         = false,
	matCausesSmoke            = false,
	matCausesFireBreath       = false,
	matCausesWeb              = false,
	matCausesUndirectedGas    = false,
	matCausesUndirectedVapor  = false,
	matCausesOceanWave        = false,
	matCausesSeaFoam          = false,
	matHasScriptAttached      = false,
	matCausesSyndrome         = false,
	matDisappearsOnHit        = false
	}
	local matName  = nil
	for k,reactionClass in ipairs(material.reaction_class) do
		if debugProjExp then print("checking reaction class #" .. k .. "...",reactionClass.value) end
		if string.find(reactionClass.value,"DFHACK") then 
			if debugProjExp then print("DFHack reaction class found!") end
			if reactionClass.value == "DFHACK_SPECIFIC_MAT"     then matName                                           = material.reaction_class[k+1].value           end
			if reactionClass.value == "DFHACK_FLOW_SIZE"        then projectileExpansionFlags.matWantsSpecificSize     = tonumber(material.reaction_class[k+1].value) end
			if reactionClass.value == "DFHACK_CAUSES_SYNDROME"  then projectileExpansionFlags.matCausesSyndrome        = true            end		
			if reactionClass.value == "DFHACK_DRAGONFIRE"       then projectileExpansionFlags.matCausesDragonFire      = true            end
			if reactionClass.value == "DFHACK_MIASMA"           then projectileExpansionFlags.matCausesMiasma          = true            end
			if reactionClass.value == "DFHACK_MIST"             then projectileExpansionFlags.matCausesMist            = true            end
			if reactionClass.value == "DFHACK_MIST2"            then projectileExpansionFlags.matCausesMist2           = true            end
			if reactionClass.value == "DFHACK_DUST"             then projectileExpansionFlags.matCausesDust            = true            end
			if reactionClass.value == "DFHACK_LAVAMIST"         then projectileExpansionFlags.matCausesLavaMist        = true            end
			if reactionClass.value == "DFHACK_SMOKE"            then projectileExpansionFlags.matCausesSmoke           = true            end
			if reactionClass.value == "DFHACK_FIREBREATH"       then projectileExpansionFlags.matCausesFireBreath      = true            end
			if reactionClass.value == "DFHACK_WEB"              then projectileExpansionFlags.matCausesWeb             = true            end
			if reactionClass.value == "DFHACK_GAS_UNDIRECTED"   then projectileExpansionFlags.matCausesUndirectedGas   = true            end
			if reactionClass.value == "DFHACK_VAPOR_UNDIRECTED" then projectileExpansionFlags.matCausesUndirectedVapor = true            end
			if reactionClass.value == "DFHACK_OCEAN_WAVE"       then projectileExpansionFlags.matCausesOceanWave       = true            end
			if reactionClass.value == "DFHACK_SEA_FOAM"         then projectileExpansionFlags.matCausesSeaFoam         = true            end
			if reactionClass.value == "DFHACK_DISAPPEARS"		then projectileExpansionFlags.matDisappearsOnHit	   = true            end
		end
		if reactionClass.value == "\\COMMAND"                   then projectileExpansionFlags.matHasScriptAttached	   = true end
	end
	if matName then projectileExpansionFlags.matWantsSpecificInorganic = findInorganicWithName(matName) end
	return projectileExpansionFlags
end

debugProjExp=false

events=require "plugins.eventful"
events.onProjItemCheckImpact.expansion=function(projectile)
	if debugProjExp then print("Thwack! Projectile item hit. Running projectileExpansion.") end
	if projectile then
		if debugProjExp then print("Found the item. Working on it.") end
		local material = getMaterial(projectile.item)
		if not material then return nil end
		local projectileExpansionFlags=getProjectileExpansionFlags(material)
		if debugProjExp then print(projectileExpansionFlags) printall(projectileExpansionFlags) end
		local syndrome = getSyndrome(material)
		local emissionMat = projectileExpansionFlags.matWantsSpecificInorganic --defaults to iron
		local flowSize = projectileExpansionFlags.matWantsSpecificSize         --defaults to 50000
		if projectileExpansionFlags.matCausesDragonFire      then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.dragonFire,0,0,flowSize) end
		if projectileExpansionFlags.matCausesMiasma          then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.miasma,0,0,flowSize) end
		if projectileExpansionFlags.matCausesMist            then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.mist,0,0,flowSize) end
		if projectileExpansionFlags.matCausesMist2           then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.mist2,0,0,flowSize) end
		if projectileExpansionFlags.matCausesDust            then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.dust,0,emissionMat,flowSize) end
		if projectileExpansionFlags.matCausesLavaMist        then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.lavaMist,0,emissionMat,flowSize) end
		if projectileExpansionFlags.matCausesSmoke           then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.smoke,0,0,flowSize) end
		if projectileExpansionFlags.matCausesFireBreath      then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.fireBreath,0,0,flowSize) end
		if projectileExpansionFlags.matCausesWeb             then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.web,0,emissionMat,flowSize) end
		if projectileExpansionFlags.matCausesUndirectedGas   then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.undirectedGas,0,emissionMat,flowSize) end
		if projectileExpansionFlags.matCausesUndirectedVapor then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.undirectedVapor,0,emissionMat,flowSize) end
		if projectileExpansionFlags.matCausesOceanWave       then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.oceanWave,0,0,flowSize) end
		if projectileExpansionFlags.matCausesSeaFoam         then dfhack.maps.spawnFlow(projectile.cur_pos,flowtypes.seaFoam,0,0,flowSize) end
		if projectileExpansionFlags.matHasScriptAttached or projectileExpansionFlags.matCausesSyndrome then
			local unit = getUnitHitByProjectile(projectile)		
			if projectileExpansionFlags.matHasScriptAttached then
				local script = getScriptFromMaterial(material)
				for k,v in ipairs(script) do
					if script[k] == "unit" then script[k] = tonumber(unit) end --that is the silliest code I have ever seen, hehe. If that particular entry in the script is the string "unit", it replaces the string "unit" with the number unit, which is the ID of a particular unit. It's similar to autosyndrome in that way.
				end
					dfhack.run_script(table.unpack(script))
				end
			if projectileExpansionFlags.matCausesSyndrome then assignSyndrome(unit,syndrome) end
		end
		if projectileExpansionFlags.matDisappearsOnHit then projectile.flags.to_be_deleted=true end
	end
	return true
end

if ... ~= "disable" then 
	print("Enabled projectileExpansion.")
else
	events.onProjItemCheckImpact.expansion = nil
	print("Disabled projectileExpansion.")
end