@echo off
set NETV=net6.0
dotnet run -c Release -f %NETV% -- -h
echo.
set NUNIT_PIPELINE_KEEP_TEMP_TEST_DATABASES=True
dotnet run -c Release -f %NETV% --no-launch-profile -- --mcp-server -cs "Data Source=(local); Integrated Security=SSPI; TrustServerCertificate=true; Encrypt=false" > mcpserver-stdio.tmp
