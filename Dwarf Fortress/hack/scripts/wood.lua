
local my_entity=df.historical_entity.find(df.global.ui.civ_id)

for k,v in pairs(my_entity.resources.organic.wood.mat_type) do
	sText=dfhack.matinfo.decode(v,my_entity.resources.organic.wood.mat_index[k])
	print(sText)

	if (sText==nil) then
		--print("nil")
	else
		--print("OK")
		print(sText.plant.id)
	end
end