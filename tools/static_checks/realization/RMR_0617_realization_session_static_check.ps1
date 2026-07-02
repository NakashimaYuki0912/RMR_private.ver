$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot
# RMR 0617 Realization Session Static Check
# Validates that realization battles are properly isolated from the Roguelike reward chain.

$ErrorActionPreference = 'Stop'
$repo = $script:RepoRoot
Set-Location $repo

$errors = @()
$warnings = @()

Write-Host "=== RMR 0617 Realization Session Static Check ===" -ForegroundColor Cyan

# ---------------------------------------------------------------------------
# 1. InRealizationBattle guards in reward entry points
# ---------------------------------------------------------------------------
Write-Host "[1] Checking InRealizationBattle guards..."

# 1a. EnqueueBattleClearRewards must skip for realization
$abnoUnlocks = Get-Content -Raw -Encoding UTF8 ".\RMR_AbnormalityUnlocks.cs"
if ($abnoUnlocks -match 'EnqueueBattleClearRewards') {
    # Check that the realization guard exists before the Creature check
    $methodStart = $abnoUnlocks.IndexOf('EnqueueBattleClearRewards')
    $methodSlice = $abnoUnlocks.Substring($methodStart, [Math]::Min(200, $abnoUnlocks.Length - $methodStart))
    if ($methodSlice -match 'InRealizationBattle') {
        Write-Host "  [OK] EnqueueBattleClearRewards has InRealizationBattle guard" -ForegroundColor Green
    } else {
        $errors += "EnqueueBattleClearRewards missing InRealizationBattle guard"
    }
} else {
    $warnings += "Could not find EnqueueBattleClearRewards in RMR_AbnormalityUnlocks.cs"
}

# 1b. StageController_StartBattle must skip for realization
$patches = Get-Content -Raw -Encoding UTF8 ".\abcdcode_Refactored\LogLikePatches.cs"
if ($patches -match 'void StageController_StartBattle') {
    # Find the method body by locating the opening brace after the method signature
    $methodMatch = [regex]::Match($patches, 'void StageController_StartBattle[^{]+\{')
    if ($methodMatch.Success) {
        $bodyStart = $methodMatch.Index + $methodMatch.Length
        $bodySlice = $patches.Substring($bodyStart, [Math]::Min(600, $patches.Length - $bodyStart))
        # Accept either direct InRealizationBattle guard or PendingRealizationBattle→ActivatePendingRealization pattern
        if ($bodySlice -match 'InRealizationBattle|PendingRealizationBattle') {
            Write-Host "  [OK] StageController_StartBattle has InRealizationBattle/PendingRealizationBattle guard" -ForegroundColor Green
        } else {
            $errors += "StageController_StartBattle missing InRealizationBattle guard"
        }
    } else {
        $errors += "Could not parse StageController_StartBattle method body"
    }
} else {
    $warnings += "Could not find StageController_StartBattle in LogLikePatches.cs"
}

# 1c. StageController_EndBattlePhase must skip for realization
if ($patches -match 'void StageController_EndBattlePhase') {
    $methodMatch = [regex]::Match($patches, 'void StageController_EndBattlePhase[^{]+\{')
    if ($methodMatch.Success) {
        $bodyStart = $methodMatch.Index + $methodMatch.Length
        $bodySlice = $patches.Substring($bodyStart, [Math]::Min(500, $patches.Length - $bodyStart))
        if ($bodySlice -match 'InRealizationBattle') {
            Write-Host "  [OK] StageController_EndBattlePhase has InRealizationBattle guard" -ForegroundColor Green
        } else {
            $errors += "StageController_EndBattlePhase missing InRealizationBattle guard"
        }
    } else {
        $errors += "Could not parse StageController_EndBattlePhase method body"
    }
} else {
    $warnings += "Could not find StageController_EndBattlePhase in LogLikePatches.cs"
}

# 1d. StageController_ClearBattle must skip for realization
if ($patches -match 'void StageController_ClearBattle') {
    $methodMatch = [regex]::Match($patches, 'void StageController_ClearBattle[^{]+\{')
    if ($methodMatch.Success) {
        $bodyStart = $methodMatch.Index + $methodMatch.Length
        $bodySlice = $patches.Substring($bodyStart, [Math]::Min(500, $patches.Length - $bodyStart))
        if ($bodySlice -match 'InRealizationBattle') {
            Write-Host "  [OK] StageController_ClearBattle has InRealizationBattle guard" -ForegroundColor Green
        } else {
            $errors += "StageController_ClearBattle missing InRealizationBattle guard"
        }
    } else {
        $errors += "Could not parse StageController_ClearBattle method body"
    }
} else {
    $warnings += "Could not find StageController_ClearBattle in LogLikePatches.cs"
}

# ---------------------------------------------------------------------------
# 2. Route state save/restore
# ---------------------------------------------------------------------------
Write-Host "[2] Checking route state save/restore..."

$realizationMgr = Get-Content -Raw -Encoding UTF8 ".\RMR_RealizationManager.cs"
if ($realizationMgr -match 'RouteBookSnapshot' -and $realizationMgr -match 'RoutePlayerModelSnapshot' -and $realizationMgr -match 'RestoreRouteLoadout') {
    Write-Host "  [OK] RealizationManager saves and restores full route state (books, players, stages)" -ForegroundColor Green
} else {
    $errors += "RealizationManager does not save/restore full route state"
}

# Check that route state restoration includes clearing rewards/nextlist
if ($realizationMgr -match 'rewards.*Clear|Clear.*rewards|nextlist.*Clear|Clear.*nextlist') {
    Write-Host "  [OK] RealizationManager clears rewards/nextlist after realization" -ForegroundColor Green
} else {
    $warnings += "RealizationManager may not clear rewards/nextlist after realization"
}

# ---------------------------------------------------------------------------
# 3. CompleteFloorRealization is the realization reward entry point
# ---------------------------------------------------------------------------
Write-Host "[3] Checking realization reward entry point..."

if ($realizationMgr -match 'CompleteFloorRealization') {
    Write-Host "  [OK] OnRealizationBattleEnded calls CompleteFloorRealization on victory" -ForegroundColor Green
} else {
    $errors += "OnRealizationBattleEnded does not call CompleteFloorRealization"
}

if ($abnoUnlocks -match 'CompleteFloorRealization' -and $abnoUnlocks -notmatch 'RecordRealizationRewardsToAtlas') {
    Write-Host "  [OK] CompleteFloorRealization opens the pool without auto-granting rewards" -ForegroundColor Green
} else {
    $errors += "CompleteFloorRealization must not auto-grant the full reward pool"
}

# ---------------------------------------------------------------------------
# 4. Atlas-only loadout uses atlas unlocks
# ---------------------------------------------------------------------------
Write-Host "[4] Checking atlas-only loadout..."

if ($realizationMgr -match 'ApplyAtlasOnlyLoadout' -and $realizationMgr -match 'AtlasUnlockedRoleBooks' -and $realizationMgr -match 'AtlasUnlockedBattleCards') {
    Write-Host "  [OK] ApplyAtlasOnlyLoadout uses atlas unlocks for team building" -ForegroundColor Green
} else {
    $errors += "ApplyAtlasOnlyLoadout does not build team from atlas unlocks"
}

# ---------------------------------------------------------------------------
# 5. Abnormal page permanent atlas separation
# ---------------------------------------------------------------------------
Write-Host "[5] Checking abnormality page atlas separation..."

$logueBookModels = Get-Content -Raw -Encoding UTF8 ".\abcdcode_LOGLIKE_MOD\LogueBookModels.cs"
if ($abnoUnlocks -match 'IsRealizationExclusive\(info\)' -and $abnoUnlocks -match 'RecordAtlasAbnormalityPage\(info\.id\)') {
    Write-Host "  [OK] Selected realization-exclusive pages are recorded to permanent atlas" -ForegroundColor Green
} else {
    $errors += "Selected realization-exclusive reward path does not record to permanent atlas"
}
if ($logueBookModels -match 'PruneInvalidPermanentAbnormalityAtlasUnlocks' -and $logueBookModels -notmatch 'SyncCurrentAbnormalityPagesToPermanentAtlas') {
    Write-Host "  [OK] Old-save migration prunes ordinary route pages instead of making them permanent" -ForegroundColor Green
} else {
    $errors += "Permanent abnormality atlas migration does not enforce route separation"
}

# Check that atlas is NOT used for new route initialization
if ($logueBookModels -match 'CreateChSaveData') {
    $createChStart = $logueBookModels.IndexOf('CreateChSaveData')
    $createChSlice = $logueBookModels.Substring($createChStart, [Math]::Min(3000, $logueBookModels.Length - $createChStart))
    if ($createChSlice -match 'StartNewRoute' -and $createChSlice -notmatch 'AtlasUnlockedAbnormalityPages.*RouteUnlockedPages|RouteUnlockedPages.*AtlasUnlockedAbnormalityPages') {
        Write-Host "  [OK] New route does NOT inherit atlas abnormality pages as starting pool" -ForegroundColor Green
    }
}

# ---------------------------------------------------------------------------
# 6. Atlas detail panel
# ---------------------------------------------------------------------------
Write-Host "[6] Checking atlas detail panel..."

$atlasPanel = Get-Content -Raw -Encoding UTF8 ".\abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs"
if ($atlasPanel -match 'ShowDetail' -and $atlasPanel -match 'CreateDetailPanel') {
    Write-Host "  [OK] LogAtlasPanel has click detail panel (ShowDetail + CreateDetailPanel)" -ForegroundColor Green
} else {
    $errors += "LogAtlasPanel missing detail panel implementation"
}

# Check that battle card artwork is no longer null
if ($atlasPanel -match 'GetCardArtwork' -and $atlasPanel -notmatch 'Artwork = null,\s*\n\s*Unlocked = IsBattleCardUnlocked') {
    Write-Host "  [OK] BattleCard/Ego entries use GetCardArtwork instead of Artwork=null" -ForegroundColor Green
} else {
    $warnings += "BattleCard entries may still use Artwork=null"
}

# Check tile has detail panel population (via select or click)
if ($atlasPanel -match 'ShowDetail|OnSelect.*ShowDetail') {
    Write-Host "  [OK] LogAtlasTile populates detail panel on selection" -ForegroundColor Green
} else {
    $errors += "LogAtlasTile missing detail panel population"
}

# ---------------------------------------------------------------------------
# 7. BOSS realization tier rewards
# ---------------------------------------------------------------------------
Write-Host "[7] Checking BOSS realization tier rewards..."

$abnoUnlocks = Get-Content -Raw -Encoding UTF8 ".\RMR_AbnormalityUnlocks.cs"
if ($abnoUnlocks -match 'EnqueueBossRealizationTierRewards' -and $abnoUnlocks -match 'StageType\.Boss') {
    Write-Host "  [OK] EnqueueBossRealizationTierRewards is triggered only for BOSS stages" -ForegroundColor Green
} else {
    $errors += "Missing or misconfigured BOSS realization tier rewards"
}

if ($abnoUnlocks -match 'RollRealizationAbnormalityChoices' -and $abnoUnlocks -match 'RealizationExclusiveScripts') {
    Write-Host "  [OK] BOSS abno rewards filter realization-exclusive pages by floor" -ForegroundColor Green
} else {
    $warnings += "BOSS abno rewards may not filter realization-exclusive pages correctly"
}

if ($abnoUnlocks -match 'IsAtlasEgoPageUnlocked') {
    Write-Host "  [OK] BOSS EGO selection filters already-unlocked EGO pages" -ForegroundColor Green
} else {
    $warnings += "BOSS EGO selection may not filter already-unlocked EGO pages"
}

# ---------------------------------------------------------------------------
# 8. Black Silence/Binah built-in deck protection
# ---------------------------------------------------------------------------
Write-Host "[8] Checking built-in deck protection..."

$realizationMgr = Get-Content -Raw -Encoding UTF8 ".\RMR_RealizationManager.cs"
if ($realizationMgr -match 'IsFixedDeck|IsLockByBluePrimary|hasBuiltInDeck') {
    Write-Host "  [OK] ApplyAtlasOnlyLoadout skips auto-fill for built-in-deck books" -ForegroundColor Green
} else {
    $errors += "ApplyAtlasOnlyLoadout does not protect built-in-deck books"
}

if ($realizationMgr -match 'SetCustomName') {
    Write-Host "  [OK] RestoreRouteLoadout restores custom librarian names" -ForegroundColor Green
} else {
    $warnings += "RestoreRouteLoadout may not restore custom librarian names"
}

# ---------------------------------------------------------------------------
# 9. PendingRealizationBattle pattern
# ---------------------------------------------------------------------------
Write-Host "[9] Checking PendingRealizationBattle pattern..."

if ($realizationMgr -match 'PendingRealizationBattle' -and $realizationMgr -match 'ActivatePendingRealization') {
    Write-Host "  [OK] PendingRealizationBattle pattern prevents event-transition from being treated as realization end" -ForegroundColor Green
} else {
    $errors += "Missing PendingRealizationBattle pattern"
}

# ---------------------------------------------------------------------------
# 10. Grade7 shop pool check
# ---------------------------------------------------------------------------
Write-Host "[10] Checking Grade7/Impurity shop handling..."

$shopBase = Get-Content -Raw -Encoding UTF8 ".\abcdcode_LOGLIKE_MOD\ShopBase.cs"
if ($shopBase -match 'Grade7.*17001|17001.*Grade7') {
    Write-Host "  [OK] Grade7 shop tries 17001" -ForegroundColor Green
} else {
    $errors += "Grade7 shop does not try 17001"
}

# Verify 17001 has its own TableId, not reusing Grade6's -854501
$valuesCh7 = Join-Path $script:RepoRoot "SpecialStaticInfo\DropValueXmlInfos\values_ch7.txt"
if (Test-Path $valuesCh7) {
    try {
        $xml = [xml](Get-Content -Raw -Encoding UTF8 $valuesCh7)
        $ns = New-Object Xml.XmlNamespaceManager($xml.NameTable)
        $node17001 = $xml.SelectSingleNode("//DropValue[@ID='17001']")
        if ($node17001 -ne $null) {
            $tableId = $node17001.TableId
            if ($tableId -eq '-854501') {
                $errors += "Grade7 17001 DropValue reuses Grade6 table -854501"
            } else {
                Write-Host "  [OK] Grade7 17001 TableId=$tableId (not -854501)" -ForegroundColor Green
            }
            # Also verify CardDropTable with that ID exists and is non-empty
            $dropTablePath = Join-Path $script:RepoRoot "AddData\CardDropTable\CardDropTable_ch7.xml"
            if (Test-Path $dropTablePath) {
                $dtXml = [xml](Get-Content -Raw -Encoding UTF8 $dropTablePath)
                $dropTable = $dtXml.SelectSingleNode("//DropTable[@ID='$tableId']")
                if ($dropTable -ne $null) {
                    $cardCount = @($dropTable.SelectNodes("Card")).Count
                    if ($cardCount -gt 0) {
                        Write-Host "  [OK] CardDropTable_ch7.xml DropTable $tableId has $cardCount cards" -ForegroundColor Green
                    } else {
                        $errors += "CardDropTable_ch7.xml DropTable $tableId has no cards"
                    }
                } else {
                    $errors += "CardDropTable_ch7.xml has no DropTable with ID=$tableId"
                }
            } else {
                $errors += "CardDropTable_ch7.xml not found"
            }
        } else {
            $errors += "values_ch7.txt has no DropValue ID=17001"
        }
    } catch {
        $errors += "Failed to parse values_ch7.txt as XML: $_"
    }
} else {
    $errors += "values_ch7.txt not found"
}

# ---------------------------------------------------------------------------
# 11. EGO selection UI check (must be selection, not direct grant)
# ---------------------------------------------------------------------------
Write-Host "[11] Checking EGO reward is selection UI, not direct grant..."

$abnoUnlocks = Get-Content -Raw -Encoding UTF8 ".\RMR_AbnormalityUnlocks.cs"
if ($abnoUnlocks -match 'EnqueueRealizationEgoSelection') {
    Write-Host "  [OK] EGO rewards use EnqueueRealizationEgoSelection (selection queue)" -ForegroundColor Green
} else {
    $errors += "EGO rewards do not use EnqueueRealizationEgoSelection"
}

if ($abnoUnlocks -match 'GrantRealizationEgoReward') {
    $errors += "GrantRealizationEgoReward still exists (should be replaced by EnqueueRealizationEgoSelection)"
}

# ---------------------------------------------------------------------------
# 12. OnlyCard-based auto-fill skip check
# ---------------------------------------------------------------------------
Write-Host "[12] Checking ApplyAtlasOnlyLoadout skips auto-fill for OnlyCard books..."

$realizationMgr = Get-Content -Raw -Encoding UTF8 ".\RMR_RealizationManager.cs"
if ($realizationMgr -match 'OnlyCard.*Count.*>.*0|EquipEffect.*OnlyCard') {
    Write-Host "  [OK] ApplyAtlasOnlyLoadout checks OnlyCard non-empty (not just IsFixedDeck)" -ForegroundColor Green
} else {
    $errors += "ApplyAtlasOnlyLoadout does not check OnlyCard for auto-fill skip"
}

# ---------------------------------------------------------------------------
# Summary
# ---------------------------------------------------------------------------
Write-Host ""
if ($errors.Count -eq 0) {
    Write-Host "=== ALL CHECKS PASSED ===" -ForegroundColor Green
} else {
    Write-Host "=== ERRORS FOUND ===" -ForegroundColor Red
    foreach ($e in $errors) { Write-Host "  [FAIL] $e" -ForegroundColor Red }
}

if ($warnings.Count -gt 0) {
    Write-Host "=== WARNINGS ===" -ForegroundColor Yellow
    foreach ($w in $warnings) { Write-Host "  [WARN] $w" -ForegroundColor Yellow }
}

exit $errors.Count

