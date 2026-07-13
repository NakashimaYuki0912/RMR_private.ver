# RogueLike Mod Reborn Release Notes

## Version

- Date: 2026-07-13
- Game: Library Of Ruina
- **Your** Workshop content id: `3743867841` (author item to update)
- Original upstream Workshop content id (reference only): `3503523710`
- Workshop package id: `abcdcodecalmmagma.LogueLikeReborn`
- Build stamp: `2026-07-13Trelease-binah-upgrade-shop10+08:00`

## Included in this release

- Hub / Help / Atlas Scheme A polish (atlas BG2, rails/tiles, click/paging fixes)
- Hide starter journey/assistant books from atlas; RMR invite text: 开始游玩RMR模组
- Upgraded battle-card names no longer show as 口口口
- Binah degraded pages (`607201`–`607205`) upgrade at rest/shop to full pages (`706201`–`706205`)
- Shop card-upgrade price: **10** eyes base, **+2** after each purchase
- Special core unlock paths retained: Black Silence, Binah (route-local after Red Mist), Blue Reverberation, Red Mist / Gebura

## Build & deploy

```powershell
cd "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"
powershell -ExecutionPolicy Bypass -File .\tools\packaging\deploy_workshop.ps1 -Configuration Release
powershell -ExecutionPolicy Bypass -File .\tools\packaging\pack_mod.ps1
```

Playable tree (**your** item):

```text
E:\Steam\steamapps\workshop\content\1256670\3743867841\
```

Zip archive (upload helper):

```text
_release_packages\archives\RougelikeModReborn_v*.zip
```

## Steam Workshop update (author)

Agent cannot log into Steam for you. After deploy+pack:

1. Fully exit Library of Ruina.
2. Open **your** Workshop item: https://steamcommunity.com/sharedfiles/filedetails/?id=3743867841  
   (Do **not** update the upstream item `3503523710`.)
3. In-game Workshop author tools: **Update** this item from  
   `E:\Steam\steamapps\workshop\content\1256670\3743867841\`  
   (or from the packed zip contents — keep `Assemblies/`, `StageModInfo.xml`, etc.).
4. Confirm `Player.log` contains:
   `Build: 2026-07-13Trelease-binah-upgrade-shop10+08:00`

## Package hygiene

- Do not ship `*.bak`, `_codex_backups`, or `DevNuggets` (pack script strips these).
- Keep `StageModInfo.xml`, `Assemblies\dlls\`, `Data\`, `Resource\` structure intact.
