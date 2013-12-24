#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie qw(:all);
use IPC::System::Simple qw(capture);

# Yoinks all the changes from master into all active side branches
# that track them.

# This ordering produces the most beautiful looking network diagram
# on github. :)
my @branches = qw(
    unified
);

# Remember our current branch.
my $now_branch = capture('git rev-parse --abbrev-ref HEAD');

# Merge into everything
foreach my $branch (@branches) {
    system("git checkout $branch");
    system("git merge --log --no-edit master");
}

# Restore our current branch
system("git checkout $now_branch");
