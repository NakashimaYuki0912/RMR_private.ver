// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardPriority_MachineDawnCard2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardPriority_MachineDawnCard2 : DiceCardPriorityBase
{
  public override int GetPriorityBonus(BattleUnitModel owner)
  {
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction == Faction.Enemy ? Faction.Player : Faction.Enemy))
    {
      if (alive.bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable) != null)
        return 5;
    }
    return -10;
  }
}
}
