local _ENV = mkmodule('fov')

local utils = require 'utils'

--[[

 Native functions:

 * None, not a cpp plugin
 
 Lua functions
 
 * get_fov(radius, pos)		generate and return fov table
 ** radius					circular radius
 ** pos						a table or obj that responds to the fields x, y, and z
 ** fov table				fields xmin, xmax, ymin, ymax, x,y,z (center coord), 
							and indexed [y][x] where 0 = shadow, 1 = visible passable, and 2 = visible obstructed/wall
 
 * print_fov(FOV)				prints the fov map to the console (for a visual reference)

--]]



-- lua port based on the python and ruby implementatoins obtained from
--http://roguebasin.roguelikedevelopment.org/index.php?title=FOV_using_recursive_shadowcasting

-- Multipliers for transforming coordinates into other octants
local MULT =	{
--oct 1  8   3   2   5   4   7   6
	{1,  0,  0, -1, -1,  0,  0,  1},--xx,
	{0,  1, -1,  0,  0, -1,  1,  0},--xy,
	{0,  1,  1,  0,  0, -1, -1,  0},--yx,
	{1,  0,  0,  1, -1,  0,  0, -1},--yy
}


local fov	-- the field of view table [y][x] (row,column)
local zz	-- for holding the working z coordinate

local SHADOW_CHAR = '#'
local SHADOW = 0
--local SHADOW = SHADOW_CHAR
local LIT_PASS_START_CHAR = '*'	-- for print_fov, start is assumed passable
local LIT_PASS_CHAR = '.'
local LIT_PASS = 1
--local LIT_PASS = LIT_PASS_CHAR
local LIT_WALL_CHAR = 'o'
local LIT_WALL = 2
--local LIT_WALL = LIT_WALL_CHAR

-- used while initializing / populating fov
-- marks pos x,y as visible to the unit (at center)
local function light(x,y,c)
	fov[y][x] = c
end

--blocked?(x, y) returns true if the tile at (x, y) blocks view of tiles beyond it (e.g. walls)
local function is_blocked(x,y)
	local tt = dfhack.maps.getTileType(x,y,zz)
	if tt then
		return not df.tiletype_shape.attrs[ df.tiletype.attrs[tt].shape ].passable_flow
	else
		return true -- unloaded block or out of bounds of map
	end
end

-- Recursive light-casting function
local function cast_light(cx, cy, row, light_start, light_end, radius, xx, xy, yx, yy, id)
	local radius_sq,dx,dy,blocked,mx,my,l_slope,r_slope,new_start
	if light_start < light_end then
		return
	end
	radius_sq = radius * radius
	for j=row,radius do -- inclusive
		dx, dy = -j - 1, -j
		blocked = false
		while dx <= 0 do
			dx = dx + 1
			-- Translate the dx, dy co-ordinates into map co-ordinates
			mx, my = cx + dx * xx + dy * xy, cy + dx * yx + dy * yy
			-- l_slope and r_slope store the slopes of the left and right
			-- extremities of the square we're considering:
			l_slope, r_slope = (dx-0.5)/(dy+0.5), (dx+0.5)/(dy-0.5)
			if light_start < r_slope then
				; --continue while ? no statements to skip, do nothing
			elseif light_end > l_slope then
				break
			else
				-- Our light beam is touching this square; light it -- moved, to light differently if obstruction
				--if (dx*dx + dy*dy) < radius_sq then light(mx, my) end
				if blocked then
					-- We are scanning a row of blocked squares
					if is_blocked(mx, my) then
						-- Our light beam is touching this square; light it
						if (dx*dx + dy*dy) < radius_sq then light(mx, my, LIT_WALL) end
						new_start = r_slope
						--next ? continue while? then simply do nothing
					else
						-- Our light beam is touching this square; light it
						if (dx*dx + dy*dy) < radius_sq then light(mx, my,LIT_PASS) end
						blocked = false
						light_start = new_start
					end
				else
					if is_blocked(mx, my) and j < radius then
						-- Our light beam is touching this square; light it
						if (dx*dx + dy*dy) < radius_sq then light(mx, my, LIT_WALL) end
						-- This is a blocking square, start a child scan
						blocked = true
						cast_light(cx, cy, j+1, light_start, l_slope,
									radius, xx, xy, yx, yy, id+1)
						new_start = r_slope
					else
						-- Our light beam is touching this square; light it
						if (dx*dx + dy*dy) < radius_sq then light(mx, my, LIT_PASS) end
						-- Our light beam is touching this square; light it
						--if (dx*dx + dy*dy) < radius_sq then light(mx, my) end
					end
				end
			end
		end -- while dx <= 0
		-- Row is scanned; do next row unless last square was blocked:
		if blocked then break end
	end -- (row..radius+1).each
end

-- Determines which co-ordinates on a 2D grid are visible
-- from a particular co-ordinate.
-- start_x, start_y: center of view
-- radius: how far field of view extends
function get_fov(radius, pos)
	local start_x,start_y,start_z = dfhack.maps.getTileSize() -- reuse start_.. for checking valid pos
	if not pos or not pos.x or not pos.y or not pos.z or 
			pos.x < 0 or pos.y < 0 or pos.z < 0 or 
			pos.x > start_x or pos.y > start_y or pos.z > start_z then
		dfhack.printerr("fov: invalid map coordinats")
		return nil
	end
	start_x,start_y,start_z = pos.x,pos.y,pos.z
	
	zz = start_z	-- global, save z reference for tiletype check
	-- initialize global fov[y][x] (row,column)
	fov = {
		xmin=start_x-radius,	-- needed for iteration, ipairs, and pairs won't work
		xmax=start_x+radius,
		ymin=start_y-radius,
		ymax=start_y+radius,
		x=start_x,y=start_y,z=start_z,	-- center pos, or start pos, for print_fov
	}
	for y=start_y-radius,start_y+radius do
		local row = {}
		for x=start_x-radius,start_x+radius do
			row[x] = SHADOW
		end
		fov[y] = row
	end
	light(start_x, start_y, LIT_PASS)
	for oct=1,8 do
		cast_light(start_x, start_y, 1, 1.0, 0.0, radius, MULT[1][oct], MULT[2][oct], MULT[3][oct], MULT[4][oct], 0)
	end
	return fov
end

    
function print_fov(FOV)
	local v, str
	for y=FOV.ymin,FOV.ymax do
		str = ''
		for x=FOV.xmin,FOV.xmax do
			v = FOV[y][x]
			if v == SHADOW then
				str = str..SHADOW_CHAR..' '
			elseif v == LIT_WALL then
				str = str..LIT_WALL_CHAR..' '
			elseif v == LIT_PASS then
				if x == FOV.x and y == FOV.y then
					str = str..LIT_PASS_START_CHAR..' '
				else
					str = str..LIT_PASS_CHAR..' '
				end
			end
		end
		print(str)
	end
end



return _ENV
