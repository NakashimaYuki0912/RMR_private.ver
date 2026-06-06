$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path

function Read-Text($relativePath) {
    Get-Content -LiteralPath (Join-Path $root $relativePath) -Raw -Encoding UTF8
}

function Require-Contains($content, $pattern, $label) {
    if ($content -notmatch [regex]::Escape($pattern)) {
        throw "Missing ${label}: $pattern"
    }
}

$models = Read-Text "abcdcode_LOGLIKE_MOD\LogueBookModels.cs"
foreach ($pattern in @(
    "AtlasUnlockedRoleBooks",
    "AtlasUnlockedBattleCards",
    "RecordAtlasRoleBook",
    "RecordAtlasBattleCard",
    "IsAtlasRoleBookUnlocked",
    "IsAtlasBattleCardUnlocked",
    'data1.AddData("atlasRoleBookUnlocks"',
    'data1.AddData("atlasBattleCardUnlocks"',
    'LoadAtlasUnlocks(save.GetData("atlasRoleBookUnlocks")',
    'LoadAtlasUnlocks(save.GetData("atlasBattleCardUnlocks")',
    "RecordAtlasBattleCard(cardId)",
    "RecordAtlasRoleBook(id)"
)) {
    Require-Contains $models $pattern "atlas unlock progress"
}

foreach ($relativePath in @(
    "abcdcode_LOGLIKE_MOD\PickUpModel_EquipDefault.cs",
    "abcdcode_LOGLIKE_MOD\ShopPickUpModel.cs"
)) {
    $content = Read-Text $relativePath
    Require-Contains $content "RecordAtlasRoleBook" "direct key page atlas record in $relativePath"
}

$atlas = Read-Text "abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs"
foreach ($pattern in @(
    ".OrderByDescending(x => x.Unlocked)",
    "HashSet<string> seenAbnormalities",
    "GetAbnormalityAtlasKey",
    "LogueBookModels.IsAtlasRoleBookUnlocked(id)",
    "LogueBookModels.IsAtlasBattleCardUnlocked(id)"
)) {
    Require-Contains $atlas $pattern "atlas unlocked ordering and de-duplication"
}

"RMR ATLAS UNLOCK CHECK PASSED"
