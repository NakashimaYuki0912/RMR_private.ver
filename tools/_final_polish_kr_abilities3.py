# -*- coding: utf-8 -*-
from pathlib import Path
import re

p = Path("Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt")
t = p.read_text(encoding="utf-8")
subs = [
    ("damage", "피해"),
    ("Damage", "피해"),
    ("Recover", "회복"),
    ("recover", "회복"),
    ("Resist", "저항"),
    ("resist", "저항"),
    ("value", "수치"),
    ("Value", "수치"),
    ("Lower", "감소"),
    ("lower", "감소"),
    ("Up ", "최대 "),
    ("times", "회"),
    ("Luck", "행운"),
    ("current", "현재"),
    ("Defensive", "방어형"),
    ("Offensive", "공격형"),
    ("plays", "사용"),
    ("using", "사용 중인"),
    ("themselves", "자신"),
    ("Once", "1회"),
    ("cannot be", "불가"),
    ("boost", "증가"),
    ("min value", "최소값"),
    ("max value", "최대값"),
    ("roll value", "굴림값"),
    ("Forced", "강제"),
    ("forced", "강제"),
    ("against", "대상:"),
    ("Targeted", "지정된"),
    ("yet use", "아직 사용하지 않은"),
    ("equal", "와 동일한"),
    ("less", "감소"),
    ("more", "이상"),
    ("With me", "나와 함께"),
    ("  ", " "),
]


def fix(m):
    body = m.group(1)
    for a, c in subs:
        body = body.replace(a, c)
    body = re.sub(r" {2,}", " ", body).strip()
    return f"<Desc>{body}</Desc>"


def block(m):
    return (
        f'<BattleCardAbility ID="{m.group(1)}">'
        + re.sub(r"<Desc>(.*?)</Desc>", fix, m.group(2), flags=re.S)
        + "</BattleCardAbility>"
    )


t2 = re.sub(
    r'<BattleCardAbility ID="(RMR_[^"]+)">(\s*(?:<Desc>[\s\S]*?</Desc>\s*)+)</BattleCardAbility>',
    block,
    t,
)
p.write_text(t2, encoding="utf-8")
res = 0
samples = []
for m in re.finditer(r'<BattleCardAbility ID="(RMR_[^"]+)">\s*<Desc>(.*?)</Desc>', t2, re.S):
    if re.search(r"[A-Za-z]{4,}", m.group(2)):
        res += 1
        if len(samples) < 12:
            samples.append((m.group(1), m.group(2)[:100]))
print("residual", res)
for a, d in samples:
    print(a, "=>", d)
