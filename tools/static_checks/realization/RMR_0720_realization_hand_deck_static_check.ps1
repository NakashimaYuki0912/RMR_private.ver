$ErrorActionPreference = 'Stop'

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = $scriptDir
while ($repoRoot -and -not (Test-Path (Join-Path $repoRoot 'RogueLike Mod Reborn.csproj'))) {
    $repoRoot = Split-Path -Parent $repoRoot
}
if (-not $repoRoot) {
    throw 'Could not locate repository root for realization hand/deck check.'
}

$patchesPath = Join-Path $repoRoot 'abcdcode_Refactored\LogLikePatches.cs'
$realizationPath = Join-Path $repoRoot 'RMR_RealizationManager.cs'
$patches = Get-Content -Raw -Encoding UTF8 $patchesPath
$realization = Get-Content -Raw -Encoding UTF8 $realizationPath
$errors = @()

function Add-Failure([string]$message) {
    $script:errors += $message
    Write-Host "[FAIL] $message" -ForegroundColor Red
}

function Add-Pass([string]$message) {
    Write-Host "[OK] $message" -ForegroundColor Green
}

function Get-SourceSlice(
    [string]$text,
    [string]$startMarker,
    [string]$endMarker,
    [string]$label) {
    $start = $text.IndexOf($startMarker, [StringComparison]::Ordinal)
    if ($start -lt 0) {
        Add-Failure "$label start marker not found"
        return ''
    }
    $end = $text.IndexOf($endMarker, $start, [StringComparison]::Ordinal)
    if ($end -lt 0) {
        Add-Failure "$label end marker not found"
        return ''
    }
    return $text.Substring($start, $end - $start)
}

Write-Host '=== RMR 0720 realization hand/deck static check ===' -ForegroundColor Cyan

$handBlock = Get-SourceSlice $patches `
    'public void BattleUnitCardsInHandUI_SetCardsObject' `
    'public bool DeckModel_MoveCardToInventory' `
    'SetCardsObject hook'

if ($handBlock) {
    if ($handBlock -match 'LogLikeMod\.CheckStage\(true\)[\s\S]{0,240}RMRRealizationManager\.InRealizationBattle|RMRRealizationManager\.InRealizationBattle[\s\S]{0,240}LogLikeMod\.CheckStage\(true\)') {
        Add-Pass 'SetCardsObject safe path includes live Floor Realization combat'
    }
    else {
        Add-Failure 'SetCardsObject falls back to vanilla during live Floor Realization combat'
    }

    if ($handBlock -match 'BattleUnitCardsInHandUI\.HandState\.BattleCard') {
        Add-Pass 'SetCardsObject safe path defaults to normal combat pages'
    }
    else {
        Add-Failure 'SetCardsObject safe path does not force the normal combat-page hand'
    }
}

$startBattleBlock = Get-SourceSlice $patches `
    'private void StageController_StartBattle_Inner' `
    'public void StageController_CreateLibrarianUnit' `
    'StageController_StartBattle_Inner'

if ($startBattleBlock) {
    $realizationBranchStart = $startBattleBlock.IndexOf(
        'if (RMRRealizationManager.InRealizationBattle)',
        [StringComparison]::Ordinal)
    $realizationBranchEnd = $startBattleBlock.IndexOf(
        '// Purple Tear',
        [Math]::Max(0, $realizationBranchStart),
        [StringComparison]::Ordinal)
    if ($realizationBranchStart -lt 0 -or $realizationBranchEnd -lt 0) {
        Add-Failure 'Could not isolate the live realization StartBattle branch'
    }
    else {
        $branch = $startBattleBlock.Substring(
            $realizationBranchStart,
            $realizationBranchEnd - $realizationBranchStart)
        $resetIndex = $branch.IndexOf('ResetBattleEgoSelectionState(self)', [StringComparison]::Ordinal)
        $origIndex = $branch.IndexOf('orig(self)', [StringComparison]::Ordinal)
        $clearIndex = $branch.IndexOf('ClearFloorEgoFromHandsAtBattleStart()', [StringComparison]::Ordinal)
        $hideIndex = $branch.IndexOf('HideHandUiUntilCombat()', [StringComparison]::Ordinal)
        $returnIndex = $branch.LastIndexOf('return;', [StringComparison]::Ordinal)

        if ($resetIndex -ge 0 -and $origIndex -gt $resetIndex -and
            $clearIndex -gt $origIndex -and $hideIndex -gt $clearIndex -and
            $returnIndex -gt $hideIndex) {
            Add-Pass 'Realization StartBattle resets EGO state and returns to combat pages after vanilla setup'
        }
        else {
            Add-Failure 'Realization StartBattle skips EGO reset/hand cleanup before its early return'
        }
    }
}

$loadoutBlock = Get-SourceSlice $realization `
    'private static bool ApplyCompendiumOnlyLoadout' `
    'private static void RestoreRouteLoadout' `
    'ApplyCompendiumOnlyLoadout'

if ($loadoutBlock) {
    if ($loadoutBlock -match 'RMRPrepareRestrictions\.IsAllowedInCombatDeckInventory\(card\)') {
        Add-Pass 'Compendium projection excludes EGO/non-inventory pages from normal decks'
    }
    else {
        Add-Failure 'Compendium projection can admit EGO pages into the normal battle-card pool'
    }

    if ($loadoutBlock -match 'CanAddCardToCurrentDeck\(cardXml\.id, unit\.bookItem\)') {
        Add-Pass 'Projected battle pages still pass key-page deck restrictions'
    }
    else {
        Add-Failure 'Projected battle pages bypass key-page deck restrictions'
    }

    if ($loadoutBlock -match 'CardEquipState\s+\w+\s*=\s*unit\.AddCardFromInventory\(cardXml\.id\);[\s\S]{0,180}if\s*\(\w+\s*==\s*CardEquipState\.Equippable\)[\s\S]{0,100}needed--;') {
        Add-Pass 'Compendium auto-fill counts only cards actually equipped into the deck'
    }
    else {
        Add-Failure 'Compendium auto-fill decrements its target even when AddCardFromInventory fails'
    }
}

if ($errors.Count -gt 0) {
    Write-Host "RMR 0720 realization hand/deck check failed: $($errors -join '; ')" -ForegroundColor Red
    exit 1
}

Write-Host 'RMR 0720 realization hand/deck check passed.' -ForegroundColor Green
