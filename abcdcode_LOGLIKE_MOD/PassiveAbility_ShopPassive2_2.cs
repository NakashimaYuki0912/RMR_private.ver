// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive2_2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

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
