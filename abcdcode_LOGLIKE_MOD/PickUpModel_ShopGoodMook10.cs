// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodMook10
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodMook10.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGoodMook10 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodMook10()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8583010));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 83010);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGoodMook10.Mook10Effect());
        }

        /// <summary>Mook10Effect</summary>

        public class Mook10Effect : GlobalLogueEffectBase
        {
            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8583010));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8583010));
            }

            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassiveMook10"];

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.card.owner.faction != Faction.Player)
                    return;
                behavior.AddAbility((DiceCardAbilityBase)new PickUpModel_ShopGoodMook10.Mook10Effect.Mook10Ability());
            }

            /// <summary>Mook10Ability</summary>

            public class Mook10Ability : DiceCardAbilityBase
            {
                public override void OnSucceedAttack(BattleUnitModel target)
                {
                    base.OnSucceedAttack(target);
                    target.TakeDamage(1, DamageType.ETC);
                }
            }
        }
    }
}
