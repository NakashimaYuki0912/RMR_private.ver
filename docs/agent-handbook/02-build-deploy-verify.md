# 构建、部署与验证

## 1. 工具链

| 工具 | 本机路径 / 说明 |
|---|---|
| MSBuild | `D:\VisualStudio\MSBuild\Current\Bin\MSBuild.exe` |
| 项目文件 | `RogueLike Mod Reborn.csproj`（.NET Framework 4.8） |
| 输出 DLL | `bin\Release\RogueLike Mod Reborn.dll` |
| 一键部署 | `tools\packaging\deploy_workshop.ps1` |
| 打 zip | `tools\packaging\pack_mod.ps1` |
| Steam | `E:\Steam\steam.exe` |
| 游戏日志 | `%USERPROFILE%\AppData\LocalLow\Project Moon\LibraryOfRuina\Player.log` |

`dotnet msbuild` 在部分环境可用；本仓库推荐 `deploy_workshop.ps1`（内含 MSBuild 候选路径）。

---

## 2. 标准部署（日常开发）

```powershell
cd "D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main"

# 游戏必须完全退出
Get-Process LibraryOfRuina -ErrorAction SilentlyContinue

powershell -ExecutionPolicy Bypass -File .\tools\packaging\deploy_workshop.ps1 -Configuration Release
```

脚本会：

1. Release 构建  
2. 备份旧 Workshop DLL 到 `Assemblies\_codex_backups\deploy_*`  
3. 复制 DLL + 同步 `AddData` / `Localize` / `SpecialStaticInfo` / `StoryInfo` / `ArtWork`  
4. 打印 SHA256 与从 DLL 字符串扫出的 `Build:` 戳  

**目标路径：**

```text
E:\Steam\steamapps\workshop\content\1256670\3743867841\Assemblies\dlls\RogueLike Mod Reborn.dll
```

### 映射表

| 源码 / 构建 | Workshop 运行位置 |
|---|---|
| `bin\Release\RogueLike Mod Reborn.dll` | `...\Assemblies\dlls\RogueLike Mod Reborn.dll` |
| `AddData\**` | `...\Assemblies\dlls\AddData\**` |
| `SpecialStaticInfo\**` | `...\Assemblies\dlls\SpecialStaticInfo\**` |
| `Localize\**` | `...\Assemblies\dlls\Localize\**` |
| `ArtWork\**` | `...\Assemblies\dlls\ArtWork\**` |
| `StoryInfo\` 等 | `...\Assemblies\dlls\` 下同名目录 |

---

## 3. 改 Build 戳（强制）

每次准备给用户测的构建，更新：

```csharp
// RMR_Core.cs
public const string BuildTimestamp = "YYYY-MM-DDT简短说明+08:00";
```

初始化日志：

```csharp
Debug.Log($"[RMR] RogueLike Mod Reborn initializing. Build: {BuildTimestamp}. ...");
```

用户 / agent 验收：

```powershell
Select-String -Path "$env:USERPROFILE\AppData\LocalLow\Project Moon\LibraryOfRuina\Player.log" `
  -Pattern "\[RMR\].*Build:"
```

**Build 字符串对不上 → 测试作废。**

---

## 4. 打包上传用 zip

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\packaging\pack_mod.ps1
```

- 从 **Workshop 运行树** 复制（不是只从源码树）  
- 删除 `*.bak`、`_codex_backups`、`DevNuggets` 等  
- 输出：`_release_packages\archives\RougelikeModReborn_v时间戳.zip`  

打包前应先 `deploy_workshop.ps1`，保证运行树 = 当前源码意图。

**Steam Workshop 上传**必须由用户在已登录的 Steam 客户端完成；agent 不能代登账号上传。  
见用户交接说明：作者物品页 `https://steamcommunity.com/sharedfiles/filedetails/?id=3743867841`（勿更新上游原作 `3503523710`）。

---

## 5. 验证等级（汇报时必须标明）

| 等级 | 含义 | 能否说「修好了」 |
|:---:|---|---|
| 1 | 源码读过 / 逻辑合理 | 否 |
| 2 | 静态脚本通过 | 否 |
| 3 | Release 编译通过 | 否 |
| 4 | 部署哈希一致 + Player.log Build 匹配 | 否（未玩） |
| 5 | 游戏内点击复现路径通过 | **可以针对该路径** |

禁止未达 5 却写「游戏内已验证无误」。

---

## 6. 静态检查

```text
tools/static_checks/
  realization/        解放战、固定牌组
  rewards/            奖励、E.G.O.、Boss 池
  shop_atlas/         商店、图鉴
  events_abnormality/ 事件、异想体
  runtime_release/    启动、打包、语言
```

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File `
  .\tools\static_checks\realization\RMR_0620_grade6_special_fixed_deck_static_check.ps1
```

脚本失败时先判断：真回归 / 断言过时 / 脚本编码问题。  
**禁止**为让旧脚本变绿而回退正确设计。

---

## 7. 运行时扫描警告

`Assemblies\dlls` 历史上有大量 `*.bak`。  
任何 `Directory.GetFiles` **必须**限制扩展名（`*.xml`），禁止无过滤枚举后当配置加载。

---

## 8. 日志过滤建议

```powershell
Select-String -Path "...\Player.log" -Pattern "\[RMR|NullReference|Exception|DefFont|Build:|Emotion pick|NonCombatNodeExit"
```

有用前缀：

- `[RMR]` 总初始化  
- `[RMR Localize]` 字体  
- `[RMRAbnormalityUnlockManager]` 异想体层 / 池  
- `[RMR] NonCombatNodeExit` 商店/神秘离开  
