#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie qw(:all);
use FindBin qw($Bin);

chdir("$Bin/..");

my @junkfiles = (
    'Dwarf Fortress/stderr.log',
    'Dwarf Fortress/stdout.log',
    'Dwarf Fortress/gamelog.txt',
    'Dwarf Fortress/forumdwarves.txt',
    'Dwarf Fortress/dfhack.history',
    'MasterworkDwarfFortress/Utilities/DwarfTherapist/log/run.log',
);

# Remove Thumbs.db
system("find . -name Thumbs.db -exec rm {} ';'");

foreach my $file (@junkfiles) {
    CORE::unlink($file);    # CORE:: means it's okay to fail
}
