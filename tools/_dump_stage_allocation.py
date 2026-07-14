# -*- coding: utf-8 -*-
"""Dump RMR stage pool + per-run slot allocation."""
from __future__ import annotations

import re
from pathlib import Path
from collections import defaultdict

ROOT = Path(__file__).resolve().parents[1]

LIMITS = {
    1: dict(Normal=2, Elite=0, Mystery=1, Shop=1, Boss=1, Rest=1, Creature=0),
    2: dict(Normal=4, Elite=0, Mystery=1, Shop=1, Boss=1, Rest=1, Creature=0),
    3: dict(Normal=4, Elite=0, Mystery=2, Shop=1, Boss=1, Rest=1, Creature=1),
    4: dict(Normal=4, Elite=0, Mystery=2, Shop=1, Boss=1, Rest=1, Creature=1),
    5: dict(Normal=5, Elite=0, Mystery=2, Shop=2, Boss=1, Rest=1, Creature=1),
    6: dict(Normal=4, Elite=1, Mystery=2, Shop=2, Boss=1, Rest=1, Creature=1),
    7: dict(Normal=5, Elite=0, Mystery=2, Shop=2, Boss=3, Rest=1, Creature=1),
}

CHAPTER_NAMES = {
    1: "都市传闻",
    2: "都市怪谈",
    3: "都市传说",
    4: "都市疾病",
    5: "都市梦魇",
    6: "都市之星",
    7: "杂质",
}


def load_stage_names() -> dict[int, str]:
    names = {}
    # Localize CN CustomStage / Enemy / Stage
    for f in (ROOT / "Localize/cn").rglob("*"):
        if not f.is_file():
            continue
        if f.suffix.lower() not in {".txt", ".xml"}:
            continue
        text = f.read_text(encoding="utf-8", errors="replace")
        for m in re.finditer(r'<text id="Stage_?(\d+)">([^<]+)</text>', text, re.I):
            names[int(m.group(1))] = m.group(2).strip()
        for m in re.finditer(r'<text id="(\d+)">([^<]+)</text>', text):
            # too broad
            pass
    # AddData StageInfo names
    for f in (ROOT / "AddData/StageInfo").glob("*.xml"):
        text = f.read_text(encoding="utf-8", errors="replace")
        for m in re.finditer(
            r'<Stage\b([^>]*)>([\s\S]*?)</Stage>', text, re.I
        ):
            idm = re.search(r'\bid\s*=\s*"(-?\d+)"', m.group(1), re.I)
            if not idm:
                continue
            sid = int(idm.group(1))
            namem = re.search(r"<Name>([^<]*)</Name>", m.group(2))
            if namem and namem.group(1).strip():
                names[sid] = namem.group(1).strip()
    # CustomStage localize
    for f in (ROOT / "Localize/cn").glob("CustomStage*.txt"):
        text = f.read_text(encoding="utf-8", errors="replace")
        for m in re.finditer(r'<text id="([^"]+)">([^<]+)</text>', text):
            # map later if needed
            pass
    return names


def parse_stage_lists() -> dict[int, list[dict]]:
    by_grade = defaultdict(list)
    for f in sorted((ROOT / "SpecialStaticInfo/StagesXmlInfos").glob("Stage_*.xml")):
        text = f.read_text(encoding="utf-8", errors="replace")
        for ch_m in re.finditer(
            r'<ChapterList\s+Chapter="Grade(\d+|All)"[^>]*>([\s\S]*?)</ChapterList>',
            text,
            re.I,
        ):
            gkey = ch_m.group(1)
            body = ch_m.group(2)
            grade = 0 if gkey == "All" else int(gkey)
            for m in re.finditer(
                r'<StageList\s+ID="(-?\d+)"\s+StageType="([^"]+)"([^/]*?)/\s*>',
                body,
            ):
                sid = int(m.group(1))
                stype = m.group(2)
                rest = m.group(3) or ""
                scriptm = re.search(r'Script="([^"]*)"', rest)
                script = scriptm.group(1) if scriptm else ""
                # comment after
                by_grade[grade].append(
                    {"id": sid, "type": stype, "script": script, "file": f.name}
                )
    return by_grade


def main():
    names = load_stage_names()
    by_grade = parse_stage_lists()

    print("=== 每层「可选池」(SpecialStaticInfo/StagesXmlInfos) ===\n")
    for grade in range(1, 8):
        stages = by_grade.get(grade, [])
        print(f"## Grade{grade} {CHAPTER_NAMES.get(grade, '')}  — 池内 {len(stages)} 条")
        g = defaultdict(list)
        for s in stages:
            g[s["type"]].append(s)
        for typ in ["Normal", "Elite", "Boss", "Creature", "Mystery", "Shop", "Rest"]:
            if typ not in g:
                continue
            print(f"  [{typ}] ×{len(g[typ])}")
            for s in sorted(g[typ], key=lambda x: x["id"]):
                nm = names.get(s["id"], "")
                extra = f" Script={s['script']}" if s["script"] else ""
                cmt = f"  // {nm}" if nm else ""
                print(f"    {s['id']}{extra}{cmt}")
        # other types
        for typ in sorted(set(g) - {"Normal", "Elite", "Boss", "Creature", "Mystery", "Shop", "Rest"}):
            print(f"  [{typ}] ×{len(g[typ])}")
            for s in sorted(g[typ], key=lambda x: x["id"]):
                print(f"    {s['id']}")
        print()

    # shared
    print("## GradeAll / 共享")
    for grade, stages in by_grade.items():
        if grade != 0:
            continue
        for s in stages:
            print(f"  {s['type']}: {s['id']} ({s['file']})")
    # membership
    for f in (ROOT / "SpecialStaticInfo/StagesXmlInfos").glob("Stage_Membership*.xml"):
        text = f.read_text(encoding="utf-8", errors="replace")
        print(f"\n{f.name}:")
        for m in re.finditer(r'StageList\s+ID="(-?\d+)"\s+StageType="([^"]+)"', text):
            print(f"  {m.group(1)} [{m.group(2)}]")

    print("\n=== 每局抽取槽位 (VanillaGamemodeReceptionList) ===\n")
    print("| 章节 | 普通 | 精英 | 神秘 | 商店 | Boss | 休息 | 异想体 | 合计* | 备注 |")
    print("|---|---:|---:|---:|---:|---:|---:|---:|---:|---|")
    for g, lim in LIMITS.items():
        total = sum(lim.values())
        note = ""
        if g == 1 or g == 2:
            note = "无异想体战"
        if g == 4:
            note = "额外固定 +80000 合同/工坊事件"
        if g == 6:
            note = "精英30%出红雾60020，否则精英槽改普通+1"
        if g == 7:
            note = "Boss×3"
        # +1 chapter event from guaranteeChapterEvent
        print(
            f"| G{g} {CHAPTER_NAMES[g]} | {lim['Normal']} | {lim['Elite']} | {lim['Mystery']} | "
            f"{lim['Shop']} | {lim['Boss']} | {lim['Rest']} | {lim['Creature']} | ~{total}+1事件 | {note} |"
        )

    print(
        """
* 合计为槽位上限；另有 guaranteeChapterEvent：从该章章节事件池再保底抽 1 个。
* 休息节点固定使用模组 ID **855**（Stage_rest），不从章节池按 Rest 类型抽。
* 商店/休息 XML 里常见 ID 111001，实际运行时 Rest 用 855。
* 抽取逻辑：打乱池后按 StageType 依次填满各槽位（HandleLimitPicking）。
"""
    )

    # StageInfo combat names for main combat ids
    print("=== 主要战斗关卡名称 (AddData/StageInfo，有 Name 的) ===\n")
    for f in sorted((ROOT / "AddData/StageInfo").glob("StageInfo_ch*.xml")):
        text = f.read_text(encoding="utf-8", errors="replace")
        pairs = []
        for m in re.finditer(
            r'<Stage\b([^>]*)>([\s\S]*?)</Stage>', text, re.I
        ):
            idm = re.search(r'\bid\s*=\s*"(-?\d+)"', m.group(1), re.I)
            if not idm:
                continue
            namem = re.search(r"<Name>([^<]*)</Name>", m.group(2))
            if namem:
                pairs.append((int(idm.group(1)), namem.group(1).strip()))
        if pairs:
            print(f"## {f.name}")
            for sid, nm in pairs:
                print(f"  {sid}: {nm}")
            print()


if __name__ == "__main__":
    main()
