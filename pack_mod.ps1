# RogueLike Mod Reborn - Pack Script (v2: preserve Workshop structure 1:1)
# Usage: .\pack_mod.ps1
# Output: RougelikeModReborn_YYYYMMDD_HHmmss.zip in project root

$ErrorActionPreference = "Stop"

$workshop = "E:\Steam\steamapps\workshop\content\1256670\3503523710"
if (-not (Test-Path $workshop)) {
    Write-Host "ERROR: Workshop not found: $workshop" -ForegroundColor Red
    exit 1
}

$projectRoot = $PSScriptRoot
$tmpDir = "$projectRoot\_release_packages\RougelikeModReborn"

# Clean + copy everything 1:1
if (Test-Path $tmpDir) { Remove-Item -Recurse -Force $tmpDir }
Copy-Item -Recurse $workshop $tmpDir
Write-Host "Copied Workshop -> temp"

$tmpAsm = "$tmpDir\Assemblies"
$tmpDlls = "$tmpAsm\dlls"

# ====== Clean dev files ======
Write-Host "Cleaning dev files..."
$removed = 0
Get-ChildItem -Recurse $tmpDir -Filter "*.bak" -ErrorAction SilentlyContinue | ForEach-Object { Remove-Item $_.FullName -Force; $removed++ }
Write-Host "  Removed $removed .bak files"

$removed = 0
Get-ChildItem -Recurse $tmpDir -Filter "*.pre_*" -ErrorAction SilentlyContinue | ForEach-Object { Remove-Item $_.FullName -Force; $removed++ }
Write-Host "  Removed $removed .pre_* files"

if (Test-Path "$tmpDlls\DevNuggets") {
    Remove-Item -Recurse -Force "$tmpDlls\DevNuggets"
    Write-Host "  Removed DevNuggets\"
}

# Remove Workshop metadata (not needed for Mods)
@("$tmpDir\desc.txt", "$tmpDir\preview.jpg.png", "$tmpDir\old changelogs.txt", "$tmpDir\mod infos") | ForEach-Object {
    if (Test-Path $_) { Remove-Item -Recurse -Force $_ }
}

# ====== Fix FormationInfo ======
Write-Host "Fixing FormationInfo..."
$fiDir = "$tmpDlls\AddData\FormationInfo"
if (-not (Test-Path $fiDir)) {
    New-Item -ItemType Directory -Force -Path $fiDir | Out-Null
    Copy-Item "$tmpDlls\AddData\FormationInfo.txt" "$fiDir\FormationInfo.txt" -Force
    Write-Host "  Created FormationInfo\FormationInfo.txt"
}

# ====== Remove Korean locale ======
Write-Host "Removing Korean locale..."
if (Test-Path "$tmpDlls\Localize\kr") {
    Remove-Item -Recurse -Force "$tmpDlls\Localize\kr"
    Write-Host "  Removed Localize\kr\"
}
$krStory = "$tmpDlls\StoryInfo\Localize\kr-story.zip"
if (Test-Path $krStory) {
    Remove-Item -Force $krStory
    Write-Host "  Removed kr-story.zip"
}

# ====== Zip ======
Write-Host "Creating zip..."
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$zipName = "RougelikeModReborn_v$timestamp.zip"
$zipPath = Join-Path $projectRoot $zipName
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }

Compress-Archive -Path "$tmpDir\*" -DestinationPath $zipPath -Force
Remove-Item -Recurse -Force $tmpDir

$zipSize = [math]::Round((Get-Item $zipPath).Length / 1MB, 2)
Write-Host ""
Write-Host "Done: $zipName ($zipSize MB)"
