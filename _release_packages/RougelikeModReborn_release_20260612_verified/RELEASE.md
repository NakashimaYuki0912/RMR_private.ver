# RogueLike Mod Reborn Release Notes

## Version

- Date: 2026-06-12
- Game: Library Of Ruina
- Workshop ID: `abcdcodecalmmagma.LogueLikeReborn`

## Included fixes

- Fixed the `FieldAccessException` crashes in `RMR_Core.cs` by replacing direct private-field access with reflection helpers.
- Adjusted shop layout so item cards use tighter spacing and `1.0` scale instead of overlapping the side panel and bottom area.
- Removed abnormality battle generation from floor 1 and floor 2. Abnormality battles start appearing again from floor 3.
- Kept reward deduplication and abnormality unlock changes in the playable build.

## Main changed files

- `RMR_Core.cs`
- `abcdcode_LOGLIKE_MOD/ShopBase.cs`
- `abcdcode_LOGLIKE_MOD/ShopGoods_Card.cs`
- `abcdcode_LOGLIKE_MOD/ShopGoods_Passive.cs`
- `abcdcode_LOGLIKE_MOD/LogueBookModels.cs`
- `abcdcode_LOGLIKE_MOD/RewardingModel.cs`
- `RMR_AbnormalityUnlocks.cs`

## Build command

```powershell
dotnet msbuild "RogueLike Mod Reborn.csproj" /p:Configuration=Release
```

The verified DLL can also be built to a temporary output folder during validation. The currently deployed playable DLL is the one under the Workshop mod directory.

## Release package structure

The playable package should keep the same structure as the in-game mod folder:

```text
RogueLike Mod Reborn/
  Assemblies/
    dlls/
      RogueLike Mod Reborn.dll
      RogueLike Mod Reborn.xml
      AddData/
      ArtWork/
      AssetBundle/
      AudioClip/
      DevNuggets/
      Localize/
      SpecialStaticInfo/
      Spine/
      StoryInfo/
  Data/
  mod infos/
  Resource/
  StageModInfo.xml
  desc.txt
  old changelogs.txt
  preview.jpg.png
```

Do not include local backup DLLs such as `*.bak` in the release package.

## Publishing notes

- GitHub Release: upload a zip of the playable mod folder only. Do not upload source code, `_release_packages`, temp builds, or backup DLLs as the playable asset.
- Steam Workshop: the same playable mod folder can be uploaded if its directory structure matches the game-loaded folder above.
- If you use one package for both GitHub and Workshop, make sure it contains only the playable files and no local backup artifacts.

## Known notes

- Visual shop layout should be checked once in game after deployment because this validation only confirmed the code change and build.
- Floor 1 and floor 2 now exclude `Creature` nodes from `VanillaGamemodeReceptionList`; floor 3 and later still allow them.
- If you update the Workshop folder manually, fully restart the game before testing so the old DLL is not kept in memory.
