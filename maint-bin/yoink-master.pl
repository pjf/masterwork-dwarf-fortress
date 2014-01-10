#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie qw(:all);
use FindBin qw($Bin);
use IPC::System::Simple qw(capture);
use Config::Tiny;

# Yoinks all the changes from master into all active side branches
# that track them.
#
# Grab branches that need yoinking from the .yoinkrc
# file at the repository root.

my $config = Config::Tiny->read("$Bin/maint-settings.ini");

my $MASTER = $config->{git}{master} || 'gold';

my @branches = split(/\s+/, $config->{yoink}{branches});

# Remember our current branch.
my $now_branch = capture('git rev-parse --abbrev-ref HEAD');

# Merge into everything
foreach my $branch (@branches) {
    system("git checkout $branch");
    system("git merge --log --no-edit $MASTER");
}

# Restore our current branch
system("git checkout $now_branch");
