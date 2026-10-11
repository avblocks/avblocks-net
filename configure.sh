#!/usr/bin/env bash

# Use the .NET that matches the native architecture of the machine (see docs/setup-mac.md).
# Under Rosetta uname reports x86_64, so on macOS ask the hardware instead.
arch=$(uname -m)
if [ "$(uname -s)" = "Darwin" ] && [ "$(sysctl -n hw.optional.arm64 2>/dev/null)" = "1" ]; then
    arch=arm64
fi
case "$arch" in
    aarch64|arm64) arch=arm64 ;;
    *)             arch=x64 ;;
esac

if [ -x "$HOME/.dotnet/$arch/dotnet" ]; then
    export DOTNET_ROOT="$HOME/.dotnet/$arch"
    export PATH="$DOTNET_ROOT:$PATH"
elif [ -x "$HOME/.dotnet/dotnet" ]; then
    # default dotnet-install.sh layout (installs the native architecture)
    export DOTNET_ROOT="$HOME/.dotnet"
    export PATH="$DOTNET_ROOT:$PATH"
else
    echo "warning: $HOME/.dotnet/$arch/dotnet not found, using the dotnet already on PATH (see docs/setup-mac.md)" >&2
fi

# x64 .NET, used by tools that need an x64 test host
if [ -x "$HOME/.dotnet/x64/dotnet" ]; then
    export DOTNET_ROOT_X64="$HOME/.dotnet/x64"
fi

export DOTNET_CLI_TELEMETRY_OPTOUT=1

nuget_source='https://api.nuget.org/v3/index.json'
if [ -z $(dotnet nuget list source | grep $nuget_source) ]; then
    dotnet nuget add source "$nuget_source" --name "nuget.org"
fi

dotnet restore
