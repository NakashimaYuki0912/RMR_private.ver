# RogueLike Mod Reborn Release Notes

## Version

- Date: 2026-07-13
- Game: Library Of Ruina
- **Your** Workshop content id: `3743867841` (author item to update)
- Original upstream Workshop content id (reference only): `3503523710`
- Workshop package id: `abcdcodecalmmagma.LogueLikeReborn`
- Build stamp: `2026-07-13Trest-abno-g4-g7+08:00`

## Included in this release

### 2026-07-13 rest / abno balance

- **Rest +1** (additive only; other event counts unchanged):
  - Urban Plague / 都市恶疾 (Grade4)
  - Urban Nightmare / 都市梦魇 (Grade5)
  - Star of the City / 都市之星 (Grade6)
  - Impurity / 杂质 (Grade7)
- **Boss liberation-exclusive abnormality 3-picks: 1 → 2** after clearing Grade4–6 bosses
  - E.G.O. 3-pick remains **1**
  - Grade7 (Impurity) exclusive abno pick count unchanged (still 1)

### Earlier same-day package notes (still in build)

- Hub / Help / Atlas polish; StageModInfo `Exist=false` (DLL hub, no broken invitation)
- Hide starter journey/assistant books from atlas; RMR invite text: 开始游玩RMR模组
- Upgraded battle-card names no longer show as 口口口
- Binah degraded pages (`607201`–`607205`) upgrade at rest/shop to full pages (`706201`–`706205`)
- Shop card-upgrade price: **10** eyes base, **+2** after each purchase
- Special core unlock paths retained: Black Silence, Binah (route-local after Red Mist), Blue Reverberation, Red Mist / Gebura

## Build & deploy

```powershell
cd "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"
powershell -ExecutionPolicy Bypass -File .\tools\packaging\deploy_workshop.ps1 -Configuration Release
powershell -ExecutionPolicy Bypass -File .\tools\packaging\prepare_workshop_upload.ps1
# then copy upload tree to BACKUPS path if VDF points there, or:
powershell -ExecutionPolicy Bypass -File .\tools\packaging\UPLOAD_3743867841.ps1
```

Playable tree (**your** item):

```text
E:\Steam\steamapps\workshop\content\1256670\3743867841\
```

## Steam Workshop update (author)

1. Fully exit Library of Ruina.
2. Open **your** Workshop item: https://steamcommunity.com/sharedfiles/filedetails/?id=3743867841  
   (Do **not** update the upstream item `3503523710`.)
3. After cloud upload, re-subscribe / verify folder, fully restart LoR.
4. Confirm `Player.log` contains:
   `Build: 2026-07-13Trest-abno-g4-g7+08:00`

## Package hygiene

- Do not ship `*.bak`, `_codex_backups`, or `DevNuggets` (pack script strips these).
- Keep `StageModInfo.xml`, `Assemblies\dlls\`, `Data\`, `Resource\` structure intact.
- StageModInfo InvitationFile entries must stay `Exist="false"`.
