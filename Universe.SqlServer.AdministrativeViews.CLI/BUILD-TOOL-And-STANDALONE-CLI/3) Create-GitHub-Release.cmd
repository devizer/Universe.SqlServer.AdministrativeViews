call Set-Environment-Variables.cmd 
if Not Defined BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS Goto :Err
for /f %%i in (%BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\PUBLIC\VERSION.TXT) do set VERSION=%%i

echo VERSION IS %VERSION%
(
  echo SqlServer Administrative Views standalone release v%VERSION%
  echo.
  echo [SqlServer Administrative Views .NET tool]^(https://www.nuget.org/packages/SqlServer.AdministrativeViews^) is also available
) | gh release create -t "v%VERSION%" -F - "v%VERSION%" %BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%\PUBLIC\*.*
if ERRORLEVEL 1 Goto :err
echo Completed: Release v%VERSION%


if ERRORLEVEL 1 Goto :err
goto :end

:err
echo FAIL. Abort

:end

