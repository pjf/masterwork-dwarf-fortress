# probability-syndrome.rb - Run commands with a certain probability
#
# = DESCRIPTION
#
# Including the PROBABILITY_TABLE stone in our reaction causes the
# probability-syndrome script to scan it, build a probability table, and
# execute one command according to the weightings listed
# 
# For example the following would effectively roll 1d5,
# and initiate an ambush by elves, humans, drow, dwarves,
# or have no effect, based upon the result.
# 
#     [PRODUCT:100:1:BOULDER:NONE:INORGANIC:PROBABILITY_TABLE]
#         [PRODUCT:20:1:BOULDER:NONE:INORGANIC:SIEGE_ELF]
#         [PRODUCT:20:1:BOULDER:NONE:INORGANIC:SIEGE_HUMAN]
#         [PRODUCT:20:1:BOULDER:NONE:INORGANIC:SIEGE_DROW]
#         [PRODUCT:20:1:BOULDER:NONE:INORGANIC:SIEGE_DWARF]
# 
# This script should have come with an inorganic_probability_syndrome.txt
# file, but if not, here are the important examples from it:
# 
#     [INORGANIC:PROBABILITY_TABLE]
#         [USE_MATERIAL_TEMPLATE:STONE_VAPOR_TEMPLATE]
#         [SYNDROME]
#             [SYN_CLASS:\AUTO_SYNDROME]
#             [SYN_CLASS:\COMMAND]
#             [SYN_CLASS:probability-syndrome]
#                 [SYN_CLASS:init]
#                 [SYN_CLASS:\REACTION_INDEX]
#                 [SYN_CLASS:\WORKER_ID]
#                 [SYN_CLASS:\LOCATION]
# 
# Sample inorganics showing how to code a probability table entry:
# 
#     [INORGANIC:SIEGE_ELF]
#         [USE_MATERIAL_TEMPLATE:STONE_VAPOR_TEMPLATE]
#         [SYNDROME]
#             [SYN_CLASS:\AUTO_SYNDROME]
#             [SYN_CLASS:\COMMAND]
#             [SYN_CLASS:probability-syndrome][SYN_CLASS:cmd]
#                 [SYN_CLASS:force]
#                 [SYN_CLASS:siege]
#                 [SYN_CLASS:FOREST]
#
# = AUTHOR
# 
# Paul Fenwick (aka Urist McTeellox), December 2013
# Updated with \ARGUMENT expansion, January 2014
# 
# Originally developed as part of the Masterwork Dwarf Fortress:
# Studded With Patches project:
# https://github.com/pjf/masterwork-dwarf-fortress/
#
# = BUGS
# 
# Bugs should be submitted to
# https://github.com/pjf/masterwork-dwarf-fortress/issues
# 
# = LICENSE
#
# You may use and/or modify this code under the same licenses as
# DFHack or Ruby. (Your choice)

# --

# There should be a way to auto-detect our script name

$SCRIPT_NAME = 'probability-syndrome'

# We need a command which looks syntactically nice in
# AutoSyndrome, but signals to us that we should ignore this
# if called with it. (The 'init' command actually does all the
# scanning and hard work)

$NO_OP_CMD   = 'cmd'

# What should our SYN_CLASSES look like at the start of the
# syndrome? Right now we use \AUTO_SYNDROME to make sure that
# the boulder is auto-destroyed.

$PROBABILITY_COMMAND_HEADER = [ '\AUTO_SYNDROME', '\COMMAND', $SCRIPT_NAME, $NO_OP_CMD ]

$DEBUG = false

def get_command_from_syndrome(product, reaction_index, worker_id, x, y, z)

    # Skip things without a material
    return nil if product.mat_index == -1

    # Find syndrome
    puts "\nExamining material index #{product.mat_index}" if $DEBUG

    syndromes = df.world.raws.inorganics[product.mat_index].material.syndrome

    # Skip things without syndromes.
    return nil if syndromes.count == 0

    # We only examine the first syndrome syn classes
    syn_classes = syndromes[0].syn_class.to_a

    # Extract the header section at the front, and see if it
    # matches what we're expecting.

    header  = syn_classes.slice!(0, $PROBABILITY_COMMAND_HEADER.size)
    command = syn_classes # Remainder

    if $DEBUG
        puts "Header : #{header.inspect}"
        puts "Command: #{command.inspect}"
    end

    return nil if not header == $PROBABILITY_COMMAND_HEADER

    puts "Valid entry found!" if $DEBUG

    # Now walk through all the arguments, and cook the meta-arguments
    # into their expansions.

    cooked_command = []

    for argument in command
        case argument
        when '\REACTION_INDEX'
            cooked_command.push(reaction_index)
        when '\WORKER_ID'
            cooked_command.push(worker_id)
        when '\LOCATION'
            cooked_command.push(x, y, z)
        else
            cooked_command.push(argument)
        end
    end

    # Return the command zipped up with spaces, since dfhack_run
    # only takes a command as if written on the dfhack cmdline

    return cooked_command.join(' ')
end

verb = $script_args.shift

puts "\nIn probability syndrome..." if $DEBUG

case verb
when 'help'
    puts "Run a syndrome with a given probability. Invoked from autosyndrome."

when 'cmd'
    # Do nothing. Used so we can make null cmds for autosyndrome

when 'init'
    # Unpack all our arguments, turning them all into integers
    reaction_index, worker_id, x, y, z = $script_args.map { |a| a.to_i }

    # Find all our reaction products.
    reaction = df.world.raws.reactions[reaction_index]
    products = reaction.products

    if $DEBUG
        puts("#{$SCRIPT_NAME} called from #{reaction.code} (#{reaction_index})")
    end

    # We start off by throwing a d100, but this will reduce
    # as we go through all the probabilities. Eg, if our first
    # reaction (20% probabiliy) fails, we'll take the next reaction
    # against a d80. This means our probability table actually functions
    # as a table. (And woe shall befall ye if ye has more than 100%
    # of probability options!)

    max_die_throw = 100

    # Now find all the ones which are tagged to execute
    # from this command

    for product in products

        command = get_command_from_syndrome(product, reaction_index, worker_id, x, y, z)

        next if not command

        probability = product.probability

        puts "\nRunning #{command} with chance #{probability}/#{max_die_throw}" if $DEBUG

        if rand(max_die_throw) < probability
            # We have a winner! Execute this and exit!

            puts "...executing #{command}" if $DEBUG

            df.dfhack_run(command)
            throw :script_finished

        else
            # We need to reduce our max die throw each time
            # so our probability table actually works like
            # people expect

            max_die_throw -= probability

            puts "...Nope! Searching for next command..." if $DEBUG
        end
    end

else
    puts("Unknown #{$SCRIPT_NAME} command (#{verb}). Try 'help'");
end
