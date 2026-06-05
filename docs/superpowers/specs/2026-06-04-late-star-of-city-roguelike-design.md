# Late Star Of The City Roguelike Design

## Goal

Complete the mod's late Star of the City chapter as a roguelike-adapted chapter, not a one-to-one recreation of the base game's late receptions.

## Scope

This first pass stays inside the existing `Grade6`/chapter 6 content. It should expose and stabilize the already-present late Star of the City data: R Corp. II, the Purple Tear, Xiao, and Distorted Yan. Impuritas Civitatis content is out of scope.

## Current Findings

The project already contains most late Star of the City data. `SpecialStaticInfo/StagesXmlInfos/Stage_ch6.xml` includes stage IDs `60014`, `60020`, `60021`, `60022`, and `60023` as boss entries. `AddData/StageInfo/StageInfo_ch6.xml` defines the corresponding encounters, including R Corp. captains, Purple Tear, Xiao, and Yan. Card, book, enemy, and localization data also exists for those families.

The main gap is integration. The chapter 6 debug start book `-6854` exists as `RoguelikeGamemode_RMR_Modded_DebugCh6`, but it is commented out in the inventory injection list. Chapter 6 also needs a safer stage pool shape for roguelike play so the player sees R Corp. II as a late elite/boss path and has multiple boss candidates without turning every late reception into a back-to-back hard wall.

## Approach

Use existing data first. The implementation should not clone base game XML or add large new encounter sets unless a missing chain is discovered during validation.

Chapter 6 will be tuned as follows:

- R Corp. captains (`60014`) remain a major boss/elite-style encounter in the pool.
- Purple Tear (`60021`), Xiao (`60022`), and Distorted Yan (`60023`) remain boss candidates.
- Red Mist (`60020`) remains in the pool only if the existing project already treats it as part of the late chapter route; otherwise it should not block the late-Star completion target.
- The existing roguelike pacing is preserved: chapter 6 initializes with one boss candidate per run, so late Star of the City bosses are seen across multiple runs rather than all in one run.
- The chapter 6 debug start (`-6854`) should be available for direct testing.
- Default and modded roguelike modes should continue to load `Grade6` from `StagesXmlList` through the existing `InitializeChapterStageList` path.

## Testing Strategy

Static validation should confirm that every chapter 6 stage ID points to an existing `StageInfo` entry, every stage unit exists in `EnemyUnitInfo`, and key reward IDs exist in equipment/localization files. Build validation should compile the project with MSBuild or Visual Studio if available.

Game testing should use the chapter 6 debug invitation (`-6854`) to start directly at the late chapter. Testers should verify that chapter 6 can start, that R Corp. II/Purple Tear/Xiao/Yan encounters can appear, that combat begins without missing script exceptions, and that rewards resolve after victory.

## Non-Goals

This pass does not add Hana Association, Olivier, Reverberation Ensemble, Black Silence, or any ending sequence content. It also does not attempt to fully recreate multi-floor base game boss pressure; balance should favor roguelike continuity.
