# Cajetan Infobar
AppBar docked to the bottom of the screen, just above the Taskbar. 

NOTE, does not know how it behaves if Taskbar is docked to any other edge then bottom.

## Usage
Place the EXE in any directory on your machine, and run it.

It saves user settings in 'AppData\Roaming\Cajetan.Infobar'.

## Screenshot
Example of how it looks with all modules enabled. 

NOTE, since I took the screenshot on at desktop machine, it shows 'No Battery'. 
If battery is present, it will show percentage, charging state or estimated time remaining in battery.

![Cajetan Infobar - Screenshot](https://github.com/reptail/Cajetan.Infobar/raw/master/Images/InfobarScreenshot.png "Cajetan Infobar - Screenshot")

## Building
Code should build out of the box, with the .NET5 SDK installed.

Run the BAT file in the root of the repostitory:

```
clean+publish.bat
```
OR run the following commands manually:
```
dotnet clean -c Release -v minimal
dotnet publish -c Release -o ./Publish --self-contained true .\Cajetan.Infobar\Cajetan.Infobar.csproj
```

This will output a single EXE output in the Publish folder, along with symbol files (which can be ignored).