// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive2_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive2_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassive2_2</summary>

    public class PassiveAbility_ShopPassive2_2 : PassiveAbilityBase
    {
        public PassiveAbility_ShopPassive2_2.StreetLightBuf curbuf;

        public override string debugDesc
        {
            get
            {
                return "무대 시작 시 무작위 다른 아군 하나에게 가로등 효과를 부여함. 막 시작 시 대상이 사망한 상태면 무작위 다른 아군 하나에게 다시 부여함. \n (가로등: 막 시작 시 행운 2를 얻음. '가로등' 지속능력을 가진 사서가 사망시 효과가 사라짐)";
            }
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.Bufing();
        }

        public void Bufing()
        {
            List<BattleUnitModel> all = BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll(x => x != this.owner);
            if (all == null || all.Count == 0)
                return;
            all.SelectOneRandom().bufListDetail.AddBuf(new PassiveAbility_ShopPassive2_2.StreetLightBuf(this));
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this.curbuf != null)
                return;
            this.Bufing();
        }

        /// <summary>StreetLightBuf</summary>

        public class StreetLightBuf : BattleUnitBuf
        {
            public PassiveAbility_ShopPassive2_2 parent;

            public StreetLightBuf(PassiveAbility_ShopPassive2_2 parent)
            {
                this.parent = parent;
                this.parent.curbuf = this;
            }

            public override void OnDie()
            {
                base.OnDie();
                this.parent.curbuf = null;
            }

            public override void OnRoundStartAfter()
            {
                base.OnRoundStartAfter();
                if (parent.curbuf != null)
                    _owner.bufListDetail.AddKeywordBufThisRoundByEtc(RoguelikeBufs.RMRLuck, 2, _owner);
            }
        }
    }
}
