set WORKSPACE=..
set LUBAN_DLL=Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t client ^
	-c cs-simple-json ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
	-x outputDataDir=%WORKSPACE%\Assets\Res\DataTable ^
	-x outputCodeDir=%WORKSPACE%\Assets\Code\Framework\Third\Luban\DataTable

pause