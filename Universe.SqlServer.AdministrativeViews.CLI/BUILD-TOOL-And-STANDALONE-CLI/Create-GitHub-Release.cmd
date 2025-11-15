call Set-Environment-Variables.cmd 
if Not Defined BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS Goto :Err
for /f %%i in (%BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\PUBLIC\VERSION.TXT) do set VERSION=%%i

echo VERSION IS %VERSION%
gh release create -t "v%VERSION%" -n "Standalone release v%VERSION%.\n .NET tool is also available: https://www.nuget.org/packages/SqlServer.AdministrativeViews" "v%VERSION%" %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\PUBLIC\*.*
if ERRORLEVEL 1 Goto :err
echo Completed: Release v%VERSION%


if ERRORLEVEL 1 Goto :err
goto :end

:err
echo FAIL. Abort

:end

