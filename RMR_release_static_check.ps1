$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path

function Assert-Text {
    param(
        [string]$Path,
        [string]$Pattern,
        [string]$Message
    )
    $text = [System.IO.File]::ReadAllText($Path, [System.Text.Encoding]::UTF8)
    if ($text -notmatch $Pattern) {
        throw $Message
    }
}

function Assert-NoText {
    param(
        [string]$Path,
        [string]$Pattern,
        [string]$Message
    )
    $text = [System.IO.File]::ReadAllText($Path, [System.Text.Encoding]::UTF8)
    if ($text -match $Pattern) {
        throw $Message
    }
}

$stageCh7 = Join-Path $root "SpecialStaticInfo\StagesXmlInfos\Stage_ch7.xml"
$stageInfoCh7 = Join-Path $root "AddData\StageInfo\StageInfo_ch7.xml"
$shopBase = Join-Path $root "abcdcode_LOGLIKE_MOD\ShopBase.cs"
$shopGoodsPassive = Join-Path $root "abcdcode_LOGLIKE_MOD\ShopGoods_Passive.cs"
$rewardingModel = Join-Path $root "abcdcode_LOGLIKE_MOD\RewardingModel.cs"
$bufs = Join-Path $root "Localize\cn\EffectTexts\RMR_bufs.xml"

Assert-Text $stageCh7 'ID="70011"\s+StageType="Mystery"' "Grade7 must reference custom mystery event 70011."
Assert-Text $stageCh7 'ID="70012"\s+StageType="Mystery"' "Grade7 must reference custom mystery event 70012."
Assert-NoText $stageCh7 'ID="991001"|ID="991002"' "Grade7 should not reference older generic mystery ids when custom ch7 events exist."
Assert-Text $stageCh7 'ID="70009"\s+StageType="Boss"' "Grade7 should expose a third high-end boss candidate."
Assert-NoText $stageInfoCh7 '<Unit>50022</Unit>' "Red Mist should not be a Grade7 boss in this package."
Assert-Text $stageInfoCh7 '<Unit>60001</Unit>' "Grade7 should include real Impuritas Hana units, not only Grade6 proxy units."
Assert-Text $stageInfoCh7 '<Unit>1301011</Unit>' "Grade7 should include real Reverberation Ensemble units."
Assert-Text $stageInfoCh7 '<Unit>60005</Unit>' "Grade7 should include Black Silence as an Impuritas boss candidate."
Assert-Text $stageInfoCh7 '<Unit>1401011</Unit>' "Grade7 should include Twisted Reverberation Band boss units."
Assert-Text $stageInfoCh7 '<Unit>80001</Unit>' "Grade7 should include Head encounter boss units."
Assert-NoText $stageInfoCh7 '<Unit>50035</Unit>|<Unit>50038</Unit>|<Unit>50051</Unit>' "Grade7 should no longer use Purple Tear, Xiao, or Yan as its Impuritas boss pool."

Assert-NoText $shopBase '\+\s*100|\+\s*200' "Shop direct goods must use contiguous UI/save indices, not large offset indices."
Assert-Text $shopBase 'CreateShop_EquipPages\(equipNum,\s*passiveNum\)' "Equip page shop goods should start after passive slots."
Assert-Text $shopBase 'CreateShop_AbnormalityPages\(abnoNum,\s*passiveNum\s*\+\s*equipNum\)' "Abnormality shop goods should start after passive and equip slots."

Assert-NoText $shopGoodsPassive 'this\.GoodScript\.id\.LogGetSaveData\(\)' "Shop passive save data must not assume GoodScript exists."
Assert-Text $shopGoodsPassive 'EmotionCardList\.Exists\(x\s*=>\s*x\.id\s*==\s*this\.storedRewardInfo\.id\)' "Abnormality purchase should dedupe by LorId, not object reference."
Assert-NoText $shopGoodsPassive 'Abnormality Page -' "Abnormality shop tooltip should be localized through pickup text."

Assert-NoText $rewardingModel 'FindAll\(y\s*=>\s*x\s*==\s*y\)' "Emotion card reward dedupe must not use object reference equality."
Assert-Text $rewardingModel 'selectedEmotionIds' "Emotion card rewards should filter already selected pages by stable id."

[xml]([System.IO.File]::ReadAllText($bufs, [System.Text.Encoding]::UTF8)) | Out-Null
Assert-NoText $bufs '<Name>Zeal</Name>|<Name>Adapt</Name>|<Name>Restoration</Name>|<Name>En Garde</Name>' "Visible buff names in Chinese localization should not remain English."

"RMR release static checks passed."
