# Translator guide (English localization)

Welcome. This document is for people polishing or expanding **English** text for **Roguelike Mod Reborn (RMR)** for *Library of Ruina*.

You do **not** need to recompile C#. You edit text files under `Localize/en/`.

---

## 1. Setup

1. Get the repo folder that contains `Localize/` and `RogueLike Mod Reborn.csproj`.
2. Open files in an editor that **shows UTF-8** (VS Code, Notepad++ with UTF-8).
3. Read [GLOSSARY.md](GLOSSARY.md) before naming anything.

Optional: run the key check (PowerShell):

```powershell
cd "...\ruina-roguelike-reborn-main"
powershell -ExecutionPolicy Bypass -File .\tools\localization\compare_ui_keys.ps1
```

---

## 2. How text is loaded

```text
Game language (cn / en / kr)
        │
        ▼
Localize/{lang}/...  files are loaded by RMR / LogLike loaders
        │
        ▼
Code looks up by string id, e.g. TextDataModel.GetText("ui_RMR_Hub_Atlas")
        │
        ▼
UI shows your English string when language is English
```

- **Same id** in `cn`, `en`, and `kr`.
- Changing only `en/UIs.txt` updates English UI.
- If a key is missing in `en`, players may see Chinese fallback, the raw key, or empty text.

---

## 3. File format (UIs.txt example)

```xml
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<localize xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <text id="ui_RMR_Hub_Atlas">Compendium</text>
  <text id="ui_RMR_Hub_ResetConfirm">Reset all permanent progress?&#10;This clears the compendium...</text>

</localize>
```

| Rule | Detail |
|------|--------|
| Edit | Only the human text inside `>...</text>` |
| Do not edit | `id="..."` unless a developer asks |
| Line break | `&#10;` inside the tag |
| Quotes / `&` | Use XML entities if needed: `&amp;` `&lt;` `&gt;` |
| Placeholders | Keep `{0}`, `{1}` etc. in the same order as CN |

---

## 4. Recommended work order for EN

### Pass A — player-facing chrome (highest impact)

File: `Localize/en/UIs.txt`

- Start hub buttons and confirmations (`ui_RMR_Hub_*`)
- Help handbook bodies (`ui_RMR_Help_Body_*`)
- Compendium labels (`ui_RMR_Atlas_*` — **display** uses “Compendium”)
- Stage type names (`Stage_*`, `Stage_*_Desc`)
- Continue / new run strings

### Pass B — RMR-authored content

- `CardInfo/RMR_CardInfo_Items.xml`, `RMR_CardInfo_Starter.xml`
- `PassiveInfo/RMR_PassiveList_Special.xml`, `PassiveList_RMRPassives.txt`
- `EnemyNameInfo/RMR_Name_Special.txt`
- `LogueEffectText/*`
- `MysteryEvents/RMR_chstart.xml`

### Pass C — large inherited LogLike bulk

Polish when quality issues are reported; do not block Pass A/B.

See [FILE_MAP.md](FILE_MAP.md).

---

## 5. Style guide (English)

1. **Tone**: clear, slightly formal, matches LoR UI (short labels; help text can be longer).
2. **Glossary first**: Compendium, Realization, Abnormality page, Key page, Combat page, Ahn.
3. **Vanilla parity**: urban stage names, floor names, E.G.O. spelling.
4. **UI length**: button labels short; avoid wrapping walls of text on small buttons.
5. **Help paragraphs**: use `&#10;&#10;` between sections; optional `【】` style headings only if EN needs structure — prefer short bold-like plain headings or blank lines.
6. **No internal slang** in player text: no “Lastest”, “CheckStage”, package ids.

### Example — correct product naming

| Context | Good | Bad |
|---------|------|-----|
| Hub button | Compendium | Atlas |
| Help title | Permanent Compendium | Permanent Atlas |
| Help body | recorded in the permanent Compendium | recorded in the Atlas |

---

## 6. QA checklist before handoff

- [ ] UTF-8 encoding; Chinese characters do not look like mojibake when opened as UTF-8
- [ ] Every key you changed still exists in `cn/UIs.txt` with the **same id**
- [ ] Placeholders `{0}` preserved
- [ ] No unescaped `<` `>` inside text that would break XML
- [ ] Glossary terms used consistently
- [ ] `compare_ui_keys.ps1` reports 0 missing keys for `en` vs `cn` (for UIs.txt)
- [ ] Developer deployed `Localize/en` and fully restarted LoR
- [ ] In-game language set to English; hub / help / compendium reviewed

---

## 7. How developers wire new strings (for reference)

```csharp
// Prefer Localize key — works for all languages once files are filled.
string s = TextDataModel.GetText("ui_RMR_Hub_Atlas");

// Hub helper with fallbacks if key missing (dev only):
// T("ui_RMR_Hub_Atlas", "图鉴", "Compendium", "도감");
```

**Translators should not add new keys** without a matching code change.

---

## 8. Deliverables from a translator

Please return:

1. Diff or zip of changed files under `Localize/en/` (or your language folder).
2. Short note: files touched + open questions (ambiguous lore, length limits).
3. Optional: list of keys still awkward in EN.

---

## 9. Contact / process

- Product questions (lore, feature meaning): mod maintainer.
- Key conflicts / missing ids: developer (code + Localize must match).
- Font / tofu bugs: developer (not a translation issue) — see agent handbook 01.
