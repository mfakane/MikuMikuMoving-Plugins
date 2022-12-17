@echo off

:: MikuMikuMoving のプラグインフォルダへのパスを指定します
set targetPath=\\Aquamarine\Documents\Mmd\tools\MikuMikuMoving\Plugins

copy "%1" "%targetPath%"