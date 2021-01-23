@ECHO OFF

RMDIR /S /Q Publish
ECHO.

dotnet clean -c Release -v minimal
dotnet publish -c Release -o ./Publish --self-contained true .\Cajetan.Infobar\Cajetan.Infobar.csproj

ECHO.
PAUSE