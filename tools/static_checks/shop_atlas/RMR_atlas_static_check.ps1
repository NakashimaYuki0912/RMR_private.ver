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
$root = $script:RepoRoot
function Read-Text($relativePath) {
    Get-Content -LiteralPath (Join-Path $root $relativePath) -Raw -Encoding UTF8
}

function Require-Contains($content, $pattern, $label) {
    if ($content -notmatch [regex]::Escape($pattern)) {
        throw "Missing ${label}: $pattern"
    }
}

function Require-NotContains($content, $pattern, $label) {
    if ($content -match [regex]::Escape($pattern)) {
        throw "Forbidden ${label}: $pattern"
    }
}

function U([int[]]$codes) {
    -join ($codes | ForEach-Object { [char]$_ })
}

$mysteryArtwork1 = U @(0x968F,0x673A,0x4E8B,0x4EF6,0x80CC,0x666F,0x31)
$mysteryArtwork2 = U @(0x968F,0x673A,0x4E8B,0x4EF6,0x80CC,0x666F,0x32)
$mysteryArtwork3 = U @(0x968F,0x673A,0x4E8B,0x4EF6,0x80CC,0x666F,0x33)
$abnormalityBattleArtwork = U @(0x5F02,0x60F3,0x4F53,0x6218,0x6597)

$unlock = Read-Text "RMR_AbnormalityUnlocks.cs"
foreach ($pattern in @(
    "AbnormalityBattleRewardCount = 3",
    "MysteryRewardCount = 1",
    "GetRewardCandidates",
    "GetTierForChapter",
    "GetTierForScript",
    "GetRewardTierForCurrentChapter"
)) {
    Require-Contains $unlock $pattern "abnormality reward rule"
}
Require-NotContains $unlock "NormalDropChance" "normal battle drop chance"
Require-NotContains $unlock "EliteDropChance" "elite battle drop chance"
Require-NotContains $unlock "UnityEngine.Random.Range(0, 100)" "probability gate"

$patches = Read-Text "abcdcode_Refactored\LogLikePatches.cs"
foreach ($pattern in @(
    "LogLikeRoutines.OnClickAtlasTab",
    "UIBattleSettingEditTap)5",
    "AtlasBtn",
    "AtlasBtnFrame",
    "Singleton<LogAtlasPanel>.Instance",
    "UIPassiveSuccessionPopup.Open",
    "UIPassiveSuccessionPopup.Close",
    "SetBattleSettingCardPanelVisible"
)) {
    Require-Contains $patches $pattern "atlas UI wiring"
}
Require-Contains $patches "LogLikeMod.AtlasBtn = LogLikeMod.CreatureBtn" "atlas replaces creature tab button"
Require-Contains $patches 'component.key = "ui_AtlasTab"' "atlas tab text on reused button"
Require-NotContains $patches "LogLikeMod.AtlasBtn.transform.localPosition" "separate atlas tab button"
Require-NotContains $patches "LogLikeRoutines.OnClickCreatureTab(self)" "visible creature tab click binding"
Require-NotContains $patches 'component.key = "ui_CreatureTab"' "visible creature tab text"

$atlas = Read-Text "abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs"
foreach ($pattern in @(
    "class LogAtlasPanel",
    "enum AtlasCategory",
    "enum AtlasSection",
    "RoleBook",
    "BattleCard",
    "AbnormalityPage",
    "EgoPage",
    'LockedTitle = "?"'
)) {
    Require-Contains $atlas $pattern "atlas panel"
}

$models = Read-Text "abcdcode_LOGLIKE_MOD\LogueBookModels.cs"
Require-Contains $models ('registeredPickUpXml._artwork = "' + $abnormalityBattleArtwork + '"') "abnormality battle artwork"

$mysteries = Read-Text "SpecialStaticInfo\MysteryXmlInfos\chAll_mysterys.xml"
foreach ($pattern in @($mysteryArtwork1, $mysteryArtwork2, $mysteryArtwork3)) {
    Require-Contains $mysteries $pattern "mystery artwork key"
}

$uis = Read-Text "Localize\cn\UIs.txt"
foreach ($pattern in @(
    "ui_AtlasTab",
    "ui_RMR_AtlasRoleBook",
    "ui_RMR_AtlasBattleCard",
    "ui_RMR_AtlasAbnormalityPage",
    "ui_RMR_AtlasEgoPage"
)) {
    Require-Contains $uis $pattern "atlas localization"
}

$artwork = Join-Path $root "ArtWork"
foreach ($file in @(($mysteryArtwork1 + ".png"), ($mysteryArtwork2 + ".png"), ($mysteryArtwork3 + ".png"), ($abnormalityBattleArtwork + ".png"))) {
    if (-not (Test-Path -LiteralPath (Join-Path $artwork $file))) {
        throw "Missing artwork file: $file"
    }
}

"RMR ATLAS STATIC CHECK PASSED"

