# Abnormality Page Unlock Loop Design

## Goal

Make abnormality page access vary by run instead of starting from a fixed selection. A run starts with no default abnormality page pool except permanent unlocks, then grows its usable pool through normal fights, abnormality fights, and mystery events.

## Player Experience

New routes begin with an empty current-route abnormality page list, then copy in any permanent unlocks. Battle emotion choices draw only from the current-route unlocked page list. If the current emotion level has no unlocked page, the selector offers a harmless fallback page named "无" so the emotion card flow never stalls.

## Encounter Sources

Normal fights can randomly offer one abnormality page reward selection after battle. Elite fights have a higher chance to offer one selection.

Abnormality battle nodes are added to the route map as a selectable stage card named "异想体战斗". They use the fourth user-provided image as the stage card artwork. Winning one grants three consecutive abnormality page reward selections.

Mystery events are added for "黑森林", "井", and "光中的手". They use the first three user-provided images as event backgrounds. They do not start follow-up fights; they directly show one abnormality page reward selection.

Boss fights do not add extra random abnormality page drops.

## Difficulty Pools

Simple pool: history, technological sciences, literature, and art. Available in Canard, Urban Myth, and Urban Legend.

Medium pool: natural sciences, language, and social sciences. Available in Urban Plague and Urban Nightmare.

Hard pool: general works, religion, and philosophy. Available in Star of the City.

Reward candidates and abnormality battle nodes both use the current progress pool.

## Unlock Unit

The unlock unit is one abnormality page, not a whole abnormality group. Picking a reward adds that page's `LorId` to the current route's unlocked abnormality page list.

Duplicate picks are filtered out where possible. If fewer than three eligible locked pages remain, the reward selector shows the available count. If no eligible locked page remains, it falls back to "无" and exits cleanly.

## Permanent Progression

Permanent progression is stored outside the route save and survives new routes.

First clear of Urban Plague permanently unlocks E.G.O pages plus final emotion level abnormality pages for history, technological sciences, literature, and art.

First clear of Urban Nightmare permanently unlocks E.G.O pages plus final emotion level abnormality pages for natural sciences, language, and social sciences.

First clear of Star of the City permanently unlocks E.G.O pages plus final emotion level abnormality pages for general works, religion, and philosophy.

When a new route starts, permanent abnormality page unlocks are copied into the route's initial unlocked abnormality page list. Permanent E.G.O unlocks are applied to the relevant E.G.O availability path while preserving the original game's floor and emotion requirements.

## Reward Rates

Initial values:

- Normal fight abnormality page drop chance: 30%.
- Elite fight abnormality page drop chance: 50%.
- Abnormality fight reward count: 3 selections.
- Mystery event reward count: 1 selection.

These values should be constants near the new unlock manager so they are easy to tune after playtesting.

## Data And Assets

The four supplied images become named artwork assets:

- `RMR_AbnoEvent_BlackForest`
- `RMR_AbnoEvent_Well`
- `RMR_AbnoEvent_HandsOfLight`
- `RMR_Stage_AbnormalityBattle`

The stage card display text adds `Stage_Creature` as "异想体战斗" and `Stage_Creature_Desc` as a short high-risk, high-reward description.

Chinese mystery text is the source of truth for this pass. English and Korean can use simple fallback translations if needed to avoid missing text.

## Architecture

Add a focused abnormality unlock service that owns current-route unlocked abnormality page IDs, permanent unlock flags, permanent page IDs, candidate filtering by progress pool and emotion level, and reward batch creation for each source.

Hook battle reward setup after stage clear so normal and elite fights can enqueue abnormality page reward selections without disturbing existing combat page, passive, book, and next-stage rewards.

Hook emotion card selection so `RewardingModel.GetCurEmotion()` reads from the route unlock list instead of fixed `EmotionSelectDic` creature tab choices. Preserve original level filtering, selected-card filtering, and target behavior.

Add mystery models for the three direct reward events. Their click handler enqueues one abnormality page reward selection and then closes the event.

Add stage XML entries for abnormality battle nodes per chapter. Add stage display handling for `StageType.Creature` so the route map can show the new card name and artwork.

## Error Handling

If artwork is missing, the event and stage card should still load using an existing fallback artwork.

If a reward candidate cannot resolve to a registered pickup or emotion card, skip it and log only under additional logging.

If no eligible abnormality pages exist for a reward source, show the "无" fallback and do not throw.

If permanent progression save data is absent or corrupt, recreate it with all flags false.

## Testing

Test-first checks should cover: new route starts without fixed creature-tab pages; permanent unlocks load into a new route; candidate filtering respects simple, medium, and hard pools; filtering respects emotion level and already-unlocked pages; reward sources enqueue the expected counts; missing candidates produce "无"; and `StageType.Creature` stage cards localize as "异想体战斗" with the new artwork key.

Static XML validation should verify unique new mystery IDs, unique new stage IDs, existing Chinese localize keys, and source-to-Workshop XML parity after deployment.

Release build and DLL deployment remain required before handing off for in-game testing.
