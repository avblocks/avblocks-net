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

### Per-architecture install

To keep an arm64 and an x64 .NET side by side, install each into its own directory. `configure.sh` selects `~/.dotnet/arm64` or `~/.dotnet/x64` based on the host architecture (and falls back to the default `~/.dotnet` install).

Apple Silicon (arm64):

```bash
./dotnet-install.sh --channel 10.0 --architecture arm64 --install-dir ~/.dotnet/arm64
```

Intel (x64):

```bash
./dotnet-install.sh --channel 10.0 --architecture x64 --install-dir ~/.dotnet/x64
```

The samples are built for the architecture of the .NET you run, so the AVBlocks .NET Core package you download must be the same architecture (`darwin-arm64` or `darwin`).

## Test

Test that you can run the `dotnet` CLI (command line interface)

```bash
~/.dotnet/dotnet --version
~/.dotnet/dotnet new console --help
```

With the per-architecture install, use `configure.sh` to put the right `dotnet` on the `PATH`:

```bash
source configure.sh
dotnet --version
dotnet --info | grep -i architecture
dotnet new console --help
```
