#!/usr/bin/env bash

# Downloads AVBlocks .NET Core (SDK) and the demo assets for the host OS and architecture.
# The SDK is extracted into ./sdk/<arch>/net10.0 and the assets into ./assets

set -e

# get script dir
script_dir=$( cd `dirname ${BASH_SOURCE[0]}` >/dev/null 2>&1 ; pwd -P )

tag="v3.4.0-demo.1"
assets_version="v5"

# detect host os and architecture
os=$(uname -s)
arch=$(uname -m)

case $os in
    Linux)  os_name="linux";  ext="tar.gz" ;;
    Darwin) os_name="darwin"; ext="zip" ;;
    *)
        echo "Unsupported OS: $os"
        exit 1
        ;;
esac

# Under Rosetta uname reports x86_64, so on macOS ask the hardware instead.
if [ "$os" = "Darwin" ] && [ "$(sysctl -n hw.optional.arm64 2>/dev/null)" = "1" ]; then
    arch="arm64"
fi

case $arch in
    x86_64)        arch="x64";   platform="$os_name" ;;
    arm64|aarch64) arch="arm64"; platform="$os_name-arm64" ;;
    *)
        echo "Unsupported architecture: $arch"
        exit 1
        ;;
esac

echo "tag: $tag"
echo "arch: $arch"
echo "platform: $platform"

# sha256 checksum tool
if command -v shasum >/dev/null 2>&1; then
    sha256_check="shasum --algorithm 256 --check"
elif command -v sha256sum >/dev/null 2>&1; then
    sha256_check="sha256sum --check"
else
    echo "Neither shasum nor sha256sum found"
    exit 1
fi

pushd $script_dir >/dev/null

# sdk
mkdir -p ./sdk/$arch/net10.0
pushd ./sdk/$arch/net10.0 >/dev/null

    package="avblocks-net10.0-$tag-$platform.$ext"
    url="https://github.com/avblocks/avblocks-net-core/releases/download/$tag"

    curl --fail --location --output ./$package $url/$package
    curl --fail --location --output ./$package.sha256 $url/$package.sha256

    # verify sha256 checksum
    $sha256_check ./$package.sha256

    # extract
    if [[ "$ext" == "zip" ]]; then
        unzip -o $package
    else
        tar -xvf $package
    fi

popd >/dev/null

# assets
mkdir -p ./assets
pushd ./assets >/dev/null

    package="avblocks_assets_$assets_version.zip"
    url="https://github.com/avblocks/avblocks-assets/releases/download/$assets_version"

    curl --fail --location --output ./$package $url/$package

    # extract
    unzip -o $package

popd >/dev/null

popd >/dev/null

echo "Done."
