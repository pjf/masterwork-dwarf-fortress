local helpstring = [[ Display an announcment and write it to the game log.
  announce message|? [color]
    message  The message you want to display.
    color    The color to display the message in.
    ?        Print this help.
]]

if not dfhack.isMapLoaded() then
	qerror('Map is not loaded.')
end

local args = {...}
if not args or args[1] == "?" then
	print(helpstring)
	return
end

local text = args[1]

-- COLOR_BLACK
-- COLOR_BLUE
-- COLOR_GREEN
-- COLOR_CYAN
-- COLOR_RED
-- COLOR_MAGENTA
-- COLOR_BROWN
-- COLOR_GREY
-- COLOR_DARKGREY
-- COLOR_LIGHTBLUE
-- COLOR_LIGHTGREEN
-- COLOR_LIGHTCYAN
-- COLOR_LIGHTRED
-- COLOR_LIGHTMAGENTA
-- COLOR_YELLOW
-- COLOR_WHITE
local color_id = args[2]
if not color_id then color_id = "COLOR_WHITE" end

local color = _G[color_id]

dfhack.gui.showAnnouncement(text, color)

local log = io.open('gamelog.txt', 'a')
log:write(text.."\n")
log:close()