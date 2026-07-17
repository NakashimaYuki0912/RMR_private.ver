// -----------------------------------------------------------------------------
// Enemy stage manager: EnemyTeamStageManager_TheCryingLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\EnemyTeamStageManager_TheCryingLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;
using UI;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>EnemyTeamStageManager_TheCryingLog</summary>

    public class EnemyTeamStageManager_TheCryingLog : EnemyTeamStageManager
    {
        public EnemyTeamStageManager_TheCrying.Phase _currentPhase;
        public int _stack = 5;
        public bool _finished;
        public bool _weak;
        public bool _recover;

        public EnemyTeamStageManager_TheCrying.Phase currentPhase => this._currentPhase;

        public int Stack => this._stack;
        #region --- Battle hooks ---


        public override void OnWaveStart()
        {
            this._stack = 5;
            this._weak = false;
            this._finished = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                alive.SetDeadSceneBlock(true);
            int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
            Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
        }

        public override void OnRoundEndTheLast() => this.CheckPhase();
        #endregion

        #region --- Getters / setters / checks ---


        public override bool IsStageFinishable() => this._finished;
        #endregion

        #region --- Battle hooks ---


        public override void OnRoundStart()
        {
            int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
            Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                PassiveAbilityBase passiveAbilityBase = alive.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_240528));
                if (passiveAbilityBase != null && !passiveAbilityBase.destroyed)
                {
                    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Player);
                    if (aliveList.Count > 0)
                    {
                        RandomUtil.SelectOne<BattleUnitModel>(aliveList).bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Stun, 1);
                        break;
                    }
                    break;
                }
            }
            if (!this._recover)
                return;
            this._recover = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                alive.cardSlotDetail.RecoverPlayPoint(1);
                alive.breakDetail.RecoverBreak(10);
            }
        }
        #endregion

        #region --- UI show / hide / build ---


        public override bool HideEnemyTarget()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                PassiveAbilityBase passiveAbilityBase = alive.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_240428));
                if (passiveAbilityBase != null && !passiveAbilityBase.destroyed)
                    return true;
            }
            return false;
        }
        #endregion

        #region --- Other helpers ---


        public override bool BlockEnemyAggroChange()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                PassiveAbilityBase passiveAbilityBase = alive.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_240428));
                if (passiveAbilityBase != null && !passiveAbilityBase.destroyed)
                    return true;
            }
            return false;
        }
        #endregion

        #region --- UI show / hide / build ---


        public override bool IsHideDiceAbilityInfo()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                PassiveAbilityBase passiveAbilityBase = alive.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_240328));
                if (passiveAbilityBase != null && !passiveAbilityBase.destroyed)
                    return true;
            }
            return false;
        }
        #endregion

        #region --- Battle hooks ---


        public override void OnAllEnemyEquipCard()
        {
            if (this._currentPhase != EnemyTeamStageManager_TheCrying.Phase.phase1 && this._currentPhase != EnemyTeamStageManager_TheCrying.Phase.FiveUnitPhase || this._stack <= 1)
                return;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Enemy);
            List<BattlePlayingCardDataInUnitModel> list = new List<BattlePlayingCardDataInUnitModel>();
            foreach (BattleUnitModel battleUnitModel in aliveList)
            {
                foreach (BattlePlayingCardDataInUnitModel cardDataInUnitModel in battleUnitModel.cardSlotDetail.cardAry)
                {
                    if (cardDataInUnitModel != null)
                    {
                        cardDataInUnitModel.card.ResetToOriginalData();
                        cardDataInUnitModel.card.CopySelf();
                        cardDataInUnitModel.ResetCardQueue();
                        list.Add(cardDataInUnitModel);
                    }
                }
            }
            if (list.Count <= 0 || this._weak)
                return;
            BattlePlayingCardDataInUnitModel cardDataInUnitModel1 = RandomUtil.SelectOne<BattlePlayingCardDataInUnitModel>(list);
            cardDataInUnitModel1.card.XmlData.DiceBehaviourList[0].Script = "cryingChildPenaltyLog";
            cardDataInUnitModel1.ResetCardQueue();
        }
        #endregion

        #region --- Getters / setters / checks ---


        public void SetAllWeak()
        {
            if (!this._weak)
            {
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    alive.Book.SetResistHP(BehaviourDetail.Slash, AtkResist.Weak);
                    alive.Book.SetResistHP(BehaviourDetail.Penetrate, AtkResist.Weak);
                    alive.Book.SetResistHP(BehaviourDetail.Hit, AtkResist.Weak);
                    alive.Book.SetResistBP(BehaviourDetail.Slash, AtkResist.Weak);
                    alive.Book.SetResistBP(BehaviourDetail.Penetrate, AtkResist.Weak);
                    alive.Book.SetResistBP(BehaviourDetail.Hit, AtkResist.Weak);
                    alive.bufListDetail.AddBuf((BattleUnitBuf)new BattleUnitBuf_theCryingWeak());
                }
            }
            this._weak = true;
        }

        public void SetPhase(EnemyTeamStageManager_TheCrying.Phase nextPhase)
        {
            this._weak = false;
            switch (nextPhase)
            {
                case EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase:
                    this.UnregisterAllUnit();
                    this.CreateTheOneUnit();
                    break;
                case EnemyTeamStageManager_TheCrying.Phase.FiveUnitPhase:
                    this.UnregisterAllUnit();
                    this.CreateFiveChildren();
                    break;
            }
            this._recover = true;
            int num = 0;
            this._currentPhase = nextPhase;
            foreach (BattleUnitModel battleUnitModel in (IEnumerable<BattleUnitModel>)BattleObjectManager.instance.GetList())
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++, true);
            BattleObjectManager.instance.InitUI();
        }
        #endregion

        #region --- Battle hooks ---


        public void UnregisterAllUnit()
        {
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Enemy))
            {
                if (!battleUnitModel.IsDead())
                    battleUnitModel.Die();
                battleUnitModel.isRegister = false;
            }
        }
        #endregion

        #region --- UI show / hide / build ---


        public void CreateTheOneUnit()
        {
            int id = this._stack < 4 ? (this._stack < 2 ? 42201 : 42101) : 42001;
            BattleUnitModel battleUnitModel = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, new LorId(LogLikeMod.ModId, id), 0, 180 + this._stack * 30);
            if (this._stack < 5)
            {
                if (this._stack == 4)
                    battleUnitModel.LoseHp(30);
                else if (this._stack == 3)
                {
                    battleUnitModel.view.ChangeSkin("TheCryingChildren_Damaged_1");
                    battleUnitModel.LoseHp(60);
                }
                else if (this._stack == 2)
                {
                    battleUnitModel.LoseHp(80 /*0x50*/);
                }
                else
                {
                    battleUnitModel.view.ChangeSkin("TheCryingChildren_Damaged_2");
                    battleUnitModel.LoseHp(100);
                }
            }
            battleUnitModel.SetDeadSceneBlock(true);
        }

        public void CreateFiveChildren()
        {
            if (this._stack >= 5)
            {
                this.AddNewChild(42005, 0, true);
                this.AddNewChild(42006, 1, true);
                this.AddNewChild(42007, 2, true);
                this.AddNewChild(42005, 3, true);
                this.AddNewChild(42006, 4, true);
            }
            else if (this._stack == 4)
            {
                this.AddNewChild(42005, 0, true);
                this.AddNewChild(42006, 1, true);
                this.AddNewChild(42007, 2, true);
                this.AddNewChild(42005, 3, true);
            }
            else if (this._stack == 3)
            {
                this.AddNewChild(42005, 0, true);
                this.AddNewChild(42006, 1, true);
                this.AddNewChild(42007, 2, true);
            }
            else if (this._stack == 2)
            {
                this.AddNewChild(42005, 0, true);
                this.AddNewChild(42006, 1, true);
            }
            else
            {
                this._finished = true;
                BattleUnitModel battleUnitModel = this.AddNewChild(42008, 0, true);
                battleUnitModel.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_10001());
                battleUnitModel.forceRetreat = true;
                PassiveAbilityBase passive1 = battleUnitModel.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_240228));
                if (passive1 != null)
                    battleUnitModel.passiveDetail.DestroyPassive(passive1);
                PassiveAbilityBase passive2 = battleUnitModel.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_240628));
                if (passive2 == null)
                    return;
                battleUnitModel.passiveDetail.DestroyPassive(passive2);
            }
        }
        #endregion

        #region --- Other helpers ---


        public BattleUnitModel AddNewChild(int id, int index, bool addAngelBuf)
        {
            BattleUnitModel battleUnitModel = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, new LorId(LogLikeMod.ModId, id), index);
            if (this._stack >= 5)
            {
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508006));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508006));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508006));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508006));
            }
            else if (this._stack >= 4)
            {
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508006));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508006));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508007));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508007));
            }
            else if (this._stack >= 3)
            {
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508007));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508007));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508007));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508007));
                battleUnitModel.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 508007));
            }
            else
            {
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508008));
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508008));
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508008));
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508008));
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508008));
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508008));
                battleUnitModel.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 508008));
            }
            if (this._stack > 1)
                battleUnitModel.SetDeadSceneBlock(true);
            return battleUnitModel;
        }

        public void CheckPhase()
        {
            switch (this._currentPhase)
            {
                case EnemyTeamStageManager_TheCrying.Phase.phase1:
                    if (!this.Phase1_AllDeadChild())
                        break;
                    this.SetPhase(EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase);
                    break;
                case EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase:
                    if (!this.Phase1_AllDeadChild())
                        break;
                    this.SetPhase(EnemyTeamStageManager_TheCrying.Phase.FiveUnitPhase);
                    break;
                case EnemyTeamStageManager_TheCrying.Phase.FiveUnitPhase:
                    if (!this.Phase1_AllDeadChild())
                        break;
                    this._stack -= 2;
                    if (this._stack > 0)
                    {
                        this.SetPhase(EnemyTeamStageManager_TheCrying.Phase.OneUnitPhase);
                        break;
                    }
                    this._finished = true;
                    break;
            }
        }

        public bool Phase1_AllDeadChild()
        {
            return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Count <= 0;
        }

        public bool Phase2_hpLimit() => false;
        #endregion

        #region --- Battle hooks ---


        public override void OnStageClear()
        {
        }

        /// <summary>enum Phase</summary>

        public enum Phase
        {
            phase1,
            OneUnitPhase,
            FiveUnitPhase,
        }

        public static implicit operator EnemyTeamStageManager_TheCrying(EnemyTeamStageManager_TheCryingLog obj)
        {
            EnemyTeamStageManager_TheCrying manager = new EnemyTeamStageManager_TheCrying();
            manager._stack = obj._stack;
            manager._weak = obj._weak;
            manager._currentPhase = obj._currentPhase;
            manager._finished = obj._finished;
            manager._recover = obj._recover;
            return manager;
        }
        #endregion

    }
}
