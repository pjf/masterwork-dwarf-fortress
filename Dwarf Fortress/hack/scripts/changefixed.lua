--[[
Description: Changes the counter value for all creatures in a radius.

Use: 
[SYN_CLASS:\COMMAND]
[SYN_CLASS:radiuseffect]
[SYN_CLASS:type]
[SYN_CLASS:\UNIT_ID]
[SYN_CLASS:radius]
[SYN_CLASS:strength]
[SYN_CLASS:target]
[SYN_CLASS:affected class]
[SYN_CLASS:affected creature]
[SYN_CLASS:affected syndrome class]
[SYN_CLASS:required tokens]
[SYN_CLASS:immune class]
[SYN_CLASS:immune creature]
[SYN_CLASS:immune syndrome class]
[SYN_CLASS:forbidden tokens]

type = counter that is changed (VALID TOKENS: web, stun, winded, unconscious, pain, nausea, dizziness, paralysis, numbness, fever, exhaustion, hunger, thirst, sleep, infection)
radius = x,y,z extent of the effect from the target creature, if x=y=z=-1 then will only be target creature, if x=y=z=0 then will only be target creatures space (VALID TOKEN: INTEGER[-1 - mapsize])
strength = amount to change counter by, note that some counters have different maximums from others, can be used to decrease counters with negative numbers (VALID TOKENS: INTEGER)
target = who is eligible to be targeted (VALID TOKENS: enemy, civ, all)
affected class = creature class that is affected, use NONE to skip this check (VALID TOKENS: ANY CREATURE CLASS TOKENS {seperated by commas}, or NONE)
affected creature = creature that is affected, use NONE to skip this check (VALID TOKENS: ANY CREATURE;CASTE COMBINATIONS {seperated by commas}, or NONE)
affected syndrome class = syndrome class that is affected, use NONE to skip this check (VALID TOKENS: ANY SYNDROME CLASS TOKENS {seperated by commas}, or NONE)
require tokens = tokens that are required, use NONE to skip this check (VALID TOKENS: ANY TOKENS FOUND IN 'tokens.txt')
immune class = creature class that is immune, use NONE to skip this check (VALID TOKENS: ANY CREATURE CLASS TOKENS {seperated by commas}, or NONE)
immune creature = creature that is immune, use NONE to skip this check (VALID TOKENS: ANY CREATURE;CASTE COMBINATIONS {seperated by commas}, or NONE)
immune syndrome class = syndrome class that is immune, use NONE to skip this check (VALID TOKENS: ANY SYNDROME CLASS TOKENS {seperated by commas}, or NONE)
forbidden tokens = tokens that are forbidden, use NONE to skip this check (VALID TOKENS: ANY TOKENS FOUND IN 'tokens.txt')

Example:
[INTERACTION:SPELL_NATURE_PLANT_ENTANGLE]
        [I_SOURCE:CREATURE_ACTION]
        [I_TARGET:C:CREATURE]
                [IT_LOCATION:CONTEXT_CREATURE]
                [IT_MANUAL_INPUT:target]
        [I_EFFECT:ADD_SYNDROME]
                [IE_TARGET:C]
                [IE_IMMEDIATE]
                [SYNDROME]
                        [SYN_CLASS:\COMMAND]
                        [SYN_CLASS:radiuseffect]
                        [SYN_CLASS:web]
                        [SYN_CLASS:\UNIT_ID]
                        [SYN_CLASS:10,10,0]
                        [SYN_CLASS:10000]
                        [SYN_CLASS:enemy]
                        [SYN_CLASS:NONE]
                        [SYN_CLASS:NONE]
                        [SYN_CLASS:NONE]
                        [SYN_CLASS:NONE]
                        [SYN_CLASS:PLANT4,PLANT3]
                        [SYN_CLASS:ELEMENTAL;EARTH,GREATER_ELEMENTAL;EARTH]
                        [SYN_CLASS:MAGIC_IMMUNITY,RESIST_MAGIC]
                        [SYN_CLASS:NONE]
                        [CE_SPEED_CHANGE:SPEED_PERC:100:START:0:END:1]

]]
args={...}

local function split(str, pat)
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

local function isSelected(unit,unitTarget,rx,ry,rz,target,aclass,acreature,asyndrome,atoken,iclass,icreature,isyndrome,itoken)
        local pos = {dfhack.units.getPosition(unitTarget)}

        local mapx, mapy, mapz = dfhack.maps.getTileSize()
        local xmin = unit.pos.x - rx
        local xmax = unit.pos.x + rx
        local ymin = unit.pos.y - ry
        local ymax = unit.pos.y + ry
        local zmin = unit.pos.z
        local zmax = unit.pos.z + rz
        if xmin < 1 then xmin = 1 end
        if ymin < 1 then ymin = 1 end
        if xmax > mapx then xmax = mapx-1 end
        if ymax > mapy then ymax = mapy-1 end
        if zmax > mapz then zmax = mapz-1 end

        if pos[1] < xmin or pos[1] > xmax then return false end
        if pos[2] < ymin or pos[2] > ymax then return false end
        if pos[3] < zmin or pos[3] > zmax then return false end

        if target == 'enemy' then
                if unit.civ_id == unitTarget.civ_id then return false end
        elseif target == 'civ' then
                if unit.civ_id ~= unitTarget.civ_id then return false end
        end

        if aclass == 'NONE' and acreature == 'NONE' and asyndrome == 'NONE' and atoken == 'NONE' and iclass == 'NONE' and icreature == 'NONE' and isyndrome == 'NONE' and itoken == 'NONE' then return true end

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

        if iclass ~= 'NONE' then
                local iclassa = split(iclass,',')
                for _,unitclass in ipairs(unitclasses) do
                        for _,x in ipairs(iclassa) do
                                if x == unitclass.value then return false end
                        end
                end
        end

        if icreature ~= 'NONE' then
                local icreaturea = split(icreature,',')
                for _,x in ipairs(icreaturea) do
                        local xsplit = split(x,';')
                        if xsplit[1] == unitracename and xsplit[2] == castename then return false end
                end
        end

        if isyndrome ~= 'NONE' then
                local isyndromea = split(isyndrome,',')
                for _,x in ipairs(actives) do
                        local synclass=syndromes[x.type].syn_class
                        for _,y in ipairs(synclass) do
                                for _,z in ipairs(isyndromea) do
                                        if z == y.value then return false end
                                end
                        end
                end
        end

        if itoken ~= 'NONE' then
                local itokena = split(itoken,',')
                for _,x in ipairs(itokena) do
                        if tokens[x] then return false end
                end
        end

        if aclass ~= 'NONE' then
                local aclassa = split(aclass,',')
                for _,unitclass in ipairs(unitclasses) do
                        for _,x in ipairs(aclassa) do
                                if x == unitclass.value then return true end
                        end
                end
        end

        if acreature ~= 'NONE' then
                local acreaturea = split(acreature,',')
                for _,x in ipairs(acreaturea) do
                        local xsplit = split(x,';')
                        if xsplit[1] == unitracename and xsplit[2] == castename then return true end
                end
        end

        if asyndrome ~= 'NONE' then
                local asyndromea = split(asyndrome,',')
                for _,x in ipairs(actives) do
                        local synclass=syndromes[x.type].syn_class
                        for _,y in ipairs(synclass) do
                                for _,z in ipairs(asyndromea) do
                                        if z == y.value then return true end
                                end
                        end
                end
        end

        if atoken ~= 'NONE' then
                local atokena = split(atoken,',')
                for _,x in ipairs(atokena) do
                        if not tokens[x] then return false end
                end
        end

        if aclass == 'NONE' and acreature == 'NONE' and asyndrome == 'NONE' and atoken == 'NONE' then return true end

        return false
end

local function effect(etype,unit,radius,strength,target,aclass,acreature,asyndrome,atoken,iclass,icreature,isyndrome,itoken)
        local i
        local unitList = df.global.world.units.active
        local value = 0
        local int16 = 30000
        local int32 = 200000000
        local radiusa = split(radius,',')
        local rx = tonumber(radiusa[1])
        local ry = tonumber(radiusa[2])
        local rz = tonumber(radiusa[3])

        if (radius == -1) then
                local unitTarget = unit
                if isSelected(unit,unitTarget,0,0,0,target,aclass,acreature,asyndrome,atoken,iclass,icreature,isyndrome,itoken) then
                        if etype == 'web' then
                                value = unitTarget.counters.webbed + strength
                                if value > int16 then value = int16 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.webbed = value
                        end
                        if etype == 'stun' then
                                value = unitTarget.counters.stunned + strength
                                if value > int16 then value = int16 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.stunned = value
                        end
                        if etype == 'winded' then 
                                value = unitTarget.counters.winded + strength
                                if value > int16 then value = int16 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.winded = value
                        end
                        if etype == 'unconscious' then 
                                value = unitTarget.counters.unconscious + strength
                                if value > int16 then value = int16 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.unconscious = value
                        end
                        if etype == 'pain' then 
                                value = unitTarget.counters.pain + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.pain = value
                        end
                        if etype == 'nausea' then 
                                value = unitTarget.counters.nausea + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.nausea = value
                        end
                        if etype == 'dizziness' then 
                                value = unitTarget.counters.dizziness + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.dizziness = value
                        end
                        if etype == 'paralysis' then 
                                value = unitTarget.counters.paralysis + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.paralysis = value
                        end
                        if etype == 'numbness' then 
                                value = unitTarget.counters.numbness + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.numbness = value
                        end
                        if etype == 'fever' then 
                                value = unitTarget.counters.fever + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.fever = value
                        end
                        if etype == 'exhaustion' then 
                                value = unitTarget.counters.exhaustion + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.exhaustion = value
                        end
                        if etype == 'hunger' then 
                                value = unitTarget.counters.hunger_timer + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.hunger_timer = value
                        end
                        if etype == 'thirst' then 
                                value = unitTarget.counters.thirst_timer + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.thirst_timer = value
                        end
                        if etype == 'sleep' then 
                                value = unitTarget.counters.sleepiness_timer + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.counters.sleepiness_timer = value
                        end
                        if etype == 'infection' then 
                                value = unitTarget.body.infection_level + strength
                                if value > int32 then value = int32 end
                                if value < 0 then value = 0 end
                                unitTarget.body.infection_level = value
                        end
        else
                for i = #unitList - 1, 0, -1 do
                        local unitTarget = unitList[i]
                        if isSelected(unit,unitTarget,rx,ry,rz,target,aclass,acreature,asyndrome,atoken,iclass,icreature,isyndrome,itoken) then
                                if etype == 'web' then
                                        value = unitTarget.counters.webbed + strength
                                        if value > int16 then value = int16 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.webbed = value
                                end
                                if etype == 'stun' then
                                        value = unitTarget.counters.stunned + strength
                                        if value > int16 then value = int16 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.stunned = value
                                end
                                if etype == 'winded' then 
                                        value = unitTarget.counters.winded + strength
                                        if value > int16 then value = int16 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.winded = value
                                end
                                if etype == 'unconscious' then 
                                        value = unitTarget.counters.unconscious + strength
                                        if value > int16 then value = int16 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.unconscious = value
                                end
                                if etype == 'pain' then 
                                        value = unitTarget.counters.pain + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.pain = value
                                end
                                if etype == 'nausea' then 
                                        value = unitTarget.counters.nausea + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.nausea = value
                                end
                                if etype == 'dizziness' then 
                                        value = unitTarget.counters.dizziness + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.dizziness = value
                                end
                                if etype == 'paralysis' then 
                                        value = unitTarget.counters.paralysis + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.paralysis = value
                                end
                                if etype == 'numbness' then 
                                        value = unitTarget.counters.numbness + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.numbness = value
                                end
                                if etype == 'fever' then 
                                        value = unitTarget.counters.fever + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.fever = value
                                end
                                if etype == 'exhaustion' then 
                                        value = unitTarget.counters.exhaustion + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.exhaustion = value
                                end
                                if etype == 'hunger' then 
                                        value = unitTarget.counters.hunger_timer + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.hunger_timer = value
                                end
                                if etype == 'thirst' then 
                                        value = unitTarget.counters.thirst_timer + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.thirst_timer = value
                                end
                                if etype == 'sleep' then 
                                        value = unitTarget.counters.sleepiness_timer + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.counters.sleepiness_timer = value
                                end
                                if etype == 'infection' then 
                                        value = unitTarget.body.infection_level + strength
                                        if value > int32 then value = int32 end
                                        if value < 0 then value = 0 end
                                        unitTarget.body.infection_level = value
                                end
                        end
                end
        end
end

local etype = args[1]
local unit = df.unit.find(tonumber(args[2]))
local radius = args[3]
local strength = tonumber(args[4])
local target = args[5]
local aclass = args[6]
local acreature = args[7]
local asyndrome = args[8]
local atoken = args[9]
local iclass = args[10]
local icreature = args[11]
local isyndrome = args[12]
local itoken = args[13]

effect(etype,unit,radius,strength,target,aclass,acreature,asyndrome,atoken,iclass,icreature,isyndrome,itoken)