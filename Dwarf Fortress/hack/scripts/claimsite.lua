-- #advfort

--this is Warmist's "claim site" script, which is (in Putnam's opinion) very important for Masterwork adventure mode.
--https://gist.github.com/warmist/5081626

local eventful=require("plugins.eventful")
function add_site(size,civ,site_type,name)
	local x=(df.global.world.map.region_x+1)%16;
	local y=(df.global.world.map.region_y+1)%16;
	local minx,miny,maxx,maxy
	if(x<size) then
		minx=0
		maxx=2*size
	elseif(x+size>16) then
		maxx=16
		minx=16-2*size
	else
		minx=x-size
		maxx=x+size
	end
		
	if(y<size) then
		miny=0
		maxy=2*size
	elseif(y+size>16) then
		maxy=16
		miny=16-2*size
	else
		miny=y-size
		maxy=y+size
	end
	
	require("plugins.dfusion.adv_tools").addSite(nil,nil,maxx,minx,maxy,miny,civ,name,site_type)
end
function reaction(reaction,unit,input_items,input_reagents,output_items,call_native)
	require("gui.dialogs").showInputPrompt("Site name", "Select a name for a new site:", nil,nil, dfhack.curry(add_site,1,unit.civ_id,0))
	call_native.value=false
end
eventful.registerReaction("LUA_HOOK_MAKE_SITE3x3_MW",reaction)