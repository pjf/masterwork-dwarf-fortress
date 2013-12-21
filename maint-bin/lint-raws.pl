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

my $raw_dir = File::Spec->catdir(
    $Bin, '..', 'Dwarf Fortress', 'raw', 'objects',
);

-d $raw_dir or die "Can't find rawdir: $raw_dir\n";

my @main_raws = bsd_glob("$raw_dir/*.txt");

my $graphics_dir = File::Spec->catdir(
    $Bin, '..', 'MasterworkDwarfFortress', 'graphics',
);

my @graphics_raws = bsd_glob("$graphics_dir/*/raw/objects/*.txt");

check_reactions(\@main_raws);
check_raw_headers([ @main_raws, @graphics_raws ]);

sub check_raw_headers {
    my ($raws) = @_;

    say "\n\n== Malformed headers ==\n\n";

    foreach my $file (@$raws) {

        # Get our filename without the extension or path.
        my ($intended_header) = ( $file =~ m{/([^/]*)\.txt$} );

        open(my $fh, '<', $file);
        my $header = <$fh>;

        next if not defined $header;    # Skip empty files.

        # Make sure our file starts with its header.

        if ($header !~ /^$intended_header\s*$/) {
            my $shortname = File::Spec->catdir(
                (File::Spec->splitdir($file))[-5..-1]
            );
            $header =~ s/\s*$//;
            printf "[ %30s ] in [ %80s ] is malformed\n",
                   $header,   $shortname
            ;
        }
    }
}

sub check_reactions { 

    my ($raws) = @_;

    # Using @ARGV and Perl's <> operator saves us time when
    # walking through lots of files

    local @ARGV = @$raws;

    my %permitted_reaction;
    my %defined_reaction;

    # Walk through everything, record seen and permitted reactions,
    # reporting what's missing
        
    while (<>) {

        # Skip adventure mode reactions. They won't be 'permitted'
        # by entities.
        next if $ARGV =~ /reaction_wanderer\.txt/;

        my $filename = (File::Spec->splitpath($ARGV))[-1];

        if (/\[PERMITTED_REACTION:([^\]]+)\]/) {
            $permitted_reaction{$1}{$filename}++;
        }
        elsif (/\[REACTION:([^\]]+)\]/) {
            $defined_reaction{$1}{$filename}++;
        }
    }

    # Errors we spot:
    #   * Reaction is permitted, but not defined
    #   * Reaction is defined, but not permitted

    say show_hash_diff(\%permitted_reaction, \%defined_reaction, "Permitted, but not defined (BUGS!)");

    say show_hash_diff(\%defined_reaction, \%permitted_reaction, "Defined, but not permitted (WARNING)");

}

sub show_hash_diff {
    my ($h1, $h2, $title) = @_;

    my $out = "\n\n== $title ==\n\n";

    my %h1 = %$h1;
    delete @h1{keys %$h2};

    foreach my $key (sort keys %h1) {
        next if $key =~ /^CHEAT/;   # Ignore cheats
        my @files = sort keys %{ $h1{$key} };
        $out .= " - [ ] $key (@files)\n";
    }

    return $out;
}
