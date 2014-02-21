-- transforms unit (by number) into another creature, choice given to user. 
-- Syntax is: unitID tickamount maxsize namefilter. A size of 0 is ignored. A length of 0 is also ignored. 
-- If no filter, all units will be sorted. A filter of ALL will also work with all units.

local dialog = require('gui.dialogs')
local script = require('gui.script')
function transform(target,race,caste,length)
    if target==nil then
        qerror("Not a valid target")
    end
    local defaultRace = target.enemy.normal_race
    local defaultCaste = target.enemy.normal_caste
    target.enemy.normal_race = race --that's it???
    target.enemy.normal_caste = caste; --that's it!
    if length>0 then dfhack.timeout(length,'ticks',function() target.enemy.normal_race = defaultRace target.enemy.normal_caste = defaultCaste end) end
end

function compareTableWithString(tbl,str)
	for k,v in ipairs(tbl) do
		if v==str then return true end
	end
end

function getBodySize(caste)
    return caste.body_size_1[#caste.body_size_1-1]
end

function selectCreature(unitID,length,size,filter) --taken straight from here, but edited so I can understand it better: https://gist.github.com/warmist/4061959/... again. Also edited for syndromeTrigger, but in a completely different way.
    size = size or 0
    filter = filter or "all"
    length = length or 2400
    local creatures=df.global.world.raws.creatures.all
    local tbl={}
    local tunit=df.unit.find(unitID)
    for cr_k,creature in ipairs(creatures) do
        for ca_k,caste in ipairs(creature.caste) do
            local name=caste.caste_name[0]
            if name=="" then name="?" end
            if (not filter or compareTableWithString(filter,creature.creature_id) or string.lower(filter[1])=="all") and (not size or size>getBodySize(caste) or size<1 and not creature.flags.DOES_NOT_EXIST) then table.insert(tbl,{name,nil,cr_k,ca_k}) end
        end
    end
    table.sort(tbl,function(a,b) return a[1]<b[1] end)
    local f=function(name,C)
        transform(tunit,C[3],C[4],length)
    end
    script.start(function()
        local ok = 
            script.showYesNoPrompt(
                "Just checking","Transform " 
                .. dfhack.TranslateName(dfhack.units.getVisibleName(tunit)) .. 
                " into a werebeast for "
                ..length/1200 ..
                " days?",
                COLOR_BROWN)
        if ok then dialog.showListPrompt("Creature Selection","Choose creature:",COLOR_WHITE,tbl,f) end
    end)
end
local args = {...}
selectCreature(tonumber(args[1]),tonumber(args[2]),tonumber(args[3]),{'WEREBEAST','SPIDER_CAVE_GIANT','CAVE_DRAGON','GREEN_DEVOURER','VORACIOUS_CAVE_CRAWLER','BEHOLDER','UMBER_HULK','BAT_GIANT_BLIGHT'})

