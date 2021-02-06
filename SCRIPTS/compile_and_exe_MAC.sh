cd ../

msbuild TSST.sln

dotnet publish -r win-x64 -c Release #--self-contained

