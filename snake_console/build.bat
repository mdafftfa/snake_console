@echo off
echo 🐍 Building Snake Console Game...
echo.

echo 📦 Cleaning previous builds...
dotnet clean

echo 🔨 Building Release version...
dotnet publish -c Release -r win-x64 --self-contained true ^
    -p:PublishSingleFile=true ^
    -p:EnableCompressionInSingleFile=true ^
    -p:IncludeNativeLibrariesForSelfExtract=true ^
    -p:IncludeAllContentForSelfExtract=true ^
    -p:OutputType=WinExe ^
    -p:ApplicationManifest=app.manifest ^
    -p:DebugType=none ^
    -p:DebugSymbols=false

echo.
echo ✅ Build completed successfully!
echo 🎮 Game ready at: %cd%\SnakeConsole_Game\snake_console.exe
echo.
pause