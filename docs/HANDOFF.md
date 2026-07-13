# LoR-RMR 会话交接（2026-07-12）

> 新会话请先读 [AGENTS.md](../AGENTS.md) 与 [agent-handbook/00-INDEX.md](./agent-handbook/00-INDEX.md)。  
> 本文是**快照**，以 `git status` 与 `RMR_Core.BuildTimestamp` 为准。

---

## 1. 工作区

```text
源码 / git:
  D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\

Workshop 运行:
  E:\Steam\steamapps\workshop\content\1256670\3743867841\Assemblies\dlls\

Player.log:
  %USERPROFILE%\AppData\LocalLow\Project Moon\LibraryOfRuina\Player.log
```

- 外层 `D:\VS_program\ruina-roguelike-reborn-main\` **没有** `.git`。  
- 原作者对照：`D:\VS_program\ruina-roguelike-reborn-main\original-codes\`（只读）。

---

## 2. 最近用户目标（本对话脉络）

1. 中文从「口口口」修到「能显示」后，仍 **糊到看不清** → 去伪粗体 + 材质。  
2. 删除无用提示：「当前层限制：没有可显示的核心书页…」。  
3. 商店 / 神秘选完下一层卡在本场（奇悭球体免疫）→ `NonCombatNodeExitPending`。  
4. 低情感出现科技终局「旋律」+「今日的表情」→ EmotionLevel 过滤 + script 别名。  
5. 商店战斗书页下移。  
6. 扫全项目同类糊字风险并修。  
7. **文档交接**（AGENTS / README / handbook）。  
8. Steam 上传：agent **不能**代传；可打包。

---

## 3. 代码状态（交接时注意）

| 项 | 说明 |
|---|---|
| Commit | `3291d33` Fix CN UI sharpness, abno emotion tiers, and shop/mystery softlocks |
| 可能未提交 | CJK 全扫：`LogRealizationPanel`、`RMR_HelpHandbookPanel`、`LogAtlasPanel`、`ModdingUtils.CreateText_TMP`、`LogLikePatches` 等；**以 `git status` 为准** |
| Build 戳 | `RMR_Core.BuildTimestamp` 当前设计为 `2026-07-12Tfix-cjk-sharp-sweep+08:00` |
| 部署 | 曾成功 `deploy_workshop.ps1`；用户须 **完全重启** 游戏验证 |

未提交改动请 **先 status 再决定 commit**，勿覆盖用户其他工作。

---

## 4. 关键实现入口

| 主题 | 文件 / API |
|---|---|
| 字体锐利 | `LogLikeMod.ApplyTmpFontPreservingSharpMaterial` |
| Hub UI | `RMR_StartHubPanel` |
| 情感层 | `RMRAbnormalityUnlockManager.GetVanillaAbnoTierForScript` / `IsOwnedPageEligibleForTeamEmotion` |
| 非战斗离开 | `RewardingModel.MarkNonCombatNodeExit` / `NonCombatNodeExitPending` |
| 商店布局 | `ShopBase.CardShape` |
| 空提示 | `RMRPrepareRestrictions.NotifyInventoryEmptyIfNeeded`（空实现） |
| 部署 | `tools/packaging/deploy_workshop.ps1` |

---

## 5. 建议的下一会话任务

1. `git status`：提交 CJK 全扫 + 文档（若用户要求）。  
2. 游戏内回归清单（Build 戳对齐后）：  
   - Hub / 解放选层 / 图鉴中文清晰  
   - 情感 1–2 无 III 级页  
   - 商店离开进下一场  
   - 中段 E.G.O. 仅已拥有  
3. （可选）打 `pack_mod.ps1` zip，用户自行 Workshop 更新。  
4. （可选）图鉴战斗书页「显示升级版」开关——见根目录 `handoff.md` 历史需求。  
5. 清理 Workshop 下大量 `.bak`（上传前 `pack_mod` 会剥，但运行树仍乱）。

---

## 6. 验证命令

```powershell
cd "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"
git status -sb
Select-String -Path "$env:USERPROFILE\AppData\LocalLow\Project Moon\LibraryOfRuina\Player.log" -Pattern "Build:|DefFont_TMP"
```

---

## 7. Suggested skills

- `systematic-debugging`：日志异常 / 软锁  
- `verification-before-completion`：声称完成前  
- `check-work`：构建与 diff 复查  

---

*更新本文时请改日期，并同步 AGENTS.md §10。*
