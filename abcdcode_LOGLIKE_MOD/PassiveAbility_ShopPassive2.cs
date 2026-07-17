// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassive2</summary>

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
