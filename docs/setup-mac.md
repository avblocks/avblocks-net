# Setup for macOS

Download the `dotnet-install.sh` script

```bash
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
```

## .NET SDK

### .NET SDK 10.0

Install with `dotnet-install.sh` script:

```bash
./dotnet-install.sh --channel 10.0
```

## Test

Test that you can run the `dotnet` CLI (command line interface)

```bash
~/.dotnet/dotnet --version
~/.dotnet/dotnet new console --help
```
