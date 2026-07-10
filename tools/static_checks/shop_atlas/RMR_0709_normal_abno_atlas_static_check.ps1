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

$models = Read-Text "abcdcode_LOGLIKE_MOD\LogueBookModels.cs"
$record = [regex]::Match($models, 'public static void RecordAtlasAbnormalityPage\(LorId id\)\s*\{[\s\S]{0,800}?\}')
if (-not $record.Success) { throw "RecordAtlasAbnormalityPage not found" }
if ($record.Value -match 'IsRealizationExclusive') {
    throw "RecordAtlasAbnormalityPage still gates on IsRealizationExclusive (normal pages must be allowed)"
}
if ($record.Value -notmatch 'rewardtype') {
    throw "RecordAtlasAbnormalityPage should validate Creature rewardtype"
}

$prune = [regex]::Match($models, 'public static void PruneInvalidPermanentAbnormalityAtlasUnlocks\(\)[\s\S]{0,1200}?AtlasUnlockedEgoPages')
if (-not $prune.Success) { throw "PruneInvalidPermanentAbnormalityAtlasUnlocks block not found" }
if ($prune.Value -notmatch 'IsRealizationExclusive') {
    throw "Prune should still special-case exclusive pages"
}
# Normal pages must not be bulk-removed solely for not being exclusive
if ($prune.Value -match 'if \(info == null \|\| !RMRAbnormalityUnlockManager\.IsRealizationExclusive') {
    throw "Prune still removes all non-exclusive abnormality pages"
}

$unlocks = Read-Text "RMR_AbnormalityUnlocks.cs"
if ($unlocks -notmatch 'UnlockShopAbnormalityPage[\s\S]{0,400}?RecordAtlasAbnormalityPage\(info\.id\)') {
    throw "UnlockShopAbnormalityPage must always RecordAtlasAbnormalityPage"
}
if ($unlocks -match 'UnlockShopAbnormalityPage[\s\S]{0,400}?if \(IsRealizationExclusive\(info\)\)\s*\n\s*LogueBookModels\.RecordAtlasAbnormalityPage') {
    throw "UnlockShopAbnormalityPage still only records exclusive pages"
}

$onPick = [regex]::Match($unlocks, 'public static bool OnEmotionPagePicked\([\s\S]*?public static void UnlockEgoForCurrentRoute')
if (-not $onPick.Success) { throw "OnEmotionPagePicked not found" }
if ($onPick.Value -match 'if \(IsRealizationExclusive\(info\)\)\s*\r?\n\s*LogueBookModels\.RecordAtlasAbnormalityPage') {
    throw "OnEmotionPagePicked still only records exclusive pages"
}
if (($onPick.Value | Select-String -Pattern 'RecordAtlasAbnormalityPage\(info\.id\)' -AllMatches).Matches.Count -lt 1) {
    throw "OnEmotionPagePicked must call RecordAtlasAbnormalityPage"
}

if ($models -notmatch 'GetRouteUnlockedPageIds\(\)') {
    throw "SyncCurrentInventoryToPermanentAtlas should sync route abnormality pages via GetRouteUnlockedPageIds"
}

"RMR_0709 normal abno atlas check PASSED"
