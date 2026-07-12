using System;
using System.Collections.Generic;
using abcdcode_LOGLIKE_MOD;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// Player-facing RMR handbook using the selected H-A illustrated index layout.
    /// </summary>
    public class RMRHelpHandbookPanel : MonoBehaviour
    {
        public static RMRHelpHandbookPanel Instance { get; private set; }

        private GameObject _root;
        private TextMeshProUGUI _bodyText;
        private TextMeshProUGUI _sectionTitle;
        private Image _bannerImage;
        private Image _iconImage;
        private ScrollRect _bodyScroll;
        private RectTransform _bodyContentRt;
        private readonly List<TextMeshProUGUI> _navLabels = new List<TextMeshProUGUI>();
        private readonly List<Image> _navFrames = new List<Image>();
        private int _index;
        public bool IsVisible => _root != null && _root.activeSelf;

        private static readonly Color ColGold = new Color(0.93f, 0.76f, 0.42f, 1f);
        private static readonly Color ColGoldDim = new Color(0.62f, 0.48f, 0.26f, 1f);
        private static readonly Color ColCream = new Color(0.93f, 0.88f, 0.78f, 1f);
        private static readonly Color ColMuted = new Color(0.72f, 0.66f, 0.55f, 1f);
        private static readonly Color ColPanel = new Color(0.11f, 0.09f, 0.07f, 0.98f);
        private static readonly Color ColNavIdle = new Color(0.22f, 0.18f, 0.13f, 1f);
        private static readonly Color ColNavOn = new Color(0.38f, 0.30f, 0.16f, 1f);
        private static readonly Color ColBodyBg = new Color(0.06f, 0.05f, 0.04f, 1f);

        private struct Page
        {
            public string NavKey;
            public string NavZh;
            public string NavEn;
            public string BodyKey;
            public string BodyZh;
            public string BodyEn;
            public string[] ArtKeys;
        }

        private static readonly Page[] Pages = BuildPlayerPages();

        private static Page[] BuildPlayerPages()
        {
            return new[]
            {
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Overview",
                    NavZh = "玩法概览",
                    NavEn = "Overview",
                    BodyKey = "ui_RMR_Help_Body_Overview",
                    ArtKeys = new[] { "随机事件背景1", "MysteryButton_Enable", "Shop_CardUpgrade_Icon" },
                    BodyZh =
                        "欢迎来到 Roguelike Mod Reborn。在这里，每次接待都将成为一段从都市传闻逐步走向杂质的独立旅程。\n\n" +
                        "开始正常游玩后，你会获得初始资源，并在不同类型的路线节点之间作出选择。每次战斗、补给与事件都会改变当前队伍，逐渐形成这一局独有的构筑。\n\n" +
                        "击败章节 Boss 可以进入更危险的都市阶段。最终击败杂质章节 Boss，即可完成本次旅程。\n\n" +
                        "路线中的金币、库存和章节进度只属于当前旅程；图鉴与楼层解放记录则会永久保留，为之后的游玩提供更多选择。",
                    BodyEn =
                        "Every reception becomes a journey from Urban Myth to Impuritas Civitatis. Choose a route, gather resources, and shape a different team in every run.\n\n" +
                        "Money, inventory, and chapter progress belong to the current run. Atlas discoveries and realization clears remain for future journeys."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Route",
                    NavZh = "路线与章节",
                    NavEn = "Route & Chapters",
                    BodyKey = "ui_RMR_Help_Body_Route",
                    ArtKeys = new[] { "随机事件背景3", "Stage_Rest", "Stage_Shop" },
                    BodyZh =
                        "每次完成当前节点后，你都可以从若干候选节点中选择下一站。不同路线会带来不同的战斗强度、奖励与风险。\n\n" +
                        "【普通战】\n旅程中最常见的战斗。适合稳定获取书页、被动和金币，并逐步完善队伍。\n\n" +
                        "【精英战】\n敌人更强，但奖励通常也更好。队伍尚未成形时，需要谨慎判断是否挑战。\n\n" +
                        "【Boss 战】\n击败章节 Boss 后，旅程将进入下一章节，并开放更高等级的敌人、奖励与商品。\n\n" +
                        "【异想体战】\n挑战异想体并获得对应书页。第一、第二章不会生成异想体战斗节点，相关位置会由休息节点替代。\n\n" +
                        "【商店、休息与神秘事件】\n这些节点提供购买、恢复或特殊选择。合理利用它们，往往比连续战斗更重要。\n\n" +
                        "击败杂质章节 Boss 后，本次旅程正式结束。",
                    BodyEn =
                        "Choose the next node after every encounter. Normal, elite, boss, abnormality, shop, rest, and mystery nodes offer different risks and rewards. Chapters one and two use rest nodes instead of abnormality battles. The Impuritas Civitatis boss ends the run."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Rewards",
                    NavZh = "战斗与构筑",
                    NavEn = "Combat & Builds",
                    BodyKey = "ui_RMR_Help_Body_Rewards",
                    ArtKeys = new[] { "异想体战斗", "Shop_CardUpgrade_Icon" },
                    BodyZh =
                        "战斗仍遵循《废墟图书馆》的基本规则。配置核心书页、战斗书页与被动能力，并通过拼点、光芒和情感等级赢得接待。\n\n" +
                        "胜利后可能获得核心书页、战斗书页、被动、异想体书页、E.G.O.、金币或特殊奖励。这些内容会进入当前路线库存。\n\n" +
                        "战斗书页按照“种类”记录。升级后，强化版本会替换原版本；重复升级会逐渐提高费用。\n\n" +
                        "队伍情感等级 1–2 对应 I 阶异想体书页，3–4 对应 II 阶，5 对应 III 阶。只会出现当前路线已经拥有的书页。\n\n" +
                        "中途 E.G.O.选择同样只会从当前路线已经拥有的 E.G.O.中产生。未获得的内容不会提前进入本局选择池。",
                    BodyEn =
                        "Build with key pages, combat pages, and passives. Victories grant resources for the current route. Emotion levels 1–2 offer tier I abnormality pages, 3–4 offer tier II, and 5 offers tier III. Only owned abnormalities and E.G.O. can appear."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Shop",
                    NavZh = "补给与事件",
                    NavEn = "Supplies & Events",
                    BodyKey = "ui_RMR_Help_Body_Shop",
                    ArtKeys = new[] { "Shop_CardUpgrade_Icon", "Stage_Shop", "随机事件背景2" },
                    BodyZh =
                        "【商店】\n使用本次旅程获得的金币购买核心书页、战斗书页、被动、异想体书页、E.G.O.或战斗书页升级。商品受到章节、解放状态和当前库存影响。\n\n" +
                        "购买战斗书页代表获得该书页种类。升级会用新版替换旧版，每次成功升级后，后续升级价格都会提高。\n\n" +
                        "【休息】\n休息节点用于调整旅程节奏，为后续战斗恢复状态或获得休整机会。\n\n" +
                        "【神秘事件】\n不同选择可能带来资源与奖励，也可能要求金币、书页或其他代价。根据当前队伍状态判断风险，也是旅程的重要部分。",
                    BodyEn =
                        "Spend run money in shops on pages, passives, abnormalities, E.G.O., and upgrades. Rest nodes offer recovery. Mystery events present choices that may grant rewards or demand a price."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Atlas",
                    NavZh = "永久图鉴",
                    NavEn = "Permanent Atlas",
                    BodyKey = "ui_RMR_Help_Body_Atlas",
                    ArtKeys = new[] { "随机事件背景2", "Shop_CardUpgrade_Icon" },
                    BodyZh =
                        "旅程中获得的角色书页、战斗书页、异想体书页与 E.G.O.会逐步记录到永久图鉴中。\n\n" +
                        "永久图鉴与当前路线库存并不相同。图鉴内容不会在新路线中自动全部加入库存，每次正常游玩仍需要重新收集和构筑。\n\n" +
                        "永久图鉴用于记录收藏、扩展后续内容池，并为楼层解放战提供编队资源。尚未解锁的项目会以未知状态显示。\n\n" +
                        "“重置永久进度”会清除图鉴与楼层解放记录，请谨慎使用。",
                    BodyEn =
                        "Discoveries are recorded in the permanent Atlas, which is separate from current-run inventory. New runs still require fresh collection and building. Atlas unlocks provide loadout resources for realizations."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Realization",
                    NavZh = "解放战与提示",
                    NavEn = "Realization & Tips",
                    BodyKey = "ui_RMR_Help_Body_Realization",
                    ArtKeys = new[] { "异想体战斗", "随机事件背景1" },
                    BodyZh =
                        "在开局菜单选择“挑战解放战”，选择目标楼层并使用永久图鉴配置临时队伍。\n\n" +
                        "解放战会直接进入所选楼层的最终多阶段战斗，不需要重复前置异想体镇压。不同楼层可能拥有不同的书页章节限制。\n\n" +
                        "首次通关会永久解锁该层专属异想体书页与 E.G.O.。已经通关的楼层可以再战，但不会重复发放首通奖励。\n\n" +
                        "解放战临时编队不会覆盖正常路线配置。选择“正常游玩”后，本次路线期间会关闭解放入口；放弃路线并重新开始后可再次挑战。\n\n" +
                        "可以直接开始正常路线，也可以先解放部分楼层扩充永久资源。根据每次获得的内容调整策略，正是本模组的核心玩法。",
                    BodyEn =
                        "Enter realizations from the start hub, choose a floor, and prepare a temporary team from permanent Atlas unlocks. First clears unlock exclusive rewards; replays do not repeat them. Temporary loadouts do not overwrite normal-run configurations."
                }
            };
        }

        private static Page[] BuildPages()
        {
            // Audience: already finished / knows vanilla LoR.
            // Only write deltas vs 原版接待 / 编队 / 战斗 / 解放.
            return new Page[]
            {
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Overview",
                    NavZh = "和原版有何不同",
                    NavEn = "Vs Vanilla",
                    BodyKey = "ui_RMR_Help_Body_Overview",
                    BodyZh =
                        "默认你已熟悉原版接待、编队、情感与解放流程。下面只写本模组改动点。\n\n" +
                        "【整体】\n" +
                        "· 原版：邀请 → 固定接待战 → 下一场。\n" +
                        "· 本模：邀请入口进入 RMR 主菜单 →「正常游玩」后变成「按章节推进的肉鸽路线」（传闻→…→杂质）。\n" +
                        "· 战斗内操作（拼点、情感、光芒等）仍是原版规则，不另教。\n\n" +
                        "【开局多了几条路】\n" +
                        "· 正常游玩 — 开一局肉鸽。\n" +
                        "· 继续旅程 — 读本地存档（有档才显示）。\n" +
                        "· 挑战解放战 — 不跑主线，直接打某层原版最终解放；首通永久解锁。\n" +
                        "· 图鉴 / 重置永久进度 — 永久档案，见对应页。\n\n" +
                        "【永久 vs 本局】\n" +
                        "· 永久：图鉴解锁、解放首通记录等（重置会清）。\n" +
                        "· 本局：金币、本局库存、章节进度（死档/重开丢失）。",
                    BodyEn =
                        "Assumes you know vanilla LoR. Only RMR deltas below.\n\n" +
                        "Receptions become a chapter-based Roguelike route instead of a fixed invite chain.\n" +
                        "Combat rules stay vanilla.\n" +
                        "Start hub adds: Normal Play, Continue, Realization, Atlas, Reset permanent progress.\n" +
                        "Permanent: atlas / realization first-clears. Per-run: money, inventory, chapter progress."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Route",
                    NavZh = "路线节点",
                    NavEn = "Route Nodes",
                    BodyKey = "ui_RMR_Help_Body_Route",
                    BodyZh =
                        "【相对原版】不再是「下一场接待是谁」，而是章节地图上选节点。\n\n" +
                        "【节点类型】\n" +
                        "· 普通 / 精英 / Boss — 难度与收益递进；Boss 胜后升章、加人、刷新节点池。\n" +
                        "· 异想体 — 接近原版镇压，掉异想体书页并进图鉴。\n" +
                        "· 商店 / 休息 / 神秘事件 — 肉鸽向补给与抉择（原版接待没有这套地图环）。\n\n" +
                        "【终局】\n" +
                        "· 杂质章 Boss 打完即本局结束，不会无限续关。\n\n" +
                        "【编队】\n" +
                        "· 仍用原版编队 UI（核心页/战斗页等）。\n" +
                        "· 图鉴入口已移出编队，改在开局菜单。\n" +
                        "· 可接待楼层数随进度变，注意人数上限。",
                    BodyEn =
                        "Instead of a fixed next reception, you pick map nodes per urban chapter.\n" +
                        "Nodes: normal / elite / boss / abnormality / shop / rest / mystery.\n" +
                        "Impurity boss ends the run.\n" +
                        "Prepare UI is still vanilla; Atlas is on the start hub only."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Rewards",
                    NavZh = "奖励与库存",
                    NavEn = "Rewards & Inventory",
                    BodyKey = "ui_RMR_Help_Body_Rewards",
                    BodyZh =
                        "【相对原版】战后不再只是「接待结算」，而是排队领：被动 / 战斗书页 / EGO 等。\n\n" +
                        "【战斗书页库存】\n" +
                        "· 按「种类」解锁与升级，不是像背包堆叠那样按张分别升级。\n" +
                        "· 升级界面不显示 x99 数量意义（升级看种类）。\n" +
                        "· 升过的版本会替换原种类，后续构筑识别升级态。\n\n" +
                        "【E.G.O.】\n" +
                        "· 仍按原版：个人 EGO + 情感等级解锁，不要当九张套牌里的普通战斗卡。\n" +
                        "· 本模在开战时会把本局已拥有的 EGO 注入个人 EGO 栏；等级够了就能选。\n" +
                        "· 误塞进战斗卡组会被尽量剥离。\n\n" +
                        "【永久图鉴】\n" +
                        "· 主线拿到的异想体 / EGO / 部分书页会写入永久图鉴，供解放战等使用。",
                    BodyEn =
                        "Post-battle reward queues (passives / pages / EGO) replace simple reception settle.\n" +
                        "Combat pages unlock/upgrade by type, not stack count.\n" +
                        "EGO stays personalEgo + emotion level; owned EGO is granted at battle start.\n" +
                        "Finds also feed the permanent atlas."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Shop",
                    NavZh = "商店",
                    NavEn = "Shop",
                    BodyKey = "ui_RMR_Help_Body_Shop",
                    BodyZh =
                        "【相对原版】接待途中有「商店节点」，用本局金币买东西（不是图书馆招待所那套）。\n\n" +
                        "【分区大致包括】\n" +
                        "核心书页 / 战斗书页 / 被动 / 异想体 / E.G.O. / 书页升级。\n\n" +
                        "【升级】\n" +
                        "选已有战斗书页种类强化；与「再买一张同名」不是一回事。\n" +
                        "钱不够会变红或不可点。",
                    BodyEn =
                        "Shop nodes spend run money on key pages, combat pages, passives, abnos, EGO, upgrades.\n" +
                        "Upgrades target card types you already own."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Realization",
                    NavZh = "解放战（改动）",
                    NavEn = "Realization",
                    BodyKey = "ui_RMR_Help_Body_Realization",
                    BodyZh =
                        "【相对原版】\n" +
                        "· 原版：剧情推进到某层后打完整解放线（含前置镇压等）。\n" +
                        "· 本模：开局菜单「挑战解放战」→ 选层 → 编队 → 直接打该层原版最终解放（多阶段），跳过前置镇压链。\n\n" +
                        "【首通 / 再战】\n" +
                        "· 首通：永久解锁该层专属奖励进图鉴。\n" +
                        "· 再战：可重复打，不再发首通奖。\n\n" +
                        "【编队限制（与原版自由书库不同）】\n" +
                        "· 历史/科技/文学/艺术：书页章节上限到都市梦魇。\n" +
                        "· 总类：可到杂质。\n" +
                        "· 其余层：多为都市之星。\n" +
                        "· 解放战被动费用上限更高（按模组规格）。\n\n" +
                        "【地图】\n" +
                        "尽量跟所选司书层；杂质等特殊图不硬改原版 MapInfo。",
                    BodyEn =
                        "From start hub only: pick floor → prepare → vanilla final multi-phase realization (no prior suppress chain).\n" +
                        "First clear permanent unlocks; replays no first-clear loot.\n" +
                        "Chapter caps on loadouts by floor; higher passive cost budget in realization."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Atlas",
                    NavZh = "图鉴",
                    NavEn = "Atlas",
                    BodyKey = "ui_RMR_Help_Body_Atlas",
                    BodyZh =
                        "【相对原版】不是图书馆「书架故事」那套，而是 RMR 永久收藏。\n\n" +
                        "· 入口：开局菜单 → 图鉴（编队页没有）。\n" +
                        "· 四类：角色书页 / 战斗书页 / 异想体书页 / EGO 战斗书页。\n" +
                        "· 未解锁显示 ?；解放战可用内容大体依赖这里已解锁集合。\n" +
                        "· 「重置永久进度」会清图鉴与解放记录。",
                    BodyEn =
                        "Permanent RMR collection (not vanilla archives UI).\n" +
                        "Open from start hub only. Four categories; realization pools use unlocks.\n" +
                        "Reset permanent progress wipes atlas + realization records."
                },
                new Page
                {
                    NavKey = "ui_RMR_Help_Nav_Tips",
                    NavZh = "注意点",
                    NavEn = "Notes",
                    BodyKey = "ui_RMR_Help_Body_Tips",
                    BodyZh =
                        "【和原版习惯冲突时】\n" +
                        "· 想打解放：去开局菜单，不要在编队里找旧入口。\n" +
                        "· 想看图鉴：同上。\n" +
                        "· 杂质 Boss = 本局终点。\n" +
                        "· 改 DLL 后必须完全退出游戏再进。\n\n" +
                        "【建议顺序（可选）】\n" +
                        "先解放几层攒图鉴 → 再开正常游玩，池子更宽。\n\n" +
                        "其余战斗技巧与原版相同，此处不重复。",
                    BodyEn =
                        "Realization & Atlas only on start hub.\n" +
                        "Impurity boss ends the run.\n" +
                        "Fully restart the game after replacing the DLL.\n" +
                        "Optional: clear some realizations before a normal run."
                },
            };
        }

        void Awake()
        {
            Instance = this;
        }

        public static void ShowOrCreate(Transform preferredParent = null)
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("RMRHelpHandbookPanel");
                Instance = go.AddComponent<RMRHelpHandbookPanel>();
            }
            Instance.Show(preferredParent);
        }

        public void Show(Transform preferredParent = null)
        {
            if (_root != null)
                Destroy(_root);
            _navLabels.Clear();
            _navFrames.Clear();
            LogLikeMod.InvalidateTmpFontCache();
            if (LogLikeMod.DefFont_TMP == null)
                Debug.LogWarning("[RMRHelpHandbookPanel] No CJK TMP font.");
            Build(preferredParent);
            Select(0);
        }

        public void Hide()
        {
            if (_root != null)
                Destroy(_root);
            _root = null;
            _navLabels.Clear();
            _navFrames.Clear();
            try { RMRRealizationLaunchHost.DestroyOverlayIfEmpty(); } catch { }
        }

        private static string T(string key, string zh, string en = null)
        {
            try
            {
                string text = TextDataModel.GetText(key);
                if (!string.IsNullOrEmpty(text) && text != key)
                    return text;
            }
            catch { }
            string lang = "";
            try { lang = TextDataModel.CurrentLanguage.ToString().ToLowerInvariant(); } catch { }
            if (lang.Contains("en") && !string.IsNullOrEmpty(en))
                return en;
            return zh;
        }

        private Transform ResolveParent(Transform preferred)
        {
            if (preferred != null)
                return preferred;
            try { return RMRRealizationLaunchHost.GetOrCreateOverlayRoot(); }
            catch { }
            return transform;
        }

        private void Build(Transform preferredParent)
        {
            Transform parent = ResolveParent(preferredParent);
            _root = new GameObject("HelpHandbookRoot", typeof(RectTransform));
            _root.transform.SetParent(parent, false);
            _root.transform.SetAsLastSibling();

            RectTransform rootRt = _root.GetComponent<RectTransform>();
            rootRt.anchorMin = Vector2.zero;
            rootRt.anchorMax = Vector2.one;
            rootRt.offsetMin = Vector2.zero;
            rootRt.offsetMax = Vector2.zero;

            var dim = _root.AddComponent<Image>();
            dim.color = new Color(0.02f, 0.015f, 0.01f, 1f);
            dim.raycastTarget = true;

            var frame = MakeSolid(_root.transform, "Frame", Vector2.zero, new Vector2(1180f, 720f), ColGoldDim);
            var card = MakeSolid(frame.transform, "Card", Vector2.zero, new Vector2(1168f, 708f), ColPanel);
            StretchInset(card.GetComponent<RectTransform>(), 4f);

            MakeTmp(card.transform, "Title", new Vector2(0f, 310f), new Vector2(1000f, 40f), 30,
                TextAlignmentOptions.Center, T("ui_RMR_Hub_Help", "玩法介绍", "How to Play")).color = ColGold;
            MakeTmp(card.transform, "Sub", new Vector2(0f, 280f), new Vector2(1000f, 22f), 13,
                TextAlignmentOptions.Center, "\u5728\u4e0d\u65ad\u53d8\u5316\u7684\u63a5\u5f85\u4e2d\uff0c\u6784\u7b51\u5c5e\u4e8e\u4f60\u7684\u56fe\u4e66\u9986  \u00b7  BUILD YOUR OWN LIBRARY").color = ColGoldDim;

            // H-A left illustrated chapter index.
            float navTop = 210f;
            float navStep = 56f;
            for (int i = 0; i < Pages.Length; i++)
            {
                int idx = i;
                float y = navTop - i * navStep;
                var btnGo = new GameObject("Nav" + i, typeof(RectTransform));
                btnGo.transform.SetParent(card.transform, false);
                var img = btnGo.AddComponent<Image>();
                img.color = ColNavIdle;
                var brt = btnGo.GetComponent<RectTransform>();
                brt.anchorMin = brt.anchorMax = new Vector2(0.5f, 0.5f);
                brt.sizeDelta = new Vector2(230f, 48f);
                brt.anchoredPosition = new Vector2(-430f, y);
                _navFrames.Add(img);
                MakeSolid(btnGo.transform, "NavAccent", new Vector2(-112f, 0f), new Vector2(3f, 40f), ColGoldDim);
                var navIcon = MakeSolid(btnGo.transform, "NavIcon", new Vector2(-88f, 0f), new Vector2(38f, 38f),
                    new Color(0.10f, 0.08f, 0.06f, 1f)).GetComponent<Image>();
                Sprite navArt = ResolveArt(Pages[i].ArtKeys);
                if (navArt != null)
                {
                    navIcon.sprite = navArt;
                    navIcon.preserveAspect = true;
                    navIcon.color = Color.white;
                }
                else
                    navIcon.enabled = false;
                var btn = btnGo.AddComponent<Button>();
                btn.targetGraphic = img;
                var label = MakeTmp(btnGo.transform, "L", new Vector2(18f, 0f), new Vector2(172f, 42f), 16,
                    TextAlignmentOptions.Center, T(Pages[i].NavKey, Pages[i].NavZh, Pages[i].NavEn));
                label.color = ColCream;
                _navLabels.Add(label);
                btn.onClick.AddListener(() =>
                {
                    try { UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click); } catch { }
                    Select(idx);
                });
            }

            // H-A right illustrated header plus one continuous scroll area.
            var bodyBg = MakeSolid(card.transform, "BodyBg", new Vector2(130f, 10f), new Vector2(800f, 520f), ColBodyBg);
            try { bodyBg.GetComponent<Image>().raycastTarget = true; } catch { }

            _bannerImage = MakeSolid(bodyBg.transform, "Banner", new Vector2(0f, 180f), new Vector2(740f, 96f),
                new Color(0.08f, 0.07f, 0.06f, 1f)).GetComponent<Image>();
            _bannerImage.preserveAspect = false;
            _bannerImage.raycastTarget = false;

            _iconImage = MakeSolid(bodyBg.transform, "Icon", new Vector2(-312f, 180f), new Vector2(72f, 72f),
                new Color(0.12f, 0.10f, 0.08f, 1f)).GetComponent<Image>();
            _iconImage.preserveAspect = true;
            _iconImage.raycastTarget = false;

            _sectionTitle = MakeTmp(bodyBg.transform, "SecTitle", new Vector2(42f, 180f), new Vector2(520f, 36f), 21,
                TextAlignmentOptions.Left, "");
            _sectionTitle.color = ColGold;

            MakeSolid(bodyBg.transform, "Rule", new Vector2(0f, 120f), new Vector2(740f, 1.5f),
                new Color(ColGoldDim.r, ColGoldDim.g, ColGoldDim.b, 0.7f));

            // Scrollbar is the only body navigation; no redundant 1/2/3 paging.
            var viewportGo = new GameObject("Viewport", typeof(RectTransform));
            viewportGo.transform.SetParent(bodyBg.transform, false);
            var viewportRt = viewportGo.GetComponent<RectTransform>();
            viewportRt.anchorMin = viewportRt.anchorMax = new Vector2(0.5f, 0.5f);
            viewportRt.sizeDelta = new Vector2(760f, 300f);
            viewportRt.anchoredPosition = new Vector2(-6f, -48f);
            var viewportImg = viewportGo.AddComponent<Image>();
            viewportImg.color = new Color(0.05f, 0.04f, 0.03f, 0.55f);
            viewportImg.raycastTarget = true;
            var mask = viewportGo.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            var contentGo = new GameObject("Content", typeof(RectTransform));
            contentGo.transform.SetParent(viewportGo.transform, false);
            _bodyContentRt = contentGo.GetComponent<RectTransform>();
            _bodyContentRt.anchorMin = new Vector2(0f, 1f);
            _bodyContentRt.anchorMax = new Vector2(1f, 1f);
            _bodyContentRt.pivot = new Vector2(0.5f, 1f);
            _bodyContentRt.anchoredPosition = Vector2.zero;
            _bodyContentRt.sizeDelta = new Vector2(0f, 320f);

            _bodyText = MakeTmp(contentGo.transform, "Body", Vector2.zero, new Vector2(720f, 300f), 17,
                TextAlignmentOptions.TopLeft, "");
            _bodyText.color = ColCream;
            _bodyText.enableWordWrapping = true;
            _bodyText.overflowMode = TextOverflowModes.Overflow;
            _bodyText.lineSpacing = 10f;
            var bodyRt = _bodyText.rectTransform;
            bodyRt.anchorMin = new Vector2(0f, 1f);
            bodyRt.anchorMax = new Vector2(1f, 1f);
            bodyRt.pivot = new Vector2(0.5f, 1f);
            bodyRt.anchoredPosition = new Vector2(0f, -10f);
            bodyRt.offsetMin = new Vector2(18f, bodyRt.offsetMin.y);
            bodyRt.offsetMax = new Vector2(-18f, -10f);
            bodyRt.sizeDelta = new Vector2(-36f, 300f);

            _bodyScroll = bodyBg.AddComponent<ScrollRect>();
            _bodyScroll.viewport = viewportRt;
            _bodyScroll.content = _bodyContentRt;
            _bodyScroll.horizontal = false;
            _bodyScroll.vertical = true;
            _bodyScroll.movementType = ScrollRect.MovementType.Clamped;
            _bodyScroll.scrollSensitivity = 30f;
            _bodyScroll.inertia = true;

            var sbGo = new GameObject("Scrollbar", typeof(RectTransform));
            sbGo.transform.SetParent(bodyBg.transform, false);
            var sbRt = sbGo.GetComponent<RectTransform>();
            sbRt.anchorMin = sbRt.anchorMax = new Vector2(0.5f, 0.5f);
            sbRt.sizeDelta = new Vector2(8f, 300f);
            sbRt.anchoredPosition = new Vector2(382f, -48f);
            var sbBg = sbGo.AddComponent<Image>();
            sbBg.color = new Color(0.18f, 0.14f, 0.10f, 0.9f);
            var scrollbar = sbGo.AddComponent<Scrollbar>();
            scrollbar.direction = Scrollbar.Direction.BottomToTop;

            var handleArea = new GameObject("SlidingArea", typeof(RectTransform));
            handleArea.transform.SetParent(sbGo.transform, false);
            var haRt = handleArea.GetComponent<RectTransform>();
            haRt.anchorMin = Vector2.zero;
            haRt.anchorMax = Vector2.one;
            haRt.offsetMin = haRt.offsetMax = Vector2.zero;

            var handle = new GameObject("Handle", typeof(RectTransform));
            handle.transform.SetParent(handleArea.transform, false);
            var hRt = handle.GetComponent<RectTransform>();
            hRt.anchorMin = Vector2.zero;
            hRt.anchorMax = Vector2.one;
            hRt.offsetMin = hRt.offsetMax = Vector2.zero;
            var hImg = handle.AddComponent<Image>();
            hImg.color = ColGoldDim;
            scrollbar.handleRect = hRt;
            scrollbar.targetGraphic = hImg;
            _bodyScroll.verticalScrollbar = scrollbar;
            try { _bodyScroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent; }
            catch { }

            var closeGo = new GameObject("Close", typeof(RectTransform));
            closeGo.transform.SetParent(card.transform, false);
            var cimg = closeGo.AddComponent<Image>();
            cimg.color = new Color(0.28f, 0.22f, 0.16f, 1f);
            var crt = closeGo.GetComponent<RectTransform>();
            crt.anchorMin = crt.anchorMax = new Vector2(0.5f, 0.5f);
            crt.sizeDelta = new Vector2(200f, 48f);
            crt.anchoredPosition = new Vector2(0f, -320f);
            var cbtn = closeGo.AddComponent<Button>();
            cbtn.targetGraphic = cimg;
            var cl = MakeTmp(closeGo.transform, "CT", Vector2.zero, new Vector2(180f, 40f), 20,
                TextAlignmentOptions.Center, T("ui_RMR_Help_Close", "关闭", "Close"));
            cl.color = ColCream;
            StretchFull(cl.rectTransform, 0f);
            cbtn.onClick.AddListener(() =>
            {
                try { UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click); } catch { }
                Hide();
            });
        }

        private void ApplyBodyText(string text)
        {
            if (_bodyText != null)
            {
                _bodyText.text = text ?? "";
                try
                {
                    _bodyText.ForceMeshUpdate();
                    float h = Math.Max(300f, _bodyText.preferredHeight + 32f);
                    if (_bodyContentRt != null)
                        _bodyContentRt.sizeDelta = new Vector2(0f, h);
                    var brt = _bodyText.rectTransform;
                    brt.sizeDelta = new Vector2(brt.sizeDelta.x, h - 8f);
                }
                catch { }
            }
            if (_bodyScroll != null)
                _bodyScroll.verticalNormalizedPosition = 1f;
        }

        private static GameObject MakeSolid(Transform parent, string name, Vector2 pos, Vector2 size, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = color;
            img.raycastTarget = false;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
            rt.anchoredPosition = pos;
            return go;
        }

        private static void StretchInset(RectTransform rt, float inset)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(inset, inset);
            rt.offsetMax = new Vector2(-inset, -inset);
            rt.sizeDelta = Vector2.zero;
        }

        private static void StretchFull(RectTransform rt, float pad)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(pad, pad);
            rt.offsetMax = new Vector2(-pad, -pad);
        }

        private static TextMeshProUGUI MakeTmp(Transform parent, string name, Vector2 pos, Vector2 size, float fontSize,
            TextAlignmentOptions align, string text)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
            rt.anchoredPosition = pos;
            // Sharp Noto SDF material; never synthetic Bold on CJK.
            LogLikeMod.ApplyTmpFontPreservingSharpMaterial(tmp, LogLikeMod.DefFont_TMP);
            tmp.fontSize = fontSize;
            tmp.color = ColCream;
            tmp.alignment = align;
            tmp.enableWordWrapping = true;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.fontStyle = FontStyles.Normal;
            tmp.richText = false;
            try { tmp.enableAutoSizing = false; } catch { /* older TMP */ }
            tmp.raycastTarget = false;
            tmp.text = text ?? "";
            return tmp;
        }

        private static Sprite ResolveArt(string[] keys, int startIndex = 0)
        {
            if (keys == null || LogLikeMod.ArtWorks == null)
                return null;
            for (int i = Math.Max(0, startIndex); i < keys.Length; i++)
            {
                string key = keys[i];
                if (string.IsNullOrEmpty(key))
                    continue;
                try
                {
                    if (LogLikeMod.ArtWorks.ContainsKey(key))
                    {
                        Sprite sprite = LogLikeMod.ArtWorks[key];
                        if (sprite != null)
                            return sprite;
                    }
                }
                catch { }
            }
            try
            {
                for (int i = Math.Max(0, startIndex); i < keys.Length; i++)
                {
                    string key = keys[i];
                    if (string.IsNullOrEmpty(key))
                        continue;
                    foreach (var pair in LogLikeMod.ArtWorks.dic)
                    {
                        if (pair.Key != null && pair.Value != null
                            && pair.Key.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                            return pair.Value;
                    }
                }
            }
            catch { }
            return null;
        }

        private void Select(int index)
        {
            _index = Math.Max(0, Math.Min(index, Pages.Length - 1));
            Page page = Pages[_index];

            if (_sectionTitle != null)
                _sectionTitle.text = T(page.NavKey, page.NavZh, page.NavEn);

            ApplyBodyText(T(page.BodyKey, page.BodyZh, page.BodyEn));

            Sprite art = ResolveArt(page.ArtKeys);
            if (_bannerImage != null)
            {
                _bannerImage.sprite = art;
                _bannerImage.enabled = true;
                _bannerImage.color = art != null
                    ? new Color(0.75f, 0.70f, 0.60f, 1f)
                    : new Color(0.08f, 0.07f, 0.06f, 1f);
            }
            if (_iconImage != null)
            {
                Sprite icon = ResolveArt(page.ArtKeys, 1) ?? art;
                _iconImage.sprite = icon;
                _iconImage.enabled = icon != null;
                _iconImage.color = Color.white;
            }

            for (int i = 0; i < _navLabels.Count; i++)
            {
                bool on = i == _index;
                if (_navLabels[i] != null)
                    _navLabels[i].color = on ? ColGold : ColCream;
                if (i < _navFrames.Count && _navFrames[i] != null)
                    _navFrames[i].color = on ? ColNavOn : ColNavIdle;
            }
        }
    }
}
