from pathlib import Path
import re

en = Path("Localize/en/DiceAbilityInfo/UpgradeCardAbility.txt").read_text(encoding="utf-8")
kr_path = Path("Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt")
kr = kr_path.read_text(encoding="utf-8")

blocks = re.findall(
    r'(  <BattleCardAbility ID="RMR_[^"]+">.*?</BattleCardAbility>\s*)',
    en,
    re.S,
)
print("EN RMR blocks", len(blocks))
existing = set(re.findall(r'<BattleCardAbility ID="(RMR_[^"]+)"', kr))
print("KR existing RMR", len(existing))
to_add = []
for b in blocks:
    m = re.search(r'ID="(RMR_[^"]+)"', b)
    if m and m.group(1) not in existing:
        to_add.append(b if b.endswith("\n") else b + "\n")
print("to add", len(to_add))
if to_add:
    insert = "  <!-- RMR abilities: EN fill until full KR localization -->\n" + "".join(to_add)
    if "</BattleCardAbilityDescRoot>" not in kr:
        raise SystemExit("closing tag missing")
    kr2 = kr.replace("</BattleCardAbilityDescRoot>", insert + "</BattleCardAbilityDescRoot>")
    kr_path.write_text(kr2, encoding="utf-8")
    print("wrote", kr_path, "size", kr_path.stat().st_size)
