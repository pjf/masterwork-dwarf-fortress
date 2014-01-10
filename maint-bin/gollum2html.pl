#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use IPC::Open2;
use Config::Tiny;
use FindBin qw($Bin);
use File::Copy qw(copy);
use autodie qw(:all);
use autodie qw(copy);

use constant DEBUG => 0;

# Script to general orc fortress pages from Gollum/Markdown
# Paul '@pjf' Fenwick, Jan 2014
# Distributed under the same terms as Perl itself.

my $HEADER = q{<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transition//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head><link rel="stylesheet" type="text/css" href="style.css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Orc Fortress</title>
<body>
};

my $FOOTER = q{</body></html>};
my $SIDEBAR = "_Sidebar";       # Sidebar file name

my $config = Config::Tiny->read("$Bin/maint-settings.ini");

my $from_dir = $config->{manual}{from};
my $to_dir   = $config->{manual}{to};

say "Reading from $from_dir" if DEBUG;
say "Writing to $to_dir"     if DEBUG;

# Make sure our from repo is up-to-date
say "Syncing git...";
chdir($from_dir);
system("git pull");
say "";

# Now find our files and generate our manual. :)

my @markdown_files = glob("$from_dir/*.md");

foreach my $from_file (@markdown_files) {
    $from_file =~ m{/(?<basename>[^/]+).md$} or die "Odd filename: $from_file";

    my $to_file = "$to_dir/$+{basename}.html";

    say "...$+{basename}";

    write_html($from_file,$to_file, $+{basename});
}

say "Done!\n" if DEBUG;

sub write_html {
    my ($from, $to, $basename) = @_;

    open (my $from_fh, '<', $from);
    open (my $to_fh,   '>', $to);

    say {$to_fh} $HEADER;

    # Add a title, if not a sidebar
    if ($basename ne $SIDEBAR) {
        say {$to_fh} "<h1>$basename</h1>";
    }

    # Connect to markdown for processing
    my ($md_fh, $html_fh);

    open2($html_fh, $md_fh, 'markdown -');

    my $text;

    # Read the whole file in and fix-up

    {
        local $/;
        $text = fixup( <$from_fh> , $basename);
    }

    # Send to markdown
    print {$md_fh} $text;
    close($md_fh);

    # Send our formatted text to the output file

    copy($html_fh, $to_fh);

    say $to_fh $FOOTER;
}

sub fixup {
    my ($text, $basename) = @_;
    local $_ = $text;

    # Fix links to images
    s{\[\[(.*)\.(png|jpg)\]\]}{<img src="$1.$2">}g;

    # Fix internal links
    s{
        \[\[
        (?<title>   [^\]|]+ )
        (?:\| (?<link>  [^\]]+  ))?
        \]\]
    }
    {linkify($+{title},$+{link},$basename)}msxge;

    # Fix code blocks
    s{```(.*?)```}{<pre>$1</pre>}msg;
    
    return $_;
}

sub linkify {
    my ($title, $link, $file) = @_;

    $link ||= $title;

    if ($file eq $SIDEBAR) {
        # Sidebar links refer to the other frame

        return qq{<a href="$link.html" target="page">$title</a>};
    }

    return qq{<a href="$link.html">$title</a>};
}
