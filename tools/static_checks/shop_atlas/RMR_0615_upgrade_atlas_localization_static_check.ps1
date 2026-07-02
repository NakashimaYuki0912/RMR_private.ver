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
function ReadUtf8($rel) { [System.IO.File]::ReadAllText((Join-Path $root $rel), [System.Text.Encoding]::UTF8) }
function AssertContains($label, $text, $needle) { if ($text -cnotlike "*$needle*") { throw "$label missing: $needle" } }
function AssertNotContains($label, $text, $needle) { if ($text -clike "*$needle*") { throw "$label still contains: $needle" } }

$cnAbility = ReadUtf8 ('Localize' + $bs + 'cn' + $bs + 'DiceAbilityInfo' + $bs + 'UpgradeCardAbility.txt')
foreach ($bad in @('Unity','Start of Clash','Combat Start','On Use','Power gain or loss','unaffected by Power','Cannot be stacked','using a Unity page')) {
    AssertNotContains 'CN upgrade ability localization' $cnAbility $bad
}

$choice = ReadUtf8 ('abcdcode_LOGLIKE_MOD' + $bs + 'MysteryModel_CardChoice.cs')
foreach ($needle in @('GradeFilterButton','CurrentGradeFilter','GetFilteredCards','GetChapterFilterName','SelectGradeFilter','SectionFromChapter')) {
    AssertContains 'Upgrade popup grade filter' $choice $needle
}
AssertContains 'Upgrade popup only shows count for real stack count' $choice 'ShouldShowCardCount(info)'

$book = ReadUtf8 ('abcdcode_LOGLIKE_MOD' + $bs + 'LogueBookModels.cs')
AssertContains 'Atlas migration helper' $book 'SyncCurrentInventoryToPermanentAtlas'
AssertContains 'Atlas migration save' $book 'SavePermanentAtlasUnlocks();'
AssertContains 'Atlas migration called after route load' $book 'SyncCurrentInventoryToPermanentAtlas();'

$shopUpgrade = ReadUtf8 ('abcdcode_LOGLIKE_MOD' + $bs + 'ShopGoods_CardUpgrade.cs')
AssertContains 'Shop upgrade artwork key' $shopUpgrade 'Shop_CardUpgrade_Icon'
AssertNotContains 'Shop upgrade no arrow fallback as primary icon' $shopUpgrade 'IconText.text = "↑"'

if (!(Test-Path (Join-Path $root ('ArtWork' + $bs + 'Shop_CardUpgrade_Icon.png')))) { throw 'Shop_CardUpgrade_Icon.png missing' }
Write-Host 'RMR upgrade atlas localization static check passed.'
