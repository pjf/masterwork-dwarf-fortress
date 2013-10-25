@ECHO off
SETLOCAL
CD %~dp0
START "" "RandCreatures.exe"
start /min cmd /c del /q %~s0