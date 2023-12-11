# Setup for macOS

Download the `dotnet-install.sh` script

```bash
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
```


## .NET Runtime

### .NET Runtime 6.0 (LTS)

```
./dotnet-install.sh --runtime dotnet --channel 6.0 --version latest
```

or download and install from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### .NET Runtime 7.0

```
./dotnet-install.sh --runtime dotnet --channel 7.0 --version latest
```

or download and install from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

### .NET Runtime 8.0 (LTS)

```
./dotnet-install.sh --runtime dotnet --channel 8.0 --version latest
```

or download and install from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## .NET SDK

### .NET SDK 6.0

This is needed by the Visual Studio Code C# extension. Install with `dotnet-install.sh` script:

```bash
./dotnet-install.sh --channel 6.0
```

### .NET SDK 7.0

This is needed by the Visual Studio Code C# extension. Install with `dotnet-install.sh` script:

```bash
./dotnet-install.sh --channel 7.0
```

### .NET SDK 8.0

Install with `dotnet-install.sh` script:

```bash
./dotnet-install.sh --channel 8.0
```

## Test

Test that you can run the `dotnet` CLI (command line interface)

```bash
~/.dotnet/dotnet --version
~/.dotnet/dotnet new console --help
```
