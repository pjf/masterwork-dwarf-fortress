-- makes custom announcements, for example when important items are created. 
-- needs to be called with autosyndrome, using a boiling rock with synclass:announcement.

-- i dont know what this does, but ok, lets leave it here.
local utils = require 'utils'

--again, no idea, but if I delete it, it wont work anymore. I could probably delete half of it though. It has something to do with the args, but I know I dont need entities. Doesnt matter.
local function findCiv(arg)
	local entities = df.global.world.entities.all
	if tonumber(arg) then return arg end
	if arg and not tonumber(arg) then 
		for eid,entity in ipairs(entities) do
			if entity.entity_raw.code == arg then return entity end
		end
	end
end

-- yep, whatever.
local args = {...}

-- Yeah, useful code: 
if not args then qerror("Needs an argument, for example: announcement golem") end

-- yep, whatever.
local EventType = args[1]

-- only works in fort/adv mode, not in main menu/startup.
if not dfhack.isMapLoaded() then
    qerror('Map is not loaded.')
end

-- list of all possible arguments. 
local function eventTypeIsNotValid()
	local eventTypes = 
	{
	"BIOLOGY_GOBLIN_ONE",
"BIOLOGY_GOBLIN_TWO",
"BIOLOGY_GOBLIN_THREE",
"BIOLOGY_GOBLIN_FOUR",
"BIOLOGY_GOBLIN_FIVE",
"BIOLOGY_KOBOLD_ONE",
"BIOLOGY_KOBOLD_TWO",
"BIOLOGY_KOBOLD_THREE",
"BIOLOGY_KOBOLD_FOUR",
"BIOLOGY_KOBOLD_FIVE",
"BIOLOGY_ORC_ONE",
"BIOLOGY_ORC_TWO",
"BIOLOGY_ORC_THREE",
"BIOLOGY_ORC_FOUR",
"BIOLOGY_ORC_FIVE",
"BIOLOGY_WARLOCK_ONE",
"BIOLOGY_WARLOCK_TWO",
"BIOLOGY_WARLOCK_THREE",
"BIOLOGY_WARLOCK_FOUR",
"BIOLOGY_WARLOCK_FIVE",
"BIOLOGY_AUTOMATON_ONE",
"BIOLOGY_AUTOMATON_TWO",
"BIOLOGY_AUTOMATON_THREE",
"BIOLOGY_AUTOMATON_FOUR",
"BIOLOGY_AUTOMATON_FIVE",
"BIOLOGY_FROST_GIANT_ONE",
"BIOLOGY_FROST_GIANT_TWO",
"BIOLOGY_FROST_GIANT_THREE",
"BIOLOGY_FROST_GIANT_FOUR",
"BIOLOGY_FROST_GIANT_FIVE",
"BIOLOGY_HUMAN_ONE",
"BIOLOGY_HUMAN_TWO",
"BIOLOGY_HUMAN_THREE",
"BIOLOGY_HUMAN_FOUR",
"BIOLOGY_HUMAN_FIVE",
"BIOLOGY_ELF_ONE",
"BIOLOGY_ELF_TWO",
"BIOLOGY_ELF_THREE",
"BIOLOGY_ELF_FOUR",
"BIOLOGY_ELF_FIVE",
"BIOLOGY_DROW_ONE",
"BIOLOGY_DROW_TWO",
"BIOLOGY_DROW_THREE",
"BIOLOGY_DROW_FOUR",
"BIOLOGY_DROW_FIVE",
"BIOLOGY_DWARF_EVIL_ONE",
"BIOLOGY_DWARF_EVIL_TWO",
"BIOLOGY_DWARF_EVIL_THREE",
"BIOLOGY_DWARF_EVIL_FOUR",
"BIOLOGY_DWARF_EVIL_FIVE",
"BIOLOGY_ANTMEN_ONE",
"BIOLOGY_ANTMEN_TWO",
"BIOLOGY_ANTMEN_THREE",
"BIOLOGY_ANTMEN_FOUR",
"BIOLOGY_ANTMEN_FIVE",
"BIOLOGY_BATMEN_ONE",
"BIOLOGY_BATMEN_TWO",
"BIOLOGY_BATMEN_THREE",
"BIOLOGY_BATMEN_FOUR",
"BIOLOGY_BATMEN_FIVE",
"BIOLOGY_GREMLIN_ONE",
"BIOLOGY_GREMLIN_TWO",
"BIOLOGY_GREMLIN_THREE",
"BIOLOGY_GREMLIN_FOUR",
"BIOLOGY_GREMLIN_FIVE",
"BIOLOGY_TROGLODYTE_ONE",
"BIOLOGY_TROGLODYTE_TWO",
"BIOLOGY_TROGLODYTE_THREE",
"BIOLOGY_TROGLODYTE_FOUR",
"BIOLOGY_TROGLODYTE_FIVE",
	}
	for _,v in ipairs(eventTypes) do
		if args[1] == v then return false end
	end
	return true
end	
	
	-- if your argument doesnt match the list above
if eventTypeIsNotValid() then
	qerror('Invalid argument. You need the name of an announcement to call.')
end

-- here comes your text
-- you can do ~60 symbols before a line-break is added. Keep it short for best effect.
-- color should accept all color tokens that are valid in raws.
--COLOR_CYAN, COLOR_WHITE, COLOR_MAGENTA, COLOR_RED, COLOR_BLACK, COLOR_GREEN, COLOR_YELLOW, COLOR_GREY, COLOR_BLUE
-- Text accepts all ANSI symbols, even things like ©¥¤¢†‡‰, calling tiles from the tileset.
-- If played without TrueTypeFont, this allows to make announcements that show tiles.

local function announcement_BIOLOGY_GOBLIN_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the goblin body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GOBLIN_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which goblin muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GOBLIN_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of goblin reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GOBLIN_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed goblin movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GOBLIN_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the goblin body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_KOBOLD_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the kobold body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_KOBOLD_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which kobold muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_KOBOLD_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of kobold reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_KOBOLD_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed kobold movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_KOBOLD_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the kobold body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ORC_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the orc body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ORC_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which orc muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ORC_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of orc reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ORC_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed orc movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ORC_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the orc body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_WARLOCK_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the warlock body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_WARLOCK_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which warlock muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_WARLOCK_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of warlock reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_WARLOCK_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed warlock movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_WARLOCK_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the warlock body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_AUTOMATON_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the automaton body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_AUTOMATON_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which automaton parts tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_AUTOMATON_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of automaton reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_AUTOMATON_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed automaton movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_AUTOMATON_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the automaton body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_FROST_GIANT_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the frost giant body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_FROST_GIANT_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which frost giant muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_FROST_GIANT_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of frost giant reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_FROST_GIANT_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed frost giant movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_FROST_GIANT_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the frost giant body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_HUMAN_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the human body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_HUMAN_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which human muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_HUMAN_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of human reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_HUMAN_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed human movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_HUMAN_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the human body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ELF_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the elven body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ELF_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which elven muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ELF_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of elven reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ELF_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed elven movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ELF_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the elven body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DROW_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the drow body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DROW_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which drow muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DROW_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of drow reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DROW_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed drow movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DROW_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the drow body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DWARF_EVIL_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the chaos-dwarf body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DWARF_EVIL_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which chaos-dwarf muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DWARF_EVIL_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of chaos-dwarf reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DWARF_EVIL_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed chaos-dwarf movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_DWARF_EVIL_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the chaos-dwarf body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ANTMEN_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the antmen body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ANTMEN_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which antmen muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ANTMEN_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of antmen reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ANTMEN_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed antmen movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_ANTMEN_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the antmen body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_BATMEN_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the batmen body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_BATMEN_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which batmen muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_BATMEN_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of batmen reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_BATMEN_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed batmen movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_BATMEN_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the batmen body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GREMLIN_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the gremlin body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GREMLIN_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which gremlin muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GREMLIN_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of gremlin reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GREMLIN_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed gremlin movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_GREMLIN_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the gremlin body (Double Damage)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_TROGLODYTE_ONE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist found a weakness in the troglodyte body composition that your warriors can exploit (-15% Strength/Tougness)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_TROGLODYTE_TWO() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has discovered which troglodyte muscles tire most and which wounds hurt the most (-15% Endurance/0% Recuperation)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_TROGLODYTE_THREE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist has given your warriors a greater understanding of troglodyte reactions (Remove NoStun, NoParalyze, NoFear)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_TROGLODYTE_FOUR() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist reconstructed troglodyte movements, allowing your soldiers to outmaneuver the enemy (-15% Agility/Speed)',
    COLOR_CYAN, true)end
local function announcement_BIOLOGY_TROGLODYTE_FIVE() dfhack.gui.showAnnouncement(
    'BIOLOGY SUCCESS: Your biologist discovered a fundemantel weakness on the troglodyte body (Double Damage)',
    COLOR_CYAN, true)end

	

-- for each announcement you need another line here.
if EventType=="BIOLOGY_GOBLIN_ONE" then announcement_BIOLOGY_GOBLIN_ONE()
elseif EventType=="BIOLOGY_GOBLIN_TWO" then announcement_BIOLOGY_GOBLIN_TWO()
elseif EventType=="BIOLOGY_GOBLIN_THREE" then announcement_BIOLOGY_GOBLIN_THREE()
elseif EventType=="BIOLOGY_GOBLIN_FOUR" then announcement_BIOLOGY_GOBLIN_FOUR()
elseif EventType=="BIOLOGY_GOBLIN_FIVE" then announcement_BIOLOGY_GOBLIN_FIVE()
elseif EventType=="BIOLOGY_KOBOLD_ONE" then announcement_BIOLOGY_KOBOLD_ONE()
elseif EventType=="BIOLOGY_KOBOLD_TWO" then announcement_BIOLOGY_KOBOLD_TWO()
elseif EventType=="BIOLOGY_KOBOLD_THREE" then announcement_BIOLOGY_KOBOLD_THREE()
elseif EventType=="BIOLOGY_KOBOLD_FOUR" then announcement_BIOLOGY_KOBOLD_FOUR()
elseif EventType=="BIOLOGY_KOBOLD_FIVE" then announcement_BIOLOGY_KOBOLD_FIVE()
elseif EventType=="BIOLOGY_ORC_ONE" then announcement_BIOLOGY_ORC_ONE()
elseif EventType=="BIOLOGY_ORC_TWO" then announcement_BIOLOGY_ORC_TWO()
elseif EventType=="BIOLOGY_ORC_THREE" then announcement_BIOLOGY_ORC_THREE()
elseif EventType=="BIOLOGY_ORC_FOUR" then announcement_BIOLOGY_ORC_FOUR()
elseif EventType=="BIOLOGY_ORC_FIVE" then announcement_BIOLOGY_ORC_FIVE()
elseif EventType=="BIOLOGY_WARLOCK_ON" then announcement_BIOLOGY_WARLOCK_ONE()
elseif EventType=="BIOLOGY_WARLOCK_TWO" then announcement_BIOLOGY_WARLOCK_TWO()
elseif EventType=="BIOLOGY_WARLOCK_THREE" then announcement_BIOLOGY_WARLOCK_THREE()
elseif EventType=="BIOLOGY_WARLOCK_FOUR" then announcement_BIOLOGY_WARLOCK_FOUR()
elseif EventType=="BIOLOGY_WARLOCK_FIVE" then announcement_BIOLOGY_WARLOCK_FIVE()
elseif EventType=="BIOLOGY_AUTOMATON_ONE" then announcement_BIOLOGY_AUTOMATON_ONE()
elseif EventType=="BIOLOGY_AUTOMATON_TWO" then announcement_BIOLOGY_AUTOMATON_TWO()
elseif EventType=="BIOLOGY_AUTOMATON_THREE" then announcement_BIOLOGY_AUTOMATON_THREE()
elseif EventType=="BIOLOGY_AUTOMATON_FOUR" then announcement_BIOLOGY_AUTOMATON_FOUR()
elseif EventType=="BIOLOGY_AUTOMATON_FIVE" then announcement_BIOLOGY_AUTOMATON_FIVE()
elseif EventType=="BIOLOGY_FROST_GIANT_ONE" then announcement_BIOLOGY_FROST_GIANT_ONE()
elseif EventType=="BIOLOGY_FROST_GIANT_TWO" then announcement_BIOLOGY_FROST_GIANT_TWO()
elseif EventType=="BIOLOGY_FROST_GIANT_THREE" then announcement_BIOLOGY_FROST_GIANT_THREE()
elseif EventType=="BIOLOGY_FROST_GIANT_FOUR" then announcement_BIOLOGY_FROST_GIANT_FOUR()
elseif EventType=="BIOLOGY_FROST_GIANT_FIVE" then announcement_BIOLOGY_FROST_GIANT_FIVE()
elseif EventType=="BIOLOGY_HUMAN_ONE" then announcement_BIOLOGY_HUMAN_ONE()
elseif EventType=="BIOLOGY_HUMAN_TWO" then announcement_BIOLOGY_HUMAN_TWO()
elseif EventType=="BIOLOGY_HUMAN_THREE" then announcement_BIOLOGY_HUMAN_THREE()
elseif EventType=="BIOLOGY_HUMAN_FOUR" then announcement_BIOLOGY_HUMAN_FOUR()
elseif EventType=="BIOLOGY_HUMAN_FIVE" then announcement_BIOLOGY_HUMAN_FIVE()
elseif EventType=="BIOLOGY_ELF_ONE" then announcement_BIOLOGY_ELF_ONE()
elseif EventType=="BIOLOGY_ELF_TWO" then announcement_BIOLOGY_ELF_TWO()
elseif EventType=="BIOLOGY_ELF_THREE" then announcement_BIOLOGY_ELF_THREE()
elseif EventType=="BIOLOGY_ELF_FOUR" then announcement_BIOLOGY_ELF_FOUR()
elseif EventType=="BIOLOGY_ELF_FIVE" then announcement_BIOLOGY_ELF_FIVE()
elseif EventType=="BIOLOGY_DROW_ONE" then announcement_BIOLOGY_DROW_ONE()
elseif EventType=="BIOLOGY_DROW_TWO" then announcement_BIOLOGY_DROW_TWO()
elseif EventType=="BIOLOGY_DROW_THREE" then announcement_BIOLOGY_DROW_THREE()
elseif EventType=="BIOLOGY_DROW_FOUR" then announcement_BIOLOGY_DROW_FOUR()
elseif EventType=="BIOLOGY_DROW_FIVE" then announcement_BIOLOGY_DROW_FIVE()
elseif EventType=="BIOLOGY_DWARF_EVIL_ONE" then announcement_BIOLOGY_DWARF_EVIL_ONE()
elseif EventType=="BIOLOGY_DWARF_EVIL_TWO" then announcement_BIOLOGY_DWARF_EVIL_TWO()
elseif EventType=="BIOLOGY_DWARF_EVIL_THREE" then announcement_BIOLOGY_DWARF_EVIL_THREE()
elseif EventType=="BIOLOGY_DWARF_EVIL_FOUR" then announcement_BIOLOGY_DWARF_EVIL_FOUR()
elseif EventType=="BIOLOGY_DWARF_EVIL_FIVE" then announcement_BIOLOGY_DWARF_EVIL_FIVE()
elseif EventType=="BIOLOGY_ANTMEN_ONE" then announcement_BIOLOGY_ANTMEN_ONE()
elseif EventType=="BIOLOGY_ANTMEN_TWO" then announcement_BIOLOGY_ANTMEN_TWO()
elseif EventType=="BIOLOGY_ANTMEN_THREE" then announcement_BIOLOGY_ANTMEN_THREE()
elseif EventType=="BIOLOGY_ANTMEN_FOUR" then announcement_BIOLOGY_ANTMEN_FOUR()
elseif EventType=="BIOLOGY_ANTMEN_FIVE" then announcement_BIOLOGY_ANTMEN_FIVE()
elseif EventType=="BIOLOGY_BATMEN_ONE" then announcement_BIOLOGY_BATMEN_ONE()
elseif EventType=="BIOLOGY_BATMEN_TWO" then announcement_BIOLOGY_BATMEN_TWO()
elseif EventType=="BIOLOGY_BATMEN_THREE" then announcement_BIOLOGY_BATMEN_THREE()
elseif EventType=="BIOLOGY_BATMEN_FOUR" then announcement_BIOLOGY_BATMEN_FOUR()
elseif EventType=="BIOLOGY_BATMEN_FIVE" then announcement_BIOLOGY_BATMEN_FIVE()
elseif EventType=="BIOLOGY_GREMLIN_ONE" then announcement_BIOLOGY_GREMLIN_ONE()
elseif EventType=="BIOLOGY_GREMLIN_TWO" then announcement_BIOLOGY_GREMLIN_TWO()
elseif EventType=="BIOLOGY_GREMLIN_THREE" then announcement_BIOLOGY_GREMLIN_THREE()
elseif EventType=="BIOLOGY_GREMLIN_FOUR" then announcement_BIOLOGY_GREMLIN_FOUR()
elseif EventType=="BIOLOGY_GREMLIN_FIVE" then announcement_BIOLOGY_GREMLIN_FIVE()
elseif EventType=="BIOLOGY_TROGLODYTE_ONE" then announcement_BIOLOGY_TROGLODYTE_ONE()
elseif EventType=="BIOLOGY_TROGLODYTE_TWO" then announcement_BIOLOGY_TROGLODYTE_TWO()
elseif EventType=="BIOLOGY_TROGLODYTE_THREE" then announcement_BIOLOGY_TROGLODYTE_THREE()
elseif EventType=="BIOLOGY_TROGLODYTE_FOUR" then announcement_BIOLOGY_TROGLODYTE_FOUR()
elseif EventType=="BIOLOGY_TROGLODYTE_FIVE" then announcement_BIOLOGY_TROGLODYTE_FIVE()
end

-- This script has been made possible by code stolen from Siren.lua, ForceEvent.lua (Putnam) and brute-force coding by me, Meph. Dont laugh, and if anyone knows how to add a units name ("Dwarf1 has created a two-handed weapon") let me know.
