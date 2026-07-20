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

function Read-Text([string]$Path) {
    return Get-Content -Raw -Encoding UTF8 -LiteralPath $Path
}

function Assert-Contains([string]$Name, [string]$Text, [string]$Pattern) {
    if ($Text -notmatch $Pattern) {
        throw "FAIL: $Name"
    }
    Write-Host "PASS: $Name"
}

function Assert-Equal([string]$Name, $Actual, $Expected) {
    if ($Actual -ne $Expected) {
        throw "FAIL: $Name (actual=$Actual expected=$Expected)"
    }
    Write-Host "PASS: $Name"
}

function Assert-True([string]$Name, [bool]$Condition) {
    if (-not $Condition) {
        throw "FAIL: $Name"
    }
    Write-Host "PASS: $Name"
}

function Get-TextBetween([string]$Name, [string]$Text, [string]$StartMarker, [string]$EndMarker) {
    $start = $Text.IndexOf($StartMarker, [System.StringComparison]::Ordinal)
    $end = if ($start -ge 0) {
        $Text.IndexOf($EndMarker, $start + $StartMarker.Length, [System.StringComparison]::Ordinal)
    } else {
        -1
    }
    if ($start -lt 0 -or $end -le $start) {
        throw "FAIL: $Name block not found"
    }
    return $Text.Substring($start, $end - $start)
}

$unlock = Read-Text (Join-Path $script:RepoRoot 'RMR_AbnormalityUnlocks.cs')
$rewarding = Read-Text (Join-Path $script:RepoRoot 'abcdcode_LOGLIKE_MOD\RewardingModel.cs')
$books = Read-Text (Join-Path $script:RepoRoot 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs')
$creatureTablePath = Join-Path $script:RepoRoot 'SpecialStaticInfo\RewardPassiveInfos\CreatureInfo_PickTable.xml'
$creatureTable = [xml](Get-Content -Raw -Encoding UTF8 -LiteralPath $creatureTablePath)

$exclusiveRows = @($creatureTable.RewardPassivesRoot.ChapterList.RewardList | Where-Object {
    $id = [int]$_.ID
    $id -ge 15370401 -and $id -le 15370436
})
Assert-Equal 'Exclusive abnormality XML block contains all 36 rows' $exclusiveRows.Count 36
Assert-Equal 'Exclusive abnormality XML IDs are unique' (@($exclusiveRows.ID | Sort-Object -Unique).Count) 36

$exclusiveFloors = [ordered]@{
    Malkuth   = @{ Min = 15370401; Max = 15370403; Roots = @('snowwhite') }
    Yesod     = @{ Min = 15370404; Max = 15370406; Roots = @('freischutz') }
    Hod       = @{ Min = 15370407; Max = 15370409; Roots = @('blackswan') }
    Netzach   = @{ Min = 15370410; Max = 15370412; Roots = @('orchestra') }
    Tiphereth = @{ Min = 15370413; Max = 15370415; Roots = @('clownofnihil') }
    Gebura    = @{ Min = 15370416; Max = 15370418; Roots = @('nothing') }
    Chesed    = @{ Min = 15370419; Max = 15370421; Roots = @('wizard') }
    Binah     = @{ Min = 15370422; Max = 15370427; Roots = @('bossbird') }
    Hokma     = @{ Min = 15370428; Max = 15370433; Roots = @('onebadmanygood', 'plaguedoctor', 'whitenight') }
    Keter     = @{ Min = 15370434; Max = 15370436; Roots = @('quietKid') }
}
$rewardMapBlock = Get-TextBetween 'RealizationRewardScriptsByFloor' $unlock 'public static readonly Dictionary<SephirahType, string[]> RealizationRewardScriptsByFloor' 'public const int RealizationExclusivePassiveIdMin'
foreach ($floor in $exclusiveFloors.Keys) {
    $floorMatch = [regex]::Match($rewardMapBlock, '\{\s*SephirahType\.' + $floor + ',\s*new\[\]\s*\{(?<roots>[^}]*)\}\s*\}')
    Assert-True "Exclusive floor '$floor' has a mapping row" $floorMatch.Success
    $actualRoots = @([regex]::Matches($floorMatch.Groups['roots'].Value, '"(?<root>[^"]+)"') | ForEach-Object {
        $_.Groups['root'].Value
    })
    Assert-Equal "Exclusive roots mapped to '$floor'" (($actualRoots | Sort-Object) -join ',') (($exclusiveFloors[$floor].Roots | Sort-Object) -join ',')
}

foreach ($row in $exclusiveRows) {
    $id = [int]$row.ID
    $script = [string]$row.Script
    $matchingFloors = @($exclusiveFloors.Keys | Where-Object {
        $definition = $exclusiveFloors[$_]
        $id -ge $definition.Min -and $id -le $definition.Max -and
            @($definition.Roots | Where-Object {
                $script.StartsWith($_, [System.StringComparison]::OrdinalIgnoreCase)
            }).Count -eq 1
    })
    Assert-Equal "Exclusive XML row $id/$script maps to one floor" $matchingFloors.Count 1
}

$dailyBlock = Get-TextBetween 'GetRewardCandidates' $unlock 'private static List<RewardPassiveInfo> GetRewardCandidates' 'private static IEnumerable<RewardPassiveInfo> GetPermanentStartingPages'
$passivePresentationBlock = Get-TextBetween 'GetPassiveRewards' $books 'public static List<EmotionCardXmlInfo> GetPassiveRewards(List<RewardPassiveInfo> list)' 'public static List<EmotionCardXmlInfo> GetPassiveRewards_Inlist'
$bossAbnoBlock = Get-TextBetween 'RollExclusiveRealizationAbnormalityChoices' $unlock 'private static List<RewardPassiveInfo> RollExclusiveRealizationAbnormalityChoices' 'private static bool EnqueueRealizationEgoSelection'
$bossFloorBlock = Get-TextBetween 'GetCompletedRealizationFloorsForBossTier' $unlock 'private static HashSet<SephirahType> GetCompletedRealizationFloorsForBossTier' '#endregion'
$egoGenerateBlock = Get-TextBetween 'EnqueueRealizationEgoSelection' $unlock 'private static bool EnqueueRealizationEgoSelection' 'public static void EnqueueRewardSelections'
$egoPresentationBlock = Get-TextBetween 'GetQueuedEgoRewards' $rewarding 'private static List<EmotionEgoXmlInfo> GetQueuedEgoRewards' 'public static void PickEmotion'

Assert-Contains 'Daily abnormality candidates use the completed-floor gate' $dailyBlock '\.Where\(CanAppearInNormalReceptionAbnoPool\)'
Assert-True 'Daily abnormality gate runs before candidate materialization' (
    $dailyBlock.IndexOf('.Where(CanAppearInNormalReceptionAbnoPool)', [System.StringComparison]::Ordinal) -lt
        $dailyBlock.IndexOf('.ToList()', [System.StringComparison]::Ordinal)
)
Assert-Contains 'Final passive reward presentation rechecks realization isolation' $passivePresentationBlock 'CanAppearInRegularRewardSelection'
Assert-Contains 'Boss abnormality candidates recheck each reward against saved floor completion' $bossAbnoBlock 'IsRealizationRewardAvailable\(info\)'
Assert-Contains 'Boss floor candidates are intersected with completed realizations' $bossFloorBlock 'IntersectWith\(CompletedRealizations\)'

Assert-Contains 'A central EGO reward gate resolves the card floor' $unlock 'CanAppearInRegularEgoRewardPool\s*\(\s*LorId\s+id\s*\)'
Assert-Contains 'Boss EGO candidate generation rechecks saved floor completion' $egoGenerateBlock 'CanAppearInRegularEgoRewardPool\(id\)'
Assert-Contains 'Final EGO reward presentation rechecks saved floor completion' $egoPresentationBlock 'CanAppearInRegularEgoRewardPool\(id\)'

$egoMapStart = $unlock.IndexOf('RealizationEgoCardsByFloor')
$egoMapEnd = $unlock.IndexOf('private static readonly string[] SimpleRoots', $egoMapStart)
if ($egoMapStart -lt 0 -or $egoMapEnd -le $egoMapStart) {
    throw 'FAIL: RealizationEgoCardsByFloor block not found'
}
$egoMapBlock = $unlock.Substring($egoMapStart, $egoMapEnd - $egoMapStart)
$egoIds = @([regex]::Matches($egoMapBlock, 'new LorId\((91\d{4})\)') | ForEach-Object {
    [int]$_.Groups[1].Value
})
Assert-Equal 'EGO floor map contains 50 cards' $egoIds.Count 50
Assert-Equal 'EGO floor map IDs are unique' (@($egoIds | Sort-Object -Unique).Count) 50

$expectedEgoIdsByFloor = [ordered]@{
    Malkuth   = @(910001..910005)
    Keter     = @(910006..910010)
    Yesod     = @(910011..910015)
    Hod       = @(910016..910020)
    Netzach   = @(910021..910025)
    Tiphereth = @(910026..910030)
    Gebura    = @(910031..910035)
    Chesed    = @(910036..910040)
    Binah     = @(910041..910045)
    Hokma     = @(910046..910050)
}
foreach ($floor in $expectedEgoIdsByFloor.Keys) {
    $floorMatch = [regex]::Match($egoMapBlock, '\{\s*SephirahType\.' + $floor + ',\s*new\[\]\s*\{(?<ids>[^}]*)\}\s*\}')
    Assert-True "EGO floor '$floor' has a mapping row" $floorMatch.Success
    $actualIds = @([regex]::Matches($floorMatch.Groups['ids'].Value, 'new LorId\((?<id>\d+)\)') | ForEach-Object {
        [int]$_.Groups['id'].Value
    })
    Assert-Equal "EGO IDs mapped to '$floor'" ($actualIds -join ',') ($expectedEgoIdsByFloor[$floor] -join ',')
}

Write-Host 'PASS: realization reward isolation static checks completed.'
