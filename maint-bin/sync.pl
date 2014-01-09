#!/usr/bin/perl
use v5.10.0;
use strict;
use warnings;
use autodie;
use IPC::System::Simple qw(capture systemx);
use FindBin qw($Bin);
use File::Spec;
use Config::Tiny;

# Does all the things that @pjf used to do by hand to sync branches

my $config = Config::Tiny->read("$Bin/maint-settings.ini");

my $now_branch = capture('git rev-parse --abbrev-ref HEAD');
chomp($now_branch);

# Sync branches
systemx(File::Spec->catdir($Bin, 'yoink-master.pl'));

my @branches = split(/\s+/,$config->{export}{branches});

@branches = "alpha beta gold" if not @branches;

# Push to github
systemx(qw(git push origin), @branches);

# Export patches
systemx(File::Spec->catdir($Bin, 'export-patches.pl'));

# Export the manual (TODO - Pick which branch to export!)
systemx(File::Spec->catdir($Bin, 'export-manual.pl'));

# And return home...
systemx(qw(git checkout), $now_branch);

# All done!
