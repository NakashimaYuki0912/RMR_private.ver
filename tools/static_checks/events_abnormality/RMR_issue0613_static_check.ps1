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
function Assert-True {
    param(
        [bool] $Condition,
        [string] $Message
    )
    if (-not $Condition) {
        throw $Message
    }
}

function Read-Text {
    param([string] $Path)
    return [System.IO.File]::ReadAllText((Join-Path $script:RepoRoot $Path))
}

$stageCh1 = Read-Text "SpecialStaticInfo/StagesXmlInfos/Stage_ch1.xml"
$stageCh2 = Read-Text "SpecialStaticInfo/StagesXmlInfos/Stage_ch2.xml"
$shopCard = Read-Text "abcdcode_LOGLIKE_MOD/ShopGoods_Card.cs"
$shopBase = Read-Text "abcdcode_LOGLIKE_MOD/ShopBase.cs"
$abnoUnlocks = Read-Text "RMR_AbnormalityUnlocks.cs"
$patches = Read-Text "abcdcode_Refactored/LogLikePatches.cs"
$core = Read-Text "RMR_Core.cs"
$rmrChStart = Read-Text "SpecialStaticInfo/MysteryXmlInfos/RMR_chstart.xml"
$rmrChStartCn = Read-Text "Localize/cn/MysteryEvents/RMR_chstart.xml"

Assert-True ($stageCh1 -notmatch 'StageType="Creature"') "Grade1 must not generate Creature nodes; use Rest instead."
Assert-True ($stageCh2 -notmatch 'StageType="Creature"') "Grade2 must not generate Creature nodes; use Rest instead."
Assert-True ($stageCh1 -match 'ID="991101"\s+StageType="Rest"') "Grade1 former abno placeholder 991101 must be Rest."
Assert-True ($stageCh2 -match 'ID="991102"\s+StageType="Rest"') "Grade2 former abno placeholder 991102 must be Rest."

Assert-True ($shopCard -match 'txt_cardNumbers\.gameObject\.SetActive\(false\)') "Shop combat pages must hide the old card count."
Assert-True ($shopCard -notmatch '--this\.count') "Shop combat page purchase must not decrement quantity."
Assert-True ($shopCard -match 'this\.gameObject\.SetActive\(false\);') "Shop combat page good should close after unlocking the type."

Assert-True ($shopBase -match 'GetShopShape_PassiveSection') "ShopBase must use sectioned passive item placement."
Assert-True ($shopBase -match 'ShopSection\.EquipPage') "ShopBase must place role books in their own section."
Assert-True ($shopBase -match 'ShopSection\.Abnormality') "ShopBase must place abnormality pages in their own section."

Assert-True ($abnoUnlocks -match 'IsRealizationRewardAvailable\(info\)') "Ordinary abnormality reward rolls must gate realization-exclusive pages by completed floor."

Assert-True ($patches -notmatch 'uiSephirahButton\.SetButtonState\(UISephirahButton\.ButtonState\.Close\);') "RMR battle setup must not forcibly close Sephirah buttons."
Assert-True ($core -match 'LoadCurrentChapterStory\(\)') "Gamemode initialization must use current chapter story helper."
Assert-True ($core -notmatch 'LoadStoryFile\(new LorId\(LogLikeMod\.ModId, 1\), null, true\)') "No route initialization should hardcode Chapter 1 story."
Assert-True ($rmrChStart -match 'Choice ID="6"') "Mirror start event must expose a realization battle choice."
Assert-True ($rmrChStartCn -match 'Choice ID="6"') "CN mirror localization must label the realization battle choice."

Write-Host "RMR issue 0613 static checks passed."


