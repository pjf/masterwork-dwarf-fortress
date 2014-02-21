--[[
Description: Causes an "eruption" of water or magma under a creature with a specified radius and a specified height.

Use: 
[SYN_CLASS:\COMMAND]
[SYN_CLASS:eruption]
[SYN_CLASS:type]
[SYN_CLASS:\UNIT_ID]
[SYN_CLASS:radius]
[SYN_CLASS:depth]

type = type of liquid created (VALID TOKENS: magma, water)
radius = x,y,z extent of the spawned liquid (VALID TOKEN: INTEGER[0 - mapsize])
depth =  amount of liquid created at center, falls off as you move away from the center to 1 at the max radius (VALID TOKENS: INTEGER[1 - 7])

Example: 
Code: [Select]
[INTERACTION:SPELL_ELEMENTAL_FIRE_VOLCANO]
        [I_SOURCE:CREATURE_ACTION]
        [I_TARGET:C:CREATURE]
                [IT_LOCATION:CONTEXT_CREATURE]
                [IT_MANUAL_INPUT:target]
        [I_EFFECT:ADD_SYNDROME]
                [IE_TARGET:C]
                [IE_IMMEDIATE]
                [SYNDROME]
                        [SYN_CLASS:\COMMAND]
                        [SYN_CLASS:eruption]
                        [SYN_CLASS:magma]
                        [SYN_CLASS:\UNIT_ID]
                        [SYN_CLASS:4,0,0]
                        [SYN_CLASS:7]
                        [CE_SPEED_CHANGE:SPEED_PERC:100:START:0:END:1]

]]

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

function eruption(etype,unit,radius,depth)
        local i
        local rando = dfhack.random.new()
        local radiusa = split(radius,',')
        local rx = tonumber(radiusa[1])
        local ry = tonumber(radiusa[2])
        local rz = tonumber(radiusa[3])

        local mapx, mapy, mapz = dfhack.maps.getTileSize()
        local xmin = unit.pos.x - rx
        local xmax = unit.pos.x + rx
        local ymin = unit.pos.y - ry
        local ymax = unit.pos.y + ry
        local zmax = unit.pos.z + rz
        if xmin < 1 then xmin = 1 end
        if ymin < 1 then ymin = 1 end
        if xmax > mapx then xmax = mapx-1 end
        if ymax > mapy then ymax = mapy-1 end
        if zmax > mapz then zmax = mapz-1 end

        local dx = xmax - xmin
        local dy = ymax - ymin
        local hx = 0
        local hy = 0

        if dx == 0 then
                hx = depth
        else
                hx = depth/dx
        end

        if dy== 0 then
                hy = depth
        else
                hy = depth/dy
        end

        for i = xmin, xmax, 1 do
                for j = ymin, ymax, 1 do
                        for k = unit.pos.z, zmax, 1 do
                                if (math.abs(i-unit.pos.x) + math.abs(j-unit.pos.y)) <= math.sqrt(rx*rx+ry*ry) then
                                        block = dfhack.maps.ensureTileBlock(i,j,k)
                                        dsgn = block.designation[i%16][j%16]
                                        if not dsgn.hidden then
                                                size = math.floor(depth-hx*math.abs(unit.pos.x-i)-hy*math.abs(unit.pos.y-j))
                                                if size < 1 then size = 1 end
                                                dsgn.flow_size = size
                                                if etype == 'magma' then
                                                        dsgn.liquid_type = true
                                                end
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
end

local etype = args[1]
local unit = df.unit.find(tonumber(args[2]))
local radius = args[3]
local depth = tonumber(args[4])

eruption(etype,unit,radius,depth)