# Setup for macOS

Download the `dotnet-install.sh` script

```bash
cd
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
```

## .net SDK

### .net 7.0

This is needed by Visual Stuio Code C# extension. Install with `dotnet-install.sh` script:

```bash
./dotnet-install.sh --channel 7.0
```

## .net Runtime

### .NET Runtime 7.0.x

```
./dotnet-install.sh --runtime dotnet --version 7.0.11
```

or download and install from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

### .NET Runtime 6.0.x

```
./dotnet-install.sh --runtime dotnet --version 6.0.22
```

or download and install from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

