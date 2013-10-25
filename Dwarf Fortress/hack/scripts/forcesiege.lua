-- Forces an attack from the civ type or number (don't use if you don't know!) specified. If no civ is specified, it will use the first valid babysnatcher civ found. Note that the siege won't happen unless progress triggers are already met.

local function findBabysnatcherCiv()
	for eid,entity in ipairs(df.global.world.entities.all) do
		if entity.entity_raw.flags.BABYSNATCHER == true then return entity end
	end
end

local function progressTriggersMet(entity)
	local siegeEntity = entity.entity_raw.progress_trigger --evil, but readable
	local fortress = df.global.ui
	if fortress.progress_population < siegeEntity.pop_siege and fortress.progress_trade < siegeEntity.prod_siege and fortress.progress_production < siegeEntity.prod_siege then return false end
	return true
end

local function findCivWithGivenToken(arg)
	local entities = df.global.world.entities.all
	if tonumber(arg) then
		local argnumber = tonumber(arg)
		if argnumber > #entities then qerror("There aren't that many civs in the world!") end
		return entities[argnumber]
	end
	if arg and not tonumber(arg) then 
		for eid,entity in ipairs(entities) do
			if entity.entity_raw.code == arg then return entity end
		end
	end
	qerror("Civ not found. Aborting!")
end

argument = ...

local function force_siege()
	local siegeEntity
	if argument
		then siegeEntity = findCivWithGivenToken(argument)
		else siegeEntity = findBabysnatcherCiv() 
	end
	if not progressTriggersMet(siegeEntity) then 
		dfhack.gui.showAnnouncement("The civilization you want to challenge isn't even interested in you.",COLOR_YELLOW) 
		qerror("Progress triggers not met.")
	end
	df.global.timed_events:insert('#', { new = df.timed_event, type = df.timed_event_type.CivAttack, season = df.global.cur_season, season_ticks = df.global.cur_season_tick, entity = siegeEntity } )
end

force_siege()