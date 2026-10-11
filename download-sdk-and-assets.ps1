# Downloads AVBlocks .NET Core (SDK) and the demo assets for the host architecture.
# The SDK is extracted into ./sdk/<arch>/net10.0 and ./sdk/<arch>/net48, and the assets into ./assets

$ErrorActionPreference = "Stop"

# get script dir
$script_dir = $PSScriptRoot

$tag = "v3.4.0-demo.1"
$assets_version = "v5"

# detect host architecture
# PROCESSOR_ARCHITEW6432 is set when PowerShell runs emulated (e.g. x64 PowerShell on ARM64).
$host_arch = if ($env:PROCESSOR_ARCHITEW6432) { $env:PROCESSOR_ARCHITEW6432 } else { $env:PROCESSOR_ARCHITECTURE }

switch ($host_arch) {
    "AMD64" { $arch = "x64";   $platform = "windows" }
    "ARM64" { $arch = "arm64"; $platform = "windows-arm64" }
    default {
        Write-Output "Unsupported architecture: $host_arch"
        exit 1
    }
}

Write-Output "tag: $tag"
Write-Output "arch: $arch"
Write-Output "platform: $platform"

function Invoke-Download([String]$url, [String]$output) {
    curl.exe --fail --location --output $output $url
    if ($LASTEXITCODE -ne 0) { throw "Download failed: $url" }
}

Push-Location $script_dir

try {
    # sdk
    $url = "https://github.com/avblocks/avblocks-net-core/releases/download/$tag"

    foreach ($framework in "net10.0", "net48") {
        New-Item -Force -ItemType Directory ./sdk/$arch/$framework | Out-Null
        Push-Location ./sdk/$arch/$framework

            $package = "avblocks-$framework-$tag-$platform.zip"

            Invoke-Download "$url/$package" "./$package"
            Invoke-Download "$url/$package.sha256" "./$package.sha256"

            # verify sha256 checksum
            $downloaded_hash = (Get-FileHash -Algorithm SHA256 "./$package").Hash.ToLower()
            $expected_hash = (Get-Content "./$package.sha256").Split(' ')[0].ToLower()
            if ($downloaded_hash -ne $expected_hash) { throw "Checksum failed: $package" }
            Write-Output "Checksum OK: $package"

            # extract
            Expand-Archive -Force -Path "./$package" -DestinationPath .

        Pop-Location
    }

    # assets
    New-Item -Force -ItemType Directory ./assets | Out-Null
    Push-Location ./assets

        $package = "avblocks_assets_$assets_version.zip"
        $url = "https://github.com/avblocks/avblocks-assets/releases/download/$assets_version"

        Invoke-Download "$url/$package" "./$package"

        # extract
        Expand-Archive -Force -Path "./$package" -DestinationPath .

    Pop-Location
}
finally {
    Pop-Location
}

Write-Output "Done."
