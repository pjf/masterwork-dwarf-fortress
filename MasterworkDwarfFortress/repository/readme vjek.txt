Information about each script

[edit]brainwash
The brainwash script modifies the personality traits of a single dwarf to match an ideal personality. What ideal means will doubtless vary from person to person, but I created my ideal based on the goal that the dwarf would be as stable and reliable as possible.
An interesting result of my ideal personality values is that dwarves without clothing are remarkably resistant to tantrums. In fact, I had an entirely naked fort for many months, and while all the dwarves were at zero happiness, there were no tantrums, no fights, no deaths. So, brainwashing works on dwarves!
Modifying the script is straightforward, just adjust the values you wish by each personality trait. Subsequent adjustments will overwrite any previous adjustments, and you can adjust a dwarf as many times or as often as you wish, as far as I call tell.
You could also make a very "unstable" and "unreliable" dwarf by modifying the values in the script, which has all sorts of entertainment potential.



[edit]elevate_mental
This script will adjust all the mental attributes of a single dwarf from whatever they currently are to the value 2600. While 2600 is not the maximum, it's very high, and you'll find this is high enough for most common activities. To adjust this value, pass the new value to the script as an argument. For example: 'elevate_mental 2700' This script can also be used to LOWER all the mental attributes of a particular dwarf. Use 'elevate_mental 200' for example. Yes, I know, it's not really "elevating" if you're lowering a value, but meh, the name is the name, change it if you want.



[edit]elevate_physical
This script will adjust all the physical attributes of a single dwarf from whatever they currently are to the value 2600. This value can also be changed by passing an argument to the script such as 'elevate_physical 3000' to adjust them all to 3000 instead of 2600. As with the elevate_mental script, this script can also be used to LOWER all the physical attributes of a particular dwarf. . I have had the need to do this, for example, with pesky nobles who insist on beating the hell out of their fellow dwarves. Not so easy to beat someone with a strength of 10, now is it?! Hee hee.



[edit]make_legendary
This script will make a single dwarf legendary in either a single skill, a group of skills, or all skills. Skills are things like Carpentry, Mining, Dodging, Lying, and so forth. Each skill has a category or class, and those classes can be seen by passing the 'classes' argument to the script. Similarly, passing 'list' to the script will show all the available skills. Specifying a single skill such as 'make_legendary MINING' will make that single skill Legendary +5. Presto! You have a legendary miner, go forth, mine legendarily!
The classes are broken up into fairly predictable groups, the most common will be the 'Normal' class which includes all the non-military and non-social skills like Mining, Carpentry, Strand Extraction, Cheese Making, Furnace Operating, and so on. Using the script thusly 'make_legendary Normal' will make the selected dwarf legendary in ALL those skills.
Speaking of all, the 'all' argument will make your dwarf legendary in every single skill there is, including military, social, medical, and normal. 'make_legendary all' and voila, truly a dwarf of legends!



[edit]rejuvenate
I wrote this script while doing some very long term experiments without births or migrants, and didn't want my dwarves dying on me. It's very simple, it just checks to see if a dwarf is older than 20 years old, and if they are, it makes them 20 years old. It does not work on children. You can try to use it on children, it just won't do anything.
[edit]armoks_blessing
This script combines the previous scripts into one that affects all dwarves currently in play. If the script is executed without any arguments, it runs the equivalent of rejuvenate, elevate_physical, elevate_mental, and brainwash on all dwarves currently on the map. It will only rejuvenate dwarves 21 years or older.
If the script has been copied into /df_install_dir/hack/scripts/ , you'll see the following
[DFHack]# ls
...
scripts:
  armoks_blessing       - Adjust all attributes, personality, age and skills of all dwarves in play
Now running the script by itself
[DFHack]# armoks_blessing
No skillname supplied, no skills will be adjusted.  Pass argument 'list' to see a skill list, 'classes' to show skill classes, or 
use 'all' if you want all skills legendary.
Adjusting Nil Laruzol
Adjusting Sibrek Sarveshtangath
Adjusting Aban Oltarmörul
Adjusting Olon Kolakmam
Adjusting Edzul Isonzas
Adjusting Ast Mamotiden
Adjusting Doren Aranònul
[DFHack]#
I ran that on a new embark with just the starting seven. What it did:
-changed all the personality values to "an ideal", the specifics of which are in the code.
-adjusted all mental and physical attributes to 2600.
-reduced the age of all dwarves older than 20 to age 20.
The script will accept arguments. Use 'list' or 'classes' to see them, but the most common ones will be:
all
Normal
MilitaryDefense
MilitaryAttack
MINING
CARPENTRY
...
all - this makes all your dwarves legendary in all skills. All. Every single one. Normal - this makes your dwarves legendary in the common labour skills plus a few extras (exactly shown with 'list' ) MilitaryDefense - the defensive military skills (dodge, armor, shield) are all made legendary MilitaryAttack - the offensive military skills (weapons, fighting, biting, etc) are all made legendary. MINING - all dwarves are made into legendary miners.
Example:
[DFHack]# armoks_blessing CARPENTRY
Adjusting Nil Laruzol
nil is now a Legendary Carpenter
Adjusting Sibrek Sarveshtangath
sibrek is now a Legendary Carpenter
Adjusting Aban Oltarmörul
aban is now a Legendary Carpenter
Adjusting Olon Kolakmam
olon is now a Legendary Carpenter
Adjusting Edzul Isonzas
edzul is now a Legendary Carpenter
Adjusting Ast Mamotiden
ast is now a Legendary Carpenter
Adjusting Doren Aranònul
doren is now a Legendary Carpenter
[DFHack]#
Another example:
[DFHack]# armoks_blessing Normal
Adjusting Nil Laruzol
Skill Mining is type: Normal and is now Legendary for nil
...
Skill Wax Working is type: Normal and is now Legendary for nil
Adjusting Sibrek Sarveshtangath
Skill Mining is type: Normal and is now Legendary for sibrek
...
Skill Wax Working is type: Normal and is now Legendary for sibrek
Adjusting Aban Oltarmörul
Skill Mining is type: Normal and is now Legendary for aban
...
Skill Wax Working is type: Normal and is now Legendary for aban
Adjusting Olon Kolakmam
Skill Mining is type: Normal and is now Legendary for olon
...
Skill Wax Working is type: Normal and is now Legendary for olon
Adjusting Edzul Isonzas
Skill Mining is type: Normal and is now Legendary for edzul
...
Skill Wax Working is type: Normal and is now Legendary for edzul
Adjusting Ast Mamotiden
Skill Mining is type: Normal and is now Legendary for ast
...
Skill Wax Working is type: Normal and is now Legendary for ast
Adjusting Doren Aranònul
Skill Mining is type: Normal and is now Legendary for doren
...
Skill Wax Working is type: Normal and is now Legendary for doren
[DFHack]#
To reiterate: the previous scripts work on single dwarves, this one works on all the dwarves currently on the map. You should be REALLY REALLY sure you want to affect ALL YOUR DWARVES before you even copy this script into your /hack/scripts/ directory, and you should definitely have a backup/save ready in case you make changes you weren't expecting.

[edit]pref_adjust for DFHack 34.11r2/r3
This is a simplified and better version of the previous script. Everything in one step (no need to clear first), adjustment is as follows:
Feb Idashzefon likes weapons, armor, shields/bucklers, plump helmets for their rounded tops, iron and steel. When possible, she prefers to consume dwarven wine. She absolutely detests trolls, buzzards, vultures, and crundles.
Sample use output:
[DFHack]# pref_adjust
Clearing Preferences for Sodel ïtebulåb
Clearing Preferences for Sibrek Idnônub
Clearing Preferences for Atír Kanbesmar
Clearing Preferences for Tobul Shorastorstist
Clearing Preferences for Ustuth Kûbukzokun
Clearing Preferences for Etur Bomrekkifed
Clearing Preferences for Doren Duthnurudib
Adjusting Sodel ïtebulåb
Before, unit Sodel ïtebulåb has 0 preferences
After, unit Sodel ïtebulåb has 11 preferences
Adjusting Sibrek Idnônub
Before, unit Sibrek Idnônub has 0 preferences
After, unit Sibrek Idnônub has 11 preferences
Adjusting Atír Kanbesmar
Before, unit Atír Kanbesmar has 0 preferences
After, unit Atír Kanbesmar has 11 preferences
Adjusting Tobul Shorastorstist
Before, unit Tobul Shorastorstist has 0 preferences
After, unit Tobul Shorastorstist has 11 preferences
Adjusting Ustuth Kûbukzokun
Before, unit Ustuth Kûbukzokun has 0 preferences
After, unit Ustuth Kûbukzokun has 11 preferences
Adjusting Etur Bomrekkifed
Before, unit Etur Bomrekkifed has 0 preferences
After, unit Etur Bomrekkifed has 11 preferences
Adjusting Doren Duthnurudib
Before, unit Doren Duthnurudib has 0 preferences
After, unit Doren Duthnurudib has 11 preferences
The current syntax within the script should make it far easier to add/customize what you want your dwarves to like or dislike.
[edit]How do I install and use the scripts?

[edit]Installation
Happily, this is the easy part. Install Dwarf Fortress. Install DFHack. Copy the scripts into the /hack/scripts/ directory. That means you click on the link to the script, and save that file into that directory.
So, if you clicked on the rejuvenate link, for example, it would show you the rejuvenate.lua source code file. In your browser, save that page as, browse to your DF installation directory, browse to /hack/, browse to /scripts/ and save the file there.
If all went well, in the DFHack command line interface, if you use the 'ls' command, it will enumerate, near the bottom, all the scripts in the /hack/scripts/ directory. If rejuvenate shows up there, you're set!
[edit]Use
If you installed the .lua script files into /hack/scripts/, you can simply type the script name, without the .lua extension, and it will be executed/run.
Alternately, you can use the 'lua' command in DFHack, and specify the exact path to the script. For example, 'lua c:\dfhack_scripts\rejuvenate.lua' if you wanted to keep your scripts separate from the dfhack directory structure for some reason.
Arguments are options passed to the script when you call it. So, something like 'elevate_physical 2700' would pass the argument 2700 to the script elevate_physical when it runs. Similarly, as mentioned above, 'lua c:\dfhack_scripts\elevate_physical.lua 2700' would also work.
For those using the scripts (or DFHack) for the first time, I highly recommend you save (or save & copy/backup) your game before you use the scripts. It is easy to make changes you may not have intended to make, and having a backup in this case is a good idea.
Most of the scripts require a dwarf to be a target within the main DF interface. This means you pause the game, press 'k', and move the cursor so it is on top of the dwarf you want. If there are multiple dwarves on a single tile, whichever one is selected in the list is the target of the script. Once the target is selected, alt-tab to the DFHack command line interface, type in the name of the script (with any optional arguments) and press enter. If all goes well, your dwarf will be modified, you can unpause the game and continue.
[edit]Bugs?