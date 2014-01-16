-- Allows for script-based wishing.
 
-- Strings be violated here. Be warned.
 
function cleanString(str)
    if not str then return '' end
    return str:lower():gsub('%W','')
end
 
function desperatelyAttemptToMatchStrings(str1,str2)
    if not str1 or not str2 then return false end
    return cleanString(str1):find(cleanString(str2)) or cleanString(str2):find(cleanString(str1))
end
 
function findPlant(str,desparate)
    --ugly ugly function, but this whole script is ugly
    if desparate then
        for k,v in ipairs(df.global.world.raws.plants.all) do
            if desperatelyAttemptToMatchStrings(str,v.id) or desperatelyAttemptToMatchStrings(str,v.name) then return {v,'plant'} end
        end
    else
        for k,v in ipairs(df.global.world.raws.plants.all) do
            if cleanString(str)==cleanString(v.id) or cleanString(v.name)==cleanString(str) then return {v,'plant'} end
        end
    end
    return {false,nil}
end
 
function findCreature(str,desparate)
    if desparate then
        for k,v in ipairs(df.global.world.raws.creatures.all) do
            if desperatelyAttemptToMatchStrings(str,v.creature_id) or desperatelyAttemptToMatchStrings(str,v.name[0]) then return {v,'creature'} end
        end
    else
        for k,v in ipairs(df.global.world.raws.creatures.all) do
            if cleanString(str)==cleanString(v.creature_id) then return {v,'creature'} end --the name equality is handled by the binsearch
        end
    end
    return {false,nil}
end
 
function findMaterialGivenPlainLanguageString(str)
    --not going to include odd substances :I
    local str=str:gsub("'",''):gsub('"','')
    local tokenStr=string.upper(str:gsub(' ','_'))
    local moddedString=tokenStr:gsub('_',':')
    for i=1,2 do
        if i==1 then
            moddedString='CREATURE:'..moddedString
        else
            moddedString='PLANT:'..moddedString
        end
        local find=dfhack.matinfo.find(tokenStr) or dfhack.matinfo.find(moddedString)
        if find then return find.type,find.index end
    end
    str=string.lower(str:gsub('_',' ')) --making sure it's all nice for the ugly part
    --this is the ugly part
    local utils=require('utils')
    local foundMatchingObject={}
    for word in str:gmatch('%a+') do
        --first, we search for an object, starting with a binsearch followed by a plant search followed by a creature search using a different creature identifier followed by a couple of nasty desparate searches.
        local binsearchResult={utils.binsearch(df.global.world.raws.creatures.alphabetic,string.lower(word),'name',utils.compare_field_key(0))}
        foundMatchingObject=foundMatchingObject[1]==true and foundMatchingObject or binsearchResult[2]==true and binsearchResult or findPlant(word) or findCreature(word) or findPlant(word,true) or findCreature(word,true)
        if foundMatchingObject[1]==true then
            for k,v in ipairs(foundMatchingObject[1].material) do --then we desperately try to find a material that matches the object.
                if desperatelyAttemptToMatchStrings(word,v.id) or desperatelyAttemptToMatchStrings(word,v.state_name.Liquid) or desperatelyAttemptToMatchStrings(word,v.state_name.Solid) or 
                desperatelyAttemptToMatchStrings(str,v.id) or desperatelyAttemptToMatchStrings(str,v.state_name.Liquid) or desperatelyAttemptToMatchStrings(str,v.state_name.Solid) then
                    if v.heat.melting_point~=60001 then
                        if foundMatchingObject[2]==true or foundMatchingObject[2]=='creature' then
                            local find = dfhack.matinfo.find('CREATURE:'..foundMatchingObject[1].creature_id..':'..v.id) --splitting the string doesn't work right now
                            return find.type,find.index
                        elseif foundMatchingObject[2]=='plant' then
                            local find = dfhack.matinfo.find('PLANT:'..foundMatchingObject[1].id..':'..v.id)
                            return find.type,find.index
                        else
                        end
                    end
                end
            end
        end
    end
    return false,false
end
 
function pluralize(str)
    local notToPlural={ --haha, imagine a natural language parser that figures out pluralization by context
        armor=true,
        ammo=true,
        siege_ammo=true,
        gloves=true,
        shoes=true,
        pants=true
    }
    if v[str] then return str end
    return str..'s'
end
 
function createItem(mat,itemType,pos)
    local item=df['item_'..df.item_type[itemType[1]]:lower()..'st']:new() --incredible
    item.id=df.global.item_next_id
    df.global.world.items.all:insert('#',item)
    df.global.item_next_id=df.global.item_next_id+1
    if itemType[2]~=-1 then
        item:setSubtype(itemType[2])
    end
    item:setMaterial(mat[1])
    item:setMaterialIndex(mat[2])
    item:categorize(true)
    item.flags.removed=true
    dfhack.items.moveToGround(item,{x=pos.x,y=pos.y,z=pos.z})
end
 
function getType(str)
    return df.item_type[str:sub(10,-16):upper()]
end
 
function findItemGivenPlainLanguageString(str)
    str=str:gsub("'",''):gsub('"','')
    local tokenStr=string.upper(str:gsub(' ','_'))
    if df.item_type[tokenStr] then return df.item_type[tokenStr],dfhack.items.getSubtypeCount(df.item_type[tokenStr]) end
    for k,v in ipairs(df.global.world.raws.itemdefs.all) do
        if v.id==tokenStr or v.name==str then return getType(tostring(v)),v.subtype end
    end
end
 
function itemTypeIsNotSupported(itemtype) --ugly ugly ugly
    return (itemType==df.item_type.EGG or itemtype==df.item_type.CORPSE or itemtype==df.item_type.CORPSEPIECE or itemtype==df.item_type.REMAINS or itemtype==df.item_type.FISH or itemtype==df.item_type.FISH_RAW or itemtype==df.item_type.VERMIN or itemtype==df.item_type.PET)
end
 
function hackWish(posOrUnit)
    local pos = df.unit:is_instance(posOrUnit) and posOrUnit.pos or posOrUnit
    local script=require('gui/script')
    script.start(function()
        local mattype,matindex,itemtype,itemsubtype,tryAgain
        repeat
            tryAgain=false
            local ok,itemString=script.showInputPrompt('demons wish','What item do you ask of me?',COLOR_LIGHTRED)
            if ok then
                itemtype,itemsubtype=findItemGivenPlainLanguageString(itemString)
            end
            if not itemtype or itemTypeIsNotSupported(itemtype) then
                script.showMessage('demons wish','Dont try to fool me!',COLOR_LIGHTRED)
                tryAgain=script.showYesNoPrompt('demons wish','Ask again?',COLOR_RED)
            end
        until not tryAgain or itemtype
        repeat
            if not tryAgain then
            tryAgain=false
			tryAgainMat=false
            local kk,matString=script.showInputPrompt('demons wish','What material should it be?',COLOR_LIGHTRED)
            if kk then
                mattype,matindex=findMaterialGivenPlainLanguageString(matString)
            end
            if not mattype then
                script.showMessage('demons wish','Dont try to fool me!',COLOR_LIGHTRED)
                tryAgainMat=script.showYesNoPrompt('demons wish','Ask again?',COLOR_RED)
            end end --tryAgain also
        until not tryAgainMat or mattype
        if mattype and itemtype then
            createItem({mattype,matindex},{itemtype,itemsubtype},pos)
        end
    end)
end
 
args={...}
 
posOrUnit=#args<1 and dfhack.gui.getSelectedUnit(true) and dfhack.gui.getSelectedUnit(true) or #args<1 and df.global.cursor or #args<3 and df.unit.find(args[1]) or {x=args[1],y=args[2],z=args[3]}
 
hackWish(posOrUnit)