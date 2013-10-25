local dialog = require('gui.dialogs')


-- Use max to keep at least some of the original caravan communication idea
civ_stats.population = math.max(civ_stats.population, ui_stats.population)

print('Home civ notified about current population.')


local function setPopCap(arg)
	if type(arg)=="number" then 
		df.global.d_init.population_cap = arg
	else
		setMigrantNumbers("Only numbers go in there, silly!")
	end
	local civ_stats = df.historical_entity.find(ui.civ_id).activity_stats
	if civ_stats then
		dfhack.run_script("fix/population_cap")
	else
		dfhack.run_script("fix/population_cap","force")
	end
end
function setMigrantNumbers(text)
	local promptArg = "200" --if not string, will give error at bottom
	text = text or "What do you want your population cap to be?"
	dialog.showInputPrompt("Pop cap selection","What do you want your population cap to be?",COLOR_WHITE,promptArg,setPopCap)
end

setMigrantNumbers()

--[[error report if promptArg is not set to string at first:
...\hack\lua\gui\widgets.lua:147: attempt to get length of local 'old' (a number value)
stack traceback:
        ...\hack\lua\gui\widgets.lua:147: in function 'onInput'
        ...\hack\lua\gui.lua:472: in function 'inputToSubviews'
        ...\hack\lua\gui\dialogs.lua:131: in function <...rtress\Mods\DFHack script tests\hack\lua\gui\dialogs.lua:119>
]]