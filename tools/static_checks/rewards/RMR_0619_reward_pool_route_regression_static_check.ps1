$ErrorActionPreference = 'Stop'

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
function Read-Text([string]$relativePath) {
    Get-Content -Raw -Encoding UTF8 (Join-Path $root $relativePath)
}

function Assert-Match([string]$label, [string]$text, [string]$pattern) {
    if ($text -notmatch $pattern) {
        throw "FAIL: $label"
    }
    Write-Host "PASS: $label"
}

function Assert-NoMatch([string]$label, [string]$text, [string]$pattern) {
    if ($text -match $pattern) {
        throw "FAIL: $label"
    }
    Write-Host "PASS: $label"
}

$logLikeMod = Read-Text 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs'
$rewarding = Read-Text 'abcdcode_LOGLIKE_MOD\RewardingModel.cs'
$unlocks = Read-Text 'RMR_AbnormalityUnlocks.cs'
$books = Read-Text 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs'
$dropTable = Read-Text 'AddData\CardDropTable\CardDropTable_ch6.xml'
$redMistBook = Read-Text 'AddData\EquipPage\EquipPage_Librarian_ch6.xml'

$loadStagesStart = $logLikeMod.IndexOf('public void LoadStages()')
$loadStagesEnd = $logLikeMod.IndexOf('public static CardDropValueXmlRoot LoadDropValues', $loadStagesStart)
if ($loadStagesStart -lt 0 -or $loadStagesEnd -lt 0) {
    throw 'FAIL: LoadStages block not found'
}
$loadStages = $logLikeMod.Substring($loadStagesStart, $loadStagesEnd - $loadStagesStart)

Assert-NoMatch 'Stage loader does not deserialize backup files' $loadStages 'GetFiles\s*\(\s*\)'
Assert-Match 'Stage loader reads only XML files for primary and satellite paths' $loadStages 'GetFiles\s*\(\s*"\*\.xml"\s*\)[\s\S]*GetFiles\s*\(\s*"\*\.xml"\s*\)'

Assert-NoMatch 'Urban Star public drop table excludes Red Mist, Black Silence and Binah exclusive cards' $dropTable '<Card>607(?:0|1|2)\d{2}</Card>'
Assert-Match 'Red Mist core page keeps exclusive OnlyCard pages' $redMistBook '<Book ID="250022">[\s\S]*<OnlyCard>607001</OnlyCard>[\s\S]*<OnlyCard>607008</OnlyCard>'

Assert-Match 'Battle reward queue retains about seventy percent of each non-null drop-book group' $rewarding 'BattleCardRewardRetentionRate\s*=\s*0\.7[\s\S]*GroupBy\(reward\s*=>\s*reward\.id\)'
Assert-Match 'Boss without an enemy drop book receives a chapter fallback' $rewarding 'EnsureBossBattleCardReward\s*\(\s*\)[\s\S]*chapterNumber \* 1000 \+ 4'

Assert-Match 'Red Mist challenge suppresses generic route rewards' $unlocks 'if\s*\(IsRedMistChallengeStage\(\)\)\s*return;[\s\S]*curstagetype == abcdcode_LOGLIKE_MOD\.StageType\.Creature'
Assert-Match 'Red Mist victory uses actual stage 60020' $unlocks 'RedMistStageId\s*=\s*60020'
Assert-Match 'Role-book grant repairs route inventory and conditionally records permanent pages' $books 'if \(!LogueBookModels\.booklist\.Any[\s\S]*ShouldRecordRoleBookInPermanentAtlas\(bookXml\)[\s\S]*if \(recordsPermanently && !LogueBookModels\.AtlasUnlockedRoleBooks\.Contains'
Assert-Match 'Old saves prune core-page exclusive cards from public inventory and atlas' $books 'SyncCurrentInventoryToPermanentAtlas\s*\(\s*\)\s*;[\s\S]*PruneCorePageExclusiveBattleCardsFromInventoryAndAtlas\s*\(\s*\)'
Assert-Match 'Equipping a fixed deck does not leave exclusive cards in public inventory' $books 'TrySetGrade6SpecialBuiltInDeckSource\s*\(\s*unitData\s*,\s*page\s*\)\s*;[\s\S]*PruneCorePageExclusiveBattleCardsFromInventoryAndAtlas\s*\(\s*\)'

Write-Host 'PASS: reward pool, exclusive card, Red Mist unlock and stage-loader regression checks completed.'

