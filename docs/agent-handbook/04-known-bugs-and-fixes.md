# 已知问题与已修复清单（2026-07 会话）

> 目的：后续 agent **不要重开已修的洞**，也不要把症状当新需求乱改。

---

## A. 本地化 / 字体

| 症状 | 根因 | 状态 | 关键代码 |
|---|---|---|---|
| 中文 口口口 | 错误字体 / Fallback / 编码 | 持续加固 | `LogLikeMod` 字体管线 |
| Hub 字糊 | Bold 伪粗体 + 材质 | **已修** | `RMR_StartHubPanel` |
| 解放战选层字糊 | 同上 | **已修** | `LogRealizationPanel` |
| 玩法介绍 / 图鉴糊 | 裸 `font=` | **已修** | `RMR_HelpHandbookPanel`、`LogAtlasPanel` |
| 商店/神秘按钮 | CreateText_TMP | **已走锐利路径** | `ModdingUtils` |
| 异想体名 触须类 口口口 | script 未映射 vanilla Name | **已修一批** | `ModScriptToVanillaScript` |
| 开场 PV / 编队名 tofu | OpeningLyrics 编码 + slot 字体 | **已修** | `LogLikePatches` / `LogLikeMod` |
| 「当前层限制：没有可显示的核心书页」 | 空库存弹窗 | **已静默** | `NotifyInventoryEmptyIfNeeded` |

---

## B. 情感 / 异想体 / E.G.O.

| 症状 | 根因 | 状态 |
|---|---|---|
| 情感 1 出现「旋律」+「今日的表情」 | 模组 script 查不到 EmotionLevel → 当 Tier I | **已修**：别名 + 静态种子 + 未知排除 |
| 中段 E.G.O. 未拥有可选 | 池错误 | **已修**：仅 owned |
| 手牌全是 E.G.O. 只能 Pass | 开局 bulk grant / 默认手 | **已修** |
| 情感满自动胜利 / 清场 | 误 EndBattle / OnPickEgo | **已修** guard |
| SetCardsObject 越界 | sprite 数组短 | **已修** 安全路径 |

### 科技层终局三选一（EmotionLevel 3，队伍情感 5）

| 中文（玩家说法） | 模组/原版 script |
|---|---|
| 音乐 / 旋律 | `SingingMachine1` → `singingMachine` |
| 棺柩 | `Butterfly3` → `butterfly3` |
| 黑炎 | `freischutz3` |

**注意：** 原版 `bluestar3` 英文名也可能译「旋律」，但是 **Hokma / EmotionLevel 2**，不是科技层终局。看描述里「节奏」等判定是歌唱机。

---

## C. 流程软锁

| 症状 | 根因 | 状态 |
|---|---|---|
| 商店/神秘选完下一层，卡在本场，空格开战但商人免疫 | 残留 NPC + 类型已变 Normal + EndBattle 恢复 | **已修** `NonCombatNodeExitPending` |
| 双方都活却进结算 | 情感 E.G.O. glitch | **已修** recovery |

---

## D. 商店布局

| 症状 | 状态 |
|---|---|
| 战斗书页与中间物品空隙过大 | **已修** `ShopBase.CardShape` Y 下移 ~60，passive 偏移同步 |

---

## E. 仍可能残留 / 需游戏内复验

- 全语言 UI 是否仍有 **裸 `font=`** 的漏网自定义控件（改 UI 时再扫）  
- 极端分辨率下商店重叠  
- 图鉴「显示升级版战斗书页」开关（见根目录 `handoff.md` 历史需求）  
- 与第三方 UI 模组（如 BCEV）共存 NRE  
- Workshop 目录大量 `.bak` 若被错误扫描加载  

---

## F. 关键 script 别名（节选）

完整表：`RMR_AbnormalityUnlocks.ModScriptToVanillaScript`

| 模组 script | 原版 script |
|---|---|
| `ShyLookToday1/2/3` | `shylook` / `shylook2` / `shylook3` |
| `SingingMachine1/2/3` | `singingmachine`… |
| `UniverseZogak2` | `fragmentspace2`（触须类） |
| `HeartofAspiration3` | `doki` |
| `Mountain1/2/3` | `danggocreature*` |
| `Clock1/2/3` | `silence*` |

查层：`GetVanillaAbnoTierForScript` → `EnsureVanillaEmotionTiersLoaded` + 静态种子。

---

## G. 用户验收时请其重启

热重载无效。必须：

1. 完全退出 Library of Ruina  
2. `deploy_workshop.ps1`  
3. 启动后看 `Player.log` 的 `Build:`  
