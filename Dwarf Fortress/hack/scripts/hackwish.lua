-- Allows for script-based wishing.
 
function createItem(mat,itemType,quality,pos)
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
    item:setSharpness(1,0)
    item:setQuality(quality-1)
    dfhack.items.moveToGround(item,{x=pos.x,y=pos.y,z=pos.z})
end
 
function qualityTable()
    return {{'None'},
    {'-Well-crafted-'},
    {'+Finely-crafted+'},
    {'*Superior*'},
    {string.char(240)..'Exceptional'..string.char(240)},
    {string.char(15)..'Masterwork'..string.char(15)}
    }
end
 
local script=require('gui/script')
 
function script.showItemPrompt(text,item_filter,hide_none)
    require('gui.materials').ItemTypeDialog{
        text=text,
        item_filter=item_filter,
        hide_none=hide_none,
        on_select=script.mkresume(true),
        on_cancel=script.mkresume(false),
        on_close=script.qresume(nil)
    }:show()
    
    return script.wait()
end
 
function hackWish(posOrUnit)
    local pos = df.unit:is_instance(posOrUnit) and posOrUnit.pos or posOrUnit
    script.start(function()
--        local amountok, amount
        local itemok,itemtype,itemsubtype=script.showItemPrompt('What item do you want?',function() return true end,true)
        local matok,mattype,matindex=script.showMaterialPrompt('Wish','And what material should it be made of?')
        local qualityok,quality=script.showListPrompt('Wish','What quality should it be?',COLOR_LIGHTGREEN,qualityTable())
--        repeat amountok,amount=script.showInputPrompt('Wish','How many do you want? (numbers only!)',COLOR_LIGHTGREEN) until tonumber(amount)
        if mattype and itemtype then
--            for i=1,tonumber(amount) do
            createItem({mattype,matindex},{itemtype,itemsubtype},quality,pos)
--            end
        end
    end)
end
 
args={...}
 
eventful=require('plugins.eventful')
 
if args[1]~='startup' then 
    local posOrUnit=#args<1 and dfhack.gui.getSelectedUnit(true) and dfhack.gui.getSelectedUnit(true) or #args<1 and df.global.cursor or #args<3 and df.unit.find(args[1]) or {x=args[1],y=args[2],z=args[3]}
    hackWish(posOrUnit)
else
    eventful.onReactionComplete.hackWishP=function(reaction,unit,input_items,input_reagents,output_items,call_native)
        if not reaction.code:find('DFHACK_WISH') then return nil end
        hackWish(unit)
    end
end