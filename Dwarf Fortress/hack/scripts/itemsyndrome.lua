-- Checks for inventory changes and applies or removes syndromes that items or their materials have. Use "disable" (minus quotes) to disable and "help" to get help.
 
local args = {...}
 
local function printItemSyndromeHelp()
    print("Arguments (non case-sensitive):")
    print('    "help": displays this dialogue.')
    print(" ")
    print('    "disable": disables the script.')
    print(" ")
    print('    "debugon/debugoff": debug mode.')
    print(" ")
    print('    "contaminantson/contaminantsoff": toggles searching for contaminants.')
    print('    Disabling speeds itemsyndrome up greatly.')
    print('    "transformReEquipOn/TransformReEquipOff": toggles transformation auto-reequip.')
end
 
itemsyndromedebug=false
 
function processArgs(args)
    for k,v in ipairs(args) do
        v=v:lower()
        if v == "help" then printItemSyndromeHelp() return end
        if v == "debugon" then itemsyndromedebug = true end
        if v == "debugoff" then itemsyndromedebug = false end
        if v == "contaminantson" then itemsyndromecontaminants = true end
        if v == "contaminantsoff" then itemsyndromecontaminants = false end
        if v == "transformreequipon" then transformationReEquip = true end
        if v == "transformreequipoff" then transformationReEquip = false end
    end
end
 
processArgs(args)
 
local function getMaterial(item)
    local material = dfhack.matinfo.decode(item) and dfhack.matinfo.decode(item) or false
    if not material then return nil end
    if material.mode ~= "inorganic"  then
        return nil
    else
        return material.material --the "material" thing up there contains a bit more info which is all pretty important but impertinent, like the creature that the material comes from
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
    local allItemSyndromes={}
    for _,material in ipairs(itemSyndromeMats) do
        for __,syndrome in ipairs(material.syndrome) do
            if syndrome.syn_name == item.subtype.name then table.insert(allItemSyndromes,syndrome) end
        end
    end
    return #allItemSyndromes>0 and allItemSyndromes or false
end
 
local function alreadyHasSyndrome(unit,syn_id)
    for _,syndrome in ipairs(unit.syndromes.active) do
        if syndrome.type == syn_id then return true end
    end
    return false
end
 
local function eraseSyndrome(target,syn_id)
    for i=#target.syndromes.active-1,0,-1 do
        if target.syndromes.active[i].type==syn_id then target.syndromes.active:erase(i) end
    end
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
    return (#syndrome.syn_affected_class==0 and #syndrome.syn_affected_creature==0 and #syndrome.syn_affected_caste==0 and #syndrome.syn_immune_class==0 and #syndrome.syn_immune_creature==0 and #syndrome.syn_immune_caste==0)
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
 
local function getValidPositions(syndrome)
    local returnTable={AffectsHauler=false,AffectsStuckins=false,IsArmorOnly=false,IsWieldedOnly=false,OnlyAffectsStuckins=false}
    for k,v in ipairs(syndrome.syn_class) do
        if v.value:find('DFHACK') then
            if v.value=="DFHACK_AFFECTS_HAULER" then returnTable.AffectsHauler=true end
            if v.value=="DFHACK_AFFECTS_STUCKIN" then returnTable.AffectsStuckins=true end
            if v.value=="DFHACK_STUCKINS_ONLY" then returnTable.OnlyAffectsStuckins=true end    
            if v.value=="DFHACK_WIELDED_ONLY" then returnTable.IsWieldedOnly=true end
            if v.value=="DFHACK_ARMOR_ONLY" then returnTable.IsArmorOnly=true end
        end
    end
    return returnTable
end
       
local function itemIsInValidPosition(item_inv, syndrome)
    local item = getValidPositions(syndrome)
    if not item_inv then return false end
    return not ((item_inv.mode == 0 and not item.AffectsHauler) or (item_inv.mode == 7 and not item.AffectsStuckins) or (item_inv.mode ~= 2 and item.IsArmorOnly) or (item_inv.mode ~=1 and item.IsWieldedOnly) or (item_inv.mode ~=7 and item.OnlyAffectsStuckins))
end
 
local function syndromeIsTransformation(syndrome)
    for _,effect in ipairs(syndrome.ce) do
        if df.creature_interaction_effect_body_transformationst:is_instance(effect) then return true end
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
 
local function addOrRemoveSyndromeDepending(unit,item_inv,syndrome)
    if syndromeIsDfHackSyndrome(syndrome) and creatureIsAffected(unit,syndrome) and itemIsInValidPosition(item_inv, syndrome) then
        if item_inv then
            assignSyndrome(unit,syndrome.id)
        else
            eraseSyndrome(unit,syndrome.id)
        end
    end
end
 
eventful=require('plugins.eventful')
 
eventful.enableEvent(eventful.eventType.INVENTORY_CHANGE,5)
 
local function checkAndAddSyndrome(unit_id,new_equip,item_id)
    local item = df.item.find(item_id)
    if not item then return false end
    local unit = df.unit.find(unit_id)
    if unit.flags1.dead then return false end
    if itemsyndromedebug then print("Checking unit #" .. unit_id) end
    local transformation = false
    if itemsyndromedebug then print("checking item #" .. item_id .." on unit #" .. unit_id) end
    local itemMaterial=getMaterial(item)
    if itemMaterial then
        for k,syndrome in ipairs(itemMaterial.syndrome) do
            if itemsyndromedebug then print("item has a syndrome, checking if item is valid for application...") end
            if syndromeIsTransformation(syndrome) then
                unitInventory = rememberInventory(unit)
                transformation = true
            end
            addOrRemoveSyndromeDepending(unit,new_equip,syndrome)
        end
    end
    local itemSyndromes = itemHasSyndrome(item)
    if itemSyndromes then
        if itemsyndromedebug then print("Item itself has a syndrome, checking if item is in correct position and creature is affected") end
        for k,syndrome in ipairs(itemSyndromes) do
            if syndromeIsTransformation(syndrome) then
                unitInventory = rememberInventory(unit)
                transformation = true
            end
            addOrRemoveSyndromeDepending(unit,new_equip,syndrome)
        end
    end
    if itemsyndromecontaminants and item.contaminants then
        if itemsyndromedebug then print("Item has contaminants. Checking for syndromes...") end
        for _,contaminant in ipairs(item.contaminants) do
            local contaminantMaterial=getMaterial(contaminant)
            if contaminantMaterial then
                for k,syndrome in ipairs(contaminantMaterial.syndrome) do
                    if itemsyndromedebug then print("Checking syndrome #" .. k .. "on contaminant #" .. _ .. " on item #" .. item_id .. " on unit #" .. unit_id ..".") end
                    if syndromeIsTransformation(syndrome) then
                        unitInventory = rememberInventory(unit)
                        transformation =true
                    end
                    addOrRemoveSyndromeDepending(unit,new_equip,syndrome)
                end
            end
        end
    end
    if transformation and transformationReEquip then dfhack.timeout(2,"ticks",function() moveAllToInventory(unit,unitInventory) end) end
end
 
eventful.onInventoryChange.itemsyndrome=function(unit_id,item_id,old_equip,new_equip)
    checkAndAddSyndrome(unit_id,new_equip,item_id)
end
 
dfhack.onStateChange.itemsyndrome=function(code)
    if code==SC_WORLD_LOADED then
        itemSyndromeMatIDs = findItemSyndromeInorganic()
        if itemSyndromeMatIDs then itemSyndromeMats = getAllItemSyndromeMats(itemSyndromeMatIDs) end
    end
end
 
if disable then
    eventful.onInventoryChange.itemsyndrome=nil
    print("Disabled itemsyndrome.")
    disable = false
else
    print("Enabled itemsyndrome.")
end