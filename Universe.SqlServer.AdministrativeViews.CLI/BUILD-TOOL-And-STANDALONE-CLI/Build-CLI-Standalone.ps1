. ..\Build-Scripts\Build.ps1 
$COMPRESSION_LEVEL="$($ENV:COMPRESSION_LEVEL)"

$rids="linux-musl-arm64 osx-x64 osx-arm64 win-x64 win-x86 win-arm64 linux-x64 linux-arm linux-arm64 linux-musl-x64".Split(' ')
$rids

$getRidTitle = { "$_" }
$buildAction = {
  $rid = $_
  $toFolder="$($ENV:BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS)\$rid"
  $tmpFolder="$($ENV:BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS)\INTERMEDIATE"
  $_ = New-Item -Path "bin\self-contained" -ItemType Directory -ErrorAction SilentlyContinue | out-null
  $_ = New-Item -Path "$tmpFolder" -ItemType Directory -ErrorAction SilentlyContinue | out-null
  & { dotnet.exe publish "/p:Version=$VERSION" "/p:AssemblyVersion=$VERSION" -c Release -f net8.0 --self-contained -r $rid -o "$toFolder" } *| tee "$tmpFolder\build.$rid.log" | out-host
  pushd $toFolder
  $archiveName="SqlServer.AdministrativeViews.CLI-$rid"
  if ($rid -like "win*") {
    & 7z a -tzip "-mx=$COMPRESSION_LEVEL" "..\PUBLIC\$archiveName.zip" * | out-null
  } else {
    & 7z a -ttar "$tmpFolder\$archiveName.tar" * | out-null
    cd "$tmpFolder"
    $_ = New-Item -Path "..\PUBLIC" -ItemType Directory -ErrorAction SilentlyContinue | out-null
    & 7z a "-tgzip" "-mx=$COMPRESSION_LEVEL" "..\PUBLIC\$archiveName.tar.gz" "$archiveName.tar" | out-null
  }
  # echo $VERSION > "..\PUBLIC\VERSION.TXT"
  Write-All-Text "$($ENV:BUILD_SQLSERVER_ADMINISTRATIVE_VIEWS)\PUBLIC\VERSION.TXT" "$VERSION"
  popd
}

$errors = @($rids | Try-Action-ForEach -ActionTitle "BUILD $VERSION Universe.SqlServer.AdministrativeViews.CLI" -Action $buildAction -ItemTitle $getRidTitle)
$totalErrors = $errors.Count;

if ($totalErrors -gt 0) { throw "Failed counter = $totalErrors" }
