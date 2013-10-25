local r=nil
local top=dfhack.gui.getCurViewscreen()
print(top.parent,top)
local cvstype=dfhack.gui.getFocusString(top)
print(dfhack.gui.getFocusString(top.parent).."/"..cvstype)
if cvstype=="dungeon_monsterstatus" then
	print("  inventory size: "..#top.inventory)
	print("  inventory_cursor: "..top.inventory_cursor)
	r=top.inventory[top.inventory_cursor].item
else
	r=dfhack.gui.getSelectedItem()
end

if r then
	r.wear=0
	r.wear_timer=0
	printall(r)
end