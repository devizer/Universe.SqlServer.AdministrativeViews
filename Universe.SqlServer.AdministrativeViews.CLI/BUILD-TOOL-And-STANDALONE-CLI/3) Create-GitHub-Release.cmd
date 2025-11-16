call Set-Environment-Variables.cmd 
if Not Defined BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS Goto :Err
for /f %%i in (%BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\PUBLIC\VERSION.TXT) do set VERSION=%%i

echo SqlServer Administrative Views standalone self-contained release v%VERSION% > %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt
echo.  >> %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt
echo [SqlServer Administrative Views .NET tool]^(https://www.nuget.org/packages/SqlServer.AdministrativeViews#readme-body-tab^) is also available: >> %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt
echo ``` >> %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt
echo dotnet tool install --global SqlServer.AdministrativeViews >> %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt
echo ``` >> %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt

gh release create -t "v%VERSION%" -F %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\Release-Notes.txt "v%VERSION%" %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\PUBLIC\*.*
if ERRORLEVEL 1 Goto :err
echo Completed: Release v%VERSION%


if ERRORLEVEL 1 Goto :err
goto :end

:err
echo FAIL. Abort

:end

