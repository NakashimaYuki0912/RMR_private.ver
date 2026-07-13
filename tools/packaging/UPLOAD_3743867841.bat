@echo off
cd /d "%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0UPLOAD_3743867841.ps1"
if errorlevel 1 pause
exit /b %ERRORLEVEL%
