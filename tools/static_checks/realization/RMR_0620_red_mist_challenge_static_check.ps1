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
function Read-Text([string]$relativePath) {
    return Get-Content -Raw -Encoding UTF8 (Join-Path $root $relativePath)
}

$stagePool = Read-Text 'SpecialStaticInfo\StagesXmlInfos\Stage_ch6.xml'
$stageInfo = Read-Text 'AddData\StageInfo\StageInfo_ch6.xml'
$enemyInfo = Read-Text 'AddData\EnemyUnitInfo\EnemyUnitInfo_ch6.xml'
$grade6Rewards = Read-Text 'SpecialStaticInfo\RewardPassiveInfos\EquipReward_ch6.xml'
$grade7Rewards = Read-Text 'SpecialStaticInfo\RewardPassiveInfos\EquipReward_ch7.xml'
$corePage = Read-Text 'AddData\EquipPage\EquipPage_Librarian_ch6.xml'
$cardInfo = Read-Text 'AddData\CardInfo\CardInfo_ch6_2.xml'
$models = Read-Text 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs'
$unlocks = Read-Text 'RMR_AbnormalityUnlocks.cs'
$patches = Read-Text 'abcdcode_Refactored\LogLikePatches.cs'
$project = Read-Text 'RogueLike Mod Reborn.csproj'
$exclusiveSet = [regex]::Match($models, 'CorePageExclusiveBattleCardIds\s*=\s*new HashSet<int>\s*\{(?<body>[\s\S]*?)\};').Groups['body'].Value
$redMistEnemy = [regex]::Match($enemyInfo, '<Enemy ID="50022">(?<body>[\s\S]*?)</Enemy>').Groups['body'].Value
$redMistRewardIds = [regex]::Match($unlocks, 'RedMistBattlePageIds\s*=\s*[\s\S]*?\{(?<body>[\s\S]*?)\};').Groups['body'].Value
$redMistRewardHelper = [regex]::Match($models, 'IsRedMistRewardBattleCard\(LorId id\)(?<body>[\s\S]*?)public static void PruneInvalidPermanentAbnormalityAtlasUnlocks').Groups['body'].Value
$errors = @()

function Assert-Match([string]$label, [string]$text, [string]$pattern) {
    if ($text -notmatch $pattern) {
        $script:errors += $label
        Write-Host "[FAIL] $label" -ForegroundColor Red
    }
    else { Write-Host "[OK] $label" -ForegroundColor Green }
}

function Assert-NoMatch([string]$label, [string]$text, [string]$pattern) {
    if ($text -match $pattern) {
        $script:errors += $label
        Write-Host "[FAIL] $label" -ForegroundColor Red
    }
    else { Write-Host "[OK] $label" -ForegroundColor Green }
}

Assert-Match 'Stage 60020 is a scripted elite challenge' $stagePool '<StageList ID="60020" StageType="Elite" Script="Ch6RedMistChallenge"\s*/>'
Assert-NoMatch 'Stage 60020 is absent from the normal pool' $stagePool '<StageList ID="60020" StageType="Normal"'
Assert-Match 'R-Corp captains are no longer a Grade6 boss choice' $stagePool '<StageList ID="60014" StageType="Normal"'
foreach ($id in 60021..60023) {
    Assert-Match "Grade6 boss pool keeps stage $id" $stagePool ('<StageList ID="{0}" StageType="Boss"' -f $id)
}
Assert-Match 'Stage 60020 still targets Red Mist enemy 50022' $stageInfo '<Stage id="60020">[\s\S]*?<Unit>50022</Unit>'
Assert-Match 'Grade6 keeps five normal slots' $models 'case ChapterGrade\.Grade6:[\s\S]*?Normal\s*=\s*5'
Assert-Match 'Grade6 has one elite slot' $models 'case ChapterGrade\.Grade6:[\s\S]*?Elite\s*=\s*1'

Assert-NoMatch 'Red Mist core page is absent from Grade6 common rewards' $grade6Rewards '<RewardList ID="250022"'
Assert-NoMatch 'Red Mist core page is absent from Grade7 common rewards' $grade7Rewards '<RewardList ID="250022"'
Assert-NoMatch 'Red Mist enemy has no generic drop book' $redMistEnemy '<DropTable'

Assert-Match 'Red Mist challenge short-circuits generic rewards at battle start' $unlocks 'if\s*\(IsRedMistChallengeStage\(\)\)\s*return;'
Assert-Match 'Red Mist rewards require an actual victory' $unlocks 'GrantRedMistChallengeVictoryRewards\(\)[\s\S]*GetAliveListWithAvailable\(Faction\.Player\)[\s\S]*GetAliveListWithAvailable\(Faction\.Enemy\)'
Assert-Match 'Red Mist rewards are guarded once per battle' $unlocks 'RedMistVictoryRewardsGrantedThisBattle'
Assert-Match 'Battle end invokes Red Mist victory rewards' $patches 'GrantRedMistChallengeVictoryRewards\s*\(\s*\)'
Assert-Match 'Red Mist challenge clears generic reward queues' $unlocks 'SuppressRedMistChallengeGenericRewards\(\)[\s\S]*rewards\?\.Clear\(\)[\s\S]*rewards_passive\?\.Clear\(\)[\s\S]*egoSelectionQueue\?\.Clear\(\)'
Assert-Match 'Battle start suppresses prequeued Red Mist generic rewards' $patches 'EnqueueBattleClearRewards\(\);\s*[\r\n]+\s*RMRAbnormalityUnlockManager\.SuppressRedMistChallengeGenericRewards\(\);'
Assert-Match 'Red Mist victory grants core page 250022' $unlocks 'RedMistCorePageId\s*=\s*250022'
Assert-Match 'Red Mist reward helper retains only 607003-607007' $redMistRewardHelper 'id\.id\s*>=\s*607003\s*&&\s*id\.id\s*<=\s*607007'
foreach ($id in 607003..607007) {
    Assert-Match "Red Mist victory grants battle page $id" $redMistRewardIds ([regex]::Escape($id.ToString()))
    Assert-Match "Core page permits battle page $id" $corePage "<OnlyCard>$id</OnlyCard>"
    Assert-Match "Battle page $id remains OnlyPage" $cardInfo ('<Card ID="{0}">[\s\S]*?<Option>OnlyPage</Option>' -f $id)
}
foreach ($id in 607001,607002,607008) {
    Assert-NoMatch "Red Mist special/EGO page $id is not granted as a battle page" $redMistRewardIds ([regex]::Escape($id.ToString()))
    Assert-NoMatch "Red Mist special/EGO page $id is pruned from public inventory/atlas" $redMistRewardHelper ([regex]::Escape($id.ToString()))
}

Assert-Match 'Challenge pickup class is compiled' $project '<Compile Include="abcdcode_LOGLIKE_MOD\\PickUpModel_Ch6RedMistChallenge.cs"\s*/>'
Assert-Match 'Challenge pickup uses requested artwork' (Read-Text 'abcdcode_LOGLIKE_MOD\PickUpModel_Ch6RedMistChallenge.cs') 'Stage_ch6_RedMistChallenge'
foreach ($locale in @('cn', 'en', 'kr')) {
    $localize = Read-Text "Localize\$locale\CustomStage_ch6.txt"
    Assert-Match "$locale challenge name exists" $localize 'Stage_RedMistChallenge'
    Assert-Match "$locale challenge description exists" $localize 'Stage_RedMistChallenge_Desc'
}

$artwork = Join-Path $root 'ArtWork\Stages\Stage_ch6\Stage_ch6_RedMistChallenge.png'
if (-not (Test-Path $artwork)) {
    $errors += 'challenge artwork exists'
    Write-Host '[FAIL] challenge artwork exists' -ForegroundColor Red
}
else { Write-Host '[OK] challenge artwork exists' -ForegroundColor Green }

if ($errors.Count -gt 0) {
    Write-Host "`nRMR 0620 Red Mist challenge check failed: $($errors -join '; ')" -ForegroundColor Red
    exit 1
}

Write-Host "`nRMR 0620 Red Mist challenge check passed." -ForegroundColor Green

