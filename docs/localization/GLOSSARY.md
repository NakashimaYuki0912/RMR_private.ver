# RMR terminology glossary (CN / EN / KR)

Use this table when translating. **Do not invent alternate names** for the same concept unless the glossary is updated.

## Core product

| Concept | 中文 | English | 한국어 | Notes |
|---------|------|---------|--------|-------|
| Mod name | Roguelike Mod Reborn / RMR | Roguelike Mod Reborn / RMR | Roguelike Mod Reborn / RMR | Branding; keep “RMR” |
| Package / unique id | — | `abcdcodecalmmagma.LogueLikeReborn` | — | Not user-facing |
| Start hub | 开局菜单 / Roguelike 开局 | Start hub / Roguelike Start | 시작 메뉴 | Full-screen after RMR entry |

## Permanent collection vs run inventory

| Concept | 中文 | English | 한국어 | Notes |
|---------|------|---------|--------|-------|
| Permanent collection UI | **图鉴** | **Compendium** | **도감** | **Not** “Atlas” in EN. Code/ids may still say Atlas. |
| Permanent collection (full) | 永久图鉴 | Permanent Compendium | 영구 도감 | Separated from run inventory |
| Run inventory | 当前路线库存 / 本局持有 | Current-run inventory | 현재 루트 보유 | Cleared on new run |
| Reset permanent progress | 重置永久进度 | Reset Permanent Progress | 영구 진행 초기화 | Wipes compendium + realization clears |

## Journey / nodes

| Concept | 中文 | English | 한국어 | Notes |
|---------|------|---------|--------|-------|
| Reception (node fight) | 接待 | Reception | 접대 | Vanilla LoR term |
| Normal battle | 普通战斗 / 普通战 | Normal battle | 일반 전투 | |
| Elite battle | 精英战斗 / 精英战 | Elite battle | 엘리트 전투 | |
| Boss battle | Boss 战 | Boss battle | 보스 전투 | |
| Abnormality battle | 异想体战斗 / 异想体战 | Abnormality battle | 환상체 전투 | |
| Shop | 商店 | Shop | 상점 | |
| Rest | 休息室 / 休息 | Rest / Resting room | 휴식실 | |
| Mystery event | 随机事件 / 神秘事件 | Mystery event | 이벤트 | |
| Continue run | 继续旅程 | Continue Run | 이어하기 | Shown when `Lastest` save exists |
| Normal Play (new run) | 正常游玩 | Normal Play | 일반 플레이 | Starts new run; overwrites continue |

## Urban stages (chapters)

Match vanilla LoR English names:

| Grade | 中文 | English | 한국어 |
|-------|------|---------|--------|
| G1 | 传闻 | Canard | 소문 |
| G2 | 都市怪谈 | Urban Myth | 도시 괴담 |
| G3 | 都市传说 | Urban Legend | 도시 전설 |
| G4 | 都市恶疾 | Urban Plague | 도시 질병 |
| G5 | 都市梦魇 | Urban Nightmare | 도시 악몽 |
| G6 | 都市之星 | Star of the City | 도시의 별 |
| G7 | 杂质 | Impurity / Impuritas Civitatis | 불순물 |

## Pages & combat

| Concept | 中文 | English | 한국어 | Notes |
|---------|------|---------|--------|-------|
| Key page | 核心书页 / 角色书页 | Key page | 핵심 책장 | Equip page |
| Combat page | 战斗书页 | Combat page | 전투 책장 | |
| Passive | 被动 / 被动能力 | Passive | 패시브 | |
| Abnormality page | 异想体书页 | Abnormality page | 환상체 책장 | Emotion-offerable pages |
| E.G.O. page | E.G.O. / EGO 战斗书页 | E.G.O. page | E.G.O. 책장 | Prefer **E.G.O.** with dots in EN |
| Ahn / money | 眼 | Ahn | 안 | RMR “money” currency label |
| Emotion level | 情感等级 | Emotion level | 감정 레벨 | Team / unit |
| Abnormality tier I/II/III | I / II / III 阶异想体书页 | Tier I / II / III abnormality pages | I/II/III 단계 | Tied to emotion 1–2 / 3–4 / 5 |

## Realization

| Concept | 中文 | English | 한국어 | Notes |
|---------|------|---------|--------|-------|
| Floor Realization | 解放战 / 楼层解放 | Floor Realization / Realization | 개방전 / 층 개방 | Vanilla “Floor Realization” |
| Challenge Realization | 挑战解放战 | Challenge Realization | 개방전 도전 | Hub button |
| Realization exclusive abno | 解放专属异想体书页 | Realization-exclusive abnormality pages | 개방전 전용 환상체 책장 | Locked until floor clear |
| First clear | 首通 / 首次通关 | First clear | 첫 클리어 | Permanent unlock only once |

## Library floors (sephirot short names)

| Sephirah | 中文 | English (RMR short) | Notes |
|----------|------|---------------------|-------|
| Malkuth | 历史 | History | Vanilla floor name |
| Yesod | 科技 | Tech | |
| Hod | 文学 | Lit | |
| Netzach | 艺术 | Art | |
| Tiphereth | 自然 | Natural | |
| Gebura | 语言 | Language | |
| Chesed | 社会 | Social | |
| Binah | 哲学 | Philosophy | |
| Hokma | 宗教 | Religion | |
| Keter | 总类 | General | |

## Anti-patterns (do not use in EN UI)

| Avoid | Prefer | Why |
|-------|--------|-----|
| Atlas (player-facing) | **Compendium** | Atlas = map book; 图鉴/도감 = collection codex |
| Ego (no dots) for pages | **E.G.O.** | Matches vanilla style |
| “Abnormality cards” | Abnormality **pages** | LoR uses pages |
| Random inventing of urban stage names | Glossary above | Continuity with base game |

## Internal names

| Kind | Name | Note |
|------|------|------|
| Preferred C# | `LogCompendiumPanel`, `CompendiumUnlocked*`, `EnsureCompendiumUnlocks` | Aligns with EN *Compendium* |
| Localize key ids | `ui_RMR_*_Atlas`, `ui_AtlasTab` | Stable ids; **display** EN = Compendium |
| Disk (do not rename) | `Lastest`, `RMR_AtlasPermanentUnlocks`, JSON keys `atlasRoleBookUnlocks`… | Historical; migration would wipe saves |
| Package | `abcdcodecalmmagma.LogueLikeReborn` | Workshop uniqueId |

Developers: full map in [CODE_TERMINOLOGY.md](CODE_TERMINOLOGY.md).
