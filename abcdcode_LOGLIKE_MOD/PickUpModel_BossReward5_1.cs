// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward5_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward5_1 : PickUpModelBase
    {
        public static DiceCardXmlInfo card;
        public override string KeywordId => "GlobalEffect_CupOfGreed_Effect";
        public override string KeywordIconId => "BossReward5";
        public PickUpModel_BossReward5_1():base()
        {

        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(PickUpModel_BossReward5_1.card);
            model.allyCardDetail.AddCardToHand(playingCard);
        }
    }
}
