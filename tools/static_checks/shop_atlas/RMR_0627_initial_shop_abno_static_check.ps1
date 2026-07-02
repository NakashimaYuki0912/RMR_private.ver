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
function Must([bool]$Condition, [string]$Message) { if (-not $Condition) { throw $Message } }

$chStartCode = Read-Text 'RMR_MysteryEvents.cs'
$shop = Read-Text 'abcdcode_LOGLIKE_MOD\ShopBase.cs'
$shopCard = Read-Text 'abcdcode_LOGLIKE_MOD\ShopGoods_Card.cs'
$bookModels = Read-Text 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs'
$csproj = Read-Text 'RogueLike Mod Reborn.csproj'
$chStartXmlPath = 'SpecialStaticInfo\MysteryXmlInfos\RMR_chstart.xml'
$chStartXml = [xml](Read-Text $chStartXmlPath)

$choiceIds = @($chStartXml.MysteryXmlRoot.Mystery.Frame |
    Where-Object { $_.ID -eq '0' } |
    ForEach-Object { $_.Choice.ID })
Must ($choiceIds.Count -eq 7) 'Initial relic event must expose six normal relic choices plus the realization entry.'
Must ($choiceIds -contains '6') 'Initial relic event is missing the floor realization choice slot.'
Must ($chStartCode -match 'case 6:') 'Floor realization entry is not handled by the active initial event script.'
Must ($chStartCode -match 'InitialRelicEntryAvailable') 'Realization entry is not gated through the initial relic availability flag.'
Must ($chStartCode -match 'SetInitialRelicEntryAvailable\(false\)') 'Normal relic choices do not close the one-shot realization gate.'

Must ($shop -match 'int equipNum = 2;') 'Shop must create two role book slots.'
Must ($shop -match 'int abnoNum = 2;') 'Shop must create two abnormality page slots.'
Must ($shop -match 'int egoNum = 2;') 'Shop must create two EGO page slots.'
Must ($shop -match 'ShopSection\.EquipPage:[\s\r\n]+return new Vector2\(-730f, 260f\)') 'Role books are not anchored to the left side.'
Must ($shop -match 'ShopSection\.EgoPage:[\s\r\n]+return new Vector2\(730f, 220f\)') 'EGO pages are not anchored to the right side.'
Must ($shop -match 'CreateShop_CardUpgrade') 'Shop card upgrade goods are not created.'
Must ($bookModels -match 'shopUpgradeCardPrice') 'Shop upgrade price is not persisted on route data.'
Must ($csproj -match 'abcdcode_LOGLIKE_MOD\\ShopGoods_CardUpgrade.cs') 'ShopGoods_CardUpgrade.cs is not compiled into the DLL.'
Must ($shopCard -match 'txt_cardNumbers\.gameObject\.SetActive\(false\)') 'Combat page shop goods still display a copy count.'

foreach ($path in @('SpecialStaticInfo\StagesXmlInfos\Stage_ch1.xml', 'SpecialStaticInfo\StagesXmlInfos\Stage_ch2.xml')) {
    $xml = [xml](Read-Text $path)
    $creature = @($xml.StagesXmlRoot.ChapterList.StageList | Where-Object { $_.StageType -eq 'Creature' })
    Must ($creature.Count -eq 0) "$path must not contain abnormality battle nodes."
}
$ch3 = [xml](Read-Text 'SpecialStaticInfo\StagesXmlInfos\Stage_ch3.xml')
$ch3Creature = @($ch3.StagesXmlRoot.ChapterList.StageList | Where-Object { $_.StageType -eq 'Creature' })
Must ($ch3Creature.Count -gt 0) 'Chapter 3 must be the first chapter with abnormality battle nodes.'

Write-Output 'PASS: initial relic choices, shop sidebars/upgrades, card count display, and chapter 3 abnormality gate are present.'

