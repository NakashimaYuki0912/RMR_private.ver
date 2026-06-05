// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.BehaviourAction_MachineDawnCard
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class BehaviourAction_MachineDawnCard : BehaviourActionBase
{
  public static bool movable = true;

  public override List<RencounterManager.MovingAction> GetMovingAction(
    ref RencounterManager.ActionAfterBehaviour self,
    ref RencounterManager.ActionAfterBehaviour opponent)
  {
    this.Log("Activate Action");
    bool flag = false;
    if (opponent.behaviourResultData != null)
      flag = opponent.behaviourResultData.IsFarAtk();
    List<RencounterManager.MovingAction> movingAction1 = new List<RencounterManager.MovingAction>();
    if (self.result == Result.Win && self.data.actionType == ActionType.Atk && !flag)
    {
      self.view.unitBottomStatUI.EnableCanvas(false);
      self.preventOverlap = false;
      RencounterManager.MovingAction movingAction2 = new RencounterManager.MovingAction(ActionDetail.S1, CharMoveState.MoveOpponent, 0.0f, false, 3f);
      movingAction2.SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE);
      movingAction1.Add(movingAction2);
      if (opponent.view.UnStopppable)
      {
        RencounterManager.MovingAction movingAction3 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, updateDir: false, delay: 0.0f);
        opponent.infoList.Add(movingAction3);
      }
      else
      {
        RencounterManager.MovingAction movingAction4 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.KnockDown, updateDir: false, delay: 0.0f);
        opponent.infoList.Add(movingAction4);
      }
      BehaviourAction_MachineDawnCard.movable = false;
    }
    else
    {
      movingAction1 = base.GetMovingAction(ref self, ref opponent);
      BehaviourAction_MachineDawnCard.movable = true;
    }
    return movingAction1;
  }
}
}
