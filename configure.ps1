dotnet nuget remove source "nuget.org"
dotnet nuget add source "https://api.nuget.org/v3/index.json" --name "nuget.org"
dotnet restore