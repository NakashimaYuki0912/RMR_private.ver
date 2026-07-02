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

$core = Read-Text 'RMR_Core.cs'
$unlocks = Read-Text 'RMR_AbnormalityUnlocks.cs'
$patches = Read-Text 'abcdcode_Refactored\LogLikePatches.cs'
$loader = Read-Text 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs'
$books = Read-Text 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs'
$realizationManager = Read-Text 'RMR_RealizationManager.cs'

Must ($core -notmatch 'RMR_BinahRedMistUnlocked|SaveSimpleFlag\(Binah') 'Binah still uses a permanent global unlock file.'
Must ($core -match 'ShouldRecordRoleBookInPermanentAtlas[\s\S]*return !IsBinahCorePage') 'Binah can still enter the permanent atlas.'
Must (([regex]::Matches($books, 'ShouldRecordRoleBookInPermanentAtlas\(').Count) -ge 4) 'Some inventory or save-load paths still record Binah in the permanent atlas.'
Must ($unlocks -match 'BinahUnlockedForCurrentRoute\s*=\s*false' -and $unlocks -match 'data\.AddData\("BinahUnlocked"') 'Binah route unlock is not reset and saved with the current route.'
Must ($patches -match 'personalEgoDetail\.ExistsCard\(\) \|\| Singleton<SpecialCardListModel>\.Instance\.ExistEgoCardBySelected\(\)') 'The original EGO toggle is not enabled for selected floor EGO pages.'
Must ($loader -match 'RestoreVanillaEnemyIdsForImpurityStage\(stageClassInfo\)') 'Grade7 stage loading does not restore vanilla enemy package semantics.'
Must ($loader -match 'new LorId\(lorId\.id\)' -and $loader -match 'new LorId\(listLorId\.id\)') 'Grade7 enemy unit IDs must normalize to base-game LorId(id), not @origin workshop IDs.'
Must ($loader -notmatch 'new LorId\("@origin",\s*(lorId|listLorId)\.id\)') 'Grade7 enemy unit IDs still normalize to @origin workshop IDs.'
Must ($realizationManager -notmatch 'SavePermanentAtlasUnlocks\(\)') 'Realization fallback still syncs current route inventory into permanent atlas.'
Must ($realizationManager -match 'SavePermanentAtlasData\(\)') 'Realization fallback should persist only atlas fallback entries.'

[xml]$stageXml = Get-Content -Raw -Encoding UTF8 'AddData\StageInfo\StageInfo_ch7.xml'
$stages = @($stageXml.StageXmlRoot.Stage)
Must ($stages.Count -eq 12) 'Grade7 must contain ten Reverberation Ensemble normal StageInfo entries plus two full boss StageInfo entries.'
foreach ($stage in $stages) {
    Must (@($stage.Wave).Count -gt 0) "Grade7 stage $($stage.id) has no wave."
    Must ((@($stage.Wave | ForEach-Object { @($_.Unit) }).Count) -gt 0) "Grade7 stage $($stage.id) has no enemy units."
}

[xml]$routeXml = Get-Content -Raw -Encoding UTF8 'SpecialStaticInfo\StagesXmlInfos\Stage_ch7.xml'
$normalIds = @($routeXml.StagesXmlRoot.ChapterList.StageList | Where-Object StageType -eq 'Normal' | ForEach-Object { [int]$_.ID })
$bossIds = @($routeXml.StagesXmlRoot.ChapterList.StageList | Where-Object StageType -eq 'Boss' | ForEach-Object { [int]$_.ID })
Must ($normalIds.Count -eq 10) 'Grade7 route must expose the ten Reverberation Ensemble member fights as normal nodes.'
foreach ($normalId in 70001..70010) {
    Must ($normalIds -contains $normalId) "Grade7 route is missing Reverberation Ensemble normal stage $normalId."
}
Must ($bossIds.Count -eq 2 -and $bossIds -contains 70020 -and $bossIds -contains 70021) 'Grade7 route must expose only full Black Silence and full Distorted Ensemble boss nodes.'
$blackSilence = @($stages | Where-Object id -eq '70020')[0]
$ensemble = @($stages | Where-Object id -eq '70021')[0]
Must (($blackSilence.Wave.ManagerScript | Select-Object -First 1) -eq 'BlackSilence') 'Grade7 boss 70020 must use the full Black Silence manager.'
Must (@($ensemble.Wave).Count -eq 3) 'Grade7 boss 70021 must keep all three Distorted Ensemble waves.'

Write-Output 'PASS: Binah route unlock, EGO toggle, and Grade7 vanilla-unit routing rules are present.'

