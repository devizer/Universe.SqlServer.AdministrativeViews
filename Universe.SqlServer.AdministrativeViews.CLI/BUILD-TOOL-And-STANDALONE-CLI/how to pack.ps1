. ..\Build-Scripts\Build.ps1 
# & dotnet clean
Say "BUILD"
$buildArgs = @(
    'build',
    '-c', 'Release',
    "/p:Version=$VERSION",
    "/p:AssemblyVersion=$VERSION",
    '/p:TargetFrameworks=net8.0%3Bnet6.0%3Bnet10.0'
)
# & dotnet @buildArgs
& C:\Apps\dotnet-10\dotnet.exe build "/p:Version=$VERSION" "/p:AssemblyVersion=$VERSION" -c Release
Say "Skip PACK"
# & dotnet pack "/p:Version=$VERSION" "/p:AssemblyVersion=$VERSION" -c Release -- '/p:TargetFrameworks="net8.0;net6.0"'
