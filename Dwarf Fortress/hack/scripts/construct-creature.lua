-- Uses a reaction to spawn a living creature that uses the materials of the reagents as body components.  Reactions must begin with LUA_HOOK_CONSTRUCTCREATURE
--[[

You can have as many reagents as you like.
Reagents starting with USE will replace materials used in the creature, in the order that they are defined.
The reagents should be inorganic.  I'm not sure what happens if you try to use an organic material, but I doubt it will work.
Make sure that each USE reagent consists of one item only.  Otherwise, the second item taken will be counted as the second material.
Using tools as intermediary reagents is probably your best option for expensive creatures.
The product field determines the creature produced, where the material is CREATURE_MAT:(your creature):NONE.  The probability should be set at 0 to avoid actually creating a boulder.
If you want to select a specific caste, the product field should have a PRODUCT_DIMENSION for the caste.
However, to make the creature's color show up based on its material, the creature must have 16 castes, one for each color.
If a USE reagent also has the word COLOR in its name, the creature's caste will be selected based on that reagent's color.  Add this into the creature's raw:

	[CASTE:00][CASTE_COLOR:0:0:0]
	[CASTE:10][CASTE_COLOR:1:0:0]
	[CASTE:20][CASTE_COLOR:2:0:0]
	[CASTE:30][CASTE_COLOR:3:0:0]
	[CASTE:40][CASTE_COLOR:4:0:0]
	[CASTE:50][CASTE_COLOR:5:0:0]
	[CASTE:60][CASTE_COLOR:6:0:0]
	[CASTE:70][CASTE_COLOR:7:0:0]
	[CASTE:01][CASTE_COLOR:0:0:1]
	[CASTE:11][CASTE_COLOR:1:0:1]
	[CASTE:21][CASTE_COLOR:2:0:1]
	[CASTE:31][CASTE_COLOR:3:0:1]
	[CASTE:41][CASTE_COLOR:4:0:1]
	[CASTE:51][CASTE_COLOR:5:0:1]
	[CASTE:61][CASTE_COLOR:6:0:1]
	[CASTE:71][CASTE_COLOR:7:0:1]
	
If a USE reagent also has the word NAME in its name, that reagent's name will be added on to the creature's first name.

sample reaction
[REACTION:LUA_HOOK_MAKE_CONSTRUCT_MECHANICAL_CAT]
[NAME:Make a mechanical cat]
[BUILDING:CRAFTSMAN:NONE]
[REAGENT:USE_COLOR_NAME_A:1:BAR:NONE:INORGANIC:NONE]
[REAGENT:USE_B:1:TRAPPARTS:NONE:INORGANIC:NONE]
[REAGENT:C:1:TRAPPARTS:NONE:NONE:NONE]
[PRODUCT:1:0:BOULDER:NONE:CREATURE_MAT:MECHANICAL_CAT:NONE]
[SKILL:MECHANICS]

The creature raw has two materials, a 'shell' material and an 'inside' material.  These must be the first materials defined in the creature raw.
The finished product will have the material of the 'bar' on the outside, and the material of the mechanisms on the inside.  The third reagent will not be used.
Only materials actually used in the creature will be counted.  You're best bet is to simply define the modifiable tissue(s) near the top of the creature raw, before defining any other materials or tissues.
This might not be the way the materials are always loaded, further testing is needed.  But if you make a creature like this one it should work fine.

To have the creature leave an itemcorpse of the material used in its construction, it must include [CREATURE_CLASS:DFHACK_CONSTRUCTCREATURE_ITEMCORPSE].
This will cause the corpse item to have the material of the first tissue defined in the raws.
	
--]]
eventful = require 'plugins.eventful'
local mo = require 'makeown'
local fov = require 'fov'
local utils = require 'utils'


--This is for the itemcorpses

local ev=require 'plugins.eventful'
ev.onUnitDeath.bla=function(u_id)
	local unit = df.unit.find(u_id)
	local creature_classes = df.global.world.raws.creatures.all[unit.race].caste[unit.caste].creature_class
	for i=0,#creature_classes-1,1 do
		if creature_classes[i].value == "DFHACK_CONSTRUCTCREATURE_ITEMCORPSE" then
			local itemcorpse = nil
			for i=0,#unit.corpse_parts-1,1 do
				part=df.item.find(unit.corpse_parts[i])
				if getmetatable(part) ~= "item_corpsepiecest" then
					itemcorpse = part
				end
			end
			if itemcorpse ~= nil then
				mat_type = unit.body.body_plan.materials.mat_type[0]
				mat_index = unit.body.body_plan.materials.mat_index[0]
				itemcorpse.mat_type = mat_type
				itemcorpse.mat_index = mat_index
			end
			break
		end
	end
end
ev.enableEvent(eventful.eventType.UNIT_DEATH,5)

function makeConstruct(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	local skill = reaction.products[0].probability + dfhack.units.getEffectiveSkill(unit,reaction.skill)
	--reaction.reagents[0].code --name of the reagent
	--job[0].mat_index --material index of actual reagent used
	--printall(input_reagents[0])

	local race = reaction.products[0].mat_index --material index of output.  If creature_mat:name:NONE, will be index of creature species
	local caste = reaction.products[0].product_dimension
	if caste == nil or caste < 0 then
		caste = 0
	end
	if race == -1 then
		print("Error: Creature not found.  Use [PRODUCT:1:0:BOULDER:NONE:CREATURE_MAT:(creature ID):NONE] for the first reaction product.")
	end
	--Handle colors
	for i=0,#reaction.reagents-1,1 do
		--job[i].flags.PRESERVE_REAGENT = true
		if string.starts(reaction.reagents[i].code,'USE') and string.find(reaction.reagents[i].code,'COLOR') then
			local r_mat_index=job[i].mat_index
			local r_mat=df.inorganic_raw.find(r_mat_index)
			local r_mat_color_0=r_mat.material.basic_color[0]
			local r_mat_color_1=r_mat.material.basic_color[1]
			local c = r_mat_color_0 .. r_mat_color_1
			if c == '00' then caste = 0
			elseif c == '10' then caste = 1
			elseif c == '20' then caste = 2
			elseif c == '30' then caste = 3
			elseif c == '40' then caste = 4
			elseif c == '50' then caste = 5
			elseif c == '60' then caste = 6
			elseif c == '70' then caste = 7
			elseif c == '01' then caste = 8
			elseif c == '11' then caste = 9
			elseif c == '21' then caste = 10
			elseif c == '31' then caste = 11
			elseif c == '41' then caste = 12
			elseif c == '51' then caste = 13
			elseif c == '61' then caste = 14
			elseif c == '71' then caste = 15
			end
			break
		end
	end
	
	race_name = df.creature_raw.find(race).name[0]
	caste_name = df.creature_raw.find(race).caste[caste].caste_name[0]
	
	local argPos
	argPos={}
	argPos.x=unit.pos.x
	argPos.y=unit.pos.y
	argPos.z=unit.pos.z
	
	--Create a new unit
	local u=PlaceUnitById(race,caste,caste_name,argPos)
	print("Constructed "..caste_name)
	if u then
		j=0
		u.name.first_name=caste_name .. ' ' .. u.id
		for i=0,#reaction.reagents-1,1 do
			if string.starts(reaction.reagents[i].code,'USE') then
				local r_mat_index=job[j].mat_index
				u.body.body_plan.materials.mat_index[j] = r_mat_index
				local r_mat=df.inorganic_raw.find(r_mat_index)
				
				if string.find(reaction.reagents[i].code,'NAME') then
					local r_mat_name=r_mat.material.state_name.Solid
					local r_mat_adj=r_mat.material.state_adj.Solid
					u.name.first_name=r_mat_adj .. ' ' .. caste_name .. ' ' .. u.id
				end

				j=j+1
			end
		end
	end
end







function update()

	dfhack.timeout(1,"ticks",function() update() end)
end

--------------------------------------------------
--------------------------------------------------
--http://lua-users.org/wiki/StringRecipes
function string.starts(String,Start)
   return string.sub(String,1,string.len(Start))==Start
end
--------------------------------------------------
--------------------------------------------------

dfhack.onStateChange.loadConstructCreature = function(code)
	local registered_reactions
	if code==SC_MAP_LOADED then
		--registered_reactions = {}
		for i,reaction in ipairs(df.global.world.raws.reactions) do
			-- register each applicable reaction (to avoid doing string check
			-- for every lua hook reaction (not just ours), this way uses identity check
			if string.starts(reaction.code,'LUA_HOOK_CONSTRUCTCREATURE') then
			-- register reaction.code
				eventful.registerReaction(reaction.code,makeConstruct)
				-- save reaction.code
				--table.insert(registered_reactions,reaction.code)
				registered_reactions = true
			end
		end
		update()
		--if #registered_reactions > 0 then print('Construct Creature: Loaded') end
		if registered_reactions then print('Construct Creature: Loaded') end
	elseif code==SC_MAP_UNLOADED then
	end
end


-- if dfhack.init has already been run, force it to think SC_WORLD_LOADED to that reactions get refreshed
if dfhack.isMapLoaded() then dfhack.onStateChange.loadConstructCreature(SC_MAP_LOADED) end




--Mostly copied from warmist's spawn script

function getCaste(race_id,caste_id)
    local cr=df.creature_raw.find(race_id)
    return cr.caste[caste_id]
end
function genBodyModifier(body_app_mod)
    local a=math.random(0,#body_app_mod.ranges-2)
    return math.random(body_app_mod.ranges[a],body_app_mod.ranges[a+1])
end
function getBodySize(caste,time)
    --todo real body size...
    return caste.body_size_1[#caste.body_size_1-1] --returns last body size
end
function genAttribute(array)
    local a=math.random(0,#array-2)
    return math.random(array[a],array[a+1])
end
function norm()
    return math.sqrt((-2)*math.log(math.random()))*math.cos(2*math.pi*math.random())
end
function normalDistributed(mean,sigma)
    return mean+sigma*norm()
end
function clampedNormal(min,median,max)
    local val=normalDistributed(median,math.sqrt(max-min))
    if val<min then return min end
    if val>max then return max end
    return val
end
function makeSoul(unit,caste)
    local tmp_soul=df.unit_soul:new()
    tmp_soul.unit_id=unit.id
    tmp_soul.name:assign(unit.name)
    tmp_soul.race=unit.race
    tmp_soul.sex=unit.sex
    tmp_soul.caste=unit.caste
    --todo skills,preferences,traits.
    local attrs=caste.attributes
    for k,v in pairs(attrs.ment_att_range) do
       local max_percent=attrs.ment_att_cap_perc[k]/100
       local cvalue=genAttribute(v)
       tmp_soul.mental_attrs[k]={value=cvalue,max_value=cvalue*max_percent}
    end
    for k,v in pairs(tmp_soul.traits) do
        local min,mean,max
        min=caste.personality.a[k]
        mean=caste.personality.b[k]
        max=caste.personality.c[k]
        tmp_soul.traits[k]=clampedNormal(min,mean,max)
    end
    unit.status.souls:insert("#",tmp_soul)
    unit.status.current_soul=tmp_soul
end
function CreateUnit(race_id,caste_id)
    local race=df.creature_raw.find(race_id)
    if race==nil then error("Invalid race_id") end
    local caste=getCaste(race_id,caste_id)
    local unit=df.unit:new()
    unit.race=race_id
    unit.caste=caste_id
    unit.id=df.global.unit_next_id
    df.global.unit_next_id=df.global.unit_next_id+1
    if caste.misc.maxage_max==-1 then
        unit.relations.old_year=-1
    else
        unit.relations.old_year=df.global.cur_year-15+math.random(caste.misc.maxage_min,caste.misc.maxage_max)
    end
    unit.sex=caste.gender
    local body=unit.body
    
    body.body_plan=caste.body_info
    local body_part_count=#body.body_plan.body_parts
    local layer_count=#body.body_plan.layer_part
    --components
    unit.relations.birth_year=df.global.cur_year-15
    --unit.relations.birth_time=??
    
    --unit.relations.old_time=?? --TODO add normal age
    local cp=body.components
    cp.body_part_status:resize(body_part_count)
    cp.numbered_masks:resize(#body.body_plan.numbered_masks)
    for num,v in ipairs(body.body_plan.numbered_masks) do
        cp.numbered_masks[num]=v
    end
    
    cp.layer_status:resize(layer_count)
    cp.layer_wound_area:resize(layer_count)
    cp.layer_cut_fraction:resize(layer_count)
    cp.layer_dent_fraction:resize(layer_count)
    cp.layer_effect_fraction:resize(layer_count)
    local attrs=caste.attributes
    for k,v in pairs(attrs.phys_att_range) do
        local max_percent=attrs.phys_att_cap_perc[k]/100
        local cvalue=genAttribute(v)
        unit.body.physical_attrs[k]={value=cvalue,max_value=cvalue*max_percent}
        --unit.body.physical_attrs:insert(k,{new=true,max_value=genMaxAttribute(v),value=genAttribute(v)})
    end
 
    body.blood_max=getBodySize(caste,0) --TODO normal values
    body.blood_count=body.blood_max
    body.infection_level=0
    unit.status2.body_part_temperature:resize(body_part_count)
    for k,v in pairs(unit.status2.body_part_temperature) do
        unit.status2.body_part_temperature[k]={new=true,whole=10067,fraction=0}
        
    end
    --------------------
    local stuff=unit.enemy
    stuff.body_part_878:resize(body_part_count) -- all = 3
    stuff.body_part_888:resize(body_part_count) -- all = 3
    stuff.body_part_relsize:resize(body_part_count) -- all =0
 
    --TODO add correct sizes. (calculate from age)
    local size=caste.body_size_2[#caste.body_size_2-1]
    body.size_info.size_cur=size
    body.size_info.size_base=size
    body.size_info.area_cur=math.pow(size,0.666)
    body.size_info.area_base=math.pow(size,0.666)
    body.size_info.area_cur=math.pow(size*10000,0.333)
    body.size_info.area_base=math.pow(size*10000,0.333)
    
    stuff.were_race=race_id
    stuff.were_caste=caste_id
    stuff.normal_race=race_id
    stuff.normal_caste=caste_id
    stuff.body_part_8a8:resize(body_part_count) -- all = 1
    stuff.body_part_base_ins:resize(body_part_count) 
    stuff.body_part_clothing_ins:resize(body_part_count) 
    stuff.body_part_8d8:resize(body_part_count) 
    unit.recuperation.healing_rate:resize(layer_count) 
    --appearance
   
    local app=unit.appearance
    app.body_modifiers:resize(#caste.body_appearance_modifiers) --3
    for k,v in pairs(app.body_modifiers) do
        app.body_modifiers[k]=genBodyModifier(caste.body_appearance_modifiers[k])
    end
    app.bp_modifiers:resize(#caste.bp_appearance.modifier_idx) --0
    for k,v in pairs(app.bp_modifiers) do
        app.bp_modifiers[k]=genBodyModifier(caste.bp_appearance.modifiers[caste.bp_appearance.modifier_idx[k]])
    end
    --app.unk_4c8:resize(33)--33
    app.tissue_style:resize(#caste.bp_appearance.style_part_idx)
    app.tissue_style_civ_id:resize(#caste.bp_appearance.style_part_idx)
    app.tissue_style_id:resize(#caste.bp_appearance.style_part_idx)
    app.tissue_style_type:resize(#caste.bp_appearance.style_part_idx)
    app.tissue_length:resize(#caste.bp_appearance.style_part_idx)
    app.genes.appearance:resize(#caste.body_appearance_modifiers+#caste.bp_appearance.modifiers) --3
    app.genes.colors:resize(#caste.color_modifiers*2) --???
    app.colors:resize(#caste.color_modifiers)--3
    
    makeSoul(unit,caste)
    
    df.global.world.units.all:insert("#",unit)
    df.global.world.units.active:insert("#",unit)
    --todo set weapon bodypart
    
    local num_inter=#caste.body_info.interactions
    unit.curse.anon_5:resize(num_inter)
    return unit
end
function findRace(name)
    for k,v in pairs(df.global.world.raws.creatures.all) do
        if v.creature_id==name then
            return k
        end
    end
    qerror("Race:"..name.." not found!")
end

function PlaceUnitById(race,caste,name,position)
    local pos=position or copyall(df.global.cursor)
    if pos.x==-30000 then
        qerror("Point your pointy thing somewhere")
    end
	
	local u=CreateUnit(race,tonumber(caste) or 0)
    u.pos:assign(pos)
    if name then
        u.name.first_name=name
        u.name.has_name=true
    end
    u.civ_id=df.global.ui.civ_id
    
    local desig,ocupan=dfhack.maps.getTileFlags(pos)
    if ocupan.unit then
        ocupan.unit_grounded=true
        u.flags1.on_ground=true
    else
        ocupan.unit=true
        --createNemesis(u)
    end
	return u
end

function createFigure(trgunit)
    local hf=df.historical_figure:new()
    hf.id=df.global.hist_figure_next_id
    hf.race=trgunit.race
    hf.caste=trgunit.caste
    df.global.hist_figure_next_id=df.global.hist_figure_next_id+1
    hf.name.first_name=trgunit.name.first_name
    hf.name.has_name=true
 
 
    df.global.world.history.figures:insert("#",hf)
    return hf
end
function createNemesis(trgunit)
    local id=df.global.nemesis_next_id
    local nem=df.nemesis_record:new()
    nem.id=id
    nem.unit_id=trgunit.id
    nem.unit=trgunit
    nem.flags:resize(1)
    nem.flags[4]=true
    nem.flags[5]=true
    nem.flags[6]=true
    nem.flags[7]=true
    nem.flags[8]=true
    nem.flags[9]=true
    --[[for k=4,8 do
        nem.flags[k]=true
    end]]
    df.global.world.nemesis.all:insert("#",nem)
    df.global.nemesis_next_id=id+1
    trgunit.general_refs:insert("#",{new=df.general_ref_is_nemesisst,nemesis_id=id})
    trgunit.flags1.important_historical_figure=true
    local gen=df.global.world.worldgen
    nem.save_file_id=gen.next_unit_chunk_id;
    gen.next_unit_chunk_id=gen.next_unit_chunk_id+1
    gen.next_unit_chunk_offset=gen.next_unit_chunk_offset+1
    
    --[[ local gen=df.global.world.worldgen
    gen.next_unit_chunk_id
    gen.next_unit_chunk_offset
    ]]
    nem.figure=createFigure(trgunit)
end




