// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.RestGood
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

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
