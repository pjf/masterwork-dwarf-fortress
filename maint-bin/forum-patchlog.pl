#!/usr/bin/perl
use v5.10.0;
use strict;
use warnings;
use autodie;
use IPC::System::Simple qw(capture systemx);
use Getopt::Std;
use FindBin qw($Bin);
use File::Spec;

=head1 NAME

forum-patchlog.pl

=head1 SYNPOSIS

    $ ./forum-patchlog.pl       # Show BB code
    $ ./forum-patchlog.pl -T    # Show text

=head1 DESCRIPTION

This program shows changes made which have not been merged with the
'master' branch, or the 'upstream' branch if our current branch is
already master.

By default, changes are shown with BB code mark-up linking to github.
The C<-T> switch shows them in plain text format.

If a F<forum-patchlog.exclude> file exists in the same directory as
this script, then any git commits mentioned in it are ignored. Branch
merges, pull requests, messages starting with 'maint-bin:', and
anything wit '#dev' in the commit line are automatically excluded.

=head1 LICENSE

Same as Perl itself.

=head1 AUTHOR

Paul Fenwick (pjf@cpan.org)

=cut

my %opts = (
    T => 0, # 'text', show without markup
    a => 0, # 'all', always show since upstream
    A => 0, # 'ALL', show everything ever
);

getopts('TaA',\%opts);

my $REPO_ROOT = "https://github.com/pjf/masterwork-dwarf-fortress";

my $ORIG_COMMIT = "70ba14c57178ef7b86133281e6d98300de2234ba";

my $now_branch = capture('git rev-parse --abbrev-ref HEAD');
chomp($now_branch);

my $parent = ($now_branch eq 'master') ? "upstream" : "master";

# -a (all) forces upstream
# -A (ALL) forces everything

if ($opts{a}) { $parent = 'upstream' }
if ($opts{A}) { $parent = '70ba14c57178ef7b86133281e6d98300de2234ba' }

my $EXCLUDED_COMMIT_RE = qr{(?:
      Merge\ branch\ '\w+'
    | Merge\ pull\ request
    | Merge\ remote-tracking\ branch
    | maint-bin:
    | export-patches
    | .gitignore
    | lint-raws
    | yoink-master
    | patch-graph?ics-from-master
    | show-tileset-diffs
    | .* \#dev\b
)}msx;

# Get our patches

my @patch_log = 
    grep { not /^\w+ $EXCLUDED_COMMIT_RE/ } capture(qq{git log --no-merges --pretty=format:"%h %s (%aN)" $parent..HEAD});
;

chomp @patch_log;

# Exclude ones we've marked as not to include in the output

my $exclude_file_re;

{
    open(my $exclude_fh, '<',
        File::Spec->catdir($Bin, 'forum-patchlog.exclude')
    );

    my @excluded_commits;

    while (<$exclude_fh>) {
        chomp;

        next if /^\s*$/;    # Skip blank lines
        next if /^\s*#/;    # Skip comments;

        s{^(\w+).*}{$1};      # Convert to just git ID

        push(@excluded_commits, $_);
    }

    # Build regexp
    $exclude_file_re = join('|',@excluded_commits);
}

# Print our changes

say "[list]" if not $opts{T};

foreach (@patch_log) {
    # Ignore anythig in our exclude file
    next if /$exclude_file_re/;

    if ($opts{T}) {
        # Text mode, print raw
        say;
    }
    else {
        # Add BB markup
        my ($id, $msg) = /^(\w+) (.*)/;
        say "[li][url=$REPO_ROOT/commit/$id]${msg}[/url][/li]";
    }
}

say "[/list]" if not $opts{T};
