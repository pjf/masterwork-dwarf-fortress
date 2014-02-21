-- Do stuff with animals
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

function isSeen(view,pos)

	if pos[1] < view.xmin or pos[1] > view.xmax then
		return false
	end

	if pos[2] < view.ymin or pos[2] > view.ymax then
		return false
	end

	return view.z == pos[3] and view[pos[2]][pos[1]] > 0
	
	--return p.z == view.z and view[p.y][p.x] > 0
end

function closeBy(unit,u,radius)
	if unit.pos.x+radius >= u.pos.x
		and unit.pos.x-radius <= u.pos.x
		and unit.pos.y+radius >= u.pos.y
		and unit.pos.y-radius <= u.pos.y
		and unit.pos.z+5 >= u.pos.z
		and unit.pos.z-2 <= u.pos.z
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
		skill.rating = 1
	end
end

local my_entity=df.historical_entity.find(df.global.ui.civ_id)
--args={...}

function checkMerit(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	merit = calculateMeritAll("console")
	local threshold = {-10000000, -100000, 1000, 100000, 1000000, 100000000}
	if merit < threshold[1] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels absolutely abhored by nature.  "..merit, COLOR_RED, true)
	elseif merit >= threshold[1] and merit < threshold[2] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels despised by nature.  ("..merit..")", COLOR_RED)
	elseif merit >= threshold[2] and merit < threshold[3] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels disliked by nature.  ("..merit..")", COLOR_RED)
	elseif merit >= threshold[3] and merit < threshold[4] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels all right with nature.  ("..merit..")", COLOR_CYAN, true)
	elseif merit >= threshold[4] and merit < threshold[5] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels content with nature.  ("..merit..")", COLOR_GREEN)
	elseif merit >= threshold[5] and merit < threshold[6] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels at peace with nature.  ("..merit..")", COLOR_GREEN)
	elseif merit >= threshold[6] then
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." feels in absolute harmony with nature.  ("..merit..")", COLOR_GREEN, true)
	end
end

function tameRoamingAnimal(reaction,unit,job,input_items,input_reagents,output_items,call_native)
	unitRaw = df.global.world.raws.creatures.all[unit.race]
	local merit = getMerit()
	local probability = reaction.products[0].probability
	local skill = dfhack.units.getEffectiveSkill(unit,reaction.skill)
	if skill == 0 then skill = 1 end
	local radius = reaction.products[0].product_dimension
	if radius < 1 then radius = 10 end
	local view = fov.get_fov(radius,unit.pos)
	
	local found=false
	local success=false
	allUnits = df.global.world.units.other.ANY_ANIMAL
	local u
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		--print((df.global.world.raws.creatures.all[u.race]).name[0])
		if isRoamingAnimal(u) then
			if closeBy(unit,u,radius) then
				found = true
				unitRaw = df.global.world.raws.creatures.all[u.race]
				casteRaw = unitRaw.caste[u.caste]
				petValue = casteRaw.misc.petvalue
				
				if math.random(100) < (1/petValue)*merit*skill*0.01 then
					u.flags1.tame = true
					u.training_level = df.animal_training_level.WellTrained
					--u.misc_trait_type.RevertWildTimer = 1200
					mo.make_own(u)
					u.relations.following = unit
					levelUp(unit,reaction.skill,petValue)
					success = true
					break
				end
			end
		end
	end
	if not found == true then
		if #input_reagents > 0 then
			dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to spot a tameable animal in range." , COLOR_BROWN, true)
			for _,v in ipairs(input_reagents or {}) do
				v.flags.PRESERVE_REAGENT = true
			end
		end
	elseif success == true then
		dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has joined us." , COLOR_GREEN, true)--color[,is_bright]
		--now deal with any creatures that were following it
		if u then
			for i=#allUnits-1,0,-1 do	-- search list in reverse
				newu = allUnits[i]
				if newu.relations.following == u then
					newu.relations.following = nil
				end
			end
		end
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to tame any animals.", COLOR_BROWN)
	end
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
			and not u.flags1.caged then
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

function getFollowInterest(u, leader)
	local unitRaw = df.global.world.raws.creatures.all[u.race]
	local casteRaw = unitRaw.caste[u.caste]
	local leaderRaw = df.global.world.raws.creatures.all[leader.race]
	local leaderCasteRaw = unitRaw.caste[leader.caste]
	
	local clusterVal = (unitRaw.cluster_number[0]+unitRaw.cluster_number[1]+leaderRaw.cluster_number[0]+leaderRaw.cluster_number[1])/4
	if clusterVal < 1 then clusterVal = 1 end
	if u.race == leader.race then sameRaceBonus = 5 else sameRaceBonus = 1 end

	if leaderCasteRaw.flags.GRAZER and leaderCasteRaw.flags.BENIGN and casteRaw.flags.CARNIVORE and casteRaw.flags.LARGE_PREDATOR then predatorFactor = 2
	elseif casteRaw.flags.GRAZER and casteRaw.flags.BENIGN and leaderCasteRaw.flags.CARNIVORE and leaderCasteRaw.flags.LARGE_PREDATOR then predatorFactor = 0.5
	else predatorFactor = 1 end

	local unitValue = casteRaw.misc.petvalue
	local leaderValue = leaderCasteRaw.misc.petvalue
	
	if u.flags1.important_historical_figure then unitValue = unitValue * 10 end
	if leader.flags1.important_historical_figure then leaderValue = leaderValue * 10 end
	
	local respectVal = leaderValue/unitValue
	
	return clusterVal * sameRaceBonus * predatorFactor * respectVal
end

function getHuntingInterest(u)
	unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	local interest = 1
	if (casteRaw.flags.LARGE_PREDATOR or casteRaw.flags.AMBUSHPREDATOR) then interest = interest * 10 end
	if (casteRaw.flags.CARNIVORE or casteRaw.flags.BONECARN) then interest = interest * 5 end
	if casteRaw.flags.MISCHIEVOUS then interest = interest * 2 end
	if casteRaw.flags.GRAZER then interest = interest / 2 end
	
	return interest
end

function getWarInterest(u)
	unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	local interest = 1
	if casteRaw.flags.LIKES_FIGHTING then interest = interest * 10 end
	--if casteRaw.flags.PRONE_TO_RAGE
	if casteRaw.flags.LARGE_PREDATOR then interest = interest * 5 end
	if casteRaw.flags.BENIGN then interest = interest / 2 end
	
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
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) then
			found = true
			unitRaw = df.global.world.raws.creatures.all[u.race]
			casteRaw = unitRaw.caste[u.caste]
			petValue = casteRaw.misc.petvalue
			if u.relations.pet_owner_id == unit.id then bondBonus = 100 else bondBonus = 1 end -- bond bonus
			if u.flags1.important_historical_figure then petValue = petValue * 10 end -- historical figure bonus
			
			if math.random(100) < (trainerSkill/petValue) * u.training_level * bondBonus then
				u.relations.following = unit
				success = true
				break
			end
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
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to teach any animals to follow.", COLOR_BROWN)
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
	
		local skill = dfhack.units.getEffectiveSkill(unit,reaction.skill)
		local trainerSkill = ((skill*skill*skill)+1)*10

		local found = false
		local success = false
		local u
	
		for i=#animals,1,-1 do
			u = animals[i]
			if u.id ~= leader.id then
				found = true
				unitRaw = df.global.world.raws.creatures.all[u.race]
				casteRaw = unitRaw.caste[u.caste]
				petValue = casteRaw.misc.petvalue
				if u.relations.pet_owner_id == unit.id then bondBonus = 100 else bondBonus = 1 end -- bond bonus
				if u.flags1.important_historical_figure then petValue = petValue * 10 end -- historical figure bonus
				
				if math.random(100) < (trainerSkill/petValue) * u.training_level * bondBonus * getFollowInterest(u,leader) then
					u.relations.following = leader
					success = true
					break
				end

			end
		end
		if not found == true then
			dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." needs at least two nearby animals for training." , COLOR_RED, true)
			for _,v in ipairs(input_reagents or {}) do
				v.flags.PRESERVE_REAGENT = true
			end
		elseif success == true then
			dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " is now following the "..(df.global.world.raws.creatures.all[leader.race]).name[0] .. ".", COLOR_GREEN)
			levelUp(unit,reaction.skill,30)
		else
			dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to train any animals to follow the "..(df.global.world.raws.creatures.all[leader.race]).name[0] .. ".", COLOR_BROWN)
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
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and u.relations.following ~= -1 then
			found = true
			unitRaw = df.global.world.raws.creatures.all[u.race]
			casteRaw = unitRaw.caste[u.caste]
			petValue = casteRaw.misc.petvalue
			if u.relations.pet_owner_id == unit.id and u.relations.following == unit then bondBonus = 100 else bondBonus = 1 end -- bond bonus
			if u.flags1.important_historical_figure then petValue = petValue * 10 end -- historical figure bonus
			
			if math.random(100) < (trainerSkill/petValue) * u.training_level / bondBonus then
				u.relations.following = nil
				success = true
				break
			end
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
	else
		dfhack.gui.showAnnouncement( "The animal is not listening to "..dfhack.TranslateName(unit.name)..".", COLOR_BROWN)
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
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and u.profession == 102 then
			found = true
			unitRaw = df.global.world.raws.creatures.all[u.race]
			casteRaw = unitRaw.caste[u.caste]
			petValue = casteRaw.misc.petvalue
			if u.relations.pet_owner_id == unit.id then bondBonus = 100 else bondBonus = 1 end -- bond bonus
			if u.flags1.important_historical_figure then petValue = petValue * 10 end -- historical figure bonus
	
			if math.random(100) < (trainerSkill/petValue) * u.training_level * bondBonus * getHuntingInterest(u) or casteRaw.flags.TRAINABLE_HUNTING then
				u.profession = 98
				success = true
				break
			end
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
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to train any animals to hunt.", COLOR_BROWN)
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
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and u.profession == 102 then
			found = true
			unitRaw = df.global.world.raws.creatures.all[u.race]
			casteRaw = unitRaw.caste[u.caste]
			petValue = casteRaw.misc.petvalue
			if u.relations.pet_owner_id == unit.id then bondBonus = 100 else bondBonus = 1 end -- bond bonus
			if u.flags1.important_historical_figure then petValue = petValue * 10 end -- historical figure bonus
			
			if math.random(100) < (trainerSkill/petValue) * u.training_level * bondBonus * getWarInterest(u) or casteRaw.flags.TRAINABLE_WAR then
				u.profession = 99
				success = true
				break
			end
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
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to train any animals for combat.", COLOR_BROWN)
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
	
	for i=#allUnits-1,0,-1 do	-- search list in reverse
		u = allUnits[i]
		if isPet(u) and closeBy(unit,u,radius) and u.profession ~= 103 and u.profession ~= 104 then
			found = true
			unitRaw = df.global.world.raws.creatures.all[u.race]
			casteRaw = unitRaw.caste[u.caste]
			petValue = casteRaw.misc.petvalue
			if u.relations.pet_owner_id == unit.id then bondBonus = 100 else bondBonus = 1 end -- bond bonus
			if u.flags1.important_historical_figure then petValue = petValue * 10 end -- historical figure bonus
			
			currentJobInterest = 1
			if u.profession == 98 then
				currentJobInterest = getHuntingInterest(u)
			elseif u.profession == 99 then
				currentJobInterest = getWarInterest(u)
			end
			
			if math.random(100) < (trainerSkill/petValue) * u.training_level * bondBonus * (10/currentJobInterest) then
				u.profession = 102
				success = true
				break
			end
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
	else
		dfhack.gui.showAnnouncement( dfhack.TranslateName(unit.name).." was unable to untrain any animals.", COLOR_BROWN)
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
	--v = df.global.world.units.active -- To include sentient creatures
	v = df.global.world.units.other.ANY_ANIMAL
	
	for i=#v-1,0,-1 do	-- search list in reverse
		u = v[i]
		if isRoamingAnimal(u)
		then
			u.flags1.tame = true
			u.training_level = df.animal_training_level.WellTrained
			--u.misc_trait_type.RevertWildTimer = 1200
			mo.make_own(u)
			dfhack.gui.showAnnouncement( "The " .. (df.global.world.raws.creatures.all[u.race]).name[0] .. " has come to live with us." , COLOR_GREEN, true)--color[,is_bright]
		end
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
		and not u.flags1.caged then
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
	end
end

function isRoamingAnimal(u)
	unitRaw = df.global.world.raws.creatures.all[u.race]
	casteRaw = unitRaw.caste[u.caste]
	if u.flags2.roaming_wilderness_population_source
	and not dfhack.units.isDead(u)
	and not u.flags1.caged
	and not u.flags1.tame
	and not dfhack.units.isOpposedToLife(u)
	and u.civ_id==-1
	and not u.flags1.merchant 
	and not u.flags1.diplomat
	and unitRaw.flags.LARGE_ROAMING
	and casteRaw.flags.NATURAL 
	and (casteRaw.flags.PET or casteRaw.flags.PET_EXOTIC)
	and u.animal.leave_countdown > 0 then
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
	then
		return true
	end
	return false
end

function druidMerit(value)
	local druidMerit = dfhack.persistent.get('druid-merit')
	if druidMerit == nil then
		dfhack.persistent.save({key='druid-merit'})
		druidMerit = dfhack.persistent.get('druid-merit')
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
	if respond=="console" then print('Accumulated merit on released animals:  +'..releasedMerit) end
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
	
	if u.flags2.roaming_wilderness_population_source==true then wildOriginMultiplier = 5
	else wildOriginMultiplier = 1 end
	if u.profession == 103 then childMultiplier = 2 
	else childMultiplier = 1 end
	
	local deathWeight = 1
	local casualty = 0
	
	local line = ""
	
	--if respond=="console" then print(casteRaw.flags[158]) end
	--if respond=="console" then print(casteRaw.flags[52]) end -- Both are unknown, and both seem to pop up on nonliving creatures.  I'm going to guess that one is NOT_LIVING and one is CANNOT_UNDEAD, which are identical.
	if casteRaw.flags[158] ~= true then
		--if respond=="console" then print(casteRaw.flags[52]) end
		
		--Pets
		if isPet(u) then
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
		and not u.flags1.diplomat then
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
	
	druidMerit(merit)
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
			end
		end
		--if #registered_reactions > 0 then print('Construct Creature: Loaded') end
		if registered_reactions then
			print('Druidism: Loaded.')
			dfhack.timeout(1,"frames",function() update() end)
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

function update()
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
	dfhack.timeout(1,"frames",function() update() end)
end
