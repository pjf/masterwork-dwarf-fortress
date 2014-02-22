args = {...}

local unit = df.unit.find(tonumber(args[1]))
local mat = args[3]
local mat_type = dfhack.matinfo.find(mat).type
local mat_index = dfhack.matinfo.find(mat).index

local item_index = df.item_type['TOOL']
local item_subtype = 'nil'

for i=0,dfhack.items.getSubtypeCount(item_index)-1 do
  local item_sub = dfhack.items.getSubtypeDef(item_index,i)
  if item_sub.id == args[2] then
	  item_subtype = item_sub.subtype
	end
end

if item_subtype == 'nil' then
  print("No item of that type found")
  return
end

local item=df['item_toolst']:new() --incredible
item.id=df.global.item_next_id
df.global.world.items.all:insert('#',item)
df.global.item_next_id=df.global.item_next_id+1
item:setSubtype(item_subtype)
item:setMaterial(mat_type)
item:setMaterialIndex(mat_index)
item:categorize(true)
item.flags.removed=true
item:setSharpness(1,0)
item:setQuality(0)
dfhack.items.moveToGround(item,{x=unit.pos.x,y=unit.pos.y,z=unit.pos.z})
