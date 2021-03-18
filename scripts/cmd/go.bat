@echo off
REM extrememly hacky way to cd by constructing a batch file 
REM which contains the CD command!
SET scr="%TEMP%\go.bat"
echo|set /p="cd " > %scr%
REM we need to use this odd format to ensure that we change drive as well as directory
jumpfs.exe find --name %1 --format "%%f%%N%%D:" >> %scr%
call %scr%
