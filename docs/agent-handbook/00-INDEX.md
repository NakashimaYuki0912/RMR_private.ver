# RMR Agent Handbook — 索引

> **谁该读这份文档？**  
> 任何第一次打开本仓库、要改 C# / XML / 本地化 / 部署的 agent 或人类。  
> 读完 `00-INDEX` + `01-localization-fonts` 再动代码，可大幅降低「口口口 / 乱码 / 糊字」回归概率。

## 必读顺序（新会话 5 分钟）

| 顺序 | 文件 | 为什么 |
|:---:|---|---|
| 1 | 本文件 | 地图与禁区 |
| 2 | [01-localization-fonts.md](./01-localization-fonts.md) | **防止 口口口 / 糊字**（历史高发） |
| 2b | [../localization/README.md](../localization/README.md) | **人类译者入口**（术语表 / EN 指南 / 文件地图） |
| 3 | [02-build-deploy-verify.md](./02-build-deploy-verify.md) | 改完怎么进游戏、怎么证明加载了新 DLL |
| 4 | [03-architecture-map.md](./03-architecture-map.md) | 改哪几个文件、调用链在哪 |
| 5 | [04-known-bugs-and-fixes.md](./04-known-bugs-and-fixes.md) | 2026-07 已修问题，避免重开洞 |
| 6 | [05-forbidden-and-safe-patterns.md](./05-forbidden-and-safe-patterns.md) | 明确「禁止」与「推荐」写法 |
| 7 | 仓库根 [AGENTS.md](../../AGENTS.md) | 长期玩法规则与验证等级 |
| 8 | 仓库根 [README.md](../../README.md) | 人类可读总览 |

## 仓库位置（本机）

```text
源码根（真正的 git 仓库）:
  D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\

外层目录（不是 git 根）:
  D:\VS_program\ruina-roguelike-reborn-main\

原作者只读对照:
  D:\VS_program\ruina-roguelike-reborn-main\original-codes\

Workshop 运行树（游戏实际加载）:
  E:\Steam\steamapps\workshop\content\1256670\3743867841\
```

**常见致命错误：** 在外层 `...\ruina-roguelike-reborn-main\` 里改文件或 `git`，而真正代码在内层 `...\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\`。

## 四类运行输入（必须分开想）

1. **C# DLL** — 编译产物 `RogueLike Mod Reborn.dll`（流程 / Harmony / UI）
2. **游戏数据** — `AddData/`、`SpecialStaticInfo/`
3. **本地化** — `Localize/{cn,en,kr}/`（缺键 → 英文或空白，**不是**字体问题）
4. **资源** — `ArtWork/`、`AssetBundle/`、`AudioClip/`、`Spine/`、`StoryInfo/`

只改源码不部署 → 游戏仍跑旧 DLL。  
只部署 DLL 不同步 XML → 新数据不进游戏。  
改本地化文件却用系统默认 ANSI 保存 → **乱码 / 口口口**。

## 当前 Build 戳（部署后必须在 Player.log 对上）

源码：`RMR_Core.BuildTimestamp`  
日志：`[RMR] RogueLike Mod Reborn initializing. Build: ...`

若日志里的 Build 字符串与源码不一致 → **玩家仍在用旧 DLL**，任何「修好了」的结论无效。

## 交接快照

- 会话级交接：`docs/HANDOFF.md`（随版本更新）
- 根目录旧笔记：`handoff.md`（图鉴升级开关等历史任务，可能过时）
- 玩法规则权威：`AGENTS.md` 第 3–5 节

## 快速命令

```powershell
cd "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"

# 构建 + 部署到 Workshop（游戏必须退出）
powershell -ExecutionPolicy Bypass -File .\tools\packaging\deploy_workshop.ps1 -Configuration Release

# 打干净 zip（上传用，会剥 bak / 备份）
powershell -ExecutionPolicy Bypass -File .\tools\packaging\pack_mod.ps1

# 确认游戏加载了新 DLL
Select-String -Path "$env:USERPROFILE\AppData\LocalLow\Project Moon\LibraryOfRuina\Player.log" -Pattern "Build:"
```
