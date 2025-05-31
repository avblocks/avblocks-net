# Download AVBlocks Core and Assets on Windows

> Scripts are PowerShell

Change to the directory where you cloned this repository:

```bash
cd avblocks-net
```

## AVBlocks Core

In the script below, change the tag to the release that you need. For the available versions check the [AVBlocks Core](https://github.com/avblocks/avblocks-core/releases) releases.   

### .NET Core 6.0

```powershell
# select version and platform
$tag='v3.1.0-demo.1'
$platform='windows'

# download
new-item -Force -ItemType Directory ./sdk/net60
cd ./sdk/net60

# sdk
curl.exe `
  --location `
  --output ./avblocks-net60-$tag-$platform.zip `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net60-$tag-$platform.zip

# sha256 checksum
curl.exe `
  --location `
  --output ./avblocks-net60-$tag-$platform.zip.sha256 `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net60-$tag-$platform.zip.sha256

# verify checksum
$downloadedHash = (Get-FileHash -Algorithm SHA256 ./avblocks-net60-$tag-$platform.zip).Hash.ToLower()
$expectedHash = (Get-Content ./avblocks-net60-$tag-$platform.zip.sha256).Split(' ')[0].ToLower()
if ($downloadedHash -eq $expectedHash) { Write-Host "Checksum OK!"; } else { { Write-Host "Checksum failed!"; } }

# unzip
expand-archive -Force -Path avblocks-net60-$tag-$platform.zip -DestinationPath .

cd ../..
```

### .NET Framework 4.8 

```powershell
new-item -Force -ItemType Directory ./sdk/net48
cd ./sdk/net48

# sdk
curl.exe `
  --location `
  --output ./avblocks-net48-$tag-$platform.zip `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net48-$tag-$platform.zip

# sha256 checksum
curl.exe `
  --location `
  --output ./avblocks-net48-$tag-$platform.zip.sha256 `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net48-$tag-$platform.zip.sha256

# verify checksum
$downloadedHash = (Get-FileHash -Algorithm SHA256 ./avblocks-net48-$tag-$platform.zip).Hash.ToLower()
$expectedHash = (Get-Content ./avblocks-net48-$tag-$platform.zip.sha256).Split(' ')[0].ToLower()
if ($downloadedHash -eq $expectedHash) { Write-Host "Checksum OK!"; } else { { Write-Host "Checksum failed!"; } }

# unzip
expand-archive -Force -Path avblocks-net48-$tag-$platform.zip -DestinationPath .

cd ../..
```

## Assets

These demo audio and video assets are used as input for the AVBlocks samples.

```powershell
new-item -Force -ItemType Directory ./assets
cd ./assets

curl.exe `
  --location `
  --output ./avblocks_assets_v1.zip `
  https://github.com/avblocks/avblocks-assets/releases/download/v1/avblocks_assets_v1.zip
  
# unzip
expand-archive -Force -Path avblocks_assets_v1.zip -DestinationPath .

cd ..
```
