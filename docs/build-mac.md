# Build on macOS

## Build

Configure project:

```bash
source configure.sh
```

Restore packages:

```bash
dotnet restore
```

Build:

```bash
dotnet build samples.sln
```

The samples are built for the architecture of the .NET you run: arm64 on Apple Silicon, x64 on Intel. The output goes to `bin/arm64/net10.0` or `bin/x64/net10.0`.

To override the architecture, pass the platform explicitly (the matching AVBlocks .NET Core package must be in `sdk/<arch>/net10.0`):

```bash
dotnet build samples.sln -p:Platform=x64
dotnet build samples.sln -p:Platform=arm64
```

