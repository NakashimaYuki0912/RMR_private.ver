# -*- coding: utf-8 -*-
"""Build KR RMR_* ability text primarily from CN descriptions (better LoR term mapping)."""
from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
CN_PATH = ROOT / "Localize/cn/DiceAbilityInfo/UpgradeCardAbility.txt"
EN_PATH = ROOT / "Localize/en/DiceAbilityInfo/UpgradeCardAbility.txt"
KR_PATH = ROOT / "Localize/kr/DiceAbilityInfo/UpgradeCardAbility.txt"

# Chinese combat UI/terms -> Korean (longest first)
CN_TO_KR = [
    ("[拼点胜利]", "[합 승리]"),
    ("[拼点失败]", "[합 패배]"),
    ("[拼点开始]", "[합 시작]"),
    ("[使用时]", "[사용시]"),
    ("[命中时]", "[적중]"),
    ("[命中]", "[적중]"),
    ("[战斗开始]", "[전투 시작]"),
    ("[闪避成功]", "[회피 성공]"),
    ("[防御成功]", "[방어 성공]"),
    ("[被击中时]", "[피격시]"),
    ("[击杀时]", "[처치시]"),
    ("[装备时发动]", "[장착시 발동]"),
    ("[装备时]", "[장착시]"),
    ("下一幕", "다음 막"),
    ("本幕", "이번 막"),
    ("下一舞台", "다음 무대"),
    ("本舞台", "이번 무대"),
    ("攻击型骰子", "공격 주사위"),
    ("防御型骰子", "방어 주사위"),
    ("反击骰子", "반격 주사위"),
    ("所有骰子", "모든 주사위"),
    ("骰子", "주사위"),
    ("战斗书页", "전투 책장"),
    ("核心书页", "핵심 책장"),
    ("本书页", "이 책장"),
    ("书页", "책장"),
    ("抽取1张", "1장 뽑음"),
    ("抽取2张", "2장 뽑음"),
    ("抽取", "뽑음"),
    ("弃置", "버림"),
    ("丢弃", "버림"),
    ("恢复", "회복"),
    ("光芒", "빛"),
    ("体力", "체력"),
    ("混乱抗性", "흐트러짐 저항"),
    ("混乱伤害", "흐트러짐 피해"),
    ("混乱", "흐트러짐"),
    ("虚弱", "취약"),
    ("破绽", "취약"),
    ("束缚", "속박"),
    ("麻痹", "마비"),
    ("烧伤", "화상"),
    ("出血", "출혈"),
    ("守护", "보호"),
    ("强壮", "힘"),
    ("忍耐", "인내"),
    ("迅捷", "신속"),
    ("烟气", "연기"),
    ("充能", "충전"),
    ("妖精", "요정"),
    ("威力", "위력"),
    ("费用", "비용"),
    ("速度", "속도"),
    ("友方", "아군"),
    ("敌方", "적"),
    ("敌人", "적"),
    ("目标", "대상"),
    ("随机", "무작위"),
    ("全部", "모든"),
    ("其他", "다른"),
    ("自身", "자신"),
    ("获得", "얻음"),
    ("赋予", "부여"),
    ("造成", "가함"),
    ("额外", "추가"),
    ("伤害", "피해"),
    ("最大", "최대"),
    ("当前", "현재"),
    ("减少", "감소"),
    ("增加", "증가"),
    ("降低", "감소"),
    ("提高", "증가"),
    ("无法", "불가"),
    ("可以", "가능"),
    ("若", "만약"),
    ("则", "이면"),
    ("并", "그리고"),
    ("与", "와"),
    ("的", "의"),
    ("层", "층"),
    ("点", ""),
    ("张", "장"),
    ("名", "명"),
    ("次", "회"),
    ("时", "시"),
    ("为", ""),
    ("将", ""),
    ("其", "그"),
    ("该", "해당"),
    ("在", ""),
    ("中", "중"),
    ("每", "매"),
    ("一", "1"),
    ("二", "2"),
    ("三", "3"),
    ("四", "4"),
    ("五", "5"),
]

EN_ORDERED = [
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
    (r"this Scene", "이번 막"),
    (r"next Scene", "다음 막"),
    (r"Offensive dice", "공격 주사위"),
    (r"Offensive die", "공격 주사위"),
    (r"Defensive dice", "방어 주사위"),
    (r"Defensive die", "방어 주사위"),
    (r"Counter Die", "반격 주사위"),
    (r"all dice", "모든 주사위"),
    (r"Combat Page", "전투 책장"),
    (r"draw a page", "책장을 1장 뽑음"),
    (r"Draw a page", "책장을 1장 뽑음"),
    (r"draw 1 page", "책장을 1장 뽑음"),
    (r"draw 2 pages", "책장을 2장 뽑음"),
    (r"Restore (\d+) Light", r"빛 \1 회복"),
    (r"recover (\d+) HP", r"체력 \1 회복"),
    (r"Recover (\d+) HP", r"체력 \1 회복"),
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
    (r"random ally", "무작위 아군"),
    (r"random enemy", "무작위 적"),
    (r"all enemies", "모든 적"),
    (r"Stagger damage", "흐트러짐 피해"),
    (r"additional damage", "추가 피해"),
    (r"Mass Attack", "광역 공격"),
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
]


def cn_to_kr(text: str) -> str:
    s = text
    for a, b in CN_TO_KR:
        s = s.replace(a, b)
    s = re.sub(r"[ \t]{2,}", " ", s)
    return s.strip()


def en_to_kr(text: str) -> str:
    s = text
    for pat, rep in EN_ORDERED:
        s = re.sub(pat, rep, s)
    s = re.sub(r"[ \t]{2,}", " ", s)
    return s.strip()


def parse_descs(path: Path) -> dict[str, list[str]]:
    text = path.read_text(encoding="utf-8", errors="replace")
    out: dict[str, list[str]] = {}
    for m in re.finditer(
        r'<BattleCardAbility ID="(RMR_[^"]+)">\s*((?:<Desc>[\s\S]*?</Desc>\s*)+)</BattleCardAbility>',
        text,
    ):
        descs = [d.strip() for d in re.findall(r"<Desc>(.*?)</Desc>", m.group(2), re.S)]
        out[m.group(1)] = descs
    return out


def main():
    cn = parse_descs(CN_PATH)
    en = parse_descs(EN_PATH)
    print("CN RMR", len(cn), "EN RMR", len(en))

    # Prefer CN->KR for shared IDs; EN->KR for EN-only
    kr_descs: dict[str, list[str]] = {}
    for aid, descs in en.items():
        if aid in cn:
            kr_descs[aid] = [cn_to_kr(d) for d in cn[aid]]
        else:
            kr_descs[aid] = [en_to_kr(d) for d in descs]
    # CN-only IDs
    for aid, descs in cn.items():
        if aid not in kr_descs:
            kr_descs[aid] = [cn_to_kr(d) for d in descs]

    print("KR target abilities", len(kr_descs))

    # Rebuild KR file: keep non-RMR content, replace/append RMR blocks
    kr_text = KR_PATH.read_text(encoding="utf-8", errors="replace")

    # Remove existing RMR blocks
    kr_wo, n_rm = re.subn(
        r'\s*<BattleCardAbility ID="RMR_[^"]+">[\s\S]*?</BattleCardAbility>',
        "",
        kr_text,
    )
    print("removed old RMR blocks", n_rm)

    blocks = []
    for aid in sorted(kr_descs.keys()):
        parts = [f'    <Desc>{d}</Desc>' for d in kr_descs[aid]]
        blocks.append(
            f'  <BattleCardAbility ID="{aid}">\n' + "\n".join(parts) + "\n  </BattleCardAbility>"
        )
    insert = (
        "\n  <!-- RMR abilities: KR localized from CN/EN combat terms -->\n"
        + "\n".join(blocks)
        + "\n"
    )
    if "</BattleCardAbilityDescRoot>" not in kr_wo:
        raise SystemExit("missing root close")
    out = kr_wo.replace("</BattleCardAbilityDescRoot>", insert + "</BattleCardAbilityDescRoot>")
    KR_PATH.write_text(out, encoding="utf-8")
    print("wrote", KR_PATH, "size", KR_PATH.stat().st_size)

    # residual EN tags
    residual = re.findall(
        r'<BattleCardAbility ID="(RMR_[^"]+)">\s*<Desc>(\[On [^\]]+\]|this Scene)',
        out,
    )
    print("residual EN tag starts", len(residual))


if __name__ == "__main__":
    main()
