@echo off

rem Deleting existing uncompressed backups
cd C:\\"Sql backups"\Unprocessed\
rm -f *.bak

rem Copying down latest uncompressed bak
aws s3 cp s3://mapcall-va-np-bucket/MapCallDevBackups/MCDev.bak C:\\"Sql backups"\Unprocessed\MCDev_%date:~-10,2%%date:~-7,2%%date:~-4,4%.bak
if %errorlevel% neq 0 (
	echo "Error: Could not download zip"
	exit /b %errorlevel%
)

rem Compressing latest uncompressed bak, if we fail here and it stops, we don't remove the old backups
"c:\Program Files\7-Zip\7z.exe" a -v250M -aoa C:\\"Sql backups"\Unprocessed\zip\McDev_%date:~-10,2%%date:~-7,2%%date:~-4,4%.bak C:\\"Sql backups"\Unprocessed\McDev_%date:~-10,2%%date:~-7,2%%date:~-4,4%.bak
if %errorlevel% neq 0 (
	echo "Error: Could not create 7z file"
	exit /b %errorlevel%
)

rem Removing existing compressed backups
cd C:\solutions\nogit\Current
rm -f *

rem Moving new compressed backups into place
move /Y C:\\"Sql backups"\Unprocessed\zip\* .

