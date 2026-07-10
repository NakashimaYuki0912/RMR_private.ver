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
        private const string AtlasUpgradeToggleLabel = "\u663e\u793a\u5347\u7ea7\u7248";

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
        private GameObject upgradeToggleRoot;
        private Image upgradeToggleFrame;
        private TextMeshProUGUI upgradeToggleLabel;
        private AtlasSection currentSection = AtlasSection.Rumor;
        private AtlasCategory currentCategory = AtlasCategory.RoleBook;
        private bool showUpgradedBattleCards;

        // Detail panel fields
        private GameObject detailPanelRoot;
        private Image detailArtwork;
        private TextMeshProUGUI detailTitle;
        private TextMeshProUGUI detailDescription;
        private AtlasEntry currentDetailEntry;

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
            CreateBattleCardUpgradeToggle();
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Image image = ModdingUtils.CreateImage(root.transform, (Sprite)null, Vector2.one, new Vector2(col * 120 - 420, 250 - row * 105), new Vector2(92f, 92f));
                    tiles.Add(image.gameObject.AddComponent<LogAtlasTile>());
                }
            }
            CreateDetailPanel();
            root.SetActive(true);
        }

        private void CreateDetailPanel()
        {
            // Detail panel on the right side of the tile grid
            detailPanelRoot = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one, new Vector2(590f, 120f), new Vector2(280f, 460f)).gameObject;

            // Title text at top of detail panel
            detailTitle = ModdingUtils.CreateText_TMP(detailPanelRoot.transform, new Vector2(0f, 200f), 22, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, Color.white, LogLikeMod.DefFont_TMP);
            detailTitle.alignment = TextAlignmentOptions.Center;
            detailTitle.text = "";

            // Artwork in the middle
            detailArtwork = ModdingUtils.CreateImage(detailPanelRoot.transform, (Sprite)null, Vector2.one, new Vector2(0f, 80f), new Vector2(180f, 180f));
            detailArtwork.preserveAspect = true;

            // Description text below artwork
            detailDescription = ModdingUtils.CreateText_TMP(detailPanelRoot.transform, new Vector2(0f, -160f), 16, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.TopLeft, Color.white, LogLikeMod.DefFont_TMP);
            detailDescription.alignment = TextAlignmentOptions.TopLeft;
            detailDescription.text = "";

            detailPanelRoot.SetActive(true);
        }

        /// <summary>
        /// Populates the detail panel with information for the given atlas entry.
        /// </summary>
        public void ShowDetail(AtlasEntry entry)
        {
            currentDetailEntry = entry;
            if (entry == null || detailPanelRoot == null || detailTitle == null || detailArtwork == null || detailDescription == null)
                return;

            try
            {
                if (!entry.Unlocked)
                {
                    detailTitle.text = "???";
                    detailArtwork.sprite = LogLikeMod.ArtWorks.ContainsKey("ItemNotFoundIcon") ? LogLikeMod.ArtWorks["ItemNotFoundIcon"] : null;
                    detailArtwork.enabled = detailArtwork.sprite != null;
                    ApplyArtworkLayout(detailArtwork, AtlasCategory.RoleBook, true);
                    detailDescription.text = "尚未解锁。";
                    return;
                }

                detailTitle.text = entry.Title ?? "?";
                Sprite art = entry.Artwork;
                if (art == null && LogLikeMod.ArtWorks.ContainsKey("ItemNotFoundIcon"))
                    art = LogLikeMod.ArtWorks["ItemNotFoundIcon"];
                detailArtwork.sprite = art;
                detailArtwork.enabled = art != null;
                ApplyArtworkLayout(detailArtwork, entry.Category, true);
                detailDescription.text = entry.Description ?? "";

                // Enrich detail with category-specific extra info
                try
                {
                    string extraInfo = BuildDetailExtraInfo(entry);
                    if (!string.IsNullOrEmpty(extraInfo))
                        detailDescription.text = extraInfo;
                }
                catch { /* detail enrichment is optional */ }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[RMR Atlas] ShowDetail error: {ex.Message}");
            }
        }

        /// <summary>
        /// Builds enriched detail text for the detail panel, based on category.
        /// </summary>
        private static string BuildDetailExtraInfo(AtlasEntry entry)
        {
            if (entry == null || !entry.Unlocked)
                return null;

            switch (entry.Category)
            {
                case AtlasCategory.RoleBook:
                    return BuildRoleBookDetail(entry.Id);
                case AtlasCategory.BattleCard:
                    return BuildBattleCardDetail(entry.Id, entry.Title);
                case AtlasCategory.AbnormalityPage:
                    return BuildAbnormalityPageDetail(entry.Id);
                case AtlasCategory.EgoPage:
                    return BuildEgoPageDetail(entry.Id);
                default:
                    return null;
            }
        }

        private static string BuildRoleBookDetail(LorId id)
        {
            try
            {
                BookXmlInfo book = Singleton<BookXmlList>.Instance.GetData(id);
                if (book == null)
                    return null;
                var lines = new List<string>();
                string displayName = RewardingModel.GetLocalizedBookName(book);
                if (string.IsNullOrEmpty(displayName) && book.CharacterSkin != null && book.CharacterSkin.Count > 0)
                    displayName = book.CharacterSkin[0];
                if (string.IsNullOrEmpty(displayName))
                    displayName = id.ToString();
                lines.Add("名称: " + displayName);
                lines.Add("HP: " + book.EquipEffect.Hp.ToString());
                lines.Add("速度: " + book.EquipEffect.SpeedMin.ToString() + "-" + book.EquipEffect.Speed.ToString());
                lines.Add("速度骰子: " + book.EquipEffect.SpeedDiceNum.ToString());
                lines.Add("耐性: 斩" + ResToText(book.EquipEffect.SResist) + " 突" + ResToText(book.EquipEffect.PResist) + " 打" + ResToText(book.EquipEffect.HResist));
                lines.Add("混乱耐性: 斩" + ResToText(book.EquipEffect.SBResist) + " 突" + ResToText(book.EquipEffect.PBResist) + " 打" + ResToText(book.EquipEffect.HBResist));
                if (book.EquipEffect.PassiveList != null && book.EquipEffect.PassiveList.Count > 0)
                {
                    lines.Add("被动:");
                    foreach (LorId pid in book.EquipEffect.PassiveList)
                    {
                        string pName = RewardingModel.GetPassiveName(pid);
                        string pDesc = RewardingModel.SanitizeDisplayText(Singleton<PassiveDescXmlList>.Instance.GetDesc(pid));
                        if (!string.IsNullOrEmpty(pName))
                            lines.Add("  • " + pName + ": " + (pDesc ?? ""));
                        else
                            lines.Add("  • " + pid.ToString());
                    }
                }
                lines.Add("ID: " + id.ToString());
                return string.Join("\n", lines.ToArray());
            }
            catch
            {
                return null;
            }
        }

        private static string BuildBattleCardDetail(LorId id, string title)
        {
            try
            {
                DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(id, true);
                if (card == null)
                    return null;
                var lines = new List<string>();
                lines.Add("卡名: " + title);
                lines.Add("费用: " + card.Spec.Cost.ToString());
                lines.Add("章节: " + card.Chapter.ToString());
                // Rarity is implicit from chapter; skip explicit rarity lookup
                if (!string.IsNullOrEmpty(card.Script))
                    lines.Add("能力: " + card.Script);
                if (card.DiceBehaviourList != null && card.DiceBehaviourList.Count > 0)
                {
                    lines.Add("骰子:");
                    for (int i = 0; i < card.DiceBehaviourList.Count; i++)
                    {
                        DiceBehaviour dice = card.DiceBehaviourList[i];
                        string script = string.IsNullOrEmpty(dice.Script) ? "" : " / " + dice.Script;
                        lines.Add("  " + (i + 1).ToString() + ". " + GetDiceDetailText(dice.Detail) + " " + dice.Min.ToString() + "-" + dice.Dice.ToString() + script);
                    }
                }
                lines.Add("ID: " + id.ToString());
                return string.Join("\n", lines.ToArray());
            }
            catch
            {
                return null;
            }
        }

        private static string BuildAbnormalityPageDetail(LorId id)
        {
            try
            {
                RewardPassiveInfo info = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id);
                if (info == null)
                    return null;
                EmotionCardXmlInfo card = LogLikeMod.GetRegisteredPickUpXml(info);
                AbnormalityCard desc = GetAbnormalityCardDesc(card, info);
                var lines = new List<string>();
                string name = (desc != null && !string.IsNullOrEmpty(desc.cardName)) ? desc.cardName : info.script;
                string ability = desc != null ? desc.abilityDesc : (info.script ?? "");
                string flavor = desc != null ? desc.flavorText : "";
                SephirahType floor = RMRAbnormalityUnlockManager.GetFloorForScript(info.script);
                bool atlasUnlocked = LogueBookModels.IsAtlasAbnormalityPageUnlocked(id);

                lines.Add("名称: " + name);
                if (!string.IsNullOrEmpty(ability))
                    lines.Add("能力: " + ability);
                if (!string.IsNullOrEmpty(flavor))
                    lines.Add("描述: " + flavor);
                lines.Add("楼层: " + (RMRRealizationManager.FloorDisplayNames.TryGetValue(floor, out string floorName) ? floorName : floor.ToString()));
                if (atlasUnlocked)
                    lines.Add("图鉴: 已解锁");
                else
                    lines.Add("图鉴: 未解锁");
                lines.Add("ID: " + id.ToString());
                return string.Join("\n", lines.ToArray());
            }
            catch
            {
                return null;
            }
        }

        private static string BuildEgoPageDetail(LorId id)
        {
            // E.G.O. pages are battle cards, so reuse battle card detail
            return BuildBattleCardDetail(id, "");
        }

        private static string ResToText(AtkResist res)
        {
            switch (res)
            {
                case AtkResist.Weak: return "耐性";     // 耐性
                case AtkResist.Normal: return "一般";    // 一般
                case AtkResist.Endure: return "耐受";    // 耐受
                case AtkResist.Immune: return "免疫";    // 免疫
                case AtkResist.Vulnerable: return "脆弱"; // 脆弱
                default: return res.ToString();
            }
        }

        /// <summary>
        /// Gets the card artwork sprite for a battle card or E.G.O. card.
        /// Vanilla cards use AssetBundleManagerRemake; mod cards use CustomizingCardArtworkLoader.
        /// </summary>
        public static Sprite GetCardArtwork(DiceCardXmlInfo card)
        {
            if (card == null)
                return null;
            try
            {
                string packageId = !string.IsNullOrEmpty(card.workshopID) ? card.workshopID : card.id.packageId;
                string[] artworkKeys = new string[2]
                {
                    card.Artwork,
                    card.id.id.ToString()
                };

                if (!string.IsNullOrEmpty(packageId) && LorId.IsModId(packageId))
                {
                    foreach (string artworkKey in artworkKeys)
                    {
                        if (string.IsNullOrEmpty(artworkKey))
                            continue;
                        try
                        {
                            Sprite sprite = Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(packageId, artworkKey);
                            if (sprite != null)
                                return sprite;
                        }
                        catch { }
                    }
                }

                foreach (string artworkKey in artworkKeys)
                {
                    if (string.IsNullOrEmpty(artworkKey))
                        continue;
                    try
                    {
                        Sprite sprite = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(artworkKey);
                        if (sprite != null)
                            return sprite;
                    }
                    catch { }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static Sprite GetCardArtworkById(LorId id)
        {
            if (id == LorId.None)
                return null;
            try
            {
                DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(id, true);
                return GetCardArtwork(card);
            }
            catch
            {
                return null;
            }
        }

        public void SetActive(bool value)
        {
            if (value)
            {
                Init();
                LogueBookModels.EnsureAtlasUnlocks();
                LogueBookModels.SyncCurrentInventoryToPermanentAtlas();
                UpdateTiles();
                Debug.Log($"[RMR Atlas] Opened. Unlocked: RoleBooks={LogueBookModels.AtlasUnlockedRoleBooks?.Count ?? 0}, BattleCards={LogueBookModels.AtlasUnlockedBattleCards?.Count ?? 0}, AbnoPages={LogueBookModels.AtlasUnlockedAbnormalityPages?.Count ?? 0}, EgoPages={LogueBookModels.AtlasUnlockedEgoPages?.Count ?? 0}. Total entries built={BuildEntries().Count}.");
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

        private void CreateBattleCardUpgradeToggle()
        {
            Image image = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one, new Vector2(-470f, -390f), new Vector2(240f, 42f));
            upgradeToggleRoot = image.gameObject;
            upgradeToggleFrame = image;
            Button button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener((UnityAction)(() =>
            {
                showUpgradedBattleCards = !showUpgradedBattleCards;
                UpdateTiles();
            }));
            upgradeToggleLabel = ModdingUtils.CreateText_TMP(image.transform, Vector2.zero, 20, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            SetBattleCardUpgradeToggleVisible(false);
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
            SetBattleCardUpgradeToggleVisible(currentCategory == AtlasCategory.BattleCard);

            // Abnormality / E.G.O. pages: flat list (no urban-chapter section filter).
            bool flatCategory = currentCategory == AtlasCategory.AbnormalityPage
                || currentCategory == AtlasCategory.EgoPage;
            for (int i = 0; i < sectionLabels.Count; i++)
            {
                if (sectionLabels[i] != null)
                    sectionLabels[i].gameObject.SetActive(!flatCategory);
            }

            List<AtlasEntry> entries = BuildEntries(showUpgradedBattleCards)
                .Where(x => x.Category == currentCategory)
                .Where(x => flatCategory || x.Section == currentSection)
                .OrderByDescending(x => x.Unlocked)
                .ThenBy(x => (x.Category == AtlasCategory.EgoPage || x.Category == AtlasCategory.AbnormalityPage)
                    ? (int)x.Floor : 0)
                .ThenBy(x => x.Title)
                .ToList();

            for (int i = 0; i < tiles.Count; i++)
                tiles[i].Init(i < entries.Count ? entries[i] : null);
        }

        public static List<AtlasEntry> BuildEntries(bool showUpgradedBattleCards = false)
        {
            List<AtlasEntry> entries = new List<AtlasEntry>();
            entries.AddRange(BuildRoleBookEntries());
            entries.AddRange(BuildBattleCardEntries(showUpgradedBattleCards));
            entries.AddRange(BuildAbnormalityEntries());
            entries.AddRange(BuildEgoEntries());
            return entries;
        }

        private static IEnumerable<AtlasEntry> BuildRoleBookEntries()
        {
            HashSet<LorId> yielded = new HashSet<LorId>();

            // 1. Yield entries from the full BookXmlList (no artificial .Take limit)
            foreach (object obj in ExtractObjects(Singleton<BookXmlList>.Instance, typeof(BookXmlInfo)))
            {
                BookXmlInfo info = obj as BookXmlInfo;
                LorId id = GetLorId(info, "id");
                if (id == LorId.None || !yielded.Add(id))
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

            // 2. Merge in any unlocked atlas IDs not covered by the BookXmlList enumeration
            LogueBookModels.EnsureAtlasUnlocks();
            if (LogueBookModels.AtlasUnlockedRoleBooks != null)
            {
                foreach (LorId atlasId in LogueBookModels.AtlasUnlockedRoleBooks)
                {
                    if (atlasId == null || atlasId == LorId.None || !yielded.Add(atlasId))
                        continue;
                    BookXmlInfo info = Singleton<BookXmlList>.Instance.GetData(atlasId);
                    if (info == null)
                        continue;
                    yield return new AtlasEntry
                    {
                        Id = atlasId,
                        Title = GetDisplayName(info, atlasId),
                        Description = atlasId.ToString(),
                        Artwork = GetBookArtwork(info),
                        Unlocked = true,
                        Category = AtlasCategory.RoleBook,
                        Section = SectionFromChapter(GetIntMember(info, "Chapter", 1)),
                        Floor = SephirahType.None
                    };
                }
            }
        }

        private static IEnumerable<AtlasEntry> BuildBattleCardEntries(bool showUpgraded)
        {
            HashSet<LorId> yielded = new HashSet<LorId>();

            // 1. Yield entries from the full ItemXmlDataList (no artificial .Take limit)
            List<DiceCardXmlInfo> sourceCards = ExtractObjects(ItemXmlDataList.instance, typeof(DiceCardXmlInfo))
                .OfType<DiceCardXmlInfo>()
                .ToList();
            foreach (DiceCardXmlInfo info in sourceCards)
            {
                LorId id = GetLorId(info, "id");
                if (id == LorId.None || !yielded.Add(id))
                    continue;
                if (RMRAbnormalityUnlockManager.IsRealizationEgoCard(id))
                    continue;
                if (info.CheckUpgradeCard())
                    continue;
                DiceCardXmlInfo displayInfo = GetDisplayCardInfo(info, showUpgraded);
                yield return new AtlasEntry
                {
                    Id = id,
                    Title = GetDisplayName(displayInfo, id),
                    Description = BuildBattleCardDescription(displayInfo, id, showUpgraded),
                    Artwork = GetCardArtwork(displayInfo),
                    Unlocked = IsBattleCardUnlocked(id),
                    Category = AtlasCategory.BattleCard,
                    Section = SectionFromChapter(GetIntMember(info, "Chapter", 1)),
                    Floor = SephirahType.None
                };
            }

            // 2. Merge in any unlocked atlas battle card IDs not covered by the ItemXmlDataList enumeration
            LogueBookModels.EnsureAtlasUnlocks();
            if (LogueBookModels.AtlasUnlockedBattleCards != null)
            {
                foreach (LorId atlasId in LogueBookModels.AtlasUnlockedBattleCards)
                {
                    if (atlasId == null || atlasId == LorId.None || !yielded.Add(atlasId))
                        continue;
                    if (RMRAbnormalityUnlockManager.IsRealizationEgoCard(atlasId))
                        continue;
                    DiceCardXmlInfo info = ItemXmlDataList.instance.GetCardItem(atlasId, true);
                    if (info == null)
                        continue;
                    if (info.CheckUpgradeCard())
                        continue;
                    DiceCardXmlInfo displayInfo = GetDisplayCardInfo(info, showUpgraded);
                    yield return new AtlasEntry
                    {
                        Id = atlasId,
                        Title = GetDisplayName(displayInfo, atlasId),
                        Description = BuildBattleCardDescription(displayInfo, atlasId, showUpgraded),
                        Artwork = GetCardArtwork(displayInfo),
                        Unlocked = true,
                        Category = AtlasCategory.BattleCard,
                        Section = SectionFromChapter(GetIntMember(info, "Chapter", 1)),
                        Floor = SephirahType.None
                    };
                }
            }
        }

        private void SetBattleCardUpgradeToggleVisible(bool visible)
        {
            if (upgradeToggleRoot == null)
                return;
            upgradeToggleRoot.SetActive(visible);
            if (upgradeToggleFrame != null)
                upgradeToggleFrame.color = showUpgradedBattleCards ? UIColorManager.Manager.GetUIColor(UIColor.Highlighted) : UIColorManager.Manager.GetUIColor(UIColor.Default);
            if (upgradeToggleLabel != null)
                upgradeToggleLabel.text = (showUpgradedBattleCards ? "[x] " : "[ ] ") + AtlasUpgradeToggleLabel;
        }

        private static DiceCardXmlInfo GetDisplayCardInfo(DiceCardXmlInfo info, bool showUpgraded)
        {
            if (!showUpgraded)
                return info;
            try
            {
                DiceCardXmlInfo upgraded = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(info.id);
                return upgraded ?? info;
            }
            catch
            {
                return info;
            }
        }

        private static string BuildBattleCardDescription(DiceCardXmlInfo displayInfo, LorId id, bool showUpgraded)
        {
            if (displayInfo == null)
                return id.ToString();
            List<string> lines = new List<string>();
            if (showUpgraded)
                lines.Add("\u5347\u7ea7\u9884\u89c8");
            lines.Add("ID: " + id.ToString());
            if (displayInfo.id != id)
                lines.Add("\u5c55\u793aID: " + displayInfo.id.ToString());
            if (displayInfo.Spec != null)
                lines.Add("\u8d39\u7528: " + displayInfo.Spec.Cost.ToString());
            lines.Add("\u7ae0\u8282: " + displayInfo.Chapter.ToString());
            if (!string.IsNullOrEmpty(displayInfo.Script))
                lines.Add("\u6548\u679c: " + displayInfo.Script);
            if (displayInfo.DiceBehaviourList != null && displayInfo.DiceBehaviourList.Count > 0)
            {
                lines.Add("\u9ab0\u5b50:");
                for (int i = 0; i < displayInfo.DiceBehaviourList.Count; i++)
                {
                    DiceBehaviour dice = displayInfo.DiceBehaviourList[i];
                    string script = string.IsNullOrEmpty(dice.Script) ? string.Empty : " / " + dice.Script;
                    lines.Add((i + 1).ToString() + ". " + GetDiceDetailText(dice.Detail) + " " + dice.Min.ToString() + "-" + dice.Dice.ToString() + script);
                }
            }
            return string.Join("\n", lines.ToArray());
        }

        private static string GetDiceDetailText(BehaviourDetail detail)
        {
            switch (detail)
            {
                case BehaviourDetail.Slash: return "\u65a9\u51fb";
                case BehaviourDetail.Penetrate: return "\u7a81\u523a";
                case BehaviourDetail.Hit: return "\u6253\u51fb";
                case BehaviourDetail.Guard: return "\u9632\u5fa1";
                case BehaviourDetail.Evasion: return "\u95ea\u907f";
                default: return detail.ToString();
            }
        }

        private static IEnumerable<AtlasEntry> BuildAbnormalityEntries()
        {
            HashSet<string> seenAbnormalities = new HashSet<string>();
            foreach (RewardPassivesInfo list in Singleton<RewardPassivesList>.Instance.infos.Where(x => x.rewardtype == PassiveRewardListType.Custom))
            {
                if (list.RewardPassiveList == null)
                    continue;
                foreach (RewardPassiveInfo info in list.RewardPassiveList.Where(x => x.rewardtype == RewardType.Creature && !RMRAbnormalityUnlockManager.IsNoAbnormalityFallback(x.id)))
                {
                    EmotionCardXmlInfo card = LogLikeMod.GetRegisteredPickUpXml(info);
                    string key = GetAbnormalityAtlasKey(card, info);
                    if (!seenAbnormalities.Add(key))
                        continue;
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
            HashSet<LorId> yielded = new HashSet<LorId>();

            // 1. EGO from RewardCardDic_Dummy (mod-defined EGO cards)
            if (LogLikeMod.RewardCardDic_Dummy != null)
            {
                foreach (KeyValuePair<string, List<EmotionEgoXmlInfo>> pair in LogLikeMod.RewardCardDic_Dummy)
                {
                    foreach (EmotionEgoXmlInfo ego in pair.Value)
                    {
                        DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(ego.CardId);
                        if (card == null || !yielded.Add(ego.CardId))
                            continue;
                        SephirahType floor = GetSephirah(ego);
                        yield return new AtlasEntry
                        {
                            Id = ego.CardId,
                            Title = GetDisplayName(card, ego.CardId),
                            Description = floor.ToString(),
                            Artwork = GetCardArtwork(card),
                            Unlocked = LogueBookModels.IsAtlasEgoPageUnlocked(ego.CardId),
                            Category = AtlasCategory.EgoPage,
                            Section = SectionFromTier(TierFromFloor(floor)),
                            Floor = floor
                        };
                    }
                }
            }

            // 2. Realization EGO cards (910001-910090) — always included regardless of RewardCardDic_Dummy
            foreach (var kvp in RMRAbnormalityUnlockManager.RealizationEgoCardsByFloor)
            {
                SephirahType floor = kvp.Key;
                foreach (LorId egoId in kvp.Value)
                {
                    if (!yielded.Add(egoId))
                        continue;
                    DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(egoId, true);
                    if (card == null)
                        continue;
                    yield return new AtlasEntry
                    {
                        Id = egoId,
                        Title = GetDisplayName(card, egoId),
                        Description = floor.ToString(),
                        Artwork = GetCardArtwork(card),
                        Unlocked = LogueBookModels.IsAtlasEgoPageUnlocked(egoId),
                        Category = AtlasCategory.EgoPage,
                        Section = SectionFromTier(RMRAbnormalityUnlockManager.GetTierForFloor(floor)),
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
            return LogueBookModels.IsAtlasRoleBookUnlocked(id)
                || LogueBookModels.booklist != null && LogueBookModels.booklist.Exists(x => x.BookId == id || x.ClassInfo.id == id);
        }

        private static bool IsBattleCardUnlocked(LorId id)
        {
            return LogueBookModels.IsAtlasBattleCardUnlocked(id)
                || LogueBookModels.GetCardList(false, true).Exists(x => x.GetID() == id);
        }

        private static string GetAbnormalityAtlasKey(EmotionCardXmlInfo card, RewardPassiveInfo info)
        {
            if (card != null && card.Script != null && card.Script.Count > 0 && !string.IsNullOrEmpty(card.Script[0]))
                return card.Script[0];
            return info.id.packageId + ":" + info.id.id.ToString();
        }

        private static bool IsAbnormalityUnlocked(RewardPassiveInfo info)
        {
            return LogueBookModels.IsAtlasAbnormalityPageUnlocked(info.id)
                || LogueBookModels.selectedEmotion != null && LogueBookModels.selectedEmotion.Exists(x => x.id == info.id)
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

        private static void ApplyArtworkLayout(Image target, AtlasCategory category, bool detail)
        {
            if (target == null)
                return;
            bool portrait = category == AtlasCategory.BattleCard || category == AtlasCategory.EgoPage;
            target.preserveAspect = true;
            target.rectTransform.sizeDelta = portrait
                ? (detail ? new Vector2(150f, 214f) : new Vector2(50f, 70f))
                : (detail ? new Vector2(180f, 180f) : new Vector2(64f, 64f));
            target.rectTransform.anchoredPosition = detail
                ? new Vector2(0f, portrait ? 76f : 80f)
                : new Vector2(0f, portrait ? 14f : 10f);
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
                {
                    artwork = ModdingUtils.CreateImage(transform, (Sprite)null, Vector2.one, new Vector2(0f, 10f), new Vector2(64f, 64f));
                    artwork.preserveAspect = true;
                }
                if (title == null)
                    title = ModdingUtils.CreateText_TMP(transform, new Vector2(0f, -36f), 19, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, Color.white, LogLikeMod.DefFont_TMP);
                artwork.sprite = entry.Unlocked ? entry.Artwork : LogLikeMod.ArtWorks["ItemNotFoundIcon"];
                artwork.enabled = artwork.sprite != null;
                ApplyArtworkLayout(artwork, entry.Unlocked ? entry.Category : AtlasCategory.RoleBook, false);
                title.text = entry.Unlocked ? entry.Title : LockedTitle;
                if (selectable == null)
                {
                    selectable = gameObject.AddComponent<UILogCustomSelectable>();
                    selectable.targetGraphic = image;
                    selectable.SelectEvent = new UnityEventBasedata();
                    // OnEnter shows tooltip AND populates detail panel
                    selectable.SelectEvent.AddListener((UnityAction<BaseEventData>)(e => OnSelect()));
                    selectable.DeselectEvent = new UnityEventBasedata();
                    selectable.DeselectEvent.AddListener((UnityAction<BaseEventData>)(e => OnExit()));
                }
            }

            private void OnSelect()
            {
                if (entry == null)
                    return;
                string name = entry.Unlocked ? entry.Title : LockedTitle;
                string desc = entry.Unlocked ? entry.Description : "尚未解锁。";
                SingletonBehavior<UIMainOverlayManager>.Instance.SetTooltip(name, desc, transform as RectTransform, Rarity.Special, UIToolTipPanelType.OnlyContent);
                // Populate detail panel safely
                try { Singleton<LogAtlasPanel>.Instance.ShowDetail(entry); } catch { }
            }

            private void OnExit()
            {
                SingletonBehavior<UIMainOverlayManager>.Instance.Close();
            }
        }
    }
}
