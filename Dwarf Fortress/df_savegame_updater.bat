@ECHO Off
ECHO -------------------------------------------------------
ECHO - Welcome to the Dwarf Fortress Savegame RAW Updater. -
ECHO -------------------------------------------------------
ECHO .
ECHO Press Control+C to cancel the Update
PAUSE
goto start


:update_savegame
if %1=="data\save\current" GOTO :eof
ECHO ------------------------------------------
ECHO Updating Savegame: %~1
IF EXIST "%~1\raw\graphics" RD /s/q "%~1\raw\graphics"
FOR %%R IN ("%~1\raw\objects\*.bak") DO DEL "%%R"
XCOPY raw "%~1\raw" /c /v /q /i /s /e /y
GOTO :eof


:start
FOR %%R IN (raw\graphics\*.txt) DO IF %%~zR EQU 0 DEL "%%R"
FOR %%R IN (data\art\TilesetAssembler\*.png) DO IF %%~zR EQU 0 DEL "%%R"
FOR %%R IN (raw\objects\*.bak) DO DEL "%%R"
FOR /d %%a IN (data\save\*.*) DO CALL :update_savegame "%%a"

ECHO .
ECHO .
ECHO ------------------------------------------
ECHO - Update Completed.                      -
ECHO - Remember to also run the INIT Updater. -
ECHO ------------------------------------------
PAUSE
GOTO :eof