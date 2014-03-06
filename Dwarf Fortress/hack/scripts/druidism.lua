-- Enables nature merit system and reactions that utilize nature merit.
--[[

---Taming wildlife (at Nature Shrine)

Probability of successfully taming a wild animal (in percent): (1/petValue) * totalMerit * trainerSkill * 0.01

Merit calculation is performed on all animals in fort's history, then added up:

wildOriginMultiplier = *5 if caught in the wild
childMultiplier = *2 if animal is a child
adultMultiplier = *2 if animal is an adult
casualtyRating = 
	*5 for most pet deaths
	*20 for pets dying of starvation or thirst
	*50 for slaughtering at a butcher's shop
	*-10 if died of old age (keeping an animal alive until it dies of old age is a GOOD thing)
	*1 for wild animals killed by pets
	*2 for wild animals killed by soldiers/hunters
	*5 for setting loose an animal and letting pets kill it
	*10 for setting loose an animal and letting soldiers/hunters kill it
	(Only killing peaceful, natural wildlife counts against you.  Killing monsters or hostile war animals is fine.) 
	(Note that killing animal item thieves DOES count, find another way to protect your hoard.)

Calculations (each of these factors are evaluated seperately):
	Keeping living pets: +(wildOriginMultiplier*petValue*childMultiplier)
	Animals released: +(wildOriginMultiplier*adultMultiplier*petValue)
	Animal death: -(petValue*casualtyRating*wildOriginMultiplier*childMultiplier)
	Animals on chains: -(petValue*wildOriginMultiplier*childMultiplier*5)
	Animals in cages: -(petValue*wildOriginMultiplier*childMultiplier*10)
	Bonus for training wild animals: +(petValue*trainingLevel*7) ("Trained" is level 1) (Bonus is applied in all cases, even for dead/released animals)

	Use the 'Commune with Nature' command at the Nature Shrine to check your rating and print all judged items to the DFHack console.
	Alternately, type 'druidism calc' into the console to do the same thing.
	
---Training	(at Advanced Animal Training Area)

Probability of training an animal for an activity (in percent):
	(((trainerSkill^3)*10)/(petValue*historicalFigureMultiplier)) * trainingLevel * bondBonus * interest in activity
	
	historicalFigureMultiplier = *10 if animal is historical figure
	bondBonus = *100 if animal is the pet of the trainer

	Interest in activity calculations:
	
	For following the trainer: 1
	For hunting: 1 * (10 if LARGE_PREDATOR) * (5 if CARNIVORE) * (2 if MISCHIEVOUS) * (0.5 if GRAZER)
	For war: 1 * (10 if LIKES_FIGHTING) * (5 if LARGE_PREDATOR) * (0.5 if BENIGN)
	For following another animal: clusterMultiplier * sameRaceMultiplier * predatorFactor * respectValue
		clusterMultiplier = average of leader's average cluster number and follower's average cluster number
		sameRaceMultiplier = *5 if leader is the same species as the follower
		predatorFactor = if leader is BENIGN GRAZER and follower is LARGE_PREDATOR CARNIVORE, *2.  If the opposite, /2.
		respectValue = petValue of leader/petValue of follower (both are multiplied by 10 if animal is historical figure)
		(For training animals to follow other animals, the one pastured closest to the trainer will be considered the leader)
	For untraining: 10/interest in training
	
	
	
	
	--New system:
	
	effectiveskill(for taming) = skill

	respectvalue = ((skill*5)^2)
	interestvalue = merit/10
	floor((1-((petvalue/2)/respectvalue))*100) = prob of success in a respect conflict
	same thing with interestvalue to determine likelihood of taming
	both must succeed to tame successfully

	<25 - 1
	<100 - 2
	<225 - 3
	<400 - 4
	<625 - 5
	<900 - 6
	<1225 - 7
	<2500 - 10
	<4900 - 14
	<5625 - 15
	<10000 - 20

	--impactvalue = ciel(sqrt(petvalue))/5 (1-20)

	impactvalue = petvalue
	trainingbonus = impactvalue*level
	casualtymultiplier = 1-50
	wildmultiplier = x5
	childbonus = x2
	adultbonus = x2 (only if not wild)
	releasevalue = adultbonus*wildmultiplier
	
	
--]]

local eventful = require 'plugins.eventful'
local mo = require 'makeown'
local fov = require 'fov'
local utils = require 'utils'


if totalMerit == nil then
	totalMerit = 0
end
if newMerit == nil then
	newMerit = 0
end
if nextUnit == nil then
	nextUnit = -1
end

args={...}
if args[1] == "see" then
	print("Merit from previous count: "..totalMerit)
	print("Current count: "..newMerit)
	print("On unit: "..nextUnit)
elseif args[1] == "calc" then
	calculateMeritAll("console")
else
	totalMerit = nil
	newMerit = 0
	nextUnit = -1
end

function isSeen(view,p)
	return p.z == view.z and view[p.y][p.x] > 0
end

function closeBy(unit,u,radius)
	if unit.pos.x+radius >= u.pos.x
		and unit.pos.x-radius <= u.pos.x
		and unit.pos.y+radius >= u.pos.y
		and unit.pos.y-radius <= u.pos.y
		and unit.pos.z+5 >= u.pos.z
		and unit.pos.z-5 <= u.pos.z
		then return true
	else
		return false
	end
end

function levelUp(unit, skillId, amount)
	max_skill = 20 
	
	local skill = df.unit_skill:new()
	local foundSkill = false
 
	for k, soulSkill in ipairs(unit.status.current_soul.skills) do
		if soulSkill.id == skillId then
			skill = soulSkill
			foundSkill = true
			break
		end
	end
 
	if foundSkill then
		-- Let's not train beyond the max skill
		if skill.rating >= max_skill then
			return false
		end
 
		skill.experience = skill.experience + amount
		if skill.experience > 100 * skill.rating + 500 then
			skill.experience = skill.experience - (100 * skill.rating + 500)
			skill.rating = skill.rating + 1
		end
	else
		skill.id = skillId
		skill.experience = amount
		skill.rating = 0
		unit.status.current_soul.skills:insert('#',skill)
	end
end

local my_entity=df.historical_entity.find(df.global.ui.civ_id)
--args={...}

function checkMerit(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	merit = calculateMeritAll("console")
	local threshold = {-100000, -10000, -1000, 1000, 10000, 100000}
	if merit < threshold[1] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels absolutely abhored by nature.  "..merit, COLOR_RED, true)
	elseif merit >= threshold[1] and merit < threshold[2] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels despised by nature.  ("..merit..")", COLOR_RED)
	elseif merit >= threshold[2] and merit < threshold[3] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels unwelcome by nature.  ("..merit..")", COLOR_RED)
	elseif merit >= threshold[3] and merit < threshold[4] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels all right with nature.  ("..merit..")", COLOR_BROWN, true)
	elseif merit >= threshold[4] and merit < threshold[5] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels content with nature.  ("..merit..")", COLOR_CYAN) -- capable of taming low-level animals
	elseif merit >= threshold[5] and merit < threshold[6] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels at peace with nature.  ("..merit..")", COLOR_GREEN) -- capable of taming high-level animals
	elseif merit >= threshold[6] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels in absolute harmony with nature.  ("..merit..")", COLOR_GREEN, true) -- capable of taming megabeasts
	end
end

function isBird(u)
	unitBlock = dfhack.maps.ensureTileBlock(u.pos.x,u.pos.y,u.pos.z)
	if u and unitBlock then
		if df.global.world.raws.creatures.all[u.race].caste[u.caste].flags.FLIER == true and unitBlock.designation[u.pos.x%16][u.pos.y%16].outside == true then return true
		else return false end
	else return false end
end

function getNewsFromAnimal(unit,u)
	creatureName = (df.global.world.raws.creatures.all[u.race]).name[0]
	if #df.global.timed_events > 0 then
		dfhack.gui.showAnnouncement( "The "..creatureName.." speaks with "..dfhack.TranslateName(unit.name).."...", COLOR_WHITE, true)
		for i=0, #df.global.timed_events-1, 1 do
			event = df.global.timed_events[i]
			distance = 0
			eventseason = event.season
			if eventseason < df.global.cur_season then eventseason = eventseason+3 end
			distance = event.season_ticks + ((df.global.cur_season - eventseason)*10080) - df.global.cur_season_tick
			entity_creature_name = {"","",""}
			if event.entity ~= nil then
				entity = event.entity
				entity_creature_name = (df.global.world.raws.creatures.all[entity.entity_raw.creature_ids[0]]).name
			end
			 -- 120 season ticks per day
			distance_string = "somewhere"
			if distance < 840 then -- 1 week
				distance_string = "right nearby"
			elseif distance < 1680 then -- 2 weeks
				distance_string = "close by"
			elseif distance < 3360 then -- 4 weeks
				distance_string = "approaching the area"
			elseif distance < 5040 then -- 6 weeks
				distance_string = "off in the distance"
			elseif distance < 6720 then -- 8 weeks
				distance_string = "somewhere in the region"
			elseif distance < 8400 then -- 10 weeks
				distance_string = "far away"
			elseif distance < 10080 then -- 12 weeks
				distance_string = "a great distance away"
			else -- 12 weeks
				distance_string = "far, far away"
			end
			
			if event.type == 0 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw a caravan of "..entity_creature_name[2].." traders "..distance_string..".", COLOR_WHITE, true)
			elseif event.type == 1 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw a group of migrating "..entity_creature_name[1].." "..distance_string..".", COLOR_WHITE, true)
			elseif event.type == 2 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw a dignified-looking "..entity_creature_name[0].." "..distance_string..".", COLOR_WHITE, true)
			elseif event.type == 3 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw a band of angry-looking "..entity_creature_name[1].." "..distance_string..".", COLOR_RED)
			elseif event.type == 4 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw (I DON'T KNOW WHAT THIS IS, TELL ME WHAT SHOWS UP) "..distance_string..".", COLOR_RED)
			elseif event.type == 5 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw a mighty beast "..distance_string..".", COLOR_RED)
			elseif event.type == 6 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw a curious-looking creature "..distance_string..".", COLOR_GREEN)
			elseif event.type == 7 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw a troublesome-looking creature "..distance_string..".", COLOR_GREEN)
			elseif event.type == 8 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw another flying creature "..distance_string..".", COLOR_GREEN)
			elseif event.type == 9 then
				dfhack.gui.showAnnouncement( "The "..creatureName.." saw a strange and unnatural monster "..distance_string..".", COLOR_RED)
			end
		end
	else
		dfhack.gui.showAnnouncement( "The "..creatureName.." has nothing interesting to report.", COLOR_WHITE, true)
	end
end

function nicknamePet(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end
	allUnits = df.global.world.units.active
	local animals = {}
	local u
	
	--Determine which animal is closest
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) then
			table.insert(animals, u)
		end
	end
	local leader = nil
	local leaderDistance = 10000
	local u
	for i=#animals,1,-1 do
		u = animals[i]
		distance = math.abs(u.pos.x - unit.pos.x) + math.abs(u.pos.y - unit.pos.y)
		if distance < leaderDistance then
			leader = u
			leaderDistance = distance
		end
	end
	--Nickname the animal
	if leader then
		local u = leader
		local uSpecies = (df.global.world.raws.creatures.all[u.race]).name[0]
		local script=require('gui/script')
		script.start(function()
			local nameok, name
			name = ""
			repeat nameok,name=script.showInputPrompt('Nickname','Give a nickname to the '..uSpecies..':',COLOR_LIGHTGREEN, amount) until true
			if name == "" then
			else
				u.name.nickname = name
				u.name.has_name = true
			end
		end)
	end
end

function tameRoamingAnimal(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local merit = getMerit()
	if merit > 0 then
		local probability = reaction.products[0].probability
		local skill = dfhack.units.getEffectiveSkill(unit,reaction.skill)
		if skill == 0 then skill = 1 end
		local radius = reaction.products[0].product_dimension
		if radius < 1 then radius = 10 end
		local view = fov.get_fov(radius,unit.pos)
		
		local lastAnimalName = ""
		local failureType = 0
		
		local found=false
		local success=false
		allUnits = df.global.world.units.other.ANY_ANIMAL
		local u
		for i=#allUnits-1,0,-1 do	-- search list in reverse
			u = allUnits[i]
			--print((df.global.world.raws.creatures.all[u.race]).name[0])
			if isRoamingAnimal(u) then
				if closeBy(unit,u,radius) then--isSeen(view,u.pos) then
					if dfhack.maps.canWalkBetween(u.pos,unit.pos) then
						found = true
						lastAnimalName = (df.global.world.raws.creatures.all[u.race]).name[0]
						unitRaw = df.global.world.raws.creatures.all[u.race]
						casteRaw = unitRaw.caste[u.caste]
						petValue = casteRaw.misc.petvalue
						
						if casteRaw.flags.PET == false and casteRaw.flags.PET_EXOTIC == false and petValue < 10000 then
							petValue = 10000
						end
						
						interestvalue = merit/10
						respectvalue = ((skill*5)^2)
						
						if math.random(100) < math.floor((1-((petValue/2)/respectvalue))*100) and math.random(100) < math.floor((1-((petValue/2)/interestvalue))*100) then
							u.flags1.tame = true
							u.training_level = df.animal_training_level.WellTrained
							--u.misc_trait_type.RevertWildTimer = 1200
							mo.make_own(u)
							u.relations.following = unit
							levelUp(unit,reaction.skill,petValue)
							success = true
							break
						else
							if 0 > math.floor((1-((petValue/2)/interestvalue))*100) then
								failureType = 2
							elseif 0 > math.floor((1-((petValue/2)/respectvalue))*100) then
								failureType = 1
							end
						end
					end
				end
			end
		end
		if not found == true then
			if #input_items > 0 then
				dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to spot a tameable animal in range." , COLOR_BROWN, true)
			end
		elseif success == true then
			dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has joined us." , COLOR_GREEN, true)--color[,is_bright]
			--now deal with any creatures that were following it
			if u then
				if isBird(u) then getNewsFromAnimal(unit,u) end
				for i=#allUnits-1,0,-1 do	-- search list in reverse
					newu = allUnits[i]
					if newu.relations.following == u then
						newu.relations.following = nil
					end
				end
			end
			if #input_items > 0 then
				input_items[0].flags.PRESERVE_REAGENT = false
			end
		else
			if failureType == 0 then
				dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to tame the "..lastAnimalName..".", COLOR_BROWN)
			elseif failureType == 1 then
				dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." is not skilled enough to tame the "..lastAnimalName..".", COLOR_BROWN)
			elseif failureType == 2 then
				dfhack.gui.showAnnouncement( "The "..lastAnimalName.." has no interest in joining us.", COLOR_BROWN)
			end
			levelUp(unit,reaction.skill,10)
			if #input_items > 0 then
				input_items[0].flags.PRESERVE_REAGENT = false
			end
		end
	else
		dfhack.gui.showAnnouncement( "You are too disconnected from nature to commune with animals.", COLOR_RED, true)
	end
end

local function getItemValue(item)
	local basevalue = 1
	local matvalue = 1
	local qualityvalue = 1
	local improvementvalue = 0
	local stack_size = 1
	local wearvalue = 1
	
	if item then
		local itemname = df.item_type[item:getType()]:lower()
		
		if itemname == "coin" then basevalue = 0.02
		elseif itemname == "corpsepiece"
			or itemname == "corpse"
			or itemname == "remains"
			or itemname == "rock"
			then basevalue = 0
		elseif itemname == "glob"
			or itemname == "seeds"
			or itemname == "drink"
			or itemname == "powder_misc"
			or itemname == "liquid_misc"
			or itemname == "orthopedic_cast"
			then basevalue = 1
		elseif itemname == "fish_raw"
			or itemname == "fish"
			or itemname == "meat"
			or itemname == "egg"
			or itemname == "plant"
			or itemname == "leaves"
			then basevalue = 2
		elseif itemname == "rough"
			or itemname == "wood"
			or itemname == "boulder"
			then basevalue = 3
		elseif itemname == "bar"
			or itemname == "blocks"
			or itemname == "gem"
			or itemname == "skin_tanned"
			then basevalue = 5
		elseif itemname == "thread"
			then basevalue = 6
		elseif itemname == "cloth"
			then basevalue = 7
		elseif itemname == "siegeammo"
			or itemname == "traction_bench"
			then basevalue = 20
		elseif itemname == "window"
			or itemname == "statue"
			then basevalue = 25
		elseif itemname == "catapultparts"
			or itemname == "ballistaparts"
			or itemname == "trapparts"
			then basevalue = 30
		else basevalue = 10
		end
		
		if itemname == "weapon"
			or itemname == "armor"
			or itemname == "shoes"
			or itemname == "gloves"
			or itemname == "shield"
			or itemname == "helm"
			or itemname == "ammo"
			or itemname == "pants"
			or itemname == "trapcomp"
			or itemname == "tool"
			then basevalue = item.subtype.value
		end
		
		
		
		material = dfhack.matinfo.decode(item)
		if material then
			matvalue = material.material.material_value
		end
		
		quality = item:getQuality()		
		if quality == 0 then qualityvalue = 1
		elseif quality == 1 then qualityvalue = 2
		elseif quality == 2 then qualityvalue = 3
		elseif quality == 3 then qualityvalue = 4
		elseif quality == 4 then qualityvalue = 5
		elseif quality == 5 then qualityvalue = 12
		end
		
		if item.flags.artifact == true then qualityvalue = 120 end
		if item:isImprovable(nil,0,0) then
			for i = 0, #item.improvements-1, 1 do
				imp = item.improvements[i]
				imp_mat = dfhack.matinfo.decode(imp.mat_type, imp.mat_index)
				imp_mat_value = imp_mat.material.material_value
				if imp.quality == 0 then imp_quality = 1
				elseif imp.quality == 1 then imp_quality = 2
				elseif imp.quality == 2 then imp_quality = 3
				elseif imp.quality == 3 then imp_quality = 4
				elseif imp.quality == 4 then imp_quality = 5
				elseif imp.quality == 5 then imp_quality = 12
				end
				imp_value = 10 * imp_mat_value * imp_quality
				improvementvalue = improvementvalue + imp_value
			end
		end
		--improvementvalue = item:getImprovementsValue(-1) -- crashes the game
		stack_size = item.stack_size
		wearvalue = 1-(item:getWear()*.25)
	end

	return math.floor((((basevalue * matvalue * qualityvalue) + improvementvalue)*stack_size)*wearvalue)
end

function releaseRoamingAnimal(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local probability = reaction.products[0].probability + dfhack.units.getEffectiveSkill(unit,reaction.skill)
	reaction.products[0].probability = probability
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 5 end
	local view = fov.get_fov(radius,unit.pos)

	local found=false
	local success=false
	local getItem=false

	v = df.global.world.units.other.ANY_ANIMAL
		
	for i=#v-1,0,-1 do	-- search list in reverse
		u = v[i]
		if closeBy(unit,u,radius) then
			if u.flags1.tame
			--and u.flags2.roaming_wilderness_population_source==true
			and not dfhack.units.isDead(u)
			and u.civ_id==df.global.ui.civ_id
			and u.relations.pet_owner_id==-1
			--and (u.training_level == df.animal_training_level.WellTrained or u.training_level == df.animal_training_level.Trained or u.training_level == df.animal_training_level.SemiWild)
			and not u.flags1.chained
			and not u.flags1.caged
			and not u.flags2.locked_in_for_trading then
				releaseAnimal(u)
				success=true
				if math.random(100) < probability then
					getItem=true
				end
				break
			end
		else
		end
	end
		
	if success == true then
		dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has been released into the wild.", COLOR_GREEN)
		levelUp(unit,reaction.skill,30)
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to find any animals to release.", COLOR_RED, true)
		for _,v in ipairs(input_reagents or {}) do
			v.flags.PRESERVE_REAGENT = true
		end
	end
	if getItem ~= true then
		reaction.products[0].probability = 0
	end
end


function talkBird(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local unitBlock = dfhack.maps.ensureTileBlock(unit.pos.x,unit.pos.y,unit.pos.z)
	if unitBlock.designation[unit.pos.x%16][unit.pos.y%16].outside == false then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." cannot talk to birds while inside." , COLOR_RED, true)
		return false
	end
	
	local merit = getMerit()
	
	if merit > 0 then		
		local found=false
		local success=false
		local creatureName=""
		allUnits = df.global.world.units.other.ANY_ANIMAL
		local u
		for i=#allUnits-1,0,-1 do	-- search list in reverse
			u = allUnits[i]
			if isRoamingAnimal(u) and isBird(u) then
				found = true
				break
			end
		end
		if not found == true then
			if #input_reagents > 0 then
				dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to spot a bird in range." , COLOR_BROWN, true)
				for _,v in ipairs(input_reagents or {}) do
					v.flags.PRESERVE_REAGENT = true
				end
			end
		else
			getNewsFromAnimal(unit,u)
			u.relations.following = nil
			u.animal.leave_countdown=2
			u.flags1.forest=true
			levelUp(unit,reaction.skill,30)
		end
	else
		dfhack.gui.showAnnouncement( "You are too disconnected from nature to commune with birds.", COLOR_RED, true)
	end
end

--[[
exp+30 per successful use

(respect for trainer) * (interest in activity) * (pet training level) * general difficulty
bond bonus: respect * 100

respect = trainer skill level^3*10 / petvalue OR petvalue (leader)/petvalue (follower)
value of a historical pet is *10

interest:
following - clusternumber*value, same species*5, predator factor, respect for leader
hunting - predator*10, carnivore*5, mischeivious*2
war - likes fighting*10, prone to rage*value+1, predator*5, benign/2

for untraining: 10/interest in training

predator factor (one is predator and carnivore and other is benign grazer) * 2, /5

]]--	

function getTrainingProb(trainer,u,reaction,multiplier)

	skill = dfhack.units.getEffectiveSkill(trainer,reaction.skill)
	respectvalue = ((skill*5)^2)
	--print("respect: "..respectvalue)
	
	local unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	petValue = casteRaw.misc.petvalue
	if multiplier ~= nil then
		respectvalue = respectvalue * multiplier
		--print("interest in activity: "..multiplier)
	end
	
	if u.relations.pet_owner_id == trainer.id then bondBonus = 100 else bondBonus = 1 end -- bond bonus
	if u.flags1.important_historical_figure then histBonus = 10 else histBonus = 1 end -- historical figure bonus
	--print("training: "..u.training_level)
	--print("names: "..bondBonus/histBonus)
	respectvalue = respectvalue * bondBonus * u.training_level / histBonus
	--print("totalRespect: "..respectvalue)
	--print("self-value: "..petValue)
	respectProb = ((1-((petValue/2)/respectvalue))*100)
	unitRaw = df.global.world.raws.creatures.all[u.race]
	--print("total prob: "..respectProb)
	return respectProb
end

function getFollowInterest(u, leader)
	local unitRaw = df.global.world.raws.creatures.all[u.race]
	local casteRaw = unitRaw.caste[u.caste]
	local leaderRaw = df.global.world.raws.creatures.all[leader.race]
	local leaderCasteRaw = leaderRaw.caste[leader.caste]
	
	currentClusterSize = 1
	v = df.global.world.units.other.ANY_ANIMAL
	for i=#v-1,0,-1 do
		z = v[i]
		if z.relations.following == leader then currentClusterSize = currentClusterSize + 1 end
	end
		--print(currentClusterSize)
	
	clusterVal = ((unitRaw.cluster_number[0]+unitRaw.cluster_number[1])/2)/currentClusterSize
	
	if u.race ~= leader.race then clusterVal = clusterVal/2 end
	if unitRaw.flags.LOOSE_CLUSTERS then clusterVal = clusterVal/2 end
	
	local unitValue = casteRaw.misc.petvalue
	local leaderValue = leaderCasteRaw.misc.petvalue
	if leaderCasteRaw.flags.CAN_LEARN and leaderCasteRaw.flags.CAN_SPEAK then
		if u.relations.pet_owner_id == leader.id then respectVal = 5 else respectVal = 1 end
	else
		respectVal = leaderValue/unitValue
	end
	
	--if u.flags1.important_historical_figure then unitValue = unitValue * 10 end
	--if leader.flags1.important_historical_figure then leaderValue = leaderValue * 10 end
	
	--if casteRaw.flags.CAN_SPEAK then unitValue * 10 end
	--if leaderRaw.flags.CAN_SPEAK then leaderValue * 10 end
	
	return clusterVal * respectVal
end

function getHuntingInterest(u)
	unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	local interest = 1
	if (casteRaw.flags.LARGE_PREDATOR or casteRaw.flags.AMBUSHPREDATOR) then interest = interest * 2 end
	if (casteRaw.flags.CARNIVORE or casteRaw.flags.BONECARN) then interest = interest * 2 end
	if casteRaw.flags.GRAZER then interest = interest / 2 end
	if casteRaw.flags.MEANDERER then interest = interest / 2 end
	return interest
end

function getWarInterest(u)
	unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	local interest = 1
	if casteRaw.flags.LIKES_FIGHTING then interest = interest * 2 end
	if casteRaw.misc.prone_to_rage > 0 then interest = interest * 2 end
	if casteRaw.flags.LARGE_PREDATOR then interest = interest * 2 end
	if casteRaw.flags.BENIGN then interest = interest / 2 end
	if casteRaw.flags.FLEEQUICK then interest = interest / 2 end
	if casteRaw.flags.MEANDERER then interest = interest / 2 end
	return interest
end
	--[[
	following - clusternumber*value, same species*5, predator factor, respect for leader
	hunting - predator*10, carnivore*5, mischeivious*2
	war - likes fighting*10, prone to rage*value+1, predator*5, benign/2
	]]--
	--casteRaw.flags.GRAZER
	--casteRaw.flags.CARNIVORE
	--casteRaw.flags.BONECARN
	--casteRaw.flags.BENIGN
	--casteRaw.flags.TRAINABLE_HUNTING
	--casteRaw.flags.TRAINABLE_WAR
	--casteRaw.flags.LARGE_PREDATOR
	--casteRaw.flags.FLEEQUICK
	
	--casteRaw.flags.MISCHIEVOUS
	--casteRaw.flags.LARGE_PREDATOR	
	--casteRaw.flags.AMBUSHPREDATOR
	--casteRaw.flags.MEANDERER
	--casteRaw.flags.LIKES_FIGHTING
	--casteRaw.flags.FEATURE_BEAST
	--casteRaw.flags.TITAN

function followUser(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	local skill = dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local trainerSkill = ((skill*skill*skill)+1)*10
	
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end

	local found = false
	local success = false
	allUnits = df.global.world.units.active
	local u
	local failureType = 0
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		cldrid = -1
		if u.relations.following ~= nil then cldrid = u.relations.following.id end
		if isPet(u) and closeBy(unit,u,radius) and unit.id ~= cldrid then
			found = true
			if u.relations.following ~= nil then
				currentLeaderInterest = getFollowInterest(u,u.relations.following)
			else
				currentLeaderInterest = 1
			end
			multiplier = getFollowInterest(u,unit)/currentLeaderInterest
			prob = getTrainingProb(unit,u,reaction,multiplier)
			
			if math.random(100) < prob then
				u.relations.following = unit
				success = true
				break
			elseif 0 < math.floor(prob) then failureType = 1 end
		end
	end
	
	if not found == true then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to spot a trainable animal in range." , COLOR_RED, true)
		for _,v in ipairs(input_reagents or {}) do
			v.flags.PRESERVE_REAGENT = true
		end
	elseif success == true then
		dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " is now following " .. dfhack.TranslateName(unit.name) .. "." , COLOR_GREEN)
		levelUp(unit,reaction.skill,30)
	elseif failureType == 1 then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to teach any animals to follow.", COLOR_BROWN)
		levelUp(unit,reaction.skill,10)
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." is not skilled enough to train the animal to follow.", COLOR_RED, true)
	end
end

function cluster(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local probability = reaction.products[0].probability + dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end
	allUnits = df.global.world.units.active
	local animals = {}
	local u
	local failureType = 0
	
	--First determine the leader
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) then
			table.insert(animals, u)
		end
	end
	local leader = nil
	local leaderDistance = 10000
	local u
	for i=#animals,1,-1 do
		u = animals[i]
		distance = math.abs(u.pos.x - unit.pos.x) + math.abs(u.pos.y - unit.pos.y)
		if distance < leaderDistance then
			leader = u
			leaderDistance = distance
		end
	end
	--Now do the actual training
	if leader then

		local found = false
		local success = false
		local u
	
		for i=#animals,1,-1 do
			u = animals[i]
			cldrid = -1
			if u.relations.following ~= nil then cldrid = u.relations.following.id end
			if u.id ~= leader.id and leader.id ~= cldrid then
				found = true
				
				if u.relations.following ~= nil then
					currentLeaderInterest = getFollowInterest(u,u.relations.following)
				else
					currentLeaderInterest = 1
				end
				
				multiplier = getFollowInterest(u,leader)/currentLeaderInterest
				
				prob = getTrainingProb(unit,u,reaction,multiplier)
				
				if math.random(100) < prob then
					u.relations.following = leader
					success = true
					break
				elseif 0 < math.floor(prob) then failureType = 1 end
			end
		end
		if not found == true then
			dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." could not find an animal to accompany the "..(df.global.world.raws.creatures.all[leader.race]).name[0].."." , COLOR_RED, true)
			for _,v in ipairs(input_reagents or {}) do
				v.flags.PRESERVE_REAGENT = true
			end
		elseif success == true then
			dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " is now following the "..(df.global.world.raws.creatures.all[leader.race]).name[0] .. ".", COLOR_GREEN)
			levelUp(unit,reaction.skill,30)
		elseif failureType == 1 then
			dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to train any animals to follow the "..(df.global.world.raws.creatures.all[leader.race]).name[0] .. ".", COLOR_BROWN)
			levelUp(unit,reaction.skill,10)
		else
			dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." is not skilled enough to train the animal to follow the "..(df.global.world.raws.creatures.all[leader.race]).name[0] .. ".", COLOR_RED, true)
		end
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to spot a trainable animal in range." , COLOR_RED, true)
	end
end

function unFollow(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local probability = reaction.products[0].probability + dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end
	allUnits = df.global.world.units.active
	local found = false
	local success = false
	local u
	local failureType = 0
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		local leader = u.relations.following
		if isPet(u) and closeBy(unit,u,radius) and leader ~= nil then
			found = true
			multiplier = 1/getFollowInterest(u,leader)
			
			prob = getTrainingProb(unit,u,reaction,multiplier)
			
			if math.random(100) < prob then
				u.relations.following = nil
				success = true
				break
			elseif 0 < math.floor(prob) then failureType = 1 end
		end
	end
	if not found == true then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." could not find any animals to dismiss." , COLOR_RED, true)
		for _,v in ipairs(input_reagents or {}) do
			v.flags.PRESERVE_REAGENT = true
		end
	elseif success == true then
		dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has been dismissed.", COLOR_GREEN)
		levelUp(unit,reaction.skill,30)
	elseif failureType == 1 then
		dfhack.gui.showAnnouncement( "The animal is not listening to "..dfhack.TranslateName(unit.name)..".", COLOR_BROWN)
		levelUp(unit,reaction.skill,10)
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." is not skilled enough to dismiss the animal.", COLOR_RED, true)
	end

end

function trainHunting(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	local skill = dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local trainerSkill = ((skill*skill*skill)+1)*10
	
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end

	local found = false
	local success = false
	allUnits = df.global.world.units.active
	local u
	local failureType = 0
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and u.profession == 102 then
			found = true
			multiplier = getHuntingInterest(u)
			
			prob = getTrainingProb(unit,u,reaction,multiplier)
			
			if math.random(100) < prob or casteRaw.flags.TRAINABLE_HUNTING then
				u.profession = 98
				success = true
				break
			elseif 0 < math.floor(prob) then failureType = 1 end
		end
	end
	
	if not found == true then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to spot a trainable animal in range." , COLOR_RED, true)
		for _,v in ipairs(input_reagents or {}) do
			v.flags.PRESERVE_REAGENT = true
		end
	elseif success == true then
		dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has been trained for hunting." , COLOR_GREEN)
		levelUp(unit,reaction.skill,30)
	elseif failureType == 1 then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to train any animals to hunt.", COLOR_BROWN)
		levelUp(unit,reaction.skill,10)
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." is not skilled enough to train the animal to hunt.", COLOR_RED, true)
	end
end

function trainWar(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	local skill = dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local trainerSkill = ((skill*skill*skill)+1)*10
	
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end

	local found = false
	local success = false
	allUnits = df.global.world.units.active
	local u
	local failureType = 0
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and u.profession == 102 then
			found = true
			multiplier = getWarInterest(u)
			
			prob = getTrainingProb(unit,u,reaction,multiplier)
			
			if math.random(100) < prob or casteRaw.flags.TRAINABLE_WAR then
				u.profession = 99
				success = true
				break
			elseif 0 < math.floor(prob) then failureType = 1 end
		end
	end
	
	if not found == true then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to spot a trainable animal in range." , COLOR_RED, true)
		for _,v in ipairs(input_reagents or {}) do
			v.flags.PRESERVE_REAGENT = true
		end
	elseif success == true then
		dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has been trained for war." , COLOR_GREEN)
		levelUp(unit,reaction.skill,30)
	elseif failureType == 1 then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to train any animals for combat.", COLOR_BROWN)
		levelUp(unit,reaction.skill,10)
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." is not skilled enough to train the animal for combat.", COLOR_RED, true)
	end
end

function unTrain(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	local skill = dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local trainerSkill = ((skill*skill*skill)+1)*10
	
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end

	local found = false
	local success = false
	allUnits = df.global.world.units.active
	local u
	local failureType = 0
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and u.profession ~= 103 and u.profession ~= 104 and u.profession ~= 102 then
			found = true
			
			currentJobInterest = 1
			if u.profession == 98 then
				multiplier = 1/getHuntingInterest(u)
			elseif u.profession == 99 then
				multiplier = 1/getWarInterest(u)
			end
			
			prob = getTrainingProb(unit,u,reaction,multiplier)
			
			if math.random(100) < prob then
				u.profession = 102
				success = true
				break
			elseif 0 < math.floor(prob) then failureType = 1 end
		end
	end
	
	if not found == true then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to spot a trained animal in range." , COLOR_RED, true)
		for _,v in ipairs(input_reagents or {}) do
			v.flags.PRESERVE_REAGENT = true
		end
	elseif success == true then
		dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has been unconditioned." , COLOR_GREEN)
		levelUp(unit,reaction.skill,30)
	elseif failureType == 1 then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to untrain any animals.", COLOR_BROWN)
		levelUp(unit,reaction.skill,10)
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." is not skilled enough to untrain the animal.", COLOR_RED, true)
	end
end



--Doesn't work
function setIdleArea(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local probability = reaction.products[0].probability + dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end
	allUnits = df.global.world.units.active
	local u
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and math.random(100) < probability then
			u.idle_area.x = unit.pos.x
			u.idle_area.y = unit.pos.y
			u.idle_area.z = unit.pos.z
		end
	end
end

--Doesn't work
function unsetIdleArea(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local probability = reaction.products[0].probability + dfhack.units.getEffectiveSkill(unit,reaction.skill)
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end
	allUnits = df.global.world.units.active
	local u
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and math.random(100) < probability then
			u.idle_area.x = -30000
			u.idle_area.y = -30000
			u.idle_area.z = -30000
		end
	end
end

function tameAllRoamingAnimals(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	v = df.global.world.units.active -- To include sentient creatures
	--v = df.global.world.units.other.ANY_ANIMAL
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local merit = getMerit()
	if merit > 0 then
		local probability = reaction.products[0].probability
		local skill = dfhack.units.getEffectiveSkill(unit,reaction.skill)
		if skill == 0 then skill = 1 end
		local success = false
		for i=#v-1,0,-1 do	-- search list in reverse
			u = v[i]
			if isRoamingAnimal(u) then
				if dfhack.maps.canWalkBetween(u.pos,unit.pos) then
					unitRaw = df.global.world.raws.creatures.all[u.race]
					casteRaw = unitRaw.caste[u.caste]
					petValue = casteRaw.misc.petvalue
					
					if casteRaw.flags.PET == false and casteRaw.flags.PET_EXOTIC == false and petValue < 10000 then
						petValue = 10000
					end
					
					interestvalue = merit/10
					--respectvalue = ((skill*5)^2)
					
					if math.random(100) < math.floor((1-((petValue/2)/interestvalue))*100) then
						u.flags1.tame = true
						u.training_level = df.animal_training_level.WellTrained
						--u.misc_trait_type.RevertWildTimer = 1200
						mo.make_own(u)
						u.relations.following = unit
						levelUp(unit,reaction.skill,petValue)
						success = true
					end
				end
			end
		end
		if success == true then
			dfhack.gui.showAnnouncement( "Animals have come in response to " .. dfhack.TranslateName(unit.name) .. "." , COLOR_GREEN, true)--color[,is_bright]
		else
			dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name) .. " called out to the creatures of the wild, but there was no response..." , COLOR_RED)
		end
	else
		dfhack.gui.showAnnouncement( "You are too out of touch with nature to tame animals this way." , COLOR_RED, true)
	end
end

function releaseAllRoamingAnimals(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	--v = df.global.world.units.active -- To include sentient creatures
	v = df.global.world.units.other.ANY_ANIMAL
	
	for i=#v-1,0,-1 do	-- search list in reverse
		u = v[i]
		--unitRaw = df.global.world.raws.creatures.all[u.race]
		if u.flags2.roaming_wilderness_population_source==true
		and not dfhack.units.isDead(u)
		and u.flags1.tame
		and u.civ_id==df.global.ui.civ_id
		and u.relations.pet_owner_id==-1
		and (u.training_level == df.animal_training_level.WellTrained or u.training_level == df.animal_training_level.Trained or u.training_level == df.animal_training_level.SemiWild)
		and not u.flags1.chained
		and not u.flags1.caged 
		and not u.flags2.locked_in_for_trading then
			releaseAnimal(u)
			dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has been released into the wild.", COLOR_RED)
		end
	end
end

function exileCivMember(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	if unit.civ_id==df.global.ui.civ_id then
		unit.civ_id=-1
		unit.flags1.tame=false
		unit.animal.leave_countdown=2
		unit.flags1.forest=true
		unit.relations.following = nil
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." has left your civilization.", COLOR_WHITE)
		
		merit = 0

		for inv_id,item_inv in ipairs(unit.inventory) do
			itemcheck = item_inv.item
			merit = merit + getItemValue(itemcheck)
		end
		
		druidMerit(merit)
	end
end

function isRoamingAnimal(u)
	unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	if u.flags2.roaming_wilderness_population_source
	and not dfhack.units.isDead(u)
	and not u.flags1.caged
	and not u.flags2.locked_in_for_trading
	and not u.flags1.tame
	and not dfhack.units.isOpposedToLife(u)
	and u.civ_id==-1
	and not u.flags1.merchant 
	and not u.flags1.diplomat
	and unitRaw.flags.LARGE_ROAMING
	and casteRaw.flags.NATURAL
	and (u.animal.leave_countdown > 0 or u.flags2.roaming_wilderness_population_source_not_a_map_feature == false) then
		return true
	end
	return false
end

function isPet(u)
	--unitRaw = df.global.world.raws.creatures.all[u.race]
	--casteRaw = unitRaw.caste[u.caste]
	if u.flags1.tame
	--and not dfhack.units.isDead(u)
	and u.civ_id==df.global.ui.civ_id
	and not dfhack.units.isOpposedToLife(u)
	and not u.flags1.merchant 
	and not u.flags1.diplomat
	and not u.flags2.locked_in_for_trading
	then
		return true
	end
	return false
end

function druidMerit(value)
	local gCode = df.global.world.world_data.active_site[0].id
	local druidMerit = dfhack.persistent.get(gCode..'_druid-merit')
	if druidMerit == nil then
		dfhack.persistent.save({key=gCode..'_druid-merit'})
		druidMerit = dfhack.persistent.get(gCode..'_druid-merit')
		druidMerit.ints[1] = 0
	end
	if value ~= nil then
		druidMerit.ints[1] = druidMerit.ints[1] + value
		druidMerit:save()
	end
	return druidMerit.ints[1]
end

--Gets merit instantly (but may cause a pause, so use it only in special situations)
function calculateMeritAll(respond)
	allUnits = df.global.world.units.all
	local u
	local merit = 0
	if respond=="console" then print("Passing judgement on behavior towards animals...") end
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		merit = merit + calculateUnitMerit(u,respond)
	end
	
	releasedMerit = druidMerit()
	if respond=="console" then print('Accumulated merit from released animals and item offerings:  +'..releasedMerit) end
	merit = merit + releasedMerit
	if respond=="console" then print("Total merit: "..merit) end
	totalMerit = merit
	newMerit = 0
	nextUnit = -1
	return(merit)
end

function calculateUnitMerit(u,respond)
	local merit = 0
	unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	name=casteRaw.caste_name[0]
	petValue = casteRaw.misc.petvalue
	
	
	local wildOriginMultiplier = 1
	if u.profession == 103 then childMultiplier = 2 
	else childMultiplier = 1 end
	
	local deathWeight = 1
	local casualty = 0
	
	local line = ""
	
	--if respond=="console" then print(casteRaw.flags[158]) end
	--if respond=="console" then print(casteRaw.flags[52]) end -- Both are unknown, and both seem to pop up on nonliving creatures.  I'm going to guess that one is NOT_LIVING and one is CANNOT_UNDEAD, which are identical.
	if casteRaw.flags[158] ~= true then
		
		--Pets
		if isPet(u) then
			if u.flags2.roaming_wilderness_population_source==true then wildOriginMultiplier = 5 end
			--first pass judgement on deaths
			if dfhack.units.isDead(u) then
				casualty = 5
				local accountedFor = false
				for _, d in ipairs(df.global.world.deaths.all) do
					--printall(d)
					death_id=d.victim
					if u.id==death_id then
						accountedFor = true
						if d.death_cause==df.death_type.STRUCK_DOWN then
							if respond=="console" then line = ('Pet '..name..' was struck down.') end
						elseif d.death_cause==df.death_type.BLEED then
							if respond=="console" then line = ('Pet '..name..' bled to death.') end
						elseif d.death_cause==df.death_type.COLLISION then
							if respond=="console" then line = ('Pet '..name..' was killed in a collision.') end
						elseif d.death_cause==df.death_type.HUNGER then
							if respond=="console" then line = ('Pet '..name..' starved to death.') end
							casualty = 20
						elseif d.death_cause==df.death_type.THIRST then
							if respond=="console" then line = ('Pet '..name..' died of thirst.') end
							casualty = 20
						elseif d.death_cause==df.death_type.SHOT then
							if respond=="console" then line = ('Pet '..name..' was shot.') end
						elseif d.death_cause==df.death_type.DROWN then
							if respond=="console" then line = ('Pet '..name..' drowned.') end
						elseif d.death_cause==df.death_type.SUFFOCATE then
							if respond=="console" then line = ('Pet '..name..' suffocated.') end
						elseif d.death_cause==df.death_type.FIRE then
							if respond=="console" then line = ('Pet '..name..' died in a fire.') end
						elseif d.death_cause==df.death_type.DRAGONFIRE then
							if respond=="console" then line = ('Pet '..name..' was killed by dragonfire.') end
						elseif d.death_cause==df.death_type.CAVEIN then
							if respond=="console" then line = ('Pet '..name..' was killed in a cave in.') end
						elseif d.death_cause==df.death_type.DRAWBRIDGE then
							if respond=="console" then line = ('Pet '..name..' was killed by a drawbridge.') end
						elseif d.death_cause==df.death_type.CAGE then
							if respond=="console" then line = ('Pet '..name..' died in a cage.') end
							casualty = 20
						elseif d.death_cause==df.death_type.MURDER then
							if respond=="console" then line = ('Pet '..name..' was murdered.') end
						elseif d.death_cause==df.death_type.TRAP then
							if respond=="console" then line = ('Pet '..name..' was killed by a trap.') end
						elseif d.death_cause==df.death_type.ABANDON then
							if respond=="console" then line = ('Pet '..name..' was abandoned.') end
						elseif d.death_cause==df.death_type.HEAT then
							if respond=="console" then line = ('Pet '..name..' died from heat.') end
						elseif d.death_cause==df.death_type.COLD then
							if respond=="console" then line = ('Pet '..name..' died from cold.') end
						elseif d.death_cause==df.death_type.SPIKE then
							if respond=="console" then line = ('Pet '..name..' was killed by a spike.') end
						elseif d.death_cause==df.death_type.ENCASE_LAVA then
							if respond=="console" then line = ('Pet '..name..' was encased in cooling lava.') end
						elseif d.death_cause==df.death_type.ENCASE_MAGMA then
							if respond=="console" then line = ('Pet '..name..' was encased in cooling magma.') end
						elseif d.death_cause==df.death_type.ENCASE_ICE then
							if respond=="console" then line = ('Pet '..name..' was encased in ice.') end
						elseif d.death_cause==df.death_type.INFECTION then
							if respond=="console" then line = ('Pet '..name..' succumbed to an infection.') end
						elseif d.death_cause==df.death_type.VEHICLE then
							if respond=="console" then line = ('Pet '..name..' was killed by a vehicle.') end
						elseif d.death_cause==df.death_type.FALLING_OBJECT then
							if respond=="console" then line = ('Pet '..name..' was killed by a falling object.') end
						elseif d.death_cause==df.death_type.DRAIN_BLOOD then
							if respond=="console" then line = ('Pet '..name..' was killed by the draining of blood.') end
						elseif d.death_cause==df.death_type.SLAUGHTER then
							if respond=="console" then line = ('Pet '..name..' was slaughtered.') end
							casualty = 50
						elseif d.death_cause==df.death_type.OLD_AGE then
							if respond=="console" then line = ('Pet '..name..' died of old age.') end
							casualty = 0
							merit = merit + (wildOriginMultiplier*petValue*10)
						else
							if respond=="console" then line = ('Pet '..name..' died.') end
						end
						break
					end
				end
				if accountedFor == false then
					if u.flags2.slaughter == true then
						if respond=="console" then line = ('Pet '..name..' was slaughtered.') end
						casualty = 50
					else
						if respond=="console" then line = ('Pet '..name..' died of unknown causes.') end
					end
				end
			else
			--Now pass judgement on living pets
				if respond=="console" then line = ('Taking care of '..name..'.') end
				merit = merit + (wildOriginMultiplier*petValue*childMultiplier) --Keeping a pet
			end
		elseif u.civ_id==-1
		and not u.flags1.tame
		and not dfhack.units.isOpposedToLife(u)
		and not u.flags1.merchant 
		and not u.flags1.diplomat
		and unitRaw.flags.LARGE_ROAMING
		and casteRaw.flags.NATURAL then
		--Wild animals (isRoamingAnimal)
			if dfhack.units.isDead(u) and u.relations.last_attacker_id ~= -1 then
				local killer = df.unit.find(u.relations.last_attacker_id)
				if killer ~= nil then
					if killer.civ_id == df.global.ui.civ_id and not killer.flags1.merchant and not killer.flags1.diplomat then
						if u.training_level == df.animal_training_level.WildUntamed then
							if isPet(killer) then
								if respond=="console" then line = ('Allowed a pet to kill a wild '..name..'.') end
								casualty = 1
							else
								if respond=="console" then line = ('Killed a wild '..name..'.') end
								casualty = 2
							end
						else
							if isPet(killer) then
								if respond=="console" then line = ('Set loose '..name..' and then set animals on it.') end
								casualty = 5
							else
								if respond=="console" then line = ('Set loose '..name..' and then killed it.') end
								casualty = 10
							end
						end
					end
				end
			end
		end
		
		--Restrained animal penalty
		if ((u.civ_id==-1 and u.training_level ~= df.animal_training_level.WildUntamed) or u.civ_id==df.global.ui.civ_id)
		and not u.flags1.merchant 
		and not u.flags1.diplomat
		and not u.flags2.locked_in_for_trading then
			if u.flags1.caged then
				merit = merit - (10*wildOriginMultiplier*childMultiplier*petValue) --Keeping an animal caged
				if respond=="console" then line = ('Keeping '..name..' caged.') end
			elseif u.flags1.chained then merit = merit - (5*wildOriginMultiplier*childMultiplier*petValue) --Keeping an animal chained
				if respond=="console" then line = ('Keeping '..name..' chained.') end
			end
		end
		
		--Training bonus (affects all creatures that originated in the wild)
		if u.flags2.roaming_wilderness_population_source==true
		and u.training_level ~= df.animal_training_level.WildUntamed
		and u.training_level ~= df.animal_training_level.SemiWild then
			merit = merit + (petValue * u.training_level * 7) -- Add bonus for training
		end
		
		merit = merit - (casualty*petValue*wildOriginMultiplier*childMultiplier)
		
		local meritLine = ""
		
		if merit > 0 then meritLine = "+"..merit
			else meritLine = ""..merit end
		
		if respond=="console" and merit ~= 0 then print(line ..'  '.. meritLine) end
		
		return merit
	else
		return 0
	end
end

function releaseAnimal(unit)
	u.flags1.tame = false
	u.civ_id=-1
	u.relations.following = nil
	u.animal.leave_countdown=2
	u.flags1.forest=true
	
	merit = 0
	unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	petValue = casteRaw.misc.petvalue
	adultMultiplier = 1
	if u.flags2.roaming_wilderness_population_source==true then wildOriginMultiplier = 5
	else 
		wildOriginMultiplier = 1 
		if u.profession ~= 103 then adultMultiplier = 2 end
	end
	local casualty = 0
	
	
	-- Add merit for release (same as keeping)
	merit = merit + (wildOriginMultiplier*adultMultiplier*petValue)
	
	--Training bonus (affects all creatures that originated in the wild)
	if u.flags2.roaming_wilderness_population_source==true
	and u.training_level ~= df.animal_training_level.WildUntamed
	and u.training_level ~= df.animal_training_level.SemiWild then
		merit = merit + (petValue * u.training_level * 7) -- Add bonus for training
	end
	
	merit = merit - (casualty*petValue*wildOriginMultiplier)

	for inv_id,item_inv in ipairs(unit.inventory) do
		itemcheck = item_inv.item
		merit = merit + getItemValue(itemcheck)
	end
	
	druidMerit(merit)
end

function callBird(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	season = df.global.cur_season
	season_ticks = df.global.cur_season_tick
	if (df.global.timed_events:insert('#', { new = df.timed_event, type = 8, season = season, season_ticks = season_ticks, entity = nil} )) then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." has summoned a bird.", COLOR_WHITE, true)
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." tried to summon a bird, but nothing happened...", COLOR_RED, true)
	end
end

function callCurious(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	season = df.global.cur_season
	season_ticks = df.global.cur_season_tick
	if (df.global.timed_events:insert('#', { new = df.timed_event, type = 8, season = season, season_ticks = season_ticks, entity = nil} )) then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." has made an offering to the wild.  Animals are coming to collect it.", COLOR_WHITE, true)
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." tried to make an offering to the wild, but nothing happened...", COLOR_RED, true)
	end
end


local function checkCuriousbeastSteal(unit_id,new_equip,item_id)
    local item = df.item.find(item_id)
    if not item then return false end
    local unit = df.unit.find(unit_id)
    if not unit then return false end
	local unitRaw = df.global.world.raws.creatures.all[unit.race]
	local casteRaw = unitRaw.caste[unit.caste]
	if unit.civ_id == -1 then
		itemInInventory = false
		for inv_id,item_inv in ipairs(unit.inventory) do
			itemchanged = item_inv.item
			if itemchanged.id == item_id then
				itemInInventory = true
				break
			end
		end
		if itemInInventory == true then
			itemvalue = getItemValue(item)
			druidMerit(itemvalue)
		else
			itemvalue = -(getItemValue(item))
			druidMerit(itemvalue)
		end
	end
end





eventful.enableEvent(eventful.eventType.INVENTORY_CHANGE,5)

eventful.onInventoryChange.itemsyndrome=function(unit_id,item_id,old_equip,new_equip)
    checkCuriousbeastSteal(unit_id,new_equip,item_id)
end




--------------------------------------------------
--------------------------------------------------
--http://lua-users.org/wiki/StringRecipes
function string.starts(String,Start)
   return string.sub(String,1,string.len(Start))==Start
end
--------------------------------------------------
--------------------------------------------------

dfhack.onStateChange.loadDruidism = function(code)
	local registered_reactions
	if code==SC_MAP_LOADED then
		--registered_reactions = {}
		for i,reaction in ipairs(df.global.world.raws.reactions) do
			if string.starts(reaction.code,'LUA_HOOK_DRUID_TAME_ANIMAL') then
				eventful.registerReaction(reaction.code,tameRoamingAnimal)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_RELEASE_ANIMAL') then
				eventful.registerReaction(reaction.code,releaseRoamingAnimal)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_NICKNAME') then
				eventful.registerReaction(reaction.code,nicknamePet)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_TAME_ALL') then
				eventful.registerReaction(reaction.code,tameAllRoamingAnimals)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_RELEASE_ALL') then
				eventful.registerReaction(reaction.code,releaseAllRoamingAnimals)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_EXILE') then
				eventful.registerReaction(reaction.code,exileCivMember)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_FOLLOW') then
				eventful.registerReaction(reaction.code,followUser)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_CLUSTER') then
				eventful.registerReaction(reaction.code,cluster)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_UNFOLLOW') then
				eventful.registerReaction(reaction.code,unFollow)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_SETIDLEAREA') then
				eventful.registerReaction(reaction.code,setIdleArea)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_UNSETIDLEAREA') then
				eventful.registerReaction(reaction.code,unsetIdleArea)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_TRAIN_HUNTING') then
				eventful.registerReaction(reaction.code,trainHunting)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_TRAIN_WAR') then
				eventful.registerReaction(reaction.code,trainWar)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_UNTRAIN') then
				eventful.registerReaction(reaction.code,unTrain)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_CHECK') then
				eventful.registerReaction(reaction.code,checkMerit)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_CALL_BIRD') then
				eventful.registerReaction(reaction.code,callBird)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_TALK_BIRD') then
				eventful.registerReaction(reaction.code,talkBird)
				registered_reactions = true
			elseif string.starts(reaction.code,'LUA_HOOK_DRUID_CALL_CURIOUS') then
				eventful.registerReaction(reaction.code,callCurious)
				registered_reactions = true
			end
		end
		--if #registered_reactions > 0 then print('Construct Creature: Loaded') end
		if registered_reactions then
			print('Druidism: Loaded.')
			dfhack.timeout(1,"ticks",function() update() end)
		end
	elseif code==SC_MAP_UNLOADED then
	end
end

-- if dfhack.init has already been run, force it to think SC_WORLD_LOADED to that reactions get refreshed
if dfhack.isMapLoaded() then dfhack.onStateChange.loadDruidism(SC_MAP_LOADED) end

function getMerit()
	if totalMerit == nil then
		return calculateMeritAll("")
	else
		return totalMerit
	end
end


timechange = 0
eventNow = false
seasonNow = false
if df.global.cur_season_tick < 3360 then
	month = 1
elseif df.global.cur_season_tick < 6720 then
	month = 2
else
	month = 3
end
power = 0
function update()
	eventFound = false
	for i=0,#df.global.timed_events-1,1 do
		event=df.global.timed_events[i]
		if event.season == df.global.cur_season and event.season_ticks <= df.global.cur_season_tick then
			if eventNow == false then
				--df.global.cur_season_tick=event.season_ticks
				event.season_ticks = df.global.cur_season_tick
				eventNow = true
			end
			eventFound = true
		end
	end
	if eventFound == false then eventNow = false end
	
	if df.global.cur_season_tick >= 3359 and df.global.cur_season_tick < 10079 and month == 1 then
		seasonNow = true
		month = 2
		if df.global.cur_season_tick > 3359 then
			df.global.cur_season_tick = 3360
		end
	elseif df.global.cur_season_tick >= 6719 and month == 2 then
		seasonNow = true
		month = 3
		if df.global.cur_season_tick > 6719 then
			df.global.cur_season_tick = 6720
		end
	elseif df.global.cur_season_tick >= 10079 then
		seasonNow = true
		month = 1
		if df.global.cur_season_tick > 10080 then
			df.global.cur_season_tick = 10079
		end
	else
		seasonNow = false
	end
	
	if eventNow == false and seasonNow == false then
		if df.global.cur_year > 0 then
			if timechange > 10000 then 
				timechange = timechange - 10000
				
				if df.global.cur_season_tick < 0 then
					df.global.cur_season_tick = df.global.cur_season_tick + 10080
					df.global.cur_season = df.global.cur_season-1
					eventNow = true
				end
				if df.global.cur_season < 0 then
					df.global.cur_season = df.global.cur_season + 4
					df.global.cur_year_tick = df.global.cur_year_tick + 403200
					df.global.cur_year = df.global.cur_year - 1
					eventNow = true
				end
				if eventNow == false then
					--df.global.cur_year_tick=df.global.cur_year_tick + 10
					--df.global.cur_season_tick=(math.floor(df.global.cur_year_tick/10))-((df.global.cur_season)*100800)
					
					df.global.cur_season_tick=df.global.cur_season_tick +1
					df.global.cur_year_tick=(df.global.cur_season_tick*10)+((df.global.cur_season)*100800)
				end
			end
		end
		timechange = timechange + power
	end
	
	--print("season_tick: "..df.global.cur_season_tick)-- = df.global.cur_season_tick + 10
	--print("year_tick: "..df.global.cur_year_tick)
	if nextUnit == -1 or nextUnit == nil then
		--Finish the old count
		releasedMerit = druidMerit()
		newMerit = newMerit + releasedMerit
		
		totalMerit = newMerit

		--Start a new count
		newMerit = 0
		allUnits = df.global.world.units.all
		nextUnit = #df.global.world.units.all-1
	end
	newMerit = newMerit + calculateUnitMerit(df.global.world.units.all[nextUnit],"")
	nextUnit = nextUnit - 1
	dfhack.timeout(1,"ticks",function() update() end)
end
