# Code commenting & sectioning conventions (RMR)

English is the default language for **code comments**, `#region` labels, and `///` XML docs.  
Player-facing strings stay in `Localize/{cn|en|kr}/`.

## Goals

1. **Navigate** large files via Visual Studio / Rider `#region` folding.
2. **Document intent** without rewriting gameplay logic.
3. **Preserve compatibility**: never rename disk save keys (`Lastest`, `RMR_AtlasPermanentUnlocks`, shop/mystery flash files, etc.).

## Product terminology

| Product (EN) | CN | KR | C# / disk notes |
|--------------|----|----|-----------------|
| Compendium | 图鉴 | 도감 | Types: `Compendium*`, `LogCompendiumPanel`. Localize keys may still say `_Atlas`. Disk: `RMR_AtlasPermanentUnlocks`. |
| Continue Run | 继续 | 이어하기 | Snapshot file name: **`Lastest`** (typo kept). |
| Realization | 完全解放 | 완전개방 | Floor exclusives 15370401–36 gated by `RMRAbnormalityUnlockManager`. |

See also: `docs/localization/GLOSSARY.md`, `CODE_TERMINOLOGY.md`.

## File header template

```csharp
// =============================================================================
// Short English purpose of this file.
// Key invariants / related types / disk keys if any.
// =============================================================================
```

Or the shorter form:

```csharp
// -----------------------------------------------------------------------------
// One-line purpose: TypeName
// Namespace/file: path
// -----------------------------------------------------------------------------
```

## Regions

- Prefer `#region --- English label ---` (triple dashes).
- Multi-class files: one region per public class (`#region --- ClassName ---`).
- Large single types: group by concern (Save/load, UI, Battle hooks, …).
- Keep `#region` / `#endregion` balanced (build does not check this; CI should).

## XML docs

- Public types: `/// <summary>...</summary>` in English.
- Ability scripts may use short auto summaries (`Passive ability: PassiveAbility_…`).
- Prefer clarifying **why** and **invariants** over restating the method name.

## Out of scope of comment passes

- `original-codes/`, `_codex_backups/`, `bin/`, `obj/`
- Decompile scrap files not listed in the `.csproj`
- Renaming public APIs or save keys for “cleanliness”

## Verification

```text
dotnet build "ruina-roguelike-reborn-main/RogueLike Mod Reborn.csproj" -c Release
```
