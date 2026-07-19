$ErrorActionPreference = 'Stop'

$script:CheckDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:CheckDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}

function Read-RepoText([string]$RelativePath) {
    $path = Join-Path $script:RepoRoot $RelativePath
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Required file missing: $RelativePath"
    }
    return [System.IO.File]::ReadAllText($path, [System.Text.Encoding]::UTF8)
}

function Assert-Contains([string]$Name, [string]$Text, [string]$Needle) {
    if (-not $Text.Contains($Needle)) {
        throw "$Name missing: $Needle"
    }
}

function Assert-NotContains([string]$Name, [string]$Text, [string]$Needle) {
    if ($Text.Contains($Needle)) {
        throw "$Name must not contain: $Needle"
    }
}

function Assert-InOrder([string]$Name, [string]$Text, [string[]]$Needles) {
    $cursor = -1
    foreach ($needle in $Needles) {
        $cursor = $Text.IndexOf($needle, $cursor + 1, [System.StringComparison]::Ordinal)
        if ($cursor -lt 0) {
            throw "$Name missing or out of order: $needle"
        }
    }
}

$logLike = Read-RepoText 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs'
$patches = Read-RepoText 'abcdcode_Refactored\LogLikePatches.cs'
$core = Read-RepoText 'RMR_Core.cs'
$project = Read-RepoText 'RogueLike Mod Reborn.csproj'
$compat = Read-RepoText 'RMR_CompatibilityPatches.cs'

Assert-Contains 'pre-LoadOthers state type' $logLike 'public sealed class BattleLocalizePreservationState'
Assert-Contains 'pre-LoadOthers capture API' $logLike 'CaptureBattleLocalizeBeforeVanillaReload()'
Assert-Contains 'post-LoadOthers restore API' $logLike 'RestoreBattleLocalizeAfterVanillaReload('
Assert-Contains 'workshop battle dialogs are preserved' $logLike 'List<BattleDialogCharacter>'
Assert-Contains 'dialog identity includes workshop package' $logLike 'entry.workshopId'
Assert-Contains 'dialog identity includes book id' $logLike 'entry.bookId'

Assert-Contains 'LoadOthers has a prefix capture' $patches '[HarmonyPrefix, HarmonyPatch(typeof(LocalizedTextLoader), nameof(LocalizedTextLoader.LoadOthers))]'
Assert-Contains 'LoadOthers prefix exports state' $patches 'out LogLikeMod.BattleLocalizePreservationState __state'
Assert-Contains 'LoadOthers prefix captures before vanilla reload' $patches '__state = LogLikeMod.CaptureBattleLocalizeBeforeVanillaReload();'
Assert-InOrder 'LoadOthers postfix restores after RMR overlays and before the second vanilla battle refresh' $patches @(
    'LogLikeMod.LoadTextData(language);',
    'LogLikeMod.RestoreBattleLocalizeAfterVanillaReload(__state, "LoadOthers");',
    'LogLikeMod.RefreshVanillaBattleLocalize(language, "LoadOthers");'
)

Assert-Contains 'compatibility source is compiled' $project '<Compile Include="RMR_CompatibilityPatches.cs" />'
Assert-Contains 'compatibility guards are installed at startup' $core 'RMR_CompatibilityPatches.Install(harmony);'
Assert-Contains 'all loaded framework copies are inspected' $compat 'AppDomain.CurrentDomain.GetAssemblies()'
Assert-Contains 'exact vulnerable framework type is targeted' $compat 'Cyaminthe.AssortedFixes.BattleCameraAndUnitPreviewFix'
Assert-Contains 'exact vulnerable framework method is targeted' $compat 'StageController_GameOver_Prefix'
Assert-Contains 'vulnerable method is wrapped with a guard prefix' $compat 'harmony.Patch(target, prefix: guard);'
Assert-Contains 'guard checks the missing battle camera' $compat 'SingletonBehavior<BattleCamManager>.Instance'
Assert-Contains 'guard skips only the vulnerable external prefix when camera is absent' $compat 'return false;'
Assert-NotContains 'compatibility must not swallow StageController.GameOver itself' $compat 'HarmonyPatch(typeof(StageController)'

Write-Host 'RMR return-title and pre-LoadOthers preservation check passed.'
