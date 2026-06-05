using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using RogueLike_Mod_Reborn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace abcdcode_LOGLIKE_MOD
{
    public enum AtlasCategory
    {
        RoleBook,
        BattleCard,
        AbnormalityPage,
        EgoPage
    }

    public enum AtlasSection
    {
        Rumor,
        UrbanLegend,
        UrbanMyth,
        UrbanIllness,
        UrbanNightmare,
        UrbanStar,
        Impurity
    }

    public sealed class AtlasEntry
    {
        public LorId Id;
        public string Title;
        public string Description;
        public Sprite Artwork;
        public bool Unlocked;
        public AtlasCategory Category;
        public AtlasSection Section;
        public SephirahType Floor;
    }

    public class LogAtlasPanel : Singleton<LogAtlasPanel>
    {
        public const string LockedTitle = "?";

        private static readonly AtlasSection[] Sections =
        {
            AtlasSection.Rumor,
            AtlasSection.UrbanLegend,
            AtlasSection.UrbanMyth,
            AtlasSection.UrbanIllness,
            AtlasSection.UrbanNightmare,
            AtlasSection.UrbanStar,
            AtlasSection.Impurity
        };

        private static readonly AtlasCategory[] Categories =
        {
            AtlasCategory.RoleBook,
            AtlasCategory.BattleCard,
            AtlasCategory.AbnormalityPage,
            AtlasCategory.EgoPage
        };

        public GameObject root;
        private readonly List<LogAtlasTile> tiles = new List<LogAtlasTile>();
        private readonly List<TextMeshProUGUI> sectionLabels = new List<TextMeshProUGUI>();
        private readonly List<TextMeshProUGUI> categoryLabels = new List<TextMeshProUGUI>();
        private AtlasSection currentSection = AtlasSection.Rumor;
        private AtlasCategory currentCategory = AtlasCategory.RoleBook;

        public static GameObject GetLogUIObj(int index)
        {
            GameObject source = (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.BattleCardPanel.gameObject;
            GameObject root = UnityEngine.Object.Instantiate<Transform>(source.transform, source.transform.parent).gameObject;
            UnityEngine.Object.Destroy(root.GetComponent<UIBattleSettingPanel>());
            for (int i = 0; i < root.transform.childCount; ++i)
                UnityEngine.Object.Destroy(root.transform.GetChild(i).gameObject);
            root.SetActive(true);
            root.transform.localPosition = Vector3.zero;
            root.transform.localScale = source.transform.localScale;
            root.GetComponent<Canvas>().enabled = true;
            root.GetComponent<Canvas>().sortingOrder += index;
            root.GetComponent<CanvasGroup>().alpha = 1f;
            root.GetComponent<CanvasGroup>().blocksRaycasts = true;
            root.GetComponent<CanvasGroup>().interactable = true;
            return root;
        }

        public void Init()
        {
            if (root != null)
            {
                root.SetActive(true);
                return;
            }

            root = GetLogUIObj(2);
            CreateTabs();
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Image image = ModdingUtils.CreateImage(root.transform, (Sprite)null, Vector2.one, new Vector2(col * 120 - 420, 250 - row * 105), new Vector2(92f, 92f));
                    tiles.Add(image.gameObject.AddComponent<LogAtlasTile>());
                }
            }
            root.SetActive(true);
        }

        public void SetActive(bool value)
        {
            if (value)
            {
                Init();
                UpdateTiles();
            }
            else if (root != null)
            {
                root.SetActive(false);
            }
        }

        private void CreateTabs()
        {
            for (int i = 0; i < Sections.Length; i++)
            {
                AtlasSection section = Sections[i];
                Image image = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one, new Vector2(i * 135 - 410, 360), new Vector2(126f, 40f));
                Button button = image.gameObject.AddComponent<Button>();
                button.targetGraphic = image;
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener((UnityAction)(() => SelectSection(section)));
                TextMeshProUGUI label = ModdingUtils.CreateText_TMP(image.transform, Vector2.zero, 20, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
                label.text = GetSectionName(section);
                sectionLabels.Add(label);
            }

            for (int i = 0; i < Categories.Length; i++)
            {
                AtlasCategory category = Categories[i];
                Image image = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one, new Vector2(i * 170 - 260, 310), new Vector2(155f, 42f));
                Button button = image.gameObject.AddComponent<Button>();
                button.targetGraphic = image;
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener((UnityAction)(() => SelectCategory(category)));
                TextMeshProUGUI label = ModdingUtils.CreateText_TMP(image.transform, Vector2.zero, 21, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
                label.text = GetCategoryName(category);
                categoryLabels.Add(label);
            }
        }

        private void SelectSection(AtlasSection section)
        {
            currentSection = section;
            UpdateTiles();
        }

        private void SelectCategory(AtlasCategory category)
        {
            currentCategory = category;
            UpdateTiles();
        }

        private void UpdateTiles()
        {
            for (int i = 0; i < sectionLabels.Count; i++)
                sectionLabels[i].color = Sections[i] == currentSection ? UIColorManager.Manager.GetUIColor(UIColor.Highlighted) : LogLikeMod.DefFontColor;
            for (int i = 0; i < categoryLabels.Count; i++)
                categoryLabels[i].color = Categories[i] == currentCategory ? UIColorManager.Manager.GetUIColor(UIColor.Highlighted) : LogLikeMod.DefFontColor;

            List<AtlasEntry> entries = BuildEntries()
                .Where(x => x.Section == currentSection && x.Category == currentCategory)
                .OrderBy(x => x.Category == AtlasCategory.EgoPage ? (int)x.Floor : 0)
                .ThenBy(x => x.Title)
                .ToList();

            for (int i = 0; i < tiles.Count; i++)
                tiles[i].Init(i < entries.Count ? entries[i] : null);
        }

        public static List<AtlasEntry> BuildEntries()
        {
            List<AtlasEntry> entries = new List<AtlasEntry>();
            entries.AddRange(BuildRoleBookEntries());
            entries.AddRange(BuildBattleCardEntries());
            entries.AddRange(BuildAbnormalityEntries());
            entries.AddRange(BuildEgoEntries());
            return entries;
        }

        private static IEnumerable<AtlasEntry> BuildRoleBookEntries()
        {
            foreach (object obj in ExtractObjects(Singleton<BookXmlList>.Instance, typeof(BookXmlInfo)).Take(260))
            {
                BookXmlInfo info = obj as BookXmlInfo;
                LorId id = GetLorId(info, "id");
                if (id == LorId.None)
                    continue;
                yield return new AtlasEntry
                {
                    Id = id,
                    Title = GetDisplayName(info, id),
                    Description = id.ToString(),
                    Artwork = GetBookArtwork(info),
                    Unlocked = IsRoleBookUnlocked(id),
                    Category = AtlasCategory.RoleBook,
                    Section = SectionFromChapter(GetIntMember(info, "Chapter", 1)),
                    Floor = SephirahType.None
                };
            }
        }

        private static IEnumerable<AtlasEntry> BuildBattleCardEntries()
        {
            foreach (object obj in ExtractObjects(ItemXmlDataList.instance, typeof(DiceCardXmlInfo)).Take(420))
            {
                DiceCardXmlInfo info = obj as DiceCardXmlInfo;
                LorId id = GetLorId(info, "id");
                if (id == LorId.None)
                    continue;
                yield return new AtlasEntry
                {
                    Id = id,
                    Title = GetDisplayName(info, id),
                    Description = id.ToString(),
                    Artwork = null,
                    Unlocked = IsBattleCardUnlocked(id),
                    Category = AtlasCategory.BattleCard,
                    Section = SectionFromChapter(GetIntMember(info, "Chapter", 1)),
                    Floor = SephirahType.None
                };
            }
        }

        private static IEnumerable<AtlasEntry> BuildAbnormalityEntries()
        {
            foreach (RewardPassivesInfo list in Singleton<RewardPassivesList>.Instance.infos.Where(x => x.rewardtype == PassiveRewardListType.Custom))
            {
                if (list.RewardPassiveList == null)
                    continue;
                foreach (RewardPassiveInfo info in list.RewardPassiveList.Where(x => x.rewardtype == RewardType.Creature && !RMRAbnormalityUnlockManager.IsNoAbnormalityFallback(x.id)))
                {
                    EmotionCardXmlInfo card = LogLikeMod.GetRegisteredPickUpXml(info);
                    AbnormalityCard desc = GetAbnormalityCardDesc(card, info);
                    yield return new AtlasEntry
                    {
                        Id = info.id,
                        Title = desc == null || string.IsNullOrEmpty(desc.cardName) ? info.script : desc.cardName,
                        Description = desc == null ? info.script : desc.abilityDesc,
                        Artwork = info.Artwork,
                        Unlocked = IsAbnormalityUnlocked(info),
                        Category = AtlasCategory.AbnormalityPage,
                        Section = SectionFromTier(RMRAbnormalityUnlockManager.GetTierForScript(info.script)),
                        Floor = GetFloorFromScript(info.script)
                    };
                }
            }
        }

        private static IEnumerable<AtlasEntry> BuildEgoEntries()
        {
            if (LogLikeMod.RewardCardDic_Dummy == null)
                yield break;
            foreach (KeyValuePair<string, List<EmotionEgoXmlInfo>> pair in LogLikeMod.RewardCardDic_Dummy)
            {
                foreach (EmotionEgoXmlInfo ego in pair.Value)
                {
                    DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(ego.CardId);
                    if (card == null)
                        continue;
                    SephirahType floor = GetSephirah(ego);
                    yield return new AtlasEntry
                    {
                        Id = ego.CardId,
                        Title = GetDisplayName(card, ego.CardId),
                        Description = floor.ToString(),
                        Artwork = null,
                        Unlocked = IsBattleCardUnlocked(ego.CardId),
                        Category = AtlasCategory.EgoPage,
                        Section = SectionFromTier(TierFromFloor(floor)),
                        Floor = floor
                    };
                }
            }
        }

        private static IEnumerable<object> ExtractObjects(object rootObject, Type wantedType)
        {
            HashSet<object> visited = new HashSet<object>();
            foreach (object obj in ExtractObjects(rootObject, wantedType, 0, visited))
                yield return obj;
        }

        private static IEnumerable<object> ExtractObjects(object rootObject, Type wantedType, int depth, HashSet<object> visited)
        {
            if (rootObject == null || depth > 3 || visited.Contains(rootObject))
                yield break;
            visited.Add(rootObject);
            if (wantedType.IsInstanceOfType(rootObject))
            {
                yield return rootObject;
                yield break;
            }
            IEnumerable enumerable = rootObject as IEnumerable;
            if (enumerable != null && !(rootObject is string))
            {
                foreach (object item in enumerable)
                {
                    foreach (object found in ExtractObjects(item, wantedType, depth + 1, visited))
                        yield return found;
                }
            }
            foreach (FieldInfo field in rootObject.GetType().GetFields(AccessTools.all))
            {
                if (field.FieldType == typeof(string) || field.FieldType.IsPrimitive)
                    continue;
                foreach (object found in ExtractObjects(field.GetValue(rootObject), wantedType, depth + 1, visited))
                    yield return found;
            }
        }

        private static bool IsRoleBookUnlocked(LorId id)
        {
            return LogueBookModels.booklist != null && LogueBookModels.booklist.Exists(x => x.BookId == id || x.ClassInfo.id == id);
        }

        private static bool IsBattleCardUnlocked(LorId id)
        {
            return LogueBookModels.GetCardList(false, true).Exists(x => x.GetID() == id);
        }

        private static bool IsAbnormalityUnlocked(RewardPassiveInfo info)
        {
            return LogueBookModels.selectedEmotion != null && LogueBookModels.selectedEmotion.Exists(x => x.id == info.id)
                || RMRAbnormalityUnlockManager.GetUnlockedEmotionCardsForBattle().Exists(x => x.id == info.id);
        }

        private static AbnormalityCard GetAbnormalityCardDesc(EmotionCardXmlInfo card, RewardPassiveInfo info)
        {
            if (card == null || card.Script == null || card.Script.Count == 0)
                return null;
            AbnormalityCard desc = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(card.Script[0]);
            PickUpModelBase pickUp = LogLikeMod.FindPickUp(card.Script[0]);
            if (pickUp != null && desc != null)
            {
                desc.cardName = pickUp.Name;
                desc.flavorText = pickUp.FlaverText;
                desc.abilityDesc = pickUp.Desc;
            }
            return desc;
        }

        private static Sprite GetBookArtwork(BookXmlInfo info)
        {
            try { return info.GetThumbSprite(); }
            catch { return null; }
        }

        private static LorId GetLorId(object obj, string name)
        {
            object value = GetMember(obj, name);
            if (value is LorId)
                return (LorId)value;
            return LorId.None;
        }

        private static int GetIntMember(object obj, string name, int fallback)
        {
            object value = GetMember(obj, name);
            return value is int ? (int)value : fallback;
        }

        private static object GetMember(object obj, string name)
        {
            if (obj == null)
                return null;
            FieldInfo field = obj.GetType().GetField(name, AccessTools.all);
            if (field != null)
                return field.GetValue(obj);
            PropertyInfo prop = obj.GetType().GetProperty(name, AccessTools.all);
            return prop == null ? null : prop.GetValue(obj, null);
        }

        private static string GetDisplayName(object obj, LorId id)
        {
            foreach (string name in new[] { "Name", "name", "BookName", "bookName", "cardName" })
            {
                object value = GetMember(obj, name);
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    return value.ToString();
            }
            return id.ToString();
        }

        private static AtlasSection SectionFromChapter(int chapter)
        {
            if (chapter <= 1)
                return AtlasSection.Rumor;
            if (chapter == 2)
                return AtlasSection.UrbanLegend;
            if (chapter == 3)
                return AtlasSection.UrbanMyth;
            if (chapter == 4)
                return AtlasSection.UrbanIllness;
            if (chapter == 5)
                return AtlasSection.UrbanNightmare;
            if (chapter == 6)
                return AtlasSection.UrbanStar;
            return AtlasSection.Impurity;
        }

        private static AtlasSection SectionFromTier(int tier)
        {
            if (tier == 1)
                return AtlasSection.Rumor;
            if (tier == 2)
                return AtlasSection.UrbanIllness;
            return AtlasSection.UrbanStar;
        }

        private static SephirahType GetSephirah(EmotionEgoXmlInfo ego)
        {
            object value = GetMember(ego, "Sephirah");
            if (value is SephirahType)
                return (SephirahType)value;
            return SephirahType.None;
        }

        private static SephirahType GetFloorFromScript(string script)
        {
            int tier = RMRAbnormalityUnlockManager.GetTierForScript(script);
            if (tier == 1)
                return SephirahType.Malkuth;
            if (tier == 2)
                return SephirahType.Tiphereth;
            return SephirahType.Keter;
        }

        private static int TierFromFloor(SephirahType floor)
        {
            if (floor == SephirahType.Malkuth || floor == SephirahType.Yesod || floor == SephirahType.Hod || floor == SephirahType.Netzach)
                return 1;
            if (floor == SephirahType.Tiphereth || floor == SephirahType.Gebura || floor == SephirahType.Chesed)
                return 2;
            return 3;
        }

        private static string GetSectionName(AtlasSection section)
        {
            switch (section)
            {
                case AtlasSection.Rumor: return "传闻";
                case AtlasSection.UrbanLegend: return "都市怪谈";
                case AtlasSection.UrbanMyth: return "都市传说";
                case AtlasSection.UrbanIllness: return "都市恶疾";
                case AtlasSection.UrbanNightmare: return "都市梦魇";
                case AtlasSection.UrbanStar: return "都市之星";
                default: return "杂质";
            }
        }

        private static string GetCategoryName(AtlasCategory category)
        {
            switch (category)
            {
                case AtlasCategory.RoleBook: return "角色书页";
                case AtlasCategory.BattleCard: return "战斗书页";
                case AtlasCategory.AbnormalityPage: return "异想体书页";
                default: return "EGO书页";
            }
        }

        public class LogAtlasTile : MonoBehaviour
        {
            private AtlasEntry entry;
            private Image image;
            private Image artwork;
            private TextMeshProUGUI title;
            private UILogCustomSelectable selectable;

            public void Init(AtlasEntry value)
            {
                entry = value;
                if (entry == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                gameObject.SetActive(true);
                image = image ?? GetComponent<Image>();
                image.sprite = LogLikeMod.ArtWorks["ShopGoodRewardFrame"];
                image.color = entry.Unlocked ? UIColorManager.Manager.GetUIColor(UIColor.Default) : new Color(0.35f, 0.35f, 0.35f, 1f);
                if (artwork == null)
                    artwork = ModdingUtils.CreateImage(transform, (Sprite)null, Vector2.one, new Vector2(0f, 10f), new Vector2(64f, 64f));
                if (title == null)
                    title = ModdingUtils.CreateText_TMP(transform, new Vector2(0f, -36f), 19, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, Color.white, LogLikeMod.DefFont_TMP);
                artwork.sprite = entry.Unlocked ? entry.Artwork : LogLikeMod.ArtWorks["ItemNotFoundIcon"];
                artwork.enabled = artwork.sprite != null;
                title.text = entry.Unlocked ? entry.Title : LockedTitle;
                if (selectable == null)
                {
                    selectable = gameObject.AddComponent<UILogCustomSelectable>();
                    selectable.targetGraphic = image;
                    selectable.SelectEvent = new UnityEventBasedata();
                    selectable.SelectEvent.AddListener((UnityAction<BaseEventData>)(e => OnEnter()));
                    selectable.DeselectEvent = new UnityEventBasedata();
                    selectable.DeselectEvent.AddListener((UnityAction<BaseEventData>)(e => OnExit()));
                }
            }

            private void OnEnter()
            {
                if (entry == null)
                    return;
                string name = entry.Unlocked ? entry.Title : LockedTitle;
                string desc = entry.Unlocked ? entry.Description : "尚未解锁。";
                SingletonBehavior<UIMainOverlayManager>.Instance.SetTooltip(name, desc, transform as RectTransform, Rarity.Special, UIToolTipPanelType.OnlyContent);
            }

            private void OnExit()
            {
                SingletonBehavior<UIMainOverlayManager>.Instance.Close();
            }
        }
    }
}
