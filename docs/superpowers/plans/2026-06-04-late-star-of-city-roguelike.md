# Late Star Of The City Roguelike Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Expose and stabilize the existing late Star of the City chapter 6 content as a roguelike-adapted playable route.

**Architecture:** Keep the existing data-driven model. Use `RMR_Core.cs` for entry/debug availability and existing XML files for stage/reward pool tuning. Add a lightweight validation script so missing stage, unit, reward, and localization links can be checked without launching the game.

**Tech Stack:** C# .NET Framework 4.8, Library of Ruina mod XML, PowerShell, Python for static XML validation.

---

### Task 1: Add Static Validation

**Files:**
- Create: `tools/validate_late_star_ch6.py`

- [x] **Step 1: Create the validation script**

Create `tools/validate_late_star_ch6.py` that parses chapter 6 stage lists, stage definitions, enemy units, key pages, and reward IDs.

- [x] **Step 2: Run validation**

Run: `python tools/validate_late_star_ch6.py`

Expected: A report listing stage IDs `60014`, `60020`, `60021`, `60022`, `60023`, whether their units exist, whether key rewards exist, and any missing links.

### Task 2: Expose Chapter 6 Debug Entry

**Files:**
- Modify: `RMR_Core.cs`

- [x] **Step 1: Enable the `-6854` debug invitation**

In `RMRCore.booksToAddToInventory`, include `new LorId(LogLikeMod.ModId, -6854)` in the static debug book list.

- [x] **Step 2: Re-run static validation**

Run: `python tools/validate_late_star_ch6.py`

Expected: Validation reports the debug entry as enabled.

### Task 3: Tune Chapter 6 Pool Shape

**Files:**
- Modify: `SpecialStaticInfo/StagesXmlInfos/Stage_ch6.xml`

- [x] **Step 1: Review chapter 6 stage typing**

Confirm that `60014`, `60021`, `60022`, and `60023` are boss candidates. Keep ordinary late Star of the City receptions as normal stages.

- [x] **Step 2: Preserve roguelike pacing**

If the pool is too boss-heavy, reduce fixed boss density by keeping only the intended late boss candidates as `Boss` and leaving normal R Corp./Liu/Index entries as `Normal`.

- [x] **Step 3: Re-run validation**

Run: `python tools/validate_late_star_ch6.py`

Expected: The report shows at least four late-Star boss candidates and no missing stage IDs.

### Task 4: Validate Reward Integration

**Files:**
- Inspect: `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch6.xml`
- Inspect: `Localize/cn/BookInfo/_Books.txt`
- Modify only if validation finds missing reward IDs.

- [x] **Step 1: Confirm key rewards**

Check that reward IDs for R Corp., Purple Tear, Xiao, and Yan key pages exist in `EquipReward_ch6.xml`.

- [x] **Step 2: Add missing reward entries if needed**

Validation found no missing reward entries, so no reward XML changes were needed.

If validation reports a missing reward, add a `RewardList` entry using the existing `EquipPage` ID and `ArtWork` ID.

- [x] **Step 3: Re-run validation**

Run: `python tools/validate_late_star_ch6.py`

Expected: No missing chapter 6 key reward IDs.

### Task 5: Build Verification

**Files:**
- No source changes expected unless compilation fails.

- [x] **Step 1: Compile with MSBuild if available**

Run: `msbuild "RogueLike Mod Reborn.sln" /p:Configuration=Release`

Expected: Project compiles or reports environment-specific missing .NET/MSBuild tooling.

Observed: `msbuild` was not available in PATH and no Visual Studio MSBuild path was found.

- [x] **Step 2: If MSBuild is unavailable, document the fallback**

Open the solution in Visual Studio 2022 and build `Release|Any CPU`.

Expected: `bin/Release/RogueLike Mod Reborn.dll` is produced.

### Task 6: Game Test Handoff

**Files:**
- No source changes.

- [ ] **Step 1: Copy the built DLL and data directories**

Copy `bin/Release/RogueLike Mod Reborn.dll`, `AddData`, `Localize`, `SpecialStaticInfo`, and `StoryInfo` into the mod's `Assemblies/dlls` deployment folder.

- [ ] **Step 2: Start from chapter 6 debug entry**

In Library of Ruina, use the `-6854` debug invitation entry to begin at chapter 6.

- [ ] **Step 3: Check runtime behavior**

Verify that chapter 6 starts, late-Star encounters appear, R Corp./Purple Tear/Xiao/Yan fights load, rewards resolve after victory, and no error log entries reference missing scripts, missing unit IDs, or missing XML data.
