-- Checks regularly if creature has an item equipped with a special syndrome and applies item's syndrome if it is. Use "disable" (minus quotes) to disable and "help" to get help.
 
local function getDelayTicks(args)
    for k,v in ipairs(args) do
        if tonumber(v) and tonumber(v) > 0 then
            return tonumber(v)
        end
    end
    return nil
end
 
local args = {...}
 
local function printItemSyndromeHelp()
    print("Arguments:")
    print('    "help": displays this dialogue.')
    print(" ")
    print('    "enable": enables itemsyndrome.')
    print(" ")
    print('    "disable": disables the script.')
    print(" ")
    print('    "force": forces an item search.')
    print(" ")
    print('    "debugon/debugoff": debug mode.')
    print(" ")
    print('    "debug2on/debug2off": super-debug')
    print('    mode, just in case it\'s crashing.')
    print('    "contaminantson/contaminantsoff": toggles searching for contaminants.')
    print('    Disabling speeds itemsyndrome up greatly.')
    print(' ')
    print("    Any number: will set the amount of time between itemsyndrome's inventory")
    print('    sweeps. Lower means more accurate, but slower; higher means the opposite.')
end
 
function processArgs(args)
    if #args==0 then enable = true return end
    for k,v in ipairs(args) do
        v=string.lower(v)
        if v == "help" then printItemSyndromeHelp() return end
        if v == "enable" then enable = true end
        if v == "disable" then disable = true end
        if v == "force" then force = true end
        if v == "debugon" then itemsyndromedebug = true end
        if v == "debugoff" then itemsyndromedebug = false end
        if v == "debug2on" then itemsyndromedebug2 = true end
        if v == "debug2off" then itemsyndromedebug2 = false end
        if v == "contaminantson" then itemsyndromecontaminants = true end
        if v == "contaminantsoff" then itemsyndromecontaminants = false end
    end
end
 
delayTicks = getDelayTicks(args) or delayTicks or 499
 
processArgs(args)
 
local function getMaterial(item)
    local material = dfhack.matinfo.decode(item) or false
    if material.mode ~= "inorganic" or not material then 
        return false
    else
        return material.material --the "material" thing up there contains a bit more info which is all pretty important, like the creature that the material comes from
    end
end
 
local function getSyndrome(material)
    if not material then return false end
    if #material.syndrome>0 then return material.syndrome[0]
    else return nil end
end
 
local function syndromeIsDfHackSyndrome(syndrome)
    for k,v in ipairs(syndrome.syn_class) do
        if v.value=="DFHACK_ITEM_SYNDROME" then return true end
    end
    return false
end
 
local function itemHasNoSubtype(item)
   local subtypedItemTypes =
    {
    df.item_armorst,
    df.item_weaponst,
    df.item_helmst,
    df.item_shoesst,
    df.item_shieldst,
    df.item_glovest,
    df.item_pantsst,
    df.item_toolst,
    df.item_siegeammost,
    df.item_ammost,
    df.item_trapcompst,
    df.item_instrumentst,
    df.item_toyst}
    for _,v in ipairs(subtypedItemTypes) do
        if v:is_instance(item) then return false end
    end
    return true
end
 
local function itemHasSyndrome(item)
    if itemHasNoSubtype(item) or not itemSyndromeMats then return nil end
    for _,material in ipairs(itemSyndromeMats) do
		if itemsyndromedebug then print(material,material.id) end
        for __,syndrome in ipairs(material.syndrome) do
			if itemsyndromedebug then print(syndrome.syn_name) end
            if syndrome.syn_name == item.subtype.name then return syndrome end
        end
    end
    return nil
end
 
local function alreadyHasSyndrome(unit,syn_id)
    for _,syndrome in ipairs(unit.syndromes.active) do
        if syndrome.type == syn_id then return true end
    end
    return false
end
 
local function assignSyndrome(target,syn_id) --taken straight from here, but edited so I can understand it better: https://gist.github.com/warmist/4061959/. Also implemented expwnent's changes for compatibility with syndromeTrigger.
    if target==nil then
        return nil
    end
    if alreadyHasSyndrome(target,syn_id) then
        local syndrome
        for k,v in ipairs(target.syndromes.active) do
            if v.type == syn_id then syndrome = v end
        end
        if not syndrome then return nil end
        syndrome.ticks=1
        return true
    end
    local newSyndrome=df.unit_syndrome:new()
    local target_syndrome=df.syndrome.find(syn_id)
    newSyndrome.type=target_syndrome.id
    newSyndrome.year=df.global.cur_year
    newSyndrome.year_time=df.global.cur_year_tick
    newSyndrome.ticks=1
    newSyndrome.unk1=1
    for k,v in ipairs(target_syndrome.ce) do
        local sympt=df.unit_syndrome.T_symptoms:new()
        sympt.ticks=1
        sympt.flags=2
        newSyndrome.symptoms:insert("#",sympt)
    end
    target.syndromes.active:insert("#",newSyndrome)
    if itemsyndromedebug then
        print("Assigned syndrome #" ..syn_id.." to unit.")
    end
    return true
end
 
local function syndromeIsIndiscriminate(syndrome)
    if #syndrome.syn_affected_class==0 and #syndrome.syn_affected_creature==0 and #syndrome.syn_affected_caste==0 and #syndrome.syn_immune_class==0 and #syndrome.syn_immune_creature==0 and #syndrome.syn_immune_caste==0 then return true end
    return false
end
 
local function creatureIsAffected(unit,syndrome)
    if syndromeIsIndiscriminate(syndrome) then return true end
    local affected = false
    local unitraws = df.creature_raw.find(unit.race)
    local casteraws = unitraws.caste[unit.caste]
    local unitracename = unitraws.creature_id
    local castename = casteraws.caste_id
    local unitclasses = casteraws.creature_class
    for _,unitclass in ipairs(unitclasses) do
        for _,syndromeclass in ipairs(syndrome.syn_affected_class) do
            if unitclass.value==syndromeclass.value then affected = true end
        end
    end
    for caste,creature in ipairs(syndrome.syn_affected_creature) do
        local affected_creature = creature.value
        local affected_caste = syndrome.syn_affected_caste[caste].value --slightly evil, but oh well, hehe.
        if affected_creature == unitracename and affected_caste == castename then affected = true end
    end
    for _,unitclass in ipairs(unitclasses) do
        for _,syndromeclass in ipairs(syndrome.syn_immune_class) do
            if unitclass.value==syndromeclass.value then affected = false end
        end
    end
    for caste,creature in ipairs(syndrome.syn_immune_creature) do
        local immune_creature = creature.value
        local immune_caste = syndrome.syn_immune_caste[caste].value
        if immune_creature == unitracename and immune_caste == castename then affected = false end
    end
    if itemsyndromedebug then
        if not affected then print ("Creature is not affected.") else print("Creature is affected") end
    end
    return affected
end
 
local function itemAffectsHauler(syndrome)
    for k,v in ipairs(syndrome.syn_class) do
        if v.value=="DFHACK_AFFECTS_HAULER" then return true end
    end
    return false
end
 
local function itemAffectsStuckins(syndrome)
    for k,v in ipairs(syndrome.syn_class) do
        if v.value=="DFHACK_AFFECTS_STUCKIN" then return true end
    end
    return false
end
 
local function itemIsArmorOnly(syndrome)
    for k,v in ipairs(syndrome.syn_class) do
        if v.value=="DFHACK_ARMOR_ONLY" then return true end
    end
    return false
end
    
local function itemIsWieldedOnly(syndrome)
    for k,v in ipairs(syndrome.syn_class) do
        if v.value=="DFHACK_WIELDED_ONLY" then return true end
    end
    return false
end
    
local function itemOnlyAffectsStuckins(syndrome)
    for k,v in ipairs(syndrome.syn_class) do
        if v.value=="DFHACK_STUCKINS_ONLY" then return true end
    end
    return false
end
        
local function itemIsInValidPosition(item_inv, syndrome)
    if (item_inv.mode == 0 and not itemAffectsHauler(syndrome)) or (item_inv.mode == 7 and not itemAffectsStuckins(syndrome)) or (item_inv.mode ~= 2 and itemIsArmorOnly(syndrome)) or (item_inv.mode ~=1 and itemIsWieldedOnly(syndrome)) or (item_inv.mode ~=7 and itemOnlyAffectsStuckins(syndrome)) then return false end
    return true
end
 
local function syndromeIsTransformation(syndrome)
    for _,effect in ipairs(syndrome.ce) do
        local effectType = tostring(effect)
        if string.find(effectType,"body_transformation") then return true end
    end
    return false
end
 
local function rememberInventory(unit)
    local invCopy = {}
    for inv_id,item_inv in ipairs(unit.inventory) do
        invCopy[inv_id+1] = {}
        local itemToWorkOn = invCopy[inv_id+1]
        itemToWorkOn.item = item_inv.item
        itemToWorkOn.mode = item_inv.mode
        itemToWorkOn.body_part_id = item_inv.body_part_id
    end
    return invCopy
end
 
local function moveAllToInventory(unit,invTable)
    for _,item_inv in ipairs(invTable) do
        dfhack.items.moveToInventory(item_inv.item,unit,item_inv.mode,item_inv.body_part_id)
    end
end
 
local function applySyndromesBasedOnItems(unit)
    if itemsyndromedebug then print("Checking " .. #unit.inventory .. " items on unit named " .. dfhack.TranslateName(dfhack.units.getVisibleName(unit))) end
    local transformation = false
    for itemid,item_inv in ipairs(unit.inventory) do
        local item = item_inv.item
        if itemsyndromedebug2 then print("checking item #" .. itemid+1 .." on " .. unit.name.first_name) end
        if getSyndrome(getMaterial(item)) then
            if itemsyndromedebug then print("item has a syndrome, checking if item is valid for application...") end
            local syndrome = getSyndrome(getMaterial(item))
            local syndromeApplied
            if syndromeIsTransformation(syndrome) then
                unitInventory = rememberInventory(unit)
                transformation = true
            end
            if syndromeIsDfHackSyndrome(syndrome) and creatureIsAffected(unit,syndrome) and itemIsInValidPosition(item_inv, syndrome) then
                assignSyndrome(unit,syndrome.id)
                syndromeApplied = true
            end
        end
        if itemHasSyndrome(item) then
            if itemsyndromedebug then print("Item itself has a syndrome, checking if item is in correct position and creature is affected") end
            local syndrome = itemHasSyndrome(item)
            if syndromeIsTransformation(syndrome) then
                unitInventory = rememberInventory(unit)
                transformation = true
            end
            if item_inv.mode~=0 and item_inv.mode~=7 and creatureIsAffected(unit,syndrome) then assignSyndrome(unit,syndrome.id) end --the mode thing is to avoid stuckins from doing
        end
        if itemsyndromecontaminants and item.contaminants then
            if itemsyndromedebug then print("Item has contaminants. Checking for syndromes...") end
            for _,contaminant in ipairs(item.contaminants) do
                if itemsyndromedebug2 then print("Checking contaminant #" .. _ .. " on item #" .. itemid .. " on " .. unit.name.first_name) end
                if getSyndrome(getMaterial(contaminant)) then
                    local syndrome = getSyndrome(getMaterial(contaminant))
                    if syndromeIsTransformation(syndrome) then
                        unitInventory = rememberInventory(unit)
                        transformation =true
                    end
                    if syndromeIsDfHackSyndrome(syndrome) and creatureIsAffected(unit,syndrome) and itemIsInValidPosition(item_inv, syndrome) then assignSyndrome(unit,syndrome.id) end
                end
            end
        end
    end
    unitIsChecked[unit.id]=false
    if transformation then dfhack.timeout(2,"ticks",function() moveAllToInventory(unit,unitInventory) end) end
end
 
local function getAllValidUnits()
    local checkedUnits = {}
    for k,unit in ipairs(df.global.world.units.active) do
        if dfhack.units.isAlive(unit) and #unit.inventory>0 then
            table.insert(checkedUnits,unit)
        end
    end
    return checkedUnits
end
 
 
local function findItems()
    local unitDelay=1
    local allUnitsToCheck=getAllValidUnits()
    if itemsyndromedebug  then print("Number of units being checked is " .. #allUnitsToCheck) end
    local numberOfUnitsToWorkOnAtOnce = math.ceil((#allUnitsToCheck*2)/delayTicks)
    if itemsyndromedebug then print("Number of units being checked at once is " .. numberOfUnitsToWorkOnAtOnce) end
    if numberOfUnitsToWorkOnAtOnce~=0 then
        local _uid=1
        repeat
            for i=1,numberOfUnitsToWorkOnAtOnce do
                local unit=allUnitsToCheck[_uid]
                if itemsyndromedebug2 then print("_uid being checked this tick is " .. _uid-1) end
                if _uid<=#allUnitsToCheck and not unitIsChecked[unit.id] then 
                    unitIsChecked[unit.id]=true
                    dfhack.timeout(unitDelay,"ticks",function()
                        applySyndromesBasedOnItems(unit)
                    end)
                end
                _uid=_uid+1
            end
            unitDelay=unitDelay+1
            if itemsyndromedebug then print("_uid this step is " .. _uid) end
        until _uid>#allUnitsToCheck
    end
end
 
local function findItemSyndromeInorganic()
    local allInorganics = {}
    for matID,material in ipairs(df.global.world.raws.inorganics) do
        if string.find(material.id,"DFHACK_ITEMSYNDROME_MATERIAL_") then table.insert(allInorganics,matID) end --the last underscore is needed to prevent duped raws; I want good modder courtesy if it kills me, dammit!
    end
    if itemsyndromedebug then printall(allInorganics) end
    if #allInorganics>0 then return allInorganics else return nil end
end
 
local function getAllItemSyndromeMats(itemSyndromeMatIDs)
    local allActualInorganics = {}
    for _,itemSyndromeMatID in ipairs(itemSyndromeMatIDs) do
        table.insert(allActualInorganics,df.global.world.raws.inorganics[itemSyndromeMatID].material)
    end
    if itemsyndromedebug then printall(allActualInorganics) end
    return allActualInorganics
end
 
 
dfhack.onStateChange.itemsyndrome = function(code) --Many thanks to Warmist for pointing this out to me!
    if code==SC_WORLD_LOADED or code==SC_MAP_LOADED then --this breaks stuff but the new timeout stuff fixes it
        if itemsyndromedebug then print("World loaded, itemsyndrome running...") end
        itemsyndrome_timeout = dfhack.timeout(1,'ticks',itemsyndrome)
		itemSyndromeMatIDs = findItemSyndromeInorganic()
		unitIsChecked={}
		if itemSyndromeMatIDs then itemSyndromeMats = getAllItemSyndromeMats(itemSyndromeMatIDs) end 
    end
end
 
function itemsyndrome()
    if itemsyndromedebug then print("Beginning cycle.") end
    findItems()
    dfhack.timeout_active(itemsyndrome_timeout,nil)
    itemsyndrome_timeout = dfhack.timeout(delayTicks,'ticks',itemsyndrome)
end
 
if enable then
    print("Enabled itemsyndrome.")
    enable = false
end
 
if force then 
    if itemsyndromedebug then print(#df.global.world.units.active) end
    findItems()
    force = false
end
 
if disable then 
    itemsyndrome = nil
    dfhack.onStateChange.itemsyndrome = nil
    print("Disabled itemsyndrome.")
    disable = false
end
 
if itemsyndromedebug then print(delayTicks) end