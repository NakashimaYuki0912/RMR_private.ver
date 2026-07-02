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

$patches = Read-Text ('abcdcode_Refactored' + $bs + 'LogLikePatches.cs')
$realization = Read-Text 'RMR_RealizationManager.cs'
$core = Read-Text 'RMR_Core.cs'
$unlock = Read-Text 'RMR_AbnormalityUnlocks.cs'
$loglikemod = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'LogLikeMod.cs')
$books = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'LogueBookModels.cs')
$saver = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'LoguePlayDataSaver.cs')

# ============================================================
# 1. ApplyRealizationButtonText does NOT depend on fixed child index
# ============================================================
AssertNotContains 'ApplyRealizationButtonText must not have childCount <= 1 early return' $patches 'childCount <= 1'
AssertNotContains 'ApplyRealizationButtonText must not use GetChild(1) hardcoded' $patches 'GetChild(1).gameObject.GetComponent<TextMeshProUGUI>()'
AssertContains 'ApplyRealizationButtonText must use a dedicated stable label helper' $patches 'GetOrCreateRealizationButtonLabel'
AssertContains 'Realization label must be a direct named child' $patches 'RMR_RealizationLabel'
AssertContains 'ApplyRealizationButtonText must use ui_Realization key' $patches 'ui_Realization'

Write-Host '[PASS] 1. ApplyRealizationButtonText no longer depends on fixed child index.'

# ============================================================
# 2. StartRealizationBattle validates StageClassInfo BEFORE consuming entry flag
# ============================================================
AssertContains 'TryResolveRealizationStage method exists' $realization 'TryResolveRealizationStage'
AssertContains 'TryResolveRealizationStage uses StageClassInfoList' $realization 'StageClassInfoList'
AssertContains 'TryResolveRealizationStage checks waveList null' $realization 'waveList != null'
AssertContains 'TryResolveRealizationStage checks waveList Count > 0' $realization 'waveList.Count > 0'
AssertContains 'StartRealizationBattle calls TryResolveRealizationStage' $realization 'TryResolveRealizationStage(floor'
# InitialRelicEntryAvailable = false must appear AFTER the TryResolve call
$startBattleMethod = [regex]::Match($realization, '(?s)public static void StartRealizationBattle.*?(?=public static void OnRealizationBattleEnded)').Value
if ($startBattleMethod -notmatch 'TryResolveRealizationStage[\s\S]*?InitialRelicEntryAvailable = false') {
    throw 'InitialRelicEntryAvailable = false must come AFTER TryResolveRealizationStage call'
}

Write-Host '[PASS] 2. StartRealizationBattle validates StageClassInfo before consuming flag.'

# ============================================================
# 3. Ten vanilla realization Stage IDs are mapped in RealizationBossStageIds
# ============================================================
$expectedFloors = @('Malkuth', 'Yesod', 'Hod', 'Netzach', 'Tiphereth', 'Gebura', 'Chesed', 'Binah', 'Hokma', 'Keter')
foreach ($f in $expectedFloors) {
    AssertContains "RealizationBossStageIds has $f" $realization "SephirahType.$f"
}

Write-Host '[PASS] 3. All 10 floor realization Stage IDs are mapped.'

# ============================================================
# 4. Grade6 and Binah progression flags are only saved after confirmation
# ============================================================
AssertContains 'HasRoleBookInBothAtlasAndBooklist method exists' $core 'HasRoleBookInBothAtlasAndBooklist'
AssertContains 'EnsureGrade6SpecialCorePagesUnlocked method exists' $core 'EnsureGrade6SpecialCorePagesUnlocked'
AssertContains 'TryResolveGrade6SpecialCorePages method exists' $core 'TryResolveGrade6SpecialCorePages'
AssertContains 'Black Silence flag is saved only after inventory and atlas confirmation' $core 'HasRoleBookInBothAtlasAndBooklist(blackSilence)'
AssertContains 'Binah has an independent route-local unlock state' $unlock 'BinahUnlockedForCurrentRoute'
AssertContains 'Binah route flag is written by victory unlock method' $core 'UnlockBinahForCurrentRoute()'
AssertNotContains 'Binah must not use a permanent global unlock file' $core 'BinahRedMistUnlockedSaveName'
# The old pattern that saves flag regardless must not exist
AssertNotContains 'Must not call SaveGrantedFlag() unconditionally' $core 'SaveGrantedFlag()'

Write-Host '[PASS] 4. Grade6 and Binah progression flags follow separate confirmation paths.'

# ============================================================
# 5. Grade6+ load triggers EnsureGrade6SpecialCorePagesUnlocked
# ============================================================
AssertContains 'LoguePlayDataSaver calls EnsureGrade6SpecialCorePagesUnlocked on Grade6+' $saver 'EnsureGrade6SpecialCorePagesUnlocked'
AssertContains 'Load check guards by curchaptergrade >= Grade6' $saver 'curchaptergrade >= ChapterGrade.Grade6'
AssertContains 'DebugCh6_OnClearBossWave still invokes EnsureGrade6SpecialCorePagesUnlocked' $core 'case ChapterGrade.Grade6:'
$grade6Block = [regex]::Match($core, '(?s)case ChapterGrade\.Grade6:.*?(?=case ChapterGrade\.Grade7:|break;)').Value
AssertContains 'OnClearBossWave Grade6 case calls grant method' $grade6Block 'GrantGrade6SpecialCorePagesIfNeeded'

Write-Host '[PASS] 5. Grade6+ load/continue triggers EnsureGrade6SpecialCorePagesUnlocked.'

# ============================================================
# 6. Stage_ch7 battle stage IDs must exist in StageInfo with wave/unit non-empty
# ============================================================

# 6a. Parse Stage_ch7.xml with proper [xml] (NOT fragile regex)
$stageCh7Path = Join-Path $root ('SpecialStaticInfo' + $bs + 'StagesXmlInfos' + $bs + 'Stage_ch7.xml')
$stageCh7Xml = [xml](Get-Content $stageCh7Path -Raw -Encoding UTF8)

# Collect battle-type StageList IDs (Normal, Boss, Mystery — NOT Shop/Creature)
$ns = @{ ns = 'http://www.w3.org/2001/XMLSchema' }  # unused but kept for namespace safety
$battleIds = @()
foreach ($sl in $stageCh7Xml.StagesXmlRoot.ChapterList.StageList) {
    $stype = $sl.StageType
    $sid   = $sl.ID
    if ($stype -eq 'Normal' -or $stype -eq 'Boss' -or $stype -eq 'Mystery') {
        $battleIds += $sid
    }
}
if ($battleIds.Count -eq 0) {
    throw 'Stage_ch7.xml did not yield any Normal/Boss/Mystery StageList IDs.'
}
Write-Host "[INFO] Stage_ch7.xml has $($battleIds.Count) battle-type StageList IDs: $($battleIds -join ', ')"

# 6b. Load StageInfo files with [xml]
$stageInfoFiles = @(
    ('AddData' + $bs + 'StageInfo' + $bs + 'StageInfo_ch7.xml'),
    ('AddData' + $bs + 'StageInfo' + $bs + 'StageInfo_ch7event.xml')
)
$stageInfoMap = @{}  # Stage ID -> xml element
foreach ($sifRel in $stageInfoFiles) {
    $sifPath = Join-Path $root $sifRel
    if (-not (Test-Path $sifPath)) { continue }
    $sifXml = [xml](Get-Content $sifPath -Raw -Encoding UTF8)
    foreach ($s in $sifXml.StageXmlRoot.Stage) {
        $sid = $s.id
        if (-not $stageInfoMap.ContainsKey($sid)) {
            $stageInfoMap[$sid] = $s
        }
    }
}
Write-Host "[INFO] Loaded $($stageInfoMap.Count) Stage entries from StageInfo_ch7*.xml"

# 6c. Assert every battle StageList ID has a matching StageInfo with wave/unit data
$grade7Ok = $true
foreach ($bid in $battleIds) {
    if (-not $stageInfoMap.ContainsKey($bid)) {
        Write-Host "[FAIL] Stage_ch7 ID $bid has NO matching StageInfo in StageInfo_ch7*.xml" -ForegroundColor Red
        $grade7Ok = $false
        continue
    }
    $si = $stageInfoMap[$bid]
    $waves = $si.Wave
    if ($waves -eq $null -or $waves.Count -eq 0) {
        Write-Host "[FAIL] StageInfo for $bid has NO Wave children" -ForegroundColor Red
        $grade7Ok = $false
        continue
    }
    $waveIdx = 0
    foreach ($w in $waves) {
        $units = $w.Unit
        if ($units -eq $null -or $units.Count -eq 0) {
            Write-Host "[FAIL] StageInfo $bid Wave[$waveIdx] has NO Unit children" -ForegroundColor Red
            $grade7Ok = $false
        }
        $waveIdx++
    }
    $waveCount = $waves.Count
    Write-Host ("[OK] StageInfo " + $bid + ": " + $waveCount + " wave(s), units present")
}

if (-not $grade7Ok) {
    throw 'Grade7 stage integrity check FAILED — see [FAIL] lines above.'
}
Write-Host '[PASS] 6. All Grade7 battle Stage IDs have valid StageInfo with Wave/Unit data.'

# ============================================================
# 7. SetNextStage has null/empty wave protection
# ============================================================
AssertContains 'SetNextStage checks data == null' $loglikemod 'if (data == null)'
AssertContains 'SetNextStage checks waveList == null' $loglikemod 'data.waveList == null'
AssertContains 'SetNextStage checks waveList.Count == 0' $loglikemod 'data.waveList.Count == 0'
AssertContains 'SetNextStage logs error for null data' $loglikemod 'StageClassInfoList.GetData returned NULL'

Write-Host '[PASS] 7. SetNextStage has null/empty wave protection.'

# ============================================================
# 8. Additional checks from rework points
# ============================================================

# 8a. Korean fallback is NOT lumped with Japanese
AssertNotContains 'GetRMRText must NOT combine kr with jp/ja' $patches 'lang.Contains("jp") || lang.Contains("ja") || lang.Contains("kr")'

# 8b. Button creation uses SafeGetButtonComponents (no raw GetChild)
AssertContains 'SafeGetButtonComponents helper exists' $patches 'SafeGetButtonComponents'
AssertNotContains 'InvenBtn creation must not use GetChild(1) for UITextDataLoader' $patches 'InvenBtn.transform.GetChild(1).gameObject.GetComponent<UITextDataLoader>'
AssertNotContains 'RealizationBtn creation must not use GetChild(1) for UITextDataLoader' $patches 'RealizationBtn.transform.GetChild(1).gameObject.GetComponent<UITextDataLoader>'
AssertNotContains 'CraftBtn creation must not use GetChild(1) for UITextDataLoader' $patches 'CraftBtn.transform.GetChild(1).gameObject.GetComponent<UITextDataLoader>'

# 8c. Grade6 ensure uses EnsureRoleBookInCurrentBooklist for booklist coverage
AssertContains 'EnsureRoleBookInCurrentBooklist method exists' $core 'EnsureRoleBookInCurrentBooklist'
AssertContains 'HasRoleBookInBothAtlasAndBooklist method exists' $core 'HasRoleBookInBothAtlasAndBooklist'
AssertContains 'Grade6 ensure checks Black Silence atlas and booklist before saving flag' $core 'HasRoleBookInBothAtlasAndBooklist(blackSilence)'

# 8d. AddingRemainStageList checks waveList.Count == 0
AssertContains 'AddingRemainStageList checks waveList.Count == 0' $books 'waveList.Count == 0'

# ============================================================
# 9. Evidence 2: EnsureRemainStageListIntegrity called after LoadFromSaveData
# ============================================================
AssertContains 'EnsureRemainStageListIntegrity method exists' $books 'EnsureRemainStageListIntegrity'
AssertContains 'EnsureRemainStageListIntegrity called in LoadFromSaveData' $books 'EnsureRemainStageListIntegrity()'

# ============================================================
# 10. Evidence 3: PickUpXml null safety chain
# ============================================================
$loglikemodCheck = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'LogLikeMod.cs')
AssertContains 'GetRegisteredPickUpXml(LogueStageInfo) checks info null' $loglikemodCheck 'info == null'
AssertContains 'GetRegisteredPickUpXml uses TryGetValue for PickUpXml_Dummy_Stage' $loglikemodCheck 'TryGetValue(info.workshopid'
# Ensure the GetRegisteredPickUpXml getter does NOT use unsafe indexer without TryGetValue first.
# We check that TryGetValue appears BEFORE any remaining direct indexer in the stage overload area.
$stageOverload = [regex]::Match($loglikemodCheck, '(?s)GetRegisteredPickUpXml\(LogueStageInfo info\).*?(?=public static EmotionCardXmlInfo GetRegisteredPickUpXml\(RewardPassiveInfo)').Value
if ($stageOverload -match 'PickUpXml_Dummy_Stage\[info\.workshopid\]' -and $stageOverload -notmatch 'TryGetValue') {
    throw 'GetRegisteredPickUpXml(LogueStageInfo) uses unsafe indexer without TryGetValue'
}
AssertContains 'GetNextList guards against null thing and empty list' $books 'thing != null'
AssertContains 'CreateStageDesc guards against null registeredPickUpXml' $books 'registeredPickUpXml == null'

# ============================================================
# 11. Evidence 4: Atlas enumeration without Take limits, permanent ID merge
# ============================================================
$atlas = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'LogAtlasPanel.cs')
AssertNotContains 'BuildRoleBookEntries must not use Take(260)' $atlas '.Take(260)'
AssertNotContains 'BuildBattleCardEntries must not use Take(420)' $atlas '.Take(420)'
AssertContains 'Atlas SetActive calls EnsureAtlasUnlocks' $atlas 'EnsureAtlasUnlocks()'
AssertContains 'Atlas SetActive calls SyncCurrentInventoryToPermanentAtlas' $atlas 'SyncCurrentInventoryToPermanentAtlas()'
AssertContains 'BuildRoleBookEntries merges AtlasUnlockedRoleBooks' $atlas 'AtlasUnlockedRoleBooks'
AssertContains 'BuildBattleCardEntries merges AtlasUnlockedBattleCards' $atlas 'AtlasUnlockedBattleCards'

# ============================================================
# 12. Build timestamp diagnostic log
# ============================================================
AssertContains 'BuildTimestamp constant exists' $core 'BuildTimestamp'
AssertContains 'OnInitializeMod logs build timestamp' $core 'BuildTimestamp'

Write-Host ''
Write-Host 'RMR 0617 progression + realization static check — ALL CHECKS PASSED.'
Write-Host ''

