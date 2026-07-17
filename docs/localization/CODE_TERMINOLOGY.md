# Code terminology map (developers)

Player-facing English uses **Compendium** (图鉴 / 도감).  
C# identifiers use the same domain word **Compendium** so code and UI agree.

## Domain → code

| Domain (docs / UI) | Preferred C# name | Notes |
|--------------------|-------------------|--------|
| Compendium / 图鉴 | `LogCompendiumPanel`, `CompendiumUnlocked*`, `EnsureCompendiumUnlocks` | Permanent collection |
| Compendium category | `CompendiumCategory` | RoleBook / BattleCard / Abno / Ego |
| Compendium section | `CompendiumSection` | Urban stage filters |
| Realization | `RMRRealizationManager`, `InRealizationBattle` | Floor Realization |
| Continue save file | disk name **`Lastest`** (historical typo) | Do **not** rename the file; talk about “continue save / Lastest” in comments |
| Roguelike package id | `RMRCore.packageId` / `LogLikeMod.ModId` | Same fan package string historically |
| Team emotion pick index | `LogLikeMod.curemotion` | 0-based counter of abno picks this fight (legacy name) |

## Disk save keys (do not rename without migration)

| C# constant / API | On-disk string | Meaning |
|-------------------|----------------|---------|
| `PermanentCompendiumSaveName` | `RMR_AtlasPermanentUnlocks` | Permanent unlock blob (legacy file name) |
| Continue snapshot | `Lastest` | Full run continue (typo preserved) |
| Save keys inside blob | `atlasRoleBookUnlocks`, etc. | Keep string literals for old saves |

## Localization key ids

UI key **ids** may still contain `Atlas` (e.g. `ui_RMR_Hub_Atlas`). That is intentional stability for translators.  
**Display values** are glossary-aligned (EN: Compendium).

## Why not rename everything on disk?

Binary/XML saves and workshop folders already use historical names. Renaming disk keys without a migration would wipe permanent progress.

## Related docs

- Player glossary: [GLOSSARY.md](GLOSSARY.md)
- Translator guide: [TRANSLATOR_GUIDE_EN.md](TRANSLATOR_GUIDE_EN.md)
