@echo off
cd /d %~dp0
if not exist "DLLs" mkdir "DLLs"
xcopy /Y %~dp0*.dll %~dp0DLLs
RMDIR /S /Q %~dp0de
RMDIR /S /Q %~dp0es
RMDIR /S /Q %~dp0fr
RMDIR /S /Q %~dp0it
RMDIR /S /Q %~dp0zh-CN
del "*.xml"
del "*.pdb"
del "*.dll"
del "NLog.config"
(goto) 2>nul & del "%~f0"