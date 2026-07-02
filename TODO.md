本轮 2026-07-02 13:34 已完成并部署

本轮先核对运行日志，发现 `Player.log` 里实际加载的仍是：

- `[RMR] RogueLike Mod Reborn initializing. Build: 2026-07-02T08:34+08:00`

这说明用户截图对应的测试没有加载 09:02 补丁；截图文件时间也早于 09:02 部署。不过日志同时暴露了一个真实的新根因：

- `SetNextStage(70011)` 进入杂质随机事件壳时，日志显示敌人为 `LorId(:28370003)`，包名被清空。
- 原因是 `RestoreVanillaEnemyIdsForImpurityStage` 把 `70001..70021` 全部当成原版杂质战斗处理，导致 `70011/70012` 的 Mod 事件壳敌人 `28370003/28370004` 被错误改成原版包语义，敌人解析失败并空引用。

本轮追加修复：

- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - `RestoreVanillaEnemyIdsForImpurityStage` 现在只作用于真正的原版杂质战斗：
    - `70001`-`70010`
    - `70020`
    - `70021`
  - 不再作用于 `70011/70012`，避免事件壳敌人 `28370003/28370004` 被去掉 Mod 包。
  - `SetNextStage` 增加 `_waveList` 反射空值恢复，避免波次列表为空时直接空引用。
  - `SetNextStage` 写入 `stageModel.ClassInfo.mapInfo` 前增加 `ClassInfo` 空值保护。
- `abcdcode_LOGLIKE_MOD/MysteryModel_Mystery_ChX_3.cs`
  - 章节通用随机事件奖励数组从 6 档扩展到 7 档，补齐杂质章节。
  - 增加章节索引 clamp，防止后续章节或异常章节值越界。
- `abcdcode_LOGLIKE_MOD/MysteryModel_Mystery_ChX_4.cs`
  - 初始化 `droplist`，避免点击一次性商店随机事件时空引用。
  - 增加空候选保护，避免 `all[0]` 越界。
- `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - 增加 70011/70012 事件壳敌人不能被去包的断言。
  - 增加 70021 三波与 70020/70021 原版包恢复范围断言。
  - 增加 chAll 随机事件第七章档位和 droplist 初始化断言。
- `RMR_Core.cs`
  - `BuildTimestamp` 更新为 `2026-07-02T13:34+08:00`。

本轮验证：

- XML 解析通过：
  - `AddData/StageInfo/StageInfo_ch7event.xml`
  - `SpecialStaticInfo/MysteryXmlInfos/ch7_mysterys.xml`
  - `SpecialStaticInfo/MysteryXmlInfos/chAll_mysterys.xml`
  - `AddData/EnemyUnitInfo/EnemyUnitInfo_chAll_event.xml`
  - `AddData/EquipPage/EquipPage_Enemy_chAll_event.xml`
  - `AddData/Passives/PassiveList_chAll_event.xml`
  - `AddData/StageInfo/StageInfo_ch7.xml`
- 静态检查通过：
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0618_reward_event_atlas_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
  - `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
- `git diff --check` 对本轮相关文件通过，仅有既有 LF -> CRLF 提示。
- Release 编译通过，仅有既有 warning：
  - `RMREffect_Duplicator.Dupe.cards` never assigned。

本轮已部署：

- Workshop 根目录：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls`
- 部署前备份：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260702_1334`
- 已部署并哈希一致：
  - `RogueLike Mod Reborn.dll` SHA-256 `AA313E7BDC86FD2BD4F4FB6F565CBD06FE9621DA60961C2B803F6677E3FD122A`
  - `AddData/StageInfo/StageInfo_ch7event.xml` SHA-256 `722CFF4570FF02F269B54626BC6BCD80F62D8A0C3DEF724573E52466F5048A41`
  - `AddData/EnemyUnitInfo/EnemyUnitInfo_chAll_event.xml` SHA-256 `9D52A50C801A3F4DBEFE27D624F6F2FFAF1139584F2C9600454048F4A13E4DC7`
  - `AddData/EquipPage/EquipPage_Enemy_chAll_event.xml` SHA-256 `A7714C7CFD682C72F9495EEBC6BDB3D49B02A3608D7D23C03DB5AF1842E2AEF5`
  - `AddData/Passives/PassiveList_chAll_event.xml` SHA-256 `9ED479C423A7CDA38925FEED93B0A9D43B59D51EB9F20A5C15AC02FB92DC6F5E`
  - `SpecialStaticInfo/MysteryXmlInfos/chAll_mysterys.xml` SHA-256 `329A22FF749971CCD0FCC42A182BC535C5DDB74769643ABEE84491B5002A687F`
  - `SpecialStaticInfo/MysteryXmlInfos/ch7_mysterys.xml` SHA-256 `9EC0DF9945D95F358A586F1501C589FE71BC4C63D63562FE73D33D703E22B59E`
  - `SpecialStaticInfo/StagesXmlInfos/Stage_ch7.xml` SHA-256 `63BC9FCD226EAE9E0DD47708D7D78079382CBC60CEF48E843E36860C0E4986C1`
  - `AddData/StageInfo/StageInfo_ch7.xml` SHA-256 `5CC23DAED81E92245735D6B97F1D03380C3A95E2C540D04B49ADFA1678580429`
  - `AddData/CardDropTable/CardDropTable_ch7.xml` SHA-256 `9E5AD46710C1D979419289C68A05C17AF74D1CFB5229948488185F21CB6DAC55`
  - `SpecialStaticInfo/DropValueXmlInfos/values_ch7.txt` SHA-256 `C6D69E23AF515815D30F2941A923B84A40EA0932E2A6E991E50D9A863EBB95B4`
  - `AddData/DropBook/Dropbook_ch7.xml` SHA-256 `576CB5E52593E16D22081F4650EB8A2CB861B6F00A9140FC58083F0CA2B671D4`
  - `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml` SHA-256 `088CAB996AE1C67A848C862FFA1CC18DE07847928D8C75D69ECC58C5A302F59C`

下一步还没做，留给游戏内实测后继续：

1. 必须完全退出并重新启动游戏，然后在 `Player.log` 看到：
   - `[RMR] RogueLike Mod Reborn initializing. Build: 2026-07-02T13:34+08:00`
2. 如果日志仍是 `08:34` 或 `09:02`，说明游戏没有加载本轮 DLL，所有游戏内现象都不能作为本轮补丁验证结果。
3. 复验 `70011/70012` 杂质随机事件：
   - 不应再出现 `LorId(:28370003)` 或 `LorId(:28370004)`。
   - 不应再在 `SetNextStage(70011/70012)` 附近 NullReference。
4. 复验 `70021`：
   - 日志应出现 `wave=1/3`、`wave=2/3`、`wave=3/3`。
   - 游戏内应连续三波，而不是只打一层。
5. 复验奖励：
   - 杂质 Boss 核心页奖励不再显示 `ModNeeded`/白卡。
   - Boss 胜利后战斗书页候选不为空。
   - 扭曲残响首通后重新到都市之星，苍蓝残响核心页与 `604021`-`604025` 均在图鉴/目录中。

本轮 2026-07-02 09:02 已完成并部署

已修复/调整：

- 杂质核心书页奖励卡显示 `ModNeeded`/白卡：
  - `LogLikeMod.RegisterPickUpXml(RewardPassiveInfo)` 注册 EquipPage 虚拟奖励卡后立即调用 `RewardingModel.CreateEquipRewardXmlData`。
  - `RewardingModel.CreateEquipRewardXmlData` 现在用核心页真实名称、描述、稀有度、能力文本和书页图标填充奖励卡显示。
  - `GetRegisteredPickUpXml(RewardPassiveInfo)` 增加 packageId/workshopID 双 fallback，避免 `@origin` 奖励查不到虚拟卡。
- 扭曲残响乐团 Boss 只有一层：
  - `LogLikeMod.SetNextStage` 不再只取 `waveList[0]`，现在把 StageInfo 的所有 Wave 加入 StageModel；`70021` 源数据本身已有 3 个 Wave。
- 击败扭曲残响乐团后只解锁苍蓝残响核心页、不解锁战斗书页：
  - `LogueBookModels.AddCard` 增加 `GetCardItem(cardId, true)` 包语义 fallback。
  - `RMR_AbnormalityUnlocks.GrantDistortedEnsembleVictoryRewards` 授予核心页和 `604021`-`604025` 后立即 `SavePermanentAtlasUnlocks()`。
- 杂质空随机事件：
  - 新增 `SpecialStaticInfo/MysteryXmlInfos/ch7_mysterys.xml`，补齐 Stage_ch7 使用的 `70011/70012` Mystery 数据。
- “本章节/下一章节战斗书页”事件总是传闻/都市传说：
  - `MysteryModel_Mystery_Ch1_1` 不再硬编码 `1001/2001`，改为按 `LogLikeMod.curchaptergrade` 动态取当前/下一章平装书；第七章下一章封顶第七章。
- 殷红迷雾彩蛋关卡概率：
  - `LogueBookModels.RedMistChallengeAppearanceChance` 从 `0.6f` 改为 `0.3f`。
- 静态检查：
  - 更新 `RMR_0618_reward_event_atlas_static_check.ps1` 的过时断言，改为验证当前 EGO 队列 API 和动态章节掉落逻辑。
  - 更新 `RMR_0628_runtime_end_and_ch7_static_check.ps1`，覆盖多 Wave Stage、ch7 Mystery、EquipPage 奖励显示和苍蓝残响战斗书页保存。
  - 更新 `RMR_0701_progression_reset_abno_rules_static_check.ps1` 的殷红迷雾概率断言为 0.3。

本轮验证：

- XML 解析通过：
  - `SpecialStaticInfo/MysteryXmlInfos/ch7_mysterys.xml`
  - `SpecialStaticInfo/StagesXmlInfos/Stage_ch7.xml`
  - `AddData/StageInfo/StageInfo_ch7.xml`
  - `AddData/StageInfo/StageInfo_ch7event.xml`
  - `AddData/CardDropTable/CardDropTable_ch7.xml`
  - `SpecialStaticInfo/DropValueXmlInfos/values_ch7.txt`
  - `AddData/DropBook/Dropbook_ch7.xml`
  - `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml`
- 静态检查通过：
  - `tools/static_checks/rewards/RMR_0618_reward_event_atlas_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
- `git diff --check` 对本轮相关文件通过，仅有既有 LF -> CRLF 提示。
- Release 编译通过，仅有既有 warning：
  - `RMREffect_Duplicator.Dupe.cards` never assigned。

本轮已部署：

- Workshop 根目录：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls`
- 部署前备份：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260702_0902`
- 已部署并哈希一致：
  - `RogueLike Mod Reborn.dll` SHA-256 `55ECBEB4B4F72403CC166F00BFCEA630D8D0E158B77A42424F0226C8726668C3`
  - `SpecialStaticInfo/MysteryXmlInfos/ch7_mysterys.xml` SHA-256 `9EC0DF9945D95F358A586F1501C589FE71BC4C63D63562FE73D33D703E22B59E`
  - `SpecialStaticInfo/StagesXmlInfos/Stage_ch7.xml` SHA-256 `63BC9FCD226EAE9E0DD47708D7D78079382CBC60CEF48E843E36860C0E4986C1`
  - `AddData/StageInfo/StageInfo_ch7.xml` SHA-256 `5CC23DAED81E92245735D6B97F1D03380C3A95E2C540D04B49ADFA1678580429`
  - `AddData/StageInfo/StageInfo_ch7event.xml` SHA-256 `722CFF4570FF02F269B54626BC6BCD80F62D8A0C3DEF724573E52466F5048A41`
  - `AddData/CardDropTable/CardDropTable_ch7.xml` SHA-256 `9E5AD46710C1D979419289C68A05C17AF74D1CFB5229948488185F21CB6DAC55`
  - `SpecialStaticInfo/DropValueXmlInfos/values_ch7.txt` SHA-256 `C6D69E23AF515815D30F2941A923B84A40EA0932E2A6E991E50D9A863EBB95B4`
  - `AddData/DropBook/Dropbook_ch7.xml` SHA-256 `576CB5E52593E16D22081F4650EB8A2CB861B6F00A9140FC58083F0CA2B671D4`
  - `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml` SHA-256 `088CAB996AE1C67A848C862FFA1CC18DE07847928D8C75D69ECC58C5A302F59C`

下一步还没做，留给游戏内实测后继续：

1. 启动游戏后先在 `Player.log` 确认加载本轮 DLL：
   - `[RMR] RogueLike Mod Reborn initializing. Build: 2026-07-02T09:02+08:00`
2. 杂质 Boss 复验：
   - 扭曲残响乐团 `70021` 是否按三波/三层连续执行。
   - Boss 胜利奖励是否显示真实核心书页信息，不再出现 `ModNeeded`/白卡。
   - Boss 胜利后是否出现战斗书页掉落。
3. 跨路线首通奖励复验：
   - 击败扭曲残响乐团后，重新到都市之星是否同时自动解锁苍蓝残响核心页和 `604021`-`604025` 战斗书页。
4. 杂质随机事件复验：
   - `70011/70012` 不再是空事件。
   - “获取本章节/下一章节战斗书页”在不同章节给对应章节掉落，不再固定传闻/都市传说。
5. 都市之星殷红迷雾复验：
   - 出现概率已改为 0.3，但概率类问题需要多条路线抽样确认。
6. 如果仍无战斗书页掉落，下一轮优先看 `Player.log` 中本场 Stage、敌人 DropBook、`QueueDropBookReward/NormalizeDropBookRewards/PickUpCards` 的实际候选是否为空或被图鉴过滤。

本轮 2026-07-02 09:02 处理中（历史记录，上方已写入完成与部署结果）

用户最新反馈：

- 杂质部分核心书页奖励仍显示 `ModNeeded`/空描述，且没有战斗书页掉落。
- 扭曲残响乐团 Boss 仍只有一层，不是三层完整流程。
- 击败扭曲残响乐团后，重新到都市之星只解锁苍蓝残响核心页，没有解锁苍蓝残响战斗书页。
- 杂质部分存在空随机事件。
- “获取本章节或下章节战斗书页”随机事件章节定位错误，仍像传闻/都市传说。
- 殷红迷雾作为彩蛋关卡，出现概率改为 0.3。

本轮已修改：

- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - `SetNextStage` 改为把 `StageClassInfo.waveList` 的所有波次加入 `StageModel`，不再只加入第 1 波；用于修复 70021 扭曲残响乐团只打一层。
  - 核心页奖励虚拟卡注册后调用 `RewardingModel.CreateEquipRewardXmlData` 生成标题/描述。
  - `GetRegisteredPickUpXml(RewardPassiveInfo)` 增加 package/workshop key 双回退，避免 `@origin` 奖励查不到虚拟卡。
- `abcdcode_LOGLIKE_MOD/RewardingModel.cs`
  - `CreateEquipRewardXmlData` 增加空值保护，并用核心页名称、章节、稀有度、能力文本和书页图标覆盖奖励卡显示。
- `abcdcode_LOGLIKE_MOD/LogueBookModels.cs`
  - `AddCard` 使用 `GetCardItem(cardId, true)` 包回退，避免苍蓝残响等战斗书页因包语义未写入图鉴。
  - 殷红迷雾关卡出现概率从 `0.6f` 改为 `0.3f`。
- `RMR_AbnormalityUnlocks.cs`
  - 扭曲残响乐团胜利授予苍蓝残响核心页和战斗书页后立即 `SavePermanentAtlasUnlocks()`。
- `RMR_Core.cs`
  - `BuildTimestamp` 更新为 `2026-07-02T09:02+08:00`。
- `abcdcode_LOGLIKE_MOD/MysteryModel_Mystery_Ch1_1.cs`
  - “本章节/下一章节战斗书页”奖励不再硬编码 `1001/2001`，改为按当前 `LogLikeMod.curchaptergrade` 动态取平装书；第七章下一章封顶第七章。
- `SpecialStaticInfo/MysteryXmlInfos/ch7_mysterys.xml`
  - 新增 `70011/70012` Mystery 数据，复用现有全章节事件和本地化，修复杂质路线空随机事件。
- `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - 殷红迷雾概率断言更新为 0.3。
- `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - 增加多波 Stage 装载、70011/70012 Mystery 数据、核心页奖励显示、苍蓝残响战斗书页持久化、动态章节奖励事件检查。

本轮尚未完成：

- 还未运行 XML 解析、静态检查、Release 编译、`git diff --check`。
- 还未部署到 Workshop。
- 还未做游戏内实测；部署后需要复验 70021 是否连续三波、杂质奖励是否显示真实核心页信息、是否出现战斗书页奖励、70011/70012 是否不再空白、苍蓝残响战斗书页是否进入图鉴/战斗书页目录。

本轮 2026-07-02 08:34 已处理并部署

用户最新反馈：

- 解放战准备界面可选角色仍只有 3 人。
- 初始遗物选择界面没有“重置所有永久进度”按钮。
- 图鉴并未全解锁，但解放战核心页和战斗书页看起来解锁过多。
- 游戏仍会莫名其妙结束。

本轮已先清除本机旧进度：

- 已备份并删除 `C:\Users\13034\AppData\LocalLow\Project Moon\LibraryOfRuina\LogueSave` 下的旧 RMR 进度文件。
- 备份目录：
  - `C:\Users\13034\AppData\LocalLow\Project Moon\LibraryOfRuina\LogueSave\_codex_save_backups\reset_20260702_082624`
- 已删除永久图鉴、楼层解放进度、红雾/漆黑静默/扭曲残响乐团首通记录、当前路线存档；保留 `RMR_ItemCatalog`。

本轮代码修复：

- `RMR_Core.cs`
  - 普通 RMR 初始事件 `-1/chstart` 进入前也会设置 `RMRRealizationManager.SetInitialRelicEntryAvailable(true)`。
  - `BuildTimestamp` 更新为 `2026-07-02T08:34+08:00`。
- `SpecialStaticInfo/MysteryXmlInfos/chstart.xml`
  - 旧初始遗物事件也加入“楼层解放战 / 重置所有永久进度 / 游戏玩法介绍”入口和确认/说明帧。
- `Localize/cn|en|kr/Mystery_Start.txt`
  - 补齐旧 `chstart` 重置确认、返回、玩法介绍、重置完成文本。
- `abcdcode_LOGLIKE_MOD/MysteryModel_ChStart.cs`
  - 旧初始遗物事件支持 Roadless Camelot、解放战入口、重置确认、玩法介绍。
  - 普通遗物选择会关闭一次性解放战入口；重置/玩法介绍不会消耗入口。
- `RMR_RealizationManager.cs`
  - `PendingRealizationBattle` 在打开准备界面前设置，避免 BattleSetting UI hook 错过解放战状态。
  - 解放战准备临时 loadout 必须来自永久图鉴集合；图鉴为空时安全中止。
  - 进入原版解放战前后强制把 StageWave 可用人数设为 5，修复仍按原版 3 人限制显示的问题。
- `abcdcode_LOGLIKE_MOD/LogueBookModels.cs`
  - 永久图鉴加载不再从 `Lastest` 当前路线存档回灌，避免旧路线库存污染解放战准备池。
  - 路线池初始化/旧存档加载会剔除空 Stage、空 `waveList` 和 0 波次 Stage。
  - `CreateStageDesc` 增加空节点/空 PickUp XML 保护。
- `abcdcode_LOGLIKE_MOD/LogLikeMod.cs`
  - `GetRegisteredPickUpXml(LogueStageInfo)` 改为 `TryGetValue`，避免缺字典键直接异常。
  - `GetPickUpXmlWorkShopId_Stage` 增加空值保护。
- `RMR_AbnormalityBattleRouter.cs`
  - 异想体战候选必须有 `mapInfo`，避免日志中 `209003` 这类候选进入后触发 `StageController.InitializeMap` 空引用。
- `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - 扩展检查旧 `chstart`、图鉴不回灌 `Lastest`、解放战 5 人准备、异想体 `mapInfo` 防护。

本轮已验证：

- XML 解析通过：
  - `SpecialStaticInfo/MysteryXmlInfos/chstart.xml`
  - `SpecialStaticInfo/MysteryXmlInfos/RMR_chstart.xml`
  - `Localize/cn/Mystery_Start.txt`
  - `Localize/en/Mystery_Start.txt`
  - `Localize/kr/Mystery_Start.txt`
  - `Localize/cn/MysteryEvents/RMR_chstart.xml`
  - `Localize/en/MysteryEvents/RMR_chstart.xml`
  - `Localize/kr/MysteryEvents/RMR_chstart.xml`
- 静态检查通过：
  - `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - `tools/static_checks/events_abnormality/RMR_chstart_localization_static_check.ps1`
  - `tools/static_checks/realization/RMR_0617_progression_realization_static_check.ps1`
  - `tools/static_checks/realization/RMR_0614_realization_reward_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_binah_route_ego_toggle_grade7_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
- Release 编译通过，仍只有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- `git diff --check` 对本轮相关文件无空白错误；仅提示这些文件下次 Git 触碰会按配置 LF -> CRLF。

本轮已部署到 Workshop：

- 部署根目录：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls`
- 部署前备份：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260702_0834`
- 已部署并哈希核对通过：
  - `RogueLike Mod Reborn.dll` SHA-256 `3D50C1AF53FF84C83D6A5D7EE40E10EA8DF087C36721DDE9869927DA53075783`
  - `SpecialStaticInfo/MysteryXmlInfos/chstart.xml` SHA-256 `71C199222924F0C6D5FB38B5FAD11B6DA5801D47FB325CC2B497341BE44AAF08`
  - `SpecialStaticInfo/MysteryXmlInfos/RMR_chstart.xml` SHA-256 `94E4304EDE91B4D7A3B3FD10E00D6D34EEA07805846A25E481B76E7ED22CEFFA`
  - `Localize/cn/Mystery_Start.txt` SHA-256 `F0F3D3A01A61F1EB38A8E73B5DCE325A46FB7F2F6062236BEDB6A3A599792F5B`
  - `Localize/en/Mystery_Start.txt` SHA-256 `F2CC5E19A932EDCEA80B00CBFB3DBA034E11FB42D4EF7A6492DF4BADF6ED62F0`
  - `Localize/kr/Mystery_Start.txt` SHA-256 `C8E0D5E23C4589578D536406F29EECE7A67B9AB9FD0F56E8C4004FE1C3E1E493`
  - `Localize/cn/MysteryEvents/RMR_chstart.xml` SHA-256 `899D308AE238CBF67875BA573E52E7CE2475762459ECADACE608726ACA575D37`
  - `Localize/en/MysteryEvents/RMR_chstart.xml` SHA-256 `C073EDF35D041E880F53B93C6E5D8C7462D097E6EE5F2750A24354A86881DD22`
  - `Localize/kr/MysteryEvents/RMR_chstart.xml` SHA-256 `F4A28B7812D1A6BF49A22F3DADD27191D24DD99176340244DD96B8ACBF03EF41`

下一步还没做，留给游戏内实测后继续：

1. 启动游戏后先在 `Player.log` 确认加载本轮 DLL：
   - `[RMR] RogueLike Mod Reborn initializing. Build: 2026-07-02T08:34+08:00`
2. 初始遗物选择界面需要同时验证旧 `chstart` 与新 `RMR_chstart` 路径：
   - 解放战入口可见。
   - “重置所有永久进度”按钮可见，有二次确认。
   - “游戏玩法介绍”可见，能返回，不消耗解放战入口。
3. 解放战准备阶段验证：
   - 可用角色应为 5 人。
   - 可选核心页/战斗书页只来自重置后的永久图鉴和默认初始 fallback，不再从旧 `Lastest` 路线库存回灌。
4. 异想体路线验证：
   - 不应再选到缺 `mapInfo` 的原版异想体 Stage。
   - 若仍突然结束，立刻检查 `Player.log` 中 `[RMR AbnoRoute] picked=...` 与 `StageController.InitializeMap` 附近异常。
5. 若游戏内仍显示只有 3 人，下一步需要继续反射确认 `StageWaveInfo` 真实可用人数成员名，当前补丁已尝试 `availableUnit`、`_availableUnit`、`AvailableUnit` 字段/属性。

本轮 2026-07-01 已处理并部署

重要追加修复（2026-07-01 23:58）

- 用户最新要求：
  - 都市之星的殷红迷雾关卡不要每次固定出现，改为约 60% 概率遇到。
  - 杂质 Boss 首通奖励要变成跨路线永久解锁：漆黑静默首通后以后到都市之星自动给漆黑静默核心页；扭曲残响乐团首通后以后到都市之星自动给扭曲苍蓝残响核心页和战斗书页。
  - 异想体战不要按楼层段随机到原版后期战斗，改为按原版异想体战可上场司书人数限制到对应 RMR 进度。
  - 初始遗物选择界面增加“重置所有永久进度”和“游戏玩法介绍”入口。
- 已调整都市之星路线生成：
  - 第六章殷红迷雾挑战 `60020` 保留为 Elite 候选，但路线生成时按 `0.6f` 概率保留。
  - 未抽中时把 Elite 需求降为 0，并把 Normal 需求 +1，避免路线少一个节点。
- 已调整杂质 Boss 首通持久化：
  - 漆黑静默首通继续写入 `RMR_BlackSilenceStageCleared`。
  - 扭曲残响乐团胜利现在写入 `RMR_DistortedEnsembleStageCleared`。
  - 到达都市之星时，漆黑静默和扭曲苍蓝残响奖励分支互不阻塞。
  - 扭曲苍蓝残响自动授予核心页 `250013` 与战斗书页 `604021`-`604025`。
- 已调整异想体战 Stage 路由：
  - 移除旧的 Low/Mid/High 楼层段候选池。
  - 现在使用原版普通异想体 Stage ID 后缀推导所需司书数量：`xxx001` 至 `xxx004` 对应 1 至 4 人。
  - RMR 进度允许人数：传闻 1、都市怪谈 2、都市传说 3、都市恶疾 4、都市梦魇及以后 5。
- 已调整初始遗物选择事件：
  - `RMR_chstart` Frame 0 新增 Choice 7：重置所有永久进度。
  - Choice 7 进入二次确认 Frame 2，确认后清空永久图鉴、楼层解放记录、红雾挑战记录、漆黑静默首通记录、扭曲残响乐团首通记录。
  - Frame 0 新增 Choice 8：游戏玩法介绍。
  - 补齐 `cn/en/kr` 的 `MysteryEvents/RMR_chstart.xml` 与 `Mystery_Start.txt` 键。
- 新增静态检查：
  - `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
- 已更新 `RMR_Core.BuildTimestamp` 为 `2026-07-01T23:57+08:00`。
- 已重新 Release 编译并部署到 Workshop：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\SpecialStaticInfo\MysteryXmlInfos\RMR_chstart.xml`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\Localize\cn\MysteryEvents\RMR_chstart.xml`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\Localize\en\MysteryEvents\RMR_chstart.xml`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\Localize\kr\MysteryEvents\RMR_chstart.xml`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\Localize\cn\Mystery_Start.txt`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\Localize\en\Mystery_Start.txt`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\Localize\kr\Mystery_Start.txt`
- 部署前备份目录：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260701_235829`
- 部署哈希核对通过：
  - DLL SHA-256: `71C5E7B5A20039B61AD3D1594F76A8AD0F7479E0DAE408EF1EB129C9D6E57C4C`
  - `RMR_chstart.xml`: `94E4304EDE91B4D7A3B3FD10E00D6D34EEA07805846A25E481B76E7ED22CEFFA`
  - `Localize/cn/MysteryEvents/RMR_chstart.xml`: `899D308AE238CBF67875BA573E52E7CE2475762459ECADACE608726ACA575D37`
  - `Localize/en/MysteryEvents/RMR_chstart.xml`: `C073EDF35D041E880F53B93C6E5D8C7462D097E6EE5F2750A24354A86881DD22`
  - `Localize/kr/MysteryEvents/RMR_chstart.xml`: `F4A28B7812D1A6BF49A22F3DADD27191D24DD99176340244DD96B8ACBF03EF41`

本轮 23:58 已验证

- XML 解析通过：
  - `SpecialStaticInfo/MysteryXmlInfos/RMR_chstart.xml`
  - `Localize/cn/MysteryEvents/RMR_chstart.xml`
  - `Localize/en/MysteryEvents/RMR_chstart.xml`
  - `Localize/kr/MysteryEvents/RMR_chstart.xml`
  - `Localize/cn/Mystery_Start.txt`
  - `Localize/en/Mystery_Start.txt`
  - `Localize/kr/Mystery_Start.txt`
- 静态检查通过：
  - `tools/static_checks/events_abnormality/RMR_0701_progression_reset_abno_rules_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_binah_route_ego_toggle_grade7_static_check.ps1`
- Release 编译通过，仍只有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- Workshop 部署哈希全部 `HASH_MATCH=True`。

本轮 23:58 还没做，留给游戏内实测后继续

1. 启动游戏后先在 `Player.log` 确认加载了本轮 DLL：
   - 查找 `[RMR] RogueLike Mod Reborn initializing. Build: 2026-07-01T23:57+08:00`
2. 游戏内验证第六章都市之星路线：
   - 殷红迷雾挑战不是每次固定出现，约 60% 路线出现。
   - 未出现时路线不应少节点，应多一个普通战替代 Elite 槽位。
3. 游戏内验证杂质 Boss 首通跨路线奖励：
   - 初次通关漆黑静默后，下一次到都市之星自动获得漆黑静默核心页。
   - 初次通关扭曲残响乐团后，下一次到都市之星自动获得扭曲苍蓝残响核心页与 `604021`-`604025`。
   - 两个首通奖励互不阻塞。
4. 游戏内验证异想体战：
   - 传闻只应进入原版 1 人异想体战候选。
   - 都市怪谈允许 2 人及以下，都市传说允许 3 人及以下，都市恶疾允许 4 人及以下，都市梦魇以后允许全部普通异想体战候选。
5. 游戏内验证初始事件：
   - 初始遗物选择界面显示“重置所有永久进度”和“游戏玩法介绍”。
   - 玩法介绍可返回，不消耗初始入口。
   - 重置需要二次确认；确认后永久图鉴、解放战记录和特殊 Boss 首通记录清空。
6. 仍存在的非本轮阻塞项：
   - `git diff --check` 仍因既有 `Localize/cn/LogueEffectText/GlobalEffect.xml` 尾随空格失败。
   - `tools/static_checks/runtime_release/RMR_release_static_check.ps1` 仍会因中文 Buff 名/描述中残留英文而失败；这是旧本地化问题，不属于本轮功能修复。

重要追加修复（2026-07-01 17:49）

- 用户反馈最新问题：
  - 杂质部分战斗书页/核心书页描述不对。
  - 杂质普通关卡没有残响乐团战斗。
  - Boss 不应是扭曲残响乐团单层，应是完整漆黑静默或完整三波扭曲残响乐团。
  - 解放战应可用 5 人，且核心页选择必须来自永久图鉴解锁，不应把未解锁内容全部放进去。
- 已调整第七章杂质关卡池：
  - `70001`-`70010` 改为十场原版残响乐团成员战，并作为普通关卡进入路线池。
  - `70020` 改为完整漆黑静默 Boss 流程。
  - `70021` 改为完整三波扭曲残响乐团 Boss 流程。
  - `70008`/`70009`/`70010` 不再作为拆开的 Boss 层使用。
- 已调整杂质奖励池：
  - 战斗书页掉落改为原版残响乐团 `704001`-`704016`、`704018`，每张卡显式使用 `Pid="@origin"`，避免显示模组内错误/缺失描述。
  - 核心页奖励改为原版残响乐团核心页 `260005`-`260014`，`WorkShopID="@origin"`，避免继续使用 Hana/Olivier 池。
- 已修解放战图鉴污染根因：
  - `RMR_RealizationManager.EnsureDefaultRealizationAtlasUnlocks()` 现在只调用 `SavePermanentAtlasData()` 保存默认初始核心页/初始牌 fallback。
  - 不再调用会同步当前路线库存的图鉴保存路径，避免进入解放战时把未真正永久解锁的当前路线书页写入永久图鉴。
  - 保留解放战临时队伍补足 5 人，且临时核心页/战斗书页列表从永久图鉴集合构造。
- 已更新对应静态脚本断言：
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0621_reported_runtime_regressions_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_binah_route_ego_toggle_grade7_static_check.ps1`
  - `tools/static_checks/runtime_release/RMR_release_static_check.ps1` 中商店异想体去重断言已对齐当前真实路径。
- 已更新 `RMR_Core.BuildTimestamp` 为 `2026-07-01T17:48+08:00`。
- 已重新 Release 编译并部署到 Workshop：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\AddData\StageInfo\StageInfo_ch7.xml`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\SpecialStaticInfo\StagesXmlInfos\Stage_ch7.xml`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\AddData\CardDropTable\CardDropTable_ch7.xml`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\SpecialStaticInfo\RewardPassiveInfos\EquipReward_ch7.xml`
- 部署前备份目录：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260701_174917`
- 部署哈希核对通过：
  - DLL SHA-256: `F31DF6DDDC0FECF999CD37E4673E5D9C8F8149AF9B598A8A5AAD0E67CB011CCE`
  - `StageInfo_ch7.xml`: `5CC23DAED81E92245735D6B97F1D03380C3A95E2C540D04B49ADFA1678580429`
  - `Stage_ch7.xml`: `63BC9FCD226EAE9E0DD47708D7D78079382CBC60CEF48E843E36860C0E4986C1`
  - `CardDropTable_ch7.xml`: `9E5AD46710C1D979419289C68A05C17AF74D1CFB5229948488185F21CB6DAC55`
  - `EquipReward_ch7.xml`: `088CAB996AE1C67A848C862FFA1CC18DE07847928D8C75D69ECC58C5A302F59C`

本轮 17:49 已验证

- XML 解析通过：
  - `AddData/StageInfo/StageInfo_ch7.xml`
  - `SpecialStaticInfo/StagesXmlInfos/Stage_ch7.xml`
  - `AddData/CardDropTable/CardDropTable_ch7.xml`
  - `SpecialStaticInfo/RewardPassiveInfos/EquipReward_ch7.xml`
- 静态检查通过：
  - `tools/static_checks/runtime_release/RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0628_user_reported_rewards_static_check.ps1`
  - `tools/static_checks/rewards/RMR_0621_reported_runtime_regressions_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_binah_route_ego_toggle_grade7_static_check.ps1`
  - `tools/static_checks/realization/RMR_0620_grade6_special_fixed_deck_static_check.ps1`
- Release 编译通过，仍只有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- Workshop 部署哈希全部 `HASH_MATCH=True`。

本轮 17:49 还没做，留给游戏内实测后继续

1. 启动游戏后先在 `Player.log` 确认加载了本轮 DLL：
   - 查找 `[RMR] RogueLike Mod Reborn initializing. Build: 2026-07-01T17:48+08:00`
2. 游戏内验证杂质普通关卡：
   - 第七章普通战应出现/进入残响乐团十场成员战，而不是 Hana-only 普通战。
   - 战斗书页目录/奖励中应显示残响乐团 `704xxx` 原版描述。
   - 核心页奖励中应显示残响乐团 `260005`-`260014` 原版描述。
3. 游戏内验证 Boss：
   - Boss 候选应是完整漆黑静默 `70020` 或完整三波扭曲残响乐团 `70021`。
   - 不应再进入拆开的 70008/70009/70010 单层 Boss。
4. 游戏内验证解放战：
   - 解放战准备界面应可用 5 人。
   - 可选核心页应只来自永久图鉴解锁集合和默认初始 fallback。
   - 如果旧存档已经被之前版本污染，可能需要额外写一次旧图鉴清理/迁移，把错误写入 `RMR_AtlasPermanentUnlocks` 的未解锁书页移除。
5. 仍存在的非本轮阻塞项：
   - `git diff --check` 仍因既有 `Localize/cn/LogueEffectText/GlobalEffect.xml` 尾随空格失败。
   - `tools/static_checks/runtime_release/RMR_release_static_check.ps1` 仍会因中文 Buff 名/描述中残留英文而失败；这是旧本地化问题，不属于本轮杂质/解放战功能修复。

重要追加修复（2026-07-01 17:12）

- 用户实测确认 16:58 DLL 仍显示默认角色战斗书页。
- 复查 `Player.log` 发现最新 DLL 已加载，但日志显示实际应用的是 `LorId(:1)`，牌组为 `2/3/4/5` 默认牌组：
  - `[RMR] Applied fixed built-in deck source LorId(:1) ... LorId(:2),LorId(:2),LorId(:3),...`
- 根因：`TryResolveGrade6SpecialBuiltInDeckSource` 先读取 `page.DeckId`，该属性会被 Roguelike 默认牌组 hook 替换成 `DeckId=1`；Binah/漆黑静默的显式 fallback Deck 8/102 位于后面，永远没有机会生效。
- 已修复为优先识别已知原版固定牌组：
  - Binah 先走 Deck 8。
  - 漆黑静默先走 Deck 102。
  - 其他固定牌组才继续使用 `page.DeckId` / `OnlyCard` 路径。
- 已更新静态脚本，新增“已知原版固定牌组 fallback 必须先于 hooked DeckId”的断言。
- 已更新 `RMR_Core.BuildTimestamp` 为 `2026-07-01T17:11+08:00`。
- 已重新 Release 编译并部署 DLL：
  - DLL SHA-256: `5A6051D3F1899B191A88D66CC12A13D12B93D770891BFC5F8002B653271516C4`
  - 备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260701_171228`

目录整理（2026-07-01 17:12）

- 根目录的历史 PowerShell 静态检查脚本已归档到 `tools/static_checks/`：
  - `realization/`
  - `rewards/`
  - `shop_atlas/`
  - `events_abnormality/`
  - `runtime_release/`
- `pack_mod.ps1` 已移动到 `tools/packaging/pack_mod.ps1`，后续打包输出进入 `_release_packages/archives/`。
- 根目录历史 zip 包已移动到 `_release_packages/archives/`。
- 已新增 `tools/static_checks/README.md` 并更新 `AGENTS.md`，说明这些脚本/zip 的来源、分类和运行方法。

重要追加修复（2026-07-01 16:59）

- 用户实测确认 Binah / 漆黑静默装备核心页后，游戏内仍显示默认角色战斗书页，而不是专属固定战斗书页。
- 复查发现上一轮只补了固定牌组来源识别和 hook fallback；`EquipNewPage` 装备核心页时没有把当前单位的真实 `DeckModel._deck` 替换为固定牌组，后续 UI/存档路径仍可能看到默认牌组。
- 已新增装备/读档共用的固定牌组应用路径：
  - Binah / 漆黑静默核心页装备后，直接把真实当前牌组替换为对应 9 张固定战斗书页。
  - 读档恢复 `Grade6SpecialBuiltInDeckSource` 时也会重新应用真实牌组。
  - 固定牌组应用成功后跳过普通牌组合法性清理，避免固定牌组被 `IsFixedDeck()` 路径移除。
- 已更新 `RMR_Core.BuildTimestamp` 为 `2026-07-01T16:58+08:00`。
- 已重新 Release 编译并部署 DLL：
  - DLL SHA-256: `369784984703B1EB04CD155FD2F88B0A94D1D42D3D241186F324840980621BCC`
  - 备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260701_165934`

重要追加修复（2026-07-01 16:49）

- 用户实测后确认第七章仍然 `CreateUnitBattleDataByEnemyUnitId` 空引用。
- 复查发现上一轮确实部署到了 Workshop，但第七章敌人 ID 被还原为 `LorId("@origin", id)`；反射确认 `@origin` 在运行时仍是 Workshop ID，`LorId(id)` 才是原版基础 ID。
- 已将第七章 70001-70010 的敌方单位 ID 还原改为 `new LorId(id)` / 空包，并在 `SetNextStage` 前再次规范化。
- 已新增运行日志：`[RMR SetNextStage] Grade7 impurity wave normalized: stage=..., enemies=...`，用于确认实际传给 `StageWaveModel.Init` 的敌人 ID。
- 已更新 `RMR_Core.BuildTimestamp` 为 `2026-07-01T16:47+08:00`，方便确认游戏加载了本轮 DLL。
- 已重新 Release 编译并部署：
  - DLL SHA-256: `BCA37A5F23C010B04A3215ECC5B6B019E1704300F77AAF2C42F771F5C2FDBF81`
  - 备份目录：`E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260701_164910`

1. 第七章杂质关卡点击失败
   - 已修 `StageWaveInfo` 原版敌人 ID 还原：`LorIdXml.pid/xmlId` 属性现在也会被还原为 `@origin`，并覆盖 70001-70010。
   - 已调整第七章 Stage：
     - 70001-70006：单幕普通杂质战。
     - 70007：漆黑静默 Boss。
     - 70008/70009/70010：扭曲残响乐团低/中/高三段单幕 Boss。
     - 移除 Head 终局战作为第七章 Boss 候选。
   - 已同步扭曲残响乐团胜利识别范围：70008-70010 都会触发对应奖励记录。

2. Binah / 漆黑静默固定默认书页
   - 已按 `D:\LoR_mods` 图片和原版 `Deck_basic.txt` 增加显式固定卡组 fallback。
   - Binah fallback：607201, 607202 x3, 607203, 607204 x2, 607205 x2。
   - 漆黑静默 fallback：702001-702009 各 1。
   - 即使原版 Deck 8/102 没被运行时取到，也不会回退成普通默认角色战斗书页。

3. 殷红迷雾获取时机
   - 已新增 `RMR_RedMistChallengeCleared` 胜利标记。
   - 未记录胜利前，会清理旧存档/图鉴中提前获得的殷红迷雾核心页 250022 和战斗书页 607003-607007，并把已装备的殷红迷雾核心页回退到默认页。
   - 真正打赢 60020 殷红迷雾挑战后才写入胜利标记并发放核心页和战斗书页。

本轮已验证

- XML 解析通过：
  - `AddData/StageInfo/StageInfo_ch7.xml`
  - `SpecialStaticInfo/StagesXmlInfos/Stage_ch7.xml`
- 静态检查通过：
  - `RMR_0628_runtime_end_and_ch7_static_check.ps1`
  - `RMR_0620_binah_route_ego_toggle_grade7_static_check.ps1`
  - `RMR_0620_binah_red_mist_progression_static_check.ps1`
  - `RMR_0620_grade6_special_fixed_deck_static_check.ps1`
  - `RMR_0620_red_mist_challenge_static_check.ps1`
- Release 编译通过，只有既有 warning：`RMREffect_Duplicator.Dupe.cards` 未赋值。
- 已部署到 Workshop：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\RogueLike Mod Reborn.dll`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\AddData\StageInfo\StageInfo_ch7.xml`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\dlls\SpecialStaticInfo\StagesXmlInfos\Stage_ch7.xml`
- 部署前备份：
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260701_120155`
  - `E:\Steam\steamapps\workshop\content\1256670\3503523710\Assemblies\_codex_backups\codex_20260701_164910`

下一步还没做，留给实测后继续

1. 启动游戏后先在 `Player.log` 确认加载了本轮部署 DLL：
   - 查找 `[RMR] RogueLike Mod Reborn initializing. Build: 2026-07-01T17:11+08:00`
   - 如果仍显示 `2026-06-29T23:50+08:00`，说明游戏没有加载最新 DLL。
2. 游戏内验证第七章：
   - 第七章普通战 70001-70006 是否能点击进入。
   - Boss 候选 70007-70010 是否能点击进入。
   - 不应再进入 Head 终局战。
   - 若仍黑屏或点击无效，记录 `Player.log` 中 `[RMR SetNextStage] Grade7 impurity wave normalized`、`StageWaveModel.Init`、`CreateUnitBattleDataByEnemyUnitId` 附近异常。
3. 游戏内验证固定牌组：
   - Binah 装备核心页后应显示/使用 607201, 607202 x3, 607203, 607204 x2, 607205 x2。
   - 漆黑静默装备核心页后应显示/使用 702001-702009 各 1。
4. 游戏内验证殷红迷雾：
   - 未打赢 60020 前，图鉴/核心页选择/战斗书页目录不应可装备殷红迷雾核心页和 607003-607007。
   - 打赢 60020 后才获得 250022 和 607003-607007。
   - 如果旧存档本来已有错误奖励，本轮进入第六章/选择挑战时应先被清理。
