********************************
Configure APPIUM on System Under Test:
********************************

1. Install WinAppDriver https://github.com/Microsoft/WinAppDriver/releases 
2. Set Windows in Developer-Mode (https://blogs.msdn.microsoft.com/appinstaller/2017/03/10/developer-mode-on-windows-desktop/ )
3. Disable Firewall or allow TCP port 4723 (not required if running local).
5. Start WinAppDriver with Admistrator rights from a command prompt (you can use StartWinAppDriver.cmd):
      "C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe" 10.205.20.141 4723/wd/hub
      (Where 10.205.20.141 is the IP of the Test Machine itself. If the system under test is the same than the client side, use "localhost" instead)
      PS: Note the space between the IP and the TCP port.




********************************
Configure the Test Project Client Side:
********************************

1. The code must run as Administrator.
2. Configure the Appium.WinDriver depending your setup.




********************************
About the project:
********************************

- Use the class attribute [Collection("UITests")] over classes testing UI. 
      This will ensure that test are not run in parellel when targeting the same system under test.
      For Specflow, you have to add the tag following tag over the "feature" declaration: 
      @xunit:collection(UITests)

- Always use the Navigation system to call an INavigable view.

- Command to run the project using xunit.runner.console:
      dotnet "%path_to_nuget%\xunit.console.dll" .\IC.Navigation.Tests.dll

- The test project can be run from Visual Studio Code:
      1. Open VSCode from root directory "IC.Navigation.Tests".
      2. Configure the build as you wish in ".vscode\launch.json".
      3. Run the test in the terminal with the command: 
         dotnet test