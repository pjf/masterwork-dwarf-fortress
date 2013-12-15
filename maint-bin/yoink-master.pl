#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie qw(:all);
use IPC::System::Simple qw(capture);

# Yoinks all the changes from master into all active side branches
# that track them.

my @branches = qw(
    unified
    drowfort
    iteru
    nerfed_diseases
    settings
);

# Remember our current branch.
my $now_branch = capture('git rev-parse --abbrev-ref HEAD');

foreach my $branch (@branches) {
    system("git checkout $branch");
    system("git merge --log --no-edit master");
}

system("git checkout $now_branch");
