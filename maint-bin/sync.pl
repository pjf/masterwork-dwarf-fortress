#!/usr/bin/perl
use v5.10.0;
use strict;
use warnings;
use autodie;
use IPC::System::Simple qw(capture systemx);
use FindBin qw($Bin);
use File::Spec;

# Does all the things that @pjf used to do by hand to sync branches

my $now_branch = capture('git rev-parse --abbrev-ref HEAD');
chomp($now_branch);

# Sync branches
systemx(File::Spec->catdir($Bin, 'yoink-master.pl'));

# Push to github
systemx(qw(git push origin master unified));

# Export patches
systemx(File::Spec->catdir($Bin, 'export-patches.pl'));

# And return home...
systemx(qw(git checkout), $now_branch);

# All done!
