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

$atlas = Read-Text "abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs"

foreach ($pattern in @(
    "showUpgradedBattleCards",
    "CreateBattleCardUpgradeToggle",
    "SetBattleCardUpgradeToggleVisible",
    "BuildEntries(showUpgradedBattleCards)",
    "BuildBattleCardEntries(bool showUpgraded)",
    "List<DiceCardXmlInfo> sourceCards",
    "GetDisplayCardInfo(info, showUpgraded)",
    "Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard",
    "BuildBattleCardDescription(displayInfo, id, showUpgraded)",
    "AtlasUpgradeToggleLabel"
)) {
    Require-Contains $atlas $pattern "battle card upgrade atlas toggle"
}

"RMR ATLAS UPGRADE CHECK PASSED"
