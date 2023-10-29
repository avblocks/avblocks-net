# Download AVBlocks Core and Assets on Windows

> Scripts are PowerShell

Change to the directory where you cloned this repository:

```bash
cd avblocks-net
```

## AVBlocks Core

In the script below, change the tag to the release that you need. For the available versions check the [AVBlocks Core](https://github.com/avblocks/avblocks-core/releases) releases.   

```powershell
# select version and platform
$tag='v3.0.0-demo.1'
$platform='windows'

# download
new-item -Force -ItemType Directory ./sdk/net60
curl.exe `
  --location `
  --output ./sdk/net60/avblocks-net60-$tag-$platform.zip `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net60-$tag-$platform.zip

new-item -Force -ItemType Directory ./sdk/net48
curl.exe `
  --location `
  --output ./sdk/net48/avblocks-net48-$tag-$platform.zip `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net48-$tag-$platform.zip

# unzip
pushd sdk/net60
expand-archive -Force -Path avblocks-net60-$tag-$platform.zip -DestinationPath .
popd

pushd sdk/net48
expand-archive -Force -Path avblocks-net48-$tag-$platform.zip -DestinationPath .
popd
```

## Assets

These demo audio and video assets are used as input for the AVBlocks samples.

```powershell
new-item -Force -ItemType Directory ./assets
curl.exe `
  --location `
  --output ./assets/avblocks_assets_v1.zip `
  https://github.com/avblocks/avblocks-assets/releases/download/v1/avblocks_assets_v1.zip
  
# unzip
pushd assets
expand-archive -Force -Path avblocks_assets_v1.zip -DestinationPath .
popd
```

