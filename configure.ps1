$env:DOTNET_CLI_TELEMETRY_OPTOUT=1

$nuget_source = 'https://api.nuget.org/v3/index.json'
if (!$(dotnet nuget list source | select-string "$nuget_source")) {
    dotnet nuget add source "https://api.nuget.org/v3/index.json" --name "nuget.org"
}

dotnet restore