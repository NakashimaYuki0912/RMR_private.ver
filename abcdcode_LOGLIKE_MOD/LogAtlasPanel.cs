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
        All = 0,
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
            AtlasSection.All,
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

        // Scheme B: encyclopedia wall — left nav (categories + sections), center cards, right detail.
        private const int CenterCols = 3;
        private const int CenterRows = 7;
        private const int CenterPageSize = CenterCols * CenterRows;

        public GameObject root;
        /// <summary>Full-screen shell under RMR overlay (parent of <see cref="root"/> content).</summary>
        private GameObject _hostRoot;
        private readonly List<LogAtlasTile> tiles = new List<LogAtlasTile>();
        private readonly List<TextMeshProUGUI> sectionLabels = new List<TextMeshProUGUI>();
        private readonly List<TextMeshProUGUI> categoryLabels = new List<TextMeshProUGUI>();
        private readonly List<Image> categoryFrames = new List<Image>();
        private readonly List<Image> sectionFrames = new List<Image>();
        private GameObject upgradeToggleRoot;
        private Image upgradeToggleFrame;
        private TextMeshProUGUI upgradeToggleLabel;
        private TextMeshProUGUI headerTitle;
        private TextMeshProUGUI pageLabel;
        private Button pagePrevBtn;
        private Button pageNextBtn;
        private int currentPage;
        private AtlasSection currentSection = AtlasSection.All;
        private AtlasCategory currentCategory = AtlasCategory.RoleBook;
        private TextMeshProUGUI _emptyHint;
        private bool showUpgradedBattleCards;

        // Detail panel fields
        private GameObject detailPanelRoot;
        private Image detailArtwork;
        private TextMeshProUGUI detailTitle;
        private TextMeshProUGUI detailDescription;
        private TextMeshProUGUI detailMeta;
        private AtlasEntry currentDetailEntry;
        private bool _openedFromHub;
        private GameObject _hubCloseBtn;
        private Action _onHubClose;

        /// <summary>True while atlas UI is shown (hub or legacy).</summary>
        public bool IsVisible =>
            (root != null && root.activeInHierarchy)
            || (_hostRoot != null && _hostRoot.activeInHierarchy);

        private static readonly Color ColGold = new Color(0.93f, 0.76f, 0.42f, 1f);
        private static readonly Color ColGoldDim = new Color(0.62f, 0.48f, 0.26f, 1f);
        private static readonly Color ColCream = new Color(0.93f, 0.88f, 0.78f, 1f);
        private static readonly Color ColMuted = new Color(0.68f, 0.62f, 0.50f, 1f);
        private static readonly Color ColPanel = new Color(0.10f, 0.08f, 0.06f, 0.96f);
        private static readonly Color ColNavIdle = new Color(0.16f, 0.13f, 0.10f, 0.98f);
        private static readonly Color ColNavOn = new Color(0.28f, 0.22f, 0.12f, 1f);

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
                if (_hostRoot != null)
                    _hostRoot.SetActive(true);
                EnsureHubCloseButton();
                return;
            }

            // Prefer BattleSetting clone only when that panel is live; else overlay.
            root = TryCreateBattleSettingCloneRoot();
            if (root == null)
                root = CreateOverlayCanvasRoot();
            BuildSchemeBChrome();
            EnsureHubCloseButton();
            root.SetActive(true);
        }

        /// <summary>
        /// Open atlas from RMR start hub (invitation). Not from battle prepare tabs.
        /// Always builds on RMR overlay — never depends on UIBattleSettingPanel.
        /// </summary>
        public void ShowFromHub(Action onClose = null)
        {
            _openedFromHub = true;
            _onHubClose = onClose;
            try { LogLikeMod.InvalidateTmpFontCache(); } catch { }
            try
            {
                LogueBookModels.EnsureAtlasUnlocks();
                LogueBookModels.SyncCurrentInventoryToPermanentAtlas();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR Atlas] EnsureAtlasUnlocks: " + ex.Message);
            }

            try
            {
                // Full rebuild every open — stale BattleSetting clones / half-built hosts break hub.
                DestroyAtlasUiTree();
                currentPage = 0;

                root = CreateOverlayCanvasRoot();
                if (root == null)
                    throw new InvalidOperationException("CreateOverlayCanvasRoot returned null");

                BuildSchemeBChrome();
                EnsureHubCloseButton();

                if (_hostRoot != null)
                {
                    _hostRoot.SetActive(true);
                    try { _hostRoot.transform.SetAsLastSibling(); } catch { }
                }
                root.SetActive(true);
                try { root.transform.SetAsLastSibling(); } catch { }

                // Bring overlay canvas above invitation UI.
                try
                {
                    Transform overlay = RMRRealizationLaunchHost.GetOrCreateOverlayRoot();
                    if (overlay != null)
                    {
                        overlay.SetAsLastSibling();
                        if (overlay.GetComponent<GraphicRaycaster>() == null)
                            overlay.gameObject.AddComponent<GraphicRaycaster>();
                    }
                }
                catch { }

                try
                {
                    // Ensure book/card localize before building titles.
                    try
                    {
                        string lang = TextDataModel.CurrentLanguage.ToString();
                        LogLikeMod.RefreshVanillaBookAndPassiveLocalize(lang, "Atlas.ShowFromHub");
                        LogLikeMod.EnsureLocalizedFonts("Atlas.ShowFromHub", repairActiveUi: true);
                    }
                    catch { }

                    UpdateTiles();
                    int total = 0;
                    try { total = BuildEntries(false).Count; } catch { }
                    Debug.Log($"[RMR Atlas] Opened from start hub. entries≈{total} tiles={tiles.Count} "
                        + $"host={(_hostRoot != null ? _hostRoot.name : "null")} root={(root != null ? root.name : "null")}");
                }
                catch (Exception tileEx)
                {
                    Debug.LogWarning("[RMR Atlas] UpdateTiles failed (UI still shown): " + tileEx);
                    try { ShowEmptyHint("图鉴数据加载失败，请查看日志。"); } catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMR Atlas] ShowFromHub failed: " + ex);
                try { DestroyAtlasUiTree(); } catch { }
                _openedFromHub = false;
                Action cb = _onHubClose;
                _onHubClose = null;
                try { cb?.Invoke(); } catch { }
                // Do not rethrow — hub already restored.
            }
        }

        public void CloseFromHub()
        {
            _openedFromHub = false;
            // Hide shell so black host does not cover restored hub.
            if (_hostRoot != null)
                _hostRoot.SetActive(false);
            else if (root != null)
                root.SetActive(false);
            Action cb = _onHubClose;
            _onHubClose = null;
            try { cb?.Invoke(); } catch { }
        }

        private void BuildSchemeBChrome()
        {
            headerTitle = CreateLabel(root.transform, new Vector2(0f, 390f), 28, ColGold, TextAlignmentOptions.Center);
            headerTitle.text = "\u6536\u85cf\u56fe\u9274"; // 收藏图鉴
            TextMeshProUGUI sub = CreateLabel(root.transform, new Vector2(0f, 360f), 14, ColGoldDim, TextAlignmentOptions.Center);
            sub.text = "LIBRARY ATLAS  \u00b7  ENCYCLOPEDIA";

            CreateLeftNav();
            CreateBattleCardUpgradeToggle();
            CreateCenterCards();
            CreatePageControls();
            CreateDetailPanel();
        }

        private void DestroyAtlasUiTree()
        {
            try
            {
                if (_hostRoot != null)
                {
                    UnityEngine.Object.Destroy(_hostRoot);
                }
                else if (root != null)
                {
                    Transform p = null;
                    try { p = root.transform != null ? root.transform.parent : null; } catch { }
                    if (p != null && p.name == "LogAtlasOverlayHost")
                        UnityEngine.Object.Destroy(p.gameObject);
                    else
                        UnityEngine.Object.Destroy(root);
                }
            }
            catch { }
            _hostRoot = null;
            root = null;
            tiles.Clear();
            sectionLabels.Clear();
            categoryLabels.Clear();
            categoryFrames.Clear();
            sectionFrames.Clear();
            upgradeToggleRoot = null;
            upgradeToggleFrame = null;
            upgradeToggleLabel = null;
            detailPanelRoot = null;
            detailArtwork = null;
            detailTitle = null;
            detailDescription = null;
            detailMeta = null;
            headerTitle = null;
            pageLabel = null;
            pagePrevBtn = null;
            pageNextBtn = null;
            _hubCloseBtn = null;
        }

        public bool TryHandleBack()
        {
            if (!IsVisible && (root == null || !root.activeSelf))
                return false;
            if (_openedFromHub)
            {
                CloseFromHub();
                return true;
            }
            SetActive(false);
            return true;
        }

        private static GameObject TryCreateBattleSettingCloneRoot()
        {
            try
            {
                // Only use when BattleSetting is actually the live UI phase.
                // At invitation hub, clone parents under inactive prepare tree → invisible.
                if (UI.UIController.Instance == null)
                    return null;
                var panel = UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel;
                if (panel == null || panel.gameObject == null || !panel.gameObject.activeInHierarchy)
                    return null;
                if (panel.EditPanel == null || panel.EditPanel.BattleCardPanel == null)
                    return null;
                return GetLogUIObj(2);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR Atlas] BattleSetting clone failed, using overlay: " + ex.Message);
                return null;
            }
        }

        private GameObject CreateOverlayCanvasRoot()
        {
            Transform parent = null;
            try { parent = RMRRealizationLaunchHost.GetOrCreateOverlayRoot(); } catch { }

            // Full-screen host under overlay canvas (must stay under Canvas for raycasts).
            GameObject host = new GameObject("LogAtlasOverlayHost", typeof(RectTransform));
            if (parent != null)
            {
                host.transform.SetParent(parent, false);
                try
                {
                    if (parent.GetComponent<GraphicRaycaster>() == null)
                        parent.gameObject.AddComponent<GraphicRaycaster>();
                    parent.SetAsLastSibling();
                }
                catch { }
            }
            else
            {
                // Last resort: own DontDestroyOnLoad canvas
                var canvasGo = new GameObject("LogAtlasCanvas", typeof(RectTransform));
                UnityEngine.Object.DontDestroyOnLoad(canvasGo);
                var canvas = canvasGo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 9000;
                var scaler = canvasGo.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                scaler.matchWidthOrHeight = 0.5f;
                canvasGo.AddComponent<GraphicRaycaster>();
                host.transform.SetParent(canvasGo.transform, false);
            }

            var hrt = host.GetComponent<RectTransform>();
            hrt.anchorMin = Vector2.zero;
            hrt.anchorMax = Vector2.one;
            hrt.offsetMin = Vector2.zero;
            hrt.offsetMax = Vector2.zero;
            hrt.localScale = Vector3.one;
            hrt.localPosition = Vector3.zero;
            hrt.SetAsLastSibling();

            var bg = host.AddComponent<Image>();
            bg.color = new Color(0.02f, 0.015f, 0.01f, 0.98f);
            bg.raycastTarget = true;

            // Content host centered — child UI uses absolute offsets around origin.
            var content = new GameObject("AtlasContent", typeof(RectTransform));
            content.transform.SetParent(host.transform, false);
            var crt = content.GetComponent<RectTransform>();
            crt.anchorMin = crt.anchorMax = new Vector2(0.5f, 0.5f);
            crt.sizeDelta = new Vector2(1600f, 900f);
            crt.anchoredPosition = Vector2.zero;
            crt.localScale = Vector3.one;
            crt.localPosition = Vector3.zero;

            _hostRoot = host;
            return content;
        }

        /// <summary>
        /// Solid panel image — never hard-depends on ArtWorks / ShopGoodRewardFrame.
        /// </summary>
        private static Image CreatePanelImage(Transform parent, Vector2 pos, Vector2 size, Color color)
        {
            var go = new GameObject("AtlasPanel", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = color;
            img.raycastTarget = true;
            try
            {
                if (LogLikeMod.ArtWorks != null && LogLikeMod.ArtWorks.ContainsKey("ShopGoodRewardFrame"))
                {
                    Sprite sp = LogLikeMod.ArtWorks["ShopGoodRewardFrame"];
                    if (sp != null)
                        img.sprite = sp;
                }
            }
            catch { }
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
            rt.anchoredPosition = pos;
            rt.localScale = Vector3.one;
            return img;
        }

        private static TextMeshProUGUI CreateLabel(Transform parent, Vector2 pos, int size, Color color, TextAlignmentOptions align)
        {
            var go = new GameObject("AtlasLabel", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(400f, size + 16f);
            rt.anchoredPosition = pos;
            try { tmp.font = LogLikeMod.DefFont_TMP; } catch { }
            tmp.fontSize = size;
            tmp.color = color;
            tmp.alignment = align;
            tmp.raycastTarget = false;
            return tmp;
        }

        private void EnsureHubCloseButton()
        {
            if (root == null)
                return;
            if (!_openedFromHub)
            {
                if (_hubCloseBtn != null)
                    _hubCloseBtn.SetActive(false);
                return;
            }
            if (_hubCloseBtn == null)
            {
                Image img = CreatePanelImage(root.transform, new Vector2(0f, -320f), new Vector2(220f, 48f),
                    new Color(0.22f, 0.17f, 0.12f, 1f));
                _hubCloseBtn = img.gameObject;
                Button btn = _hubCloseBtn.AddComponent<Button>();
                btn.targetGraphic = img;
                btn.onClick = new Button.ButtonClickedEvent();
                btn.onClick.AddListener(() =>
                {
                    try { UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click); } catch { }
                    CloseFromHub();
                });
                TextMeshProUGUI backLabel = CreateLabel(_hubCloseBtn.transform, Vector2.zero, 20, ColCream, TextAlignmentOptions.Center);
                backLabel.text = "\u8fd4\u56de"; // 返回
                try
                {
                    var lrt = backLabel.GetComponent<RectTransform>();
                    lrt.anchorMin = Vector2.zero;
                    lrt.anchorMax = Vector2.one;
                    lrt.offsetMin = Vector2.zero;
                    lrt.offsetMax = Vector2.zero;
                    lrt.anchoredPosition = Vector2.zero;
                }
                catch { }
            }
            _hubCloseBtn.SetActive(true);
            try { _hubCloseBtn.transform.SetAsLastSibling(); } catch { }
        }

        /// <summary>
        /// Left nav: 4 fixed category rails (角色 / 战斗 / 异想体 / EGO战斗) + chapter/floor sections.
        /// </summary>
        private void CreateLeftNav()
        {
            // Category column — always includes 异想体书页 + EGO战斗书页 as dedicated rails.
            for (int i = 0; i < Categories.Length; i++)
            {
                AtlasCategory category = Categories[i];
                Image image = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one,
                    new Vector2(-560f, 280f - i * 52f), new Vector2(170f, 46f));
                try { image.color = ColNavIdle; } catch { }
                categoryFrames.Add(image);
                Button button = image.gameObject.AddComponent<Button>();
                button.targetGraphic = image;
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener((UnityAction)(() => SelectCategory(category)));
                TextMeshProUGUI label = ModdingUtils.CreateText_TMP(image.transform, Vector2.zero, 18, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColCream, LogLikeMod.DefFont_TMP);
                label.text = GetCategoryName(category);
                categoryLabels.Add(label);
            }

            // Section / floor filters under categories
            TextMeshProUGUI secHdr = ModdingUtils.CreateText_TMP(root.transform, new Vector2(-560f, 55f), 14, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColGoldDim, LogLikeMod.DefFont_TMP);
            secHdr.text = "\u7ae0\u8282 / \u5206\u7c7b"; // 章节 / 分类

            for (int i = 0; i < Sections.Length; i++)
            {
                AtlasSection section = Sections[i];
                Image image = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one,
                    new Vector2(-560f, 20f - i * 36f), new Vector2(170f, 32f));
                try { image.color = ColNavIdle; } catch { }
                sectionFrames.Add(image);
                Button button = image.gameObject.AddComponent<Button>();
                button.targetGraphic = image;
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener((UnityAction)(() => SelectSection(section)));
                TextMeshProUGUI label = ModdingUtils.CreateText_TMP(image.transform, Vector2.zero, 15, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColMuted, LogLikeMod.DefFont_TMP);
                label.text = GetSectionName(section);
                sectionLabels.Add(label);
            }
        }

        private void CreateCenterCards()
        {
            // Encyclopedia cards: 3×7 horizontal entries in the middle.
            // Use CreatePanelImage — ModdingUtils.CreateImage can fail silently at invitation-time.
            float startX = -250f;
            float startY = 280f;
            float stepX = 220f;
            float stepY = 72f;
            for (int row = 0; row < CenterRows; row++)
            {
                for (int col = 0; col < CenterCols; col++)
                {
                    Image image = CreatePanelImage(root.transform,
                        new Vector2(startX + col * stepX, startY - row * stepY),
                        new Vector2(208f, 64f), ColPanel);
                    tiles.Add(image.gameObject.AddComponent<LogAtlasTile>());
                }
            }

            _emptyHint = CreateLabel(root.transform, new Vector2(-30f, 40f), 18, ColMuted, TextAlignmentOptions.Center);
            _emptyHint.text = "";
            try
            {
                var rt = _emptyHint.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(600f, 80f);
            }
            catch { }
        }

        private void ShowEmptyHint(string msg)
        {
            if (_emptyHint == null)
                return;
            _emptyHint.text = msg ?? "";
            _emptyHint.gameObject.SetActive(!string.IsNullOrEmpty(msg));
        }

        private void CreatePageControls()
        {
            pageLabel = ModdingUtils.CreateText_TMP(root.transform, new Vector2(-30f, -250f), 16, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColCream, LogLikeMod.DefFont_TMP);
            pageLabel.text = "1 / 1";

            Image prevImg = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one, new Vector2(-120f, -250f), new Vector2(70f, 32f));
            pagePrevBtn = prevImg.gameObject.AddComponent<Button>();
            pagePrevBtn.targetGraphic = prevImg;
            pagePrevBtn.onClick = new Button.ButtonClickedEvent();
            pagePrevBtn.onClick.AddListener((UnityAction)(() =>
            {
                if (currentPage > 0)
                {
                    currentPage--;
                    UpdateTiles();
                }
            }));
            ModdingUtils.CreateText_TMP(prevImg.transform, Vector2.zero, 16, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColCream, LogLikeMod.DefFont_TMP).text = "<";

            Image nextImg = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one, new Vector2(60f, -250f), new Vector2(70f, 32f));
            pageNextBtn = nextImg.gameObject.AddComponent<Button>();
            pageNextBtn.targetGraphic = nextImg;
            pageNextBtn.onClick = new Button.ButtonClickedEvent();
            pageNextBtn.onClick.AddListener((UnityAction)(() =>
            {
                currentPage++;
                UpdateTiles();
            }));
            ModdingUtils.CreateText_TMP(nextImg.transform, Vector2.zero, 16, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColCream, LogLikeMod.DefFont_TMP).text = ">";
        }

        private void CreateDetailPanel()
        {
            // Right-side reading desk detail (Scheme B).
            detailPanelRoot = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one, new Vector2(520f, 40f), new Vector2(300f, 520f)).gameObject;
            try
            {
                var img = detailPanelRoot.GetComponent<Image>();
                if (img != null)
                    img.color = ColPanel;
            }
            catch { }

            detailTitle = ModdingUtils.CreateText_TMP(detailPanelRoot.transform, new Vector2(0f, 230f), 22, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColGold, LogLikeMod.DefFont_TMP);
            detailTitle.alignment = TextAlignmentOptions.Center;
            detailTitle.text = "";

            detailMeta = ModdingUtils.CreateText_TMP(detailPanelRoot.transform, new Vector2(0f, 200f), 14, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColMuted, LogLikeMod.DefFont_TMP);
            detailMeta.text = "";

            detailArtwork = ModdingUtils.CreateImage(detailPanelRoot.transform, (Sprite)null, Vector2.one, new Vector2(0f, 80f), new Vector2(180f, 180f));
            detailArtwork.preserveAspect = true;

            detailDescription = ModdingUtils.CreateText_TMP(detailPanelRoot.transform, new Vector2(0f, -150f), 15, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.TopLeft, ColCream, LogLikeMod.DefFont_TMP);
            detailDescription.alignment = TextAlignmentOptions.TopLeft;
            detailDescription.text = "";
            try
            {
                var rt = detailDescription.GetComponent<RectTransform>();
                if (rt != null)
                    rt.sizeDelta = new Vector2(260f, 220f);
            }
            catch { }

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
                    if (detailMeta != null)
                        detailMeta.text = GetCategoryName(entry.Category);
                    detailArtwork.sprite = LogLikeMod.ArtWorks.ContainsKey("ItemNotFoundIcon") ? LogLikeMod.ArtWorks["ItemNotFoundIcon"] : null;
                    detailArtwork.enabled = detailArtwork.sprite != null;
                    ApplyArtworkLayout(detailArtwork, AtlasCategory.RoleBook, true);
                    detailDescription.text = "\u5c1a\u672a\u89e3\u9501\u3002"; // 尚未解锁。
                    return;
                }

                detailTitle.text = entry.Title ?? "?";
                if (detailMeta != null)
                    detailMeta.text = BuildDetailMetaLine(entry);
                Sprite art = entry.Artwork;
                if (art == null && LogLikeMod.ArtWorks.ContainsKey("ItemNotFoundIcon"))
                    art = LogLikeMod.ArtWorks["ItemNotFoundIcon"];
                detailArtwork.sprite = art;
                if (art != null)
                {
                    detailArtwork.color = Color.white;
                    detailArtwork.enabled = true;
                }
                else
                {
                    detailArtwork.color = new Color(0.12f, 0.1f, 0.08f, 1f);
                    detailArtwork.enabled = true;
                }
                ApplyArtworkLayout(detailArtwork, entry.Category, true);
                detailDescription.text = entry.Description ?? "";

                // Enrich detail with category-specific extra info (no raw LorId dump).
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

        private static string BuildDetailMetaLine(AtlasEntry entry)
        {
            if (entry == null)
                return "";
            string cat = GetCategoryName(entry.Category);
            if (entry.Category == AtlasCategory.AbnormalityPage || entry.Category == AtlasCategory.EgoPage)
            {
                string floor = RMRRealizationManager.FloorDisplayNames != null
                    && RMRRealizationManager.FloorDisplayNames.TryGetValue(entry.Floor, out string fn)
                    ? fn : entry.Floor.ToString();
                return cat + "  \u00b7  " + floor;
            }
            return cat + "  \u00b7  " + GetSectionName(entry.Section);
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
                if (!string.IsNullOrEmpty(title))
                    lines.Add(title);
                lines.Add("\u8d39\u7528: " + card.Spec.Cost.ToString());
                lines.Add("\u7ae0\u8282: " + card.Chapter.ToString());
                if (!string.IsNullOrEmpty(card.Script))
                    lines.Add("\u80fd\u529b: " + card.Script);
                if (card.DiceBehaviourList != null && card.DiceBehaviourList.Count > 0)
                {
                    lines.Add("\u9ab0\u5b50:");
                    for (int i = 0; i < card.DiceBehaviourList.Count; i++)
                    {
                        DiceBehaviour dice = card.DiceBehaviourList[i];
                        string script = string.IsNullOrEmpty(dice.Script) ? "" : " / " + dice.Script;
                        lines.Add("  " + (i + 1).ToString() + ". " + GetDiceDetailText(dice.Detail) + " " + dice.Min.ToString() + "-" + dice.Dice.ToString() + script);
                    }
                }
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

                lines.Add(name);
                if (!string.IsNullOrEmpty(ability))
                    lines.Add("\u80fd\u529b: " + ability);
                if (!string.IsNullOrEmpty(flavor))
                    lines.Add(flavor);
                lines.Add("\u697c\u5c42: " + (RMRRealizationManager.FloorDisplayNames.TryGetValue(floor, out string floorName) ? floorName : floor.ToString()));
                lines.Add(atlasUnlocked ? "\u56fe\u9274: \u5df2\u89e3\u9501" : "\u56fe\u9274: \u672a\u89e3\u9501");
                return string.Join("\n", lines.ToArray());
            }
            catch
            {
                return null;
            }
        }

        private static string BuildEgoPageDetail(LorId id)
        {
            // E.G.O. combat pages — battle-card style detail, labeled as EGO.
            string body = BuildBattleCardDetail(id, "");
            if (string.IsNullOrEmpty(body))
                return "E.G.O. \u6218\u6597\u4e66\u9875";
            return "E.G.O.\n" + body;
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
                // Prepare-tab path disabled — atlas is opened from start hub via ShowFromHub.
                // Keep method for legacy callers; still builds panel if forced.
                _openedFromHub = false;
                Init();
                LogueBookModels.EnsureAtlasUnlocks();
                LogueBookModels.SyncCurrentInventoryToPermanentAtlas();
                UpdateTiles();
                EnsureHubCloseButton();
                Debug.Log($"[RMR Atlas] Opened. Unlocked: RoleBooks={LogueBookModels.AtlasUnlockedRoleBooks?.Count ?? 0}, BattleCards={LogueBookModels.AtlasUnlockedBattleCards?.Count ?? 0}, AbnoPages={LogueBookModels.AtlasUnlockedAbnormalityPages?.Count ?? 0}, EgoPages={LogueBookModels.AtlasUnlockedEgoPages?.Count ?? 0}. Total entries built={BuildEntries().Count}.");
            }
            else if (root != null)
            {
                root.SetActive(false);
                _openedFromHub = false;
            }
        }

        private void CreateBattleCardUpgradeToggle()
        {
            Image image = ModdingUtils.CreateImage(root.transform, "ShopGoodRewardFrame", Vector2.one, new Vector2(-560f, -250f), new Vector2(170f, 36f));
            upgradeToggleRoot = image.gameObject;
            upgradeToggleFrame = image;
            Button button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener((UnityAction)(() =>
            {
                showUpgradedBattleCards = !showUpgradedBattleCards;
                currentPage = 0;
                UpdateTiles();
            }));
            upgradeToggleLabel = ModdingUtils.CreateText_TMP(image.transform, Vector2.zero, 15, Vector2.zero, Vector2.one, Vector2.zero, TextAlignmentOptions.Center, ColCream, LogLikeMod.DefFont_TMP);
            SetBattleCardUpgradeToggleVisible(false);
        }

        private void SelectSection(AtlasSection section)
        {
            currentSection = section;
            currentPage = 0;
            UpdateTiles();
        }

        private void SelectCategory(AtlasCategory category)
        {
            currentCategory = category;
            currentPage = 0;
            UpdateTiles();
        }

        private void UpdateTiles()
        {
            // Highlight category rails (always visible: 角色 / 战斗 / 异想体 / EGO战斗).
            for (int i = 0; i < categoryLabels.Count; i++)
            {
                bool on = Categories[i] == currentCategory;
                if (categoryLabels[i] != null)
                    categoryLabels[i].color = on ? ColGold : ColCream;
                if (i < categoryFrames.Count && categoryFrames[i] != null)
                {
                    try { categoryFrames[i].color = on ? ColNavOn : ColNavIdle; } catch { }
                }
            }

            SetBattleCardUpgradeToggleVisible(currentCategory == AtlasCategory.BattleCard);

            // Urban-chapter filter only for role/battle; abno/EGO use flat list (sections hide).
            bool flatCategory = currentCategory == AtlasCategory.AbnormalityPage
                || currentCategory == AtlasCategory.EgoPage;
            for (int i = 0; i < sectionLabels.Count; i++)
            {
                if (sectionLabels[i] != null)
                    sectionLabels[i].gameObject.SetActive(!flatCategory);
                if (i < sectionFrames.Count && sectionFrames[i] != null)
                    sectionFrames[i].gameObject.SetActive(!flatCategory);
            }
            if (!flatCategory)
            {
                for (int i = 0; i < sectionLabels.Count; i++)
                {
                    bool on = Sections[i] == currentSection;
                    if (sectionLabels[i] != null)
                        sectionLabels[i].color = on ? ColGold : ColMuted;
                    if (i < sectionFrames.Count && sectionFrames[i] != null)
                    {
                        try { sectionFrames[i].color = on ? ColNavOn : ColNavIdle; } catch { }
                    }
                }
            }

            List<AtlasEntry> entries;
            try
            {
                entries = BuildEntries(showUpgradedBattleCards)
                    .Where(x => x != null && x.Category == currentCategory)
                    .Where(x => flatCategory || currentSection == AtlasSection.All || x.Section == currentSection)
                    .OrderByDescending(x => x.Unlocked)
                    .ThenBy(x => (x.Category == AtlasCategory.EgoPage || x.Category == AtlasCategory.AbnormalityPage)
                        ? (int)x.Floor : 0)
                    .ThenBy(x => x.Title ?? "")
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMR Atlas] BuildEntries filter failed: " + ex);
                entries = new List<AtlasEntry>();
            }

            Debug.Log($"[RMR Atlas] UpdateTiles cat={currentCategory} sec={currentSection} count={entries.Count}");

            if (entries.Count == 0)
            {
                ShowEmptyHint(flatCategory
                    ? "本分类暂无条目（异想体/EGO 需游玩或解放战后解锁）。"
                    : "本节暂无条目，请点左侧「全部」或切换分类。");
            }
            else
            {
                ShowEmptyHint("");
            }

            int totalPages = Math.Max(1, (entries.Count + CenterPageSize - 1) / CenterPageSize);
            if (currentPage >= totalPages)
                currentPage = totalPages - 1;
            if (currentPage < 0)
                currentPage = 0;
            int start = currentPage * CenterPageSize;

            for (int i = 0; i < tiles.Count; i++)
            {
                int idx = start + i;
                try
                {
                    tiles[i].Init(idx < entries.Count ? entries[idx] : null);
                }
                catch (Exception tileEx)
                {
                    Debug.LogWarning("[RMR Atlas] tile.Init failed: " + tileEx.Message);
                    try { tiles[i].gameObject.SetActive(false); } catch { }
                }
            }

            if (pageLabel != null)
                pageLabel.text = (currentPage + 1).ToString() + " / " + totalPages.ToString();
            if (pagePrevBtn != null)
                pagePrevBtn.interactable = currentPage > 0;
            if (pageNextBtn != null)
                pageNextBtn.interactable = currentPage < totalPages - 1;
        }

        public static List<AtlasEntry> BuildEntries(bool showUpgradedBattleCards = false)
        {
            List<AtlasEntry> entries = new List<AtlasEntry>();
            try { entries.AddRange(BuildRoleBookEntries()); }
            catch (Exception ex) { Debug.LogWarning("[RMR Atlas] BuildRoleBookEntries: " + ex.Message); }
            try { entries.AddRange(BuildBattleCardEntries(showUpgradedBattleCards)); }
            catch (Exception ex) { Debug.LogWarning("[RMR Atlas] BuildBattleCardEntries: " + ex.Message); }
            try { entries.AddRange(BuildAbnormalityEntries()); }
            catch (Exception ex) { Debug.LogWarning("[RMR Atlas] BuildAbnormalityEntries: " + ex.Message); }
            try { entries.AddRange(BuildEgoEntries()); }
            catch (Exception ex) { Debug.LogWarning("[RMR Atlas] BuildEgoEntries: " + ex.Message); }
            return entries;
        }

        private static List<BookXmlInfo> EnumerateAllBooks()
        {
            var result = new List<BookXmlInfo>();
            var seen = new HashSet<string>();
            void Add(BookXmlInfo info)
            {
                if (info == null || info.id == null || info.id == LorId.None)
                    return;
                string key = (info.id.packageId ?? "") + ":" + info.id.id;
                if (!seen.Add(key))
                    return;
                result.Add(info);
            }

            try
            {
                List<BookXmlInfo> list = Singleton<BookXmlList>.Instance?.GetList();
                if (list != null)
                {
                    foreach (BookXmlInfo b in list)
                        Add(b);
                }
            }
            catch (Exception ex) { Debug.LogWarning("[RMR Atlas] BookXmlList.GetList: " + ex.Message); }

            // Workshop packages
            try
            {
                Dictionary<string, List<BookXmlInfo>> workshop = Singleton<BookXmlList>.Instance?.GetAllWorkshopData();
                if (workshop != null)
                {
                    foreach (var kv in workshop)
                    {
                        if (kv.Value == null) continue;
                        foreach (BookXmlInfo b in kv.Value)
                            Add(b);
                    }
                }
            }
            catch { /* ignore */ }

            // Atlas unlocks that might not be in GetList
            try
            {
                LogueBookModels.EnsureAtlasUnlocks();
                if (LogueBookModels.AtlasUnlockedRoleBooks != null)
                {
                    foreach (LorId id in LogueBookModels.AtlasUnlockedRoleBooks)
                    {
                        if (id == null || id == LorId.None) continue;
                        BookXmlInfo b = Singleton<BookXmlList>.Instance?.GetData(id);
                        Add(b);
                    }
                }
            }
            catch { /* ignore */ }

            // Fallback reflection walk if still empty
            if (result.Count == 0)
            {
                try
                {
                    foreach (object obj in ExtractObjects(Singleton<BookXmlList>.Instance, typeof(BookXmlInfo)))
                        Add(obj as BookXmlInfo);
                }
                catch (Exception ex) { Debug.LogWarning("[RMR Atlas] ExtractObjects books: " + ex.Message); }
            }

            return result;
        }

        private static IEnumerable<AtlasEntry> BuildRoleBookEntries()
        {
            foreach (BookXmlInfo info in EnumerateAllBooks())
            {
                if (info == null || info.id == null)
                    continue;
                // Skip error stubs
                try
                {
                    if (info.isError)
                        continue;
                }
                catch { /* ignore */ }

                string title = RewardingModel.GetLocalizedBookName(info);
                if (string.IsNullOrEmpty(title) || RewardingModel.IsPoorDisplayNamePublic(title))
                    title = GetDisplayName(info, info.id);
                if (string.IsNullOrEmpty(title))
                    title = info.id.id.ToString();

                yield return new AtlasEntry
                {
                    Id = info.id,
                    Title = title,
                    Description = title,
                    Artwork = GetBookArtwork(info),
                    Unlocked = IsRoleBookUnlocked(info.id),
                    Category = AtlasCategory.RoleBook,
                    Section = SectionFromChapter(GetIntMember(info, "Chapter", 1)),
                    Floor = SephirahType.None
                };
            }
        }

        private static List<DiceCardXmlInfo> EnumerateAllCards()
        {
            var result = new List<DiceCardXmlInfo>();
            var seen = new HashSet<string>();
            void Add(DiceCardXmlInfo info)
            {
                if (info == null || info.id == null || info.id == LorId.None)
                    return;
                string key = (info.id.packageId ?? "") + ":" + info.id.id;
                if (!seen.Add(key))
                    return;
                result.Add(info);
            }

            // Prefer known APIs over deep reflection (ExtractObjects can throw/stack).
            try
            {
                var instance = ItemXmlDataList.instance;
                if (instance != null)
                {
                    // Try GetList / GetAllCardList via reflection
                    foreach (string methodName in new[] { "GetList", "GetCardList", "GetAllCardList" })
                    {
                        try
                        {
                            var mi = instance.GetType().GetMethod(methodName, AccessTools.all, null, Type.EmptyTypes, null);
                            if (mi == null) continue;
                            object ret = mi.Invoke(instance, null);
                            if (ret is System.Collections.IEnumerable en)
                            {
                                foreach (object o in en)
                                {
                                    if (o is DiceCardXmlInfo c)
                                        Add(c);
                                }
                                if (result.Count > 0)
                                    break;
                            }
                        }
                        catch { /* try next */ }
                    }
                }
            }
            catch { /* ignore */ }

            if (result.Count == 0)
            {
                try
                {
                    foreach (object obj in ExtractObjects(ItemXmlDataList.instance, typeof(DiceCardXmlInfo)))
                        Add(obj as DiceCardXmlInfo);
                }
                catch (Exception ex) { Debug.LogWarning("[RMR Atlas] ExtractObjects cards: " + ex.Message); }
            }

            try
            {
                LogueBookModels.EnsureAtlasUnlocks();
                if (LogueBookModels.AtlasUnlockedBattleCards != null)
                {
                    foreach (LorId id in LogueBookModels.AtlasUnlockedBattleCards)
                    {
                        if (id == null) continue;
                        DiceCardXmlInfo c = ItemXmlDataList.instance?.GetCardItem(id, true);
                        Add(c);
                    }
                }
            }
            catch { /* ignore */ }

            return result;
        }

        private static IEnumerable<AtlasEntry> BuildBattleCardEntries(bool showUpgraded)
        {
            foreach (DiceCardXmlInfo info in EnumerateAllCards())
            {
                if (info == null || info.id == null)
                    continue;
                if (RMRAbnormalityUnlockManager.IsRealizationEgoCard(info.id))
                    continue;
                try
                {
                    if (info.CheckUpgradeCard())
                        continue;
                }
                catch { /* ignore */ }

                DiceCardXmlInfo displayInfo = GetDisplayCardInfo(info, showUpgraded);
                string title = GetDisplayName(displayInfo, info.id);
                try
                {
                    string loc = RewardingModel.GetLocalizedCardName(displayInfo);
                    if (!string.IsNullOrEmpty(loc) && !RewardingModel.IsPoorDisplayNamePublic(loc))
                        title = loc;
                    else if (!string.IsNullOrEmpty(displayInfo.workshopName)
                        && !RewardingModel.IsPoorDisplayNamePublic(displayInfo.workshopName))
                        title = displayInfo.workshopName;
                }
                catch { /* ignore */ }

                yield return new AtlasEntry
                {
                    Id = info.id,
                    Title = title,
                    Description = BuildBattleCardDescription(displayInfo, info.id, showUpgraded),
                    Artwork = GetCardArtwork(displayInfo),
                    Unlocked = IsBattleCardUnlocked(info.id),
                    Category = AtlasCategory.BattleCard,
                    Section = SectionFromChapter(GetIntMember(info, "Chapter", 1)),
                    Floor = SephirahType.None
                };
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
                return "";
            List<string> lines = new List<string>();
            if (showUpgraded)
                lines.Add("\u5347\u7ea7\u9884\u89c8");
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
            var root = Singleton<RewardPassivesList>.Instance;
            if (root?.infos == null)
                yield break;
            foreach (RewardPassivesInfo list in root.infos)
            {
                if (list == null || list.RewardPassiveList == null)
                    continue;
                // Include Custom + any Creature rewards (not only Custom list type).
                foreach (RewardPassiveInfo info in list.RewardPassiveList)
                {
                    if (info == null || info.rewardtype != RewardType.Creature)
                        continue;
                    if (RMRAbnormalityUnlockManager.IsNoAbnormalityFallback(info.id))
                        continue;
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
            try
            {
                if (LogueBookModels.IsAtlasRoleBookUnlocked(id))
                    return true;
            }
            catch { /* ignore */ }
            try
            {
                if (LogueBookModels.booklist != null
                    && LogueBookModels.booklist.Exists(x => x != null
                        && (x.BookId == id || (x.ClassInfo != null && x.ClassInfo.id == id))))
                    return true;
            }
            catch { /* ignore */ }
            return false;
        }

        private static bool IsBattleCardUnlocked(LorId id)
        {
            try
            {
                if (LogueBookModels.IsAtlasBattleCardUnlocked(id))
                    return true;
            }
            catch { /* ignore */ }
            try
            {
                var cards = LogueBookModels.GetCardList(false, true);
                if (cards != null && cards.Exists(x => x != null && x.GetID() == id))
                    return true;
            }
            catch { /* invitation hub may not have run inventory */ }
            return false;
        }

        private static string GetAbnormalityAtlasKey(EmotionCardXmlInfo card, RewardPassiveInfo info)
        {
            if (card != null && card.Script != null && card.Script.Count > 0 && !string.IsNullOrEmpty(card.Script[0]))
                return card.Script[0];
            if (info?.id == null)
                return Guid.NewGuid().ToString();
            return (info.id.packageId ?? "") + ":" + info.id.id.ToString();
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
            if (info != null)
                RMRAbnormalityUnlockManager.EnsureVanillaEmotionPresentation(info, card);
            PickUpModelBase pickUp = LogLikeMod.FindPickUp(card.Script[0]);
            PickUpModel_RMRVanillaEmotion.InjectResolvedDesc(card, pickUp);
            // UI key is card.Name after ApplyVanillaEmotionPresentation (e.g. SnowWhite_Vine).
            string key = !string.IsNullOrEmpty(card.Name) ? card.Name : card.Script[0];
            return Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(key);
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

        // GetSectionName — include 全部

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
                case AtlasSection.All: return "\u5168\u90e8"; // 全部
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
                case AtlasCategory.RoleBook: return "\u89d2\u8272\u4e66\u9875"; // 角色书页
                case AtlasCategory.BattleCard: return "\u6218\u6597\u4e66\u9875"; // 战斗书页
                case AtlasCategory.AbnormalityPage: return "\u5f02\u60f3\u4f53\u4e66\u9875"; // 异想体书页
                case AtlasCategory.EgoPage: return "EGO\u6218\u6597\u4e66\u9875"; // EGO战斗书页
                default: return "EGO\u6218\u6597\u4e66\u9875";
            }
        }

        /// <summary>
        /// Scheme B encyclopedia card: left thumbnail + title + subtitle (not raw ID grid).
        /// </summary>
        public class LogAtlasTile : MonoBehaviour
        {
            private AtlasEntry entry;
            private Image image;
            private Image artwork;
            private TextMeshProUGUI title;
            private TextMeshProUGUI subtitle;
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
                try
                {
                    if (LogLikeMod.ArtWorks != null && LogLikeMod.ArtWorks.ContainsKey("ShopGoodRewardFrame"))
                        image.sprite = LogLikeMod.ArtWorks["ShopGoodRewardFrame"];
                }
                catch { }
                image.color = entry.Unlocked
                    ? new Color(0.14f, 0.11f, 0.08f, 1f)
                    : new Color(0.10f, 0.09f, 0.08f, 0.85f);

                if (artwork == null)
                {
                    var artGo = new GameObject("Art", typeof(RectTransform));
                    artGo.transform.SetParent(transform, false);
                    artwork = artGo.AddComponent<Image>();
                    artwork.preserveAspect = true;
                    artwork.raycastTarget = false;
                    var artRt = artGo.GetComponent<RectTransform>();
                    artRt.anchorMin = artRt.anchorMax = new Vector2(0.5f, 0.5f);
                    artRt.sizeDelta = new Vector2(48f, 48f);
                    artRt.anchoredPosition = new Vector2(-72f, 0f);
                }
                if (title == null)
                {
                    title = CreateLabel(transform, new Vector2(28f, 10f), 16, ColCream, TextAlignmentOptions.Left);
                    try { title.GetComponent<RectTransform>().sizeDelta = new Vector2(140f, 24f); } catch { }
                }
                if (subtitle == null)
                {
                    subtitle = CreateLabel(transform, new Vector2(28f, -12f), 12, ColMuted, TextAlignmentOptions.Left);
                    try { subtitle.GetComponent<RectTransform>().sizeDelta = new Vector2(140f, 20f); } catch { }
                }

                try
                {
                    if (title.font == null && LogLikeMod.DefFont_TMP != null)
                        title.font = LogLikeMod.DefFont_TMP;
                    if (subtitle.font == null && LogLikeMod.DefFont_TMP != null)
                        subtitle.font = LogLikeMod.DefFont_TMP;
                }
                catch { }

                if (entry.Unlocked)
                {
                    artwork.sprite = entry.Artwork;
                    if (artwork.sprite == null && LogLikeMod.ArtWorks != null && LogLikeMod.ArtWorks.ContainsKey("ItemNotFoundIcon"))
                        artwork.sprite = LogLikeMod.ArtWorks["ItemNotFoundIcon"];
                    title.text = Truncate(entry.Title ?? "?", 14);
                    title.color = ColCream;
                    subtitle.text = BuildSubtitle(entry);
                    subtitle.color = ColMuted;
                }
                else
                {
                    artwork.sprite = LogLikeMod.ArtWorks != null && LogLikeMod.ArtWorks.ContainsKey("ItemNotFoundIcon")
                        ? LogLikeMod.ArtWorks["ItemNotFoundIcon"] : null;
                    title.text = LockedTitle;
                    title.color = ColMuted;
                    subtitle.text = "\u672a\u89e3\u9501"; // 未解锁
                    subtitle.color = new Color(0.45f, 0.40f, 0.32f, 1f);
                }
                // Always show art node; use solid tint if no sprite (avoid white default square).
                if (artwork.sprite == null)
                {
                    artwork.sprite = null;
                    artwork.color = new Color(0.2f, 0.16f, 0.12f, 1f);
                    artwork.enabled = true;
                }
                else
                {
                    artwork.color = Color.white;
                    artwork.enabled = true;
                }
                try
                {
                    artwork.rectTransform.sizeDelta = new Vector2(48f, 48f);
                    artwork.rectTransform.anchoredPosition = new Vector2(-72f, 0f);
                }
                catch { }

                if (selectable == null)
                {
                    try
                    {
                        selectable = gameObject.AddComponent<UILogCustomSelectable>();
                        selectable.targetGraphic = image;
                        selectable.SelectEvent = new UnityEventBasedata();
                        selectable.SelectEvent.AddListener((UnityAction<BaseEventData>)(e => OnSelect()));
                        selectable.DeselectEvent = new UnityEventBasedata();
                        selectable.DeselectEvent.AddListener((UnityAction<BaseEventData>)(e => OnExit()));
                    }
                    catch
                    {
                        // Fallback: plain Button if custom selectable missing
                        var btn = gameObject.GetComponent<Button>() ?? gameObject.AddComponent<Button>();
                        btn.targetGraphic = image;
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(() => OnSelect());
                    }
                }
            }

            private static string Truncate(string s, int max)
            {
                if (string.IsNullOrEmpty(s) || s.Length <= max)
                    return s ?? "";
                return s.Substring(0, max) + "\u2026";
            }

            private static string BuildSubtitle(AtlasEntry e)
            {
                if (e == null)
                    return "";
                switch (e.Category)
                {
                    case AtlasCategory.BattleCard:
                    case AtlasCategory.EgoPage:
                        return GetCategoryName(e.Category);
                    case AtlasCategory.AbnormalityPage:
                        if (RMRRealizationManager.FloorDisplayNames != null
                            && RMRRealizationManager.FloorDisplayNames.TryGetValue(e.Floor, out string fn))
                            return fn;
                        return GetCategoryName(e.Category);
                    default:
                        return GetSectionName(e.Section);
                }
            }

            private void OnSelect()
            {
                if (entry == null)
                    return;
                string name = entry.Unlocked ? entry.Title : LockedTitle;
                string desc = entry.Unlocked
                    ? (string.IsNullOrEmpty(entry.Description) ? GetCategoryName(entry.Category) : entry.Description)
                    : "\u5c1a\u672a\u89e3\u9501\u3002";
                try
                {
                    SingletonBehavior<UIMainOverlayManager>.Instance.SetTooltip(name, desc, transform as RectTransform, Rarity.Special, UIToolTipPanelType.OnlyContent);
                }
                catch { }
                try { Singleton<LogAtlasPanel>.Instance.ShowDetail(entry); } catch { }
            }

            private void OnExit()
            {
                try { SingletonBehavior<UIMainOverlayManager>.Instance.Close(); } catch { }
            }
        }
    }
}
