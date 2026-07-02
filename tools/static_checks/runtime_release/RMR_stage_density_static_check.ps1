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

$models = Read-Text "abcdcode_LOGLIKE_MOD\LogueBookModels.cs"

foreach ($pattern in @(
    "int numCreature = stageLimits.Creature",
    "StageType.Creature && numCreature > 0",
    "--numCreature",
    "public int Creature",
    "Normal = 2",
    "Normal = 4",
    "Normal = 5",
    "Normal = 6",
    "Creature = 0",
    "Creature = 1"
)) {
    Require-Contains $models $pattern "expanded stage density and creature stage picking"
}

$earlyStageFiles = @(
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch1.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch2.xml"
)
foreach ($stageFile in $earlyStageFiles) {
    $content = Read-Text $stageFile
    if ($content -match [regex]::Escape('StageType="Creature"')) {
        throw "Forbidden creature stage in early chapter: $stageFile"
    }
    Require-Contains $content 'StageType="Rest"' "rest replacement in $stageFile"
}

$stageFiles = @(
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

