#!/usr/bin/perl -w
use 5.010;
use strict;
use warnings;
use autodie qw(:all);
use WWW::Mechanize;
use Markdown::phpBB;
use Config::Tiny;
use File::Slurp qw(read_file);
use autodie qw(read_file);
use FindBin qw($Bin);
use IPC::System::Simple qw(capture);

my $mech = WWW::Mechanize::Urist->new(
    autocheck => 1,
);

my $md2phpbb = Markdown::phpBB->new;

my $config = Config::Tiny->read("$Bin/maint-settings.ini");

my $forumpages = $config->{forumpages};

# Generate latest changes for substitutions.

my $latest_changes = capture("$Bin/forum-patchlog.pl -m");
my $all_changes    = capture("$Bin/forum-patchlog.pl -mA");

# Change to our pages directory and make sure it's up to date
chdir($config->{forum}{pageroot});
system("git pull");

# Log in to Bay12
$mech->login($config->{forum}{user}, $config->{forum}{pass});

# Update all our pages! :)

foreach my $page (keys %$forumpages) {

    say "Updating $page... on $forumpages->{$page}";

    my $url = $forumpages->{$page};

    my $markdown = read_file($page);

    # Subtitute in our changelogs

    $markdown =~ s{ {% \s* latest-changes \s* %} }{$latest_changes}gx;
    $markdown =~ s{ {% \s* all-changes    \s* %} }{$all_changes}gx;

    # Now convert

    my $phpbb    =  $md2phpbb->convert($markdown);

    # Navigate to the post we wish to replace

    $mech->get($url);

    # Enter the text and save! :)

    $mech->form_id('postmodify');
    $mech->field('message',$phpbb);
    $mech->click_button(value => 'Save');
}


BEGIN {
    package WWW::Mechanize::Urist;

    use Digest::SHA qw(sha1_hex);

    our @ISA = qw(WWW::Mechanize);

    sub login {
        my ($this, $user, $pass) = @_;

        $mech->get('http://www.bay12forums.com/smf/');

        my $content = $mech->content;

        # Extract session ID

        my ($session) = ($content =~ /hashLoginPassword\(this, '(.*?)'/ );

        # SMF requires us to do a little crypto dance before we can login.

        my $hash = sha1_hex(sha1_hex(lc($user) . $pass) . $session);

        $mech->submit_form(
            form_id => 'guest_form',
            fields => {
                user => $user,
                hash_passwrd => $hash,
                cookielength => 60,
            },
        );

        return;
    }
}
