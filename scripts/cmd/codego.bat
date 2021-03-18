@echo off
REM extrememly hacky way to cd by constructing a batch file 
REM which contains the command we want to run!
SET scr="%TEMP%\codego.bat"
echo|set /p="code --goto " > %scr%
REM we need to use this odd format to ensure that we change directory
jumpfs.exe find --name %1 --format "%%p:%%l%%c" >> %scr%
call %scr%
