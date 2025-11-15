set BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS=M:\Temp\Universe.SqlServer.AdministrativeViews.CLI-BUILD
rd /q /s "%BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS%"
powershell -f Build-CLI-Standalone.ps1
