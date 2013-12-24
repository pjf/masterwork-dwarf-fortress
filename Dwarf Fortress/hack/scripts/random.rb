# Given a number (N) and a command, runs that command with a N% chance
# This is great if you want a reaction product that *sometimes* does
# something, but not always.

# Original script by Paul '@pjf' Fenwick (aka Urist McTeellox) You can
# use, modify, and redistribute this code under a Creative Commons
# Zero (CC0) license

percent = $script_args[0] || 'help'
command = $script_args.drop(1).join(" ")

if percent == 'help'
    puts <<EOS

random N command - Runs a command N percent of the time.

Examples:

    random 10 siren             # Wake dwarves 10% of the time
    random 25 force megabeast   # 25% chance of megabeast attack

EOS

    throw :script_finished
end

if rand(100) < percent.to_i
    df.dfhack_run command
end

