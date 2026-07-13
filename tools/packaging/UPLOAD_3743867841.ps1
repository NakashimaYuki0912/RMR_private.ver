$ErrorActionPreference = "Stop"
Write-Host "============================================================"
Write-Host " Upload RMR fan work -> Workshop 3743867841"
Write-Host " Account: gffnj3"
Write-Host "============================================================"

$steamcmd = "E:\Steam\steamcmd\steamcmd.exe"
$vdf = Join-Path $PSScriptRoot "workshop_item_3743867841.vdf"
$content = "E:\Steam\steamapps\workshop\content\1256670_BACKUPS\3743867841_upload"

if (-not (Test-Path $steamcmd)) { throw "steamcmd missing: $steamcmd" }
if (-not (Test-Path $vdf)) { throw "VDF missing: $vdf" }
if (-not (Test-Path (Join-Path $content "Assemblies\dlls\RogueLike Mod Reborn.dll"))) {
  throw "Upload package missing: $content"
}
$smi = Get-Content (Join-Path $content "StageModInfo.xml") -Raw
if ($smi -match 'Exist="true"') { throw "StageModInfo still has Exist=true - abort" }
if ($smi -notmatch 'Exist="false"') { throw "StageModInfo missing Exist=false - abort" }

Write-Host "Package OK: $content"
Write-Host "VDF: $vdf"
Write-Host ""
Write-Host "You will be asked for Steam password / Steam Guard."
Write-Host "Press Enter to start steamcmd login+upload..."
[void][System.Console]::ReadLine()

& $steamcmd +login gffnj3 +workshop_build_item $vdf +quit
$code = $LASTEXITCODE
Write-Host ""
if ($code -eq 0) {
  Write-Host "steamcmd exit 0. If it printed Success, refresh:"
  Write-Host "  https://steamcommunity.com/sharedfiles/filedetails/?id=3743867841"
} else {
  Write-Host "steamcmd exit code: $code"
  Write-Host "Common: wrong password, Steam Guard, network."
}
Write-Host "Press Enter to close..."
[void][System.Console]::ReadLine()
exit $code