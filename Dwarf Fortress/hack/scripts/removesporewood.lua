local my_entity=df.historical_entity.find(df.global.ui.civ_id)
local sText=" "
local k=0
local v=1

for x,y in pairs(df.global.world.entities.all) do
my_entity=y

k=0
	while k < #my_entity.resources.organic.wood.mat_index do
		v=my_entity.resources.organic.wood.mat_type[k]
		sText=dfhack.matinfo.decode(v,my_entity.resources.organic.wood.mat_index[k])
		if (sText==nil) then
			--LIQUID barrels
			my_entity.resources.organic.wood.mat_type:erase(k)
			my_entity.resources.organic.wood.mat_index:erase(k)
			k=k-1
		else
			if(sText.material.id=="WOOD") then
				if(sText.plant.id=="SPORE_TREE") then
					my_entity.resources.organic.wood.mat_type:erase(k)
					my_entity.resources.organic.wood.mat_index:erase(k)
					k=k-1
				end
			end
		end
		k=k+1
	end

k=0
	while k < #my_entity.resources.misc_mat.crafts.mat_index do
		v=my_entity.resources.misc_mat.crafts.mat_type[k]
		sText=dfhack.matinfo.decode(v,my_entity.resources.misc_mat.crafts.mat_index[k])
		if (sText==nil) then
			--LIQUID barrels
			my_entity.resources.misc_mat.crafts.mat_type:erase(k)
			my_entity.resources.misc_mat.crafts.mat_index:erase(k)
			k=k-1
		else
			if(sText.material.id=="WOOD") then
				if(sText.plant.id=="SPORE_TREE") then
					my_entity.resources.misc_mat.crafts.mat_type:erase(k)
					my_entity.resources.misc_mat.crafts.mat_index:erase(k)
					k=k-1
				end
			end
		end
		k=k+1
	end

k=0
	while k < #my_entity.resources.misc_mat.barrels.mat_index do
		v=my_entity.resources.misc_mat.barrels.mat_type[k]
		sText=dfhack.matinfo.decode(v,my_entity.resources.misc_mat.barrels.mat_index[k])
		if (sText==nil) then
			--LIQUID barrels
			my_entity.resources.misc_mat.barrels.mat_type:erase(k)
			my_entity.resources.misc_mat.barrels.mat_index:erase(k)
			k=k-1
		else
			if(sText.material.id=="WOOD") then
				if(sText.plant.id=="SPORE_TREE") then
					my_entity.resources.misc_mat.barrels.mat_type:erase(k)
					my_entity.resources.misc_mat.barrels.mat_index:erase(k)
					k=k-1
				end
			end
		end
		k=k+1
	end

k=0
	while k < #my_entity.resources.misc_mat.wood2.mat_index do
		v=my_entity.resources.misc_mat.wood2.mat_type[k]
		sText=dfhack.matinfo.decode(v,my_entity.resources.misc_mat.wood2.mat_index[k])
		if (sText==nil) then
			--LIQUID wood2
			my_entity.resources.misc_mat.wood2.mat_type:erase(k)
			my_entity.resources.misc_mat.wood2.mat_index:erase(k)
			k=k-1
		else
			if(sText.material.id=="WOOD") then
				if(sText.plant.id=="SPORE_TREE") then
					my_entity.resources.misc_mat.wood2.mat_type:erase(k)
					my_entity.resources.misc_mat.wood2.mat_index:erase(k)
					k=k-1
				end
			end
		end
		k=k+1
	end

k=0
	while k < #my_entity.resources.misc_mat.cages.mat_index do
		v=my_entity.resources.misc_mat.cages.mat_type[k]
		sText=dfhack.matinfo.decode(v,my_entity.resources.misc_mat.cages.mat_index[k])
		if (sText==nil) then
			--LIQUID cages
			my_entity.resources.misc_mat.cages.mat_type:erase(k)
			my_entity.resources.misc_mat.cages.mat_index:erase(k)
			k=k-1
		else
			if(sText.material.id=="WOOD") then
				if(sText.plant.id=="SPORE_TREE") then
					my_entity.resources.misc_mat.cages.mat_type:erase(k)
					my_entity.resources.misc_mat.cages.mat_index:erase(k)
					k=k-1
				end
			end
		end
		k=k+1
	end


end