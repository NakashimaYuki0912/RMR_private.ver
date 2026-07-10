using System;
using System.Collections.Generic;
using abcdcode_LOGLIKE_MOD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// Left-nav + right-body handbook. Parent must be under a Canvas.
    /// </summary>
    public class RMRHelpHandbookPanel : MonoBehaviour
    {
        public static RMRHelpHandbookPanel Instance { get; private set; }

        private GameObject _root;
        private TextMeshProUGUI _bodyText;
        private readonly List<TextMeshProUGUI> _navLabels = new List<TextMeshProUGUI>();
        private int _index;
        public bool IsVisible => _root != null && _root.activeSelf;

        private static readonly string[] NavKeys =
        {
            "ui_RMR_Help_Nav_Overview",
            "ui_RMR_Help_Nav_Route",
            "ui_RMR_Help_Nav_Shop",
            "ui_RMR_Help_Nav_Realization",
            "ui_RMR_Help_Nav_AbnoEgo",
        };

        private static readonly string[] BodyKeys =
        {
            "ui_RMR_Help_Body_Overview",
            "ui_RMR_Help_Body_Route",
            "ui_RMR_Help_Body_Shop",
            "ui_RMR_Help_Body_Realization",
            "ui_RMR_Help_Body_AbnoEgo",
        };

        private static readonly string[] NavFallbackZh =
        {
            "\u603b\u89c8",
            "\u8def\u7ebf\u4e0e\u7ae0\u8282",
            "\u5546\u5e97\u4e0e\u5347\u7ea7",
            "\u89e3\u653e\u6218\u4e0e\u56fe\u9274",
            "\u5f02\u60f3\u4f53 / E.G.O.",
        };

        private static readonly string[] BodyFallbackZh =
        {
            "\u672c\u6a21\u7ec4\u5c06\u63a5\u5f85\u6539\u4e3a\u6309\u7ae0\u8282\u63a8\u8fdb\u7684 Roguelike \u8def\u7ebf\u3002\u5f00\u5c40\u4e3b\u83dc\u5355\u53ef\u5148\u6311\u6218\u697c\u5c42\u89e3\u653e\u4ee5\u6c38\u4e45\u89e3\u9501\u5956\u52b1\uff0c\u518d\u8fdb\u5165\u6b63\u5e38\u6e38\u73a9\u3002",
            "\u6bcf\u7ae0\u5728\u5730\u56fe\u8282\u70b9\u4e2d\u7ecf\u5386\u666e\u901a\u6218\u3001\u7cbe\u82f1\u3001Boss\u3001\u5546\u5e97\u3001\u4f11\u606f\u4e0e\u795e\u79d8\u4e8b\u4ef6\u3002\u51fb\u8d25 Boss \u540e\u5347\u7ae0\u3001\u52a0\u4eba\u5e76\u5237\u65b0\u8282\u70b9\u6c60\u3002",
            "\u5546\u5e97\u5206\u533a\u51fa\u552e\u6838\u5fc3\u4e66\u9875\u3001\u6218\u6597\u4e66\u9875\u3001\u88ab\u52a8\u3001\u5f02\u60f3\u4f53\u4e0e E.G.O.\u3002\u6218\u6597\u4e66\u9875\u6309\u79cd\u7c7b\u89e3\u9501\uff1b\u53ef\u5728\u5546\u5e97\u5347\u7ea7\u4e66\u9875\u3002",
            "\u89e3\u653e\u6218\u4ec5\u4ece\u5f00\u5c40\u4e3b\u83dc\u5355\u8fdb\u5165\u3002\u9996\u6b21\u901a\u5173\u6c38\u4e45\u89e3\u9501\u8be5\u5c42\u4e13\u5c5e\u5f02\u60f3\u4f53\u4e0e E.G.O.\uff1b\u5df2\u901a\u5173\u53ef\u518d\u6218\u4f46\u4e0d\u518d\u53d1\u5956\u3002\u7f16\u961f\u4e0e\u6218\u4e2d\u9009\u9875\u4f9d\u8d56\u6c38\u4e45\u56fe\u9274\u3002",
            "\u6b63\u5e38\u6e38\u73a9\u83b7\u5f97\u7684\u5f02\u60f3\u4f53\u4e66\u9875\u4e5f\u4f1a\u5199\u5165\u6c38\u4e45\u56fe\u9274\uff0c\u4f9b\u89e3\u653e\u6218\u9009\u9875\u4f7f\u7528\u3002\u56fe\u9274\u4e2d\u5f02\u60f3\u4f53\u4e0e E.G.O. \u4e0d\u6309\u90fd\u5e02\u7ae0\u8282\u5206\u6bb5\uff0c\u76f4\u63a5\u5c55\u793a\u3002",
        };

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
        }

        private static string T(string key, string fallback)
        {
            try
            {
                string text = TextDataModel.GetText(key);
                if (!string.IsNullOrEmpty(text) && text != key)
                    return text;
            }
            catch { }
            return fallback;
        }

        private Transform ResolveParent(Transform preferred)
        {
            if (preferred != null)
                return preferred;
            try
            {
                if (LogLikeMod.LogUIObjs != null && LogLikeMod.LogUIObjs.ContainsKey(90) && LogLikeMod.LogUIObjs[90] != null)
                    return LogLikeMod.LogUIObjs[90].transform;
            }
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
            dim.color = new Color(0.04f, 0.03f, 0.02f, 0.9f);
            dim.raycastTarget = true;

            // Panel card
            var card = new GameObject("Card", typeof(RectTransform));
            card.transform.SetParent(_root.transform, false);
            var cardImg = card.AddComponent<Image>();
            cardImg.color = new Color(0.16f, 0.14f, 0.11f, 0.98f);
            var cardRt = card.GetComponent<RectTransform>();
            cardRt.anchorMin = cardRt.anchorMax = new Vector2(0.5f, 0.5f);
            cardRt.sizeDelta = new Vector2(980f, 560f);
            cardRt.anchoredPosition = Vector2.zero;

            // Title
            MakeTmp(card.transform, "Title", new Vector2(0f, 240f), new Vector2(900f, 40f), 28,
                TextAlignmentOptions.Center, T("ui_RMR_Hub_Help", "\u73a9\u6cd5\u4ecb\u7ecd"));

            // Nav
            for (int i = 0; i < NavKeys.Length; i++)
            {
                int idx = i;
                float y = 160f - i * 56f;
                var btnGo = new GameObject("Nav" + i, typeof(RectTransform));
                btnGo.transform.SetParent(card.transform, false);
                var img = btnGo.AddComponent<Image>();
                img.color = new Color(0.28f, 0.24f, 0.18f, 1f);
                var brt = btnGo.GetComponent<RectTransform>();
                brt.anchorMin = brt.anchorMax = new Vector2(0.5f, 0.5f);
                brt.sizeDelta = new Vector2(240f, 48f);
                brt.anchoredPosition = new Vector2(-320f, y);
                var btn = btnGo.AddComponent<Button>();
                btn.targetGraphic = img;
                var label = MakeTmp(btnGo.transform, "L", Vector2.zero, new Vector2(220f, 40f), 18,
                    TextAlignmentOptions.Center, T(NavKeys[i], NavFallbackZh[i]));
                // stretch label
                var lrt = label.rectTransform;
                lrt.anchorMin = Vector2.zero;
                lrt.anchorMax = Vector2.one;
                lrt.offsetMin = new Vector2(8f, 4f);
                lrt.offsetMax = new Vector2(-8f, -4f);
                _navLabels.Add(label);
                btn.onClick.AddListener(() => Select(idx));
            }

            // Body background
            var bodyBg = new GameObject("BodyBg", typeof(RectTransform));
            bodyBg.transform.SetParent(card.transform, false);
            var bodyBgImg = bodyBg.AddComponent<Image>();
            bodyBgImg.color = new Color(0.1f, 0.09f, 0.08f, 1f);
            var bodyBgRt = bodyBg.GetComponent<RectTransform>();
            bodyBgRt.anchorMin = bodyBgRt.anchorMax = new Vector2(0.5f, 0.5f);
            bodyBgRt.sizeDelta = new Vector2(580f, 360f);
            bodyBgRt.anchoredPosition = new Vector2(140f, 10f);

            _bodyText = MakeTmp(bodyBg.transform, "Body", Vector2.zero, new Vector2(540f, 320f), 18,
                TextAlignmentOptions.TopLeft, "");
            var brt2 = _bodyText.rectTransform;
            brt2.anchorMin = Vector2.zero;
            brt2.anchorMax = Vector2.one;
            brt2.offsetMin = new Vector2(16f, 16f);
            brt2.offsetMax = new Vector2(-16f, -16f);
            _bodyText.enableWordWrapping = true;
            _bodyText.overflowMode = TextOverflowModes.Overflow;

            // Close
            var closeGo = new GameObject("Close", typeof(RectTransform));
            closeGo.transform.SetParent(card.transform, false);
            var cimg = closeGo.AddComponent<Image>();
            cimg.color = new Color(0.35f, 0.28f, 0.2f, 1f);
            var crt = closeGo.GetComponent<RectTransform>();
            crt.anchorMin = crt.anchorMax = new Vector2(0.5f, 0.5f);
            crt.sizeDelta = new Vector2(180f, 48f);
            crt.anchoredPosition = new Vector2(0f, -240f);
            var cbtn = closeGo.AddComponent<Button>();
            cbtn.targetGraphic = cimg;
            var cl = MakeTmp(closeGo.transform, "CT", Vector2.zero, new Vector2(160f, 40f), 20,
                TextAlignmentOptions.Center, T("ui_RMR_Help_Close", "\u5173\u95ed"));
            var clrt = cl.rectTransform;
            clrt.anchorMin = Vector2.zero;
            clrt.anchorMax = Vector2.one;
            clrt.offsetMin = clrt.offsetMax = Vector2.zero;
            cbtn.onClick.AddListener(Hide);
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
            tmp.font = LogLikeMod.DefFont_TMP;
            tmp.fontSize = fontSize;
            tmp.color = LogLikeMod.DefFontColor;
            tmp.alignment = align;
            tmp.enableWordWrapping = true;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.text = text ?? "";
            return tmp;
        }

        private void Select(int index)
        {
            _index = Math.Max(0, Math.Min(index, BodyKeys.Length - 1));
            if (_bodyText != null)
                _bodyText.text = T(BodyKeys[_index], BodyFallbackZh[_index]);
            for (int i = 0; i < _navLabels.Count; i++)
            {
                if (_navLabels[i] == null)
                    continue;
                _navLabels[i].color = i == _index
                    ? new Color(0.95f, 0.8f, 0.45f)
                    : LogLikeMod.DefFontColor;
            }
        }
    }
}
