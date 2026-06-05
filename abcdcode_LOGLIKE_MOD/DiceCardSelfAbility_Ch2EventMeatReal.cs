// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_Ch2EventMeatReal
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class DiceCardSelfAbility_Ch2EventMeatReal : LogDiceCardSelfAbility
    {
        public override void OnUseInstance(
          BattleUnitModel unit,
          BattleDiceCardModel self,
          BattleUnitModel targetUnit)
        {
            unit.RecoverHP(20);
            BookModel bookItem = unit.UnitData.unitData.bookItem;
            this.DeleteCard(unit, new LorId(LogLikeMod.ModId, 2000002));
            if (!bookItem.GetDeckAll_nocopy()[bookItem.GetCurrentDeckIndex()].MoveCardToInventory(new LorId(LogLikeMod.ModId, 2000002)))
                return;
            LogueBookModels.DeleteCard(new LorId(LogLikeMod.ModId, 2000002));
        }
    }
}
