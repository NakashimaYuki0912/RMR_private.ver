// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassive2 : PassiveAbilityBase
{
  public GameObject effect;

  public override string debugDesc => "2막에 위력 +2";

  public override void OnRoundStart()
  {
    base.OnRoundStart();
    if (Singleton<StageController>.Instance.RoundTurn == 2)
    {
      Sprite artWork = LogLikeMod.ArtWorks["ShopPassive2"];
      this.effect = new GameObject();
      this.effect.transform.localScale = new Vector3(2f, 2f);
      this.effect.transform.parent = this.owner.view.transform;
      this.effect.layer = LayerMask.NameToLayer("Effect");
      this.effect.transform.localPosition = new Vector3(0.0f, 0.0f);
      SpriteRenderer spriteRenderer = this.effect.AddComponent<SpriteRenderer>();
      spriteRenderer.sprite = artWork;
      Color color = spriteRenderer.color;
      color.a = 0.5f;
      spriteRenderer.color = color;
      spriteRenderer.enabled = true;
      this.effect.SetActive(true);
    }
    else if ( this.effect !=  null)
    {
      Object.Destroy( this.effect);
      this.effect = (GameObject) null;
    }
  }

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    if (Singleton<StageController>.Instance.RoundTurn != 2)
      return;
    this.owner.battleCardResultLog?.SetPassiveAbility((PassiveAbilityBase) this);
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = 2
    });
  }
}
}
