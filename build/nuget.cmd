for /R %%i in (*.nuspec) do NuGet.exe pack %%i
pause