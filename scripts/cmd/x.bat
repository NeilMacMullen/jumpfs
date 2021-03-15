@echo off
REM extrememly hacky way to call explorer by constructing a batch file 
REM which contains the command!
SET scr="%TEMP%\x.bat"
echo|set /p="explorer.exe " > %scr%
REM we need to use this odd format to ensure that we change directory
jumpfs.exe find -name %1  >> %scr%
call %scr%