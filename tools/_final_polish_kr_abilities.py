# -*- coding: utf-8 -*-
from pathlib import Path
import re

p = Path("Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt")
t = p.read_text(encoding="utf-8")

# Apply aggressive cleanup on RMR Desc contents only
fixes = [
    (r"\bInflict\b", "부여"),
    (r"\bGain\b", "얻음"),
    (r"\bDraw\b", "뽑음"),
    (r"\bdeals?\b", "가함"),
    (r"\bno damage\b", "피해 없음"),
    (r"\bless damage\b", "피해 감소"),
    (r"\bequal their\b", "대상의"),
    (r"\bequal to\b", "와 같은"),
    (r"\bMax\.\b", "최대"),
    (r"\bMax\b", "최대"),
    (r"\bShield\b", "보호막"),
    (r"\bCritical Strike\b", "치명타"),
    (r"\bSlash Clash\b", "참격 합"),
    (r"\bClash\b", "합"),
    (r"\bEmotion Level\b", "감정 단계"),
    (r"\babove\b", "이상"),
    (r"\bon self\b", "자신에게"),
    (r"\brest Act\b", "이번 무대 동안"),
    (r"\brest of the Act\b", "이번 무대 동안"),
    (r"\beach Scene\b", "매 막"),
    (r"\bat start\b", "시작 시"),
    (r"\bCan only be used at\b", "다음 조건에서만 사용 가능:"),
    (r"\band above\b", "이상"),
    (r"\b그리고\b", "그리고"),
    (r"\b또는\b", "또는"),
    (r"\b가짐\b", "가짐"),
    (r"\bReduce\b", "감소"),
    (r"\bof\b", ""),
    (r"\bby\b", ""),
    (r"\bon\b", ""),
    (r"\bin\b", ""),
    (r"\bat\b", ""),
    (r"\btheir\b", "그"),
    (r"\bself\b", "자신"),
    (r"\buser\b", "자신"),
    (r"\bopponent\b", "대상"),
    (r"\bboth\b", "양쪽"),
    (r"\breycle\b", "재사용"),
    (r"\brecycle\b", "재사용"),
    (r"\bFragile\b", "취약"),
    (r"\b  +\b", " "),
    (r" {2,}", " "),
]

def polish_desc(m):
    body = m.group(1)
    for a, b in fixes:
        body = re.sub(a, b, body, flags=re.I)
    body = re.sub(r" {2,}", " ", body).strip()
    # fix doubled Korean particles from leftover English glue
    body = body.replace("얻음 얻음", "얻음").replace("부여 부여", "부여")
    return f"<Desc>{body}</Desc>"

def polish_block(m):
    aid, inner = m.group(1), m.group(2)
    inner2 = re.sub(r"<Desc>(.*?)</Desc>", polish_desc, inner, flags=re.S)
    return f'<BattleCardAbility ID="{aid}">{inner2}</BattleCardAbility>'

t2, n = re.subn(
    r'<BattleCardAbility ID="(RMR_[^"]+)">(\s*(?:<Desc>[\s\S]*?</Desc>\s*)+)</BattleCardAbility>',
    polish_block,
    t,
)
p.write_text(t2, encoding="utf-8")
print("polished", n)

# residual latin words count in RMR desc
residual = 0
samples = []
for m in re.finditer(r'<BattleCardAbility ID="(RMR_[^"]+)">\s*<Desc>(.*?)</Desc>', t2, re.S):
    if re.search(r"[A-Za-z]{4,}", m.group(2)):
        residual += 1
        if len(samples) < 12:
            samples.append((m.group(1), m.group(2)[:100]))
print("still has English words (4+ letters):", residual)
for a, d in samples:
    print(" ", a, "=>", d)
