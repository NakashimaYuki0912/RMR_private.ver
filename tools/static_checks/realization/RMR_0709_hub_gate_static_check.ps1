$ErrorActionPreference = "Stop"
$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot

function Read-Text($relativePath) {
    Get-Content -LiteralPath (Join-Path $script:RepoRoot $relativePath) -Raw -Encoding UTF8
}

$rm = Read-Text "RMR_RealizationManager.cs"
foreach ($p in @(
    "HubSessionActive",
    "NormalPlayStarted",
    "BeginHubSession",
    "StartNormalPlayFromHub",
    "CanEnterRealizationFromHub",
    "EndHubSessionToLibrary"
)) {
    if ($rm -notmatch [regex]::Escape($p)) {
        throw "Missing gate API: $p"
    }
}

if ($rm -notmatch "CanEnterRealizationFromHub\(\)") {
    throw "StartRealizationBattle must gate via CanEnterRealizationFromHub()"
}

# Multi-floor hub: must not permanently close entry merely by starting a battle
# (old one-shot: InitialRelicEntryAvailable = false right after resolve).
if ($rm -match 'StartRealizationBattle[\s\S]{0,800}?InitialRelicEntryAvailable\s*=\s*false') {
    throw "StartRealizationBattle still consumes InitialRelicEntryAvailable (blocks multi-floor hub)"
}

"RMR_0709 hub gate check PASSED"
