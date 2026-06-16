# LoR-RMR Codex 项目默认指令

本文件是 **Library of Ruina — Roguelike Mod Reborn（LoR-RMR）** 仓库的长期项目提示词。Codex Desktop 打开本仓库后应优先遵守本文件；不要反复执行 `/init` 覆盖它。

> 本文件记录的是最终设计约束和工作方法，不保证当前工作树已经完整实现。开始任何任务前，必须以当前源码、Git 状态、静态检查和构建结果重新核验。

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
RMR_issue0613_static_check.ps1
RMR_realization_static_check.ps1
RMR_realization_unlock_static_check.ps1
RMR_0614_realization_reward_static_check.ps1
RMR_0614_all_floor_realization_static_check.ps1
RMR_0614_ui_atlas_persistence_static_check.ps1
RMR_0614_downward_abno_pool_static_check.ps1
RMR_0614_shop_upgrade_static_check.ps1
RMR_0615_upgrade_atlas_localization_static_check.ps1
RMR_0615_runtime_regression_static_check.ps1
RMR_abnormality_unlock_static_check.ps1
RMR_chstart_localization_static_check.ps1
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
