$ErrorActionPreference = 'Stop'


$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot
function Read-Text([string]$Path) {
    return Get-Content -Raw -Encoding UTF8 -LiteralPath $Path
}

function Assert-Contains([string]$Name, [string]$Text, [string]$Pattern) {
    if ($Text -notmatch $Pattern) {
        throw "FAIL: $Name"
    }
    Write-Host "PASS: $Name"
}

function Assert-NotContains([string]$Name, [string]$Text, [string]$Pattern) {
    if ($Text -match $Pattern) {
        throw "FAIL: $Name"
    }
    Write-Host "PASS: $Name"
}

$root = $script:RepoRoot
$logLikeMod = Read-Text (Join-Path $root 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs')

$loadStart = $logLikeMod.IndexOf('public static void LoadEquipPages()')
$loadEnd = $logLikeMod.IndexOf('public static List<BookXmlInfo> LoadEquipPage', $loadStart)
if ($loadStart -lt 0 -or $loadEnd -lt 0) {
    throw 'FAIL: LoadEquipPages block not found'
}
$loadBlock = $logLikeMod.Substring($loadStart, $loadEnd - $loadStart)

Assert-Contains 'Core-page loader only scans XML files in main AddData folder' `
    $loadBlock 'directoryInfo1\.GetFiles\s*\(\s*"\*\.xml"\s*\)'

Assert-Contains 'Core-page loader only scans XML files in extension AddData folders' `
    $loadBlock 'directoryInfo2\.GetFiles\s*\(\s*"\*\.xml"\s*\)'

Assert-NotContains 'Core-page loader must not scan every file including backups' `
    $loadBlock 'GetFiles\s*\(\s*\)'

$sourceEquipPath = Join-Path $root 'AddData\EquipPage'
$backupFiles = Get-ChildItem -LiteralPath $sourceEquipPath -File -Filter '*.bak' -ErrorAction SilentlyContinue
if ($backupFiles.Count -gt 0) {
    throw "FAIL: AddData\EquipPage contains backup files that the runtime directory can accidentally load: $($backupFiles.Name -join ', ')"
}
Write-Host 'PASS: source AddData\EquipPage has no backup files'

Write-Host 'PASS: equip page loader static checks completed.'

