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

$rewarding = Read-Text 'abcdcode_LOGLIKE_MOD\RewardingModel.cs'
$cardReward = Read-Text 'abcdcode_LOGLIKE_MOD\MysteryModel_CardReward.cs'
$patches = Read-Text 'abcdcode_Refactored\LogLikePatches.cs'
$unlocks = Read-Text 'RMR_AbnormalityUnlocks.cs'
$core = Read-Text 'RMR_Core.cs'
$shop = Read-Text 'abcdcode_LOGLIKE_MOD\ShopBase.cs'

Must ($rewarding -notmatch 'Compacted' -and $rewarding -match 'BattleCardRewardRetentionRate\s*=\s*0\.7') 'Drop books are not retaining roughly seventy percent of real copies.'
Must ($cardReward -match 'rewards\.Remove\(LogLikeMod\.rewards\.Find') 'Selecting a drop book must consume one copy, matching original-codes.'
Must ($cardReward -notmatch 'CombatPagePicksPerDropBook|remainingCardPicks') 'One drop-book copy must correspond to one combat-page choice.'
Must ($rewarding -match 'TryStartUnlockedEgoChoice') 'Emotion-level flow does not try an unlocked EGO choice.'
Must ($patches -match 'rewardFlag\s*==\s*RewardingModel\.RewardFlag\.EgoCardReward[\s\S]*RecordAtlasEgoPage') 'End-of-battle EGO reward handling is missing.'
Must ($patches -match 'else[\s\r\n]+orig\(self, egoCard\)') 'In-battle EGO picks must call the vanilla OnPickEgoCard path.'
Must ($unlocks -match 'GetPermanentStartingPages\(\)[\s\S]*!IsRealizationExclusive') 'Realization-exclusive abnormality pages still enter the route before being selected.'
Must ($unlocks -match 'PruneUnselectedRealizationPagesFromRoute') 'Old route unlocks are not pruned before Boss reward generation.'
Must ($unlocks -match 'GrantRedMistChallengeVictoryRewards\(\)[\s\S]*TryAddUniqueRoleBookToInventoryAndAtlas\(redMistBookId\)') 'Red Mist core page is not granted from the victory-only path.'
Must ($shop -match 'EquipPage:[\s\r\n]+\s*return new Vector2\(-730f, 260f\)') 'Role books are not anchored in the left sidebar.'
Must ($shop -match 'EgoPage:[\s\r\n]+\s*return new Vector2\(730f, 220f\)') 'EGO pages are not anchored in the right sidebar.'
Must ($shop -match 'basePos\.y \+ id \* GetSupplementalSectionStep\(section\)' -and $shop -match 'return section == ShopSection\.CardUpgrade \? 0f : -230f') 'Sidebar goods are not laid out vertically.'

Write-Output 'PASS: runtime reward, EGO choice, Red Mist gate, and sidebar shop layout rules are present.'

