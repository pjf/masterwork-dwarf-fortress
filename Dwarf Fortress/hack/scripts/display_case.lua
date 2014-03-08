local ev=require("plugins.eventful")

local gui = require 'gui'
local guidm = require 'gui.dwarfmode'
local widgets =require 'gui.widgets'

display_case_view = defclass(display_case_view, guidm.MenuOverlay)
display_case_view.ATTRS={
    wshop=DEFAULT_NIL,
    frame_background=dfhack.pen.parse{ch=32,fg=0,bg=0}
}
function display_case_view:init(args)
    self.item=self.wshop.contained_items[0].item --todo check if always the case!
    local item_name=dfhack.items.getDescription(self.item,0,true)
    local label_text={item_name}
    local artifact_ref=dfhack.items.getGeneralRef(self.item,df.general_ref_type.ARTIFACT)
    local art_label1
    local art_label2
    if artifact_ref then
        local artifactrec=df.artifact_record.find(artifact_ref.artifact_id)
        art_label1=dfhack.TranslateName(artifactrec.name,false)
        art_label2=dfhack.TranslateName(artifactrec.name,true)
        if art_label1==art_label2 then
            art_label2=nil
        end
    end
    self:addviews{
    widgets.Panel{
        subviews = {
            widgets.Label{ text="Display Case", frame={t=1,l=1} },
            widgets.Label{ text="Currently displaying", frame={xalign=0.5,yalign=0.2},auto_width=true },
            widgets.Label{ text=label_text,auto_width=true,frame={t=6} },
            widgets.Label{ text=art_label1,auto_width=true,frame={t=7} },
            widgets.Label{ text=art_label2,auto_width=true,frame={t=8} },
            widgets.Label{ text={{key='DESTROYBUILDING',key_sep=": ",text="Remove Building"}}, frame={b=2,l=1} },
            widgets.Label{ text={{key='LEAVESCREEN',key_sep=": ",text="Done"}}, frame={b=1,l=1} }
        }
    }
    }
end

function display_case_view:onInput(keys)
    local allowedKeys={
        "CURSOR_RIGHT","CURSOR_LEFT","CURSOR_UP","CURSOR_DOWN",
        "CURSOR_UPRIGHT","CURSOR_UPLEFT","CURSOR_DOWNRIGHT","CURSOR_DOWNLEFT","CURSOR_UP_Z","CURSOR_DOWN_Z","DESTROYBUILDING"}
    if keys.LEAVESCREEN then
        self:dismiss()
        self:sendInputToParent('LEAVESCREEN')
    else
        for _,name in ipairs(allowedKeys) do
            if keys[name] then
                self:sendInputToParent(name)
                break
            end
        end
        self:inputToSubviews(keys)
    end
    if df.global.world.selected_building ~= self.wshop then
        self:dismiss()
        return
    end
end

function drawSidebar(wshop)
    local valid_focus="dwarfmode/QueryBuilding/Some"
    if string.sub(dfhack.gui.getCurFocus(),1,#valid_focus)==valid_focus and
        wshop:getMaxBuildStage()==wshop:getBuildStage() 
    then
        local sidebar=display_case_view{wshop=wshop}
        sidebar:show()
    end
end
ev.registerSidebar("DISPLAY_CASE",drawSidebar)