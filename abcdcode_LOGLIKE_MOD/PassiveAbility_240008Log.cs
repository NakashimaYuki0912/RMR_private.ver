// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_240008Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_240008Log : PassiveAbilityBase
    {
        public BattleUnitModel _noahUnit;
        public bool _readyGetOff;
        public int _noahMaxHp = 52;
        public int _noahHp = 52;
        public int _patternCount;
        public PassiveAbility_240008.BossState _state;

        public override string debugDesc
        {
            get => "매 막 마다 손과 덱에 있는 모든 책장을 소멸시키고 사용할 책장을 손에 추가. 모든 책장의 비용이 0이 됨.";
        }

        public bool HasNoah => this._noahUnit == null;

        public PassiveAbility_240008.BossState State => this._state;

        public override void OnWaveStart()
        {
            this._noahHp = this._noahMaxHp;
            this._noahUnit = (BattleUnitModel)null;
            this._readyGetOff = false;
            this._patternCount = 0;
        }

        public override int SpeedDiceNumAdder()
        {
            PassiveAbility_240008.BossState state = this._state;
            if (state <= PassiveAbility_240008.BossState.NoahDead)
                return 2;
            return state != PassiveAbility_240008.BossState.EmaOnly ? 1 : 1;
        }

        public override int SpeedDiceBreakedAdder()
        {
            switch (this._state)
            {
                case PassiveAbility_240008.BossState.EmaNoah:
                case PassiveAbility_240008.BossState.EmaOnly:
                    return 0;
                case PassiveAbility_240008.BossState.NoahDead:
                    return 1;
                default:
                    return 0;
            }
        }

        public override void OnRoundStart()
        {
            int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
            Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
            this.owner.allyCardDetail.ExhaustAllCards();
            this.owner.cardSlotDetail.RecoverPlayPoint(6);
            if (this._patternCount % 3 == 0)
            {
                this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, 503002));
                this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, 503008));
            }
            else if (this._patternCount % 3 == 1)
            {
                this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, 503002));
                this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, 503002));
            }
            else
            {
                this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, 503001));
                this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, 503002));
            }
            if (this._state == PassiveAbility_240008.BossState.EmaNoah)
                this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, 503003));
            else if (this._state != PassiveAbility_240008.BossState.EmaOnly && this._state == PassiveAbility_240008.BossState.NoahDead)
            {
                this.owner.Book.SetResistHP(BehaviourDetail.Slash, AtkResist.Weak);
                this.owner.Book.SetResistHP(BehaviourDetail.Penetrate, AtkResist.Weak);
                this.owner.Book.SetResistHP(BehaviourDetail.Hit, AtkResist.Weak);
            }
            foreach (BattleDiceCardModel battleDiceCardModel in this.owner.allyCardDetail.GetHand())
                battleDiceCardModel.SetCostToZero();
            ++this._patternCount;
        }

        public override void OnRoundEnd()
        {
        }

        public override void OnRoundEndTheLast()
        {
            if (this.HasNoah)
            {
                if (this._readyGetOff)
                {
                    this.GetOffNoah();
                    this.owner.UnitData.unitData.SetCustomName(Singleton<CharactersNameXmlList>.Instance.GetName(96 /*0x60*/));
                    BattleObjectManager.instance.InitUI();
                }
            }
            else
            {
                if (this._noahUnit.IsDead())
                {
                    this._noahHp = 0;
                    Singleton<StageController>.Instance.RemoveUnit(Faction.Enemy, 4);
                    this.owner.UnitData.unitData.SetCustomName(Singleton<CharactersNameXmlList>.Instance.GetName(96 /*0x60*/));
                }
                else
                {
                    this._noahHp = (int)this._noahUnit.hp;
                    Singleton<StageController>.Instance.RemoveUnit(Faction.Enemy, 4);
                    this.owner.UnitData.unitData.SetCustomName(Singleton<CharactersNameXmlList>.Instance.GetName(95));
                }
                this._noahUnit = (BattleUnitModel)null;
                BattleObjectManager.instance.InitUI();
            }
            if (!this.HasNoah)
            {
                if (this._state != PassiveAbility_240008.BossState.EmaOnly)
                    this.SetMapFilter(false);
                this._state = PassiveAbility_240008.BossState.EmaOnly;
                this.owner.view.ChangeSkin("Ema");
            }
            else if (this._noahHp <= 0)
            {
                if (this._state != PassiveAbility_240008.BossState.NoahDead)
                    this.SetMapBrightness(false);
                this._state = PassiveAbility_240008.BossState.NoahDead;
                this.owner.view.ChangeSkin("EmaNoah_NoahDown");
            }
            else
            {
                if (this._state != 0)
                    this.SetMapFilter(true);
                this._state = PassiveAbility_240008.BossState.EmaNoah;
                this.owner.view.ChangeSkin("EmaNoah");
            }
        }

        public void SetMapFilter(bool union)
        {
            if (!(SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject is CircusMapManager))
                return;
            (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject as CircusMapManager).SetFilter(union);
        }

        public void SetMapBrightness(bool toBright)
        {
            try
            {
                if (!(SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject is CircusMapManager))
                    return;
                (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject as CircusMapManager).SetMapBrightness(toBright);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public override bool OnBreakGageZero()
        {
            if (!this.HasNoah || this._noahHp <= 0)
                return false;
            this._readyGetOff = true;
            return true;
        }

        public void GetOffNoah()
        {
            if (this._noahHp <= 0)
                return;
            this._readyGetOff = false;
            this._noahUnit = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, new LorId(LogLikeMod.ModId, 40010), 4);
            if (this._noahUnit != null)
            {
                this._noahUnit.LoseHp(this._noahMaxHp - this._noahHp);
                this._noahUnit.SetDeadSceneBlock(true);
            }
            this.owner.ResetBreakGauge();
            int num = 0;
            foreach (BattleUnitModel battleUnitModel in (IEnumerable<BattleUnitModel>)BattleObjectManager.instance.GetList())
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++);
        }

        public override void OnDieOtherUnit(BattleUnitModel unit) => base.OnDieOtherUnit(unit);

        public enum BossState
        {
            EmaNoah,
            NoahDead,
            EmaOnly,
        }
    }
}
