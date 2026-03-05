@echo off
set NETV=net6.0
dotnet run -c Release -f %NETV% -- -h
echo.
dotnet run -c Release -f %NETV% -- -o "Administrative Views\Local Demo {InstanceName} {Version} on {Platform}\{InstanceName} {Version} on {Platform}" -cs "Data Source=(local); Integrated Security=SSPI; TrustServerCertificate=true; Encrypt=false"
