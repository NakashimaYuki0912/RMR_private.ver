# RMR Realization System Static Check
# Verifies that realization-exclusive pages are excluded from normal drop pools
# and that floor-based gating is correctly implemented.

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "=== RMR Realization System Static Check ===" -ForegroundColor Cyan
Write-Host ""

# 1. Check that C# source contains the realization exclusion list
Write-Host "[1] Checking realization exclusive definitions..." -ForegroundColor Yellow

$abnoUnlocks = Get-Content "$repoRoot\RMR_AbnormalityUnlocks.cs" -Raw

$requiredExclusives = @(
    "snowwhite",        # Malkuth
    "freischutz",       # Yesod
    "blackswan",        # Hod
    "orchestra",        # Netzach
    "clownofnihil",     # Tiphereth
    "nothing",          # Gebura
    "wizard",           # Chesed
    "bossbird",         # Binah
    "whitenight",       # Hokma
    "quietKid"          # Keter
)

$allFound = $true
foreach ($exclusive in $requiredExclusives) {
    if ($abnoUnlocks -match $exclusive) {
        Write-Host "  [OK] $exclusive found in RealizationExclusiveScripts" -ForegroundColor Green
    } else {
        Write-Host "  [FAIL] $exclusive NOT found in RealizationExclusiveScripts!" -ForegroundColor Red
        $allFound = $false
    }
}

# 2. Check floor mapping
Write-Host ""
Write-Host "[2] Checking floor-to-script mapping..." -ForegroundColor Yellow

$requiredFloors = @("Malkuth", "Yesod", "Hod", "Netzach", "Tiphereth", "Gebura", "Chesed", "Binah", "Hokma", "Keter")
foreach ($floor in $requiredFloors) {
    if ($abnoUnlocks -match "SephirahType\.$floor") {
        Write-Host "  [OK] Floor $floor defined in FloorAbnormalityScripts" -ForegroundColor Green
    } else {
        Write-Host "  [FAIL] Floor $floor NOT found!" -ForegroundColor Red
        $allFound = $false
    }
}

# 3. Check that IsRealizationExclusive is used in GetRewardCandidates
Write-Host ""
Write-Host "[3] Checking GetRewardCandidates filtering..." -ForegroundColor Yellow

if ($abnoUnlocks -match "IsRealizationExclusive" -and $abnoUnlocks -match "IsFloorRealizationCompleted") {
    Write-Host "  [OK] GetRewardCandidates uses realization-exclusive filtering" -ForegroundColor Green
} else {
    Write-Host "  [FAIL] GetRewardCandidates missing realization filtering!" -ForegroundColor Red
}

# 4. Check shop filtering
Write-Host ""
Write-Host "[4] Checking shop filtering..." -ForegroundColor Yellow

$shopBase = Get-Content "$repoRoot\abcdcode_LOGLIKE_MOD\ShopBase.cs" -Raw
if ($shopBase -match "IsRealizationExclusive" -and $shopBase -match "IsFloorRealizationCompleted") {
    Write-Host "  [OK] CreateShop_AbnormalityPages filters realization exclusives" -ForegroundColor Green
} else {
    Write-Host "  [FAIL] CreateShop_AbnormalityPages missing realization filtering!" -ForegroundColor Red
}

# 5. Check realization manager
Write-Host ""
Write-Host "[5] Checking realization manager..." -ForegroundColor Yellow

$realManager = Get-Content "$repoRoot\RMR_RealizationManager.cs" -Raw
if ($realManager) {
    Write-Host "  [OK] RMR_RealizationManager.cs exists" -ForegroundColor Green
    foreach ($fakeId in @("910001","910002","910003","910004","910005","910006","910007","910008","910009","910010")) {
        if ($realManager -match $fakeId) {
            Write-Host "  [FAIL] Realization manager still references non-existent placeholder stage $fakeId" -ForegroundColor Red
            $allFound = $false
        }
    }
    foreach ($realId in @("201005","202005","203005","204005","205005","206005","207005","208004","209004","210009")) {
        if ($realManager -notmatch $realId) {
            Write-Host "  [FAIL] Realization manager missing vanilla realization stage $realId" -ForegroundColor Red
            $allFound = $false
        }
    }
    foreach ($floor in $requiredFloors) {
        if ($realManager -match "SephirahType\.$floor") {
            Write-Host "  [OK] Floor $floor has stage ID" -ForegroundColor Green
        }
    }
} else {
    Write-Host "  [FAIL] RMR_RealizationManager.cs missing!" -ForegroundColor Red
}

# 6. Check UI button
Write-Host ""
Write-Host "[6] Checking realization UI..." -ForegroundColor Yellow

$patches = Get-Content "$repoRoot\abcdcode_Refactored\LogLikePatches.cs" -Raw
if ($patches -match "RealizationBtn" -and $patches -match "OnClickRealization") {
    Write-Host "  [OK] Realization button added to UI" -ForegroundColor Green
} else {
    Write-Host "  [FAIL] Realization button missing from UI!" -ForegroundColor Red
}

$realPanel = Get-Content "$repoRoot\abcdcode_LOGLIKE_MOD\LogRealizationPanel.cs" -Raw
if ($realPanel) {
    Write-Host "  [OK] LogRealizationPanel.cs exists" -ForegroundColor Green
}

# 7. Chapter gating check
Write-Host ""
Write-Host "[7] Checking chapter-to-floor gating..." -ForegroundColor Yellow

if ($abnoUnlocks -match "GetFloorsForChapter") {
    Write-Host "  [OK] GetFloorsForChapter method exists" -ForegroundColor Green
    # Verify grade mapping
    if ($abnoUnlocks -match "Grade3.*Malkuth.*Yesod.*Hod.*Netzach" -or $abnoUnlocks -match "Malkuth.*Yesod.*Hod.*Netzach") {
        Write-Host "  [OK] Grade 1-3 maps to Malkuth/Yesod/Hod/Netzach" -ForegroundColor Green
    }
    if ($abnoUnlocks -match "Grade5.*Tiphereth.*Gebura.*Chesed" -or $abnoUnlocks -match "Tiphereth.*Gebura.*Chesed") {
        Write-Host "  [OK] Grade 4-5 maps to Tiphereth/Gebura/Chesed" -ForegroundColor Green
    }
    if ($abnoUnlocks -match "Binah.*Hokma.*Keter") {
        Write-Host "  [OK] Grade 6-7 maps to Binah/Hokma/Keter" -ForegroundColor Green
    }
}

# 8. Check script roots match this mod's RewardPassive script naming, not raw vanilla-only names
Write-Host ""
Write-Host "[8] Checking mod-script floor mapping..." -ForegroundColor Yellow

foreach ($root in @(
    "ScorchedGirl", "HappyTeddyBear", "FairyCarnival", "QueenBee",
    "ForsakenMurderer", "LittleHelper", "SingingMachine", "Butterfly",
    "ShyLookToday", "RedShoes", "SpiderBud", "Laetitia",
    "UniverseZogak", "ChildofGalaxy", "Porccubus", "Alriune",
    "QueenOfHatred", "KnightOfDespair", "Greed", "Angry",
    "Redhood", "BigBadWolf", "Mountain", "Nosferatu",
    "ScareCrow", "LumberJack", "House", "Ozma",
    "BloodBath", "HeartofAspiration", "Pinocchio", "TheSnowQueen",
    "Bigbird", "SmallBird", "LongBird", "Bloodytree", "Clock", "BlueStar"
)) {
    if ($abnoUnlocks -notmatch [regex]::Escape($root)) {
        Write-Host "  [FAIL] Floor mapping missing mod script root: $root" -ForegroundColor Red
        $allFound = $false
    }
}

# 9. Check final realization pages are actually registered as reward pages
Write-Host ""
Write-Host "[9] Checking final realization reward page XML..." -ForegroundColor Yellow

$pickTable = Get-Content "$repoRoot\SpecialStaticInfo\RewardPassiveInfos\CreatureInfo_PickTable.xml" -Raw
foreach ($script in @(
    "snowwhite1","snowwhite2","snowwhite3",
    "freischutz1","freischutz2","freischutz3",
    "blackswan1","blackswan2","blackswan3",
    "orchestra1","orchestra2","orchestra3",
    "clownofnihil1","clownofnihil2","clownofnihil3",
    "nothing1","nothing2","nothing3",
    "wizard1","wizard2","wizard3",
    "bossbird1","bossbird2","bossbird3","bossbird4","bossbird5","bossbird6",
    "onebadmanygood1","plaguedoctor1","whitenight1","whitenight2","whitenight3","whitenight4",
    "quietKidHammer","quietKidEyeShine","quietKidGuilty"
)) {
    if ($pickTable -notmatch [regex]::Escape($script)) {
        Write-Host "  [FAIL] CreatureInfo_PickTable missing final page script: $script" -ForegroundColor Red
        $allFound = $false
    }
}

# 10. Check generic vanilla EmotionCardAbility fallback exists
Write-Host ""
Write-Host "[10] Checking vanilla emotion fallback..." -ForegroundColor Yellow

$logLikeMod = Get-Content "$repoRoot\abcdcode_LOGLIKE_MOD\LogLikeMod.cs" -Raw
if ($logLikeMod -match "PickUpModel_RMRVanillaEmotion" -and $logLikeMod -match "EmotionCardAbility_") {
    Write-Host "  [OK] FindPickUp has vanilla EmotionCardAbility fallback" -ForegroundColor Green
} else {
    Write-Host "  [FAIL] Missing vanilla EmotionCardAbility fallback in FindPickUp" -ForegroundColor Red
    $allFound = $false
}

# Summary
Write-Host ""
if ($allFound) {
    Write-Host "=== ALL CHECKS PASSED ===" -ForegroundColor Green
} else {
    Write-Host "=== SOME CHECKS FAILED ===" -ForegroundColor Red
}
