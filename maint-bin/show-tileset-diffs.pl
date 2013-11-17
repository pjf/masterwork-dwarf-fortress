#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie;
use File::Spec;
use FindBin qw($RealBin);
use Cwd qw(getcwd abs_path);
use IPC::System::Simple qw(capturex);

# What's our definitive source of raws?

my $PATCH_FROM = abs_path(
    File::Spec->catfile($RealBin, '..', 'Dwarf Fortress/raw/objects')
);

# Where be our graphics files?

my $GRAPHICS_DIRS = abs_path(
    File::Spec->catfile($RealBin, '..', 'MasterworkDwarfFortress/graphics')
);

# List of harmless tags. We skip diffs which only consist of these.
# (Altough not really harmless; these would be be better called 'display'
# tags)

my %harmless;

@harmless{qw(
    TILE
    CREATURE_TILE
    CREATURE_SOLDIER_TILE
    GLOWTILE

    COLOR
    DISPLAY_COLOR
    GLOWCOLOR

    ITEM_SYMBOL

    SHRUB_TILE
    DEAD_SHRUB_TILE
    PICKED_TILE

    SHRUB_COLOR
    DEAD_SHRUB_COLOR
    PICKED_COLOR

    GRASS_TILES
    GRASS_COLORS

    TREE_TILE
    DEAD_TREE_TILE
    SAPLING_TILE
    DEAD_SAPLING_TILE

    TREE_COLOR
    DEAD_TREE_COLOR
    SAPLING_COLOR
    DEAD_SAPLING_COLOR

    STATE_NAME_ADJ
)} = ();

my $contains_display_info_re = '^[+-][^\n]*?(?:\[' . join('|\[', keys %harmless). ')';

# NOTE : STATE_NAME_ADJ is *not* a display attribute, but
# it appears in oodles of places combined with display tags,
# so we skip it for now.

# Which files do we skip?
#
# TODO: The gem entries contain very dense entries, and this code
# isn't smart enough to deal with them right now. So we skip it
# to avoid excessive noise.

my %skip = (
    'inorganic_stone_gem.txt' => 1,
);

# Print our headers

say "Finding potential tileset differences...";
say "Excluding: ", join(" ", sort keys %skip);
say "Known display tags are: ", join(" ", sort keys %harmless);
say "\n\n\n";

# Let's go looking in graphics directories!

my @directories = glob("$GRAPHICS_DIRS/*");

# Walk through all our graphics dirs

foreach my $base_dir (@directories) {
    next if not -d $base_dir;           # Skip non-dirs

    # Traverse into each graphics dir

    my $dir = File::Spec->catfile($base_dir, 'raw', 'objects');

    chdir($dir);

    # Examine each file, and look for changes

    foreach my $raw_file (glob("*")) {

        next if $skip{$raw_file};

        my @patches;

        {
            local $/ = "\n@@";  # End of patch marker

            # Generate potential differences

            @patches = capturex(
                [0,1], 'diff', '-b', '-u',
                File::Spec->catfile($base_dir, qw(raw objects), $raw_file),
                File::Spec->catfile($PATCH_FROM, $raw_file),
            );

            chomp @patches;

        }

        # Print the ones which are significant, and their headers
        # as appropriate.

        my %printed_headers;
        my $header;

        foreach my $patch (@patches) {
            given ($patch) {
                when (/^(?:\+\+\+|---)/) { $header = $patch }
                when (significant_change($patch)) {
                    if ($patch =~ /($contains_display_info_re)/ms) {

                        # TODO: Unravel the patch so that only the
                        # non-display sections are included.

                        # Throwing these on STDERR isn't ideal.
                        warn "$patch\n";
                    }
                    else {
                        print $header if not $printed_headers{$header}++;
                        print "\n\@\@$patch";
                    }
                }
            }
        }

        print "\n\n";
    }
}

# Takes a patch and figures out if it's a significant change

sub significant_change {
    my ($patch) = @_;

    my @lines = split(/\n+/,$patch);

    foreach (@lines) {
        return 1 if /^(?:\+\+\+|---)/;  # Always print headers
        next if not /^[+-]/;            # Skip context lines
        while (my ($tag) = m/\G[^\[]*\[(\w+)[^\]]*]/gc) {
            return 1 unless exists $harmless{$tag};
        }
    }

    return 0;
}
