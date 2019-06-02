:: Batch to copy in the installation directory of "Windows Application Driver"
:: Set the target to "localhost" when host and client are same
:: or the IP of the current machine when the client is remotly.
:: Run this script with ADMINISTRATOR rights.


SET target=localhost
::SET target=10.192.102.3

SET currentDir=%~dp0
"%currentDir%WinAppDriver.exe" %target% 4723/wd/hub
IF NOT [%ERRORLEVEL%] == [0] (
	ECHO ERROR: %ERRORLEVEL% 
	PAUSE
)