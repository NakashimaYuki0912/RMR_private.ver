# Localization file map

All paths are under `Localize/{cn|en|kr}/`.

## Priority for English UI work

| Priority | Path | Content |
|:--------:|------|---------|
| P0 | `UIs.txt` | Hub, help, compendium UI, shop/rest chrome, stage type labels, popups |
| P1 | `CardInfo/RMR_*.xml`, `CardInfo_Special.txt` | RMR-only combat pages / items / starter |
| P1 | `PassiveInfo/RMR_*.xml`, `PassiveList_RMRPassives.txt` | RMR passives |
| P1 | `EnemyNameInfo/RMR_Name_Special.txt` | Special names |
| P1 | `LogueEffectText/*.xml` | Global effects, pickups, boss rewards text |
| P1 | `MysteryEvents/RMR_chstart.xml`, `Mystery_Start.txt` | Start / membership mysteries |
| P2 | `Mystery2.txt` … `Mystery6.txt`, `MysteryEvents/Loglike_*` | Chapter mystery events |
| P2 | `CreaturePickUp.txt`, `CreaturePickUp_Table.xml` | Abnormality pickup names/desc |
| P2 | `BookInfo/*` | Key page stories / names for added books |
| P2 | `EffectTexts/RMR_bufs.xml` | Buffer effect text |
| P3 | Rest of `CardInfo_*`, `PassiveList_*` | Large upstream LogLike bulk text |

## Folder roles

### `UIs.txt`

XML list of `<text id="key">value</text>`.

Used by:

- Hub / help / compendium (`ui_RMR_*`)
- Shop / rest chrome (`ui_Shop*`, `ui_Rest*`)
- Stage type picker (`Stage_*`)
- Generic battle-end prompts (`BattleEnd_*`)

**Keys must match across cn / en / kr.** Currently UI key count is aligned (see `tools/localization/compare_ui_keys.ps1`).

### `CardInfo/`

Combat page names and descriptions.

- Prefer editing **`RMR_CardInfo_*.xml`** for RMR-authored content.
- Large `CardInfo_ch*.txt` files are mostly inherited LogLike content; polish when needed.

### `PassiveInfo/`

Passive ability names/descriptions.

### `BookInfo/`

Key page names and book story text.

### `EnemyNameInfo/`

Character display names for units.

### `MysteryEvents/` + `Mystery*.txt` / `Mystery*.xml`

Branching event dialogue and choice text.

### `LogueEffectText/`

Global permanent effects, craft-related labels, pickup models.

### `EffectTexts/`

Battle buffer / status text for custom RMR keywords.

### `CreaturePickUp*`

Abnormality pickup presentation (names, flavor) for reward UI.

## Encoding

| Rule | Detail |
|------|--------|
| Encoding | **UTF-8** (no BOM preferred; BOM is OK if consistent) |
| Forbidden | Windows ANSI / GBK / “ANSI as UTF-8” mis-save |
| Line endings | LF or CRLF both OK; do not mix binary garbage |
| Newlines in XML text | Prefer `&#10;` inside a single-line tag for multi-line UI |

## What is *not* in Localize/

| Asset | Where |
|-------|--------|
| Stage recipes / drop tables | `AddData/`, `SpecialStaticInfo/` |
| Hardcoded C# fallbacks | `RMR_StartHubPanel.T(...)`, help panel `BodyEn` / `BodyZh` |
| Vanilla game strings | Game install (not this mod) |

Hardcoded fallbacks only show if the key is missing. Prefer fixing `Localize/` over growing fallbacks.
