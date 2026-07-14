# -*- coding: utf-8 -*-
"""Audit Localize CN keys vs EN/KR, and CJK leftovers in EN + C# hardcodes."""
from __future__ import annotations

import collections
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
LOC = ROOT / "Localize"

ID_PATTERNS = [
    re.compile(r'<text\s+id="([^"]+)"', re.I),
    re.compile(r'<BattleCardDesc\s+ID="([^"]+)"', re.I),
    re.compile(r'<BookDesc\s+ID="([^"]+)"', re.I),
    re.compile(r'<PassiveDesc\s+ID="([^"]+)"', re.I),
    re.compile(r'<BattleDialog\s+ID="([^"]+)"', re.I),
    re.compile(r'<DropBookDesc\s+ID="([^"]+)"', re.I),
    re.compile(r'<CharacterName\s+ID="([^"]+)"', re.I),
    re.compile(r'<Name\s+ID="([^"]+)"', re.I),
    re.compile(r'<LogueEffectInfo\s+Id="([^"]+)"', re.I),
    re.compile(r'<Buf\s+ID="([^"]+)"', re.I),
    re.compile(r'<Buff\s+ID="([^"]+)"', re.I),
    re.compile(r'<EffectText\s+ID="([^"]+)"', re.I),
    re.compile(r'<BattleCardAbility\s+ID="([^"]+)"', re.I),
    re.compile(r'<Stage\s+id="([^"]+)"', re.I),
    re.compile(r'id="(ui_RMR_[^"]+)"', re.I),
    re.compile(r'id="(Shop_[^"]+)"', re.I),
    re.compile(r'id="(CardCheck[^"]+)"', re.I),
    re.compile(r'<Desc\s+ID="([^"]+)"', re.I),
    re.compile(r'<ability\s+id="([^"]+)"', re.I),
]

CJK = re.compile(r"[\u4e00-\u9fff]")


def rel_keys(lang_dir: Path):
    out = {}
    for f in lang_dir.rglob("*"):
        if not f.is_file() or f.suffix.lower() not in {".txt", ".xml"}:
            continue
        rel = str(f.relative_to(lang_dir)).replace("\\", "/")
        text = f.read_text(encoding="utf-8", errors="replace")
        keys = set()
        for pat in ID_PATTERNS:
            keys.update(m.group(1) for m in pat.finditer(text))
        out[rel] = keys
    return out


def main():
    cn_map = rel_keys(LOC / "cn")
    en_map = rel_keys(LOC / "en")
    kr_map = rel_keys(LOC / "kr")

    cn_files, en_files, kr_files = set(cn_map), set(en_map), set(kr_map)

    print("=== FILES in CN missing EN ===")
    for f in sorted(cn_files - en_files):
        print(" ", f)
    print("=== FILES in CN missing KR ===")
    for f in sorted(cn_files - kr_files):
        print(" ", f)
    print("=== FILES only EN (not CN) ===", sorted(en_files - cn_files))
    print("=== FILES only KR (not CN) ===", sorted(kr_files - cn_files))

    missing_en = []
    print("\n=== KEYS in CN missing EN ===")
    for f in sorted(cn_files & en_files):
        miss = sorted(cn_map[f] - en_map[f])
        if not miss:
            continue
        print(f"  [{f}] missing {len(miss)}:")
        for k in miss[:40]:
            print(f"    - {k}")
            missing_en.append((f, k))
        if len(miss) > 40:
            print(f"    ... +{len(miss) - 40} more")
            missing_en.extend((f, k) for k in miss[40:])

    missing_kr = []
    print("\n=== KEYS in CN missing KR ===")
    for f in sorted(cn_files & kr_files):
        miss = sorted(cn_map[f] - kr_map[f])
        if not miss:
            continue
        print(f"  [{f}] missing {len(miss)}:")
        for k in miss[:40]:
            print(f"    - {k}")
            missing_kr.append((f, k))
        if len(miss) > 40:
            print(f"    ... +{len(miss) - 40} more")
            missing_kr.extend((f, k) for k in miss[40:])

    print(f"\nCN file count={len(cn_files)} EN={len(en_files)} KR={len(kr_files)}")
    print(f"CN total keys={sum(len(v) for v in cn_map.values())}")
    print(f"MISSING EN keys={len(missing_en)}")
    print(f"MISSING KR keys={len(missing_kr)}")

    print("\n=== EN files still containing Chinese CJK ===")
    for f in sorted(en_files):
        text = (LOC / "en" / f).read_text(encoding="utf-8", errors="replace")
        runs = re.findall(r"[\u4e00-\u9fff]+", text)
        if runs:
            print(f"  {f}: {len(runs)} runs, sample={runs[:8]}")

    print("\n=== KR files still containing Chinese CJK ===")
    for f in sorted(kr_files):
        text = (LOC / "kr" / f).read_text(encoding="utf-8", errors="replace")
        runs = re.findall(r"[\u4e00-\u9fff]+", text)
        if runs:
            print(f"  {f}: {len(runs)} runs, sample={runs[:8]}")

    # Hardcoded Chinese in C# (non-comment)
    print("\n=== Hardcoded CJK in C# sources ===")
    by = collections.defaultdict(list)
    skip = {"bin", "obj", "dependencies", "_release_packages", "_codex_backups", "tools"}
    for f in ROOT.rglob("*.cs"):
        if any(p in f.parts for p in skip):
            continue
        try:
            lines = f.read_text(encoding="utf-8", errors="replace").splitlines()
        except Exception:
            continue
        for i, line in enumerate(lines, 1):
            s = line.strip()
            if not s or s.startswith("//") or s.startswith("/*") or s.startswith("*"):
                continue
            if CJK.search(line):
                by[str(f.relative_to(ROOT))].append((i, s[:140]))
    print(f"Files with CJK: {len(by)}")
    for path in sorted(by):
        print(f"  {path}: {len(by[path])} lines")
        for i, line in by[path][:4]:
            print(f"    L{i}: {line}")

    # Focus UIs.txt RMR keys
    print("\n=== UIs.txt RMR/ui keys CN vs EN vs KR ===")
    def ui_keys(lang):
        p = LOC / lang / "UIs.txt"
        if not p.exists():
            return set()
        return set(ID_PATTERNS[0].findall(p.read_text(encoding="utf-8", errors="replace")))

    cn_ui, en_ui, kr_ui = ui_keys("cn"), ui_keys("en"), ui_keys("kr")
    rmr_cn = {k for k in cn_ui if "RMR" in k or k.startswith("ui_")}
    print("CN RMR/ui keys:", len(rmr_cn))
    print("Missing EN:", sorted(rmr_cn - en_ui))
    print("Missing KR:", sorted(rmr_cn - kr_ui))

    # Values equal to Chinese for same id in EN (copy-paste)
    print("\n=== EN entries whose text still looks mostly Chinese (UIs + RMR) ===")
    for rel in sorted(en_files):
        if not any(x in rel for x in ("UIs", "RMR", "MysteryEvents", "LogueEffect", "chstart", "Help")):
            continue
        en_text = (LOC / "en" / rel).read_text(encoding="utf-8", errors="replace")
        # extract <text id="x">...</text> blocks roughly
        for m in re.finditer(
            r'<text\s+id="([^"]+)">\s*(.*?)\s*</text>',
            en_text,
            re.I | re.S,
        ):
            body = re.sub(r"<[^>]+>", "", m.group(2))
            if CJK.search(body):
                print(f"  {rel} :: {m.group(1)} => {body[:80]!r}")


if __name__ == "__main__":
    main()
