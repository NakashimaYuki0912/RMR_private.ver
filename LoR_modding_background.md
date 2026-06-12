# Library of Ruina Modding Background Notes

Source: `D:\Library of Ruina Modding For Dummies.pdf`

This file keeps the parts of the guide that matter for future RMR changes. It is a working reference, not a full copy of the PDF.

## Project Structure

- Library of Ruina mod data is mainly stored in XML files.
- Invitation Editor is useful for bootstrapping content, but hand-editing XML is often more reliable for real mod work.
- A typical content dependency order is:
  - combat pages
  - librarian key pages
  - enemy key pages
  - drop books
  - enemy unit info
  - stage info
- Stage XML should be checked together with enemy unit XML, key page XML, deck XML, passive XML, card XML, localization, and drop tables.

## LorId Rules

- `LorId` has two parts: `packageId` and numeric ID.
- Vanilla game content uses an empty package ID.
- Modded content uses the mod package ID.
- When referencing original Library of Ruina stages, cards, passives, or pages from code, prefer an explicit empty package ID, for example `new LorId("", id)`.
- When referencing this mod's custom XML content, use `LogLikeMod.ModId` or the package ID expected by the existing RMR code.
- A common source of bugs is looking up vanilla content with the mod package ID, or looking up mod content with an empty package ID.

## XML Script Naming

- Page scripts inherit from `DiceCardSelfAbilityBase` and class names must start with `DiceCardSelfAbility_`.
- Dice scripts inherit from `DiceCardAbilityBase` and class names must start with `DiceCardAbility_`.
- Passive scripts inherit from `PassiveAbilityBase` and class names must start with `PassiveAbility_`.
- Behaviour/action scripts inherit from `BehaviourActionBase` and class names must start with `BehaviourAction_`.
- XML references omit these prefixes. If the class prefix is wrong, the game will not find the script.

## Combat Page XML

- Card rarity values are `Common`, `Uncommon`, `Rare`, and `Unique`.
- `Unique` pages are normally limited to one copy per deck.
- `Spec` controls range/type: `Near`, `Far`, `FarArea`, `FarAreaEach`, `Special`, `Instance`.
- `Special` can break Invitation Editor, so use it carefully in XML.
- A combat page can only have one page script, but that script can do multiple things.
- Dice must use exact `Detail` values:
  - `Slash`
  - `Penetrate`
  - `Hit`
  - `Guard`
  - `Evasion`
- Misspelling `Detail` can silently default dice behavior and create bad cards.
- Chapter mapping:
  - 1 = Canard / 传闻
  - 2 = Urban Myth / 都市怪谈
  - 3 = Urban Legend / 都市传说
  - 4 = Urban Plague / 都市恶疾
  - 5 = Urban Nightmare / 都市梦魇
  - 6 = Star of the City / 都市之星
  - 7 = Impuritas Civitatis / 杂质

## Key Page XML

- Key page XML defines HP, stagger, speed dice, resistances, passives, light, rarity, icon, chapter, episode, and skin.
- `Pid="@origin"` is used to reference original game passives from XML.
- Mod passives normally do not need `Pid`, unless referencing another package.
- `RangeType` controls whether a key page can use melee, ranged, or hybrid pages.
- `SpeedDiceNum` controls natural speed dice count.
- `SuccessionPossibleNumber` can increase passive attribution limit; the guide notes 18 as a possible high-end value for Impuritas Civitatis.
- Impuritas key pages generally use 4/4 starting and maximum light in vanilla-like balance.

## DLL / Effect Timing

- Ruina effect timing matters; do not assume card, dice, passive, abnormality, and buff effects run in arbitrary order.
- Useful event families include:
  - `OnUseCard`
  - `OnRoundStart`
  - `OnRoundEnd`
  - `OnWaveStart`
  - `OnSucceedAttack`
  - `OnWinParrying`
  - `OnLoseParrying`
  - `BeforeRollDice`
  - `OnRollDice`
  - `BeforeGiveDamage`
- For normal attacks, on-hit order is generally page, dice, passive, abnormality page, then buffs.
- For mass attacks, order differs and some normal attack hooks do not behave the same way.
- When porting vanilla effects or abnormality behavior, confirm the exact override used in vanilla code.

## Testing And Reference Workflow

- Use decompilers or Tiphereth Database to inspect vanilla scripts.
- Workshop mods are usually under `Steam\steamapps\workshop\content\1256670\<workshop_id>`.
- Local test mods can be placed under `LibraryOfRuina_Data\Mods`.
- When copying patterns from other mods or vanilla code, understand the hook and ID assumptions before adapting it.

## Notes For This Roguelike Mod

- When adding real vanilla abnormality or Impuritas stages, do not fake them by copying lower-chapter custom enemies unless that is explicitly the design.
- For real vanilla content lookup in code, use empty package ID.
- For RMR custom wrappers, ensure every stage has:
  - valid `StageClassInfo`
  - non-empty `waveList`
  - valid enemy unit IDs
  - matching enemy key pages, decks, passives, and localization
- If a stage can black-screen, first check stage ID lookup, package ID, `waveList`, map info, and missing scripts.
