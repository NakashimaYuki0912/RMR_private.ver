// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_RestGood4
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_RestGood4.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_RestGood4</summary>

    public class PickUpModel_RestGood4 : RestPickUp
    {
        public PickUpModel_RestGood4()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 800004));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 800004);
            this.type = RestPickUp.RestPickUpType.Main;
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnChoice(RestGood good) => LogueBookModels.AddMoney(15);
    }
}
