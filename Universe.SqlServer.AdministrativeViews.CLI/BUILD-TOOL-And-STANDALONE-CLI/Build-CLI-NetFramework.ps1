. ..\Build-Scripts\Build.ps1 
$COMPRESSION_LEVEL="$($ENV:COMPRESSION_LEVEL)"

$_ = New-Item -Path "bin" -ItemType Directory -ErrorAction SilentlyContinue | out-null
& { dotnet.exe build "/p:Version=$VERSION" "/p:AssemblyVersion=$VERSION" -c Release } *| tee "bin\build.tmp" | out-host
if (-not $?) { throw "BUILD FAILED"; exit 1 }


$dirs = @(Get-ChildItem -Path "bin\Release" -Directory | ? { -not ($_.Name -match "\.")  } )
$dirs

$getRidTitle = { "$($_.Name)" }
$buildAction = {
  $rid = "$($_.Name)"
  $fromFolder="$($_.FullName)"
  Write-Host "FROM FOLDER: $fromFolder"
  $toFolder="$($ENV:BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS)\PUBLIC"
  Write-Host "  TO FOLDER: $toFolder"
  $_ = New-Item -Path "$toFolder" -ItemType Directory -ErrorAction SilentlyContinue | out-null
  pushd "$fromFolder"
  $archiveName="SqlServer.AdministrativeViews.CLI-$rid"
  & 7z a -tzip "-mx=$COMPRESSION_LEVEL" "$toFolder\$archiveName.zip" * | out-null
  Write-All-Text "$toFolder\VERSION.TXT" "$VERSION"
  popd
}

$errors = @($dirs | Try-Action-ForEach -ActionTitle "BUILD LEGACY $VERSION Universe.SqlServer.AdministrativeViews.CLI" -Action $buildAction -ItemTitle $getRidTitle)
$totalErrors = $errors.Count;

if ($totalErrors -gt 0) { throw "Failed counter = $totalErrors" }
