# Upload / update Steam Workshop item for Library of Ruina.
# Default: YOUR item 3743867841 (not upstream 3503523710).
#
# Prerequisites:
#   1. deploy_workshop.ps1 -Configuration Release
#   2. prepare_workshop_upload.ps1
#   3. Steam Guard / login (this script will prompt via steamcmd)
#
# Usage:
#   powershell -ExecutionPolicy Bypass -File .\tools\packaging\upload_workshop.ps1
#   powershell -File .\tools\packaging\upload_workshop.ps1 -SteamUser "your_steam_username"
#
# Visibility in the VDF is 0=Public. You can still change visibility on the web page later.

param(
    [string]$WorkshopContentId = "3743867841",
    [string]$SteamUser = "",
    [string]$VdfPath = "",
    [string]$SteamCmdPath = ""
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent (Split-Path -Parent $scriptDir)

$uploadRoot = "E:\Steam\steamapps\workshop\content\1256670\${WorkshopContentId}_upload"
if (-not (Test-Path $uploadRoot)) {
    Write-Host "Upload tree missing — preparing now..." -ForegroundColor Yellow
    & powershell -ExecutionPolicy Bypass -File (Join-Path $scriptDir "prepare_workshop_upload.ps1") -WorkshopContentId $WorkshopContentId
}

if (-not $VdfPath) {
    $VdfPath = Join-Path $scriptDir "workshop_item_${WorkshopContentId}.vdf"
}
if (-not (Test-Path $VdfPath)) {
    throw "VDF missing: $VdfPath"
}

# Resolve steamcmd
$candidates = @(
    $SteamCmdPath,
    "E:\Steam\steamcmd\steamcmd.exe",
    "E:\Steam\steamcmd.exe",
    "C:\steamcmd\steamcmd.exe",
    "$env:USERPROFILE\steamcmd\steamcmd.exe",
    "D:\steamcmd\steamcmd.exe"
) | Where-Object { $_ -and (Test-Path $_) }

if (-not $candidates) {
    $installDir = "E:\Steam\steamcmd"
    Write-Host "steamcmd not found. Downloading to $installDir ..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Force -Path $installDir | Out-Null
    $zip = Join-Path $installDir "steamcmd.zip"
    Invoke-WebRequest -Uri "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" -OutFile $zip
    Expand-Archive -Path $zip -DestinationPath $installDir -Force
    Remove-Item $zip -Force -ErrorAction SilentlyContinue
    $steamcmd = Join-Path $installDir "steamcmd.exe"
    if (-not (Test-Path $steamcmd)) { throw "Failed to install steamcmd" }
}
else {
    $steamcmd = $candidates | Select-Object -First 1
}

Write-Host "Using steamcmd: $steamcmd" -ForegroundColor Cyan
Write-Host "VDF: $VdfPath" -ForegroundColor Cyan
Write-Host "Content: $uploadRoot" -ForegroundColor Cyan
Write-Host ""
Write-Host "IMPORTANT:" -ForegroundColor Yellow
Write-Host "  - This updates YOUR Workshop item $WorkshopContentId on Steam servers."
Write-Host "  - Local folder edits alone NEVER change the Workshop page Updated date."
Write-Host "  - steamcmd will prompt for Steam password + Steam Guard if needed."
Write-Host "  - Keep Steam Desktop client open is OK; close LibraryOfRuina if it locks files."
Write-Host ""

if (-not $SteamUser) {
    $SteamUser = Read-Host "Steam username (account that owns workshop item $WorkshopContentId)"
}
if ([string]::IsNullOrWhiteSpace($SteamUser)) {
    throw "Steam username required."
}

# First run may update steamcmd itself
& $steamcmd +quit | Out-Null

Write-Host "Starting workshop_build_item ..." -ForegroundColor Cyan
& $steamcmd `
    +login $SteamUser `
    +workshop_build_item $VdfPath `
    +quit

if ($LASTEXITCODE -ne 0) {
    Write-Host "steamcmd exited with code $LASTEXITCODE" -ForegroundColor Red
    Write-Host "Common issues: wrong password, Steam Guard, item not owned by this account, path too long, or content folder empty." -ForegroundColor Yellow
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "If steamcmd reported success:" -ForegroundColor Green
Write-Host "  1. Refresh https://steamcommunity.com/sharedfiles/filedetails/?id=$WorkshopContentId"
Write-Host "  2. You should see a new Updated / Change Note time."
Write-Host "  3. Change Visibility from Hidden -> Public on the item page (OWNER CONTROLS)."
Write-Host "  4. Subscribe on a test account / re-subscribe and fully restart LoR."
Write-Host "  5. Player.log should contain Build: 2026-07-13Trelease-binah-upgrade-shop10+08:00"
