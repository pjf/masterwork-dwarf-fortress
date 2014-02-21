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

function weather3(stype,number,itype,strength,duration,test)
        print(weathercontinue)
        if weathercontinue then
                dfhack.timeout(1000,'ticks',weather(stype,number,itype,strength,test))
        else
                return
        end
end

function weather2(cbid)
        return function (stopweather)
                weathercontinue = false
        end
end

function weather(stype,number,itype,strength,duration,test)
        return function (startweather)
                local i
                local rando = dfhack.random.new()
                local snum = flowtypes[stype]
                local inum = 0
                if itype ~= 0 then
                        inum = dfhack.matinfo.find(itype).index
                end

                local mapx, mapy, mapz = dfhack.maps.getTileSize()
                local xmin = 2
                local xmax = mapx - 1
                local ymin = 2
                local ymax = mapy - 1

                local dx = xmax - xmin
                local dy = ymax - ymin
                local pos = {}
                pos.x = 0
                pos.y = 0
                pos.z = 0

                for i = 1, number, 1 do

                        local rollx = rando:random(dx)
                        local rolly = rando:random(dy)

                        pos.x = rollx
                        pos.y = rolly
                        pos.z = 20
                
                        local j = 0
                        while not dfhack.maps.ensureTileBlock(pos.x,pos.y,pos.z+j).designation[pos.x%16][pos.y%16].outside do
                                j = j + 1
                        end
                        pos.z = pos.z + j
                        dfhack.maps.spawnFlow(pos,snum,0,inum,strength)
                end
                weather3(stype,number,itype,strength,test)
        end
end

local stype = args[1]
local number = tonumber(args[2])
local strength = tonumber(args[3])
local duration = tonumber(args[4])
local itype = 0
if #args > 4 then
        itype = args[5]
end
local test = 'abc'
weathercontinue = true

dfhack.timeout(1,'ticks',weather(stype,number,itype,strength,test))
dfhack.timeout(duration,'days',weather2(test))