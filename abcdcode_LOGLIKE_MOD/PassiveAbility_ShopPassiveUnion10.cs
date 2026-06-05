// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveUnion10
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveUnion10 : PassiveAbilityBase
{
  public List<LorId> usedlist;

  public override string debugDesc => "이전 전투에서 사용했던 전투 책장 사용 시 최대값 + 1. 행운 1 적용";

  public override void OnWaveStart()
  {
    this.usedlist = new List<LorId>();
    List<GlobalLogueEffectBase> all = Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().FindAll((Predicate<GlobalLogueEffectBase>) (x => x is PassiveAbility_ShopPassiveUnion10.Union10Effect));
    if (all.Count > 0)
    {
      PassiveAbility_ShopPassiveUnion10.Union10Effect effect = (PassiveAbility_ShopPassiveUnion10.Union10Effect) all.Find((Predicate<GlobalLogueEffectBase>) (x => (x as PassiveAbility_ShopPassiveUnion10.Union10Effect).owner == this.owner.UnitData));
      if (effect != null)
      {
        this.usedlist.AddRange((IEnumerable<LorId>) effect.usedlist);
        Singleton<GlobalLogueEffectManager>.Instance.RemoveEffect((GlobalLogueEffectBase) effect);
      }
    }
    new PassiveAbility_ShopPassiveUnion10.Union10Effect().Init(this.owner);
  }

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    base.BeforeRollDice(behavior);
    if (!this.usedlist.Contains(behavior.card.card.GetID()))
      return;
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      max = 1
    });
  }

  public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
  {
    int diceMin = behavior.GetDiceMin();
    int diceMax = behavior.GetDiceMax();
    int num1 = diceResult;
    for (int index = 0; index < 1; ++index)
    {
      int num2 = DiceStatCalculator.MakeDiceResult(diceMin, diceMax, 0);
      if (num2 > num1)
        num1 = num2;
    }
    diceResult = num1;
  }

  public class Union10Effect : GlobalLogueEffectBase
  {
    public UnitBattleDataModel owner;
    public List<LorId> usedlist;

    public void Init(BattleUnitModel owner)
    {
      this.owner = owner.UnitData;
      this.usedlist = new List<LorId>();
    }

    public override SaveData GetSaveData()
    {
      SaveData saveData = base.GetSaveData();
      SaveData data = new SaveData();
      if (this.usedlist.Count > 0)
      {
        foreach (LorId id in this.usedlist)
          data.AddToList(id.LogGetSaveData());
      }
      saveData.AddData("usedlist", data);
      saveData.AddData("owner", this.owner.unitData.bookItem.BookId.LogGetSaveData());
      return saveData;
    }

    public override void LoadFromSaveData(SaveData save)
    {
      base.LoadFromSaveData(save);
      this.usedlist = new List<LorId>();
      foreach (SaveData data in save.GetData("usedlist"))
        this.usedlist.Add(ExtensionUtils.LogLoadFromSaveData(data));
      this.owner = LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>) (x => x.unitData.bookItem.BookId == ExtensionUtils.LogLoadFromSaveData(save.GetData("owner"))));
    }

    public override void OnUseCard(BattlePlayingCardDataInUnitModel cardmodel)
    {
      base.OnUseCard(cardmodel);
      LorId id = cardmodel.card.GetID();
      if (this.usedlist.Contains(id))
        return;
      this.usedlist.Add(id);
    }
  }
}
}
