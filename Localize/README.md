# Localize/

Game-facing strings for **Roguelike Mod Reborn (RMR)**.

| Folder | Language |
|--------|----------|
| `cn/` | Chinese (Simplified) |
| `en/` | English |
| `kr/` | Korean (**not** Japanese) |

## For translators

Start here:

1. [docs/localization/README.md](../docs/localization/README.md)
2. [docs/localization/TRANSLATOR_GUIDE_EN.md](../docs/localization/TRANSLATOR_GUIDE_EN.md)
3. [docs/localization/GLOSSARY.md](../docs/localization/GLOSSARY.md)
4. [docs/localization/FILE_MAP.md](../docs/localization/FILE_MAP.md)

**Rules**

- Keep the **same relative path and filename** across languages.
- **Do not rename** `id="..."` attributes.
- Save as **UTF-8**.
- English player text: use **Compendium** (not “Atlas”) for 图鉴 / 도감.

## For developers

- Deploy by copying this tree to  
  `...\workshop\content\1256670\<id>\Assemblies\dlls\Localize\`
- Font / tofu rules: `docs/agent-handbook/01-localization-fonts.md`
- Key audit: `tools/localization/compare_ui_keys.ps1`

## Note on key names containing “Atlas”

XML/C# ids may still say `Atlas` (historical). Display strings for English must follow the glossary (**Compendium**).
