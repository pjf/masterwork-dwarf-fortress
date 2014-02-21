Simple command line tool for tinkering with regions after world generation.
Export and import all RAW related data from uncompressed world.dat and world.sav files.
(still working on the sync command, for those who remember it)

Drop in the DF folder, open a command prompt, and run it.

Usage:
dfworldtinker <region> <export|import|sync> <file>

Examples:
dfworldtinker region1 export -
dfworldtinker 1 export exportfile.txt
dfworldtinker 1 import modifiedfile.txt


All parameters are required.
Regions can either be entered with their full name, or just as their number for convenience.
Entering a single dash for the input/output file reads from or writes to terminal.
If no commands given, enters a very simple interactive mode.
Commands can be the first letter, e.g. "e" or "i".

* Only able to edit non-compressed saved games and regions. See your init file. *

Official Supported DF versions:
0.34.11