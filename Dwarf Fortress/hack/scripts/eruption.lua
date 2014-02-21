fov = require 'fov'

args={...}

function eruption(etype,unit,radius,height)
        local view = fov.get_fov(radius, unit.pos)
        local i
        local rando = dfhack.random.new()

        local dx = view.xmax - view.xmin
        local dy = view.ymax - view.ymin
        local hx = height/dx

        for i = view.xmin, view.xmax, 1 do
                for j = view.ymin, view.ymax, 1 do
                        if (math.abs(i-unit.pos.x) + math.abs(j-unit.pos.y)) <= radius then
                                block = dfhack.maps.getTileBlock(i,j,view.z)
                                dsgn = block.designation[i%16][j%16]
                                size = math.floor(height-hx*(math.abs(unit.pos.x-i)+math.abs(unit.pos.y-j)))
                                if size < 1 then size = 1 end
                                dsgn.flow_size = size
                                if etype == 'magma' then
                                        dsgn.liquid_type = true
                                flow = block.liquid_flow[i%16][j%16]
                                flow.temp_flow_timer = 10
                                flow.unk_1 = 10
                                block.flags.update_liquid = true
                                block.flags.update_liquid_twice = true
                                end
                        end
                end
        end
end

local etype = args[1]
local unit = df.unit.find(tonumber(args[2]))
local radius = tonumber(args[3])
local height = tonumber(args[4])

eruption(etype,unit,radius,height)