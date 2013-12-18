#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie;

# Raw linting code by Paul '@pjf' Fenwick
# You can use, modify, and redistribute this code under
# the same terms as Perl itself.

use FindBin qw($Bin);
use File::Spec;
use File::Glob qw(bsd_glob);    # Oh my glob!

my $rawdir = File::Spec->catdir(
    $Bin, '..', 'Dwarf Fortress', 'raw', 'objects',
);

-d $rawdir or die "Can't find rawdir: $rawdir\n";

# Using @ARGV and Perl's <> operator saves us time when
# walking through lots of files

local @ARGV = bsd_glob("$rawdir/*.txt");

my %permitted_reaction;
my %defined_reaction;

# Walk through everything, record seen and permitted reactions,
# reporting what's missing
    
while (<>) {

    # Skip adventure mode reactions. They won't be 'permitted'
    # by entities.
    next if $ARGV =~ /reaction_wanderer\.txt/;

    if (/\[PERMITTED_REACTION:([^\]]+)\]/) {
        $permitted_reaction{$1}++;
    }
    elsif (/\[REACTION:([^\]]+)\]/) {
        $defined_reaction{$1}++;
    }
}

# Errors we spot:
#   * Reaction is permitted, but not defined
#   * Reaction is defined, but not permitted

say show_hash_diff(\%permitted_reaction, \%defined_reaction, "Permitted, but not defined (BUGS!)");

say show_hash_diff(\%defined_reaction, \%permitted_reaction, "Defined, but not permitted (WARNING)");

sub show_hash_diff {
    my ($h1, $h2, $title) = @_;

    my $out = "\n\n== $title==\n\n";

    my %h1 = %$h1;
    delete @h1{keys %$h2};

    foreach my $key (sort keys %h1) {
        next if $key =~ /^CHEAT/;   # Ignore cheats
        $out .= "    * $key\n";
    }

    return $out;
}
