# -*- coding: utf-8 -*-
"""Clean remaining English leftovers in KR RMR ability descs."""
from pathlib import Path
import re

p = Path("Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt")
t = p.read_text(encoding="utf-8")

# order matters
subs = [
    ("Inflict ", "부여 "),
    ("Gain ", "얻음 "),
    ("Draw ", "뽑음 "),
    ("deal ", "가함 "),
    ("deals ", "가함 "),
    ("no damage", "피해 없음"),
    ("less damage", "감소된 피해"),
    ("additional", "추가"),
    ("equal to", "와 동일한"),
    ("equal ", "와 동일한 "),
    ("Max.", "최대"),
    ("permanently", "영구적으로"),
    ("loses", "잃음"),
    ("Destroy all", "모두 파괴"),
    ("Destroy ", "파괴 "),
    ("Only usable", "다음 상태에서만 사용 가능:"),
    ("state", "상태"),
    ("Blade Unlocked", "검 해금"),
    ("If ", "만약 "),
    ("if ", "만약 "),
    ("more ", "이상 "),
    ("whose ", "의 "),
    ("initially targeting another", "처음에 다른 대상을 지정한"),
    ("is targeted", "대상이 됨"),
    ("targeting", "대상으로 지정"),
    ("another", "다른"),
    ("higher", "이상"),
    ("chance", "확률"),
    ("recycle", "재사용"),
    ("both", "양쪽"),
    ("next ", "다음 "),
    ("'s ", "의 "),
    ("’s ", "의 "),
    ("Critical Strike", "치명타"),
    ("Slash Clash", "참격 합"),
    ("Emotion Level", "감정 단계"),
    ("above", "이상"),
    ("on self", "자신에게"),
    ("rest Act", "이번 무대 동안"),
    ("Can only be used at", "다음 조건에서만 사용 가능:"),
    ("and above", "이상"),
    ("This Speed", "이 속도"),
    ("has ", "가짐 "),
    ("usable", "사용 가능"),
    ("Only ", "오직 "),
    (" in ", " "),
    (" of ", " "),
    (" the ", " "),
    (" a ", " "),
    (" an ", " "),
    (" to ", " "),
    (" for ", " "),
    (" on ", " "),
    (" at ", " "),
    (" by ", " "),
    (" is ", " "),
    (" was ", " "),
    (" were ", " "),
    (" with ", " "),
    (" from ", " "),
    ("  ", " "),
]

def fix(m):
    body = m.group(1)
    for a, b in subs:
        body = body.replace(a, b)
        body = body.replace(a.lower(), b) if a[0].isupper() else body
    body = re.sub(r" {2,}", " ", body).strip()
    return f"<Desc>{body}</Desc>"

t2 = re.sub(
    r'<BattleCardAbility ID="(RMR_[^"]+)">(\s*(?:<Desc>[\s\S]*?</Desc>\s*)+)</BattleCardAbility>',
    lambda m: f'<BattleCardAbility ID="{m.group(1)}">'
    + re.sub(r"<Desc>(.*?)</Desc>", fix, m.group(2), flags=re.S)
    + "</BattleCardAbility>",
    t,
)
p.write_text(t2, encoding="utf-8")

residual = 0
samples = []
for m in re.finditer(r'<BattleCardAbility ID="(RMR_[^"]+)">\s*<Desc>(.*?)</Desc>', t2, re.S):
    if re.search(r"[A-Za-z]{5,}", m.group(2)):
        residual += 1
        if len(samples) < 15:
            samples.append((m.group(1), m.group(2)[:110]))
print("residual long EN words:", residual)
for a, d in samples:
    print(a, "=>", d)
