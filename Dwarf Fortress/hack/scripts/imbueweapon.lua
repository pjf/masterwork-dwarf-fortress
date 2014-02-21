--[[
Description: Changes the material of your current weapon for a set period of time

Use: 
[SYN_CLASS:\COMMAND]
[SYN_CLASS:imbueweapon]
[SYN_CLASS:\UNIT_ID]
[SYN_CLASS:material]
[SYN_CLASS:time]

material = the inorganic that you want to change your weapon into (VALID TOKENS: INORGANIC_SUBTYPE)
time = number of in game ticks that you want the change to last for, if you want the effect to be permanent use 0 (VALID TOKENS: INTEGER[0+])

Example: 
[INTERACTION:SPELL_DIVINE_HOLY_HOLY_WEAPON]
        [I_SOURCE:CREATURE_ACTION]
        [I_TARGET:C:CREATURE]
                [IT_LOCATION:CONTEXT_CREATURE]
                [IT_MANUAL_INPUT:target]
        [I_EFFECT:ADD_SYNDROME]
                [IE_TARGET:C]
                [IE_IMMEDIATE]
                [SYNDROME]
                        [SYN_CLASS:\COMMAND]
                        [SYN_CLASS:imbueweapon]
                        [SYN_CLASS:\UNIT_ID]
                        [SYN_CLASS:HOLY]
                        [SYN_CLASS:3]
                        [CE_SPEED_CHANGE:SPEED_PERC:100:START:0:END:1]

]]

args = {...}

function createcallback(item,stype,sindex)
        return function (resetweapon)
                item.mat_type = stype
                item.mat_index = sindex
        end
end

local unit = df.unit.find(tonumber(args[1]))
local mat = args[2]
local time = tonumber(args[3])
local mat_type = dfhack.matinfo.find(mat).type
local mat_index = dfhack.matinfo.find(mat).index

local inv = unit.inventory
local mode = 0
local weapon = 'nil'
for i = 0, #inv - 1, 1 do
        mode = inv[i].mode
        if mode == 1 then
                weapon = i
        end
end

if weapon == 'nil' then 
        print('No weapon equiped')
        return
end

local item = inv[weapon].item
local stype = item.mat_type
local sindex = item.mat_index
item.mat_type = mat_type
item.mat_index = mat_index

if time ~= 0 then
        dfhack.timeout(time,'ticks',createcallback(item,stype,sindex))
end