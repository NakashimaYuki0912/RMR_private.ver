import re
from pathlib import Path

for lang in ["cn", "en", "kr"]:
    p = Path(f"Localize/{lang}/DiceAbilityInfo/UpgradeCardAbility.txt")
    t = p.read_text(encoding="utf-8", errors="replace")
    ids = set(re.findall(r'<BattleCardAbility ID="([^"]+)"', t))
    rmr = {i for i in ids if i.startswith("RMR_")}
    print(lang, "total", len(ids), "RMR_", len(rmr))

cn = set(re.findall(r'<BattleCardAbility ID="([^"]+)"', Path("Localize/cn/DiceAbilityInfo/UpgradeCardAbility.txt").read_text(encoding="utf-8", errors="replace")))
en = set(re.findall(r'<BattleCardAbility ID="([^"]+)"', Path("Localize/en/DiceAbilityInfo/UpgradeCardAbility.txt").read_text(encoding="utf-8", errors="replace")))
kr = set(re.findall(r'<BattleCardAbility ID="([^"]+)"', Path("Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt").read_text(encoding="utf-8", errors="replace")))
print("CN-EN missing", len(cn - en), sorted(cn - en)[:20])
print("CN-KR missing RMR", len([x for x in (cn - kr) if x.startswith("RMR_")]))
print("EN has RMR not in KR", len([x for x in (en - kr) if x.startswith("RMR_")]))

for lang in ["cn", "en", "kr"]:
    t = Path(f"Localize/{lang}/UIs.txt").read_text(encoding="utf-8", errors="replace")
    keys = set(re.findall(r'<text id="([^"]+)"', t))
    helpk = sorted(k for k in keys if "Help" in k or k.startswith("ui_RMR"))
    print(lang, "ui_RMR/Help", len(helpk))
    # missing help bodies?
    for k in sorted(k for k in keys if "Help_Body" in k or "Help_Nav" in k):
        pass

cn_ui = set(re.findall(r'<text id="([^"]+)"', Path("Localize/cn/UIs.txt").read_text(encoding="utf-8", errors="replace")))
en_ui = set(re.findall(r'<text id="([^"]+)"', Path("Localize/en/UIs.txt").read_text(encoding="utf-8", errors="replace")))
kr_ui = set(re.findall(r'<text id="([^"]+)"', Path("Localize/kr/UIs.txt").read_text(encoding="utf-8", errors="replace")))
print("Help keys CN-EN", sorted(k for k in cn_ui - en_ui if "Help" in k or "RMR" in k))
print("Help keys CN-KR", sorted(k for k in cn_ui - kr_ui if "Help" in k or "RMR" in k))
