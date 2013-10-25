@echo off

:SET_CLASSPATH

set CLASSPATH=.\
set CLASSPATH=%CLASSPATH%;lib\MP3SPI\mp3spi1.9.5.jar;lib\MP3SPI\jl1.0.1.jar
set CLASSPATH=%CLASSPATH%;lib\OGGSPI\jogg-0.0.7.jar;lib\OGGSPI\jorbis-0.0.15.jar;lib\OGGSPI\vorbisspi1.0.3.jar
set CLASSPATH=%CLASSPATH%;lib\tritonus_share.jar
set CLASSPATH=%CLASSPATH%;lib\commons-codec-1.4\commons-codec-1.4.jar
set CLASSPATH=%CLASSPATH%;lib\autoUpdater.jar
set CLASSPATH=%CLASSPATH%;lib\jansi-1.8.jar
set CLASSPATH=%CLASSPATH%;soundSense.jar

java -Djava.util.logging.config.file=logging.properties -cp %CLASSPATH% cz.zweistein.df.soundsense.SoundSense

cmd -k