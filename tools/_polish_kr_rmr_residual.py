# -*- coding: utf-8 -*-
"""Second pass: polish residual English fragments inside KR RMR ability Desc."""
from __future__ import annotations

import re
from pathlib import Path

KR_PATH = Path(__file__).resolve().parents[1] / "Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt"

REPL = [
    (r"\[On Clash Win\]", "[합 승리]"),
    (r"\[On Clash Lose\]", "[합 패배]"),
    (r"\[On Use\]", "[사용시]"),
    (r"\[On Hit\]", "[적중]"),
    (r"\[Combat Start\]", "[전투 시작]"),
    (r"\[On Evade\]", "[회피 성공]"),
    (r"\[On Block\]", "[방어 성공]"),
    (r"\[When Hit\]", "[피격시]"),
    (r"\[On Kill\]", "[처치시]"),
    (r"this Scene", "이번 막"),
    (r"next Scene", "다음 막"),
    (r"this scene", "이번 막"),
    (r"next scene", "다음 막"),
    (r"Offensive dice", "공격 주사위"),
    (r"Offensive die", "공격 주사위"),
    (r"Defensive dice", "방어 주사위"),
    (r"Defensive die", "방어 주사위"),
    (r"Counter Die", "반격 주사위"),
    (r"all dice", "모든 주사위"),
    (r"Combat Page", "전투 책장"),
    (r"draw a page", "책장을 1장 뽑음"),
    (r"Draw a page", "책장을 1장 뽑음"),
    (r"discard it and draw a page", "그것을 버리고 책장을 1장 뽑음"),
    (r"discard it", "그것을 버림"),
    (r"discard", "버림"),
    (r"Restore (\d+) Light", r"빛 \1 회복"),
    (r"restore (\d+) Light", r"빛 \1 회복"),
    (r"Recover (\d+) HP", r"체력 \1 회복"),
    (r"recover (\d+) HP", r"체력 \1 회복"),
    (r"(\d+) Strength", r"힘 \1"),
    (r"(\d+) Endurance", r"인내 \1"),
    (r"(\d+) Protection", r"보호 \1"),
    (r"(\d+) Feeble", r"취약 \1"),
    (r"(\d+) Disarm", r"무장 해제 \1"),
    (r"(\d+) Bind", r"속박 \1"),
    (r"(\d+) Paralysis", r"마비 \1"),
    (r"(\d+) Burn", r"화상 \1"),
    (r"(\d+) Bleed", r"출혈 \1"),
    (r"(\d+) Haste", r"신속 \1"),
    (r"(\d+) Smoke", r"연기 \1"),
    (r"(\d+) Charge", r"충전 \1"),
    (r"Power \+(\d+)", r"위력 +\1"),
    (r"\+(\d+) Power", r"위력 +\1"),
    (r"all other allies", "다른 모든 아군"),
    (r"all allies", "모든 아군"),
    (r"other allies", "다른 아군"),
    (r"random ally", "무작위 아군"),
    (r"random enemy", "무작위 적"),
    (r"all enemies", "모든 적"),
    (r"the target", "대상"),
    (r"this page", "이 책장"),
    (r"This page", "이 책장"),
    (r"Stagger damage", "흐트러짐 피해"),
    (r"additional damage", "추가 피해"),
    (r"Mass Attack", "광역 공격"),
    (r"Single-use", "1회용"),
    (r"cannot act", "행동 불가"),
    (r"at the start of the Scene", "막 시작 시"),
    (r"at the end of the Scene", "막 종료 시"),
    (r"until the end of the Scene", "이번 막 종료까지"),
    (r"for the next Scene", "다음 막 동안"),
    (r"for this Scene", "이번 막 동안"),
    (r"\bHP\b", "체력"),
    (r"\bLight\b", "빛"),
    (r"\bPower\b", "위력"),
    (r"\bCost\b", "비용"),
    (r"\bBurn\b", "화상"),
    (r"\bBleed\b", "출혈"),
    (r"\bBind\b", "속박"),
    (r"\bFeeble\b", "취약"),
    (r"\bDisarm\b", "무장 해제"),
    (r"\bParalysis\b", "마비"),
    (r"\bProtection\b", "보호"),
    (r"\bStrength\b", "힘"),
    (r"\bEndurance\b", "인내"),
    (r"\bHaste\b", "신속"),
    (r"\bSmoke\b", "연기"),
    (r"\bCharge\b", "충전"),
    (r"\bStagger\b", "흐트러짐"),
    (r"\bally\b", "아군"),
    (r"\ballies\b", "아군"),
    (r"\benemy\b", "적"),
    (r"\benemies\b", "적"),
    (r"\btarget\b", "대상"),
    (r"\bpage\b", "책장"),
    (r"\bpages\b", "책장"),
    (r"\bdice\b", "주사위"),
    (r"\bdie\b", "주사위"),
    (r"\bgain\b", "얻음"),
    (r"\bgains\b", "얻음"),
    (r"\binflict\b", "부여"),
    (r"\bgive\b", "부여"),
    (r"\bdeal\b", "가함"),
    (r"\band\b", "그리고"),
    (r"\bor\b", "또는"),
    (r"\bthe\b", ""),
    (r"\ba\b", ""),
    (r"\ban\b", ""),
    (r"\bto\b", ""),
    (r"\bfor\b", ""),
    (r"\bif\b", "만약"),
    (r"\bwhen\b", "때"),
    (r"  +", " "),
]


def main():
    text = KR_PATH.read_text(encoding="utf-8")

    def fix_desc(m: re.Match) -> str:
        body = m.group(1)
        for pat, rep in REPL:
            body = re.sub(pat, rep, body, flags=re.I)
        body = re.sub(r" {2,}", " ", body).strip()
        return f"<Desc>{body}</Desc>"

    # only polish RMR ability Desc tags
    def fix_block(m: re.Match) -> str:
        aid = m.group(1)
        inner = m.group(2)
        inner2 = re.sub(r"<Desc>(.*?)</Desc>", fix_desc, inner, flags=re.S)
        return f'<BattleCardAbility ID="{aid}">{inner2}</BattleCardAbility>'

    new_text, n = re.subn(
        r'<BattleCardAbility ID="(RMR_[^"]+)">(\s*(?:<Desc>[\s\S]*?</Desc>\s*)+)</BattleCardAbility>',
        fix_block,
        text,
    )
    print("polished blocks", n)
    KR_PATH.write_text(new_text, encoding="utf-8")

    residual = re.findall(
        r'<BattleCardAbility ID="(RMR_[^"]+)">\s*<Desc>([^<]*(?:\[On |this Scene|Strength|Offensive)[^<]*)</Desc>',
        new_text,
    )
    print("residual EN-like", len(residual))
    for a, d in residual[:12]:
        print(" ", a, "=>", d[:100])


if __name__ == "__main__":
    main()
