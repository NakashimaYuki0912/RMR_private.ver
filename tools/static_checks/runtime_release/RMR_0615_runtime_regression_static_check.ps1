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
$bs = [char]92
$root = $script:RepoRoot
function Read-Text($rel) { Get-Content (Join-Path $root $rel) -Raw -Encoding UTF8 }
function AssertContains($label, $text, $needle) {
    if ($text -cnotlike "*$needle*") { throw "$label missing: $needle" }
}
function AssertNotContains($label, $text, $needle) {
    if ($text -clike "*$needle*") { throw "$label forbidden: $needle" }
}
$realization = Read-Text 'RMR_RealizationManager.cs'
$books = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'LogueBookModels.cs')
$shop = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'ShopBase.cs')
$shopUpgrade = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'ShopGoods_CardUpgrade.cs')
AssertContains 'Realization immediate transition helper' $realization 'FinishCurrentEventForRealizationTransition'
AssertContains 'Realization transition defeats current wave' $realization 'GetWave(Singleton<StageController>.Instance.CurrentWave).Defeat();'
AssertContains 'Realization uses vanilla stage id' $realization 'new LorId(string.Empty, stageIdNum)'
AssertContains 'Atlas sync includes player loadout helper' $books 'SyncPlayerLoadoutToPermanentAtlas'
AssertContains 'Atlas sync includes current deck cards' $books 'GetCardListFromCurrentDeck()'
AssertContains 'Atlas sync filters and records equipped player book' $books 'ShouldRecordRoleBookInPermanentAtlas(equippedPage)'
AssertContains 'CreatePlayer persists starter atlas' $books 'SyncCurrentInventoryToPermanentAtlas();'
AssertContains 'Permanent equip shop count two' $shop 'PermanentEquipPageShopCount = 2'
AssertContains 'Permanent abnormality shop count two' $shop 'PermanentAbnormalityShopCount = 2'
AssertContains 'Shop passive null guard' $shop 'passiveinfos == null || passiveinfos.Count == 0'
AssertContains 'Shop null pickup guard' $shop 'pickUp == null'
AssertContains 'Shop compact section layout helper' $shop 'GetSupplementalSectionBasePosition'
AssertContains 'Shop upgrade artwork key' $shopUpgrade 'Shop_CardUpgrade_Icon'
AssertNotContains 'Shop upgrade should not rely on arrow text' $shopUpgrade 'IconText.text = "↑"'
if (!(Test-Path (Join-Path $root ('ArtWork' + $bs + 'Shop_CardUpgrade_Icon.png')))) { throw 'Shop_CardUpgrade_Icon.png missing from source ArtWork' }
Write-Host 'RMR runtime regression static check passed.'

