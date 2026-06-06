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
    "int numCreature = stageLimits.Creature",
    "StageType.Creature && numCreature > 0",
    "--numCreature",
    "public int Creature",
    "Normal = 6",
    "Normal = 5",
    "Normal = 10",
    "Normal = 11",
    "Creature = 1"
)) {
    Require-Contains $models $pattern "expanded stage density and creature stage picking"
}

$stageFiles = @(
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch1.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch2.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch3.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch4.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch5.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch6.xml"
)

foreach ($stageFile in $stageFiles) {
    $content = Read-Text $stageFile
    Require-Contains $content 'StageType="Creature"' "creature stage in $stageFile"
}

"RMR STAGE DENSITY CHECK PASSED"
