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

function Require-NotContains($content, $pattern, $label) {
    if ($content -match [regex]::Escape($pattern)) {
        throw "Forbidden ${label}: $pattern"
    }
}

$router = Read-Text "RMR_AbnormalityBattleRouter.cs"
foreach ($pattern in @(
    "class RMRAbnormalityBattleRouter",
    "GetCandidateStageIds",
    "PickStageForChapter",
    "LowTierStageIds",
    "MidTierStageIds",
    "HighTierStageIds",
    "201001",
    "202001",
    "203001",
    "204001",
    "205001",
    "206001",
    "207001",
    "210001",
    "208001",
    "209001"
)) {
    Require-Contains $router $pattern "abnormality battle router"
}

foreach ($finalStoryId in @(
    "201005",
    "202005",
    "203005",
    "204005",
    "205005",
    "206005",
    "207005",
    "208004",
    "209004",
    "210005",
    "210006",
    "210007",
    "210008",
    "210009"
)) {
    Require-NotContains $router $finalStoryId "final story abnormality battle id"
}

$patches = Read-Text "abcdcode_Refactored\LogLikePatches.cs"
Require-Contains $patches "RMRAbnormalityBattleRouter.PickStageForChapter" "creature stage routing hook"
Require-Contains $patches "stage.type == StageType.Creature" "creature stage branch"

$core = Read-Text "RogueLike Mod Reborn.csproj"
Require-Contains $core "RMR_AbnormalityBattleRouter.cs" "router compile include"

$stages = @(
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch1.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch2.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch3.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch4.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch5.xml",
    "SpecialStaticInfo\StagesXmlInfos\Stage_ch6.xml"
)
foreach ($stageFile in $stages) {
    $content = Read-Text $stageFile
    Require-Contains $content 'StageType="Creature"' "route creature card in $stageFile"
}

"RMR ABNORMALITY BATTLE STATIC CHECK PASSED"
