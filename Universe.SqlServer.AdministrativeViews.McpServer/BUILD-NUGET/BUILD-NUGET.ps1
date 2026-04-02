del bin/*
. ..\..\Build-Scripts\Build.ps1

$serverJsonPath = "..\.mcp\server.json"


# Using --arg to safely pass $VERSION and 'walk' for 1.6+ compatibility
& jq --arg ver "$VERSION" '.version = $ver' $serverJsonPath | Out-File "$serverJsonPath.1.tmp" -Encoding UTF8
$exitCode1 = $Global:LASTEXITCODE
& jq --arg ver "$VERSION" '.packages[0].version = $ver' "$serverJsonPath.1.tmp" | Out-File "$serverJsonPath.2.tmp" -Encoding UTF8
$exitCode2 = $Global:LASTEXITCODE


if ($exitCode1 -eq 0 -and $exitCode2 -eq 0) {
    Write-Host "Success: Updated version to '$VERSION' in '$serverJsonPath'" -ForegroundColor Green
} else {
    $errorMessage = "Fail: Could not update version to '$VERSION' in '$serverJsonPath'"
    Write-Host $errorMessage -ForegroundColor Red
    throw $errorMessage
}

Copy-Item -Path "$serverJsonPath.2.tmp" -Destination $serverJsonPath -Force

& dotnet pack ..\Universe.SqlServer.AdministrativeViews.McpServer.csproj  -c Release -o "bin" -v:q /p:NoWarn=NETSDK1215 /p:Version="$VERSION" /p:FileVersion="$VERSION" /p:AssemblyVersion="$VERSION" /p:PackageVersion="$VERSION"
