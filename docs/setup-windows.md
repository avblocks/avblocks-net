# Setup for Windows

> All scripts are PowerShell

Download the `dotnet-install.ps1` script

```powershell
Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1';
```


## .NET Runtime

### .NET Runtime 6.0 (LTS)

```powershell
./dotnet-install.ps1 -InstallDir '~/.dotnet' -Channel 6.0 -Runtime dotnet -Version latest
```

or download and install from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### .NET Runtime 7.0

```powershell
./dotnet-install.ps1 -InstallDir '~/.dotnet' -Channel 7.0 -Runtime dotnet -Version latest
```

or download and install from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

### .NET Runtime 8.0 (LTS)

```powershell
./dotnet-install.ps1 -InstallDir '~/.dotnet' -Channel 8.0 -Runtime dotnet -Version latest
```

or download and install from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

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

### .NET 6.0

```powershell
./dotnet-install.ps1 -InstallDir '~/.dotnet' -Channel 6.0
```

### .NET 7.0

```powershell
./dotnet-install.ps1 -InstallDir '~/.dotnet' -Channel 7.0
```

### .NET 8.0

```powershell
./dotnet-install.ps1 -InstallDir '~/.dotnet' -Channel 8.0
```

 ## Test

Test that you can run the `dotnet` CLI (command line interface)

```powershell
dotnet --version
dotnet new console --help
```
