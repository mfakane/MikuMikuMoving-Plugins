@echo off

rem enumerate through directories and copy their bin/Release/net461 to Publish dir

cd %~dp0

if not exist Publish mkdir Publish

for /f "delims=" %%i in ('dir /b /ad') do (
    echo %%i
    if exist %%i\bin\Release\net461\%%i.dll (
        copy %%i\bin\Release\net461\%%i.dll Publish
        copy %%i\bin\Release\net461\%%i.txt Publish
    )
)