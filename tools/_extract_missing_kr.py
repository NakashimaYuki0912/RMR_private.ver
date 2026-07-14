from pathlib import Path
import re

files_keys = {
    "CardInfo/CardInfo_Special.txt": ["706108"],
    "CardInfo/CardInfo_ch1_ForEnemy.txt": ["-104006"],
    "CraftEffect.txt": [
        "CraftEquipChapter7Desc",
        "CraftEquipChapter7Name",
        "CraftExCardChapter5Desc",
        "CraftExCardChapter5Name",
        "CraftExCardChapter6Desc",
        "CraftExCardChapter6Name",
    ],
    "DiceAbilityInfo/EditCardAbility.txt": ["costdownLogEdit", "fireIlsumLogEdit"],
    "DropBookInfo/DropBookInfo.txt": ["LogueLikeBook_Ch0_1_Modded"],
    "EnemyNameInfo/_CharactersName.xml": ["2001"],
    "Mystery4.txt": ["MysteryCh4_1Frame0Choice2Desc"],
    "PassiveInfo/PassiveList_ch1_event.txt": ["28480004", "854"],
    "UIs.txt": ["BattleEnd_EgoReward"],
}


def extract_blocks(text, key):
    esc = re.escape(key)
    patterns = [
        rf"(  <BattleCardDesc ID=\"{esc}\">.*?</BattleCardDesc>)",
        rf"(  <text id=\"{esc}\">.*?</text>)",
        rf"(  <BattleCardAbility ID=\"{esc}\">.*?</BattleCardAbility>)",
        rf"(  <PassiveDesc ID=\"{esc}\">.*?</PassiveDesc>)",
        rf"(  <Name ID=\"{esc}\">.*?</Name>)",
        rf"(  <CharacterName ID=\"{esc}\">.*?</CharacterName>)",
        rf"(  <DropBookDesc ID=\"{esc}\">.*?</DropBookDesc>)",
        rf"(  <BookDesc ID=\"{esc}\">.*?</BookDesc>)",
    ]
    for p in patterns:
        m = re.search(p, text, re.S | re.I)
        if m:
            return m.group(1)
    idx = text.find(f'"{key}"')
    if idx >= 0:
        return text[max(0, idx - 40) : idx + 250]
    return None


for rel, keys in files_keys.items():
    cn = Path("Localize/cn") / rel
    en = Path("Localize/en") / rel
    cn_t = cn.read_text(encoding="utf-8", errors="replace") if cn.exists() else ""
    en_t = en.read_text(encoding="utf-8", errors="replace") if en.exists() else ""
    print("====", rel)
    for k in keys:
        print(" KEY", k)
        eb = extract_blocks(en_t, k)
        cb = extract_blocks(cn_t, k)
        print("  EN:", (eb or "NONE")[:400].replace("\n", " | "))
        print("  CN:", (cb or "NONE")[:400].replace("\n", " | "))
