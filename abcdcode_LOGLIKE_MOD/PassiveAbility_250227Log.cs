// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_250227Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_250227Log : PassiveAbilityBase
    {
        public int _teleportCondition = 350;
        public int _patternCount;
        public int _teleported;
        public int _stanceCooltime;
        public List<PurpleStance> _alreayUsed = new List<PurpleStance>();
        public PassiveAbility_250127 _stancePassive;
        public int _areaCoolTime = 1;
        public bool _teleportReady;
        public int _dmgReduction;

        public override int SpeedDiceNumAdder() => this._patternCount <= 3 ? 2 : 3;

        public int GetCurrentSpeedDiceNum()
        {
            return this.owner.Book.GetSpeedDiceRule(this.owner).Roll(this.owner).Count;
        }

        public override void OnWaveStart()
        {
            this._patternCount = this.owner.UnitData.floorBattleData.param1;
            this._teleported = this.owner.UnitData.floorBattleData.param2;
            this._areaCoolTime = this._teleported <= 0 ? 0 : 1;
            this._stancePassive = this.owner.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_250127)) as PassiveAbility_250127;
        }

        public override void OnRoundEnd()
        {
            this.owner.cardSlotDetail.RecoverPlayPoint(this.owner.cardSlotDetail.GetMaxPlayPoint());
        }

        public override void OnRoundStart()
        {
            if (this.owner.UnitData.floorBattleData.param2 > 0 || !this._teleportReady && (double)this.owner.hp > (double)this._teleportCondition)
                return;
            LogLikeMod.purpleexcept = true;
            this.owner.UnitData.floorBattleData.param2 = 1;
            List<StageLibraryFloorModel> availableFloorList = Singleton<StageController>.Instance.GetStageModel().GetAvailableFloorList();
            availableFloorList.RemoveAll(x => x.Sephirah != Singleton<StageController>.Instance.CurrentFloor);
            if (availableFloorList.Count > 0)
                Singleton<StageController>.Instance.ChangeFloorForcely(RandomUtil.SelectOne(availableFloorList).Sephirah, this.owner);
        }

        public override void OnRoundStartAfter()
        {
            --this._stanceCooltime;
            if (this._stanceCooltime <= 0)
                this.UpdateStance();
            this.SetCards();
            ++this._patternCount;
            this.owner.UnitData.floorBattleData.param1 = this._patternCount;
        }

        public void SetCards()
        {
            this.owner.allyCardDetail.ExhaustAllCards();
            if (this.owner.UnitData.floorBattleData.param2 > 0)
            {
                if (this._areaCoolTime <= 0)
                {
                    this.AddNewCard(609013);
                    this._areaCoolTime = 2;
                }
                else
                    --this._areaCoolTime;
            }
            switch (this._stancePassive.CurrentStance)
            {
                case PurpleStance.Slash:
                    this.SetCards_slash();
                    break;
                case PurpleStance.Penetrate:
                    this.SetCards_penetrate();
                    break;
                case PurpleStance.Hit:
                    this.SetCards_hit();
                    break;
                case PurpleStance.Defense:
                    this.SetCards_defense();
                    break;
            }
            ++this._patternCount;
        }

        public void SetCards_slash()
        {
            this.AddNewCard(609001);
            this.AddNewCard(609002);
            this.AddNewCard(609003);
            this.AddNewCard(609001);
            this.AddNewCard(609002);
            this.AddNewCard(609003);
        }

        public void SetCards_penetrate()
        {
            this.AddNewCard(609004);
            this.AddNewCard(609005);
            this.AddNewCard(609006);
            this.AddNewCard(609004);
            this.AddNewCard(609005);
            this.AddNewCard(609006);
        }

        public void SetCards_hit()
        {
            this.AddNewCard(609007);
            this.AddNewCard(609008);
            this.AddNewCard(609009);
            this.AddNewCard(609007);
            this.AddNewCard(609008);
            this.AddNewCard(609009);
        }

        public void SetCards_defense()
        {
            this.AddNewCard(609010);
            this.AddNewCard(609011);
            this.AddNewCard(609012);
            this.AddNewCard(609010);
            this.AddNewCard(609011);
        }

        public void UpdateStance()
        {
            if (this._alreayUsed.Count >= 4)
                this._alreayUsed.Clear();
            int num = this.owner.UnitData.floorBattleData.param2;
            if (num > 0 && !this._alreayUsed.Contains(PurpleStance.Defense))
                this._alreayUsed.Add(PurpleStance.Defense);
            List<PurpleStance> list = new List<PurpleStance>()
            {
              PurpleStance.Slash,
              PurpleStance.Penetrate,
              PurpleStance.Hit,
              PurpleStance.Defense
            };
            foreach (PurpleStance purpleStance in this._alreayUsed)
                list.Remove(purpleStance);
            switch (RandomUtil.SelectOne<PurpleStance>(list))
            {
                case PurpleStance.Slash:
                    this._stancePassive.ChangeStance_slash();
                    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.SlashPowerUp, 1);
                    this._stanceCooltime = num <= 0 ? 2 : 1;
                    this._alreayUsed.Add(PurpleStance.Slash);
                    break;
                case PurpleStance.Penetrate:
                    this._stancePassive.ChangeStance_penetrate();
                    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.PenetratePowerUp, 1);
                    this._stanceCooltime = num <= 0 ? 2 : 1;
                    this._alreayUsed.Add(PurpleStance.Penetrate);
                    break;
                case PurpleStance.Hit:
                    this._stancePassive.ChangeStance_hit();
                    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.HitPowerUp, 1);
                    this._stanceCooltime = num <= 0 ? 2 : 1;
                    this._alreayUsed.Add(PurpleStance.Hit);
                    break;
                case PurpleStance.Defense:
                    this._stancePassive.ChangeStance_defense();
                    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DefensePowerUp, 1);
                    this._stanceCooltime = 1;
                    this._alreayUsed.Add(PurpleStance.Defense);
                    break;
            }
        }

        public int AddNewCard(int id)
        {
            BattleDiceCardModel battleDiceCardModel = this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, id));
            return battleDiceCardModel != null ? battleDiceCardModel.GetOriginCost() : 1;
        }

        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            this._dmgReduction = 0;
            if (this.owner.UnitData.floorBattleData.param2 <= 0 && ((double)this.owner.hp <= (double)this._teleportCondition || (double)this.owner.hp - (double)dmg <= (double)this._teleportCondition))
            {
                this._dmgReduction = (int)((double)this._teleportCondition - ((double)this.owner.hp - (double)dmg));
                this._teleportReady = true;
            }
            return base.BeforeTakeDamage(attacker, dmg);
        }

        public override int GetDamageReductionAll()
        {
            int damageReductionAll;
            if (this.owner.UnitData.floorBattleData.param2 <= 0 && (double)this.owner.hp <= (double)this._teleportCondition)
            {
                damageReductionAll = 9999;
                this._teleportReady = true;
            }
            else
                damageReductionAll = this._dmgReduction;
            return damageReductionAll;
        }

        public override void OnBattleEnd_alive()
        {
            bool teleportReady = this._teleportReady;
        }
    }
}
