#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie qw(:all);
use FindBin qw($Bin);
use IPC::System::Simple qw(capture);

# Yoinks all the changes from master into all active side branches
# that track them.
#
# Grab branches that need yoinking from the .yoinkrc
# file at the repository root.

my $MASTER = 'gold';

my @branches = do{
    open(my $yoinkrc, '<', "$Bin/../.yoinkrc");
    <$yoinkrc>;
};

chomp(@branches);

# Remember our current branch.
my $now_branch = capture('git rev-parse --abbrev-ref HEAD');

# Merge into everything
foreach my $branch (@branches) {
    system("git checkout $branch");
    system("git merge --log --no-edit $MASTER");
}

# Restore our current branch
system("git checkout $now_branch");
