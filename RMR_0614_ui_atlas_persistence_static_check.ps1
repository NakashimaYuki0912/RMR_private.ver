$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
function ReadText($rel) { Get-Content -LiteralPath (Join-Path $root $rel) -Raw -Encoding UTF8 }
function AssertContains($name, $text, $needle) { if ($text -notlike "*$needle*") { throw "$name missing: $needle" } }
function AssertNotContains($name, $text, $needle) { if ($text -like "*$needle*") { throw "$name should not contain: $needle" } }

$patches = ReadText 'abcdcode_Refactored\LogLikePatches.cs'
$panel = ReadText 'abcdcode_LOGLIKE_MOD\LogRealizationPanel.cs'
$books = ReadText 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs'
$unlock = ReadText 'RMR_AbnormalityUnlocks.cs'
$manager = ReadText 'RMR_RealizationManager.cs'
$shop = ReadText 'abcdcode_LOGLIKE_MOD\ShopBase.cs'
$atlas = ReadText 'abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs'

AssertContains 'realization button direct text helper' $patches 'ApplyRealizationButtonText'
AssertContains 'realization button localized text key' $patches 'ui_RealizationDesc'
AssertContains 'realization panel title localization' $panel 'ui_RMR_RealizationTitle'
AssertContains 'realization panel subtitle localization' $panel 'ui_RMR_RealizationDesc'
AssertContains 'realization panel compact title position' $panel 'new Vector2(0f, 310f)'
AssertContains 'realization manager event transition helper' $manager 'FinishCurrentEventForRealizationTransition'
AssertContains 'realization manager defeats current event wave' $manager 'GetWave(Singleton<StageController>.Instance.CurrentWave).Defeat();'
AssertContains 'realization manager ends battle after queueing challenge' $manager 'EndBattle()'

AssertContains 'atlas persistent save name' $books 'RMR_AtlasPermanentUnlocks'
AssertContains 'atlas abnormality permanent set' $books 'AtlasUnlockedAbnormalityPages'
AssertContains 'atlas abnormality record method' $books 'RecordAtlasAbnormalityPage'
AssertContains 'atlas ego record method' $books 'RecordAtlasEgoPage'
AssertContains 'atlas permanent load' $books 'LoadPermanentAtlasUnlocks'
AssertContains 'atlas permanent save' $books 'SavePermanentAtlasUnlocks'
AssertContains 'atlas abnormality UI uses permanent set' $atlas 'IsAtlasAbnormalityPageUnlocked'
AssertContains 'atlas dedicated ego set' $books 'AtlasUnlockedEgoPages'
AssertContains 'atlas ego UI uses dedicated permanent set' $atlas 'IsAtlasEgoPageUnlocked'
AssertContains 'selected realization abnormality records to atlas' $unlock 'RecordAtlasAbnormalityPage'

$pickStart = $unlock.IndexOf('public static bool OnEmotionPagePicked')
$pickEnd = $unlock.IndexOf('public static EmotionCardXmlInfo GetNoAbnormalityFallback', $pickStart)
if ($pickStart -lt 0 -or $pickEnd -lt 0) { throw 'OnEmotionPagePicked block not found' }
$pickBlock = $unlock.Substring($pickStart, $pickEnd - $pickStart)
AssertContains 'only realization-exclusive picks become permanent' $pickBlock 'IsRealizationExclusive(info)'
AssertContains 'selected realization-exclusive picks record to atlas' $pickBlock 'RecordAtlasAbnormalityPage(info.id)'

$inventorySyncStart = $books.IndexOf('public static void SyncCurrentInventoryToPermanentAtlas')
$inventorySyncEnd = $books.IndexOf('private static bool SyncPlayerLoadoutToPermanentAtlas', $inventorySyncStart)
if ($inventorySyncStart -lt 0 -or $inventorySyncEnd -lt 0) { throw 'SyncCurrentInventoryToPermanentAtlas block not found' }
$inventorySyncBlock = $books.Substring($inventorySyncStart, $inventorySyncEnd - $inventorySyncStart)
AssertNotContains 'inventory atlas sync excludes route abnormality pages' $inventorySyncBlock 'SyncCurrentAbnormalityPagesToPermanentAtlas'

AssertNotContains 'route abnormality migration helper is removed' $books 'public static bool SyncCurrentAbnormalityPagesToPermanentAtlas'
AssertNotContains 'save loading never migrates route abnormality pages to permanent atlas' $books 'SyncCurrentAbnormalityPagesToPermanentAtlas();'
AssertContains 'old saves prune invalid permanent abnormality entries' $books 'PruneInvalidPermanentAbnormalityAtlasUnlocks'
AssertContains 'pruning retains only realization-exclusive entries' $books 'IsRealizationExclusive(info)'
AssertContains 'pruning requires the floor realization to be completed' $books 'IsFloorRealizationCompleted(floor)'
AssertNotContains 'completed realizations do not auto-grant their reward pool' $unlock 'RecordRealizationRewardsToAtlas'

AssertContains 'shop compact supplemental layout helper' $shop 'GetSupplementalSectionBasePosition'
AssertContains 'shop equip left sidebar point' $shop 'return new Vector2(-730f, 260f);'
AssertContains 'shop abno left sidebar point' $shop 'return new Vector2(-730f, -200f);'
AssertContains 'shop ego right sidebar point' $shop 'return new Vector2(560f, 220f);'
AssertContains 'shop upgrade right sidebar point' $shop 'return new Vector2(730f, -210f);'

Write-Host 'RMR UI/atlas persistence static check passed.'

