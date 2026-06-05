$ErrorActionPreference = "Stop"

$root = "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"
$requiredFiles = @(
  "RMR_AbnormalityUnlocks.cs",
  "RMR_MysteryEvents.cs",
  "abcdcode_Refactored\LogLikePatches.cs",
  "abcdcode_LOGLIKE_MOD\RewardingModel.cs",
  "abcdcode_LOGLIKE_MOD\LogueBookModels.cs",
  "Localize\cn\UIs.txt",
  "SpecialStaticInfo\MysteryXmlInfos\chAll_mysterys.xml"
)

foreach ($file in $requiredFiles) {
  if (!(Test-Path (Join-Path $root $file))) {
    throw "Missing required file: $file"
  }
}

$unlock = Get-Content (Join-Path $root "RMR_AbnormalityUnlocks.cs") -Raw
foreach ($pattern in @(
  "class RMRAbnormalityUnlockManager",
  "NormalDropChance",
  "EliteDropChance",
  "AbnormalityBattleRewardCount",
  "MysteryRewardCount",
  "GetUnlockedEmotionCardsForBattle",
  "EnqueueRewardSelections",
  "RecordPermanentClear",
  "GetNoAbnormalityFallback"
)) {
  if ($unlock -notmatch [regex]::Escape($pattern)) {
    throw "Unlock manager missing $pattern"
  }
}

$patches = Get-Content (Join-Path $root "abcdcode_Refactored\LogLikePatches.cs") -Raw
foreach ($pattern in @(
  "RMRAbnormalityUnlockManager.GetUnlockedEmotionCardsForBattle",
  "RMRAbnormalityUnlockManager.EnqueueBattleClearRewards",
  "RMRAbnormalityUnlockManager.RecordPermanentClear",
  "RMRAbnormalityUnlockManager.OnEmotionPagePicked",
  "StageType.Creature"
)) {
  if ($patches -notmatch [regex]::Escape($pattern)) {
    throw "Patch missing $pattern"
  }
}

$rewarding = Get-Content (Join-Path $root "abcdcode_LOGLIKE_MOD\RewardingModel.cs") -Raw
foreach ($pattern in @(
  "RMRAbnormalityUnlockManager.GetNoAbnormalityFallback"
)) {
  if ($rewarding -notmatch [regex]::Escape($pattern)) {
    throw "RewardingModel missing $pattern"
  }
}

$logue = Get-Content (Join-Path $root "abcdcode_LOGLIKE_MOD\LogueBookModels.cs") -Raw
foreach ($pattern in @(
  "RMRAbnormalityUnlockManager.StartNewRoute",
  "RMRAbnormalityUnlockManager.SaveRouteUnlocks",
  "RMRAbnormalityUnlockManager.LoadRouteUnlocks"
)) {
  if ($logue -notmatch [regex]::Escape($pattern)) {
    throw "LogueBookModels missing $pattern"
  }
}

$events = Get-Content (Join-Path $root "RMR_MysteryEvents.cs") -Raw
foreach ($pattern in @(
  "MysteryModel_RMR_AbnoBlackForest",
  "MysteryModel_RMR_AbnoWell",
  "MysteryModel_RMR_AbnoHandsOfLight",
  "RMRAbnormalityUnlockManager.EnqueueRewardSelections"
)) {
  if ($events -notmatch [regex]::Escape($pattern)) {
    throw "Mystery events missing $pattern"
  }
}

$ui = Get-Content (Join-Path $root "Localize\cn\UIs.txt") -Raw
foreach ($pattern in @(
  "Stage_Creature",
  "Stage_Creature_Desc"
)) {
  if ($ui -notmatch [regex]::Escape($pattern)) {
    throw "Missing UI localize $pattern"
  }
}

$mysteryXml = Get-Content (Join-Path $root "SpecialStaticInfo\MysteryXmlInfos\chAll_mysterys.xml") -Raw
foreach ($pattern in @(
  "RMR_AbnoBlackForest",
  "RMR_AbnoWell",
  "RMR_AbnoHandsOfLight",
  "RMR_AbnoEvent_BlackForest",
  "RMR_AbnoEvent_Well",
  "RMR_AbnoEvent_HandsOfLight"
)) {
  if ($mysteryXml -notmatch [regex]::Escape($pattern)) {
    throw "Mystery XML missing $pattern"
  }
}

$stageFiles = @("Stage_ch1.xml", "Stage_ch2.xml", "Stage_ch3.xml", "Stage_ch4.xml", "Stage_ch5.xml", "Stage_ch6.xml")
foreach ($stageFile in $stageFiles) {
  $text = Get-Content (Join-Path $root "SpecialStaticInfo\StagesXmlInfos\$stageFile") -Raw
  if ($text -notmatch 'StageType="Creature"') {
    throw "$stageFile missing Creature stage"
  }
}

"ABNORMALITY UNLOCK STATIC CHECK PASSED"
