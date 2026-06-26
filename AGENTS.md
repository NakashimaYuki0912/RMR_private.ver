# LoR-RMR Codex 项目默认指令

本文件是 **Library of Ruina — Roguelike Mod Reborn（LoR-RMR）** 仓库的长期项目提示词。Codex Desktop 打开本仓库后应优先遵守本文件；不要反复执行 `/init` 覆盖它。

> 本文件记录的是最终设计约束和工作方法，不保证当前工作树已经完整实现。开始任何任务前，必须以当前源码、Git 状态、静态检查和构建结果重新核验。

## 0. 项目概览

LoR-RMR 是《Library of Ruina》的非官方 Roguelike 模组改造版。项目核心目标是把原版接待流程改造成按章节推进的 Roguelike 路线：玩家在地图节点中经历普通战斗、精英战、Boss、商店、休息室、神秘事件、异想体奖励与楼层解放战，并通过永久图鉴、路线内奖励和解放战门控逐步扩展可用内容。

本仓库同时包含四类运行输入，排查问题时必须把它们分开看：

1. **C# 模组 DLL**：`.csproj` 编译出的 `RogueLike Mod Reborn.dll`，负责 Harmony 补丁、流程状态、奖励队列、UI、存档和运行时加载。
2. **游戏数据 XML/TXT**：`AddData/`、`SpecialStaticInfo/` 和 `Localize/` 下的 Stage、卡牌、核心书页、掉落、本地化和奖励池。
3. **美术与运行资源**：`ArtWork/`、`AssetBundle/`、`AudioClip/`、`Spine/`、`StoryInfo/` 等资源目录。
4. **验证与交接材料**：`RMR_*static_check.ps1`、`docs/HANDOFF.md`、`D:\sketch.txt` 和本文件。

主要功能边界如下：

| 功能域 | 关键文件 | 主要风险 |
|---|---|---|
| 模组初始化、章节、路线推进 | `RMR_Core.cs`、`RMR_MapManagers.cs`、`abcdcode_LOGLIKE_MOD/LogLikeMod.cs` | 章节状态错位、黑屏、节点类型错误、旧存档兼容 |
| 神秘事件与初始入口 | `RMR_MysteryEvents.cs`、`RMR_chstart.xml`、`MysteryModel_*` | 事件 UI 空引用、初始解放战入口被绕过或重复开启 |
| 楼层解放战 | `RMR_RealizationManager.cs`、`LogRealizationPanel.cs` | Stage ID 包语义错误、路线配置污染、退出后未恢复 |
| 异想体/E.G.O. 奖励门控 | `RMR_AbnormalityUnlocks.cs`、`RewardPassiveInfos/` | 未解放奖励提前进入池、普通异想体被永久化、Boss 奖励空候选 |
| 战斗结算奖励 | `RewardingModel.cs`、`PickUpModel_*`、`MysteryModel_CardReward.cs` | 奖励队列死循环、跳过不推进、核心页和战斗书页互相覆盖 |
| 商店与卡牌升级 | `ShopBase.cs`、`ShopGoods_Card.cs`、`ShopGoods_CardUpgrade.cs`、`LogCardUpgradeManager.cs` | 商品重叠、价格/存档不一致、升级候选非法 |
| 永久图鉴与存档 | `LogueBookModels.cs`、`LoguePlayDataSaver.cs`、`LogAtlasPanel.cs` | 当前路线和永久记录混淆、卡面缺失、旧存档迁移失败 |
| 数据与本地化 | `AddData/`、`SpecialStaticInfo/`、`Localize/{cn,en,kr}/` | XML 无法解析、编码污染、中文界面残留英文触发词 |

维护本项目时，始终区分“源码设计意图”“当前工作树实际状态”“Workshop 运行树实际加载内容”和“游戏内实测结果”。`AGENTS.md` 只提供约束和导航，不替代源码阅读、构建、部署哈希核对或游戏日志验证。

## 1. 启动流程

每次新会话或切换任务时，先完成以下检查，再修改代码：

1. 确认当前目录是真正的源码根目录，应能看到 `RogueLike Mod Reborn.csproj`、`RMR_Core.cs`、`abcdcode_LOGLIKE_MOD/` 等内容；不要在仅有 `.git` 的空工作区中修改。
2. 执行 `git status --short`，检查已有未提交改动。
3. 阅读与任务相关的源码、XML、本地化文件和静态检查脚本，追踪完整调用链，不能仅凭文件名猜测。
4. 用中文简要说明：相关既有规则、可能根因、拟修改文件、验证方法。
5. 采用最小范围修改，不覆盖、回滚或整理用户已有的无关改动。

若本文件与用户当前明确要求冲突，以用户当前要求为最高优先级；发现设计冲突时必须指出，不得静默选择旧方案。

## 2. 项目结构与重点文件

常见目录和职责：

- `RMR_Core.cs`：章节、路线、事件和主流程状态。
- `RMR_MysteryEvents.cs`：神秘事件及初始事件逻辑。
- `RMR_RealizationManager.cs`：解放战入口、原版 Stage 跳转、临时图鉴配置和战后恢复。
- `RMR_AbnormalityUnlocks.cs`：异想体奖励池、解放战完成状态和永久奖励门控。
- `abcdcode_LOGLIKE_MOD/LogueBookModels.cs`：路线持有内容、永久图鉴和存档。
- `abcdcode_LOGLIKE_MOD/LogAtlasPanel.cs`：图鉴界面。
- `abcdcode_LOGLIKE_MOD/LogRealizationPanel.cs`：楼层解放战选择界面。
- `abcdcode_LOGLIKE_MOD/RewardingModel.cs`：战斗结算奖励。
- `abcdcode_LOGLIKE_MOD/ShopBase.cs`：商店商品生成和布局。
- `abcdcode_LOGLIKE_MOD/ShopGoods_Card.cs`：战斗书页购买。
- `abcdcode_LOGLIKE_MOD/ShopGoods_CardUpgrade.cs`：商店卡牌升级商品。
- `abcdcode_LOGLIKE_MOD/MysteryModel_CardChoice.cs`：卡牌选择与分类 UI。
- `abcdcode_Refactored/LogLikePatches.cs`：Harmony 补丁、楼层音乐等运行时行为。
- `SpecialStaticInfo/StagesXmlInfos/Stage_ch*.xml`：章节地图节点。
- `SpecialStaticInfo/MysteryXmlInfos/RMR_chstart.xml`：初始事件结构。
- `Localize/cn/`、`Localize/en/`、`Localize/kr/`：当前项目实际使用的本地化目录。
- `ArtWork/Shop_CardUpgrade_Icon.png`：商店升级卡牌专用图标。

不要修改 `_release_packages/`、Workshop 或游戏安装目录来代替修改源码。只有用户明确要求部署或打包时，才同步构建产物与资源。

### 2.1 本机源码树与 Workshop 运行树（2026-06-19）

本机实际源码根目录：

```text
D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\
├─ RogueLike Mod Reborn.csproj        # Release 构建入口
├─ RMR_*.cs                           # 模组主流程、奖励、解放战、路由
├─ abcdcode_LOGLIKE_MOD\              # Roguelike 模型、UI、商店、存档
├─ abcdcode_Refactored\               # Harmony 补丁与运行时钩子
├─ AddData\                            # 卡牌、核心书页、敌人、Stage 等游戏数据
├─ SpecialStaticInfo\                 # 节点、事件、奖励池、掉落权重
├─ Localize\{cn,en,kr}\               # 实际加载的本地化
├─ ArtWork\                            # PNG 等图片资源
├─ AssetBundle\ AudioClip\ Spine\     # 若存在则为运行资源
├─ StoryInfo\                          # 剧情资源
├─ RMR_*static_check.ps1              # 静态回归脚本
├─ docs\HANDOFF.md                    # 新会话交接
└─ pack_mod.ps1                       # 从 Workshop 运行树打包，不代表源码已同步
```

Steam Workshop 项目根目录：

```text
E:\Steam\steamapps\workshop\content\1256670\3503523710\
├─ StageModInfo.xml
├─ Data\
├─ Resource\
├─ mod infos\
└─ Assemblies\
   ├─ dlls\                           # 本模组实际运行根目录
   │  ├─ RogueLike Mod Reborn.dll    # 每次 DLL 更新必须覆盖到这里
   │  ├─ AddData\
   │  ├─ SpecialStaticInfo\
   │  ├─ Localize\
   │  ├─ ArtWork\
   │  ├─ AssetBundle\ AudioClip\ Spine\ StoryInfo\
   │  └─ RogueLike Mod Reborn.xml
   ├─ _codex_backups\                 # 部署前 DLL/XML 备份统一放这里
   ├─ 1FrameworkAssemblies\
   └─ 0Harmony.dll 等框架依赖
```

源码到实际运行位置的固定映射：

| 源码/构建产物 | Workshop 实际运行位置 |
|---|---|
| Release `RogueLike Mod Reborn.dll` | `...\Assemblies\dlls\RogueLike Mod Reborn.dll` |
| `AddData\**` | `...\Assemblies\dlls\AddData\**` |
| `SpecialStaticInfo\**` | `...\Assemblies\dlls\SpecialStaticInfo\**` |
| `Localize\**` | `...\Assemblies\dlls\Localize\**` |
| `ArtWork\**` | `...\Assemblies\dlls\ArtWork\**` |
| `AssetBundle\`、`AudioClip\`、`Spine\`、`StoryInfo\` | `...\Assemblies\dlls\` 下同名目录 |

部署规则：

1. 先确认游戏进程 `LibraryOfRuina` 已退出；运行中不得覆盖 DLL。
2. Release 构建优先输出到 `%TEMP%` 的独立 `out/obj`，避免仓库 `bin/obj` ACL 问题。
3. 覆盖前把旧 DLL 和本次涉及的 XML/资源备份到 `...\Assemblies\_codex_backups\`。
4. 复制 DLL 后必须比较构建产物与 `...\Assemblies\dlls\RogueLike Mod Reborn.dll` 的 SHA-256。
5. 修改 XML/TXT/PNG 时必须同步对应的 `Assemblies\dlls` 子路径并逐项比较哈希；只同步 DLL 不足以完成部署。
6. `Assemblies\dlls` 中历史上存在 `.bak` 文件；所有运行时目录扫描必须限制真实扩展名（例如 `GetFiles("*.xml")`），不得把备份文件当配置加载。

运行日志与配置：

- 游戏日志：`C:\Users\13034\AppData\LocalLow\Project Moon\LibraryOfRuina\Player.log`
- 模组配置：`C:\Users\13034\AppData\LocalLow\Project Moon\LibraryOfRuina\ModConfigs\RMR_Config.xml`
- 每次 DLL 更新后，先在 `Player.log` 搜索 `[RMR] RogueLike Mod Reborn initializing. Build:`，确认游戏实际加载了本次 `BuildTimestamp`，再判断后续测试结果。

### 2.2 原作者源码基线与故障回归原则

原作者未经当前项目修改的源码基线位于：

```text
D:\VS_program\ruina-roguelike-reborn-main\original-codes\
```

该目录包含原作者版本的 `RMR_*.cs`、`abcdcode_LOGLIKE_MOD/`、`abcdcode_Refactored/`、`AddData/`、`SpecialStaticInfo/`、`Localize/`、项目文件及依赖结构。它的用途是**只读对照和严重回归时的恢复基线**，不是当前工作目录，也不能从这里直接构建或部署。

当后续修改出现启动崩溃、奖励流程死循环、主流程黑屏、Stage 路由整体错乱等大问题时：

1. 先停止继续叠加补丁，保存当前 `git status`、相关 diff、`Player.log` 和已部署 DLL 哈希。
2. 按故障调用链逐文件比较当前源码与 `original-codes` 中的同名文件，先确认原作者的生命周期、队列推进和数据加载方式。
3. 优先让出问题的函数或模块回归原作者实现，再以最小补丁重新加入已经确认必须保留的新玩法规则。
4. 不得直接把整个 `original-codes` 覆盖到当前仓库；当前仓库已有解放战、永久图鉴、第七章、商店升级和本地化等新增功能，整仓覆盖会造成更大范围的数据丢失。
5. 回归后必须重新运行相关静态脚本、Release 构建、XML 解析和 `git diff --check`，再部署到 Workshop。
6. `original-codes` 与当前源码的差异只用于判断原作者行为，不代表旧实现自动满足当前 `AGENTS.md` 的最终玩法规则；若两者冲突，以用户当前明确要求为准。

## 3. 不可回退的玩法规则

### 3.1 章节与路线

- 第一章和第二章不得生成异想体战斗节点；原异想体节点应替换为 `Rest` 休息室。
- 每章开场动画必须按当前 `ChapterGrade` 播放，禁止硬编码为第一章“传闻”。
- 玩家选择的楼层应持续决定后续幕的接待音乐；第二幕及以后不得自动回退到总类层音乐。
- 普通战斗和商店的异想体书页池采用向下兼容：当前章节可以出现本章及更早章节的普通异想体书页，但仍需执行去重、未拥有和解放战门控。
- 都市传说、都市恶疾的 `Normal` 战斗异想体书页掉落率目标为约 50%；精英战和异想体战不使用该降率规则。
- 调试或章节直达入口必须同步设置章节等级、章节步数和章节起点状态，不能只生成一次目标章节节点后又按传闻继续。

### 3.2 永久图鉴与当前路线

- 角色书页和战斗书页属于永久图鉴内容；路线加载、玩家当前装备的核心书页和当前牌组都应同步到永久记录，兼容旧存档迁移。
- 普通异想体书页只属于当前 Roguelike 路线，不写入永久图鉴，新路线不应继承。
- 只有解放战胜利后的专属异想体书页和 E.G.O. 书页写入永久图鉴/永久奖励池。
- 永久图鉴、当前路线和解放战临时配置必须分离，不能因为一次解放战污染当前路线或让普通异想体永久化。

### 3.3 解放战入口与战斗配置

- 解放战唯一入口是模组开始后第一个“初始遗物选择事件”。
- 入口是一次性的：选择普通初始遗物或正式开始解放战后立即关闭；其他事件、章节、按钮或直接方法调用均不得绕过该限制。
- 楼层选择必须连接到《废墟图书馆》原版楼层解放战 Stage，而不是普通异想体战或模组自制替代战。
- 原版 Stage ID 必须使用原版包语义，不得错误附加模组 WorkshopId：

| 楼层 | Sephirah | 原版 Stage ID |
|---|---|---:|
| 历史层 | Malkuth | 201005 |
| 科技层 | Yesod | 202005 |
| 文学层 | Hod | 203005 |
| 艺术层 | Netzach | 204005 |
| 自然层 | Tiphereth | 205005 |
| 语言层 | Gebura | 206005 |
| 社会层 | Chesed | 207005 |
| 哲学层 | Binah | 208004 |
| 宗教层 | Hokma | 209004 |
| 总类层 | Keter | 210009 |

- 进入解放战前，临时把可使用内容限制为永久图鉴已解锁的角色书页和战斗书页。
- 必须保存当前路线配置；解放战结束、失败、中途退出或异常时都要恢复，避免污染正常路线。
- 点击楼层后必须真正结束当前初始事件并转入目标 Stage，不能只关闭面板、设置变量或停留在事件界面。

### 3.4 解放战奖励门控

- 解放战胜利记录对应楼层完成状态，并永久开放该层专属异想体书页和 E.G.O. 奖励。
- 奖励不是未通关前直接出现，也不是把整层全部普通异想体页永久赠送。
- 完成后，专属奖励才可进入后续商店或战斗结算的随机候选池；仍需去重、未拥有和升级版过滤。
- 已知 E.G.O. 奖励 ID 段：

| 楼层 | E.G.O. ID |
|---|---|
| 历史层 | 910001–910005 |
| 科技层 | 910011–910015 |
| 文学层 | 910016–910020 |
| 艺术层 | 910021–910025 |
| 自然层 | 910026–910030 |
| 语言层 | 910031–910035 |
| 社会层 | 910036–910040 |
| 哲学层 | 910041–910045 |
| 宗教层 | 910046–910050 |
| 总类层 | 910086–910090 |

- 已知专属脚本根应保持与当前源码映射一致：
  - 历史层：`snowwhite`
  - 科技层：`SingingMachine1`（音乐）、`Butterfly3`（棺柩）、`freischutz3`（黑焰）
  - 文学层：`blackswan`
  - 艺术层：`orchestra`
  - 自然层：`clownofnihil`
  - 语言层：`nothing`
  - 社会层：`wizard`
  - 哲学层：`bossbird`
  - 宗教层：`onebadmanygood`、`plaguedoctor`、`whitenight`
  - 总类层：`quietKidHammer`、`quietKidEyeShine`、`quietKidGuilty`
- 匹配脚本根时必须兼容数字后缀和无数字后缀，尤其不能漏掉 `quietKid*`。
- 奖励映射的最终事实应以当前代码、游戏 XML 和静态检查交叉验证，不得仅凭本文件重新猜测或重写。

## 4. 商店与卡牌升级规则

- 战斗书页商品代表“获得一种书页”，不显示库存数量，也不按 `count` 递减。
- 商店必须分别布局角色/核心书页、战斗书页、被动遗物、异想体书页、E.G.O. 书页和卡牌升级入口；可以紧凑，但不得集中挤在中心或相互重叠。
- 核心书页商品常驻数量为 2，异想体书页商品常驻数量为 2；若候选不足，应安全降级，不能生成空白商品或抛出异常。
- 未完成对应楼层解放战时，专属异想体书页和 E.G.O. 书页不得进入商店。
- 商店卡牌升级初始价格为 20，每次成功升级后价格增加 5，并在当前路线存档中持久化。
- 升级只能选择永久图鉴/当前规则允许且存在有效升级版的战斗书页；成功后删除旧版并加入升级版。
- 商店和休息处的升级选择都按章节分类：传闻、都市怪谈、都市传说、都市恶疾、都市梦魇、都市之星、杂质。
- 商店升级入口使用 `ArtWork/Shop_CardUpgrade_Icon.png`；休息处升级图标保持原状。
- 不使用 Unicode/Emoji 字符充当最终 UI 图标，也不要重新加回图标左上数字或右上武器标记。

## 5. UI 与本地化

- 解放战入口、标题、说明、楼层按钮、状态文字、卡牌升级名称和说明必须使用项目实际加载的本地化机制。
- 当前仓库主要维护 `cn`、`en`、`kr`。不要把 `kr` 当成日文；若用户要求日文，先确认游戏实际使用的 locale 目录或语言键，再新增。
- 不允许因缺失键而显示空白；可提供安全 fallback，但不能用硬编码英文覆盖中文/韩文界面。
- 中文升级书页描述中不得残留 `On Use`、`Start of Clash`、`Unity` 等未本地化英文触发词，除非它们是项目明确保留的专有名词。
- 修改 XML、本地化或含中文的 PowerShell/C# 文件时必须显式使用 UTF-8，避免默认编码造成乱码或损坏。
- 若需要稳定地在 C# 中保存中文 UI 名称，可使用 `\uXXXX` 转义；不要把终端显示乱码误判为文件本身损坏。

## 6. 编码与修改原则

- C# 延续现有项目风格：4 空格缩进，类型和公开成员使用 PascalCase，局部变量使用 camelCase。
- 优先修复根因和统一数据源，不用零散坐标、魔法偏移、重复条件或临时 fallback 掩盖问题。
- 不进行与当前任务无关的大规模重构，不批量格式化整个文件。
- 保持存档向后兼容；新增字段必须有默认值、旧存档迁移和损坏数据回退。
- 对空集合使用正确的短路条件，避免类似 `value == null && value.Count == 0` 的空引用风险。
- 高频 Harmony 反射和 XML 反序列化可考虑缓存，但应作为独立低风险任务，不与功能修复混在一起。
- 调试日志必须受配置开关控制；不要在 `Update()` 等高频路径无条件输出。
- 权限阻止写入时应明确说明并申请权限；若当前环境确实无法修改，提供可直接执行的精确补丁或外部代理提示词，并在用户应用后重新验证，不能声称已经落地。

## 7. 验证流程

修改前先检查现有脚本；修改相关功能时运行直接相关脚本以及基础回归脚本。常见脚本包括：

```text
RMR_0614_all_floor_realization_static_check.ps1
RMR_0614_downward_abno_pool_static_check.ps1
RMR_0614_realization_reward_static_check.ps1
RMR_0614_shop_upgrade_static_check.ps1
RMR_0614_ui_atlas_persistence_static_check.ps1
RMR_0615_upgrade_atlas_localization_static_check.ps1
RMR_0615_runtime_regression_static_check.ps1
RMR_0616_grade6_special_core_pages_static_check.ps1
RMR_0616_high_chapter_reward_static_check.ps1
RMR_0617_progression_realization_static_check.ps1
RMR_0617_realization_session_static_check.ps1
RMR_0618_codex_review_static_check.ps1
RMR_0618_reward_event_atlas_static_check.ps1
RMR_0618_reward_queue_static_check.ps1
RMR_0618_startup_crash_static_check.ps1
RMR_0619_card_reward_empty_choice_static_check.ps1
RMR_0619_debug_chapter_start_static_check.ps1
RMR_0619_equip_page_loader_static_check.ps1
RMR_0619_mystery_start_event_static_check.ps1
RMR_0619_reward_overlay_and_boss_realization_static_check.ps1
RMR_0619_reward_pool_route_regression_static_check.ps1
RMR_0620_binah_red_mist_progression_static_check.ps1
RMR_0620_binah_route_ego_toggle_grade7_static_check.ps1
RMR_0620_ego_realization_two_pick_static_check.ps1
RMR_0620_grade6_special_fixed_deck_static_check.ps1
RMR_0620_red_mist_challenge_static_check.ps1
RMR_0620_runtime_reward_ego_shop_static_check.ps1
RMR_0621_followup_runtime_regressions_static_check.ps1
RMR_0621_reported_runtime_regressions_static_check.ps1
RMR_abnormality_battle_static_check.ps1
RMR_abnormality_unlock_static_check.ps1
RMR_atlas_static_check.ps1
RMR_atlas_unlock_static_check.ps1
RMR_atlas_upgrade_static_check.ps1
RMR_chstart_localization_static_check.ps1
RMR_issue0613_static_check.ps1
RMR_realization_static_check.ps1
RMR_realization_unlock_static_check.ps1
RMR_release_static_check.ps1
RMR_shop_localization_static_check.ps1
RMR_stage_density_static_check.ps1
```

静态脚本应验证行为约束，而不是依赖容易变化的固定源码文本。脚本失败时先判断是真回归、旧断言还是脚本编码错误，不要为了让过时脚本变绿而回退正确设计。

### Release 构建

若仓库内 `bin/obj` 因 ACL 无法写入，使用仓库外临时目录：

```powershell
$out = Join-Path $env:TEMP 'rmr_build_out'
$obj = Join-Path $env:TEMP 'rmr_obj_out'
New-Item -ItemType Directory -Force -Path $out, $obj | Out-Null

dotnet msbuild '.\RogueLike Mod Reborn.csproj' `
  /p:Configuration=Release `
  /p:OutputPath="$out\" `
  /p:BaseIntermediateOutputPath="$obj\"
```

### XML 与差异检查

- 修改 XML 后使用 `[xml](Get-Content -Raw -Encoding UTF8 ...)` 逐个解析。
- 执行 `git diff --check`。
- 人工检查 `git diff --stat` 和实际 diff，确认没有编码污染、字面 `` `r`n ``、意外 BOM/换行变化或无关文件修改。
- 构建产物与部署目录可用 SHA-256 核对，但哈希只证明文件一致，不证明游戏内逻辑正确。

## 8. 必须区分的验证等级

汇报时必须明确区分：

1. **源码检查通过**：仅确认代码结构或规则存在。
2. **静态脚本通过**：仅确认脚本覆盖的约束。
3. **Release 编译通过**：仅确认可编译。
4. **部署核对通过**：仅确认 DLL/XML/图片已复制且哈希一致。
5. **游戏内实测通过**：必须真实启动游戏并完成对应点击、战斗或存档流程。

没有启动游戏时，禁止使用“已经彻底修好”“游戏内验证无误”等表述。

游戏内重点复验项：

- 初始遗物事件中解放战入口及多语言文字是否正常。
- 点击十个楼层是否进入正确原版解放战。
- 解放战是否只使用永久图鉴内容，结束或异常退出后路线配置是否恢复。
- 解放前后专属异想体页/E.G.O. 是否正确门控并跨路线永久保存。
- 普通异想体页是否只保留在当前路线。
- 图鉴是否包含初始核心书页、当前装备和当前牌组。
- 商店各商品在不同分辨率下是否不重叠，核心页和异想体页是否各常驻 2 个。
- 商店升级价格、章节分类、图标和存档是否正确。
- 第二幕及以后楼层音乐是否保持，章节过场是否匹配当前章节。

## 9. 完成任务时的输出

完成后用中文说明：

- 根因和采用的修复方式。
- 实际修改的文件。
- 运行过的静态脚本、构建和解析检查及其真实结果。
- 是否执行部署、部署到哪里、是否核对哈希。
- 尚未进行的游戏内测试和残余风险。

只有当新的用户决定改变了长期玩法规则时，才更新本 `AGENTS.md`；普通代码修改不要把临时进度、机器路径、DLL 哈希或会话过程不断追加到本文件。

## 10. 当前交接快照（2026-06-19，后续验收完成后应删除或更新）

### 10.1 交接材料的含义

- `D:\sketch.txt` 当前是 DeepSeek 对 2026-06-18 至 2026-06-19 工作的**完工总结**，不是待执行提示词；除非用户明确要求，不要覆盖、追加或把它当作新的修改指令。
- 总结中的 `PASS`、编译成功、已部署等均是外部代理声明。新会话必须重新读取实际源码、脚本输出和运行目录，不能直接据此声称问题已修复。
- 当前工作树包含大量已有修改和未跟踪文件。不要执行清理、回滚、批量格式化或覆盖用户改动；只核验当前问题涉及的文件。

### 10.2 DeepSeek 声称完成的重点工作

- 启动稳定性：购买/领取流程改为先验证再修改状态；卡图反射加载加固；核心书页加载器限制为 `*.xml`，避免 `.bak` 导致 ID `250001` 重复。
- 战斗奖励：调整普通战斗和 Boss 奖励队列、跳过按钮、旧选择 UI 销毁顺序、DropBook 去重、空战斗书页候选处理。
- 解放战奖励：结算前加载解放战进度，按已完成楼层动态生成专属异想体书页与 E.G.O. 候选。
- 事件与调试入口：事件章节判定改为使用当前进度；Debug 第六章入口不再被 `curstage=855` 覆盖，并同步章节起始 Stage。
- 图鉴：补充卡面加载，并采用 C 方案的等比纵向缩略图。
- 初始事件：为 `MysteryBase.SwapFrame`、`FrameObj` 和选项列表增加空值防护。
- 外部总结声称 Release 编译零错误、13 个静态脚本通过，并已同步部分 DLL/XML/TXT/PNG 到 Workshop；这些状态仍需重新验证。

### 10.3 下一会话必须优先验收

1. 对照 `D:\sketch.txt`，逐项检查实际 diff 和完整调用链，重点文件为：
   - `abcdcode_LOGLIKE_MOD/RewardingModel.cs`
   - `abcdcode_LOGLIKE_MOD/MysteryModel_CardReward.cs`
   - `abcdcode_LOGLIKE_MOD/MysteryModel_Mystery_Ch1_1.cs`
   - `abcdcode_LOGLIKE_MOD/MysteryBase.cs`
   - `abcdcode_LOGLIKE_MOD/LogAtlasPanel.cs`
   - `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
   - `abcdcode_Refactored/LogLikePatches.cs`
   - `RMR_AbnormalityUnlocks.cs`
   - `RMR_Core.cs`
2. 重新运行与上述改动直接相关的 13 个静态脚本；记录真实输出，不沿用总结中的 `PASS`。
3. 重新执行 Release 构建、`git diff --check`，并检查实际 diff 是否存在编码污染、过宽修改或失效断言。
4. 用户要求部署时，再核对源码构建产物与 Workshop 中实际运行的 DLL、XML、TXT、PNG 哈希；不得只核对 DLL。
5. 游戏内按顺序复验：
   - 普通战斗选择战斗书页后，不再重复出现战斗书页或覆盖核心书页选择。
   - Boss 战结算不出现空选项，跳过可推进；已完成解放战的专属异想体书页和 E.G.O. 奖励进入候选池。
   - Debug 第六章从休息室跳过后不黑屏，章节等级、步数和起点保持第六章状态。
   - 图鉴卡面可见且比例正常。
   - 启动时不再出现核心书页 ID `250001` 重复报错。

在完成上述游戏内实测前，只能报告源码、静态脚本、编译或部署核对等级，不得将 DeepSeek 总结升级为“游戏内已修复”。

## 11. Git 同步要求

每次修改完之后 git init 一下然后 git push，这个文件夹已经绑定了用户 GitHub 的一个仓库。
