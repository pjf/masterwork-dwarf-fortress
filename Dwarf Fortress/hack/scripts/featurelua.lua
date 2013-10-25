-- lua version of feature plugin.
helpstring=[[ featurelua [ list | show ID [true|false] | IDbyName [name] | IDbyType [type] | magmaWorkshops | ? ]
 
	list   list map features

 show ID true|false   hide or show a map feature 
			ex. featurelua 3 show true
 
 IDbyType [type]  returns the ID for the first instance of a given feature type. 
			
			ex. featurelua IDbyType subterranean_from_layer
 
		Types are: pit,
        	magma_pool,
        	volcano,
        	deep_special_tube,
        	deep_surface_portal, 
        	subterranean_from_layer,
        	magma_core_from_layer,
        	feature_underworld_from_layer
 
IDbyName [name]  returns the ID for the first instance of a given feature type. 
			ex featurelua IDbyName "magma pool"
 
magmaWorkshops enables magma workshops.

? prints this helpful wall of text. 
 ]]
args={...}
if args[1]=="?" then
    print(helpstring)
    return
end


local utils = require("utils") --for call_with_string

function getFeatureByName(name)
    for index,value in ipairs(df.global.world.cur_savegame.map_features) do
		if utils.call_with_string(value,"getName")==name then
			return index
		end
	end
end

--[[ Supported Arguments:

 	outdoor_river,
 	cave,
        pit,
        magma_pool,
        volcano,
        deep_special_tube,
        deep_surface_portal,
        subterranean_from_layer,
        magma_core_from_layer,
        feature_underworld_from_layer
]]--
function getFeatureByType(type)
    for index,value in ipairs(df.global.world.cur_savegame.map_features) do
		local featureType=value:getType()
		if df.feature_type[featureType]==type then
			return index
		end
	end
end

-- call with string wraps function with temporary string to put value in
-- in a similar way:
function featureList()
	local ret={}
	for index,value in ipairs(df.global.world.cur_savegame.map_features) do
		local featureType=value:getType()
		table.insert(ret,index,{utils.call_with_string(value,"getName"), df.feature_type[featureType]})
	end  
	return ret
end

function printFeatures() --to console
	
	for index,value in ipairs	
		(df.global.world.cur_savegame.map_features) do
			local featureType=value:getType()
			
			local discovered
			
			if value.flags[df.feature_init_flags.Discovered]  then
				discovered="Discovered"
			else
				discovered="Undiscovered"
			end
			
			--discovered=utils.call_with_string(value.flags,"is_set",df.feature_init_flags[df.feature_init_flags.Discovered]) 
			
			print(index,'\t',df.feature_type[featureType],'\t',utils.call_with_string(value,"getName"),'\t',discovered)	
	end  
	return ret
end

-- sets a feature as shown/discovered or hidden
function setDiscoveredByID(ID, discovered)
	for index,value in ipairs	
		(df.global.world.cur_savegame.map_features) do
		local featureType=value:getType()
		if index == ID then
			value.flags[df.feature_init_flags.Discovered]=discovered
			
			local state
			
			if discovered then 
				state ="shown." 
			else 
				state="hidden." 
			end
			
			print(utils.call_with_string(value,"getName"), " is now ", state)
		end
			
	end  
end

if args[1]=="list" then
    printall(printFeatures())
    return
end

if args[1]=="IDbyName" then
    print(getFeatureByName(args[2]))
    return
end

if args[1]=="IDbyType" then
    print(getFeatureByType(args[2]))
    return
end

if args[1]=="show" then
	if args[3]== "true" then
		setDiscoveredByID(tonumber(args[2]), true)
	else
		setDiscoveredByID(tonumber(args[2]), false)
	end	
	return
end

if args[1]=="magmaWorkshops" then
	setDiscoveredByID(getFeatureByType("magma_core_from_layer"), true)
	return
end

print(helpstring)
    return

		