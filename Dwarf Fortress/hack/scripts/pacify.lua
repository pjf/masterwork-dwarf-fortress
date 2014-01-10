-- Remove the invader marks from an unit
local unit=dfhack.gui.getSelectedUnit()
if not unit then qerror("No unit selected") end

local mo = require 'makeown'

mo.make_own(unit)
-- Taking down all the hostility flags
unit.flags1.marauder = false
unit.flags1.active_invader = false
unit.flags1.hidden_in_ambush = false
unit.flags1.hidden_ambusher = false
unit.flags1.invades = false
unit.flags1.coward = false
unit.flags1.invader_origin = false
unit.flags2.underworld = false
unit.flags2.visitor_uninvited = false
unit.invasion_id = -1
