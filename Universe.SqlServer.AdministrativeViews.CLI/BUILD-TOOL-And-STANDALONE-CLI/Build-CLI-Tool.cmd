rem 1) NEEDS C:\Apps\dotnet-10\dotnet.exe
rem 2) NEEDS "C:\Program Files\Git\bin\bash.exe"

"C:\Program Files\Git\bin\bash.exe" -c "export LC_ALL=en_US.UTF-8; Reset-Target-Framework -fw 'net6.0;net8.0;net10.0'"
rd /q /s bin
powershell -f "how to pack.ps1"
"C:\Program Files\Git\bin\bash.exe" -c "export LC_ALL=en_US.UTF-8; Reset-Target-Framework -fw 'net6.0;net35;net40;net45;net46;net462;net48;net8.0'"
