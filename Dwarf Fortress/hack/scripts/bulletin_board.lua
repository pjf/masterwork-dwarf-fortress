local ev=require("plugins.eventful")

local gui = require 'gui'
local guidm = require 'gui.dwarfmode'
local widgets =require 'gui.widgets'


function selectRandom(list,wfunction)
    wfunction = wfunction or function(v) return 1 end
    
    local wsum=0
    local parts={}
    for k,v in ipairs(list) do
        local w=wfunction(v,k)
        wsum=wsum+w
        table.insert(parts,{wsum,v})
    end
    for k,v in pairs(parts) do
        print(k,v[1],v[2])
    end
    local choice=math.random()*wsum
    print("choice:",choice,wsum)
    for k,v in ipairs(parts) do
        if v[1]>=choice then
            print("chosen:",v[2])
            return v[2]
            
        end
    end
end
bulletin_board_view = defclass(bulletin_board_view, guidm.MenuOverlay)
bulletin_board_view.ATTRS={
    wshop=DEFAULT_NIL,
    frame_background=dfhack.pen.parse{ch=32,fg=0,bg=0}
}

function bulletin_board_view:init(args)
    self.mngr=bulletin_board_manager{building=args.wshop}
    
    self:addviews{
    widgets.Panel{
        subviews = {
            widgets.Label{ text="Bulletin Board", frame={t=1,l=1} },
            widgets.Label{ text="Posts:", frame={t=3,l=1} },
            widgets.List{ view_id="posts",frame={l=1,b=4,r=1,t=4,yalign=0} ,on_select=self:callback("updateByName"),scroll_keys=widgets.SECONDSCROLL},
            widgets.Label{ view_id="byName", frame={b=4,l=1} },
            widgets.Label{ text={{key='CUSTOM_C',key_sep=": ",text="Clear", on_activate=self:callback("clear")}}, frame={b=3,l=1} },
            widgets.Label{ text={{key='DESTROYBUILDING',key_sep=": ",text="Remove Building"}}, frame={b=2,l=1} },
            widgets.Label{ text={{key='LEAVESCREEN',key_sep=": ",text="Done"}}, frame={b=1,l=1} }
        }
    }
    }
    self:regenView()
end
function bulletin_board_view:updateByName(idx,choice)
    if choice then
        self.subviews.byName:setText("By :"..choice.name)
    else
        self.subviews.byName:setText("")
    end
end
function bulletin_board_view:regenView()
    local mngr=self.mngr
    local posts={}
    for i=1,mngr:count() do
        local name,text=mngr:getString(i)
        table.insert(posts,{text=text,name=name})
        
    end
    self.subviews.posts:setChoices(posts)
end
function bulletin_board_view:clear()
    self.mngr:clear()
    self:regenView()
end
function bulletin_board_view:onInput(keys)
    local allowedKeys={
        "CURSOR_RIGHT","CURSOR_LEFT","CURSOR_UP","CURSOR_DOWN",
        "CURSOR_UPRIGHT","CURSOR_UPLEFT","CURSOR_DOWNRIGHT","CURSOR_DOWNLEFT",
        "CURSOR_UP_Z","CURSOR_DOWN_Z","CHANGETAB","DESTROYBUILDING"}
    if keys.LEAVESCREEN then
        self:dismiss()
        self:sendInputToParent('LEAVESCREEN')
    else
        self:inputToSubviews(keys)
        for _,name in ipairs(allowedKeys) do
            if keys[name] then
                self:sendInputToParent(name)
                self:updateLayout()
                break
            end
        end
    end
    if df.global.world.selected_building ~= self.wshop then
        self:dismiss()
        return
    end
end
--will be using general_ref_creaturest for entries
--[[
        .race -> unit
        .caste 
        .anon_1 -> type
        .anon_2 -> thought (up to 224) | god hfid | etc...
]]
bulletin_board_manager = defclass(bulletin_board_manager)
bulletin_board_manager.ATTRS={
    building=DEFAULT_NIL,
    maxSize=10,
}
function bulletin_board_manager:removeFirst()
    for k,v in pairs(self.building.general_refs) do
        if v:getType()==df.general_ref_type.CREATURE then
            self.building.general_refs:erase(k)
            v:delete()
            return
        end
    end
end
function bulletin_board_manager:clear()
    local ids={}
    for k,v in pairs(self.building.general_refs) do
        if v:getType()==df.general_ref_type.CREATURE then
            table.insert(ids,k)
        end
    end
    for i=#ids,1,-1 do
        local ref=self.building.general_refs[ids[i]]
        self.building.general_refs:erase(ids[i])
        ref:delete()
    end
end
function bulletin_board_manager:addNew()
    if self:count()>self.maxSize then
        self:removeFirst()
    end
    local gen_ref=df.general_ref_creaturest:new()
    self.building.general_refs:insert("#",gen_ref)
    return gen_ref
end
function bulletin_board_manager:addFromUnit(unit)
    local ref=self:addNew()
    ref.race=unit.id
    local failed=true
    while failed do
        failed=false
        local typeChoice={"thought","god","pet"}
        local typeChosen=selectRandom(typeChoice)
        if typeChosen=="thought" then
            ref.anon_1=0
            local choice=selectRandom(unit.status.recent_events,function(v) return math.abs(v.severity)+1 end)
            ref.anon_2=choice.type
            --TODO add better weight
        elseif typeChosen=="god" then
            local hf=dfhack.units.getNemesis(unit).figure
            local gods={}
            for k,v in ipairs(hf.histfig_links) do
                if v:getType()==df.histfig_hf_link_type.DEITY then
                    table.insert(gods,v.target_hf)
                end
            end
            if #gods==0 then
                failed=true
            else
                ref.anon_1=1
                local choice=selectRandom(gods)
                ref.anon_2=choice
            end
        elseif typeChosen=="pet" then
            ref.anon_1=2
            local pet=dfhack.units.getSpecificRef(unit, df.specific_ref_type.PETINFO_OWNER)
            if pet then
                pet=pet.pet
                local unit=df.units.find(pet.pet_unit_id)
                ref.anon_2=unit.race
            else
                ref.anon_2=math.random(0,#df.global.world.raws.creatures.all-1)
            end
            --pets are held in specific refs in unit
            --specific_ref_type.PETINFO_OWNER
            --picture of pet or just random unit?
        end
    end
    --also could write about hated/loved units, gods, kitties (or other pets)
end
function bulletin_board_manager:count()
    local num=0
    for k,v in pairs(self.building.general_refs) do
        if v:getType()==df.general_ref_type.CREATURE then
            num=num+1
        end
    end
    return num
end
function bulletin_board_manager:getRef(ref_num)
    local num=0
    for k,v in pairs(self.building.general_refs) do
        if v:getType()==df.general_ref_type.CREATURE then
            num=num+1
            if num==ref_num then 
                return v
            end
        end
    end
    return nil
end
function bulletin_board_manager:decodeUnitPicture(ref,unit)
    local raw=df.global.world.raws.creatures.all[ref.anon_2]
    return string.format("Scribbled a picture of %s",raw.name[0])
end
function bulletin_board_manager:decodeGod(ref,unit)
    local hf=df.historical_figure.find(ref.anon_2)
    return string.format("Encouragement to worshiping %s",dfhack.TranslateName(hf.name,true,false))
    --todo add spheres
end
function bulletin_board_manager:decodeThought(ref,unit)
    local thought=ref.anon_2
    return string.format("Some thoughs about %s",df.unit_thought_type[thought])
end
function bulletin_board_manager:getString(num)
    local ref=self:getRef(num)
    local unit=df.unit.find(ref.race)
    
    local name
    if unit then
        name=dfhack.TranslateName(unit.name)
    else
        name="Someone"
    end
    if ref.anon_1==0 then
        return name,self:decodeThought(ref,unit)
    elseif ref.anon_1==1 then
        return name,self:decodeGod(ref,unit)
    elseif ref.anon_1==2 then
        return name,self:decodeUnitPicture(ref,unit)
    else
        return name,"??"
    end
end

function drawSidebar(wshop)
    local valid_focus="dwarfmode/QueryBuilding/Some"
    if string.sub(dfhack.gui.getCurFocus(),1,#valid_focus)==valid_focus and
        wshop:getMaxBuildStage()==wshop:getBuildStage() 
    then
        local sidebar=bulletin_board_view{wshop=wshop}
        sidebar:show()
    end
end
function getBuildingFromJob(job)
    for k,v in ipairs(job.general_refs) do
        if v:getType()==df.general_ref_type.BUILDING_HOLDER then
            return df.building.find(v.building_id)
        end
    end
end
function postMessageDone(reaction,unit)
    local building=getBuildingFromJob(unit.job.current_job)
    print("Posted on building:"..tostring(building))
    local mngr=bulletin_board_manager{building=building}
    mngr:addFromUnit(unit)
    print("Posts:",mngr:count())
    for i=1,mngr:count() do
        print(mngr:getString(i))
    end
end
function getLastJobLink()
    local st=df.global.world.job_list
    while st.next~=nil do
        st=st.next
    end
    return st
end
function addNewJob(job)
    local lastLink=getLastJobLink()
    local newLink=df.job_list_link:new()
    newLink.prev=lastLink
    lastLink.next=newLink
    newLink.item=job
    job.list_link=newLink
end
function UnassignJob(job,unit,unit_pos)
    unit.job.current_job=nil
end
function makeJob(args)
    local newJob=df.job:new()
    newJob.id=df.global.job_next_id
    df.global.job_next_id=df.global.job_next_id+1
    --newJob.flags.special=true
    newJob.job_type=args.job_type
    newJob.completion_timer=-1

    newJob.pos:assign(args.pos)
    args.job=newJob
    local failed
    for k,v in ipairs(args.pre_actions or {}) do
        local ok,msg=v(args)
        if not ok then
            failed=msg
            break
        end
    end
    if failed==nil then
        AssignUnitToJob(newJob,args.unit,args.from_pos)
        for k,v in ipairs(args.post_actions or {}) do
            local ok,msg=v(args)
            if not ok then
                failed=msg
                break
            end
        end
        if failed then
            UnassignJob(newJob,args.unit)
        end
    end
    if failed==nil then
        addNewJob(newJob)
        return newJob
    else
        newJob:delete()
        return false,failed
    end
    
end
function AssignUnitToJob(job,unit,unit_pos)
    job.general_refs:insert("#",{new=df.general_ref_unit_workerst,unit_id=unit.id})
    unit.job.current_job=job
    unit_pos=unit_pos or {x=job.pos.x,y=job.pos.y,z=job.pos.z}
    unit.path.dest:assign(unit_pos)
    return true
end
function AssignBuildingRef(bld)
    return function(args)
        args.job.general_refs:insert("#",{new=df.general_ref_building_holderst,building_id=bld.id})
        bld.jobs:insert("#",args.job)
    end
end
function assignReactionName(args)
    args.job.reaction_name="LUA_HOOK_POST_ON_BOARD"
end
function makePostJob(building,unit)
    local args={}
    args.pos={x=building.centerx,y=building.centery,z=building.z}
    args.job_type=df.job_type.CustomReaction
    args.pre_actions={assignReactionName}
    args.post_actions={AssignBuildingRef(building)}
    args.unit=unit
    local ok,msg=makeJob(args)
    if not ok then
        qerror(msg)
    end
end
function addJob(wshop)
    if math.random()<0.65 then
        return
    end
    local DISTANCE=25
    local dsq=DISTANCE*DISTANCE
    print("Trying to add job:"..tostring(wshop))
    local dwarfs_near={}
    for k,v in pairs(df.global.world.units.active) do
        if dfhack.units.isCitizen(v) and v.job.current_job==nil then
            local dx=wshop.centerx-v.pos.x
            local dy=wshop.centery-v.pos.y
            local dz=wshop.z-v.pos.z
            if dx*dx+dy*dy<dsq and dz==0 then --only on same level
                table.insert(dwarfs_near,v)
            end
        end
    end
    if #dwarfs_near>0 then
        print(string.format("found %d dwarfs nearby",#dwarfs_near))
        makePostJob(wshop,dwarfs_near[math.random(1,#dwarfs_near-1)])
    end
end
ev.registerSidebar("BULLETIN_BOARD",drawSidebar)
ev.registerReaction("LUA_HOOK_POST_ON_BOARD",postMessageDone)
require("plugins.building-hacks").registerBuilding{name="BULLETIN_BOARD",action={500,addJob}}