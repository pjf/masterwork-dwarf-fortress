#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie qw(:all);
use IPC::System::Simple qw(systemx);
use FindBin qw($Bin);
use Cwd;
use File::Spec;

# Simple code for exporting the masterwork manual.
# By Paul '@pjf' Fenwick, January 2014
# You can use and/or modify this code under the same terms as Perl itself.

my $export_file = "$ENV{HOME}/Dropbox/Public/MWDF/MWDF-manual.zip"; 

# Move to our repo root.
chdir("$Bin/..");

# Find out what it's called
my $repo_dir = (File::Spec->splitdir(cwd()))[-1];   # Find

# Move one above repo root. This lets us make sure our
# zipfile starts with a directory.
chdir(".."); 

my @include_files = glob("$repo_dir/*.html");

push @include_files, "$repo_dir/Quick-Guides";

push @include_files, glob("$repo_dir/MasterworkDwarfFortress/*repository");

CORE::unlink($export_file); # Remove old zipfile
systemx('zip', '-r', '-q', $export_file, @include_files);
