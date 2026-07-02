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
function Read-Text([string]$Path) { Get-Content -Raw -Encoding UTF8 $Path }
function Must([bool]$Condition, [string]$Message) { if (-not $Condition) { throw "FAIL: $Message" } }

$rewarding = Read-Text 'abcdcode_LOGLIKE_MOD\RewardingModel.cs'
$shopCard = Read-Text 'abcdcode_LOGLIKE_MOD\ShopGoods_Card.cs'
$patches = Read-Text 'abcdcode_Refactored\LogLikePatches.cs'
$vanillaEmotion = Read-Text 'abcdcode_LOGLIKE_MOD\PickUpModel_RMRVanillaEmotion.cs'
$loader = Read-Text 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs'
$dropValues = Read-Text 'SpecialStaticInfo\DropValueXmlInfos\values_ch7.txt'
$dropTable = Read-Text 'AddData\CardDropTable\CardDropTable_ch7.xml'
$equipRewards = Read-Text 'SpecialStaticInfo\RewardPassiveInfos\EquipReward_ch7.xml'

Must ($rewarding -match 'BattleCardRewardRetentionRate\s*=\s*0\.7') 'Battle drop-book copies are not reduced by roughly 30 percent.'
Must ($rewarding -match 'PickUpCards\(CardDropValueXmlInfo info\)[\s\S]*if \(info == null\)') 'Missing DropValue still crashes PickUpCards.'
Must ($shopCard -match 'IsEgoPage \? -250f : 250f') 'Right-side EGO detail preview is not opened toward the center.'
Must ($patches -match 'ResetBattleEgoSelectionState' -and $patches -match 'if \(isPurpleTransition\)[\s\S]*orig\(self\);[\s\S]*return;') 'EGO state is not reset per battle or Purple Tear transitions still rebuild battle state.'
Must ($vanillaEmotion -match 'typeof\(EmotionCardAbilityBase\)\.Assembly' -and $vanillaEmotion -match 'StringComparison\.OrdinalIgnoreCase') 'Vanilla realization scripts are not searched in the base game assembly or are still case-sensitive.'
Must ($loader -match 'workshopid == "@origin"') 'Reward lists cannot reference vanilla key pages.'
Must ($dropValues -match '<DropValue ID="7004"') 'Impurity boss fallback DropValue 7004 is missing.'
[xml]$dropTableXml = $dropTable
$combatPageIds = @($dropTableXml.CardDropTableXmlRoot.DropTable.Card | ForEach-Object { [int]$_.'#text' })
$expectedCombatPageIds = @(704001, 704002, 704003, 704004, 704005, 704006, 704007, 704008, 704009, 704010, 704011, 704012, 704013, 704014, 704015, 704016, 704018)
$combatPagePoolIsExact = $combatPageIds.Count -eq $expectedCombatPageIds.Count -and
    @($expectedCombatPageIds | Where-Object { $_ -notin $combatPageIds }).Count -eq 0
Must $combatPagePoolIsExact 'Impurity combat-page pool must contain exactly Reverberation Ensemble pages 704001-704016 and 704018.'

[xml]$equipRewardXml = $equipRewards
$keyPageIds = @($equipRewardXml.RewardPassivesRoot.ChapterList.RewardList | ForEach-Object { [int]$_.ID })
$expectedKeyPageIds = @(260005, 260006, 260007, 260008, 260009, 260010, 260011, 260012, 260013, 260014)
$keyPagePoolIsExact = $equipRewardXml.RewardPassivesRoot.ChapterList.WorkShopID -eq '@origin' -and
    $keyPageIds.Count -eq $expectedKeyPageIds.Count -and
    @($expectedKeyPageIds | Where-Object { $_ -notin $keyPageIds }).Count -eq 0
Must $keyPagePoolIsExact 'Impurity key-page pool must contain exactly the ten Reverberation Ensemble core pages.'

[xml]$stageRoot = Get-Content -Raw -Encoding UTF8 'SpecialStaticInfo\StagesXmlInfos\Stage_ch7.xml'
$bossIds = @($stageRoot.StagesXmlRoot.ChapterList.StageList | Where-Object StageType -eq 'Boss' | ForEach-Object { [int]$_.ID })
Must ($bossIds.Count -eq 2 -and $bossIds -contains 70020 -and $bossIds -contains 70021) 'Impurity boss pool must contain only full Black Silence and full three-act Distorted Ensemble.'

[xml]$stageInfo = Get-Content -Raw -Encoding UTF8 'AddData\StageInfo\StageInfo_ch7.xml'
$ensemble = @($stageInfo.StageXmlRoot.Stage | Where-Object id -eq '70021')[0]
Must (@($ensemble.Wave).Count -eq 3) 'Distorted Ensemble boss is not a three-act reception.'

Write-Output 'PASS: 0621 runtime regression rules are present.'

