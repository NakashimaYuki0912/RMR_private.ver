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
$path = Join-Path $root "Localize\cn\LogueEffectText\GlobalEffect.xml"
$cn = Get-Content -LiteralPath $path -Raw -Encoding UTF8

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

foreach ($pattern in @(
    "<Name>Timepiece</Name>",
    "<Name>Joker Card</Name>",
    "<Name>Moonstone Gyroscope</Name>",
    "<Name>Duffel Bag</Name>",
    "<Name>Avalon Stimpack</Name>",
    "<Name>Budget 'DIY-Workshopper' Toolkit</Name>",
    "<Name>Sentinel Bracers</Name>",
    "<Name>Concept Converter</Name>",
    "<Name>Color Mixer</Name>",
    "<Name>Taking Point: Expose Weakness</Name>",
    "<Name>Construction and You: Finding Weaknesses</Name>",
    "At the end of the 7th Scene",
    "At the start of the Act",
    "[No entry for now.]"
)) {
    Require-NotContains $cn $pattern "English shop localization"
}

$xml = New-Object System.Xml.XmlDocument
$xml.PreserveWhitespace = $true
$xml.Load($path)

foreach ($id in @(
    "RMR_Timepiece",
    "RMR_Jokercard",
    "RMR_MoonstoneGyro",
    "RMR_Duffelbag",
    "RMR_AvalonStimpack",
    "RMR_BudgetToolkit",
    "RMR_SentinelBracers",
    "RMR_ConceptConverter",
    "RMR_ColorMixer",
    "RMR_WhiteCotton",
    "RMR_ExposeWeakness",
    "RMR_FindingWeakness"
)) {
    $node = $xml.SelectSingleNode("//LogueEffectInfo[@Id='$id']")
    if ($null -eq $node) {
        throw "Missing localized item node: $id"
    }
    foreach ($field in @("Name", "Desc", "FlavorText")) {
        $text = $node.SelectSingleNode($field).InnerText
        if ([string]::IsNullOrWhiteSpace($text) -or $text -notmatch "[^\u0000-\u007F]") {
            throw "Item $id $field is not localized: $text"
        }
    }
}

"RMR SHOP LOCALIZATION CHECK PASSED"

