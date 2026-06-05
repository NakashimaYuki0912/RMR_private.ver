# Abnormality Page Unlock Loop Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace fixed abnormality page access with run-based single-page unlocks from normal fights, abnormality fights, and mystery events.

**Architecture:** Add a focused unlock manager in `RMR_AbnormalityUnlocks.cs`, then hook existing reward and emotion-card paths with small calls. XML and localization add the new mystery events and `StageType.Creature` map cards.

**Tech Stack:** C#, Harmony patches, existing RMR XML loaders, PowerShell static checks, `dotnet msbuild`.

---

## File Structure

- Create: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_AbnormalityUnlocks.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_Refactored\LogLikePatches.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RewardingModel.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogueBookModels.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_MysteryEvents.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_Core.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\SpecialStaticInfo\StagesXmlInfos\Stage_ch*.xml`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\SpecialStaticInfo\MysteryXmlInfos\chAll_mysterys.xml`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\Localize\cn\UIs.txt`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\Localize\cn\MysteryAll.xml`
- Test: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_abnormality_unlock_static_check.ps1`

### Task 1: Backup And Failing Static Check

- [ ] **Step 1: Create backup**

Create `D:\VS_program\rmr_backups\pre_abno_unlock_<timestamp>\source` and copy every file listed above before editing. Also copy the current Workshop DLL and `SpecialStaticInfo` XML files before deployment.

- [ ] **Step 2: Write failing static check**

Create `RMR_abnormality_unlock_static_check.ps1` that asserts:

```powershell
$root = "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"
$requiredFiles = @(
  "RMR_AbnormalityUnlocks.cs",
  "RMR_MysteryEvents.cs",
  "abcdcode_Refactored\LogLikePatches.cs",
  "abcdcode_LOGLIKE_MOD\RewardingModel.cs",
  "abcdcode_LOGLIKE_MOD\LogueBookModels.cs",
  "Localize\cn\UIs.txt",
  "SpecialStaticInfo\MysteryXmlInfos\chAll_mysterys.xml"
)
foreach ($file in $requiredFiles) {
  if (!(Test-Path (Join-Path $root $file))) { throw "Missing required file: $file" }
}
$unlock = Get-Content (Join-Path $root "RMR_AbnormalityUnlocks.cs") -Raw
foreach ($pattern in @("class RMRAbnormalityUnlockManager", "NormalDropChance", "EliteDropChance", "AbnormalityBattleRewardCount", "MysteryRewardCount", "GetUnlockedEmotionCardsForBattle", "EnqueueRewardSelections", "RecordPermanentClear")) {
  if ($unlock -notmatch [regex]::Escape($pattern)) { throw "Unlock manager missing $pattern" }
}
$patches = Get-Content (Join-Path $root "abcdcode_Refactored\LogLikePatches.cs") -Raw
foreach ($pattern in @("RMRAbnormalityUnlockManager.GetUnlockedEmotionCardsForBattle", "RMRAbnormalityUnlockManager.EnqueueBattleClearRewards", "StageType.Creature")) {
  if ($patches -notmatch [regex]::Escape($pattern)) { throw "Patch missing $pattern" }
}
$rewarding = Get-Content (Join-Path $root "abcdcode_LOGLIKE_MOD\RewardingModel.cs") -Raw
foreach ($pattern in @("RMRAbnormalityUnlockManager.GetNoAbnormalityFallback", "RMRAbnormalityUnlockManager.OnEmotionPagePicked")) {
  if ($rewarding -notmatch [regex]::Escape($pattern)) { throw "RewardingModel missing $pattern" }
}
$ui = Get-Content (Join-Path $root "Localize\cn\UIs.txt") -Raw
foreach ($pattern in @("Stage_Creature", "异想体战斗", "Stage_Creature_Desc")) {
  if ($ui -notmatch [regex]::Escape($pattern)) { throw "Missing UI localize $pattern" }
}
"ABNORMALITY UNLOCK STATIC CHECK PASSED"
```

- [ ] **Step 3: Run test to verify it fails**

Run: `powershell -ExecutionPolicy Bypass -File .\RMR_abnormality_unlock_static_check.ps1`

Expected: FAIL with missing `RMR_AbnormalityUnlocks.cs`.

### Task 2: Unlock Manager

- [ ] **Step 1: Implement minimal manager**

Create `RMR_AbnormalityUnlocks.cs` with constants, route unlock list, permanent progression load/save helpers, candidate filtering, reward batch creation, and a no-op fallback card helper.

- [ ] **Step 2: Run static check**

Run the static check. Expected: it still fails on missing hook calls.

### Task 3: Battle And Emotion Hooks

- [ ] **Step 1: Replace fixed creature tab battle pool**

In `StageController_StartBattle`, replace the loop that fills `LogueBookModels.EmotionCardList` from `EmotionSelectDic` with `RMRAbnormalityUnlockManager.GetUnlockedEmotionCardsForBattle()`.

- [ ] **Step 2: Enqueue battle clear abnormality rewards**

In the battle end reward setup path, enqueue 1 reward for normal/elite probability hits and 3 rewards for `StageType.Creature`.

- [ ] **Step 3: Handle picked pages and fallback**

In `RewardingModel.GetCurEmotion()` return the fallback card when level has no unlocked candidates. In `StageLibraryFloorModel_OnPickPassiveCard`, call `RMRAbnormalityUnlockManager.OnEmotionPagePicked(card)` after a real abnormality page is picked, and ignore fallback pollution.

- [ ] **Step 4: Run static check**

Expected: static check passes for C# symbols but XML/localize may still fail.

### Task 4: Mystery, Stages, And Localize

- [ ] **Step 1: Add mystery events**

Add three `Mystery` entries to `chAll_mysterys.xml` with unique IDs, scripts, one frame, and one choice.

- [ ] **Step 2: Add mystery scripts**

Add three classes to `RMR_MysteryEvents.cs` that enqueue one mystery reward selection and close normally.

- [ ] **Step 3: Add abnormality battle stage nodes**

Add `StageType="Creature"` entries to each chapter stage XML. Use simple pool chapters for Grade1-Grade3, medium for Grade4-Grade5, and hard for Grade6.

- [ ] **Step 4: Add localize text**

Add Chinese UI and mystery text. Add simple English/Korean fallback only if their files require the keys to avoid missing text.

- [ ] **Step 5: Run static check**

Expected: `ABNORMALITY UNLOCK STATIC CHECK PASSED`.

### Task 5: Build, Deploy, And Verify

- [ ] **Step 1: Build Release**

Run: `dotnet msbuild "RogueLike Mod Reborn.csproj" -p:Configuration=Release`

Expected: exit 0 with no new warnings.

- [ ] **Step 2: Deploy DLL and data**

Copy Release DLL and changed XML/localize/assets into `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls`.

- [ ] **Step 3: Verify source and Workshop parity**

Compare hashes for deployed DLL and every changed XML/localize asset.

- [ ] **Step 4: Report focused game-test checklist**

Ask the user to test: new run start, normal fight drop, mystery event choice, abnormality battle triple reward, emotion choice fallback, and first-clear permanent unlocks.
