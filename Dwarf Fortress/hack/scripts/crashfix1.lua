function process_hive(hive)
    for i,iinfo in ipairs(hive.contained_items) do
        if df.item_verminst:is_instance(iinfo.item) then
            return
        end
    end

    local found = false

    for i,iinfo in ipairs(hive.contained_items) do
        if iinfo.use_mode == 0 and iinfo.item.flags.in_building then
            found = true
            iinfo.item.flags.in_building = false
        end
    end

    if found then
        print('Fixed a broken hive at ('..hive.x1..','..hive.y1..','..hive.z..')')
    end
end

for i,hive in ipairs(df.global.world.buildings.other.HIVE) do
    process_hive(hive)
end