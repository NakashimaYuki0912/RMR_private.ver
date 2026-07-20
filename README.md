# Roguelike Mod REBORN（LoR-RMR）— 本机改造版

基于 **Library of Ruina** 模组 Roguelike Mod REBORN 的非官方改造与维护仓库。  
作者发布页：[Workshop 3743867841](https://steamcommunity.com/sharedfiles/filedetails/?id=3743867841) · 上游原作参考：[3503523710](https://steamcommunity.com/sharedfiles/filedetails/?id=3503523710)。  
原作 / 原模组请优先支持；本仓库用于本地可玩修复、中文本地化质量、解放战与 Roguelike 流程加固。

> AI 与人类协作项目。欢迎提 issue；改代码前请先读 **Agent 文档**，避免再次引入「口口口 / 糊字 / 软锁」。

---

## 给后续 Agent / 协作者（最重要）

**从这里开始，不要直接盲改：**

1. **[AGENTS.md](./AGENTS.md)** — 项目长期规则（玩法、禁区、验证等级）  
2. **[docs/agent-handbook/00-INDEX.md](./docs/agent-handbook/00-INDEX.md)** — 分册手册  
3. **[docs/agent-handbook/01-localization-fonts.md](./docs/agent-handbook/01-localization-fonts.md)** — **防中文口口口与糊字**  
4. **[docs/HANDOFF.md](./docs/HANDOFF.md)** — 当前交接快照  

### 仓库路径（本机）

```text
源码 + git 根目录（在这里改、在这里 commit）:
  D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\

外层文件夹不是 git 仓库:
  D:\VS_program\ruina-roguelike-reborn-main\

原作者只读对照:
  D:\VS_program\ruina-roguelike-reborn-main\original-codes\

本地测试目录（不受 Steam 订阅/同步影响）:
  E:\Steam\steamapps\common\Library Of Ruina\LibraryOfRuina_Data\Mods\RMR_REBORN_LOCAL\
    Assemblies\dlls\RogueLike Mod Reborn.dll   ← 日常测试目标

Workshop 发布暂存目录:
  E:\Steam\steamapps\workshop\content\1256670\3743867841\
    Assemblies\dlls\RogueLike Mod Reborn.dll
```

### 一键构建部署（本地测试）

```powershell
cd "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"
# 先完全退出 Library of Ruina
powershell -ExecutionPolicy Bypass -File .\tools\packaging\deploy_local.ps1 -Configuration Release
```

模组列表会显示 **`[LOCAL TEST] RMR REBORN fan work`**。若同时订阅 Workshop 版，两项可同时显示，但测试时只能启用其中一个；二者共用内部 package ID，不能并行加载。

启动游戏后，在日志中确认：

```text
%USERPROFILE%\AppData\LocalLow\Project Moon\LibraryOfRuina\Player.log
→ [RMR] RogueLike Mod Reborn initializing. Build: <与 RMR_Core.BuildTimestamp 一致>
```

### 绝对不要做的事（摘要）

| 禁止 | 后果 |
|---|---|
| 自建 UI：`tmp.font = ...` + `FontStyles.Bold` | 中文糊 / 坏 SDF |
| ANSI/GBK 保存 Localize 或中文源码 | 口口口、乱码 |
| 游戏运行中覆盖 DLL | 加载失败或测到旧代码 |
| 未部署却声称游戏内已修 | 误导用户 |
| 整仓覆盖 `original-codes` | 丢失解放战/图鉴等改造 |
| 情感选书把未知页当 I 级 | 终局页过早出现 |
| 商店离开不做 NonCombat 标记 | 卡在免疫商人 |

正确赋字体：

```csharp
LogLikeMod.ApplyTmpFontPreservingSharpMaterial(tmp, LogLikeMod.DefFont_TMP);
```

---

## 项目是什么

把废墟图书馆接待改成 Roguelike 路线：

- 章节推进：普通战、精英、Boss、商店、休息、神秘事件  
- 异想体书页 / E.G.O. 与 **楼层解放战**门控  
- 永久图鉴与路线持有分离  
- 开局全屏 Hub：正常游玩 / 继续 / 挑战解放战 / 玩法介绍 / 图鉴  

技术栈：C# / Harmony / Unity TMP，目标框架 .NET Framework 4.8，依赖见 `dependencies/`。

---

## 目录结构（精简）

```text
RMR_*.cs                      模组入口与各系统
abcdcode_LOGLIKE_MOD/         核心逻辑、商店、奖励、UI、存档（体量最大）
abcdcode_Refactored/          Harmony 补丁（LogLikePatches.cs 等）
AddData/                      卡牌、敌人、Stage 等游戏数据
SpecialStaticInfo/            节点图、事件、奖励池
Localize/{cn,en,kr}/          本地化（UTF-8）
ArtWork/ StoryInfo/ ...       资源
tools/packaging/              deploy_workshop.ps1, pack_mod.ps1
tools/static_checks/          回归脚本
docs/agent-handbook/          Agent 分册文档
```

---

## 玩家安装（Mods 目录方式）

若使用 zip 手动安装（非 Workshop 订阅）：

1. 下载 Release / 打包 zip  
2. 解压到：

   `...\Library Of Ruina\LibraryOfRuina_Data\Mods\<你的文件夹>\`

3. 保证 **第一层** 能看到类似：

   - `RogueLike Mod Reborn.dll` **或** 本项目 Workshop 布局下的 `Assemblies\dlls\...`  
   - 以及 `StageModInfo.xml`、数据目录  

4. **不要**多套一层空文件夹；更新时建议删旧文件夹再装。  

**注意：** 本机开发以 **作者 Steam Workshop 目录 3743867841** 为准（上游原作 3503523710 仅作对照）；`deploy_workshop.ps1` 默认写作者路径。

作者 Workshop 发布页：  
https://steamcommunity.com/sharedfiles/filedetails/?id=3743867841  

---

## 开发工作流

| 步骤 | 命令 / 动作 |
|---|---|
| 改源码 | 仅在 git 根目录 |
| 更新 Build 戳 | `RMR_Core.BuildTimestamp` |
| 部署 | `tools\packaging\deploy_workshop.ps1 -Configuration Release` |
| 打上传包 | `tools\packaging\pack_mod.ps1` |
| 静态检查 | `tools\static_checks\**\*.ps1` |
| 验证 | 完全重启游戏 + 对 `Player.log` Build |

Steam Workshop **上传/更新**必须由账号所有者在 Steam 客户端操作；工具只能帮你打干净包。

---

## 近期修复方向（2026-07）

- 中文显示：口口口、Hub/解放战/图鉴糊字  
- 情感书页按原版 EmotionLevel 分层（避免低情感出科技终局页）  
- 中段 E.G.O. 仅已拥有；手牌不被 E.G.O. 淹没  
- 商店/神秘离开后不再卡免疫 NPC  
- 空库存无意义提示已移除  

详见 [docs/agent-handbook/04-known-bugs-and-fixes.md](./docs/agent-handbook/04-known-bugs-and-fixes.md)。

---

## 许可与致谢

- 见 [LICENSE](./LICENSE)  
- 原 Roguelike 模组作者 ABCDCODE / 社区 REBORN 维护者  
- Project Moon《Library of Ruina》  

本改造不附属 Project Moon 官方。

---

## 文档地图

| 文件 | 读者 |
|---|---|
| [AGENTS.md](./AGENTS.md) | **所有改代码的 agent** |
| [docs/agent-handbook/](./docs/agent-handbook/) | Agent 分册（字体/部署/架构/禁区） |
| [docs/HANDOFF.md](./docs/HANDOFF.md) | 会话交接 |
| [RELEASE.md](./RELEASE.md) | 历史发布说明 |
| [LoR_modding_background.md](./LoR_modding_background.md) | LoR 模组背景 |
