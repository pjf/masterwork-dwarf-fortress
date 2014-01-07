-- Allows for reaction-based wishing.
 
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
 
function getItemType(itemdef)
	return df.item_type[tostring(itemdef._type):sub(16,-4):upper()]
end
 
function findItemGivenPlainLanguageString(str)
    local str=str:gsub("'",''):gsub('"','')
    local tokenStr=string.upper(str:gsub(' ','_'))
	if df.item_type[tokenStr] then return df.item_type[tokenStr],-1 end
	for k,v in ipairs(df.global.world.raws.itemdefs.all) do
		if v.id==tokenStr or v.name==str then return getItemType(v),v.subtype end
	end
end
 
local eventful=require('plugins.eventful')
eventful.onReactionComplete.putnamWish=function(reaction,unit,input_items,input_reagents,output_items,call_native)
	local script=require('gui/script')
	print('Wish trying to run.')
	if not reaction.code:find('DFHACK_WISH_SET') then return nil end
	print('Wish running.')
    script.start(function()
        local mattype,matindex,itemtype,itemsubtype,tryAgain
        repeat
            local ok,itemString=script.showInputPrompt('Wish','What item do you want from me?',COLOR_LIGHTGREEN)
            if ok then
                itemtype,itemsubtype=findItemGivenPlainLanguageString(itemString)
            end
            if not itemtype then
                script.showMessage('Darkpact','Dont try to fool me!',COLOR_LIGHTGREEN)
                tryAgain=script.showYesNoPrompt('Darkpact','Chose another?',COLOR_LIGHTGREEN)
            end
        until not tryAgain or itemtype
        repeat
			if not tryAgain then
            local kk,matString=script.showInputPrompt('Darkpact','What material do you wish?',COLOR_LIGHTGREEN)
            if kk then
                mattype,matindex=findMaterialGivenPlainLanguageString(matString)
            end
            if not mattype then
                script.showMessage('Darkpact','Invalid material!',COLOR_LIGHTGREEN)
                tryAgain=script.showYesNoPrompt('Darkpact','Chose another?',COLOR_LIGHTGREEN)
            end end --tryAgain also
        until not tryAgain or mattype
        if mattype and itemtype then
            for k,v in ipairs(df.global.world.raws.reactions) do
                if v.code:find('DFHACK_WISH_GIVE') then
                    for _,product in ipairs(v.products) do
                        product.mat_type=mattype
                        product.mat_index=matindex
						product.item_type=itemtype
						product.item_subtype=itemsubtype
                    end
                end
            end
        end
    end)
end