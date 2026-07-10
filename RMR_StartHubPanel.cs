using System;
using System.Collections;
using System.Collections.Generic;
using abcdcode_LOGLIKE_MOD;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// What the player chose on the invitation-time start hub.
    /// Consumed once the reception actually starts (OnWaveStartInitialEvent).
    /// </summary>
    public enum RMRLaunchIntent
    {
        None = 0,
        NormalPlay = 1,
        Realization = 2,
    }

    /// <summary>
    /// RMR mode select: shown right after clicking the mod entry on the invitation panel,
    /// BEFORE ConfirmSendInvitation / in-run battle prepare.
    /// </summary>
    public class RMRStartHubPanel : MonoBehaviour
    {
        public static RMRStartHubPanel Instance { get; private set; }

        /// <summary>Intent chosen on the invitation hub; read by OnWaveStartInitialEvent.</summary>
        public static RMRLaunchIntent LaunchIntent { get; private set; }

        private GameObject _root;
        private GameObject _confirmRoot;
        private bool _visible;
        private UIInvitationRightMainPanel _invitationPanel;
        public bool IsVisible => _visible;

        void Awake()
        {
            Instance = this;
        }

        public static void ClearLaunchIntent()
        {
            LaunchIntent = RMRLaunchIntent.None;
            RMRRealizationManager.PendingLaunchIntent = RMRLaunchIntent.None;
            // Do not clear PendingRealizationFloor here — HandlePostInvitationLaunch still needs it
            // after ClearLaunchIntent when the invitation is already in flight.
        }

        public static void SetLaunchIntent(RMRLaunchIntent intent)
        {
            LaunchIntent = intent;
            RMRRealizationManager.PendingLaunchIntent = intent;
            Debug.Log($"[RMRStartHubPanel] LaunchIntent set to {intent}");
        }

        /// <summary>
        /// Open hub on the invitation UI (before sending the invitation).
        /// </summary>
        public static void ShowOnInvitation(UIInvitationRightMainPanel invitation)
        {
            if (invitation == null)
            {
                Debug.LogError("[RMRStartHubPanel] ShowOnInvitation: invitation panel is null.");
                return;
            }
            if (Instance == null)
            {
                GameObject go = new GameObject("RMRStartHubPanel");
                Instance = go.AddComponent<RMRStartHubPanel>();
            }
            Instance._invitationPanel = invitation;
            Instance.ShowInternal(invitation.transform);
        }

        /// <summary>Legacy in-run show — blocked during realization; prefer ShowOnInvitation.</summary>
        public static void ShowOrCreate()
        {
            if (RMRRealizationManager.InRealizationBattle
                || RMRRealizationManager.PendingRealizationBattle
                || RMRRealizationManager.RealizationCombatLive)
            {
                Debug.LogWarning("[RMRStartHubPanel] ShowOrCreate blocked — realization in progress.");
                return;
            }
            // After realization we return to the library main page; do not re-open mode select in-run.
            Debug.LogWarning("[RMRStartHubPanel] ShowOrCreate ignored — hub is invitation-time only.");
        }

        public void Show()
        {
            // Keep for API compatibility; invitation path uses ShowInternal.
            if (_invitationPanel != null)
                ShowInternal(_invitationPanel.transform);
        }

        private void ShowInternal(Transform parent)
        {
            if (_visible)
                Hide();
            RMRRealizationManager.BeginHubSession();
            EnsureChineseCapableFont();
            BuildUi(parent);
            _visible = true;
            StartCoroutine(RebuildIfFontImproves(parent));
        }

        private static void EnsureChineseCapableFont()
        {
            try
            {
                LogLikeMod.InvalidateTmpFontCache();
                TMP_FontAsset font = LogLikeMod.DefFont_TMP;
                if (font == null)
                    Debug.LogWarning("[RMRStartHubPanel] No CJK-capable TMP font resolved.");
                else
                    Debug.Log($"[RMRStartHubPanel] Using TMP font '{font.name}'.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMRStartHubPanel] EnsureChineseCapableFont: " + ex.Message);
            }
        }

        private IEnumerator RebuildIfFontImproves(Transform parent)
        {
            TMP_FontAsset before = LogLikeMod.DefFont_TMP;
            yield return null;
            yield return null;
            LogLikeMod.InvalidateTmpFontCache();
            TMP_FontAsset after = LogLikeMod.DefFont_TMP;
            if (after != null && !object.ReferenceEquals(after, before) && _visible)
            {
                if (_root != null)
                    Destroy(_root);
                _root = null;
                _confirmRoot = null;
                BuildUi(parent);
            }
        }

        public void Hide()
        {
            if (!_visible && _root == null)
                return;
            _visible = false;
            if (_root != null)
                Destroy(_root);
            _root = null;
            _confirmRoot = null;
        }

        public bool TryHandleBack()
        {
            if (!_visible)
                return false;
            if (_confirmRoot != null)
            {
                Destroy(_confirmRoot);
                _confirmRoot = null;
                return true;
            }
            // Invitation hub: back = close hub only (stay on invitation).
            Hide();
            return true;
        }

        private static string T(string key, string zh, string en = null, string kr = null)
        {
            try
            {
                string text = TextDataModel.GetText(key);
                if (!string.IsNullOrEmpty(text) && text != key)
                    return text;
            }
            catch { }
            string lang = TextDataModel.CurrentLanguage.ToString().ToLowerInvariant();
            if (lang.Contains("en") && !string.IsNullOrEmpty(en))
                return en;
            if ((lang.Contains("kr") || lang.Contains("ko")) && !string.IsNullOrEmpty(kr))
                return kr;
            return zh;
        }

        private void BuildUi(Transform parent)
        {
            if (parent == null)
                parent = transform;

            _root = new GameObject("StartHubRoot", typeof(RectTransform));
            _root.transform.SetParent(parent, false);
            _root.transform.SetAsLastSibling();

            RectTransform rootRt = _root.GetComponent<RectTransform>();
            rootRt.anchorMin = Vector2.zero;
            rootRt.anchorMax = Vector2.one;
            rootRt.offsetMin = Vector2.zero;
            rootRt.offsetMax = Vector2.zero;

            // Fullscreen dim
            var dimGo = new GameObject("Dim", typeof(RectTransform));
            dimGo.transform.SetParent(_root.transform, false);
            var dimImg = dimGo.AddComponent<Image>();
            dimImg.color = new Color(0.05f, 0.04f, 0.03f, 0.88f);
            var dimRt = dimGo.GetComponent<RectTransform>();
            dimRt.anchorMin = Vector2.zero;
            dimRt.anchorMax = Vector2.one;
            dimRt.offsetMin = Vector2.zero;
            dimRt.offsetMax = Vector2.zero;
            dimImg.raycastTarget = true;

            // Center card
            var card = new GameObject("Card", typeof(RectTransform));
            card.transform.SetParent(_root.transform, false);
            var cardImg = card.AddComponent<Image>();
            cardImg.color = new Color(0.18f, 0.15f, 0.12f, 0.96f);
            var cardRt = card.GetComponent<RectTransform>();
            cardRt.anchorMin = cardRt.anchorMax = new Vector2(0.5f, 0.5f);
            cardRt.sizeDelta = new Vector2(560f, 520f);
            cardRt.anchoredPosition = Vector2.zero;

            AddLabel(card.transform, T("ui_RMR_HubTitle", "Roguelike \u5f00\u5c40", "Roguelike Start", "Roguelike \uc2dc\uc791"),
                new Vector2(0f, 200f), 34, new Vector2(500f, 50f));
            AddLabel(card.transform, T("ui_RMR_HubDesc",
                    "\u53ef\u5148\u6311\u6218\u89e3\u653e\u6218\u4ee5\u6c38\u4e45\u89e3\u9501\u5956\u52b1\uff0c\u6216\u76f4\u63a5\u5f00\u59cb\u6b63\u5e38\u6e38\u73a9\u3002",
                    "Challenge realization for permanent unlocks, or start a normal run.",
                    "\ud574\ubc29\uc804\uc73c\ub85c \uc601\uad6c \ud574\uae08\ud558\uac70\ub098 \uc77c\ubc18 \ub7f0\uc744 \uc2dc\uc791\ud558\uc138\uc694."),
                new Vector2(0f, 140f), 18, new Vector2(500f, 70f));

            float y = 60f;
            AddMenuButton(card.transform, T("ui_RMR_Hub_Play", "\u6b63\u5e38\u6e38\u73a9", "Normal Play", "\uc77c\ubc18 \ud50c\ub808\uc774"), y, OnClickPlay);
            y -= 70f;
            AddMenuButton(card.transform, T("ui_RMR_Hub_Realization", "\u6311\u6218\u89e3\u653e\u6218", "Realization", "\ud574\ubc29\uc804 \ub3c4\uc804"), y, OnClickRealization);
            y -= 70f;
            AddMenuButton(card.transform, T("ui_RMR_Hub_Help", "\u73a9\u6cd5\u4ecb\u7ecd", "How to Play", "\ud50c\ub808\uc774 \uc18c\uac1c"), y, OnClickHelp);
            y -= 70f;
            AddMenuButton(card.transform, T("ui_RMR_Hub_Exit", "\u9000\u51fa", "Exit", "\uc885\ub8cc"), y, OnClickExit);
        }

        private static TextMeshProUGUI AddLabel(Transform parent, string text, Vector2 pos, int size, Vector2 box)
        {
            var go = new GameObject("Label", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = box;
            rt.anchoredPosition = pos;
            tmp.font = LogLikeMod.DefFont_TMP;
            tmp.fontSize = size;
            tmp.color = LogLikeMod.DefFontColor;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = true;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.text = text;
            return tmp;
        }

        private void AddMenuButton(Transform parent, string label, float y, UnityAction action)
        {
            var go = new GameObject("HubBtn", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = new Color(0.32f, 0.26f, 0.18f, 0.98f);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(420f, 56f);
            rt.anchoredPosition = new Vector2(0f, y);
            var btn = go.AddComponent<Button>();
            btn.targetGraphic = img;

            var textGo = new GameObject("Text", typeof(RectTransform));
            textGo.transform.SetParent(go.transform, false);
            var tmp = textGo.AddComponent<TextMeshProUGUI>();
            var trt = textGo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = new Vector2(12f, 4f);
            trt.offsetMax = new Vector2(-12f, -4f);
            tmp.font = LogLikeMod.DefFont_TMP;
            tmp.fontSize = 24;
            tmp.color = LogLikeMod.DefFontColor;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = false;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.text = label;

            btn.onClick.AddListener(() =>
            {
                try { UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click); } catch { }
                try { action(); }
                catch (Exception ex) { Debug.LogError("[RMRStartHubPanel] " + ex); }
            });
        }

        private bool EnsureInvitationReady()
        {
            if (_invitationPanel != null)
                return true;
            Debug.LogError("[RMRStartHubPanel] Invitation panel missing — open hub from mod entry only.");
            return false;
        }

        private void SendInvitationAndCloseHub()
        {
            if (!EnsureInvitationReady())
                return;
            List<string> except;
            if (LogLikeMod.CheckExceptionModExist(out except))
            {
                string text = TextDataModel.GetText("ui_ExceptionWithLog") + Environment.NewLine;
                foreach (string str in except)
                    text = text + "-" + str + Environment.NewLine;
                UIAlarmPopup.instance.SetAlarmText(text);
                return;
            }

            bool first = true;
            _invitationPanel.SetCustomInvToggle(true);
            foreach (UIInvitationBookSlot slot in _invitationPanel.invitationbookSlots)
            {
                if (first)
                    slot.ApplySlotid(new LorId(LogLikeMod.ModId, -853), true);
                else
                    slot.SetEmptySlot();
                first = false;
            }
            // New reception: allow OnWaveStartInitialEvent to run once for this invite.
            RMRCore.ResetPostInvitationLaunchGate();
            Hide();
            _invitationPanel.ConfirmSendInvitation();
        }

        private void OnClickPlay()
        {
            // Start a normal Roguelike run after invitation is sent.
            RMRRealizationManager.ClearPendingRealizationFloor();
            SetLaunchIntent(RMRLaunchIntent.NormalPlay);
            RMRRealizationManager.StartNormalPlayFromHub();
            SendInvitationAndCloseHub();
        }

        private void OnClickRealization()
        {
            // Pick floor on invitation UI FIRST, then send invitation → direct boss prepare.
            // Never enter initial mystery / chapter intro story for this path.
            SetLaunchIntent(RMRLaunchIntent.Realization);
            RMRRealizationManager.BeginHubSession();
            RMRRealizationManager.ClearPendingRealizationFloor();
            try
            {
                LogLikeMod.InvalidateTmpFontCache();
                var _ = LogLikeMod.DefFont_TMP;
                LogRealizationPanel panel = LogRealizationPanel.Instance;
                if (panel == null)
                {
                    GameObject go = new GameObject("LogRealizationPanel");
                    panel = go.AddComponent<LogRealizationPanel>();
                }
                Transform parent = _root != null ? _root.transform : transform;
                panel.ShowForInvitationPick(parent, floor =>
                {
                    RMRRealizationManager.PendingRealizationFloor = floor;
                    RMRRealizationManager.BeginHubSession();
                    SetLaunchIntent(RMRLaunchIntent.Realization);
                    Debug.Log($"[RMRStartHubPanel] Realization floor pre-selected: {floor}");
                    SendInvitationAndCloseHub();
                });
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMRStartHubPanel] Failed to open invitation-time floor panel: " + ex);
                // Fallback: send invitation and open floor panel after reception (still no mystery).
                SendInvitationAndCloseHub();
            }
        }

        private void OnClickHelp()
        {
            Transform parent = _root != null ? _root.transform : transform;
            RMRHelpHandbookPanel.ShowOrCreate(parent);
        }

        private void OnClickExit()
        {
            ShowExitConfirm();
        }

        private void ShowExitConfirm()
        {
            if (_confirmRoot != null)
                Destroy(_confirmRoot);
            _confirmRoot = new GameObject("ExitConfirm", typeof(RectTransform));
            _confirmRoot.transform.SetParent(_root.transform, false);
            _confirmRoot.transform.SetAsLastSibling();
            var crt = _confirmRoot.GetComponent<RectTransform>();
            crt.anchorMin = Vector2.zero;
            crt.anchorMax = Vector2.one;
            crt.offsetMin = Vector2.zero;
            crt.offsetMax = Vector2.zero;

            var dim = _confirmRoot.AddComponent<Image>();
            dim.color = new Color(0f, 0f, 0f, 0.55f);

            var box = new GameObject("Box", typeof(RectTransform));
            box.transform.SetParent(_confirmRoot.transform, false);
            var boxImg = box.AddComponent<Image>();
            boxImg.color = new Color(0.2f, 0.17f, 0.13f, 0.98f);
            var brt = box.GetComponent<RectTransform>();
            brt.anchorMin = brt.anchorMax = new Vector2(0.5f, 0.5f);
            brt.sizeDelta = new Vector2(480f, 200f);

            AddLabel(box.transform, T("ui_RMR_Hub_ExitConfirm",
                    "\u662f\u5426\u5173\u95ed\u5f00\u5c40\u83dc\u5355\uff1f",
                    "Close the start menu?",
                    "\uc2dc\uc791 \uba54\ub274\ub97c \ub2eb\uc744\uae4c\uc694?"),
                new Vector2(0f, 40f), 20, new Vector2(440f, 70f));

            AddConfirmButton(box.transform, T("ui_RMR_Hub_ExitYes", "\u786e\u8ba4", "OK", "\ud655\uc778"), -100f, () =>
            {
                LaunchIntent = RMRLaunchIntent.None;
                RMRRealizationManager.EndHubSessionToLibrary();
                Hide();
            });
            AddConfirmButton(box.transform, T("ui_RMR_Hub_ExitNo", "\u53d6\u6d88", "Cancel", "\ucde8\uc18c"), 100f, () =>
            {
                if (_confirmRoot != null)
                    Destroy(_confirmRoot);
                _confirmRoot = null;
            });
        }

        private void AddConfirmButton(Transform parent, string label, float x, UnityAction action)
        {
            var go = new GameObject("Confirm", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = new Color(0.35f, 0.28f, 0.2f, 1f);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(160f, 44f);
            rt.anchoredPosition = new Vector2(x, -50f);
            var btn = go.AddComponent<Button>();
            btn.targetGraphic = img;
            var tgo = new GameObject("T", typeof(RectTransform));
            tgo.transform.SetParent(go.transform, false);
            var tmp = tgo.AddComponent<TextMeshProUGUI>();
            var trt = tgo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = trt.offsetMax = Vector2.zero;
            tmp.font = LogLikeMod.DefFont_TMP;
            tmp.fontSize = 20;
            tmp.color = LogLikeMod.DefFontColor;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.text = label;
            btn.onClick.AddListener(action);
        }
    }
}
