#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie;
use File::Spec;
use FindBin qw($Bin);
use Cwd qw(getcwd abs_path);
use IPC::System::Simple qw(capturex);

# Patches everything in the graphics directories based upon whatever
# we see via 'git diff' in the main Dwarf Fortress directory. This
# is intended if you're one of the fine folks who is making changes
# to Masterwork, and want to make sure it gets to all the graphics
# sets correctly.
#
# By Paul '@pjf' Fenwick.
# This is open source code. You can use, modify, and distribute it under
# the same terms and Perl 5 itself.

# This script assumes it's being run from the graphics directory,
# so let's get there.

chdir("$Bin/../MasterworkDwarfFortress/graphics");

my $PATCH_FROM = abs_path('../../Dwarf Fortress/raw/objects');

my $orig_dir = getcwd();

my @directories = glob("*");

# Walk through all our graphics dirs

foreach my $base_dir (@directories) {
    next if not -d $base_dir;           # Skip non-dirs

    # Traverse into each graphics dir

    my $dir = File::Spec->catfile($base_dir, 'raw', 'objects');

    say "\n== $base_dir ==";

    chdir($dir);

    # Examine each file, and see if it needs patching

    foreach my $raw_file (glob("*")) {

        # Use git to diff our master file, giving us a patch just for this
        # file.
        my $patch = capturex('git', 'diff', File::Spec->catfile($PATCH_FROM, $raw_file));

        # If we have a patch to apply, then do so

        if ($patch) {
            print "\n+ ";
            open(my $patch_fh, '|-', 'patch', '--no-backup-if-mismatch','-r','-','-N');

            say {$patch_fh} $patch;
            CORE::close($patch_fh); # Ignore close failures.
        }
    }

    # Head back home, so we can do it all again.
    chdir($orig_dir);
}

