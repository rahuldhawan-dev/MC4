tasklist /FI "IMAGENAME eq iexplore.exe" 2>NUL | find /I /N "iexplore.exe">NUL
if "%ERRORLEVEL%" == "0" taskkill /IM iexplore.exe /f