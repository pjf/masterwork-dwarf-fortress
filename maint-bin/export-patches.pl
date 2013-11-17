#!/usr/bin/perl
use v5.10.0;
use strict;
use warnings;
use autodie;
use IPC::System::Simple qw(capture systemx);

my $UPSTREAM      = 'upstream';     # git branch with official MWDF latest.
my $PATCHLOG_FILE = 'PATCHLOG.txt'; # Tiny git history goes here.
my $EXPORT_DIR    = "$ENV{HOME}/Dropbox/Public/MWDF";

my $EXCLUDED_FILES_RE = qr{
    ^MasterworkDwarfFortress/Utilities/DwarfTherapist|
    ^maint-bin|
    \.pl$|
    \.gitignore$
}msx;

my @BRANCHES = qw(master unified);

# Remember what branch we're on now
my $now_branch = capture('git rev-parse --abbrev-ref HEAD');
chomp($now_branch);

# Make patches for all our important branches
foreach my $branch (@BRANCHES) {
    make_patch_from_branch($branch);
}

# Return to our original branch
say "Returning to $now_branch";
systemx('git','checkout',$now_branch);

sub make_patch_from_branch {
    my ($branch) = @_;

    say "Packaging $branch";

    # Switch to our branch
    systemx('git','checkout',$branch);

    # Find all the files different from upstream
    my @changed_files = capture("git diff $UPSTREAM --name-only");
    chomp(@changed_files);

    # Filter out things we shouldn't be packagig.
    my @to_package = grep { not m{$EXCLUDED_FILES_RE} } @changed_files;

    # Write our patch-log
    my $patch_log = capture("git log --oneline $UPSTREAM..HEAD");

    open(my $patchlog_fh, '>', $PATCHLOG_FILE);
    my $date = gmtime();
    say {$patchlog_fh} "# Patches on '$branch' generated at $date GMT\n";
    print {$patchlog_fh} $patch_log;
    close($patchlog_fh);

    # Package!
    my $zipname = "MWDF-patch-$branch.zip";

    # Remove old zip, if exists (CORE::unlink ignores errors)
    CORE::unlink($zipname);

    # Zippity zip!
    systemx('zip','-r', '-q', $zipname, @to_package, $PATCHLOG_FILE);

    # Clean up!
    unlink($PATCHLOG_FILE);

    # Move to export dir

    # TODO: Allow this step to be skipped, for benefit of non-PJF
    # contributors.

    rename($zipname, "$EXPORT_DIR/$zipname");

    return;
}
