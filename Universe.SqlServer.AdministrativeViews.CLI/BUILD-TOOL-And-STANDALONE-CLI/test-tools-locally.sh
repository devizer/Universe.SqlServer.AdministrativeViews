dotnet tool uninstall --global SqlServer.AdministrativeViews >/dev/null 2>&1 || true
Say --Reset-Stopwatch
echo DOWNLOADING .NET Installer
try-and-retry curl -kfSL -o /tmp/install-DOTNET.sh https://raw.githubusercontent.com/devizer/test-and-build/master/lab/install-DOTNET.sh
for netver in 10.0 8.0 6.0; do
  export DOTNET_VERSIONS="$netver"
  export SKIP_DOTNET_DEPENDENCIES=True
  export DOTNET_TARGET_DIR=/usr/local/share/dotnet
  sudo rm -rf "$DOTNET_TARGET_DIR"
  sudo rm -rf $HOME/.dotnet/tools
  bash /tmp/install-DOTNET.sh
  # script=https://raw.githubusercontent.com/devizer/test-and-build/master/lab/install-DOTNET.sh; (wget -q -nv --no-check-certificate -O - $script 2>/dev/null || curl -ksSL $script) | bash; test -s /usr/share/dotnet/dotnet && sudo ln -f -s /usr/share/dotnet/dotnet /usr/local/bin/dotnet; test -s /usr/local/share/dotnet/dotnet && sudo ln -f -s /usr/local/share/dotnet/dotnet /usr/local/bin/dotnet; 
  mkdir -p sudo 
  rm -rf "$HOME/.dotnet/tools"
  export PATH="${DOTNET_TARGET_DIR}:$HOME/.dotnet/tools:$PATH"
  dotnet --info
  dotnet tool uninstall --global SqlServer.AdministrativeViews >/dev/null 2>&1 || true
  dotnet tool install --global SqlServer.AdministrativeViews 
  Say "DOTNET $netver, SqlServer.AdministrativeViews VERSION is [$(SqlServer.AdministrativeViews -v)]"
  SqlServer.AdministrativeViews -v
done
