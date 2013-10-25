-- Makes everyone worship someone. Usage: [scriptname] [unit's first name]
args = {...}

local function findWorshipTarget(filter)
    for _,fig in ipairs(df.global.world.history.figures) do
        if fig.name.first_name == filter then return fig.id end
    end
end

local function setAllWorshippersInWorldToWorshipFig(filter)
    local worshipTarget = findWorshipTarget(filter)
    for k,fig in ipairs(df.global.world.history.figures) do
        fig.histfig_links:insert('#',{new = df.histfig_hf_link_deityst,target_hf=worshipTarget,link_strength=9})
    end
end

setAllWorshippersInWorldToWorshipFig(tostring(args[1]))