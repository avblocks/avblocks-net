# Download AVBlocks .NET Core and Assets on macOS

Change to the directory where you cloned this repository:

```bash
cd avblocks-net
```

## AVBlocks .NET Core

In the script below, change the tag to the release that you need. For the available versions check the [AVBlocks .NET Core](https://github.com/avblocks/avblocks-net-core/releases) releases.   

```bash
# select version and platform
tag="v3.3.0-demo.1"
platform="darwin"

# download
mkdir -p ./sdk/net10.0
cd ./sdk/net10.0

# sdk
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.zip \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip


# sha256 checksum
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.zip.sha256 \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip.sha256

# verify sha256 checksum
shasum --check ./avblocks-net10.0-$tag-$platform.zip.sha256

# unzip
unzip avblocks-net10.0-$tag-$platform.zip

cd ../..
```

## Assets

These demo audio and video assets are used as input for the AVBlocks samples.

```bash
mkdir -p ./assets
cd ./assets

curl \
  --location \
  --output ./avblocks_assets_v3.zip \
  https://github.com/avblocks/avblocks-assets/releases/download/v3/avblocks_assets_v3.zip
  
# unzip
unzip avblocks_assets_v3.zip

cd ..
```
