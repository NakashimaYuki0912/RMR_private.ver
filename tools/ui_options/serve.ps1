$ErrorActionPreference = "Stop"
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $here

$pyCandidates = @(
    "D:\miniconda\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python311\python.exe",
    "$env:LOCALAPPDATA\Programs\Python\Python312\python.exe",
    "python"
)
$py = $null
foreach ($c in $pyCandidates) {
    if ($c -eq "python") {
        $cmd = Get-Command python -ErrorAction SilentlyContinue
        if ($cmd) { $py = $cmd.Source; break }
    } elseif (Test-Path $c) {
        $py = $c; break
    }
}
if (-not $py) { throw "Python not found" }

foreach ($port in 8765, 8766) {
    Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue |
        ForEach-Object {
            if ($_.OwningProcess -gt 4) {
                Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue
            }
        }
}
Start-Sleep -Milliseconds 600

$port = 8765
Write-Host "Starting: $py serve_http.py $port"
$proc = Start-Process -FilePath $py -ArgumentList @("serve_http.py", "$port") -WorkingDirectory $here -PassThru -WindowStyle Minimized

Start-Sleep -Milliseconds 900
$url = "http://127.0.0.1:$port/realization_floor_ui.html"
try {
    $r = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 3
    Write-Host "OK $($r.StatusCode) $url"
    Start-Process $url
} catch {
    Write-Host "Server not responding: $($_.Exception.Message)"
    Write-Host "You can still open offline: $here\realization_floor_ui.html"
    Start-Process "$here\realization_floor_ui.html"
}

Write-Host "PID=$($proc.Id)  Press Ctrl+C in that window to stop, or close the minimized console."
