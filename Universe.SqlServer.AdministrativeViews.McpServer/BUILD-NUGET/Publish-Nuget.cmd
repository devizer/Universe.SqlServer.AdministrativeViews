set /p apikey=<\Cloud\vg\PUTTY\SqlInsights-Private\apikey
dotnet nuget push bin\*.nupkg --api-key %apikey% --source https://api.nuget.org/v3/index.json