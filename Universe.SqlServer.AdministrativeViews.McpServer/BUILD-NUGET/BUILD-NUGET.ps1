del bin/*
. ..\..\Build-Scripts\Build.ps1

$serverJsonPath = "..\.mcp\server.json"

# Using --arg to safely pass $VERSION and 'walk' for 1.6+ compatibility
$json1 = jq --arg ver "$VERSION" '.version = $ver' $serverJsonPath
$exitCode1 = $Global:LASTEXITCODE
[System.IO.File]::WriteAllLines("$serverJsonPath.1.tmp", $json1)
$json2 = jq --arg ver "$VERSION" '.packages[0].version = $ver' "$serverJsonPath.1.tmp"
$exitCode2 = $Global:LASTEXITCODE
[System.IO.File]::WriteAllLines("$serverJsonPath.2.tmp", $json1)


if ($exitCode1 -eq 0 -and $exitCode2 -eq 0) {
    Write-Host "Success: Updated version to '$VERSION' in '$serverJsonPath'" -ForegroundColor Green
} else {
    $errorMessage = "Fail: Could not update version to '$VERSION' in '$serverJsonPath'"
    Write-Host $errorMessage -ForegroundColor Red
    throw $errorMessage
}

Copy-Item -Path "$serverJsonPath.2.tmp" -Destination $serverJsonPath -Force

& dotnet pack ..\Universe.SqlServer.AdministrativeViews.McpServer.csproj  -c Release -o "bin" -v:q /p:NoWarn=NETSDK1215 /p:Version="$VERSION" /p:FileVersion="$VERSION" /p:AssemblyVersion="$VERSION" /p:PackageVersion="$VERSION"
