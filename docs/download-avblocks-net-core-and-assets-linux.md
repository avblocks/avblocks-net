# Download AVBlocks .NET Core and Assets on Linux

Change to the directory where you cloned this repository:

```bash
cd avblocks-net
```

## Script

To download the SDK and the assets for the host OS and architecture in one step, run from the repository root:

```bash
./download-sdk-and-assets.sh
```

The SDK is extracted into `sdk/<arch>/net10.0` and the assets into `assets/`. To change the release, edit `tag` at the top of the script. The manual steps below do the same.

## AVBlocks Core

In the script below, change the tag to the release that you need. For the available versions check the [AVBlocks .NET Core](https://github.com/avblocks/avblocks-net-core/releases) releases.   

### x64

```bash
# select version and platform
tag="v3.4.0-demo.1"
platform="linux"

# download
mkdir -p ./sdk/x64/net10.0
cd ./sdk/x64/net10.0

# sdk
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.tar.gz \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.tar.gz

# sha256 checksum
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.tar.gz.sha256 \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.tar.gz.sha256

# verify sha256 checksum
shasum --check ./avblocks-net10.0-$tag-$platform.tar.gz.sha256
  
# unzip
tar -xvf avblocks-net10.0-$tag-$platform.tar.gz

cd ../..
```

### arm64

```bash
# select version and platform
tag="v3.4.0-demo.1"
platform="linux-arm64"

# download
mkdir -p ./sdk/arm64/net10.0
cd ./sdk/arm64/net10.0

# sdk
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.tar.gz \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.tar.gz

# sha256 checksum
curl \
  --location \
  --output ./avblocks-net10.0-$tag-$platform.tar.gz.sha256 \
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.tar.gz.sha256

# verify sha256 checksum
shasum --check ./avblocks-net10.0-$tag-$platform.tar.gz.sha256
  
# unzip
tar -xvf avblocks-net10.0-$tag-$platform.tar.gz

cd ../..
```

## Assets

These demo audio and video assets are used as input for the AVBlocks samples.

```bash
mkdir -p ./assets
cd ./assets

curl \
  --location \
  --output ./avblocks_assets_v5.zip \
  https://github.com/avblocks/avblocks-assets/releases/download/v5/avblocks_assets_v5.zip
  
# unzip
unzip avblocks_assets_v5.zip

cd ..
```
