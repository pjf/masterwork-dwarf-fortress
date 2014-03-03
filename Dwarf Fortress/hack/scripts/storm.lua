--[[
Description: Creates a number of random explosions of a specific type using spawnflow (same type of code as Putnam's projectileExpansion). Hits in a radius around the targeted unit once. Only hits outside.

Use: 
[SYN_CLASS:\COMMAND][SYN_CLASS:storm][SYN_CLASS:type][SYN_CLASS:\UNIT_ID][SYN_CLASS:radius][SYN_CLASS:number][SYN_CLASS:strength][SYN_CLASS:inorganic]
type = counter that is changed (VALID TOKENS: miasma, mist, mist2, dust, lavamist, smoke, dragonfire, firebreath, web, undirectedgas, undirectedvapor, oceanwave, seafoam)
radius = number of tiles away from target creature you want flows to be spawned in the x-y plane (VALID TOKENS: INTEGER[0 - map size])
number = amount of flows to spawn (VALID TOKENS: INTEGER[1+])
strength = size of the flow to spawn (VALID TOKENS: INTEGER[1+])
inorganic = some of the flows take an inorganic as a further argument for dust, lavamist, web, undirectedgas, and undirectedvapor add this extra syndrome class (VALID TOKENS: INORGANIC_SUBTYPE)

Example: 
[INTERACTION:SPELL_ELEMENTAL_FIRE_METEOR_STORM]
        [I_SOURCE:CREATURE_ACTION]
        [I_TARGET:C:CREATURE]
                [IT_LOCATION:CONTEXT_CREATURE]
                [IT_MANUAL_INPUT:target]
        [I_EFFECT:ADD_SYNDROME]
                [IE_TARGET:C]
                [IE_IMMEDIATE]
                [SYNDROME]
                        [SYN_CLASS:\COMMAND]
                        [SYN_CLASS:storm]
                        [SYN_CLASS:firebreath]
                        [SYN_CLASS:\UNIT_ID]
                        [SYN_CLASS:25]
                        [SYN_CLASS:5]
                        [SYN_CLASS:50]
                        [CE_SPEED_CHANGE:SPEED_PERC:100:START:0:END:1]

]]

args={...}

flowtypes = {
miasma = 0,
mist = 1,
mist2 = 2,
dust = 3,
lavamist = 4,
smoke = 5,
dragonfire = 6,
firebreath = 7,
web = 8,
undirectedgas = 9,
undirectedvapor = 10,
oceanwave = 11,
seafoam = 12
}

function storm(stype,unit,radius,number,itype,strength)

        local i
        local rando = dfhack.random.new()
        local snum = flowtypes[stype]
        local inum = 0
        if itype ~= 0 then
                inum = dfhack.matinfo.find(itype).index
        end

        local mapx, mapy, mapz = dfhack.maps.getTileSize()
        local xmin = unit.pos.x - radius
        local xmax = unit.pos.x + radius
        local ymin = unit.pos.y - radius
        local ymax = unit.pos.y + radius
        if xmin < 1 then xmin = 1 end
        if ymin < 1 then ymin = 1 end
        if xmax > mapx then xmax = mapx-1 end
        if ymax > mapy then ymax = mapy-1 end

        local dx = xmax - xmin
        local dy = ymax - ymin
        local pos = {}
        pos.x = 0
        pos.y = 0
        pos.z = 0

        for i = 1, number, 1 do

                local rollx = rando:random(dx) - radius
                local rolly = rando:random(dy) - radius

                pos.x = unit.pos.x + rollx
                pos.y = unit.pos.y + rolly
                pos.z = unit.pos.z
                
                local j = 0
                while not dfhack.maps.ensureTileBlock(pos.x,pos.y,pos.z+j).designation[pos.x%16][pos.y%16].outside do
                        j = j + 1
                end
                pos.z = pos.z + j
                dfhack.maps.spawnFlow(pos,snum,0,inum,strength)
        end
end

local stype = args[1]
local unit = df.unit.find(tonumber(args[2]))
local radius = tonumber(args[3])
local number = tonumber(args[4])
local strength = tonumber(args[5])
local itype = 0
if #args > 5 then
        itype = args[6]
end

storm(stype,unit,radius,number,itype,strength)