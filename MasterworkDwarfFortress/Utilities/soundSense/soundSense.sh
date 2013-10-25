#!/bin/sh
dir=${0%/*}
if [ -d "$dir" ]; then
 cd "$dir"
fi
CLASSPATH='soundSense.jar:lib/MP3SPI/mp3spi1.9.5.jar:lib/MP3SPI/jl1.0.1.jar:lib/OGGSPI/jogg-0.0.7.jar:lib/OGGSPI/jorbis-0.0.15.jar:lib/OGGSPI/vorbisspi1.0.3.jar:lib/tritonus_share.jar:lib/autoUpdater.jar:lib/jansi-1.8.jar:lib/commons-codec-1.4/commons-codec-1.4.jar'
if [ -e /usr/bin/aoss/ ] ; then
 aoss java -Djava.util.logging.config.file=logging.properties -cp $CLASSPATH cz.zweistein.df.soundsense.SoundSense
else
 java -Djava.util.logging.config.file=logging.properties -cp $CLASSPATH cz.zweistein.df.soundsense.SoundSense
fi