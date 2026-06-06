# Handoff

## 当前问题
- 图鉴里的`战斗书页`页签还没有“是否显示升级后版本”的切换框。
- 现在查看战斗书页图鉴时，只能看到当前默认展示的数据，没法直接在图鉴里切到升级后的版本来对比数值。
- 需求目标是：在图鉴的`战斗书页`栏左下角加一个选项框，勾选后显示升级版，方便查看单张战斗书页升级后的数值。

## 已验证内容
- 图鉴主界面已经接入，入口在[abcdcode_Refactored/LogLikePatches.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_Refactored/LogLikePatches.cs) 的战斗设置按钮注入逻辑里，`ui_AtlasTab` 已经可以打开图鉴面板。
- 图鉴数据源已经在[abcdcode_LOGLIKE_MOD/LogAtlasPanel.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_LOGLIKE_MOD/LogAtlasPanel.cs) 里整理好了，`BuildEntries()` 会汇总角色书页、战斗书页、异想体书页、EGO 书页。
- 战斗书页的升级解析能力已经存在，`[abcdcode_LOGLIKE_MOD/LogCardUpgradeManager.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_LOGLIKE_MOD/LogCardUpgradeManager.cs)` 里已经有 `GetUpgradeCard(...)` 和 `GetAllUpgradesCard(...)`。
- 图鉴解锁持久化已经修过，`[abcdcode_LOGLIKE_MOD/LogueBookModels.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_LOGLIKE_MOD/LogueBookModels.cs)` 里现在有 `AtlasUnlockedRoleBooks` 和 `AtlasUnlockedBattleCards`，并且 `AddCard(...)`、`AddBook(...)` 会记录图鉴解锁。
- 图鉴按钮本体已经在战斗设置界面里挂好了，相关 UI 复用逻辑在[abcdcode_Refactored/LogLikePatches.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_Refactored/LogLikePatches.cs) 的 `UIBattleSettingEditPanel_Open`、`UIBattleSettingEditPanel_SetBUttonState`。

## 怀疑根因
- 问题主要不在升级数据层，而在图鉴展示层。
- 目前 `LogAtlasPanel.BuildBattleCardEntries()` 直接枚举 `ItemXmlDataList.instance` 里的 `DiceCardXmlInfo`，没有一个“展示升级版”的开关状态。
- 也就是说，升级器已经能算出升级后的卡，但图鉴没把这个能力接进来，导致 UI 层和数据层脱节。
- 目前也没有为这个展示状态做持久化或分类切换，所以刷新图鉴时始终只按当前默认逻辑渲染。

## 修改过的位置
- [abcdcode_LOGLIKE_MOD/LogAtlasPanel.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_LOGLIKE_MOD/LogAtlasPanel.cs)
  - 图鉴面板主体、分类按钮、条目构建、战斗书页条目来源都在这里。
  - `BuildBattleCardEntries()` 是最需要接入“升级版显示”开关的地方。
- [abcdcode_LOGLIKE_MOD/LogCardUpgradeManager.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_LOGLIKE_MOD/LogCardUpgradeManager.cs)
  - 升级解析入口已经可用。
  - 这里不用大改，主要是给图鉴展示调用。
- [abcdcode_LOGLIKE_MOD/LogueBookModels.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_LOGLIKE_MOD/LogueBookModels.cs)
  - 图鉴解锁持久化、战斗书页记录、角色书页记录都已经在这里。
  - 当前任务不需要重做解锁逻辑，但后续如果要把“显示升级版”做成记忆项或配置项，可以从这里接。
- [abcdcode_Refactored/LogLikePatches.cs](D:/VS_program/ruina-roguelike-reborn-main/ruina-roguelike-reborn-main/abcdcode_Refactored/LogLikePatches.cs)
  - 战斗设置界面按钮注入在这里。
  - 如果图鉴左下角的选项框需要依附于战斗设置 UI 的布局，这里可能还要补一个 UI 挂钩或显隐控制。

## 下一步调试方案
1. 在 `LogAtlasPanel` 里加一个仅对`BattleCard`页签生效的布尔开关，例如 `showUpgradedBattleCards`。
2. 在图鉴面板左下角加一个选项框或切换按钮，切换后只影响战斗书页展示，不影响角色书页、异想体书页和 EGO 书页。
3. 接入升级解析：
   - 关掉开关时，继续显示基础版战斗书页。
   - 打开开关时，用 `LogCardUpgradeManager.GetUpgradeCard(...)` 或 `GetAllUpgradesCard(...)` 提供的升级版数据来构建展示项。
4. 明确展示策略：
   - 推荐先只展示“下一阶段升级版”或“当前对应升级版”，避免一次把所有升级分支都堆进图鉴。
   - 如果一张卡有多个升级层，先确认是要看“第一层升级”还是“最高层升级”，再落实现。
5. 做一张可升级的战斗书页回归测试：
   - 进入图鉴的`战斗书页`页签。
   - 切换开关后确认标题、数值、描述、立绘/图标是否一起变化。
   - 再确认切回基础版后内容恢复。
6. 如果 UI 空间不够，再回头调整图鉴左下角按钮位置，避免和保存/载入卡组按钮挤在一起。

## 备注
- 这次问题不是“升级系统坏了”，而是“图鉴展示没有接上升级系统”。
- 如果只看代码风险，最容易出错的点是展示用的 `LorId`、升级后的 `DiceCardXmlInfo`、以及解锁判定是否要继续按基础卡片 ID 判断。
