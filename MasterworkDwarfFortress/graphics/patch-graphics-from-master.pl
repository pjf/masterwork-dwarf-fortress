#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie;
use File::Spec;
use Cwd qw(getcwd abs_path);
use IPC::System::Simple qw(capturex);

my $PATCH_FROM = abs_path('../../Dwarf Fortress/raw/objects');

# Patches everything in the graphics directories based upon whatever
# we see via 'git diff' in the main Dwarf Fortress directory. This
# is intended if you're one of the fine folks who is making changes
# to Masterwork, and want to make sure it gets to all the graphics
# sets correctly.
#
# By Paul '@pjf' Fenwick.
# This is open source code. You can use, modify, and distribute it under
# the same terms and Perl 5 itself.

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
            # open(my $patch_fh, '|-', 'patch', '--no-backup-if-mismatch');
            open(my $patch_fh, '|-', 'patch', '--dry-run');

            say {$patch_fh} $patch;
            close($patch_fh);
        }
    }

    # Head back home, so we can do it all again.
    chdir($orig_dir);
}

