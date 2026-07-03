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
