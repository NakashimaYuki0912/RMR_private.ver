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

function Require-NotContains($content, $pattern, $label) {
    if ($content -match [regex]::Escape($pattern)) {
        throw "Forbidden ${label}: $pattern"
    }
}

function Require-LocalizedNode($path, $xpath, $fields) {
    $xml = New-Object System.Xml.XmlDocument
    $xml.PreserveWhitespace = $true
    $xml.Load((Join-Path $root $path))
    $node = $xml.SelectSingleNode($xpath)
    if ($null -eq $node) {
        throw "Missing localized node: $path $xpath"
    }
    foreach ($field in $fields) {
        $text = $node.SelectSingleNode($field).InnerText
        if ([string]::IsNullOrWhiteSpace($text) -or $text -notmatch "[^\u0000-\u007F]") {
            throw "Field is not localized: $path $xpath $field = $text"
        }
    }
}

$items = Read-Text "Localize\cn\LogueEffectText\RMR_chstart_items.xml"
$mystery = Read-Text "Localize\cn\MysteryEvents\RMR_chstart.xml"
$bufs = Read-Text "Localize\cn\EffectTexts\RMR_bufs.xml"
$effects = Read-Text "RMR_GlobalEffects.cs"

foreach ($pattern in @(
    "Roadless Camelot",
    "Choose [common]Roadless Camelot",
    "At the start of each Scene, a random librarian",
    "The crystallization of an utopia",
    "Each time this character uses a Combat Page"
)) {
    Require-NotContains $items $pattern "English chapter-start item text"
    Require-NotContains $mystery $pattern "English chapter-start mystery text"
    Require-NotContains $bufs $pattern "English chapter-start buff text"
}

Require-LocalizedNode "Localize\cn\LogueEffectText\RMR_chstart_items.xml" "//LogueEffectInfo[@Id='RMR_RoadlessCamelot']" @("Name", "Desc", "FlavorText", "CatalogDesc")
Require-LocalizedNode "Localize\cn\EffectTexts\RMR_bufs.xml" "//BattleEffectText[@ID='RMR_RoadlessCamelotBuf']" @("Name", "Desc")

$roadlessCamelotCn = -join @([char]0x65e0, [char]0x8def, [char]0x5361, [char]0x7f8e, [char]0x6d1b)
$roadlessFlavorCn = -join @([char]0x6ca1, [char]0x6709, [char]0x5165, [char]0x53e3, [char]0x7684, [char]0x7406, [char]0x60f3, [char]0x4e61)

foreach ($pattern in @(
    "public override string GetEffectName()",
    "public override string GetEffectDesc()",
    $roadlessCamelotCn,
    $roadlessFlavorCn
)) {
    if ($effects -notmatch [regex]::Escape($pattern)) {
        throw "Missing Roadless Camelot runtime Chinese fallback: $pattern"
    }
}

"RMR CHSTART LOCALIZATION CHECK PASSED"

