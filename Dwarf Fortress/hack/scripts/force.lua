-- Forces an event.

local function findCiv(arg)
	local entities = df.global.world.entities.all
	if tonumber(arg) then return arg end
	if arg and not tonumber(arg) then 
		for eid,entity in ipairs(entities) do
			if entity.entity_raw.code == arg then return entity end
		end
	end
end

local args = {...}

if not args then qerror("Needs an argument. Valid arguments are caravan, migrants, diplomat, megabeast, curiousbeast, mischievousbeast, flier, siege and nightcreature. Second argument is civ, either raw entity ID or \"player\" for player's civ.") end

local EventType = args[1]

if args[2] == "player" then forceEntity = df.global.world.entities.all[df.global.ui.civ_id] end --for some reason it makes the forceEntity the player race by default anyway. Actually pretty convenient.

if args[2] ~= "player" then forceEntity = findCiv(args[2]) end

if (EventType == "caravan" or EventType == "diplomat" or EventType == "siege" or EventType == "migrants") and not forceEntity then
		qerror('Caravan, diplomat and siege require a civilization ID to be included.')
end

local function eventTypeIsNotValid()
	local eventTypes = 
	{
	"caravan",
	"migrants",
	"diplomat",
	"megabeast",
	"curiousbeast",
	"mischevousbeast",
	"mischeviousbeast",
	"flier",
	"siege",
	"nightcreature"
	}
	for _,v in ipairs(eventTypes) do
		if args[1] == v then return false end
	end
	return true
end	
	

--code may be kind of bad below :V Putnam ain't experienced in lua...
if eventTypeIsNotValid() then
	qerror('Invalid argument. Valid arguments are caravan, migrants, diplomat, megabeast, curiousbeast, mischievousbeast, flier, siege and nightcreature.')
end


local function force_megabeast()
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.Megabeast, season = df.global.cur_season, season_ticks = df.global.cur_season_tick } )
end

local function force_migrants()
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.Migrants, season = df.global.cur_season, season_ticks = df.global.cur_season_tick, entity = df.global.world.entities.all[df.global.ui.civ_id] } )
end

local function force_caravan()
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.Caravan, season = df.global.cur_season, season_ticks = df.global.cur_season_tick, entity = forceEntity } )
end

local function force_diplomat()
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.Diplomat, season = df.global.cur_season, season_ticks = df.global.cur_season_tick, entity = forceEntity } )
end

local function force_curious()
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.WildlifeCurious, season = df.global.cur_season, season_ticks = df.global.cur_season_tick } )
end

local function force_mischievous()
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.WildlifeMichievous, season = df.global.cur_season, season_ticks = df.global.cur_season_tick } )
end

local function force_flier()
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.WildlifeFlier, season = df.global.cur_season, season_ticks = df.global.cur_season_tick } )
end

local function force_siege()
df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.CivAttack, season = df.global.cur_season, season_ticks = df.global.cur_season_tick, entity = forceEntity } )
end

local function force_nightcreature()
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.NightCreature, season = df.global.cur_season, season_ticks = df.global.cur_season_tick } )
end

--this code may be bad too :V

if EventType=="caravan" then force_caravan()
	elseif EventType=="migrants" then force_migrants()
	elseif EventType=="siege" then force_siege()
	elseif EventType=="megabeast" then force_megabeast()
	elseif EventType=="diplomat" then force_diplomat()
	elseif EventType=="curiousbeast" then force_curious()
	elseif EventType=="mischievousbeast" or EventType=="mischeviousbeast" then force_mischievous()
	elseif EventType=="flier" then force_flier()
	elseif EventType=="nightcreature" then force_nightcreature()
end
	