# RMR Static Checks

These PowerShell scripts are repo-local regression probes created during bug-fix sessions. They are not production mod inputs and are not loaded by Library of Ruina.

Run them from the repository root or by full path:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\tools\static_checks\realization\RMR_0620_grade6_special_fixed_deck_static_check.ps1
```

Each script bootstraps its own repository root by walking upward until it finds `RogueLike Mod Reborn.csproj`, so scripts can stay organized below this folder.

## Categories

- `realization/`: floor realization routing, Binah/Red Mist/Black Silence progression, special fixed decks, and realization reward checks.
- `rewards/`: combat-page rewards, E.G.O. choices, reward queues, boss reward pools, and user-reported reward regressions.
- `shop_atlas/`: shop layout/upgrades, atlas persistence, unlock checks, and shop/localization probes.
- `events_abnormality/`: initial event, mystery event, abnormality battle/unlock, chapter-start localization, and issue-specific event checks.
- `runtime_release/`: startup/runtime regressions, release sync, language sync, stage density, equip-page loader, and chapter-7/end-flow checks.

## Packaging

`tools\packaging\pack_mod.ps1` is the Workshop packaging helper. It packages from the Workshop runtime tree, not directly from source. New root-level zip outputs should go under `_release_packages\archives\`.
