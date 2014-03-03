local args={...}
local f,err=load (table.concat (args," "))
if not f then qerror(err) end
f()