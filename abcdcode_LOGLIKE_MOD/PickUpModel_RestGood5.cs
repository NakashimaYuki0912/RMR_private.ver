// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_RestGood5
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_RestGood5.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_RestGood5</summary>

    public class PickUpModel_RestGood5 : RestPickUp
    {
        public PickUpModel_RestGood5()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 800005));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 800005);
            this.type = RestPickUp.RestPickUpType.Main;
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            model.RecoverHP(model.MaxHp);
            if (!model.IsDead())
                return;
            model.Revive(model.MaxHp);
        }

        public override void OnChoice(RestGood good) => RestPickUp.AddPassiveReward(this.id);
    }
}
