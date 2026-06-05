from __future__ import annotations

import re
import sys
import xml.etree.ElementTree as ET
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
LATE_BOSS_IDS = {60014, 60020, 60021, 60022, 60023}
EXPECTED_DEBUG_ENTRY = "new LorId(LogLikeMod.ModId, -6854)"
EXPECTED_REWARD_IDS = {
    250051: "Purple Tear",
    252001: "R Corp Myo",
    253001: "R Corp Maxim",
    254001: "R Corp Rudolph",
    255001: "Xiao",
    256001: "Yan",
}


def read_text(path: Path) -> str:
    for encoding in ("utf-8-sig", "utf-8", "cp949", "cp1252"):
        try:
            return path.read_text(encoding=encoding)
        except UnicodeDecodeError:
            continue
    return path.read_text(errors="replace")


def parse_xml(path: Path) -> ET.Element:
    return ET.fromstring(read_text(path))


def collect_ids_by_regex(glob: str, pattern: str) -> set[int]:
    ids: set[int] = set()
    rx = re.compile(pattern)
    for path in ROOT.glob(glob):
        for match in rx.finditer(read_text(path)):
            ids.add(int(match.group(1)))
    return ids


def collect_stage_pool() -> dict[int, str]:
    path = ROOT / "SpecialStaticInfo" / "StagesXmlInfos" / "Stage_ch6.xml"
    root = parse_xml(path)
    stages: dict[int, str] = {}
    for node in root.findall(".//StageList"):
        stage_id = int(node.attrib["ID"])
        stages[stage_id] = node.attrib.get("StageType", "")
    return stages


def collect_stage_units() -> dict[int, list[int]]:
    path = ROOT / "AddData" / "StageInfo" / "StageInfo_ch6.xml"
    root = parse_xml(path)
    stages: dict[int, list[int]] = {}
    for stage in root.findall(".//Stage"):
        stage_id = int(stage.attrib["id"])
        units = [int(unit.text) for unit in stage.findall(".//Unit") if unit.text]
        stages[stage_id] = units
    return stages


def validate() -> int:
    stage_pool = collect_stage_pool()
    stage_units = collect_stage_units()
    enemy_ids = collect_ids_by_regex("AddData/EnemyUnitInfo/*.xml", r"<Enemy\s+ID=\"(-?\d+)\"")
    reward_ids = collect_ids_by_regex(
        "SpecialStaticInfo/RewardPassiveInfos/*.xml",
        r"<RewardList\s+ID=\"(-?\d+)\"",
    )
    book_text_ids = collect_ids_by_regex(
        "Localize/cn/BookInfo/*.*",
        r"<BookDesc\s+BookID=\"(-?\d+)\"",
    )
    core_text = read_text(ROOT / "RMR_Core.cs")

    errors: list[str] = []

    print("Late Star of the City chapter 6 validation")
    print("=" * 48)

    debug_enabled = EXPECTED_DEBUG_ENTRY in core_text and f"//, {EXPECTED_DEBUG_ENTRY}" not in core_text
    print(f"Debug chapter 6 entry (-6854): {'enabled' if debug_enabled else 'disabled'}")
    if not debug_enabled:
        errors.append("Chapter 6 debug entry -6854 is not enabled in RMRCore.booksToAddToInventory.")

    print("\nLate boss stages:")
    boss_candidates = 0
    for stage_id in sorted(LATE_BOSS_IDS):
        stage_type = stage_pool.get(stage_id)
        if stage_type == "Boss":
            boss_candidates += 1
        units = stage_units.get(stage_id)
        unit_status = "missing stage"
        if units is not None:
            missing_units = [unit for unit in units if unit not in enemy_ids]
            unit_status = "units ok" if not missing_units else f"missing units {missing_units}"
            if missing_units:
                errors.append(f"Stage {stage_id} references missing enemy units: {missing_units}")
        if stage_type is None:
            errors.append(f"Stage {stage_id} is missing from SpecialStaticInfo/StagesXmlInfos/Stage_ch6.xml")
        if units is None:
            errors.append(f"Stage {stage_id} is missing from AddData/StageInfo/StageInfo_ch6.xml")
        print(f"- {stage_id}: type={stage_type or 'missing'}, {unit_status}")
    print(f"Late boss candidates in chapter 6 pool: {boss_candidates}")
    if boss_candidates < 4:
        errors.append("Chapter 6 has fewer than four late-Star boss candidates.")

    print("\nRoguelike pacing:")
    print("- Grade6 uses the existing vanilla limits: Normal=5, Mystery=2, Shop=2, Boss=1, Rest=2.")
    print("- One boss candidate is selected per run at chapter initialization.")

    print("\nExpected key rewards:")
    for reward_id, label in sorted(EXPECTED_REWARD_IDS.items()):
        in_reward_pool = reward_id in reward_ids
        has_cn_text = reward_id in book_text_ids
        print(
            f"- {reward_id} {label}: "
            f"reward={'yes' if in_reward_pool else 'no'}, "
            f"cn_text={'yes' if has_cn_text else 'no'}"
        )
        if not in_reward_pool:
            errors.append(f"Reward ID {reward_id} ({label}) is missing from reward passive XML.")
        if not has_cn_text:
            errors.append(f"Reward ID {reward_id} ({label}) is missing from Localize/cn/BookInfo.")

    print("\nResult:")
    if errors:
        for error in errors:
            print(f"FAIL: {error}")
        return 1
    print("PASS: chapter 6 late-Star integration links are present.")
    return 0


if __name__ == "__main__":
    sys.exit(validate())
