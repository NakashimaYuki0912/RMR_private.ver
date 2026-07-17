// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_BossReward5_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_BossReward5_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
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
