验证结束，问题图片在D:/LoR_mods下面，现在单独描述

1. ![PowerToys_Paste_20260703003648](D:\LoR_mods\PowerToys_Paste_20260703003648.png)

存在大部分战斗书页描述出现口口口，包括司书使用的核心书页也有这个情况，是下午修改之后出现的

2. ![PowerToys_Paste_20260703003701](D:\LoR_mods\PowerToys_Paste_20260703003701.png)

解放战可选角色依然只有三人

3. ![PowerToys_Paste_20260703003914](D:\LoR_mods\PowerToys_Paste_20260703003914.png)

原本正常能有角色图片的核心书页选择部分出现了空白，这个问题在商店展示中没有出现

![PowerToys_Paste_20260703004019](D:\LoR_mods\PowerToys_Paste_20260703004019.png)

4. ![PowerToys_Paste_20260703004128](D:\LoR_mods\PowerToys_Paste_20260703004128.png)

阿尔加利亚的战斗书页不对

我发的![image-20260703005218715](C:\Users\13034\AppData\Roaming\Typora\typora-user-images\image-20260703005218715.png)才是对的

5. 杂质部分掉落的核心书页依然是Mod Needed，而不是从原版杂质关卡部分进行选择，比如Hana协会，或者扭曲残响乐团的核心书页以及战斗书页，boss战前会有艺术级书页选择，那个选择是杂质池子里的书页，虽然说他的名称显示也有口口口问题就是了

---

## 2026-07-03 本轮修复记录

### sub-agent / 分模块结论

- realization_capacity_agent：完成只读调查。结论是解放战人数上限没有命中真实字段；原版 `StageWaveInfo` 字段为 `availableNumber`，运行态 `StageWaveModel` 字段为 `_availableUnitNumber`，旧代码只尝试写 `availableUnit/_availableUnit/AvailableUnit`，因此准备界面仍显示 `3/3`。
- reward_origin_agent / argalia_deck_agent / impurity_drop_agent：后台 explorer 会话被系统回收，等待时返回 `not_found`。主 Agent 按同样模块继续调查并修复，未使用它们修改文件。

### 已修改文件

- `RMR_RealizationManager.cs`
  - 修复解放战准备阶段可用人数：进入原版解放战后同时写 `StageWaveInfo.availableNumber` 和 `StageWaveModel._availableUnitNumber` 为 5。
- `abcdcode_LOGLIKE_MOD/RewardingModel.cs`
  - 新增原版 ID 兼容查询：`@origin` / 空包 / 纯整数 ID 互相兜底。
  - 核心页奖励改用本地化书名，不再直接使用 `BookXmlInfo.InnerName`。
  - 被动名、核心页奖励、战斗书页掉落和三选一战斗书页池都接入原版 ID 兜底。
  - 增加空掉落池保护，避免候选全空时随机空列表导致结算异常。
- `abcdcode_Refactored/LogLikePatches.cs`
  - 核心页奖励卡 UI 识别 `EquipPage` 奖励后改用 `BookModel.GetThumbSprite()`，修复奖励选择界面核心页白图。
- `abcdcode_LOGLIKE_MOD/ShopGoods_Passive.cs`
  - 商店/奖励说明用原版 ID 兼容查询和本地化书名。
  - 原版核心页商品图标缺失时用核心页缩略图兜底。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-03T01:31+08:00`。
  - 苍蓝残响自动解锁战斗书页从旧模组页 `604021-604025` 改为原版页 `704001, 704011, 704012, 704013, 704014, 705011, 705012`。
- `RMR_AbnormalityUnlocks.cs`
  - 扭曲残响乐团胜利奖励同步改为发放上述原版苍蓝残响战斗书页。
- `AddData/EquipPage/EquipPage_Librarian_ch6.xml`
  - 苍蓝残响核心页 `OnlyCard` 改为上述 7 张原版战斗书页。
- `abcdcode_LOGLIKE_MOD/LogueBookModels.cs`
  - 装备核心页加载 `OnlyCard` 时增加原版卡牌兜底，避免模组核心页绑定原版专属页时加载失败。
- `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - 静态检查期望从旧 `604021-604025` 更新为新的原版苍蓝残响战斗书页组合。

### 已验证

- XML 解析通过：
  - `AddData/EquipPage/EquipPage_Librarian_ch6.xml`
  - `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml`
  - `AddData/CardDropTable/CardDropTable_ch7.xml`
  - `SpecialStaticInfo/DropValueXmlInfos/values_ch7.txt`
- 静态检查通过：
  - `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - `tools/static_checks/realization/RMR_0617_realization_session_static_check.ps1`
- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out_0703\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - XML：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\AddData\EquipPage\EquipPage_Librarian_ch6.xml`
  - 备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\0703_reward_realization_20260703_081047`
  - DLL SHA-256：`2B548EC68098A4B508A72230617D3BD7B35263911FCD5147847738E369DF7761`
  - XML SHA-256：`472D80673782D7D7AB330410E6522C4FBAD5D2DCC5182ED2956259C212CA92F7`

### 还没做 / 下一次优先验证

- 尚未启动游戏进行游戏内实测。需要进入游戏后在 `Player.log` 确认加载到 `Build: 2026-07-03T01:31+08:00`。
- 游戏内需要重点验证：
  - 解放战准备界面是否从 `3/3` 变为可选 5 人。
  - 杂质核心页奖励是否不再显示 `Mod Needed`，并能显示原版残响乐团核心页名称和说明。
  - 杂质战斗书页奖励是否出现 `704001-704018` 池内原版战斗书页。
  - 核心页奖励三选一是否不再白图。
  - 苍蓝残响核心页装备后是否显示并使用 `704001, 704011, 704012, 704013, 704014, 705011, 705012`。
  - 若普通战斗书页描述仍出现口口口，需要继续查 `BattleCardDescXmlList` / `CardInfo` 本地化加载链；本轮主要修的是核心页奖励和原版 ID 解析链。

---

## 2026-07-03 第二轮修复记录

### sub-agent / 分模块结论

- impurity_core_reward_agent：完成只读调查。`260005-260014` 本身是原版杂质/残响乐团核心页，但第七章没有进入额外核心页奖励通道；`@origin` 解析在展示、商店候选、领取入库之间不统一。若要覆盖用户说的 Hana 协会，需要把 `260001-260004` 加入第七章核心页池。
- localization_box_agent：完成只读调查。`Player.log` 显示加载 `Localize\cn` 和 CJK TMP 字体，但部分复用原版 UI 只写 `.text` 没换 TMP 字体；部分原版数据本身仍含韩文名称。建议加递归 TMP 字体兜底和少量已知文本替换。
- argalia_special_page_agent：完成只读调查。阿尔加利亚战斗书页已被授予，但 `PruneCorePageExclusiveBattleCardsFromInventoryAndAtlas()` 会删除核心页 OnlyCard；固定牌组识别也没包含 Blue Reverberation，因此装备后仍会回落默认牌组。原版阿尔加利亚核心页 `OnlyCard` 是 `704001,704011,704012,704013,704014,705010,705011`。

### 已修改文件

- `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml`
  - 第七章核心页奖励池加入 `260001-260004` Hana 协会/协会线核心页，并保留 `260005-260014` 残响乐团核心页。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-03T08:56+08:00`。
  - 苍蓝残响自动解锁战斗书页改为原版阿尔加利亚核心页口径：`704001,704011,704012,704013,704014,705010,705011`。
  - 新增 Blue Reverberation 核心页识别，并给 tooltip 套用 RMR TMP 字体。
- `RMR_AbnormalityUnlocks.cs`
  - 扭曲残响乐团胜利奖励同步改为上述 7 张阿尔加利亚专属战斗书页。
- `AddData/EquipPage/EquipPage_Librarian_ch6.xml`
  - 苍蓝残响核心页 `OnlyCard` 从 `705012` 改为原版 `705010`。
- `abcdcode_LOGLIKE_MOD/LogueBookModels.cs`
  - 专属卡 prune 增加阿尔加利亚白名单，避免永久图鉴/库存里的专属战斗书页被删。
  - Blue Reverberation 进入特殊固定牌组识别，装备后按 OnlyCard 生成专属牌组。
  - 核心页入库、永久图鉴清理、已有核心页判断改用 origin-aware 路径。

---

## 2026-07-04 本轮修复记录

### 已调查结论

- 阿尔加利亚之页仍然不对的主要原因是玩家侧 `AddData/EquipPage/EquipPage_Librarian_ch6.xml` 注册了本模组包内的 `Book ID="250013"`，与原版阿尔加利亚之页 ID 冲突，奖励/图鉴会解析到这个错误包装页。
- 解放战 5 人准备后进战斗只有第一个司书有牌组，是因为 `RMR_RealizationManager.ApplyAtlasOnlyLoadout()` 先调用 `EquipNewPage(UnitDataModel, ...)`，后调用 `CreatePlayerBattle()`；而 `EquipNewPage(UnitDataModel, ...)` 依赖已经存在的 `playerBattleModel`，因此装备和填牌组静默失败。
- 原版异想体书页描述启动后出现口口口、切换语言后恢复，原因更接近原版 `LocalizedTextLoader.LoadOthers(language)` 中的异想体文本表未在模组启动后重新刷新，而不是 `Localize/cn` 文件整体编码损坏。

### 已修改文件

- `AddData/EquipPage/EquipPage_Librarian_ch6.xml`
  - 删除玩家侧本模组 `Book ID="250013"` 阿尔加利亚包装页，改为只通过原版 `new LorId(250013)` 使用原版阿尔加利亚之页。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-04T16:30+08:00`。
  - 增加旧本模组阿尔加利亚 ID 识别和归一化，旧存档里的 `abcdcodecalmmagma.LogueLikeReborn:250013` 会迁移为原版 `250013`。
- `abcdcode_LOGLIKE_MOD/LogueBookModels.cs`
  - 读取核心页存档、读取永久图鉴、核心页入库时归一化旧阿尔加利亚 ID。
  - 明确排除 Blue Reverberation 进入 Grade6 特殊固定牌组逻辑，旧固定牌组来源也会归一化后清理，避免阿尔加利亚继续被锁牌组。
- `RMR_RealizationManager.cs`
  - 解放战临时 5 人队伍先 `CreatePlayerBattle()` 并标记 `IsAddedBattle=true`，再逐个装备图鉴核心页和填充牌组，避免 2-5 号司书空牌组。
- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - 启动时在模组本地化加载后调用原版 `LoadAbnormalityCardDescriptions()` 和 `LoadAbnormalityAbilityDescription()`，模拟切语言后的原版异想体文本刷新。
- `tools/static_checks/realization/RMR_0617_realization_session_static_check.ps1`
  - 增加解放战配队顺序检查和玩家侧不再注册本地 `250013` 检查。
- `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - 增加 Blue Reverberation 显式排除固定牌组和旧来源归一化检查。
- `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - 增加启动刷新原版异想体文本表检查。

### 已验证

- XML 解析通过：
  - `AddData/EquipPage/EquipPage_Librarian_ch6.xml`
  - `Localize/cn/UIs.txt`
- 静态检查通过：
  - `tools/static_checks/realization/RMR_0617_realization_session_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out_0704_1630\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - XML：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\AddData\EquipPage\EquipPage_Librarian_ch6.xml`
  - 备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\0704_argalia_realization_language_20260704_163203`
  - DLL SHA-256：`817DACF50ED9CC729850DAB1E79913768D8DA468351E42E1BB37A72F51F27B1F`
  - XML SHA-256：`0968B5C8EC6C94DC489174FF94EB38DA88B9E47E79E2034FE0E520FCFD43EB43`

### 还没做 / 下一次优先验证

- 尚未启动游戏进行游戏内实测。进入游戏后先在 `Player.log` 确认加载到 `Build: 2026-07-04T16:30+08:00`。
- 游戏内重点验证：
  - 阿尔加利亚核心页是否显示为原版阿尔加利亚之页，而不是本模组包装页或都市之星敌对版本。
  - 阿尔加利亚战斗书页是否可编辑，且不再被固定牌组逻辑锁死。
  - 解放战准备配置 5 人后，进入战斗 5 名司书是否都有有效牌组。
  - 启动后不切语言时，原版异想体书页名称和描述是否仍出现口口口。
  - 若仍有战斗书页/核心书页口口口，需要继续查 `BattleCardDescXmlList` / `BookDesc` / 对应 UI TMP 字体，而不是只查 `Localize/cn` 文件编码。
- `abcdcode_LOGLIKE_MOD/RewardingModel.cs`
  - 增加已知杂质核心页中文名兜底和残响/Hana 韩文片段替换。
  - 核心页、被动、奖励说明输出统一经过 `SanitizeDisplayText()`。
- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - 战斗书页目录/详情卡名与能力描述套用 `SanitizeDisplayText()` 和 RMR TMP 字体。
- `abcdcode_Refactored/LogLikePatches.cs`
  - 新增 `ApplyRmrTmpFont()`；异想体页 UI、奖励页 UI、文本覆盖、被动名/描述走字体和文本兜底。
  - 第七章也进入额外核心页奖励通道。
- `abcdcode_LOGLIKE_MOD/PickUpModel_EquipDefault.cs`
  - 核心页奖励领取入库改用 origin-aware 解析。
- `abcdcode_LOGLIKE_MOD/ShopBase.cs`
  - 商店核心页候选改用 origin-aware 解析和拥有过滤。
- `abcdcode_LOGLIKE_MOD/LogAtlasPanel.cs`
  - 图鉴核心页详情名和被动描述改用本地化/文本兜底。
- `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - 更新苍蓝残响专属战斗书页期望。
- `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
  - 第七章核心页奖励池期望改为 `260001-260014`。

### 已验证

- XML 解析通过：
  - `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml`
  - `AddData/EquipPage/EquipPage_Librarian_ch6.xml`
- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out_0703_0856\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 静态检查通过：
  - `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - XML：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\AddData\EquipPage\EquipPage_Librarian_ch6.xml`
  - XML：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\SpecialStaticInfo\RewardPassiveInfos\EquipReward_ch7.xml`
  - 备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\0703_argalia_impurity_text_20260703_085621`
  - DLL 二次部署备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\0703_argalia_impurity_text_dll_redeploy_20260703_085748`
  - DLL SHA-256：`4C385DC1354CE3A7CD406FCEF46D125C096971D9ACED79160A39CD4D16321079`
  - `EquipPage_Librarian_ch6.xml` SHA-256：`98266F467615FBC8513B808E446487D5C82B72EA74C3BCF8B4F13BB9241C9F00`
  - `EquipReward_ch7.xml` SHA-256：`5A43568CEC74AC7AF6E1D438A2A841D8A6C07AAAF577AFD9B225BAF19EA930D2`

### 还没做 / 下一次优先验证

- 尚未完成部署后的游戏内实测。部署后需要在 `Player.log` 确认加载到 `Build: 2026-07-03T08:56+08:00`。
- 游戏内重点验证：
  - 第七章核心页奖励是否出现 Hana 协会和残响乐团核心页，不再显示 `Mod Needed`。
  - 阿尔加利亚核心页装备后是否显示/使用 `704001,704011,704012,704013,704014,705010,705011`。
  - 阿尔加利亚专属战斗书页是否不再从图鉴/库存被 prune 删除。
  - 异想体书页、核心页、战斗书页描述中的 `口口口` 是否明显减少或消失。
  - 解放战准备界面 5 人问题仍需游戏内再次验收；本轮没有继续改解放战人数逻辑。

---

## 2026-07-03 第三轮修复记录

### 本轮反馈

- 阿尔加利亚专属战斗书页已经出现，但阿尔加利亚核心页仍被设定为不可修改战斗卡组。
- 异想体书页、核心书页、战斗书页描述中的 `口口口` 仍未解决。

### 已修改文件

- `abcdcode_LOGLIKE_MOD/LogueBookModels.cs`
  - 阿尔加利亚/苍蓝残响从 `IsGrade6SpecialBuiltInDeckPage()` 固定卡组识别中移除，保留专属 `OnlyCard` 绑定但不再强制固定牌组。
  - 新增 `IsEditableBlueReverberationDeck()`，允许 RMR 准备界面中阿尔加利亚绕过原版 `IsLockByBluePrimary()` 卡组锁。
  - 读档、保存、查询固定卡组来源时清理旧路线里残留的阿尔加利亚固定卡组来源，避免旧存档继续锁卡组。
  - 装备核心页时仍对阿尔加利亚使用 origin-aware 版本刷新核心页数据，保证 `OnlyCard` 可绑定原版专属战斗书页。
- `abcdcode_Refactored/LogLikePatches.cs`
  - `BookModel.IsFixedDeck` 补丁中让 RMR 上下文里的阿尔加利亚返回可编辑。
  - `BookModel.AddCardFromInventoryToCurrentDeck` 补丁中让阿尔加利亚绕过蓝残响原版锁，并使用 origin-aware 卡牌查询。
  - 扩展 `ApplyRmrTmpFont()`：即使字体为空也会清理文本；字体存在时递归替换子节点 TMP 字体并执行 `SanitizeDisplayText()`。
  - 新增 `BattleDiceCardUI.SetCard`、`UIOriginCardSlot.SetData` 字体刷新钩子，并补到库存卡槽、被动转移弹窗。
- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - 核心页/掉落书页槽在设置名称后执行文本清理和 RMR TMP 字体刷新。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-03T09:11+08:00`。
- `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - 增加阿尔加利亚不是固定卡组、旧固定来源会被清理、蓝残响锁可绕过的断言。
- `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - 增加卡牌/书页 UI 递归 TMP 字体刷新和文本清理断言。

### 已验证

- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- 静态检查通过：
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - DLL SHA-256：`6CAEB0D665036FD953C6496109BFC6B419B8F7852740C4918050793EA3F41DF2`
- Git：
  - 本地提交已完成，提交信息为 `Fix Argalia deck editing and RMR text fonts`；精确哈希以 `git log -1 --oneline` 为准。
  - `git push` 未完成：远端 `https://github.com/izasaraba/RougelikeModReborn_private-version.git` 返回 `Repository not found`。

### 还没做 / 下一次优先验证

- 尚未启动游戏做实测。需要在 `Player.log` 确认加载到 `Build: 2026-07-03T09:11+08:00`。
- 游戏内重点验证：
  - 阿尔加利亚核心页装备后卡组是否可编辑。
  - 阿尔加利亚专属战斗书页是否仍在目录中，并可加入/移出卡组。
  - 普通固定卡组核心页如 Binah、漆黑静默是否仍保持固定专属牌组，不被本轮改动放开。
  - 异想体书页、核心页、战斗书页描述中的 `口口口` 是否消失；如果仍有，需要继续定位具体 UI 类或实际缺字字体。
  - 解放战准备界面可用角色仍需用户继续反馈；本轮没有继续修改解放战人数逻辑。

---

## 2026-07-03 第四轮修复记录

### 本轮反馈

- 游戏启动时弹出 `LogLikeMod Init error : Patching exception in method null`。
- `Player.log` 确认加载的是上一轮 DLL：`Build: 2026-07-03T09:11+08:00`，随后在 `Harmony.CreateAndPatchAll(typeof(LogLikePatches))` 阶段失败。

### 根因

- 上一轮新增的 `BattleDiceCardUI.SetCard` 字体刷新 HarmonyPatch 写成了单参数签名：
  - 错误：`SetCard(BattleDiceCardModel)`
  - 游戏实际签名：`SetCard(BattleDiceCardModel, BattleDiceCardUI.Option[])`
- Harmony 在运行时找不到目标方法，因此以 `method null` 形式中断整个 Mod 初始化。

### 已修改文件

- `abcdcode_Refactored/LogLikePatches.cs`
  - 将 `BattleDiceCardUI.SetCard` patch 签名修正为 `BattleDiceCardModel + BattleDiceCardUI.Option[]`。
- `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - 增加 `BattleDiceCardUI.SetCard` 双参数签名断言。
  - 将 `AssertContains` 从 `-like` 通配符匹配改为 `.Contains()`，避免 `Option[]` 中的 `[]` 被 PowerShell 当通配符解析。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-03T13:33+08:00`。

### 已验证

- `Player.log` 已确认上一轮故障 DLL 被实际加载，错误发生在 `PatchAll`。
- 通过反射检查游戏程序集：
  - `BattleDiceCardUI.SetCard(BattleDiceCardModel cardModel, BattleDiceCardUI+Option[] options)`
  - `UI.UIOriginCardSlot.SetData(DiceCardItemModel cardmodel)`
- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- 静态检查通过：
  - `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - DLL SHA-256：`2C4EBBA6807075228C66D9349947B5073A33D99442DC0FCF5A9AFD2E378CCB24`

### 还没做 / 下一次优先验证

- 尚未重新启动游戏验证 `PatchAll` 错误消失。需要在 `Player.log` 确认加载到 `Build: 2026-07-03T13:33+08:00`，且不再出现 `LogLikeMod Init error : Patching exception in method null`。
- 启动通过后再继续验证上一轮目标：
  - 阿尔加利亚核心页卡组是否可编辑。
  - 阿尔加利亚专属战斗书页是否可加入/移出卡组。
  - `口口口` 是否仍出现在具体哪些 UI 上。

---

## 2026-07-03 第五轮修复记录

### 本轮反馈

- `口口口` 问题仍未解决。
- 用户要求先一步一步查，确认问题之后再修改。

### 已确认的问题来源

- 不是 `Localize/cn`、`AddData`、`SpecialStaticInfo` 整体 UTF-8 损坏：扫描未发现这些目录存在 UTF-8 replacement char 污染。
- `Player.log` 显示游戏语言为 `cn`，Mod 也加载 `Localize\cn`。
- 已确认前几轮只给卡牌名、能力文本、部分核心页/异想体页 UI 套用了 RMR TMP 字体，但战斗书页的骰子行为描述 UI 仍在 `SetBehaviourInfo` 后由原版逻辑重新写入文本，没有重新套用 RMR TMP 字体。
- 通过反射确认运行时真实签名：
  - `BattleDiceCard_BehaviourDescUI.SetBehaviourInfo(DiceBehaviour, LorId, List<DiceBehaviour>, bool)`
  - `UI.UIDetailCardDescSlot.SetBehaviourInfo(DiceBehaviour, LorId, List<DiceBehaviour>, bool)`

### 已修改文件

- `abcdcode_Refactored/LogLikePatches.cs`
  - 新增 `BattleDiceCard_BehaviourDescUI.SetBehaviourInfo` 后置补丁，在 RMR 战斗/准备上下文中递归刷新 RMR TMP 字体。
  - 新增 `UI.UIDetailCardDescSlot.SetBehaviourInfo` 后置补丁，在详情卡骰子描述刷新后递归刷新 RMR TMP 字体。
- `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - 增加两个骰子行为描述字体补丁断言。
  - 增加四参数 `SetBehaviourInfo` 运行时签名断言，避免再次写错 Harmony Patch 目标。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-03T13:45+08:00`。

### 已验证

- 修改前静态探针为红：
  - 缺少 `BattleDiceCard_BehaviourDescUI_SetBehaviourInfo_RmrFont`
  - 缺少 `UIDetailCardDescSlot_SetBehaviourInfo_RmrFont`
- 修改后静态探针为绿：
  - 两个骰子行为描述字体补丁均存在。
- 静态检查通过：
  - `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - 备份：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\RogueLike Mod Reborn.dll.0703_text_desc_20260703_134812.bak`
  - DLL SHA-256：`0920EA2450ECFD1B06D55E82ACF927FAA88D5C8907688B44E259AC650D1F6041`

### 还没做 / 下一次优先验证

- 尚未启动游戏做视觉实测。需要在 `Player.log` 确认加载到 `Build: 2026-07-03T13:45+08:00`。
- 游戏内优先检查：
  - 战斗书页目录中骰子行为描述是否仍出现 `口口口`。
  - 右侧详情卡的骰子行为描述是否仍出现 `口口口`。
  - 异想体书页、核心页、战斗书页说明里是否还有未命中的 UI。
- 若仍有 `口口口`，下一步不要继续盲目加字体补丁，应拿用户截图定位具体 UI 类；已知还存在另一类数据问题：`Localize/cn` 和 `AddData` 中有少量原始韩文/假名文本，需要和字体问题分开处理。

---

## 2026-07-03 第六轮修复记录

### 本轮反馈

- 部署 `Build: 2026-07-03T13:45+08:00` 后，游戏内仍有 `口口口`。
- 用户要求查看 `Player.log`，必要时只对照 `original-codes` 相关部分，不要把现有玩法改回原作者版本。

### 已确认的问题来源

- `Player.log` 已确认实际加载到 `Build: 2026-07-03T13:45+08:00`，没有再出现 `LogLikeMod Init error` 或 `PatchAll` 报错。
- `Player.log` 显示语言为 `cn`，Mod 加载的是 `Localize\cn`。
- 扫描源码和数据目录未发现文本本身包含 `口口/□□/�`，所以不是整批本地化文件编码污染。
- 对照 `original-codes` 相关部分发现关键差异：
  - 原作者 `LogLikeMod.DefFont_TMP.get` 在为空时直接取 `LocalizedFontSetter.font_NotoSans`。
  - 当前版本改成扫描兼容字体，并且 `UIOptionWindow.Open` 与 `BattleMoneyUI.Create` 会把选项窗口下拉框字体写入 `DefFont_TMP`。
  - 游戏程序集里的 `LocalizedFontSetter` 实际存在 `cnFont_notoSansCJKsc`、`cnFont_notoSerifCJKsc`、`font_NotoSans` 等字段；中文界面应优先使用原生 CJK 字体，而不是任意下拉框字体。

### 已修改文件

- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - 新增 `ResolvePreferredLocalizedTmpFont()`，按语言优先选择游戏原生字体字段。
  - `cn/trcn` 优先使用 `cnFont_notoSansCJKsc`，其次 `cnFont_notoSerifCJKsc`，再回退通用 Noto 字体。
  - `DefFont_TMP` getter/setter 会优先使用当前语言的 preferred 字体，阻止选项窗口下拉框字体覆盖中文 CJK 字体。
  - 增加一次性日志：`[RMR Localize] Using preferred TMP font ...`，方便下一次从 `Player.log` 验证真实字体来源。
- `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - 增加中文原生 CJK TMP 字体字段、preferred 字体解析和日志断言。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-03T14:08+08:00`。

### 已验证

- 修改前红/绿探针为红：
  - 缺少 `cnFont_notoSansCJKsc`
  - 缺少 `ResolvePreferredLocalizedTmpFont`
  - 缺少 `Using preferred TMP font`
- 修改后红/绿探针为绿。
- 静态检查通过：
  - `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - 备份：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\RogueLike Mod Reborn.dll.0703_preferred_cjk_font_20260703_140528.bak`
  - DLL SHA-256：`81DDA1B8FEF711C4F85883B565A5AB60CEB83C4196B8FBCDF89E561E208C2667`

### 还没做 / 下一次优先验证

- 尚未启动游戏做视觉实测。需要在 `Player.log` 确认加载到 `Build: 2026-07-03T14:08+08:00`。
- 启动后需要在 `Player.log` 搜索：
  - `[RMR Localize] Using preferred TMP font`
  - 期望字体名来自 `cnFont_notoSansCJKsc` 或同等中文 CJK 字体。
- 游戏内继续检查：
  - 异想体书页描述是否还出现 `口口口`。
  - 核心书页描述是否还出现 `口口口`。
  - 战斗书页目录和右侧详情卡是否还出现 `口口口`。
- 如果仍有 `口口口`，下一步应按最新截图定位未命中的具体 UI 类，而不是继续扩大字体逻辑；本轮只修字体来源，不改玩法。

---

## 2026-07-03 第七轮修复记录

### 本轮反馈

- `口口口` 仍然存在；用户指出问题重点不是 `し协会`、`하나协会` 这种日韩字符，而是杂质层战斗书页奖励封面、异想体书页介绍、部分战斗书页介绍和核心书页说明显示为方框。
- 用户怀疑不是没部署，而是之前某次修改导致的显示回归。

### 已确认的问题来源

- `git status` 修改前为干净状态。
- `Player.log` 已确认上一轮 DLL 实际加载到 `Build: 2026-07-03T14:08+08:00`，并加载 `Localize\cn`，所以不是简单的未部署。
- `Player.log` 没有出现 `[RMR Localize] Using preferred TMP font ...` 日志，说明实际运行中不能确认已经切到中文 CJK TMP 字体。
- 对照 `original-codes` 后确认：原作者版本没有 `ApplyRmrTmpFont()` 这种递归强制替换整个 UI 子树 TMP 字体的逻辑。
- 当前版本的 `ApplyRmrTmpFont()` 会无条件执行 `text.font = LogLikeMod.DefFont_TMP`。如果 `DefFont_TMP` 是选项下拉框字体或覆盖不完整的字体，就会把原本能正常显示的原版 UI 文字打成方框。这个行为正好覆盖卡牌、异想体页、核心页和 Tooltip 等用户反馈区域。

### 已修改文件

- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - 新增 `CanTmpFontRenderText()`，用 TMP 字体和 fallback 字体递归检查当前文本是否可渲染。
  - 检查时跳过富文本标签和空白/控制字符，避免 `<color>` 等标记误判。
- `abcdcode_Refactored/LogLikePatches.cs`
  - 修改 `ApplyRmrTmpFont()`：先执行 `SanitizeDisplayText()` 清理文本，再只在“当前字体无法渲染且候选 RMR 字体可以渲染”时替换 TMP 字体。
  - 不再无条件覆盖原版卡牌、异想体、核心页 UI 已有字体。
- `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - 增加 `CanTmpFontRenderText()`、`ShouldUseRmrTmpFont()` 和“保留可渲染原版字体”的断言。
  - 更新旧的直写 `text.text = RewardingModel.SanitizeDisplayText(text.text)` 断言为新的 `sanitizedText` 流程。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-03T21:32+08:00`。

### 已验证

- 静态检查通过：
  - `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out_textfont_guard\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - 备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\0703_textfont_guard_20260703_211221`
  - DLL SHA-256：`7E0741370F9EE05975AE1DF40ED1E53D7E68302909A858F60840A9ACF3D7E508`

### 还没做 / 下一次优先验证

- 尚未启动游戏做视觉实测。需要在 `Player.log` 确认加载到 `Build: 2026-07-03T21:32+08:00`。
- 游戏内优先检查：
  - 杂质层战斗书页奖励封面是否不再显示 `口口口`。
  - 异想体书页介绍是否恢复正常。
  - 核心书页说明和战斗书页说明是否恢复正常。
- 如果仍然有 `口口口`，下一步应抓具体截图中的页面类型和卡/页 ID，再检查对应 UI 的实际 TMP 字体名；不要继续扩大玩法逻辑或奖励逻辑。

---

## 2026-07-04 第八轮修复记录

### 本轮反馈

- 用户反馈 2026-07-04 01:26 左右截图中仍然存在 `口口口`。
- 用户指出昨天之前版本没有这个问题，怀疑是 2026-07-03 多轮修改时上下文过多导致继续修改出错。

### 已确认的问题来源

- `Player.log` 确认游戏实际加载了上一轮部署 DLL：
  - `Build: 2026-07-03T21:32+08:00`
  - 本地化加载路径为 `...\Assemblies\dlls\Localize\cn`
  - `option.dat` 中游戏语言为 `cn`
- 因此这次不是简单的“没有部署”。
- 从 `e998173`（2026-07-02 基线）到当前 HEAD 的差异看，问题相关变更集中在：
  - `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - `abcdcode_Refactored/LogLikePatches.cs`
- 2026-07-03 新增了 `ApplyRmrTmpFont()`，并把它挂到卡牌、异想体页、核心页、Tooltip 和被动转移等 UI。原作者基线和 2026-07-02 基线都没有这条递归强制处理整棵 UI 子树 TMP 字体的逻辑。
- `Player.log` 没有出现 `Using preferred TMP font` / `Selected TMP font`，说明上一轮 preferred-font 实验没有在实测日志里形成可验证的字体选择证据。
- 最可能根因：递归 TMP 字体刷新链覆盖了原版 UI 本来可用的字体，导致卡牌、异想体页、核心页等原版文本显示路径出现方框。

### 已修改文件

- `abcdcode_Refactored/LogLikePatches.cs`
  - 移除 `ApplyRmrTmpFont()` 和 `ShouldUseRmrTmpFont()`。
  - 移除卡牌、异想体、核心页、Tooltip 等 UI 上的递归 TMP 字体刷新调用。
  - 移除 font-only Harmony postfix：
    - `BattleDiceCardUI_SetCard_RmrFont`
    - `UIOriginCardSlot_SetData_RmrFont`
    - `BattleDiceCard_BehaviourDescUI_SetBehaviourInfo_RmrFont`
    - `UIDetailCardDescSlot_SetBehaviourInfo_RmrFont`
- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - 回退 2026-07-03 新增的 preferred TMP 字体选择实验。
  - 移除 `ResolvePreferredLocalizedTmpFont()`、`CanTmpFontRenderText()` 等只服务于递归字体替换链的代码。
  - 保留 2026-07-02 已有的语言同步、文本字典清理和多字符 CJK 字体探针。
  - 移除卡牌/核心页 UI 里新增的递归字体刷新调用，保留文本本地化/展示名兜底。
- `RMR_Core.cs`
  - 构建时间戳更新为 `2026-07-04T01:52+08:00`。
  - 移除 Tooltip 上的递归 TMP 字体刷新调用。
- `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - 更新静态检查：要求保留 2026-07-02 的语言同步规则，同时禁止重新引入递归 `ApplyRmrTmpFont` 和 font-only 卡牌/骰子描述 postfix。

### 已验证

- 静态检查通过：
  - `tools/static_checks/runtime_release/RMR_0629_language_sync_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
- 额外源码扫描通过：
  - 未发现 `ApplyRmrTmpFont`
  - 未发现 `BattleDiceCardUI_SetCard_RmrFont`
  - 未发现 `UIOriginCardSlot_SetData_RmrFont`
  - 未发现 `SetBehaviourInfo_RmrFont`
  - 未发现 `ResolvePreferredLocalizedTmpFont`
- `git diff --check`：无空白错误；仅有仓库既有 LF/CRLF 提示。
- Release 编译通过：
  - 输出 DLL：`C:\Users\13034\AppData\Local\Temp\rmr_build_out_font_revert\RogueLike Mod Reborn.dll`
  - 仅有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - DLL：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - 备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\0704_revert_recursive_font_patch_20260704_013358`
  - DLL SHA-256：`8093CDDE06BBA93112F1D84146D65255C536F88F7079B38E761111F4538C7291`

### 还没做 / 下一次优先验证

- 尚未启动游戏做视觉实测。需要在 `Player.log` 确认加载到：
  - `Build: 2026-07-04T01:52+08:00`
- 游戏内优先验证：
  - 杂质层战斗书页奖励封面是否恢复。
  - 异想体书页介绍是否恢复。
  - 核心书页说明和战斗书页说明是否恢复。
- 如果仍有 `口口口`，下一步不要再改字体链，应按具体卡/页 ID 检查其本地化 ID 和描述来源。
---

## 2026-07-04 02:15 本轮修复记录：杂质 Mod Needed、阿尔加利亚原版核心页、解放战空牌组

### 用户最新反馈

- 杂质核心页奖励仍出现 `Mod Needed`；如果不是原版可用内容就删除。
- 阿尔加利亚奖励给到的是当前模组包装的敌对/都市之星口径核心页，不是原版通关扭曲残响乐团后的阿尔加利亚之页。
- 阿尔加利亚战斗书页已出现，但进入战斗表现仍像敌方阿尔加利亚。
- 解放战胜利后仍需确认完成状态；解放战准备阶段配置好战斗书页后，进入战斗可能没有可用战斗书页。
- 杂质战斗书页奖励封面 `口口口` 可能与语言切换/缓存有关，用户切换语言再切回后可恢复。

### 已核查结论

- `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml` 中 `260005-260014` 在当前源码和 `original-codes` 的本地化表里都没有对应玩家核心页名称，且用户实测显示 `Mod Needed`，本轮不再把它们视为有效杂质核心页池。
- 当前仓库新增了 `AddData/EquipPage/EquipPage_Librarian_ch6.xml:250013` 模组包装页；原作者基线没有这个玩家侧包装页。奖励路径原先写 `new LorId(LogLikeMod.ModId, 250013)`，会授予模组包装页而不是原版包语义的阿尔加利亚之页。
- `ApplyAtlasOnlyLoadout()` 原先只要核心页有 `OnlyCard` 就跳过自动填牌；这会让可编辑专属核心页在解放战临时图鉴配队中空牌组开战。
- `Player.log` 有 `ui_RMR_RealizationTitle/Desc/Challenge/Cleared/Close` 缺键记录，`Localize/{cn,en,kr}/UIs.txt` 确实缺这些键。

### 已修改文件

- `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml`
  - 杂质核心页奖励池删除 `260005-260014`，保留当前已验证可显示的 `260001-260004`。
- `RMR_Core.cs`
  - BuildTimestamp 更新为 `2026-07-04T02:06+08:00`。
  - 新增 `GetBlueReverberationCorePageLorId()`，阿尔加利亚奖励使用原版 `new LorId(250013)`。
  - 新增 `PruneLegacyBlueReverberationCorePageUnlocks()`，进入都市之星自动奖励时清理旧存档/当前路线中的模组包装 `LogLikeMod.ModId:250013`。
  - `EnsureRoleBookInCurrentBooklist()` 改用 origin-aware 解析，避免原版包 ID 入库失败。
  - `BookModel.SetXmlInfo` 后缀的 OnlyCard 解析增加原版兜底和 null guard，避免模组核心页绑定原版专属战斗书页时加载空项。
- `RMR_AbnormalityUnlocks.cs`
  - 扭曲残响乐团胜利奖励改为授予 `RMRCore.GetBlueReverberationCorePageLorId()`，并同步清理旧模组包装阿尔加利亚页。
- `RMR_RealizationManager.cs`
  - 解放战临时图鉴配队不再把所有 `OnlyCard` 核心页当固定牌组；只有真正固定/锁定牌组才跳过自动填牌。
- `abcdcode_Refactored/LogLikePatches.cs`
  - `UnitDataModel.GetDeckForBattle` 的默认牌组补足扩展到解放战战斗阶段，降低空牌组进入战斗的风险。
- `Localize/cn/UIs.txt`、`Localize/en/UIs.txt`、`Localize/kr/UIs.txt`
  - 补齐 `ui_RMR_RealizationTitle/Desc/Challenge/Cleared/Close`。
- 静态检查脚本：
  - `tools/static_checks/realization/RMR_0617_realization_session_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0621_reported_runtime_regressions_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - 更新断言：`260005-260014` 是本轮确认的 `Mod Needed` 候选，应禁止出现在杂质核心页池；OnlyCard 不应阻止解放战临时配队自动填牌。

### 已验证

- 红/绿静态核查已从失败转为通过：
  - 杂质核心池不含 `260005-260014`。
  - 阿尔加利亚授予路径不再使用模组包 `250013`。
  - 解放战临时配队不再因 generic `OnlyCard` 跳过填牌。
  - 三个语言的解放战 UI 键齐全。
- XML 解析通过：
  - `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml`
  - `Localize/cn/UIs.txt`
  - `Localize/en/UIs.txt`
  - `Localize/kr/UIs.txt`
- 静态脚本通过：
  - `RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - `RMR_0617_realization_session_static_check.ps1`
  - `RMR_0628_user_reported_rewards_static_check.ps1`
  - `RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - `RMR_0621_reported_runtime_regressions_static_check.ps1`
  - `RMR_0629_language_sync_static_check.ps1`
- `git diff --check` 无空白错误；只有 Git 的 LF/CRLF 提示。
- Release 编译通过，输出到 `%TEMP%\rmr_build_out\RogueLike Mod Reborn.dll`；仅有既有 CS0649 警告。
- 已部署到 Workshop 运行树并逐项哈希核对：
  - `RogueLike Mod Reborn.dll` SHA-256 `93D8A4645E4E6B5B8F6E272F990CA7B23E4ED7B4FC1D669BA2A0E8E2279032AC`
  - `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml` SHA-256 `575A64CEE5D35BCB126CE67426A33BF1C4CF20205B98693967F4744BE18594AF`
  - `Localize/cn/UIs.txt` SHA-256 `70A6D7D4745562BE5F6CE62C35428FAAE7E69B426517E1166745FBD5B0DC9365`
  - `Localize/en/UIs.txt` SHA-256 `DB3E0C79B71E2D259A1DD1983C0D90620BEBA059279316F1CE904A30ED016A51`
  - `Localize/kr/UIs.txt` SHA-256 `2027FA90770407F389D020CEF846B3938BBD5CCC20705A8DFB4EC7737A9FE149`
  - 部署前备份目录：`E:/Steam/steamapps/workshop/content/1256670/3503523710/Assemblies/_codex_backups/deploy_20260704_021641`

### 下一步还没做

- 部署后需要用户游戏内验证：
  - 杂质核心页奖励是否不再出现 `Mod Needed`。
  - 旧存档里已获得的模组包装阿尔加利亚是否在下次都市之星奖励/扭曲残响胜利奖励后被替换为原版 `@origin:250013`。
  - 阿尔加利亚核心页装备后是否使用原版阿尔加利亚核心页行为，并且可配置/使用正确专属战斗书页。
  - 解放战准备配置好牌组后，进入战斗是否仍会空牌组。
  - 解放战胜利后完成状态是否在面板和后续奖励池中一致。
  - 若杂质战斗书页封面仍有 `口口口`，下一轮应抓具体卡牌 ID 和页面类型，不再继续扩大字体补丁。
