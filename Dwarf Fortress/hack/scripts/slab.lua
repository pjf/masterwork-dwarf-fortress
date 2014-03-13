function createItem(mat,description,quality,pos)
    local item=df.item_slabst:new()
    item.id=df.global.item_next_id
    df.global.world.items.all:insert('#',item)
    df.global.item_next_id=df.global.item_next_id+1
    item:setMaterial(mat[1])
    item:setMaterialIndex(mat[2])
    item:categorize(true)
    item.flags.removed=true
    item:setSharpness(0,0)
    item:setQuality(quality-0)
    item.description=description
    dfhack.items.moveToGround(item,{x=pos.x,y=pos.y,z=pos.z})
    return item
end

function posIsValid(pos)
    return (pos.x~=30000 and pos.y~=30000 and pos.z~=30000) and pos or false
end

function engraveSlab()
    local script=require('gui.script') ---require('gui.script')
    local pos
    if df.global.gamemode~=1 then
        pos=posIsValid(df.global.cursor) or posIsValid(dfhack.gui.getSelectedUnit(true).pos) or posIsValid(dfhack.gui.getSelectedItem(true).pos)
        if not posIsValid(pos) then qerror('Needs a valid position (select an item or unit or place your cursor somewhere!') end
    else
        pos=posIsValid(df.global.world.units.active[0].pos) or posIsValid(df.global.cursor) or posIsValid(dfhack.gui.getSelectedUnit(true).pos) or posIsValid(dfhack.gui.getSelectedItem(true).pos)
    end
    script.start(function()
	local matinfo=dfhack.matinfo.find('WOOD') --you can just change this to whatever, as would be expected in the raws, though omitting CREATURE_MAT/PLANT_MAT/INORGANIC
    local descriptionok,description=script.showInputPrompt('Slab','What should the slab say?',COLOR_WHITE)
    local item=createItem({matinfo.type,matinfo.index},description,0,pos) 
    end)
end

engraveSlab()