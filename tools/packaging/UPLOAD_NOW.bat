@echo off
setlocal
title RMR Workshop Upload 3743867841
cd /d "%~dp0"

echo ============================================================
echo  Upload RMR fan work -^> Workshop 3743867841
echo  Account: gffnj3
echo ============================================================
echo.

set "STEAMCMD=E:\Steam\steamcmd\steamcmd.exe"
set "VDF=%~dp0workshop_item_3743867841.vdf"
set "PKG=E:\Steam\steamapps\workshop\content\1256670_BACKUPS\3743867841_upload"
set "DLL=%PKG%\Assemblies\dlls\RogueLike Mod Reborn.dll"

if not exist "%STEAMCMD%" (
  echo ERROR: steamcmd missing:
  echo   %STEAMCMD%
  goto :fail
)
if not exist "%VDF%" (
  echo ERROR: VDF missing:
  echo   %VDF%
  goto :fail
)
if not exist "%DLL%" (
  echo ERROR: Upload package DLL missing:
  echo   %DLL%
  goto :fail
)

echo Package OK
echo   %DLL%
echo VDF:
echo   %VDF%
echo.
echo You will be asked for Steam password / Steam Guard.
echo Fully exit Library of Ruina first if it is running.
echo.
echo Press any key to start steamcmd login + upload...
pause >nul

"%STEAMCMD%" +login gffnj3 +workshop_build_item "%VDF%" +quit
set "CODE=%ERRORLEVEL%"
echo.
if "%CODE%"=="0" (
  echo steamcmd exit 0. If it printed Success, refresh:
  echo   https://steamcommunity.com/sharedfiles/filedetails/?id=3743867841
) else (
  echo steamcmd exit code: %CODE%
  echo Common: wrong password, Steam Guard, network, path issues.
)
echo.
echo Press any key to close...
pause >nul
exit /b %CODE%

:fail
echo.
echo Press any key to close...
pause >nul
exit /b 1
