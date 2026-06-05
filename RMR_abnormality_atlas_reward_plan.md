# Abnormality Reward And Atlas Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make abnormality-page rewards deterministic by chapter tier, keep `异想体战斗` as the high-reward route node, and add an atlas that shows owned and unowned key pages, combat pages, abnormality pages, and E.G.O. pages with locked entries rendered as question marks.

**Architecture:** Keep chapter-tier reward selection in `RMR_AbnormalityUnlocks.cs` as the single source of truth for pool filtering and reward counts. Add a separate atlas data/presentation layer that reads from the existing master XML lists and current save data, then renders inside a cloned battle-setting UI with a new `Atlas` tab button. Use the mod's existing artwork loader and copy the four local PNGs into the mod `ArtWork` folder so the UI can reference them by filename keys.

**Tech Stack:** C#, Harmony patches, Unity UI, existing XML/localize loaders, PowerShell static checks, `dotnet msbuild`.

---

## File Structure

- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_AbnormalityUnlocks.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RewardingModel.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_Refactored\LogLikePatches.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogLikeMod.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogueBookModels.cs`
- Create: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_MysteryEvents.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\SpecialStaticInfo\MysteryXmlInfos\chAll_mysterys.xml`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\Localize\cn\UIs.txt`
- Copy into mod assets: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\ArtWork\`
- Test: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_atlas_static_check.ps1`

## Reward Pool Rules

- `传闻 / 都市怪谈 / 都市传说` only roll abnormality pages from the simple pool: `历史 / 科技 / 文学 / 艺术`.
- `都市恶疾 / 都市梦魇` only roll abnormality pages from the medium pool: `自然 / 语言 / 社会`.
- `都市之星` only rolls abnormality pages from the hard pool: `哲学 / 宗教 / 总类`.
- Normal battles always enqueue one abnormality reward choice.
- `异想体战斗` always enqueues three abnormality reward choices.
- Mystery events `黑森林 / 井 / 光中的手` enqueue one abnormality reward choice and do not branch into another battle.
- If the current emotion level has no valid unlocked abnormality page, reward flow still returns the `无` fallback instead of leaving an empty choice screen.

## Atlas Rules

- Atlas sections are `传闻`, `都市怪谈`, `都市传说`, `都市恶疾`, `都市梦魇`, `都市之星`, and `杂质`.
- Atlas categories are `角色书页`, `战斗书页`, `异想体书页`, and `EGO书页`.
- EGO pages are grouped by floor using their `Sephirah` metadata when present.
- Battle pages use the same master card list that the shop and battle reward code already rely on; the atlas only needs read-only access and does not create a new persistence format.
- Locked entries render as a `?` tile with the same frame styling as unlocked entries so the atlas still scans cleanly.

---

### Task 1: Lock Down The Reward Rules With A Failing Static Check

**Files:**
- Create: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_atlas_static_check.ps1`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_AbnormalityUnlocks.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_Refactored\LogLikePatches.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\Localize\cn\UIs.txt`

- [ ] **Step 1: Write the failing static check**

```powershell
$root = "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"
$content = Get-Content (Join-Path $root "RMR_AbnormalityUnlocks.cs") -Raw
foreach ($pattern in @(
  "NormalDropChance",
  "EliteDropChance",
  "AbnormalityBattleRewardCount",
  "MysteryRewardCount",
  "GetRewardCandidates",
  "GetTierForChapter",
  "GetTierForScript"
)) {
  if ($content -notmatch [regex]::Escape($pattern)) { throw "Missing $pattern" }
}
$patches = Get-Content (Join-Path $root "abcdcode_Refactored\\LogLikePatches.cs") -Raw
foreach ($pattern in @(
  "LogLikeRoutines.OnClickAtlasTab",
  "UIBattleSettingEditTap)5",
  "AtlasBtn",
  "AtlasBtnFrame"
)) {
  if ($patches -notmatch [regex]::Escape($pattern)) { throw "Missing $pattern" }
}
"RMR ATLAS STATIC CHECK PASSED"
```

- [ ] **Step 2: Run the check and watch it fail**

Run: `powershell -ExecutionPolicy Bypass -File .\\RMR_atlas_static_check.ps1`

Expected: fail on the missing atlas button and atlas tab wiring.

- [ ] **Step 3: Add the reward-rule helper surface**

Implement the chapter-tier helpers in `RMR_AbnormalityUnlocks.cs` so the rest of the feature can call one shared rule set:

```csharp
public static int GetTierForChapter(ChapterGrade grade)
{
    if (grade <= ChapterGrade.Grade3)
        return 1;
    if (grade <= ChapterGrade.Grade5)
        return 2;
    return 3;
}
```

- [ ] **Step 4: Re-run the check**

Run the static check again and confirm it now fails only on atlas UI wiring.

### Task 2: Make Battle Rewards Use The Chapter Pools

**Files:**
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_AbnormalityUnlocks.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RewardingModel.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_Refactored\LogLikePatches.cs`

- [ ] **Step 1: Write the failing behavior for fixed reward counts**

Use the static check to assert that `EnqueueBattleClearRewards()` calls a single-path normal reward and a three-roll creature reward, and that `RewardingModel.GetCurEmotion()` still falls back to `无` when no unlocked page exists.

- [ ] **Step 2: Implement the minimal routing**

Keep `StageType.Normal` and `StageType.Elite` as one abnormality reward choice each, remove the probability gate, and keep `StageType.Creature` as three queued choice screens. Chapter filtering should stay inside `GetRewardCandidates()` so the stage hook stays small.

- [ ] **Step 3: Verify the battle reward flow**

Run the static check and then a Release build:

```powershell
dotnet msbuild ".\\RogueLike Mod Reborn.csproj" -p:Configuration=Release -p:OutputPath="$env:TEMP\\rmr_atlas_build_out\\" -p:BaseIntermediateOutputPath="$env:TEMP\\rmr_atlas_obj_out\\"
```

Expected: build succeeds with no new warnings.

- [ ] **Step 4: Smoke test the battle rewards**

In-game, confirm that:

1. A normal fight always opens one abnormality choice.
2. An `异想体战斗` opens three consecutive abnormality choice screens.
3. A chapter with no valid page at the current emotion level shows `无` instead of a blank reward state.

### Task 3: Build The Atlas Data Model

**Files:**
- Create: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogueBookModels.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogLikeMod.cs`

- [ ] **Step 1: Write the atlas model first**

Add a plain data layer that can build entries from the live master lists without creating new save data:

```csharp
public enum AtlasCategory { RoleBook, BattleCard, AbnormalityPage, EgoPage }
public enum AtlasSection { Rumor, UrbanLegend, UrbanMyth, UrbanIllness, UrbanNightmare, UrbanStar, Impurity }

public sealed class AtlasEntry
{
    public string Title;
    public string Description;
    public Sprite Artwork;
    public bool Unlocked;
    public AtlasCategory Category;
    public AtlasSection Section;
    public SephirahType Floor;
}
```

- [ ] **Step 2: Run the static check and watch it fail on missing UI**

Expected failure: atlas panel and atlas button hooks are still absent.

- [ ] **Step 3: Implement read-only builders**

Build entries from the already-loaded data sources:

1. `BookXmlList.Instance.GetList()` for role pages.
2. `ItemXmlDataList.instance.GetCardItem(...)` lookups for combat pages.
3. `Singleton<RewardPassivesList>.Instance` for abnormality pages.
4. `EmotionEgoXmlInfo` / `LogLikeMod.RewardCardDic_Dummy` for EGO pages, grouped by `Sephirah`.

The atlas should derive `Unlocked` from existing save-state and ownership flags, not from a new atlas save file.

- [ ] **Step 4: Verify the model is stable**

Run the static check again and confirm the atlas model file is present and referenced.

### Task 4: Add The Atlas UI And Battle-Setting Button

**Files:**
- Create: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_Refactored\LogLikePatches.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogLikeMod.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\Localize\cn\UIs.txt`

- [ ] **Step 1: Write the failing UI check**

Add checks for:

```powershell
foreach ($pattern in @(
  "class LogAtlasPanel",
  "AtlasBtn",
  "AtlasBtnFrame",
  "OnClickAtlasTab",
  "SetBUttonState((UIBattleSettingEditTap)5)",
  "ui_AtlasTab"
)) {
  if ($patches -notmatch [regex]::Escape($pattern)) { throw "Missing $pattern" }
}
```

- [ ] **Step 2: Implement the Atlas panel**

Mirror the existing cloned-panel pattern used by `GlobalLogueInventoryPanel`, `LogCreatureTabPanel`, and `LogCraftPanel`, but render:

1. Chapter tabs across the top.
2. Category tabs for `角色书页`, `战斗书页`, `异想体书页`, and `EGO书页`.
3. Floor grouping for EGO entries.
4. Question-mark placeholders for locked entries.

Use the battle-setting canvas clone as the root so the atlas behaves like the other mod panels and can be closed with the existing back-button flow.

- [ ] **Step 3: Add the Atlas button**

In `UIBattleSettingEditPanel_Open`, clone the existing battle-card button one more time, position it after `Craft`, label it `ui_AtlasTab`, and route it to `SetBUttonState((UIBattleSettingEditTap)5)`.

- [ ] **Step 4: Add the Atlas tab state**

Extend the tab-switch hook so state `5` hides the other mod panels and shows `LogAtlasPanel`.

- [ ] **Step 5: Re-run the static check**

Expected: the check passes once atlas wiring is present.

### Task 5: Swap In The Local Artwork Files

**Files:**
- Copy into: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\ArtWork\`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\RMR_MysteryEvents.cs`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\SpecialStaticInfo\MysteryXmlInfos\chAll_mysterys.xml`
- Modify: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogueBookModels.cs`

- [ ] **Step 1: Copy the four PNGs from `D:\LoR_mods`**

Copy these files into the mod `ArtWork` directory so the existing artwork loader can resolve them by filename:

1. `D:\LoR_mods\随机事件背景1.png`
2. `D:\LoR_mods\随机事件背景2.png`
3. `D:\LoR_mods\随机事件背景3.png`
4. `D:\LoR_mods\异想体战斗.png`

- [ ] **Step 2: Rename the artwork keys in XML/code to match the files**

Use the filenames without extensions as the in-game keys:

1. `随机事件背景1`
2. `随机事件背景2`
3. `随机事件背景3`
4. `异想体战斗`

Update the three mystery entries and the creature-stage card text so they point at those keys instead of the earlier placeholder artwork names.

- [ ] **Step 3: Verify the artwork loads**

Run the static check plus a quick in-game open of the mystery event and `异想体战斗` node to confirm the background art is no longer falling back to the placeholder image.

### Task 6: Build, Deploy, And Verify

**Files:**
- Output: `D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\bin\Release\RogueLike Mod Reborn.dll`
- Deploy: `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\`

- [ ] **Step 1: Build Release**

Run:

```powershell
dotnet msbuild ".\\RogueLike Mod Reborn.csproj" -p:Configuration=Release -p:OutputPath="$env:TEMP\\rmr_atlas_build_out\\" -p:BaseIntermediateOutputPath="$env:TEMP\\rmr_atlas_obj_out\\"
```

Expected: exit 0.

- [ ] **Step 2: Deploy the DLL and changed assets**

Copy the rebuilt DLL, the updated XML/localize files, and the four PNGs into the Workshop install.

- [ ] **Step 3: Compare hashes**

Confirm that the source build and Workshop DLL match byte-for-byte after deployment.

- [ ] **Step 4: Smoke test the complete loop**

Test these cases in-game:

1. A normal fight always gives one abnormality choice.
2. An `异想体战斗` gives three choice screens and uses the `异想体战斗` artwork.
3. The three mystery events show the new local background art.
4. The atlas opens from the new button.
5. The atlas shows owned entries as filled art and unowned entries as `?`.
6. EGO pages are grouped by floor.
7. Chapter buckets still respect `传闻 / 都市怪谈 / 都市传说`, `都市恶疾 / 都市梦魇`, `都市之星`, and `杂质`.
