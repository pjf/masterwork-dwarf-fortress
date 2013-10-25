Urist Vespasian, Arena Editor, v1.0
by ManaUser

Okay, strictly speaking, this doesn't really edit the arena, but it 
does make that task a whole lot easier. What it actually does is 
translate the arena text file into a bitmap image, which can be 
edited in almost any image editor (even MS Paint). Then of course it 
can turn that image back into a valid arena text file.

HOW TO INSTALL

Just extract UristV.exe to the same directory as dwarfort.exe... 
actually you can put it anywhere, but you'll have to browse to find 
your arena.txt file manually. UristV.ahk is the source code in a 
language called AutoHotkey. If you like, you can get it (free) at 
http://www.autohotkey.com/ otherwise, just ignore that file. :)

I've also included two sample arenas, one as a bitmap, one as text.

USING THIS PROGRAM

Step 1. Click TXT->BMP, you will be prompted first to select an arena 
text file (such as the original arena.txt), then to enter a name for 
the bitmap to be created. Then, after a short but annoying wait, your 
file is ready. If you checked "Auto Open" the new file will 
automatically open in the associated application as soon as it's done.

Step 2. Edit the image. More on this below.

Step 3. Click BMP->TXT, just like step one in reverse, so select your 
edited bitmap, enter/select the name of the text file to save it to. 
It will no doubt occur to savvy users that if they wish to save 
directly over the original arena.txt it would be a good idea to back 
it up before completing this step.

PLAYING AN ARENA

In order to play an arena you created, you will have to replace the 
original arena.txt file in the data\init subfolder of your Dwarf 
Fortress installation. Then just start Dwarf Fortress and enter the 
"Object Testing Arena" as normal.

EDITING THE IMAGE

You will see a narrow but very tall bitmap image. Just the like arena 
text file itself, it will be divided vertically into 9 sections 
corresponding to Z-levels -4 to 4. Each section is 144x144 pixels, 
representing an equal number of tiles. The white lines between them 
are for your reference only.

It's a paletted image, using the following colors:

      Black = Empty Space
  Dark Gray = Floor
Medium Gray = Wall (Also comes with a floor on top)
 Light Gray = Ramp (Up)
      Brown = Fortification 
       Cyan = Water Source (Edge of map only)
       Blue = Water
 Light Blue = Water with a Ramp in it
        Red = Magma
  Light Red = Magma with a Ramp in it
      White = Grid Lines (Just empty space if used on map)

See the wiki for more information about how all these work:
http://df.magmawiki.com/index.php/DF2010:Arena

LIMITATIONS

Neither Dwarf Fortress nor UristV support arena sizes other than 
144x144x9, so don't bother trying. Also I warn you that this program 
does virtually no error checking, the result is provided on a strictly 
"Garbage In Garbage Out" basis. DF may hang if you try to load a 
malformed arena. Additionally UristV is very simply-minded in its 
bitmap reading. The image must have the same palette as those that 
come out of this program. If you use a full color bitmap, or one in 
some other palette (even the same colors in a different order)... well
It's going to suck, 'nuff said.

Finally, even a properly encoded arena file can cause DF to hang in 
some cases. Exactly what those cases are needs more investigation, but 
one thing that did it for me was suspending large amounts of lava over 
the map. (Don't worry you can still do that, it may just take you a 
few tries to find a configuration DF can handle. In fact one of sample 
maps I included is just that.)