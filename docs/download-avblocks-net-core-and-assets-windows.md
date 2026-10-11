# Download AVBlocks Core and Assets on Windows

> Scripts are PowerShell

Change to the directory where you cloned this repository:

```bash
cd avblocks-net
```

## Script

To download the SDK and the assets for the host architecture in one step, run from the repository root:

```powershell
./download-sdk-and-assets.ps1
```

The SDK is extracted into `sdk/<arch>/net10.0` and `sdk/<arch>/net48`, and the assets into `assets/`. To change the release, edit `$tag` at the top of the script. The manual steps below do the same.

## AVBlocks Core

In the script below, change the tag to the release that you need. For the available versions check the [AVBlocks Core](https://github.com/avblocks/avblocks-core/releases) releases.   

### x64: .NET Core 10.0

```powershell
# select version and platform
$tag='v3.4.0-demo.1'
$platform='windows'

# download
new-item -Force -ItemType Directory ./sdk/x64/net10.0
cd ./sdk/x64/net10.0

# sdk
curl.exe `
  --location `
  --output ./avblocks-net10.0-$tag-$platform.zip `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip

# sha256 checksum
curl.exe `
  --location `
  --output ./avblocks-net10.0-$tag-$platform.zip.sha256 `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip.sha256

# verify checksum
$downloadedHash = (Get-FileHash -Algorithm SHA256 ./avblocks-net10.0-$tag-$platform.zip).Hash.ToLower()
$expectedHash = (Get-Content ./avblocks-net10.0-$tag-$platform.zip.sha256).Split(' ')[0].ToLower()
if ($downloadedHash -eq $expectedHash) { Write-Host "Checksum OK!"; } else { { Write-Host "Checksum failed!"; } }

# unzip
expand-archive -Force -Path avblocks-net10.0-$tag-$platform.zip -DestinationPath .

cd ../..
```

### x64: .NET Framework 4.8 

```powershell
new-item -Force -ItemType Directory ./sdk/x64/net48
cd ./sdk/x64/net48

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

### arm64: .NET Core 10.0

```powershell
# select version and platform
$tag='v3.4.0-demo.1'
$platform='windows-arm64'

# download
new-item -Force -ItemType Directory ./sdk/arm64/net10.0
cd ./sdk/arm64/net10.0

# sdk
curl.exe `
  --location `
  --output ./avblocks-net10.0-$tag-$platform.zip `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip

# sha256 checksum
curl.exe `
  --location `
  --output ./avblocks-net10.0-$tag-$platform.zip.sha256 `
  https://github.com/avblocks/avblocks-net-core/releases/download/$tag/avblocks-net10.0-$tag-$platform.zip.sha256

# verify checksum
$downloadedHash = (Get-FileHash -Algorithm SHA256 ./avblocks-net10.0-$tag-$platform.zip).Hash.ToLower()
$expectedHash = (Get-Content ./avblocks-net10.0-$tag-$platform.zip.sha256).Split(' ')[0].ToLower()
if ($downloadedHash -eq $expectedHash) { Write-Host "Checksum OK!"; } else { { Write-Host "Checksum failed!"; } }

# unzip
expand-archive -Force -Path avblocks-net10.0-$tag-$platform.zip -DestinationPath .

cd ../..
```

### arm64: .NET Framework 4.8 

```powershell
new-item -Force -ItemType Directory ./sdk/arm64/net48
cd ./sdk/arm64/net48

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
  --output ./avblocks_assets_v5.zip `
  https://github.com/avblocks/avblocks-assets/releases/download/v5/avblocks_assets_v5.zip
  
# unzip
expand-archive -Force -Path avblocks_assets_v5.zip -DestinationPath .

cd ..
```
