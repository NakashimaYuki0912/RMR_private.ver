# -*- coding: utf-8 -*-
# Replace the whole changenote value in the VDF with a clean bilingual note.
import io, re

p = r"D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\tools\packaging\workshop_item_3743867841.vdf"
data = io.open(p, "r", encoding="utf-8").read()

note = (
    "[h2]2026-07-21 Update[/h2]\\n[list]\\n"
    "[*][b]Floor Realization reward isolation[/b]: Realization-exclusive Abnormality Pages and E.G.O. "
    "are now strictly gated behind each Floor's first Realization clear "
    "(triple-checked by passive ID range, script whitelist and unique-level).\\n"
    "[*]Normal battle reward pools, shop pools and permanent-tier unlock pools no longer leak locked Realization-exclusive pages.\\n"
    "[*]Loading an existing run save now sanitizes any locked exclusive pages that leaked in from older versions.\\n"
    "[/list]\\n\\n"
    "[h2]2026-07-21 更新[/h2]\\n[list]\\n"
    "[*][b]解放战奖励隔离[/b]：解放战专属异想体书页与E.G.O现在严格绑定各楼层解放战首通解锁"
    "（被动ID段、脚本白名单、Unique等级三重校验）。\\n"
    "[*]普通战斗奖励池、商店池与永久层级解锁池不再混入未解锁的解放战专属书页。\\n"
    "[*]读取旧存档时会自动清理此前版本混入的未解锁专属书页。\\n"
    "[/list]"
)

# Replace whole changenote line
pattern = re.compile(r'("changenote"\s+")(?:[^"\\]|\\.)*(")', re.S)
new_data, n = pattern.subn(lambda m: m.group(1) + note + m.group(2), data)
assert n == 1, "changenote not replaced: %d" % n

io.open(p, "w", encoding="utf-8", newline="").write(new_data)
print("OK changenote replaced, file length:", len(new_data))
