// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.UILogCustomSelectable
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class UILogCustomSelectable :
      UICustomSelectable,
      IPointerClickHandler,
      IEventSystemHandler,
      ISubmitHandler
    {
        public Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();

        public Button.ButtonClickedEvent onClick
        {
            get => this.m_OnClick;
            set => this.m_OnClick = value;
        }

        public void Press()
        {
            if (!this.IsActive() || !this.IsInteractable())
                return;
            UISystemProfilerApi.AddMarker("Button.onClick", this);
            this.m_OnClick.Invoke();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != 0)
                return;
            this.Press();
        }

        public new void OnSubmit(BaseEventData eventData)
        {
            this.Press();
            if (!this.IsActive() || !this.IsInteractable())
                return;
            this.DoStateTransition(Selectable.SelectionState.Pressed, false);
            this.StartCoroutine(this.OnFinishSubmit());
        }

        public IEnumerator OnFinishSubmit()
        {
            float fadeTime = this.colors.fadeDuration;
            float elapsedTime = 0.0f;
            while ((double)elapsedTime < (double)fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            this.DoStateTransition(this.currentSelectionState, false);
        }
    }
}
