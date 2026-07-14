# -*- coding: utf-8 -*-
from pathlib import Path
import re

p = Path("Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt")
t = p.read_text(encoding="utf-8")
words = [
    ("Dice ", "주사위 "),
    ("For every", "마다"),
    ("For ", "동안 "),
    ("min 수치", "최소 수치"),
    ("roll 수치", "굴림 수치"),
    ("After Use", "사용 후"),
    ("purge", "제거"),
    ("purged amount", "제거된 수치"),
    ("twice", "2배"),
    ("Give ", "부여 "),
    ("Add ", "추가 "),
    ("hand", "손"),
    ("clashing it", "합 중인 대상"),
    ("are unaffected", "영향받지 않음"),
    ("loss", "손실"),
    ("stacked", "중첩"),
    ("Unity", "단결"),
    ("Shield", "보호막"),
    ("Destroy", "파괴"),
    ("Only usable", "다음 상태에서만 사용 가능"),
    ("state", "상태"),
    ("Blade Unlocked", "검 해금"),
    ("unaffected", "영향받지 않음"),
    ("cannot", "불가"),
    ("  ", " "),
]


def fix(m):
    body = m.group(1)
    for a, c in words:
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
res = sum(
    1
    for m in re.finditer(r"<Desc>(.*?)</Desc>", t2, re.S)
    if re.search(r"[A-Za-z]{5,}", m.group(1))
)
print("residual long EN words in any Desc:", res)
print("total RMR ids", len(re.findall(r'ID="RMR_', t2)))
