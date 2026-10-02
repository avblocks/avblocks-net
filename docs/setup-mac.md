# Setup for macOS

Download the `dotnet-install.sh` script

```bash
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
```

## .NET SDK

### .NET SDK 10.0

Install with `dotnet-install.sh` script. The script installs the .NET that matches your Mac's architecture (arm64 on Apple Silicon, x64 on Intel):

```bash
./dotnet-install.sh --channel 10.0
```

To install a specific architecture explicitly (for example x64 .NET under Rosetta on Apple Silicon), add `--architecture`:

```bash
./dotnet-install.sh --channel 10.0 --architecture arm64   # or x64
```

The samples are built for the architecture of the .NET you run, so the AVBlocks .NET Core package you download must be the same architecture (`darwin-arm64` or `darwin`).
Check which one you have:

```bash
~/.dotnet/dotnet --info | grep -i architecture
```

## Test

Test that you can run the `dotnet` CLI (command line interface)

```bash
~/.dotnet/dotnet --version
~/.dotnet/dotnet new console --help
```
