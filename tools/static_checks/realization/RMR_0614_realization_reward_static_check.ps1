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
function ReadText($rel) { Get-Content -LiteralPath (Join-Path $root $rel) -Raw -Encoding UTF8 }
function AssertContains($name, $text, $needle) {
    if ($text -notlike "*$needle*") { throw "$name missing: $needle" }
}
function AssertNotContains($name, $text, $needle) {
    if ($text -like "*$needle*") { throw "$name should not contain: $needle" }
}

$unlock = ReadText 'RMR_AbnormalityUnlocks.cs'
$shop = ReadText 'abcdcode_LOGLIKE_MOD\ShopBase.cs'
$rewarding = ReadText 'abcdcode_LOGLIKE_MOD\RewardingModel.cs'
$realization = ReadText 'RMR_RealizationManager.cs'
$cnStart = ReadText 'Localize\cn\MysteryEvents\RMR_chstart.xml'
$enStart = ReadText 'Localize\en\MysteryEvents\RMR_chstart.xml'

AssertContains 'CN start choice' $cnStart '<Choice ID="6">'
AssertContains 'EN start choice' $enStart '<Choice ID="6">'
AssertContains 'Unlock manager Yesod music' $unlock 'SingingMachine1'
AssertContains 'Unlock manager Yesod coffin' $unlock 'Butterfly3'
AssertContains 'Unlock manager Yesod black flame/freischutz' $unlock 'freischutz3'
AssertContains 'Unlock manager Yesod EGO 910011' $unlock '910011'
AssertContains 'Unlock manager Yesod EGO 910012' $unlock '910012'
AssertContains 'Unlock manager Yesod EGO 910013' $unlock '910013'
AssertContains 'Unlock manager Yesod EGO 910014' $unlock '910014'
AssertContains 'Unlock manager Yesod EGO 910015' $unlock '910015'
AssertContains 'Unlock manager gated candidates' $unlock 'IsRealizationRewardAvailable(info)'
AssertContains 'Unlock manager shop abno helper' $unlock 'GetShopEligibleAbnormalityPages'
AssertContains 'Unlock manager shop ego helper' $unlock 'GetUnlockedRealizationEgoCardsForRewards'
$completeStart = $unlock.IndexOf('public static void CompleteFloorRealization')
$completeEnd = $unlock.IndexOf('public static HashSet<SephirahType> GetFloorsForChapter', $completeStart)
if ($completeStart -lt 0 -or $completeEnd -lt 0) { throw 'CompleteFloorRealization block not found for direct grant check' }
$completeBlock = $unlock.Substring($completeStart, $completeEnd - $completeStart)
AssertNotContains 'CompleteFloorRealization direct grants' $completeBlock 'UnlockPage('

# EGO now handled via independent egoSelectionQueue in StartPickReward, not mixed into PickUpCards
AssertNotContains 'RewardingModel EGO removed from battle pool' $rewarding 'GetUnlockedRealizationEgoCardsForRewards'
AssertContains 'RewardingModel EGO queue' $rewarding 'egoSelectionQueue'
AssertContains 'ShopBase EGO section' $shop 'EgoPage'
AssertContains 'ShopBase EGO creator' $shop 'CreateShop_EgoPages'
AssertContains 'ShopBase abnormality helper' $shop 'GetShopEligibleAbnormalityPages'
AssertContains 'Realization manager ends mystery' $realization 'Singleton<MysteryManager>.Instance.EndMystery'
AssertContains 'Realization manager atlas loadout start' $realization 'ApplyAtlasOnlyLoadout'
AssertContains 'Realization manager atlas loadout restore' $realization 'RestoreRouteLoadout'
AssertContains 'Realization manager atlas books' $realization 'AtlasUnlockedRoleBooks'
AssertContains 'Realization manager atlas cards' $realization 'AtlasUnlockedBattleCards'

Write-Host 'RMR 0614 realization reward static check passed.'

