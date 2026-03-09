call Set-Environment-Variables.cmd 
if Not Defined BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS Goto :Err
for /f %%i in (%BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\PUBLIC\VERSION.TXT) do set VERSION=%%i

echo SQL Server Administrative Views standalone self-contained release for Windows, Linux, and macOS v%VERSION% > %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes-1st-line.txt
copy /b %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes-1st-line.txt + Github-Release-Notes-Body.md  %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt

gh release delete "v%VERSION%" --cleanup-tag --yes
gh release create -t "v%VERSION%" -F %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt "v%VERSION%" %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\PUBLIC\*.*
if ERRORLEVEL 1 Goto :err
echo Completed: Release v%VERSION%


if ERRORLEVEL 1 Goto :err
goto :end

:err
echo FAIL. Abort

:end

