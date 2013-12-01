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
	"marker",
	"markerstop",
	"golemold",
	"loot",
	"lich",
	"engineer",
	"megabeastreanimate",
	"rockforge",
	"gemforge",
	"glassforge",
	"warbeast",
	"fletcher",
	"greatweapon",
	"artificer",
	"machine",
	"siren",
	"steamengine",
	"magma",
	"greatforge",
	"ammocaster",
	"gunsmith",
	"workbench",
	"siegeworks",
	"armory",
	"weaponry",
	"runearmory",
	"runeweaponry",
	"golemforge",
	"volcanicfoundry",
	"wizardschool",
	"townportal",
	"altarwhite",
	"altarblack",
	"altarelemental",
	"altarelair",
	"altarelwater",
	"altarelearth",
	"warpstone",
	"vampire",
	"necromancy",
	"werebeast",
	"megabeast",
	"demon",
	"trebuchet",
	"coinmint",
	"printingpress",
	"arbalest",
	"gas",
	"tarpitch",
	"finishingforge",
	"blastfurnace",
	"metallurgist",
	"crucible",
	"alchemy",
	"chemistry",
	"biology",
	"botanical",
	"toxic",
	"book",
"spelltome",
"catalystrod",
"phylactery",
"arcane",
"firemage1",
"firemage2",
"firemage3",
"airmage1",
"airmage2",
"airmage3",
"watermage1",
"watermage2",
"watermage3",
"earthmage1",
"earthmage2",
"earthmage3",
"blackmagev",
"blackmaged",
"blackmagel",
"whitemageh",
"whitemagep",
"whitemagew",
"pipeweed",
"bagofloot",
"musket",
"pistol",
"cannon",
"largegem",
"lobotomy",
"batchcoke",
"batchmetalalu",
"batchmetalbra",
"batchmetalbro",
"batchmetalcop",
"batchmetaliro",
"batchmetalgol",
"batchmetalpig",
"batchmetalpla",
"batchmetalsil",
"batchmetalste",
"batchmetalspr",
"batchmetaltin",
"batchmetalzin",
"bread",
"candy",
"cavecheese",
"adawafer",
"fire",
"armok1",
"armok2",
"armok3",
"armok4",
"armok5",
"armok6",
"armokdeath",
"golemheart",
"guardian",
"weaponupgrade",
"armorupgrade",
"shieldupgrade",
"plateset",
"ink",
"trapupgrade",
"bedroomset",
"noblebedroomset",
"officeroomset",
"graveroomset",
"diningroomset",
"storageroomset",
"crateopen",
"blackpowder",
"megabeastbutcher",
"archeologist",
"bardgear",
"diplomatgear",
"caravangear",
"spygear",
"expeditiongear",
"mercenarygear",
"expedloot",
"expedcaravan",
"runeweapon",
"runeammo",
"runetrap",
"poisonweapon",
"poisontrap",
"poisonammo",
"blueprint",
"clothset",
"leatherarmorset",
"golemactivate",
"golemspeed",
"golemstrength",
"golemaoe",
"golem",
"runebeast",
"beastarmor",
"landmine",
"turret",
"arenafight",
"runearmor",
"caravanforce",
"migrantforce",
"diplomatforce",
"beastforce",
"siegeforce",
"guild",
"military",
"exitguild",
"exitmilitary",
"prison",
"smokepipe",
"music",
"mutation",
"printingpressrunning",
"speech",
"twohandedweapon",
"kiteshield",
"armorset",
"legendary",
"apostle",
"vampiretrans",
"devampiretrans",
"werebeasttrans",
"undeadtrans",
"arbalesttrans",
"trebuchettrans",
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
local function announcement_golemold()
dfhack.gui.showAnnouncement(
    'A mighty golem has been created.',
    COLOR_BROWN, true
)
end

local function announcement_loot()
dfhack.gui.showAnnouncement(
    'Your warriors succesfully raided their target.',
    COLOR_BROWN, true
)
end

local function announcement_lich()
dfhack.gui.showAnnouncement(
    'Blinded by the promise of power, a dwarf has turned into a lich!',
    COLOR_BROWN, true
)
end

local function announcement_greatweapon() dfhack.gui.showAnnouncement(
    'A two-handed weapon has been created.',
    COLOR_BROWN, true)end
	
	
local function announcement_artificer() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: ARTIFICERS CHAMBER - A dwarf has learned to build clockwork creatures, giving you access to powerful metal warbeasts.',
    COLOR_CYAN, true)end
local function announcement_machine() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: MACHINE FACTORY - A dwarf discovered a way to build landmines, turrets and other defensive constructs. Industrious dwarves build these from toolkits.',
    COLOR_CYAN, true)end
local function announcement_siren() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: ALARM SIREN - Combining pipes and pressurized air, a dwarf has invented a siren. It can be used to wake up sleeping dwarves and stop breaks, calling you dwarves to war.',
    COLOR_CYAN, true)end
local function announcement_steamengine() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: STEAM ENGINE - Pressurized and heated water allow the production of mechanical power, which can be transfered by axles and gears.',
    COLOR_CYAN, true)end
local function announcement_magma() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: LIQUID SPAWNER - Studying gems and rock, a dwarf realized the importance of magma as fuel. A magma-version of each furnace is made available and the Liquid Spawner can produce the magma.',
    COLOR_CYAN, true)end	
local function announcement_greatforge() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: GREATFORGE - Careful measurements of bodysize and reach, a dwarf designed several two-handed weapons and greater shields. Conventional forges are too small, so the Greatforge was developed.',
    COLOR_CYAN, true)end
local function announcement_ammocaster() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: AMMO CASTER - Studying impact and velocity of bolts, a dwarf designed three new ammo types and the casting process to mass produce them.',
    COLOR_CYAN, true)end
local function announcement_siegeworks() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: HEAVY SIEGEWORKS - Taking pressure evaluations and stress levels of ballista and catapult parts, a dwarf has discovered a way to produce these from metals.',
    COLOR_CYAN, true)end
local function announcement_weaponry() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: WEAPONRY - Forging and sharpening processes were improved by careful experimentation and a new way to upgrade melee weapons was discovered.',
    COLOR_CYAN, true)end
local function announcement_armory() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: ARMORY - Combining leather straps, mailshirts and armorplates, a dwarf has developed the full platemail armor. The very best of dwarven protection, this armor is created in the specialised Armory.',	
    COLOR_CYAN, true)end
local function announcement_runeweaponry() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: RUNE WEAPONRY - Experimenting with mystical symbols of dwarven ancients, a dwarf has discovered the offensive properties of rune-coating. Weapons, traps and ammo can now be coated in damaging runes.',
    COLOR_CYAN, true)end
local function announcement_runearmory() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: RUNE ARMORY - Experimenting with the mystical symbols of dwarven ancients, a dwarf has discovered the defensive properties of rune-coating. Armors can now be coated in protective runes.',
    COLOR_CYAN, true)end
local function announcement_gunsmith() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: GUNSMITH - Despite the loss of several fingers, a dwarf finished the invention of blackpowdered projectile weapons. Muskets, Flintlock-Pistols and Hand-Cannons are now part of your arsenal.',
    COLOR_CYAN, true)end
local function announcement_workbench() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: INVENTORS WORKBENCH - Watching moods and foreign weaponry, a dwarf devised a specialised workbench that can copy or create those items directly.',
    COLOR_CYAN, true)end
local function announcement_golemforge() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: GOLEMFORGE - After extensive research, a dwarf rediscovered the old art of Golem-Making. Dwarves can transfer their souls into statues of moving metal, forming the ultimate warrior.',	
    COLOR_CYAN, true)end
local function announcement_volcanicfoundry() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: VOLCANIC FOUNDRY - After many experiments and just as many burned eyebrows, a dwarf discovered a new alloy, volcanic metal. It is the prime material a fortress can produce and does extra damage against frost giants.',
    COLOR_CYAN, true)end
local function announcement_trebuchet() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: TREBUCHET - After studying catapult parts and pulleys, complex mechanical formulas have resulted in a more advanced catapult, the trebuchet. Firing area-of-effect ammo, this machine is deadly.',
    COLOR_CYAN, true)end
	local function announcement_book() dfhack.gui.showAnnouncement(
    'A dwarf has written a book.',
    COLOR_CYAN, true)end
	
local function announcement_spelltome() dfhack.gui.showAnnouncement(
    'A dwarf has written a spelltome.',
    COLOR_CYAN, true)end
	
local function announcement_catalystrod() dfhack.gui.showAnnouncement(
    'A catalystrod has been created, ready to channel magical power.',
    COLOR_CYAN, true)end
	
local function announcement_phylactery() dfhack.gui.showAnnouncement(
    'A phylactery has been created, ready to store magical power.',
    COLOR_CYAN, true)end
	
local function announcement_arcane() dfhack.gui.showAnnouncement(
    'A dwarf has become a student of the Wizards School.',
    COLOR_CYAN, true)end
	
local function announcement_firemage1() dfhack.gui.showAnnouncement(
    'A dwarf has learned the secret of firemagic.',
    COLOR_CYAN, true)end
	
local function announcement_firemage2() dfhack.gui.showAnnouncement(
    'A dwarf has expanded his knowledge about firemagic.',
    COLOR_CYAN, true)end
	
local function announcement_firemage3() dfhack.gui.showAnnouncement(
    'A dwarf has mastered the art of conjuring fire.',
    COLOR_CYAN, true)end
	
local function announcement_airmage1() dfhack.gui.showAnnouncement(
    'A dwarf has learned the secret of airbending.',
    COLOR_CYAN, true)end
	
local function announcement_airmage2() dfhack.gui.showAnnouncement(
    'A dwarf has expanded his knowledge about airbending.',
    COLOR_CYAN, true)end
	
local function announcement_airmage3() dfhack.gui.showAnnouncement(
    'A dwarf has mastered the art of airbending.',
    COLOR_CYAN, true)end
	
local function announcement_watermage1() dfhack.gui.showAnnouncement(
    'A dwarf has learned the secret of watermagic.',
    COLOR_CYAN, true)end
	
local function announcement_watermage2() dfhack.gui.showAnnouncement(
    'A dwarf has expanded his knowledge of watermagic.',
    COLOR_CYAN, true)end
	
local function announcement_watermage3() dfhack.gui.showAnnouncement(
    'A dwarf has mastered the art of watermagic.',
    COLOR_CYAN, true)end
	
local function announcement_earthmage1() dfhack.gui.showAnnouncement(
    'A dwarf has learned the secret of earthmagic.',
    COLOR_CYAN, true)end
	
local function announcement_earthmage2() dfhack.gui.showAnnouncement(
    'A dwarf has expanded his knowledge of earthmagic.',
    COLOR_CYAN, true)end
	
local function announcement_earthmage3() dfhack.gui.showAnnouncement(
    'A dwarf has mastered the art of earthmagic.',
    COLOR_CYAN, true)end
	
local function announcement_blackmagev() dfhack.gui.showAnnouncement(
    'A dwarf has been turned into a vampire mage.',
    COLOR_CYAN, true)end
	
local function announcement_blackmaged() dfhack.gui.showAnnouncement(
    'A dwarf has bound a demon to his will.',
    COLOR_CYAN, true)end
	
local function announcement_blackmagel() dfhack.gui.showAnnouncement(
    'A dwarf turned undead, serving the fortress as a Lich.',
    COLOR_CYAN, true)end
	
local function announcement_whitemageh() dfhack.gui.showAnnouncement(
    'A dwarf has become a cleric healer.',
    COLOR_CYAN, true)end
	
local function announcement_whitemagep() dfhack.gui.showAnnouncement(
    'A dwarf has spoken the vow of an Arcane Protector.',
    COLOR_CYAN, true)end
	
local function announcement_whitemagew() dfhack.gui.showAnnouncement(
    'A dwarf has become a Witch Hunter.',
    COLOR_CYAN, true)end
	
local function announcement_pipeweed() dfhack.gui.showAnnouncement (
    'A bag of pipeweed has been created.',
    COLOR_CYAN, true)end
	
local function announcement_bagofloot() dfhack.gui.showAnnouncement(
    'A bag of kobold loot has been opened.',
    COLOR_CYAN, true)end
	
local function announcement_musket() dfhack.gui.showAnnouncement(
    'A musket has been created in the Gunsmith.',
    COLOR_CYAN, true)end
	
local function announcement_pistol() dfhack.gui.showAnnouncement(
    'A flint-lock pistol has been created in the Gunsmith.',
    COLOR_CYAN, true)end
	
local function announcement_cannon() dfhack.gui.showAnnouncement(
    'A hand-cannon has been created in the Gunsmith.',
    COLOR_CYAN, true)end
	
local function announcement_largegem() dfhack.gui.showAnnouncement(
    'A large gem has been created in the Gemcutter.',
    COLOR_CYAN, true)end
	
local function announcement_lobotomy() dfhack.gui.showAnnouncement(
    'A dwarf has been lobotomized.',
    COLOR_CYAN, true)end
	
local function announcement_batchcoke() dfhack.gui.showAnnouncement(
    'A batch of coke has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalalu() dfhack.gui.showAnnouncement(
    'A batch of aluminum bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalbra() dfhack.gui.showAnnouncement(
    'A batch of brass bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalbro() dfhack.gui.showAnnouncement(
    'A batch of bronze bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalcop() dfhack.gui.showAnnouncement(
    'A batch of copper bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetaliro() dfhack.gui.showAnnouncement(
    'A batch of iron bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalgol() dfhack.gui.showAnnouncement(
    'A batch of gold bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalpig() dfhack.gui.showAnnouncement(
    'A batch of pig iron bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalpla() dfhack.gui.showAnnouncement(
    'A batch of platinum has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalsil() dfhack.gui.showAnnouncement(
    'A batch of silver bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalste() dfhack.gui.showAnnouncement(
    'A batch of steel bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalspr() dfhack.gui.showAnnouncement(
    'A batch of spring-steel bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetaltin() dfhack.gui.showAnnouncement(
    'A batch of tin bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_batchmetalzin() dfhack.gui.showAnnouncement(
    'A batch of zinc bars has been created in the Blast Furnace.',
    COLOR_CYAN, true)end
	
local function announcement_bread() dfhack.gui.showAnnouncement(
    'A dwarf baked a loaf of bread in the Kitchen.',
    COLOR_CYAN, true)end
	
local function announcement_candy() dfhack.gui.showAnnouncement(
    'A dwarf made candy in the Kitchen.',
    COLOR_CYAN, true)end
	
local function announcement_cavecheese() dfhack.gui.showAnnouncement(
    'A dwarf made cave-fungi cheese in the Kitchen.',
    COLOR_CYAN, true)end
	
local function announcement_adawafer() dfhack.gui.showAnnouncement(
    'An adamantine wafer has been finished.',
    COLOR_CYAN, true)end
	
local function announcement_fire() dfhack.gui.showAnnouncement(
    'A fire has been started.',
    COLOR_CYAN, true)end
	
local function announcement_armok1() dfhack.gui.showAnnouncement(
    'Armok is watching you.',
    COLOR_CYAN, true)end
	
local function announcement_armok2() dfhack.gui.showAnnouncement(
    'Armok is pleased by your prayers.',
    COLOR_CYAN, true)end
	
local function announcement_armok3() dfhack.gui.showAnnouncement(
    'Armok hears your prayers.',
    COLOR_CYAN, true)end
	
local function announcement_armok4() dfhack.gui.showAnnouncement(
    'Armok is pleased by your offering.',
    COLOR_CYAN, true)end
	
local function announcement_armok5() dfhack.gui.showAnnouncement(
    'Armok rewards those true of faith.',
    COLOR_CYAN, true)end
	
local function announcement_armok6() dfhack.gui.showAnnouncement(
    'Armok accepts your sacrifice.',
    COLOR_CYAN, true)end
	
local function announcement_armokdeath() dfhack.gui.showAnnouncement(
    'A dwarf has been sacrificed to Armok, God of Blood.',
    COLOR_CYAN, true)end
	
local function announcement_golemheart() dfhack.gui.showAnnouncement(
    'A golemheart has been created.',
    COLOR_CYAN, true)end
	
local function announcement_guardian() dfhack.gui.showAnnouncement(
    'A Guardian of Armok has been created.',
    COLOR_CYAN, true)end
	
local function announcement_weaponupgrade() dfhack.gui.showAnnouncement(
    'A weapon has been upgraded.',
    COLOR_CYAN, true)end
	
local function announcement_armorupgrade() dfhack.gui.showAnnouncement(
    'An armor has been upgraded.',
    COLOR_CYAN, true)end
	
local function announcement_shieldupgrade() dfhack.gui.showAnnouncement(
    'A shield has been upgraded.',
    COLOR_CYAN, true)end
	
local function announcement_plateset() dfhack.gui.showAnnouncement(
    'A set of platearmor has been finished.',
    COLOR_CYAN, true)end
	
local function announcement_ink() dfhack.gui.showAnnouncement(
    'A batch of ink has been created.',
    COLOR_CYAN, true)end
	
local function announcement_trapupgrade() dfhack.gui.showAnnouncement(
    'A trap has been upgraded.',
    COLOR_CYAN, true)end
	
local function announcement_bedroomset() dfhack.gui.showAnnouncement(
    'A bedroom set has been finished in the Furniture Shop.',
    COLOR_CYAN, true)end
	
local function announcement_noblebedroomset() dfhack.gui.showAnnouncement(
    'A noble bedroom set has been finished in the Furniture Shop.',
    COLOR_CYAN, true)end
	
local function announcement_officeroomset() dfhack.gui.showAnnouncement(
    'A office set has been finished in the Furniture Shop.',
    COLOR_CYAN, true)end
	
local function announcement_graveroomset() dfhack.gui.showAnnouncement(
    'A grave set has been finished in the Furniture Shop..',
    COLOR_CYAN, true)end
	
local function announcement_diningroomset() dfhack.gui.showAnnouncement(
    'A diningroom set has been finished in the Furniture Shop.',
    COLOR_CYAN, true)end
	
local function announcement_storageroomset() dfhack.gui.showAnnouncement(
    'A storage set has been finished in the Furniture Shop.',
    COLOR_CYAN, true)end
	
local function announcement_crateopen() dfhack.gui.showAnnouncement(
    'A crate has been opened at the Trade Storehouse.',
    COLOR_CYAN, true)end
	
local function announcement_blackpowder() dfhack.gui.showAnnouncement(
    'A blackpowder pouch has been created.',
    COLOR_CYAN, true)end
	
local function announcement_megabeastbutcher() dfhack.gui.showAnnouncement(
    'A megabeast has been butchered.',
    COLOR_CYAN, true)end
	
local function announcement_archeologist() dfhack.gui.showAnnouncement(
    'The Archeologist has discovered something.',
    COLOR_CYAN, true)end
	
local function announcement_bardgear() dfhack.gui.showAnnouncement(
    'A bard is ready for his journey.',
    COLOR_CYAN, true)end
	
local function announcement_diplomatgear() dfhack.gui.showAnnouncement(
    'A diplomat is ready for his journey.',
    COLOR_CYAN, true)end
	
local function announcement_caravangear() dfhack.gui.showAnnouncement(
    'A caravan wagon is loaded and ready for its journey.',
    COLOR_CYAN, true)end
	
local function announcement_spygear() dfhack.gui.showAnnouncement(
    'A spy is awaiting your command.',
    COLOR_CYAN, true)end
	
local function announcement_expeditiongear() dfhack.gui.showAnnouncement(
    'An expedition is ready to be send on its way.',
    COLOR_CYAN, true)end
	
local function announcement_mercenarygear() dfhack.gui.showAnnouncement(
    'A group of mercenaries awaits your command.',
    COLOR_CYAN, true)end
	
local function announcement_expedloot() dfhack.gui.showAnnouncement(
    'Loot has been returned to the fortress.',
    COLOR_CYAN, true)end
	
local function announcement_expedcaravan() dfhack.gui.showAnnouncement(
    'A caravan has returned to the fortress.',
    COLOR_CYAN, true)end
	
local function announcement_runeweapon() dfhack.gui.showAnnouncement(
    'A weapon has been engraved with runes.',
    COLOR_CYAN, true)end
	
local function announcement_runeammo() dfhack.gui.showAnnouncement(
    'A stack of ammo has been engraved with runes.',
    COLOR_CYAN, true)end
	
local function announcement_runetrap() dfhack.gui.showAnnouncement(
    'A trap has been engraved with runes.',
    COLOR_CYAN, true)end
	
local function announcement_poisonweapon() dfhack.gui.showAnnouncement(
    'A weapon has been poisoned.',
    COLOR_CYAN, true)end
	
local function announcement_poisontrap() dfhack.gui.showAnnouncement(
    'A trap has been poisoned.',
    COLOR_CYAN, true)end
	
local function announcement_poisonammo() dfhack.gui.showAnnouncement(
    'A stack of ammo has been poisoned.',
    COLOR_CYAN, true)end
	
local function announcement_blueprint() dfhack.gui.showAnnouncement(
    'The Architect finished a blueprint.',
    COLOR_CYAN, true)end
	
local function announcement_clothset() dfhack.gui.showAnnouncement(
    'The Tailor finished a set of clothing.',
    COLOR_CYAN, true)end
	
local function announcement_leatherarmorset() dfhack.gui.showAnnouncement(
    'The Tailor finished a set of leather armor.',
    COLOR_CYAN, true)end
	
local function announcement_golemactivate() dfhack.gui.showAnnouncement(
    'A golem has been activated.',
    COLOR_CYAN, true)end
	
local function announcement_golemspeed() dfhack.gui.showAnnouncement(
    'A golem has been upgraded with greater speed.',
    COLOR_CYAN, true)end
	
local function announcement_golemstrength() dfhack.gui.showAnnouncement(
    'A golem has been upgraded with greater power.',
    COLOR_CYAN, true)end
	
local function announcement_golemaoe() dfhack.gui.showAnnouncement(
    'A golem has been given a powerful new attack.',
    COLOR_CYAN, true)end
	
local function announcement_golem() dfhack.gui.showAnnouncement(
    'A mighty golem has been created, encasing the soul of a dwarf in cold, hard metal.',
    COLOR_CYAN, true)end
	
local function announcement_runebeast() dfhack.gui.showAnnouncement(
    'An armored warbeast has been engraved with runes.',
    COLOR_CYAN, true)end
	
local function announcement_beastarmor() dfhack.gui.showAnnouncement(
    'A warbeast has been clad in armor.',
    COLOR_CYAN, true)end
	
local function announcement_landmine() dfhack.gui.showAnnouncement(
    'A landmine has been created.',
    COLOR_CYAN, true)end
	
local function announcement_turret() dfhack.gui.showAnnouncement(
    'A turret has been created.',
    COLOR_CYAN, true)end
	
local function announcement_arenafight() dfhack.gui.showAnnouncement(
    'A monster has been summoned to the Colosseum.',
    COLOR_CYAN, true)end
	
local function announcement_runearmor() dfhack.gui.showAnnouncement(
    'An armor has been engraved with runes.',
    COLOR_CYAN, true)end
	
local function announcement_caravanforce() dfhack.gui.showAnnouncement(
    'Your diplomat has invited a caravan.',
    COLOR_CYAN, true)end
	
local function announcement_migrantforce() dfhack.gui.showAnnouncement(
    'Your diplomat has invited migration.',
    COLOR_CYAN, true)end
	
local function announcement_diplomatforce() dfhack.gui.showAnnouncement(
    'Your diplomat has invited foreign diplomats for a meeting.',
    COLOR_CYAN, true)end
	
local function announcement_beastforce() dfhack.gui.showAnnouncement(
    'Your diplomat has called out challanges to great beasts.',
    COLOR_CYAN, true)end
	
local function announcement_siegeforce() dfhack.gui.showAnnouncement(
    'Your diplomat has declared a war.',
    COLOR_CYAN, true)end
	
local function announcement_guild() dfhack.gui.showAnnouncement(
    'A dwarf has joined a guild.',
    COLOR_CYAN, true)end
	
local function announcement_military() dfhack.gui.showAnnouncement(
    'A dwarf has joined a military group.',
    COLOR_CYAN, true)end
	
local function announcement_exitguild() dfhack.gui.showAnnouncement(
    'A dwarf has left a guild.',
    COLOR_CYAN, true)end
	
local function announcement_exitmilitary() dfhack.gui.showAnnouncement(
    'A dwarf has left a military group.',
    COLOR_CYAN, true)end
	
local function announcement_prison() dfhack.gui.showAnnouncement(
    'A dwarf has been thrown into prison.',
    COLOR_CYAN, true)end
	
local function announcement_smokepipe() dfhack.gui.showAnnouncement(
    'A dwarf has smoked a relaxing pipe.',
    COLOR_CYAN, true)end
	
local function announcement_music() dfhack.gui.showAnnouncement(
    'A bard is playing music in the Tavern.',
    COLOR_CYAN, true)end
	
local function announcement_mutation() dfhack.gui.showAnnouncement(
    'Something has mutated in the Warpstone Pool.',
    COLOR_CYAN, true)end
	
local function announcement_printingpressrunning() dfhack.gui.showAnnouncement(
    'The Printing Press is running at full power.',
    COLOR_CYAN, true)end
	
local function announcement_speech() dfhack.gui.showAnnouncement(
    'A dwarf is holding a public speech.',
    COLOR_CYAN, true)end
	
local function announcement_twohandedweapon() dfhack.gui.showAnnouncement(
    'A two-handed weapon has been created in the Greatforge.',
    COLOR_CYAN, true)end
	
local function announcement_kiteshield() dfhack.gui.showAnnouncement(
    'A kiteshield has been created in the Greatforge.',
    COLOR_CYAN, true)end
	
local function announcement_armorset() dfhack.gui.showAnnouncement(
    'An armorset has been finished in the Greatforge.',
    COLOR_CYAN, true)end
	
local function announcement_legendary() dfhack.gui.showAnnouncement(
    'A dwarf has been made a legend.',
    COLOR_CYAN, true)end
	
local function announcement_apostle() dfhack.gui.showAnnouncement(
    'A dwarf has joined the Apostles of Armok.',
    COLOR_CYAN, true)end
	
local function announcement_vampire() dfhack.gui.showAnnouncement(
    'A dwarf has been turned into a vampire.',
    COLOR_CYAN, true)end
	
local function announcement_devampire() dfhack.gui.showAnnouncement(
    'A dwarf has been cured from vampirism.',
    COLOR_CYAN, true)end
	
local function announcement_werebeast() dfhack.gui.showAnnouncement(
    'A dwarf has been turned into a werebeast.',
    COLOR_CYAN, true)end
	
local function announcement_undead() dfhack.gui.showAnnouncement(
    'A dwarf summons dark powers to raise the dead!',
    COLOR_CYAN, true)end
	
local function announcement_arbalest() dfhack.gui.showAnnouncement(
    'An Arabalest is now manned.',
    COLOR_CYAN, true)end
	
local function announcement_trebuchet() dfhack.gui.showAnnouncement(
    'A Trebuchet is now manned.',
    COLOR_CYAN, true)end
	

local function announcement_arbalest() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: ARBALEST - Combining levers, pulleys and crossbows, a dwarf has created a new form of siege weaponry, the arbalest battery. Firing quick volleys of bolts, this machine works well against high numbers of foes.',
    COLOR_CYAN, true)end
local function announcement_gas() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: GAS DISPERSER - Fuming acids and occasional bursts of madness aside, research proved a success. Acids can now be boiled into a gas-cloud affecting all creatures in range.',
    COLOR_CYAN, true)end
local function announcement_tarpitch() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: TAR POT - A seemingly simply invention, the lighting of tar, pitch and boiling oil in a designated area proves a valueable addition to dwarven defenses.',	
    COLOR_CYAN, true)end
local function announcement_vampire() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: VAMPIRES ABODE - Experimenting with blood and the occult, a dwarf has recreated Porphyric Hemophilia, also known as Vampirism. Now you know why you fear science.',
    COLOR_CYAN, true)end
local function announcement_necromancy() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: NECROMANCERS CRYPT - Studying dead animals and bones, the hearts of the living and more unusually non-dwarven topics, this dwarf has approached the secret of immortality. With a twist.',
    COLOR_CYAN, true)end
local function announcement_werebeast() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: WEREBEASTS LAIR - Examination of transformations and moon-cycles allowed greater insights into Lycanthrophy, which causes were-transformations. Leading dwarven scientists believe this can be harnessed to create super-soldiers.',
    COLOR_CYAN, true)end
local function announcement_megabeast() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: MEGABEASTS CELL - Autopsy results of megabeast corpses have shown tremendous amounts of new informations. Completely different from normal creatures, the soul is directly accessable and can be used to resurrect the monster or empower a single dwarf with its might.',
    COLOR_CYAN, true)end
local function announcement_demon() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: DEMONS DEN - Studying the dead carcass of demonic beasts, a dwarf found the very essence of chaotic influence in them. Infusing a dwarf with this substance could give incredible powers, at a price.',	
    COLOR_CYAN, true)end
local function announcement_wizardschool() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: WIZARDS SCHOOL - Ley lines, Tarot cards, Star formations and more did not misguide dwarven researchers, which discovered the magical properties of platinum. Used as a catalyst it can awaken the spark of magic in a dwarf.',
    COLOR_CYAN, true)end
	local function announcement_townportal() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: TOWNPORTAL - A frame of platinum and a door made of orichalcum unlocked the secrets of teleportation. You can now summon dwarves directly from the mountainhomes.',
    COLOR_CYAN, true)end
local function announcement_altarwhite() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: ENLIGHTEND ALTAR - Conducting experiments on plants and trees that only appear in good biomes, a wizards student discovered their magical properties. Many positive effects are linked to these items and can even be used to battle evil.',
    COLOR_CYAN, true)end
local function announcement_altarblack() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: NECROPHAGIC ALTAR - Conducting experiments on plants and trees that only appear in evil biomes, a wizards student discovered their magical properties. Many devastating effects are linked to these items and can even be used to battle good.',
    COLOR_CYAN, true)end
local function announcement_altarelemental() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: EMBERBLAZE ALTAR - Combining the lifeforce of fire with obsidian and kindling this spirit with oil, a student of the wizards school has found a way to control fire',
    COLOR_CYAN, true)end
	local function announcement_altarair() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: STORMTEMPEST ALTAR - A student of the wizards school has learned to control the flow of air. While initially thought uninteresting, this attidude changed when the teacher was hit by lighting',
    COLOR_CYAN, true)end
	local function announcement_altarwater() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: OCEANGUIDE ALTAR - Guiding the miniatur waves and currents inside a bucket of water, a student of the Wizards School has discovered that its inhabited by magical spirits which can be controled',
    COLOR_CYAN, true)end
	local function announcement_altarearth() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: MOUNTAINHEART ALTAR - A former miner who became a student of magic quickly understood the magical properties of the very stone around us. Which explains the strange murmurs he often heard in the deep',
    COLOR_CYAN, true)end
local function announcement_warpstone() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: WARPSTONE POOL - Careful experimentation with the volatile and highly toxic warpstone, a way to mutate creatures into monsters has been developed. No warranties.',	
    COLOR_CYAN, true)end
local function announcement_blastfurnace() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: BLAST FURNACE - Combining a smelter with greater capacity and bellows to raise the temperature, this new invention allows smelting of great ore batches.',
    COLOR_CYAN, true)end
local function announcement_finishingforge() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: FINISHING FORGE - Studying the bending yield and strain of metallic ores, a dwarf devised a way to cold-hammer ore into metal bars. Inefficient in outcome, but very fuel efficient.',
    COLOR_CYAN, true)end
local function announcement_metallurgist() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: METALLURGIST - Combining different base metals into more powerful alloys is the result of complex studies and experiments. Dwarves can now create alloys in the Metallurgist.',
    COLOR_CYAN, true)end
local function announcement_crucible() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: CRUCIBLE - Experimenting with melting points and high-temperature cauldrons, dwarven sciences newest creation is the Crucible. Higher metals can be smelted here, and some alloys are created more efficiently.',
    COLOR_CYAN, true)end
local function announcement_alchemy() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: ALCHEMY LAB - Tranmutation studies and alchemical properties of glass and gems have been collected and put together in a great magnum opum, allowing dwarves to practice alchemy.',	
    COLOR_CYAN, true)end
local function announcement_chemistry() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: CHEMISTRY LAB - Finding base elements in many minerals and ores, dwarven scientists have created exciting new developements converning fire and explosions, as well as melting things* with acid. *Flesh',
    COLOR_CYAN, true)end
local function announcement_biology() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: BIOLOGY LAB - Autopsy results and careful observation of their natural enemies have lead to better understanding of their foes. Combat improvements and new tactics against specific races can now be developed.',
    COLOR_CYAN, true)end
local function announcement_botanical() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: HERBALIST LAB - Undisturbed by being called "A dirty Elf-Fondler", a dwarven scientist his carefully categorized herbs and plants, together with their healing properties and other applications.',
    COLOR_CYAN, true)end
local function announcement_toxic() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: TOXIC LAB - Drowish in mind, a dwarven scientist has managed to develope a way to use tallow as binding agent to apply poisons on weapons, traps and ammo.',
    COLOR_CYAN, true)end
local function announcement_printingpress() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: PRINTING PRESS - Dwarven ingenuity has led to a machine that can create multiple batches of books and pamphlets. Cheap mass production of reading materials leads to a growth of Speaker Podiums thoughout the fortress.',
    COLOR_CYAN, true)end
local function announcement_coinmint() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: COIN MINT - Finest dwarven mechanical engineering, as well as many work accidents, have lead to the invention of the mint. Dies press big batches of coins directly from metal, without the need for fuel.',
    COLOR_CYAN, true)end
local function announcement_megabeastreanimate() dfhack.gui.showAnnouncement(
    'A megabeast has been reanimated with the help of a changeling',
    COLOR_CYAN, true)end
	local function announcement_engineer() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: ENGINEER - Advancements in automated traps and many mutilated lab mice have lead to new engineering insight about deadly trap upgrades.',
    COLOR_CYAN, true)end
local function announcement_fletcher() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: FLETCHER - Aerodynamics and tension tests have improved both ranged weapons and ammo. Repeating crossbows and different types of arrow-heads are now available.',
    COLOR_CYAN, true)end
	local function announcement_rockforge() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: ROCKFORGE - Breakline and brittleness of stones thoroughly researched, a way to create weapons and armor of pure rock has been developed. Cheap, but heavy.',
    COLOR_CYAN, true)end
local function announcement_gemforge() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: GEMFORGE - After watching a diamond cut glass, dwarven scientists immediatly started to work on gem weaponry. Their success allows the production of gem weapons, trapparts and even armor.',
    COLOR_CYAN, true)end
	local function announcement_glassforge() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: GLASSFORGE - Copying and improving on the design of smelters and forges, the Glassforge enables the manufacturing glass armor and weapon. The dwarven military  is still highly suspicious of becoming glass cannons, but the theory behind it is sound.',
    COLOR_CYAN, true)end
local function announcement_warbeast() dfhack.gui.showAnnouncement(
    'SCIENCE DISCOVERY: WARBEAST KENNELS - Constructing prototype armor for different bodyshapes, complete with spikes and knives and pictures of cheese on it. Every dwarves dream; and now a possibility. Armor your warbeasts today!',
    COLOR_CYAN, true)end
	
local function announcement_marker() dfhack.gui.showAnnouncement(
    'PANIC!!! A Marker has been uncovered by the Archeologist and all corpses mutate into monsters. Destroy the Marker with the help of Armok',
    COLOR_CYAN, true)end
local function announcement_markerstop() dfhack.gui.showAnnouncement(
    'SUCCESS! The necromorph threat is over',
    COLOR_CYAN, true)end
	

-- for each announcement you need another line here.
if EventType=="golemold" then announcement_golem()
	elseif EventType=="loot" then announcement_loot()
	elseif EventType=="lich" then announcement_lich()
	elseif EventType=="greatweapon" then announcement_greatweapon()
	elseif EventType=="artificer" then announcement_artificer()
	elseif EventType=="machine" then announcement_machine()
	elseif EventType=="siren" then announcement_siren()
	elseif EventType=="steamengine" then announcement_steamengine()
	elseif EventType=="magma" then announcement_magma()
	elseif EventType=="greatforge" then announcement_greatforge()
	elseif EventType=="ammocaster" then announcement_ammocaster()
	elseif EventType=="gunsmith" then announcement_gunsmith()
	elseif EventType=="workbench" then announcement_workbench()
	elseif EventType=="siegeworks" then announcement_siegeworks()
	elseif EventType=="armory" then announcement_armory()
	elseif EventType=="weaponry" then announcement_weaponry()
	elseif EventType=="runearmory" then announcement_runearmory()
	elseif EventType=="runeweaponry" then announcement_runeweaponry()
	elseif EventType=="townportal" then announcement_townportal()
	elseif EventType=="golemforge" then announcement_golemforge()
	elseif EventType=="volcanicfoundry" then announcement_volcanicfoundry()
	elseif EventType=="wizardschool" then announcement_wizardschool()
	elseif EventType=="altarwhite" then announcement_altarwhite()
	elseif EventType=="altarblack" then announcement_altarblack()
	elseif EventType=="altarelemental" then announcement_altarelemental()
	elseif EventType=="warpstone" then announcement_warpstone()
	elseif EventType=="vampire" then announcement_vampire()
	elseif EventType=="demon" then announcement_demon()
	elseif EventType=="werebeast" then announcement_werebeast()
	elseif EventType=="megabeast" then announcement_megabeast()
	elseif EventType=="necromancy" then announcement_necromancy()
	elseif EventType=="trebuchet" then announcement_trebuchet()
	elseif EventType=="arbalest" then announcement_arbalest()
	elseif EventType=="gas" then announcement_gas()
	elseif EventType=="tarpitch" then announcement_tarpitch()
	elseif EventType=="finishingforge" then announcement_finishingforge()
	elseif EventType=="blastfurnace" then announcement_blastfurnace()
	elseif EventType=="coinmint" then announcement_coinmint()
	elseif EventType=="printingpress" then announcement_printingpress()
	elseif EventType=="crucible" then announcement_crucible()
	elseif EventType=="metallurgist" then announcement_metallurgist()
	elseif EventType=="alchemy" then announcement_alchemy()
	elseif EventType=="chemistry" then announcement_chemistry()
	elseif EventType=="biology" then announcement_biology()
	elseif EventType=="botanical" then announcement_botanical()
	elseif EventType=="toxic" then announcement_toxic()
	elseif EventType=="warbeast" then announcement_warbeast()
	elseif EventType=="engineer" then announcement_engineer()
	elseif EventType=="rockforge" then announcement_rockforge()
	elseif EventType=="gemforge" then announcement_gemforge()
	elseif EventType=="glassforge" then announcement_glassforge()
	elseif EventType=="fletcher" then announcement_fletcher()
	elseif EventType=="book" then announcement_book()
elseif EventType=="spelltome" then announcement_spelltome()
elseif EventType=="catalystrod" then announcement_catalystrod()
elseif EventType=="phylactery" then announcement_phylactery()
elseif EventType=="arcane" then announcement_arcane()
elseif EventType=="firemage1" then announcement_firemage1()
elseif EventType=="firemage2" then announcement_firemage2()
elseif EventType=="firemage3" then announcement_firemage3()
elseif EventType=="airmage1" then announcement_airmage1()
elseif EventType=="airmage2" then announcement_airmage2()
elseif EventType=="airmage3" then announcement_airmage3()
elseif EventType=="watermage1" then announcement_watermage1()
elseif EventType=="watermage2" then announcement_watermage2()
elseif EventType=="watermage3" then announcement_watermage3()
elseif EventType=="earthmage1" then announcement_earthmage1()
elseif EventType=="earthmage2" then announcement_earthmage2()
elseif EventType=="earthmage3" then announcement_earthmage3()
elseif EventType=="blackmagev" then announcement_blackmagev()
elseif EventType=="blackmaged" then announcement_blackmaged()
elseif EventType=="blackmagel" then announcement_blackmagel()
elseif EventType=="whitemageh" then announcement_whitemageh()
elseif EventType=="whitemagep" then announcement_whitemagep()
elseif EventType=="whitemagew" then announcement_whitemagew()
elseif EventType=="pipeweed" then announcement_pipeweed()
elseif EventType=="bagofloot" then announcement_bagofloot()
elseif EventType=="musket" then announcement_musket()
elseif EventType=="pistol" then announcement_pistol()
elseif EventType=="cannon" then announcement_cannon()
elseif EventType=="largegem" then announcement_largegem()
elseif EventType=="lobotomy" then announcement_lobotomy()
elseif EventType=="batchcoke" then announcement_batchcoke()
elseif EventType=="batchmetalalu" then announcement_batchmetalalu()
elseif EventType=="batchmetalbra" then announcement_batchmetalbra()
elseif EventType=="batchmetalbro" then announcement_batchmetalbro()
elseif EventType=="batchmetalcop" then announcement_batchmetalcop()
elseif EventType=="batchmetaliro" then announcement_batchmetaliro()
elseif EventType=="batchmetalgol" then announcement_batchmetalgol()
elseif EventType=="batchmetalpig" then announcement_batchmetalpig()
elseif EventType=="batchmetalpla" then announcement_batchmetalpla()
elseif EventType=="batchmetalsil" then announcement_batchmetalsil()
elseif EventType=="batchmetalste" then announcement_batchmetalste()
elseif EventType=="batchmetalspr" then announcement_batchmetalspr()
elseif EventType=="batchmetaltin" then announcement_batchmetaltin()
elseif EventType=="batchmetalzin" then announcement_batchmetalzin()
elseif EventType=="bread" then announcement_bread()
elseif EventType=="candy" then announcement_candy()
elseif EventType=="cavecheese" then announcement_cavecheese()
elseif EventType=="adawafer" then announcement_adawafer()
elseif EventType=="fire" then announcement_fire()
elseif EventType=="armok1" then announcement_armok1()
elseif EventType=="armok2" then announcement_armok2()
elseif EventType=="armok3" then announcement_armok3()
elseif EventType=="armok4" then announcement_armok4()
elseif EventType=="armok5" then announcement_armok5()
elseif EventType=="armok6" then announcement_armok6()
elseif EventType=="armokdeath" then announcement_armokdeath()
elseif EventType=="golemheart" then announcement_golemheart()
elseif EventType=="guardian" then announcement_guardian()
elseif EventType=="weaponupgrade" then announcement_weaponupgrade()
elseif EventType=="armorupgrade" then announcement_armorupgrade()
elseif EventType=="shieldupgrade" then announcement_shieldupgrade()
elseif EventType=="plateset" then announcement_plateset()
elseif EventType=="ink" then announcement_ink()
elseif EventType=="trapupgrade" then announcement_trapupgrade()
elseif EventType=="bedroomset" then announcement_bedroomset()
elseif EventType=="noblebedroomset" then announcement_noblebedroomset()
elseif EventType=="officeroomset" then announcement_officeroomset()
elseif EventType=="graveroomset" then announcement_graveroomset()
elseif EventType=="diningroomset" then announcement_diningroomset()
elseif EventType=="storageroomset" then announcement_storageroomset()
elseif EventType=="crateopen" then announcement_crateopen()
elseif EventType=="blackpowder" then announcement_blackpowder()
elseif EventType=="megabeastbutcher" then announcement_megabeastbutcher()
elseif EventType=="archeologist" then announcement_archeologist()
elseif EventType=="bardgear" then announcement_bardgear()
elseif EventType=="diplomatgear" then announcement_diplomatgear()
elseif EventType=="caravangear" then announcement_caravangear()
elseif EventType=="spygear" then announcement_spygear()
elseif EventType=="expeditiongear" then announcement_expeditiongear()
elseif EventType=="mercenarygear" then announcement_mercenarygear()
elseif EventType=="expedloot" then announcement_expedloot()
elseif EventType=="expedcaravan" then announcement_expedcaravan()
elseif EventType=="runeweapon" then announcement_runeweapon()
elseif EventType=="runeammo" then announcement_runeammo()
elseif EventType=="runetrap" then announcement_runetrap()
elseif EventType=="poisonweapon" then announcement_poisonweapon()
elseif EventType=="poisontrap" then announcement_poisontrap()
elseif EventType=="poisonammo" then announcement_poisonammo()
elseif EventType=="blueprint" then announcement_blueprint()
elseif EventType=="clothset" then announcement_clothset()
elseif EventType=="leatherarmorset" then announcement_leatherarmorset()
elseif EventType=="golemactivate" then announcement_golemactivate()
elseif EventType=="golemspeed" then announcement_golemspeed()
elseif EventType=="golemstrength" then announcement_golemstrength()
elseif EventType=="golemaoe" then announcement_golemaoe()
elseif EventType=="golem" then announcement_golem()
elseif EventType=="runebeast" then announcement_runebeast()
elseif EventType=="beastarmor" then announcement_beastarmor()
elseif EventType=="megabeastreanimate" then announcement_megabeastreanimate()
elseif EventType=="landmine" then announcement_landmine()
elseif EventType=="turret" then announcement_turret()
elseif EventType=="arenafight" then announcement_arenafight()
elseif EventType=="runearmor" then announcement_runearmor()
elseif EventType=="caravanforce" then announcement_caravanforce()
elseif EventType=="migrantforce" then announcement_migrantforce()
elseif EventType=="diplomatforce" then announcement_diplomatforce()
elseif EventType=="beastforce" then announcement_beastforce()
elseif EventType=="siegeforce" then announcement_siegeforce()
elseif EventType=="guild" then announcement_guild()
elseif EventType=="military" then announcement_military()
elseif EventType=="exitguild" then announcement_exitguild()
elseif EventType=="exitmilitary" then announcement_exitmilitary()
elseif EventType=="prison" then announcement_prison()
elseif EventType=="smokepipe" then announcement_smokepipe()
elseif EventType=="music" then announcement_music()
elseif EventType=="mutation" then announcement_mutation()
elseif EventType=="printingpressrunning" then announcement_printingpressrunning()
elseif EventType=="speech" then announcement_speech()
elseif EventType=="twohandedweapon" then announcement_twohandedweapon()
elseif EventType=="kiteshield" then announcement_kiteshield()
elseif EventType=="armorset" then announcement_armorset()
elseif EventType=="legendary" then announcement_legendary()
elseif EventType=="apostle" then announcement_apostle()
elseif EventType=="vampiretrans" then announcement_vampiretrans()
elseif EventType=="devampiretrans" then announcement_devampiretrans()
elseif EventType=="werebeasttrans" then announcement_werebeasttrans()
elseif EventType=="undeadtrans" then announcement_undeadtrans()
elseif EventType=="arbalesttrans" then announcement_arbalesttrans()
elseif EventType=="trebuchettrans" then announcement_trebuchettrans()
elseif EventType=="markerstop" then announcement_markerstop()
elseif EventType=="marker" then announcement_marker()
end

-- This script has been made possible by code stolen from Siren.lua, ForceEvent.lua (Putnam) and brute-force coding by me, Meph. Dont laugh, and if anyone knows how to add a units name ("Dwarf1 has created a two-handed weapon") let me know.
