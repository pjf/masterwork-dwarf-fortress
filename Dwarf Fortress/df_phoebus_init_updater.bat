@ECHO Off
ECHO -------------------------------------------------------------
ECHO - Welcome to the Dwarf Fortress (TrueType:On) INIT Updater. -
ECHO -------------------------------------------------------------
ECHO .
ECHO Press Control+C to cancel the Update
PAUSE
GOTO :start

:start
ECHO .
ECHO .
FOR %%R IN (data\init\*.txt) DO IF %%~zR EQU 0 DEL "%%R"
FOR %%R IN (data\init\*.phoebus.txt) DO DEL "%%R"
FOR %%R IN (data\init\*.old.txt) DO DEL "%%R"
FOR %%R IN (data\init\phoebus\*.txt) DO IF %%~zR EQU 0 DEL "%%R"
COPY "data\init\phoebus\*.txt" "data\init" /V /Y
if ERRORLEVEL 1 GOTO :error
GOTO :success

:success
ECHO .
ECHO .
ECHO --------------------------------------------------
ECHO - Update Completed.                              -
ECHO - Remember to also run the Savegame RAW Updater. -
ECHO --------------------------------------------------
PAUSE
GOTO :end

:error
ECHO .
ECHO .
ECHO --------------------------------------------------
ECHO - Dwarf Fortress INIT Updater has failed.        -
ECHO - Please copy the content of "data\init\phoebus" -
ECHO - into "data\init" manually.                     -
ECHO --------------------------------------------------
PAUSE
GOTO :end

:end
