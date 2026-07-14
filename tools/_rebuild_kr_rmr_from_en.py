# -*- coding: utf-8 -*-
"""Rebuild all KR RMR_* ability Desc from EN with thorough LoR term mapping."""
from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
EN_PATH = ROOT / "Localize/en/DiceAbilityInfo/UpgradeCardAbility.txt"
KR_PATH = ROOT / "Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt"

# Longest / most specific first
RULES: list[tuple[str, str]] = [
    (r"\[On Clash Win\]", "[합 승리]"),
    (r"\[On Clash Lose\]", "[합 패배]"),
    (r"\[Clash Win\]", "[합 승리]"),
    (r"\[Clash Lose\]", "[합 패배]"),
    (r"\[On Use\]", "[사용시]"),
    (r"\[On Hit\]", "[적중]"),
    (r"\[Combat Start\]", "[전투 시작]"),
    (r"\[On Evade\]", "[회피 성공]"),
    (r"\[On Block\]", "[방어 성공]"),
    (r"\[When Hit\]", "[피격시]"),
    (r"\[On Kill\]", "[처치시]"),
    (r"\[On Crit\]", "[치명타]"),
    (r"\[On Play\]", "[사용시]"),
    (r"\[On Scroll\]", "[스크롤 시]"),
    (r"\[Start of Clash\]", "[합 시작]"),
    (r"\[End of Clash\]", "[합 종료]"),
    (r"this Scene", "이번 막"),
    (r"next Scene", "다음 막"),
    (r"this scene", "이번 막"),
    (r"next scene", "다음 막"),
    (r"the Scene", "이번 막"),
    (r"the next Scene", "다음 막"),
    (r"until the end of the Scene", "이번 막 종료까지"),
    (r"at the start of the Scene", "막 시작 시"),
    (r"at the end of the Scene", "막 종료 시"),
    (r"for the next Scene", "다음 막 동안"),
    (r"for this Scene", "이번 막 동안"),
    (r"Offensive dice", "공격 주사위"),
    (r"Offensive die", "공격 주사위"),
    (r"Defensive dice", "방어 주사위"),
    (r"Defensive die", "방어 주사위"),
    (r"Counter Die", "반격 주사위"),
    (r"Counter die", "반격 주사위"),
    (r"Speed die", "속도 주사위"),
    (r"Speed Die", "속도 주사위"),
    (r"all dice on this page", "이 책장의 모든 주사위"),
    (r"all dice", "모든 주사위"),
    (r"both user's and opponent's dice", "자신과 대상의 주사위 모두"),
    (r"opponent's dice", "대상의 주사위"),
    (r"user's dice", "자신의 주사위"),
    (r"Combat Page", "전투 책장"),
    (r"combat page", "전투 책장"),
    (r"Key Page", "핵심 책장"),
    (r"this page", "이 책장"),
    (r"This page", "이 책장"),
    (r"this page's Cost", "이 책장의 비용"),
    (r"draw a page", "책장을 1장 뽑음"),
    (r"Draw a page", "책장을 1장 뽑음"),
    (r"draw 1 page", "책장을 1장 뽑음"),
    (r"draw 2 pages", "책장을 2장 뽑음"),
    (r"discard it and draw a page", "그것을 버리고 책장을 1장 뽑음"),
    (r"discard it", "그것을 버림"),
    (r"discard", "버림"),
    (r"Restore (\d+) Light", r"빛 \1 회복"),
    (r"restore (\d+) Light", r"빛 \1 회복"),
    (r"Recover (\d+)%? HP", r"체력 \1 회복"),
    (r"recover (\d+)%? HP", r"체력 \1 회복"),
    (r"(\d+) Strength", r"힘 \1"),
    (r"(\d+) Endurance", r"인내 \1"),
    (r"(\d+) Protection", r"보호 \1"),
    (r"(\d+) Feeble", r"취약 \1"),
    (r"(\d+) Disarm", r"무장 해제 \1"),
    (r"(\d+) Bind", r"속박 \1"),
    (r"(\d+) Paralysis", r"마비 \1"),
    (r"(\d+) Burn", r"화상 \1"),
    (r"(\d+) Bleed", r"출혈 \1"),
    (r"(\d+) Fragile", r"취약 \1"),
    (r"(\d+) Haste", r"신속 \1"),
    (r"(\d+) Smoke", r"연기 \1"),
    (r"(\d+) Charge", r"충전 \1"),
    (r"(\d+) Fairy", r"요정 \1"),
    (r"Power \+(\d+)", r"위력 +\1"),
    (r"\+(\d+) Power", r"위력 +\1"),
    (r"Speed \+(\d+)", r"속도 +\1"),
    (r"\[Speed \+(\d+)\]", r"[속도 +\1]"),
    (r"Cost -(\d+)", r"비용 -\1"),
    (r"Reduce Cost of this page by (\d+)", r"이 책장의 비용 \1 감소"),
    (r"Reduce Cost by (\d+)", r"비용 \1 감소"),
    (r"lowered by the number of", "수만큼 감소:"),
    (r"lowered by", "만큼 감소"),
    (r"all other allies", "다른 모든 아군"),
    (r"all allies", "모든 아군"),
    (r"other allies", "다른 아군"),
    (r"random ally", "무작위 아군"),
    (r"random enemy", "무작위 적"),
    (r"all enemies", "모든 적"),
    (r"the target", "대상"),
    (r"the opponent", "대상"),
    (r"opponent", "대상"),
    (r"this character", "자신"),
    (r"the librarian", "사서"),
    (r"Stagger damage", "흐트러짐 피해"),
    (r"additional damage", "추가 피해"),
    (r"Mass Attack", "광역 공격"),
    (r"mass attack", "광역 공격"),
    (r"Single-use", "1회용"),
    (r"cannot act", "행동 불가"),
    (r"cannot be redirected", "대상 변경 불가"),
    (r"Unclashable", "합 불가"),
    (r"recycle both", "양쪽 모두 재사용"),
    (r"if .* is Offensive", "대상 주사위가 공격형이면"),
    (r"is Offensive", "공격형이면"),
    (r"On Play", "사용시"),
    (r"On Crit", "치명타"),
    (r"\[Adapt\]", "[적응]"),
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
    (r"\bFairy\b", "요정"),
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
    (r"\bof\b", ""),
    (r"\bhas\b", "가짐"),
    (r"\bThis\b", "이"),
    (r"\bthis\b", "이"),
    (r"\buser\b", "자신"),
    (r"  +", " "),
]


def translate(en: str) -> str:
    s = en
    for pat, rep in RULES:
        s = re.sub(pat, rep, s)
    s = re.sub(r" {2,}", " ", s)
    s = re.sub(r"\s+\n", "\n", s)
    return s.strip()


def parse_en() -> dict[str, list[str]]:
    text = EN_PATH.read_text(encoding="utf-8")
    out: dict[str, list[str]] = {}
    for m in re.finditer(
        r'<BattleCardAbility ID="(RMR_[^"]+)">\s*((?:<Desc>[\s\S]*?</Desc>\s*)+)</BattleCardAbility>',
        text,
    ):
        out[m.group(1)] = [d.strip() for d in re.findall(r"<Desc>(.*?)</Desc>", m.group(2), re.S)]
    return out


def main():
    en = parse_en()
    print("EN abilities", len(en))
    kr = KR_PATH.read_text(encoding="utf-8")
    kr_wo = re.sub(
        r'\s*<BattleCardAbility ID="RMR_[^"]+">[\s\S]*?</BattleCardAbility>',
        "",
        kr,
    )
    blocks = []
    for aid in sorted(en):
        descs = [f"    <Desc>{translate(d)}</Desc>" for d in en[aid]]
        blocks.append(f'  <BattleCardAbility ID="{aid}">\n' + "\n".join(descs) + "\n  </BattleCardAbility>")
    insert = "\n  <!-- RMR abilities KR (from EN LoR terms) -->\n" + "\n".join(blocks) + "\n"
    out = kr_wo.replace("</BattleCardAbilityDescRoot>", insert + "</BattleCardAbilityDescRoot>")
    KR_PATH.write_text(out, encoding="utf-8")
    print("wrote", KR_PATH.stat().st_size)

    residual = []
    for m in re.finditer(
        r'<BattleCardAbility ID="(RMR_[^"]+)">\s*<Desc>(.*?)</Desc>',
        out,
        re.S,
    ):
        d = m.group(2)
        if re.search(r"\[On |this Scene|Strength|Offensive|Draw |Gain |Inflict ", d):
            residual.append((m.group(1), d[:90]))
    print("residual", len(residual))
    for a, d in residual[:15]:
        print(" ", a, d)


if __name__ == "__main__":
    main()
