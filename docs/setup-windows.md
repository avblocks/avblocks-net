# Setup for Windows

## .net SDK

### .net 7.0

Install with Windows Package Manager (winget):

```powershell
winget install Microsoft.DotNet.SDK.7
```

### .net 6.0

Install with Windows Package Manager (winget):

```powershell
winget install Microsoft.DotNet.SDK.6
```

### .net 4.8 

```powershell
cd ~/Downloads

Start-BitsTransfer `
    -Source 'https://go.microsoft.com/fwlink/?linkid=2088517'  `
    -Destination ".\ndp48-devpack-enu.exe"
& ./ndp48-devpack-enu.exe /q /norestart /log ndp48-devpack-enu.log

# For non-silent install
# & ./ndp48-devpack-enu.exe 
```
## .net Runtime 

### .net 6.0 runtime

```powershell
Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1';
./dotnet-install.ps1 -InstallDir '~/.dotnet' -Version '6.0.8' -Runtime 'dotnet'
```
