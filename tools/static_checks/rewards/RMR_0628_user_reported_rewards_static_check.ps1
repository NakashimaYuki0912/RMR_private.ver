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
Set-Location $root

$failures = New-Object System.Collections.Generic.List[string]

function Assert-Contains {
    param(
        [string]$Path,
        [string]$Pattern,
        [string]$Message
    )
    $content = Get-Content -Raw -Encoding UTF8 $Path
    if ($content -notmatch $Pattern) {
        $failures.Add($Message)
    }
}

Assert-Contains '.\abcdcode_LOGLIKE_MOD\RewardingModel.cs' 'NormalBattleCardRewardRetentionRate\s*=\s*0\.49' 'normal battle card reward retention should be reduced by about 30 percent from 0.70'
Assert-Contains '.\abcdcode_LOGLIKE_MOD\RewardingModel.cs' 'GetBattleCardRewardRetentionRate\(\)' 'drop book reward normalization should use stage-aware retention'
Assert-Contains '.\abcdcode_LOGLIKE_MOD\RewardingModel.cs' 'GetDropBookRewardSelectionCap\(\)' 'late normal drop-book reward selections should be capped'
Assert-Contains '.\abcdcode_LOGLIKE_MOD\ShopBase.cs' 'case ShopSection\.EgoPage:\s*return new Vector2\(560f,\s*220f\)' 'shop EGO cards should be shifted left to keep tooltip/description on screen'
Assert-Contains '.\abcdcode_LOGLIKE_MOD\ShopGoods_Card.cs' 'IsEgoPage \? -250f : 250f' 'shop EGO detail preview should open toward the center'
Assert-Contains '.\abcdcode_LOGLIKE_MOD\LogLikeMod.cs' 'ApplyVanillaEmotionPresentation\(info,\s*emotionCardXmlInfo\)' 'registered abnormality reward cards should use vanilla presentation data where available'
Assert-Contains '.\RMR_AbnormalityUnlocks.cs' 'BossRealizationRewardScriptsByFloor\s*=\s*FloorAbnormalityScripts' 'boss realization abnormality rewards should use full completed-floor pools'
Assert-Contains '.\RMR_AbnormalityUnlocks.cs' 'GetBossRealizationRewardFloorForScript\(info\.script\)' 'boss realization abnormality rolls should use boss-specific floor matching'
Assert-Contains '.\abcdcode_Refactored\LogLikePatches.cs' 'CapturePurpleTransitionEmotionState\(\)' 'Purple Tear transition should capture librarian emotion before forced floor switch'
Assert-Contains '.\abcdcode_Refactored\LogLikePatches.cs' 'RestorePurpleTransitionEmotionState\(emotionStates\)' 'Purple Tear transition should restore librarian emotion after forced floor switch'
Assert-Contains '.\abcdcode_Refactored\LogLikePatches.cs' 'if\s*\(LogLikeMod\.purpleexcept\)\s*\{\s*orig\(self,\s*deltaTime\);\s*return;' 'Purple Tear transition should skip roguelike end-battle reward cleanup'

[xml](Get-Content -Raw -Encoding UTF8 '.\SpecialStaticInfo\DropValueXmlInfos\values_ch7.txt') | Out-Null
[xml](Get-Content -Raw -Encoding UTF8 '.\AddData\StageInfo\StageInfo_ch7.xml') | Out-Null
[xml](Get-Content -Raw -Encoding UTF8 '.\SpecialStaticInfo\StagesXmlInfos\Stage_ch7.xml') | Out-Null
[xml](Get-Content -Raw -Encoding UTF8 '.\AddData\CardDropTable\CardDropTable_ch7.xml') | Out-Null
[xml](Get-Content -Raw -Encoding UTF8 '.\SpecialStaticInfo\RewardPassiveInfos\EquipReward_ch7.xml') | Out-Null

$dropValues = [xml](Get-Content -Raw -Encoding UTF8 '.\SpecialStaticInfo\DropValueXmlInfos\values_ch7.txt')
$dropValueIds = @($dropValues.CardDropValueXmlRoot.DropValue | ForEach-Object { $_.ID })
foreach ($id in '7001','7002','7003','7004','17001') {
    if ($dropValueIds -notcontains $id) {
        $failures.Add("missing chapter 7 DropValue $id")
    }
}

$stages = [xml](Get-Content -Raw -Encoding UTF8 '.\AddData\StageInfo\StageInfo_ch7.xml')
$blackSilence = @($stages.StageXmlRoot.Stage | Where-Object { $_.id -eq '70020' })[0]
if ($null -eq $blackSilence -or ($blackSilence.Wave.ManagerScript | Select-Object -First 1) -ne 'BlackSilence') {
    $failures.Add('stage 70020 should be the full Black Silence boss flow')
}
$twisted = @($stages.StageXmlRoot.Stage | Where-Object { $_.id -eq '70021' })[0]
if ($null -eq $twisted -or @($twisted.Wave).Count -ne 3) {
    $failures.Add('stage 70021 should be the full three-wave Twisted Reverberation Band boss')
}
if ($null -eq ($twisted.Wave | Where-Object { $_.ManagerScript -eq 'TwistedReverberationBand_Middle' })) {
    $failures.Add('stage 70021 should include the middle Twisted Reverberation Band manager')
}

$route = [xml](Get-Content -Raw -Encoding UTF8 '.\SpecialStaticInfo\StagesXmlInfos\Stage_ch7.xml')
$normalIds = @($route.StagesXmlRoot.ChapterList.StageList | Where-Object { $_.StageType -eq 'Normal' } | ForEach-Object { $_.ID })
$bossIds = @($route.StagesXmlRoot.ChapterList.StageList | Where-Object { $_.StageType -eq 'Boss' } | ForEach-Object { $_.ID })
foreach ($normalId in '70001','70002','70003','70004','70005','70006','70007','70008','70009','70010') {
    if ($normalIds -notcontains $normalId) {
        $failures.Add("chapter 7 normal pool should contain Reverberation Ensemble member stage $normalId")
    }
}
if ($bossIds.Count -ne 2 -or $bossIds -notcontains '70020' -or $bossIds -notcontains '70021') {
    $failures.Add('chapter 7 boss pool should contain only 70020 and 70021')
}

$dropTable = [xml](Get-Content -Raw -Encoding UTF8 '.\AddData\CardDropTable\CardDropTable_ch7.xml')
$combatCards = @($dropTable.CardDropTableXmlRoot.DropTable.Card | ForEach-Object { $_.InnerText })
foreach ($cardId in '704001','704002','704003','704004','704005','704006','704007','704008','704009','704010','704011','704012','704013','704014','704015','704016','704018') {
    if ($combatCards -notcontains $cardId) {
        $failures.Add("chapter 7 Reverberation Ensemble drop table missing card $cardId")
    }
}
if ($combatCards -contains '701001') {
    $failures.Add('chapter 7 combat-page pool should not remain Hana-only')
}

$equipReward = [xml](Get-Content -Raw -Encoding UTF8 '.\SpecialStaticInfo\RewardPassiveInfos\EquipReward_ch7.xml')
$equipIds = @($equipReward.RewardPassivesRoot.ChapterList.RewardList | ForEach-Object { $_.ID })
foreach ($equipId in '260001','260002','260003','260004','260005','260006','260007','260008','260009','260010','260011','260012','260013','260014') {
    if ($equipIds -notcontains $equipId) {
        $failures.Add("chapter 7 impurity core-page pool missing $equipId")
    }
}

if ($failures.Count -gt 0) {
    Write-Host "STATIC_FAILED=$($failures.Count)"
    foreach ($failure in $failures) {
        Write-Host "FAIL: $failure"
    }
    exit 1
}

Write-Host 'STATIC_TOTAL=15 STATIC_FAILED=0'

