DFAnnouncementFilter v1.01

CHANGELOG:
-Bugfix: Changing the path should now should work without needing to restart the program now.
-Bugfix: log-region plugin setting can actually be turned off and on now.
-Added: Text to the filter setting screen to make it's functionality clearer.

Requires a JRE (Java Runtime Environment) to run, get the latest one here : 
http://www.oracle.com/technetwork/java/javase/downloads/index.html

Make sure you grab the regular JRE and not the server JRE or JDK.

Usage :
Just run it and it will show you a live feed of announcements in a running Df game,
according to whatever filter settings you have set. 

DFHack plugin support:
Currently only supports the log-region plugin (it's the only one I could find that actually
writes anything to the gamelog, which is what this program reads announcements from).

If you have a plugin that adds announcements and you want them supported, make it so the 
plugin will also write the announcement to the gamelog.txt file, let me know what it writes,
and I'll add support for it.

Custom filters:
You can add your own filters. The way this works is you specify a particular bit of text
that you want it to KEEP, so any line in the gamelog.txt which is found that contains the 
specified text will be displayed.

Note that this will only work with exact matches, so if you add a filter that says 
"herp derp", and the gamelog has a line that says "herp da derp", it will NOT be matched.

Questions/Concerns/Comments/Suggestions/Bugs:
Please email to dfstorymaker@gmail.com, or send a pm to TheGazelle on Reddit or the Bay12
forums, or just post in the Bay12Thread here : http://www.bay12forums.com/smf/index.php?topic=130030.0