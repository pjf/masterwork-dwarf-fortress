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

my $all_raws = [ @main_raws, @graphics_raws ];

check_reactions(\@main_raws);
check_raw_headers($all_raws);
check_duplicate_objects($all_raws);

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
    my %hotkey;
    my %reaction_class;

    my $reaction;
    
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
            $reaction = $1; # Remeber this for hotkey conflicts
            $defined_reaction{$1}{$filename}++;
        }
        elsif (/\[BUILDING:(?<building>[^:]+):(?<hotkey>[^]]+)\]/) {

            # Skip 'none' hotkeys. Also, WTF, are there two ways of
            # specify a hotkey isn't there?
            next if $+{hotkey} =~ /^(?:CUSTOM_)?NONE$/;

            # If reactions are in different files, we'll assume
            # they're for different races, and it's okay if they
            # have the same hotkey.

            # Yes, we're totally munging keys into a string here to
            # simplify the conflict detection code later on.

            my $key = sprintf("%-30s%-30s%-15s",$filename, $+{building}, $+{hotkey});

            push( @{$hotkey{$key}}, $reaction);
        }
        elsif (/\[REACTION_CLASS:([^\]]+)\]/) {
            $reaction_class{$1}++;   # Count seen reaction classes
        }
    }

    # Errors we spot:
    #   * Reaction is permitted, but not defined
    #   * Reaction is defined, but not permitted
    #   * Hotkeys over-assigned

    say show_hash_diff(\%permitted_reaction, \%defined_reaction, "Permitted, but not defined (BUGS!)");

    say show_hash_diff(\%defined_reaction, \%permitted_reaction, "Defined, but not permitted (WARNING)");

    # Show used-once reaction classes

    # Show hotkey conflicts

    say "\n\n== Hotkey Conflicts ==\n\n";

    foreach my $key (sort keys %hotkey) {

        my $count = @{$hotkey{$key}};

        if ($count > 1) {       # Conflict!
            say "$key ($count)";
        }
    }

    say "\n\n== Used once reaction classes ==\n\n";

    foreach my $class (sort keys %reaction_class) {
        if ($reaction_class{$class} == 1) {
            say "Reaction class used only once: $class";
        }
    }

    
    return;
}

sub check_duplicate_objects {
    my ($files) = @_;

    local @ARGV = @$files;

    my %defined_object_count;
    my $object_type;
    
    my $file = "";;

    say "\n\n== Duplicate Objects ==\n\n";

    while (<>) {

        if ($file ne $ARGV) {
            # We've changed files!
            display_duplicate_raws($file, \%defined_object_count);
            $file = $ARGV;
            undef $object_type;
            %defined_object_count = ();;
        }

        if (/\[OBJECT:([^\]]+)\]/) {
            # What objects are defined in this file?
            $object_type = $1;
        }
        elsif ($object_type and /\[(\Q$object_type\E:[^\]]+)\]/) {
            # Check for duplicated objects
            $defined_object_count{$1}++;
        }
    }


}

sub display_duplicate_raws {

    my ($file, $obj_count) = @_;

    foreach my $obj (keys %$obj_count) {
        next if $obj_count->{$obj} <= 1;

        say "$obj ($file)"
    }
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
