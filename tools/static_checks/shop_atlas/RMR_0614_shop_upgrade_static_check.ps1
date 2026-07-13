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
$shop = Get-Content (Join-Path $root 'abcdcode_LOGLIKE_MOD\ShopBase.cs') -Raw
$upgradePath = Join-Path $root 'abcdcode_LOGLIKE_MOD\ShopGoods_CardUpgrade.cs'
$csproj = Get-Content (Join-Path $root 'RogueLike Mod Reborn.csproj') -Raw
function AssertContains($label, $text, $needle) {
    if ($text -notlike "*$needle*") { throw "$label missing: $needle" }
}
AssertContains 'ShopSection card upgrade slot' $shop 'CardUpgrade'
AssertContains 'Upgrade base price' $shop 'UpgradeCardBasePrice = 10'
AssertContains 'Upgrade price step' $shop 'UpgradeCardPriceStep = 2'
AssertContains 'Upgrade creator method' $shop 'CreateShop_CardUpgrade'
AssertContains 'Upgrade save bucket' $shop 'Upgrades'
AssertContains 'Upgrade route save price' (Get-Content (Join-Path $root 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs') -Raw) 'shopUpgradeCardPrice'
AssertContains 'Spread wide shop layout left' $shop 'new Vector2(-730f'
AssertContains 'Spread wide shop layout right' $shop 'new Vector2(730f'
AssertContains 'Supplemental upgrade layout' $shop 'ShopSection.CardUpgrade'
if (!(Test-Path $upgradePath)) { throw 'ShopGoods_CardUpgrade.cs missing' }
$upgrade = Get-Content $upgradePath -Raw
AssertContains 'ShopGoods_CardUpgrade.cs compile include' $csproj 'abcdcode_LOGLIKE_MOD\ShopGoods_CardUpgrade.cs'
AssertContains 'Upgrade goods class' $upgrade 'class ShopGoods_CardUpgrade'
AssertContains 'Upgrade eligible card filter' $upgrade 'CheckCanUpgrade'
AssertContains 'Upgrade options lookup' $upgrade 'GetAllUpgradesCard'
AssertContains 'Upgrade applies new card' $upgrade 'LogueBookModels.AddCard'
AssertContains 'Upgrade removes old card' $upgrade 'LogueBookModels.DeleteCard'
AssertContains 'Upgrade increments price' $upgrade 'OnCardUpgradePurchased'
AssertContains 'Upgrade art icon key' $upgrade 'Shop_CardUpgrade_Icon'
if (!(Test-Path (Join-Path $root 'ArtWork\Shop_CardUpgrade_Icon.png'))) { throw 'Shop_CardUpgrade_Icon.png missing' }
if ($upgrade -clike '*IconText.text = "↑"*') { throw 'Upgrade goods still uses arrow text as primary icon' }
AssertContains 'Upgrade localized name key' $upgrade 'TextDataModel.GetText("Shop_CardUpgrade_Name")'
AssertContains 'Upgrade localized desc key' $upgrade 'TextDataModel.GetText("Shop_CardUpgrade_Desc")'
$cnUi = Get-Content (Join-Path $root 'Localize\cn\UIs.txt') -Raw -Encoding UTF8
$enUi = Get-Content (Join-Path $root 'Localize\en\UIs.txt') -Raw -Encoding UTF8
$krUi = Get-Content (Join-Path $root 'Localize\kr\UIs.txt') -Raw -Encoding UTF8
AssertContains 'CN upgrade name localization' $cnUi '<text id="Shop_CardUpgrade_Name">卡牌升级</text>'
AssertContains 'CN upgrade desc localization' $cnUi '<text id="Shop_CardUpgrade_Desc">卡牌升级</text>'
AssertContains 'EN upgrade name localization' $enUi '<text id="Shop_CardUpgrade_Name">Card Upgrade</text>'
AssertContains 'EN upgrade desc localization' $enUi '<text id="Shop_CardUpgrade_Desc">Card Upgrade</text>'
AssertContains 'KR upgrade name localization' $krUi '<text id="Shop_CardUpgrade_Name">카드 강화</text>'
AssertContains 'KR upgrade desc localization' $krUi '<text id="Shop_CardUpgrade_Desc">카드 강화</text>'
Write-Host 'RMR shop upgrade static check passed.'






