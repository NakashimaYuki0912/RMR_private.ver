@echo off
chcp 65001 >nul
cd /d "%~dp0"
title RMR-UI-Options

set PY=
if exist "D:\miniconda\python.exe" set "PY=D:\miniconda\python.exe"
if "%PY%"=="" if exist "%LOCALAPPDATA%\Programs\Python\Python311\python.exe" set "PY=%LOCALAPPDATA%\Programs\Python\Python311\python.exe"
if "%PY%"=="" if exist "%LOCALAPPDATA%\Programs\Python\Python312\python.exe" set "PY=%LOCALAPPDATA%\Programs\Python\Python312\python.exe"
if "%PY%"=="" (
  where python >nul 2>&1 && set "PY=python"
)
if "%PY%"=="" (
  echo [ERROR] 找不到 Python。
  echo 你仍可双击 atlas_ui.html 离线预览（不依赖服务器）。
  start "" "%~dp0atlas_ui.html"
  pause
  exit /b 1
)

echo 清理 8765 端口占用...
powershell -NoProfile -ExecutionPolicy Bypass -Command "foreach ($p in 8765,8766) { Get-NetTCPConnection -LocalPort $p -ErrorAction SilentlyContinue | ForEach-Object { if ($_.OwningProcess -gt 4) { Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue } } }"
timeout /t 1 /nobreak >nul

echo 用 %PY% 启动服务...
start "RMR-UI-Options-Server" /MIN "%PY%" "%~dp0serve_http.py" 8765
timeout /t 2 /nobreak >nul

echo 打开浏览器...
start "" "http://127.0.0.1:8765/atlas_ui.html"

echo.
echo 若浏览器仍显示 refused:
echo   1) 双击本目录 atlas_ui.html 离线打开（推荐）
echo   2) 或看最小化窗口 RMR-UI-Options-Server 是否有报错
echo.
echo 图鉴:     http://127.0.0.1:8765/atlas_ui.html
echo 解放战UI: http://127.0.0.1:8765/realization_floor_ui.html
echo.
pause
