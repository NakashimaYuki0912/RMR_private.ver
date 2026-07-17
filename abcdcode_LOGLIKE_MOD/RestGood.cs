// -----------------------------------------------------------------------------
// Library of Ruina mod script: RestGood
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RestGood.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>RestGood</summary>

    public class RestGood : MonoBehaviour
    {
        public Sprite GoodSprite;
        public RestPickUp GoodScript;
        public RewardPassiveInfo goodinfo;
        public MysteryModel_Rest parent;
        public bool update;

        public virtual void SetParent(MysteryModel_Rest p)
        {
            this.parent = p;
            if (p.Inited)
                return;
            this.GoodScript.Init();
        }

        public void Set(RewardPassiveInfo goodinfo)
        {
            this.goodinfo = goodinfo;
            this.GoodScript = LogLikeMod.FindPickUp(goodinfo.script) as RestPickUp;
            this.GoodSprite = this.goodinfo.id.packageId == LogLikeMod.ModId ? LogLikeMod.ArtWorks[goodinfo.iconartwork] : LogLikeMod.ModdedArtWorks[(this.goodinfo.id.packageId, this.goodinfo.iconartwork)];
            UILogCustomSelectable customSelectable = ModdingUtils.CreateLogSelectable(this.transform, this.GoodSprite, new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(150f, 150f));
            Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
            buttonClickedEvent.AddListener(() => this.OnClickGoods());
            customSelectable.onClick = buttonClickedEvent;
            customSelectable.SelectEvent = new UnityEventBasedata();
            customSelectable.SelectEvent.AddListener(e => this.OnPointerEnter());
            customSelectable.DeselectEvent = new UnityEventBasedata();
            customSelectable.DeselectEvent.AddListener(e => this.OnPointerExit());
        }

        public void OnClickGoods()
        {
            if (!this.GoodScript.CheckCondition())
                return;
            this.GoodScript.OnChoice(this);
            SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay();
            this.parent.ChoiceRest(this);
        }

        public bool OnChoiceOther(RestGood other) => this.GoodScript.OnChoiceOther(other);

        public void OnPointerEnter()
        {
            SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(this.GoodScript.Name, this.GoodScript.Desc, this.GoodSprite, this.gameObject);
            this.update = true;
        }

        public void OnPointerExit()
        {
            SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay();
            this.update = false;
        }

        public void Update()
        {
            if (!this.update)
                return;
            SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(this.GoodScript.Name, this.GoodScript.Desc, this.GoodSprite, this.gameObject);
        }
    }
}
