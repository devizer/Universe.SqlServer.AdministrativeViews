call Set-Environment-Variables.cmd 
if Not Defined BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS Goto :Err

pushd ..
rd /q /s "%BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%"
rd /q /s "bin"
powershell -f BUILD-TOOL-And-STANDALONE-CLI\Build-CLI-NetFramework.ps1 
if ERRORLEVEL 1 Goto :err
rem ON LINUX!!!! for +x
powershell -f BUILD-TOOL-And-STANDALONE-CLI\Build-CLI-Standalone.ps1
if ERRORLEVEL 1 Goto :err
goto :end

:err
echo FAIL. Abort

:end
popd