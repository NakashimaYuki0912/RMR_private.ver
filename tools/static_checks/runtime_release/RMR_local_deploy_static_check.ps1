$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
while ($root -and -not (Test-Path (Join-Path $root 'RogueLike Mod Reborn.csproj'))) {
    $root = Split-Path -Parent $root
}
if (-not $root) { throw 'Could not locate repository root for static check.' }

function ReadText([string]$relativePath) {
    Get-Content -LiteralPath (Join-Path $root $relativePath) -Raw -Encoding UTF8
}
function AssertContains([string]$name, [string]$text, [string]$needle) {
    if (-not $text.Contains($needle)) { throw "$name missing: $needle" }
}

$deploy = ReadText 'tools/packaging/deploy_local.ps1'
$stage = ReadText 'tools/packaging/StageModInfo.local.xml'

AssertContains 'local deploy target root' $deploy 'LibraryOfRuina_Data\Mods'
AssertContains 'dedicated local directory name' $deploy 'RMR_REBORN_LOCAL'
AssertContains 'nested RMR runtime path' $deploy 'Assemblies\dlls'
AssertContains 'game running guard' $deploy 'Get-Process -Name "LibraryOfRuina"'
AssertContains 'local StageModInfo template' $deploy 'StageModInfo.local.xml'
AssertContains 'post-copy DLL hash verification' $deploy 'Get-FileHash $dstDll -Algorithm SHA256'
if ($deploy -like '*workshop\content*') { throw 'Local deploy must not write to Steam Workshop content.' }
AssertContains 'distinct local list title' $stage '[LOCAL TEST] RMR REBORN fan work'
AssertContains 'same RMR package identity' $stage '<ID>abcdcodecalmmagma.LogueLikeReborn</ID>'

Write-Host 'RMR local deploy static check passed.'
