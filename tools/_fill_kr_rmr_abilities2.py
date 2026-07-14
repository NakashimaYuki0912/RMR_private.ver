import re
from pathlib import Path

en = Path("Localize/en/DiceAbilityInfo/UpgradeCardAbility.txt").read_text(encoding="utf-8")
kr_path = Path("Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt")
kr = kr_path.read_text(encoding="utf-8")

# More robust: find each RMR ability start, then match to next closing tag
en_ids = set(re.findall(r'<BattleCardAbility ID="(RMR_[^"]+)"', en))
kr_ids = set(re.findall(r'<BattleCardAbility ID="(RMR_[^"]+)"', kr))
print("en", len(en_ids), "kr", len(kr_ids), "missing", len(en_ids - kr_ids))

# Split by ability open tags more carefully
parts = re.split(r'(?=<BattleCardAbility\b)', en)
blocks_by_id = {}
for part in parts:
    m = re.match(r'<BattleCardAbility ID="(RMR_[^"]+)"[\s\S]*?</BattleCardAbility>\s*', part)
    if m:
        blocks_by_id[m.group(1)] = m.group(0)
        if not blocks_by_id[m.group(1)].startswith("  "):
            blocks_by_id[m.group(1)] = "  " + blocks_by_id[m.group(1)].lstrip()

print("parsed blocks", len(blocks_by_id))
missing = sorted(en_ids - kr_ids)
print("sample missing", missing[:15])
print("missing without block", [i for i in missing if i not in blocks_by_id][:15])

to_add = []
for mid in missing:
    b = blocks_by_id.get(mid)
    if not b:
        # synthesize empty desc from EN name if possible
        continue
    if not b.endswith("\n"):
        b += "\n"
    to_add.append(b)

print("to add", len(to_add))
if to_add:
    insert = "  <!-- RMR abilities pass2: EN fill -->\n" + "".join(to_add)
    if insert not in kr:
        kr2 = kr.replace("</BattleCardAbilityDescRoot>", insert + "</BattleCardAbilityDescRoot>")
        kr_path.write_text(kr2, encoding="utf-8")
        print("wrote size", kr_path.stat().st_size)

# recheck
kr = kr_path.read_text(encoding="utf-8")
kr_ids = set(re.findall(r'<BattleCardAbility ID="(RMR_[^"]+)"', kr))
print("final missing", len(en_ids - kr_ids))
