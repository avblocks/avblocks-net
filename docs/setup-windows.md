# Setup for Windows

> All scripts are PowerShell

Download the `dotnet-install.ps1` script

```powershell
Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1';
```

## .NET SDK

### .NET 4.8 

```powershell
mkdir ~/Downloads
cd ~/Downloads

Start-BitsTransfer `
    -Source 'https://go.microsoft.com/fwlink/?linkid=2088517'  `
    -Destination ".\ndp48-devpack-enu.exe"
& ./ndp48-devpack-enu.exe /q /norestart /log ndp48-devpack-enu.log

# For non-silent install
# & ./ndp48-devpack-enu.exe 
```

### .NET 10.0

```powershell
./dotnet-install.ps1 -InstallDir '~/.dotnet' -Channel 10.0
```

 ## Test

Test that you can run the `dotnet` CLI (command line interface)

```powershell
dotnet --version
dotnet new console --help
```
