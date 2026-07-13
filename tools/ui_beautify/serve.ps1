# Serve RMR UI beautify gallery on a free local port.
# Usage: powershell -ExecutionPolicy Bypass -File .\tools\ui_beautify\serve.ps1

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$port = 8765

# pick free port
for ($p = 8765; $p -le 8785; $p++) {
    $busy = Get-NetTCPConnection -LocalPort $p -ErrorAction SilentlyContinue | Select-Object -First 1
    if (-not $busy) { $port = $p; break }
}

Write-Host "Serving $root"
Write-Host "Open: http://127.0.0.1:$port/"
Write-Host "Press Ctrl+C to stop."

Set-Location $root
python -m http.server $port --bind 127.0.0.1
