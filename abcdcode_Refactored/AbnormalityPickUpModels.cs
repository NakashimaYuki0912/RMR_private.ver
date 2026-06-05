using Battle.CameraFilter;
using Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LOR_DiceSystem;
using Battle.CreatureEffect;

namespace abcdcode_LOGLIKE_MOD
{
    public class PickUpModel_Alriune : PickUpModelBase
    {
        public PickUpModel_Alriune()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Alriune_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Alriune_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370341));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370342));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370343));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 4
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Alriune0 : CreaturePickUpModel
    {
        public PickUpModel_Alriune0()
        {
            this.level = 4;
            this.ids = new List<LorId>()
			{
			  new LorId(LogLikeMod.ModId, 15370341),
			  new LorId(LogLikeMod.ModId, 15370342),
			  new LorId(LogLikeMod.ModId, 15370343)
			};
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Alriune_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Alriune"];
    }

    public class PickUpModel_Alriune1 : CreaturePickUpModel
    {
        public PickUpModel_Alriune1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Alriune1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Alriune1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Alriune1_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370341), (EmotionCardAbilityBase)new PickUpModel_Alriune1.LogEmotionCardAbility_Alriune1(), model);
        }

        public class LogEmotionCardAbility_Alriune1 : EmotionCardAbilityBase
        {
            public const int healMin = 5;
            public const int healMax = 10;
            public const int _bDmgMin = 4;
            public const int _bDmgMax = 8;
            public const float _hpRate = 0.1f;
            public int _dmgStack;

            public int Heal => RandomUtil.Range(5, 10);

            public int BDmg => RandomUtil.Range(4, 8);

            public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
            {
                if (this._owner.IsImmuneDmg())
                    return base.BeforeTakeDamage(attacker, dmg);
                if (this._owner.passiveDetail.IsInvincible())
                    return base.BeforeTakeDamage(attacker, dmg);
                this._dmgStack += dmg;
                if ((double)this._dmgStack >= (double)this._owner.MaxHp * 0.10000000149011612)
                    this.Ability();
                return base.BeforeTakeDamage(attacker, dmg);
            }

            public void Ability()
            {
                this._dmgStack = 0;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                    alive.TakeBreakDamage(this.BDmg, DamageType.Emotion, this._owner);
                this._owner.RecoverHP(this.Heal);
                if (!Singleton<StageController>.Instance.IsLogState())
                {
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_FlowerPiece", 1f, this._owner.view, this._owner.view, 2f);
                    SoundEffectPlayer.PlaySound("Creature/Ali_FarAtk");
                }
                else
                {
                    this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_FlowerPiece", 2f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Ali_FarAtk");
                }
            }
        }
    }

    public class PickUpModel_Alriune2 : CreaturePickUpModel
    {
        public PickUpModel_Alriune2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Alriune2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Alriune2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Alriune2_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370342), (EmotionCardAbilityBase)new PickUpModel_Alriune2.LogEmotionCardAbility_Alriune2(), model);
        }

        public class LogEmotionCardAbility_Alriune2 : EmotionCardAbilityBase
        {
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel owner = atkDice?.owner;
                if (owner == null)
                    return;
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Alriune_Debuf, 1, this._owner);
                owner.battleCardResultLog?.SetCreatureAbilityEffect("0/Alriune_Stun_Effect", 1f);
                owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Ali_Guard");
            }
        }
    }

    public class PickUpModel_Alriune3 : CreaturePickUpModel
    {
        public PickUpModel_Alriune3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Alriune3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Alriune3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Alriune3_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370343), (EmotionCardAbilityBase)new PickUpModel_Alriune3.LogEmotionCardAbility_Alriune3(), model);
        }

        public class LogEmotionCardAbility_Alriune3 : EmotionCardAbilityBase
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Enemy);
                if (aliveList.Count <= 0)
                    return;
                aliveList[UnityEngine.Random.Range(0, aliveList.Count)].bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune(this._owner));
            }

            public class BattleUnitBuf_Emotion_Alriune : BattleUnitBuf
            {
                public const int _bDmgMin = 3;
                public const int _bDmgMax = 7;
                public const int _maxCnt = 4;
                public BattleUnitModel _target;
                public int cnt;
                public Battle.CreatureEffect.CreatureEffect _aura;

                public static int BDmg => RandomUtil.Range(3, 7);

                public override string keywordId => "Alriune_Flower";

                public override string keywordIconId => "Alriune_Petal";

                public BattleUnitBuf_Emotion_Alriune(BattleUnitModel target)
                {
                    this._target = target;
                    this.stack = 0;
                }

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_Spring", 1f, this._owner.view, this._owner.view);
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }

                public override void OnDie()
                {
                    base.OnDie();
                    this.Destroy();
                }

                public override void Destroy()
                {
                    base.Destroy();
                    this.DestroyAura();
                }

                public void DestroyAura()
                {
                    if (!(this._aura != null))
                        return;
                    GameObject.Destroy(this._aura.gameObject);
                    this._aura = (Battle.CreatureEffect.CreatureEffect)null;
                }

                public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
                {
                    base.OnTakeDamageByAttack(atkDice, dmg);
                    BattleUnitModel owner = atkDice?.card?.owner;
                    if (owner == null || owner != this._target || this.cnt >= 4)
                        return;
                    ++this.cnt;
                    if (this.cnt >= 4)
                    {
                        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                            alive.TakeBreakDamage(PickUpModel_Alriune3.LogEmotionCardAbility_Alriune3.BattleUnitBuf_Emotion_Alriune.BDmg, DamageType.Buf, this._owner);
                        this._target?.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune2());
                    }
                }

                public void Curtain()
                {
                    Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_SpringAct");
                    if (original != null)
                    {
                        Battle.CreatureEffect.CreatureEffect creatureEffect = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                        if ((creatureEffect != null ? creatureEffect.gameObject.GetComponent<AutoDestruct>() : null) == null)
                        {
                            AutoDestruct autoDestruct = creatureEffect != null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                            if (autoDestruct != null)
                                autoDestruct.time = 3f;
                        }
                    }
                    SoundEffectPlayer.PlaySound("Creature/Ali_curtain");
                }
            }

            public class BattleUnitBuf_Emotion_Alriune2 : BattleUnitBuf
            {
                public bool added = true;
                public bool effect;

                public override string keywordId => "NoTargeting";

                public override string keywordIconId => "Alriune_Attacker";

                public override bool IsTargetable() => this.added;

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    if (this.added)
                        this.added = false;
                    else
                        this.Destroy();
                }

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    if (this.effect)
                        return;
                    this.effect = true;
                    this.Curtain();
                }

                public void Curtain()
                {
                    Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_SpringAct");
                    if (!(original != null))
                        return;
                    Battle.CreatureEffect.CreatureEffect creatureEffect = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                    if ((creatureEffect != null ? creatureEffect.gameObject.GetComponent<AutoDestruct>() : null) == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect != null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                        if (autoDestruct != null)
                            autoDestruct.time = 3f;
                    }
                }
            }
        }
    }

    public class PickUpModel_Angry : PickUpModelBase
    {
        public PickUpModel_Angry()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Angry_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Angry_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370351));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370352));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370353));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 4
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Angry0 : CreaturePickUpModel
    {
        public PickUpModel_Angry0()
        {
            this.level = 4;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370351),
      new LorId(LogLikeMod.ModId, 15370352),
      new LorId(LogLikeMod.ModId, 15370353)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Angry_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Angry"];
    }

    public class PickUpModel_Angry1 : CreaturePickUpModel
    {
        public PickUpModel_Angry1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Angry1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Angry1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Angry1_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370351), (EmotionCardAbilityBase)new PickUpModel_Angry1.LogEmotionCardAbility_Angry1(), model);
        }

        public class LogEmotionCardAbility_Angry1 : EmotionCardAbilityBase
        {
            public override void OnWaveStart()
            {
                base.OnWaveStart();
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_Angry1.LogEmotionCardAbility_Angry1.BattleUnitBuf_Emotion_Wrath_Berserk)) != null)
                    return;
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_Angry1.LogEmotionCardAbility_Angry1.BattleUnitBuf_Emotion_Wrath_Berserk());
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_Angry1.LogEmotionCardAbility_Angry1.BattleUnitBuf_Emotion_Wrath_Berserk());
            }

            public class BattleUnitBuf_Emotion_Wrath_Berserk : BattleUnitBuf
            {
                public GameObject aura;
                public const int _str = 2;
                public const int _pp = 2;
                public const int _draw = 2;

                public override string keywordId => "Angry_Angry";

                public override string keywordIconId => "Wrath_Head";

                public override bool IsControllable => this.Controlable();

                public bool Controlable()
                {
                    return this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null;
                }

                public override bool TeamKill() => true;

                public override void OnRoundEndTheLast()
                {
                    base.OnRoundEndTheLast();
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2, this._owner);
                }

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    this._owner.cardSlotDetail.RecoverPlayPoint(2);
                    this._owner.allyCardDetail.DrawCards(2);
                }

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Rage", 1f, owner.view, owner.view);
                    this.aura = fxCreatureEffect != null ? fxCreatureEffect.gameObject : (GameObject)null;
                    SoundEffectPlayer.PlaySound("Creature/Angry_Meet");
                }

                public override void OnDie()
                {
                    base.OnDie();
                    this.Destroy();
                }

                public override void Destroy()
                {
                    base.Destroy();
                    this.DestroyAura();
                }

                public void DestroyAura()
                {
                    if (!(this.aura != null))
                        return;
                    UnityEngine.Object.Destroy(this.aura);
                    this.aura = (GameObject)null;
                }
            }
        }
    }

    public class PickUpModel_Angry2 : CreaturePickUpModel
    {
        public PickUpModel_Angry2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Angry2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Angry2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Angry2_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370352), (EmotionCardAbilityBase)new PickUpModel_Angry2.LogEmotionCardAbility_Angry2(), model);
        }

        public class LogEmotionCardAbility_Angry2 : EmotionCardAbilityBase
        {
            public const int _pp = 3;
            public const int _hp = 10;
            public const int _break = 10;

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null)
                    return;
                if (this.GetAliveFriend() == null)
                {
                    target.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_Angry2.LogEmotionCardAbility_Angry2.BattleUnitBuf_Emotion_Wrath_Friend());
                }
                else
                {
                    if (this.GetAliveFriend() != target)
                        return;
                    target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_ATKMarker", 1.5f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Angry_R_StrongAtk");
                }
            }

            public override void OnKill(BattleUnitModel target)
            {
                base.OnKill(target);
                if (target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(y => y is PickUpModel_Angry2.LogEmotionCardAbility_Angry2.BattleUnitBuf_Emotion_Wrath_Friend)) != null)
                {
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                    {
                        alive.cardSlotDetail.RecoverPlayPoint(3);
                        alive.breakDetail.RecoverBreak(10);
                        alive.RecoverHP(10);
                    }
                    this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.KillEffect));
                }
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Angry_Vert2");
            }

            public void KillEffect()
            {
                CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.6f);
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/5_T/FX_IllusionCard_5_T_SmokeWater");
                if (!(original != null))
                    return;
                Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                if ((creatureEffect != null ? creatureEffect.gameObject.GetComponent<AutoDestruct>() : null) == null)
                {
                    AutoDestruct autoDestruct = creatureEffect != null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }

            public BattleUnitModel GetAliveFriend()
            {
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(y => y is PickUpModel_Angry2.LogEmotionCardAbility_Angry2.BattleUnitBuf_Emotion_Wrath_Friend)) != null));
            }

            public class BattleUnitBuf_Emotion_Wrath_Friend : BattleUnitBuf
            {
                public override string keywordId => "Angry_Friend";

                public override string keywordIconId => "Reclus_Head";

                public BattleUnitBuf_Emotion_Wrath_Friend() => this.stack = 0;
            }
        }
    }

    public class PickUpModel_Angry3 : CreaturePickUpModel
    {
        public PickUpModel_Angry3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Angry3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Angry3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Angry3_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370353), (EmotionCardAbilityBase)new PickUpModel_Angry3.LogEmotionCardAbility_Angry3(), model);
        }

        public class LogEmotionCardAbility_Angry3 : EmotionCardAbilityBase
        {
            public const int _decay = 5;
            public bool _effect;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 5, this._owner);
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (!this._effect)
                {
                    this._effect = true;
                    Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/5/Servant_Emotion_Effect");
                    if (original != null)
                    {
                        Battle.CreatureEffect.CreatureEffect creatureEffect = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                        if ((creatureEffect != null ? creatureEffect.gameObject.GetComponent<AutoDestruct>() : null) == null)
                        {
                            AutoDestruct autoDestruct = creatureEffect != null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                            if (autoDestruct != null)
                            {
                                autoDestruct.time = 3f;
                                autoDestruct.DestroyWhenDisable();
                            }
                        }
                    }
                    SoundEffectPlayer.PlaySound("Creature/Angry_StrongFinish");
                }
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    int kewordBufStack = alive.bufListDetail.GetKewordBufStack(KeywordBuf.Decay);
                    if (kewordBufStack > 0)
                        alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, kewordBufStack, this._owner);
                }
            }
        }
    }

    public class PickUpModel_BigBadWolf : PickUpModelBase
    {
        public PickUpModel_BigBadWolf()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BigBadWolf_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BigBadWolf_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370161));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370162));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370163));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_BigBadWolf0 : CreaturePickUpModel
    {
        public PickUpModel_BigBadWolf0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370161),
      new LorId(LogLikeMod.ModId, 15370162),
      new LorId(LogLikeMod.ModId, 15370163)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_BigBadWolf_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_BigBadWolf"];
    }

    public class PickUpModel_BigBadWolf1 : CreaturePickUpModel
    {
        public PickUpModel_BigBadWolf1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BigBadWolf1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BigBadWolf1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BigBadWolf1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370161), (EmotionCardAbilityBase)new PickUpModel_BigBadWolf1.LogEmotionCardAbility_BigBadWolf1(), model);
        }

        public class LogEmotionCardAbility_BigBadWolf1 : EmotionCardAbilityBase
        {
            public const int _powMin = 1;
            public const int _powMax = 2;
            public const int _healMin = 3;
            public const int _healMax = 7;
            public BattleDiceBehavior last;
            public bool win;

            public static int Pow => RandomUtil.Range(1, 2);

            public static int Heal => RandomUtil.Range(3, 7);

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Wolf_Filter_Sheep", false, 2f);
            }

            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnUseCard(curCard);
                this.win = false;
                if (curCard == null)
                    return;
                BattleDiceBehavior[] array = curCard.cardBehaviorQueue?.ToArray();
                if (array != null && array.Length != 0)
                    this.last = array[array.Length - 1];
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                BattlePlayingCardDataInUnitModel card = behavior?.card;
                if (card == null || behavior == this.last)
                    return;
                this.win = true;
                card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus()
                {
                    power = PickUpModel_BigBadWolf1.LogEmotionCardAbility_BigBadWolf1.Pow
                });
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                if (behavior != this.last)
                    return;
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Wolf_Bite");
                if (this.win)
                    this._owner.RecoverHP(PickUpModel_BigBadWolf1.LogEmotionCardAbility_BigBadWolf1.Heal);
            }
        }
    }

    public class PickUpModel_BigBadWolf2 : CreaturePickUpModel
    {
        public PickUpModel_BigBadWolf2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BigBadWolf2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BigBadWolf2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BigBadWolf2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370162), (EmotionCardAbilityBase)new PickUpModel_BigBadWolf2.LogEmotionCardAbility_BigBadWolf2(), model);
        }

        public class LogEmotionCardAbility_BigBadWolf2 : EmotionCardAbilityBase
        {
            public const float _dmgCondition = 0.25f;
            public int _accumulatedDmg;
            public bool trigger;
            public Battle.CreatureEffect.CreatureEffect aura;
            public string path = "6/BigBadWolf_Emotion_Aura";

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                if (!(this.aura != null))
                    return;
                this.DestroyAura();
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._accumulatedDmg = 0;
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (!this.trigger)
                    return;
                this.trigger = false;
                this._accumulatedDmg = 0;
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_BigBadWolf2.LogEmotionCardAbility_BigBadWolf2.BattleUnitBuf_Emotion_Wolf_Claw());
                if (this.aura == null)
                    this.aura = this.MakeEffect(this.path, target: this._owner, apply: false);
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.DestroyAura();
            }

            public override void OnDie(BattleUnitModel killer)
            {
                base.OnDie(killer);
                this.DestroyAura();
            }

            public void DestroyAura()
            {
                if (this.aura != null && this.aura.gameObject != null)
                    GameObject.Destroy(this.aura.gameObject);
                this.aura = (Battle.CreatureEffect.CreatureEffect)null;
            }

            public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
            {
                base.BeforeTakeDamage(attacker, dmg);
                if (!this._owner.passiveDetail.IsInvincible())
                    this._accumulatedDmg += dmg;
                if ((double)this._accumulatedDmg >= (double)this._owner.MaxHp * 0.25)
                    this.trigger = true;
                return false;
            }

            public class BattleUnitBuf_Emotion_Wolf_Claw : BattleUnitBuf
            {
                public const int _strMin = 2;
                public const int _strMax = 2;
                public const int _bleedMin = 1;
                public const int _bleedMax = 1;

                public override string keywordId => "Wolf_Claw";

                public static int Str => RandomUtil.Range(2, 2);

                public static int Bleed => RandomUtil.Range(1, 1);

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, PickUpModel_BigBadWolf2.LogEmotionCardAbility_BigBadWolf2.BattleUnitBuf_Emotion_Wolf_Claw.Str, owner);
                    SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Wolf_FogChange");
                }

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    base.OnSuccessAttack(behavior);
                    behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, PickUpModel_BigBadWolf2.LogEmotionCardAbility_BigBadWolf2.BattleUnitBuf_Emotion_Wolf_Claw.Bleed, this._owner);
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }

                public override bool IsTargetable() => false;

                public override bool DirectAttack() => true;
            }
        }
    }

    public class PickUpModel_BigBadWolf3 : CreaturePickUpModel
    {
        public PickUpModel_BigBadWolf3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BigBadWolf3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BigBadWolf3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BigBadWolf3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370163), (EmotionCardAbilityBase)new PickUpModel_BigBadWolf3.LogEmotionCardAbility_BigBadWolf3(), model);
        }

        public class LogEmotionCardAbility_BigBadWolf3 : EmotionCardAbilityBase
        {
            public const int _powMin = 1;
            public const int _powMax = 2;
            public const int _vulnMin = 1;
            public const int _vulnMax = 2;
            public const int _cntMax = 3;
            public BattleUnitModel target;
            public int cnt;

            public static int Pow => RandomUtil.Range(1, 2);

            public static int Vuln => RandomUtil.Range(1, 2);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.cnt = 0;
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Wolf_Filter_Eye", false, 2f);
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                this.target = (BattleUnitModel)null;
                if (behavior.Type != BehaviourType.Standby)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = PickUpModel_BigBadWolf3.LogEmotionCardAbility_BigBadWolf3.Pow
                });
                this.target = behavior.card?.target;
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                if (this.cnt >= 3)
                {
                    this.target = (BattleUnitModel)null;
                }
                else
                {
                    if (behavior.Type != BehaviourType.Standby || this.target == null)
                        return;
                    ++this.cnt;
                    this.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, PickUpModel_BigBadWolf3.LogEmotionCardAbility_BigBadWolf3.Vuln, this._owner);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Wolf_Scratch");
                    this.target = (BattleUnitModel)null;
                }
            }
        }
    }

    public class PickUpModel_Bigbird : PickUpModelBase
    {
        public PickUpModel_Bigbird()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Bigbird_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Bigbird_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370081));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370082));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370083));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Bigbird0 : CreaturePickUpModel
    {
        public PickUpModel_Bigbird0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370081),
      new LorId(LogLikeMod.ModId, 15370082),
      new LorId(LogLikeMod.ModId, 15370083)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Bigbird_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Bigbird"];
    }

    public class PickUpModel_Bigbird1 : CreaturePickUpModel
    {
        public PickUpModel_Bigbird1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Bigbird1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Bigbird1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Bigbird1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370081), (EmotionCardAbilityBase)new PickUpModel_Bigbird1.LogEmotionCardAbility_Bigbird1(), model);
        }

        public class LogEmotionCardAbility_Bigbird1 : EmotionCardAbilityBase
        {
            public const int _addedRecoverPP = 1;
            public bool _effect;

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._owner.cardSlotDetail.SetRecoverPoint(1 + this._owner.cardSlotDetail.GetRecoverPlayPoint());
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._owner.cardSlotDetail.RecoverPlayPoint(this._owner.cardSlotDetail.GetMaxPlayPoint());
                this._owner.cardSlotDetail.SetRecoverPoint(1 + this._owner.cardSlotDetail.GetRecoverPlayPoint());
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._effect)
                    return;
                this._effect = true;
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BigBird_Filter_Bg", false, 2f);
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BigBird_Filter_Fg", false, 2f);
                SoundEffectPlayer.PlaySound("Creature/Bigbird_Eyes");
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                List<BattleUnitModel> list = new List<BattleUnitModel>();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                {
                    if (alive != this._owner)
                    {
                        alive.cardSlotDetail.SetRecoverPointDefault();
                        list.Add(alive);
                    }
                }
                if (list.Count <= 0)
                    return;
                RandomUtil.SelectOne<BattleUnitModel>(list)?.cardSlotDetail.SetRecoverPoint(0);
            }
        }
    }

    public class PickUpModel_Bigbird2 : CreaturePickUpModel
    {
        public PickUpModel_Bigbird2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Bigbird2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Bigbird2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Bigbird2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370082), (EmotionCardAbilityBase)new PickUpModel_Bigbird2.LogEmotionCardAbility_Bigbird2(), model);
        }

        public class LogEmotionCardAbility_Bigbird2 : EmotionCardAbilityBase
        {
            public override bool CanForcelyAggro() => true;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Lamp", 1f, this._owner.view, this._owner.view, 3f);
                SoundEffectPlayer.PlaySound("Creature/Bigbird_Attract");
            }
        }
    }

    public class PickUpModel_Bigbird3 : CreaturePickUpModel
    {
        public PickUpModel_Bigbird3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Bigbird3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Bigbird3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Bigbird3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370083), (EmotionCardAbilityBase)new PickUpModel_Bigbird3.LogEmotionCardAbility_Bigbird3(), model);
        }

        public class LogEmotionCardAbility_Bigbird3 : EmotionCardAbilityBase
        {
            public const int _pow = 1;
            public const float _hpRate = 0.25f;
            public Battle.CreatureEffect.CreatureEffect _aura;

            public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
            {
                base.OnParryingStart(card);
                this.DestroyAura();
                BattleUnitModel target = card?.target;
                if (target == null || target.speedDiceResult[card.targetSlotOrder].value <= card.speedDiceResultValue)
                    return;
                this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_See_Red", 1f, this._owner.view, this._owner.view);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Bigbird_MouseOpen");
                this._owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.DestroyAura));
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                {
                    power = 1
                });
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || (double)target.hp > (double)target.MaxHp * 0.25)
                    return;
                target.currentDiceAction?.DestroyDice(DiceMatch.AllDice);
                target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_DiceBroken", 3f);
                target.battleCardResultLog?.SetCreatureEffectSound("Creature/Bigbird_HeadCut");
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.DestroyAura();
            }

            public void DestroyAura()
            {
                if (!(this._aura != null))
                    return;
                GameObject.Destroy(this._aura.gameObject);
                this._aura = (Battle.CreatureEffect.CreatureEffect)null;
            }
        }
    }

    public class PickUpModel_BloodBath : PickUpModelBase
    {
        public PickUpModel_BloodBath()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BloodBath_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BloodBath_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370001));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370002));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370003));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_BloodBath0 : CreaturePickUpModel
    {
        public PickUpModel_BloodBath0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370001),
      new LorId(LogLikeMod.ModId, 15370002),
      new LorId(LogLikeMod.ModId, 15370003)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_BloodBath_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_BloodBath"];
    }

    public class PickUpModel_BloodBath1 : CreaturePickUpModel
    {
        public PickUpModel_BloodBath1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BloodBath1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BloodBath1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BloodBath1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370001), (EmotionCardAbilityBase)new PickUpModel_BloodBath1.LogEmotionCardAbility_bloodbath1(), model);
        }

        public class LogEmotionCardAbility_bloodbath1 : EmotionCardAbilityBase
        {
            public Battle.CreatureEffect.CreatureEffect effect;

            public static int Pow => RandomUtil.Range(1, 2);

            public static int BrDmg => RandomUtil.Range(3, 5);

            public override void OnSelectEmotion()
            {
                this.effect = this.MakeEffect("0/BloodyBath_Blood");
                if (this.effect != null)
                    this.effect.transform.SetParent(this._owner.view.characterRotationCenter.transform.parent);
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BloodBath_Water");
                if (soundEffectPlayer == null)
                    return;
                soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
            }

            public override int GetBreakDamageReduction(BattleDiceBehavior behavior)
            {
                int brDmg = PickUpModel_BloodBath1.LogEmotionCardAbility_bloodbath1.BrDmg;
                this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, brDmg);
                return -brDmg;
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (!this.IsDefenseDice(behavior.Detail))
                    return;
                int pow = PickUpModel_BloodBath1.LogEmotionCardAbility_bloodbath1.Pow;
                this._owner.battleCardResultLog?.SetEmotionAbility(false, this._emotionCard, 1, ResultOption.Default);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = pow
                });
            }

            public override void OnLayerChanged(string layerName)
            {
                if (layerName == "Character")
                    layerName = "CharacterUI";
                if (!(this.effect != null))
                    return;
                this.effect.SetLayer(layerName);
            }
        }
    }

    public class PickUpModel_BloodBath2 : CreaturePickUpModel
    {
        public PickUpModel_BloodBath2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BloodBath2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BloodBath2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BloodBath2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370002), (EmotionCardAbilityBase)new PickUpModel_BloodBath2.LogEmotionCardAbility_bloodbath2(), model);
        }

        public class LogEmotionCardAbility_bloodbath2 : EmotionCardAbilityBase
        {
            public const float _prob = 0.2f;
            public const int _redMin = 2;
            public const int _redMax = 5;

            public static bool Prob => (double)RandomUtil.valueForProb < 0.20000000298023224;

            public static int Reduce => RandomUtil.Range(2, 5);

            public override int GetDamageReduction(BattleDiceBehavior behavior)
            {
                if (PickUpModel_BloodBath2.LogEmotionCardAbility_bloodbath2.Prob)
                {
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                    return 9999;
                }
                if (behavior.Detail != BehaviourDetail.Slash)
                    return base.GetDamageReduction(behavior);
                int reduce = PickUpModel_BloodBath2.LogEmotionCardAbility_bloodbath2.Reduce;
                this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, -reduce);
                this._owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                return reduce;
            }
        }
    }

    public class PickUpModel_BloodBath3 : CreaturePickUpModel
    {
        public PickUpModel_BloodBath3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BloodBath3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BloodBath3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BloodBath3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370003), (EmotionCardAbilityBase)new PickUpModel_BloodBath3.LogEmotionCardAbility_bloodbath3(), model);
        }

        public class LogEmotionCardAbility_bloodbath3 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 3;
            public const int _dmgMax = 10;
            public const int _maxStack = 3;
            public BattleUnitModel _target;
            public int _stack;

            public override int GetCounter() => this._stack;

            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                base.OnRollDice(behavior);
                if (!this.IsAttackDice(behavior.Detail) || this._target == null || this._target == behavior.card.target)
                    return;
                if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf)) is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf buf)
                    this._target.bufListDetail.RemoveBuf((BattleUnitBuf)buf);
                this._target = (BattleUnitModel)null;
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                if (this._target == behavior.card.target)
                {
                    ++this._stack;
                    if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf)) is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf bloodBathHandDebuf)
                        bloodBathHandDebuf.OnHit();
                    if (this._stack < 3)
                        return;
                    this.Ability();
                }
                else
                {
                    this._target = behavior.card.target;
                    this._stack = 1;
                    this._target.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_bloodbath3.BloodBath_HandDebuf());
                    if (!(this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf)) is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf bloodBathHandDebuf))
                        return;
                    bloodBathHandDebuf.OnHit();
                }
            }

            public void Ability()
            {
                if (this._target == null)
                    return;
                this._target.TakeBreakDamage(RandomUtil.Range(3, 10), DamageType.Emotion, this._owner);
                if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf)) is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf buf)
                    this._target.bufListDetail.RemoveBuf((BattleUnitBuf)buf);
                this._target.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_PaleHand_Hit", 3f);
                this._target = (BattleUnitModel)null;
                this._stack = 0;
            }

            public class BloodBath_HandDebuf : BattleUnitBuf
            {
                public override string keywordIconId => "BloodBath_Hand";

                public override string keywordId => "Bloodbath_Hands";

                public BloodBath_HandDebuf() => this.stack = 0;

                public void OnHit() => ++this.stack;
            }
        }
    }

    public class PickUpModel_Bloodytree : PickUpModelBase
    {
        public PickUpModel_Bloodytree()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Bloodytree_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Bloodytree_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370091));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370092));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370093));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Bloodytree0 : CreaturePickUpModel
    {
        public PickUpModel_Bloodytree0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370091),
      new LorId(LogLikeMod.ModId, 15370092),
      new LorId(LogLikeMod.ModId, 15370093)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_Bloodytree_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Bloodytree"];
    }

    public class PickUpModel_Bloodytree1 : CreaturePickUpModel
    {
        public PickUpModel_Bloodytree1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Bloodytree1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Bloodytree1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Bloodytree1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370091), (EmotionCardAbilityBase)new PickUpModel_Bloodytree1.LogEmotionCardAbility_Bloodytree1(), model);
        }

        public class LogEmotionCardAbility_Bloodytree1 : EmotionCardAbilityBase
        {
            public override void OnGiveDeflect(BattleDiceBehavior behavior)
            {
                int num = Mathf.RoundToInt((float)behavior.DiceResultValue * 0.5f);
                behavior.card?.target?.TakeDamage(num, DamageType.Emotion, this._owner);
                if (num > 0)
                    Singleton<StageController>.Instance.waveHistory.AddHokmaAchievementCount(num);
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("9_H/FX_IllusionCard_9_H_StickPiercing", 2f);
            }
        }
    }

    public class PickUpModel_Bloodytree2 : CreaturePickUpModel
    {
        public PickUpModel_Bloodytree2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Bloodytree2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Bloodytree2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Bloodytree2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370092), (EmotionCardAbilityBase)new PickUpModel_Bloodytree2.LogEmotionCardAbility_Bloodytree2(), model);
        }

        public class LogEmotionCardAbility_Bloodytree2 : EmotionCardAbilityBase
        {
            public const int _dmgAddMin = 2;
            public const int _dmgAddMax = 4;
            public const int _dmgRedMin = 3;
            public const int _dmgRedMax = 6;
            public const int _powAddMin = 1;
            public const int _powAddMax = 3;

            public int DmgAdd => RandomUtil.Range(2, 4);

            public int DmgRed => RandomUtil.Range(3, 6);

            public int PowAdd => RandomUtil.Range(1, 3);

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Eye", 1f, this._owner.view, this._owner.view);
                SoundEffectPlayer.PlaySound("Creature/MustSee_Wake_Storng");
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Eye", 1f, this._owner.view, this._owner.view);
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!behavior.IsParrying())
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = this.DmgAdd
                    });
                }
                else
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = -this.DmgRed
                    });
                    if (behavior.Detail != BehaviourDetail.Guard)
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        power = this.PowAdd
                    });
                }
            }
        }
    }

    public class PickUpModel_Bloodytree3 : CreaturePickUpModel
    {
        public PickUpModel_Bloodytree3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Bloodytree3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Bloodytree3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Bloodytree3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370093), (EmotionCardAbilityBase)new PickUpModel_Bloodytree3.LogEmotionCardAbility_Bloodytree3(), model);
        }

        public class LogEmotionCardAbility_Bloodytree3 : EmotionCardAbilityBase
        {
            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                if (behavior.Detail != BehaviourDetail.Guard)
                    return;
                this._owner.battleCardResultLog?.SetCreatureAbilityEffect("9/HokmaFirst_Guard", 0.8f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowWhite_NormalAtk");
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null)
                    return;
                BattlePlayingCardDataInUnitModel currentDiceAction = target.currentDiceAction;
                if (currentDiceAction != null)
                {
                    BattleDiceBehavior currentBehavior = currentDiceAction.currentBehavior;
                    if (currentBehavior != null)
                        currentBehavior.ApplyDiceStatBonus(new DiceStatBonus()
                        {
                            guardBreakMultiplier = 2
                        });
                }
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    guardBreakMultiplier = 2
                });
            }
        }
    }

    public class PickUpModel_BlueStar : PickUpModelBase
    {
        public PickUpModel_BlueStar()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BlueStar_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BlueStar_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370291));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370292));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370293));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_BlueStar0 : CreaturePickUpModel
    {
        public PickUpModel_BlueStar0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370291),
      new LorId(LogLikeMod.ModId, 15370292),
      new LorId(LogLikeMod.ModId, 15370293)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_BlueStar_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_BlueStar"];
    }

    public class PickUpModel_BlueStar1 : CreaturePickUpModel
    {
        public PickUpModel_BlueStar1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BlueStar1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BlueStar1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BlueStar1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370291), (EmotionCardAbilityBase)new PickUpModel_BlueStar1.LogEmotionCardAbility_BlueStar1(), model);
        }

        public class LogEmotionCardAbility_BlueStar1 : EmotionCardAbilityBase
        {
            public const int _id = 1100018;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._owner.allyCardDetail.AddNewCard(1100018);
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._owner.allyCardDetail.AddNewCardToDeck(1100018);
            }
        }
    }

    public class PickUpModel_BlueStar2 : CreaturePickUpModel
    {
        public PickUpModel_BlueStar2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BlueStar2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BlueStar2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BlueStar2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370292), (EmotionCardAbilityBase)new PickUpModel_BlueStar2.LogEmotionCardAbility_BlueStar2(), model);
        }

        public class LogEmotionCardAbility_BlueStar2 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 3;
            public const int _dmgMax = 7;
            public const int _str = 1;
            public bool triggered;

            public int Dmg => RandomUtil.Range(3, 7);

            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                this.triggered = false;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive.IsBreakLifeZero())
                    {
                        this.triggered = true;
                        alive.TakeDamage(this.Dmg, DamageType.Emotion, this._owner);
                        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_JudgementExplo", 1f, alive.view, alive.view, 2f);
                        SoundEffectPlayer.PlaySound("Creature/BlueStar_Suicide");
                    }
                }
                if (!this.triggered)
                    return;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                    alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
            }
        }
    }

    public class PickUpModel_BlueStar3 : CreaturePickUpModel
    {
        public PickUpModel_BlueStar3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_BlueStar3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_BlueStar3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_BlueStar3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370293), (EmotionCardAbilityBase)new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3(), model);
        }

        public class LogEmotionCardAbility_BlueStar3 : EmotionCardAbilityBase
        {
            public bool _effect;
            public SoundEffectPlayer _loop;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf());
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool());
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_bluestar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf)) == null)
                    return;
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_Voice");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if ((creatureEffect != null ? creatureEffect.gameObject.GetComponent<AutoDestruct>() : null) == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect != null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 5f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                SoundEffectPlayer.PlaySound("Creature/BlueStar_Atk");
                SingletonBehavior<BattleSoundManager>.Instance.EndBgm();
                if (this._loop == null)
                    this._loop = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BlueStar_Bgm", true, parent: SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.transform);
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.DestroyLoopSound();
            }

            public override void OnEndBattlePhase()
            {
                base.OnEndBattlePhase();
                this.DestroyLoopSound();
            }

            public void DestroyLoopSound()
            {
                if (!(this._loop != null))
                    return;
                SingletonBehavior<BattleSoundManager>.Instance.StartBgm();
                this._loop.ManualDestroy();
                this._loop = (SoundEffectPlayer)null;
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool());
            }

            public class BattleUnitBuf_Emotion_BlueStar_SoundBuf : BattleUnitBuf
            {
                public const int _bDmgMin = 2;
                public const int _bDmgMax = 4;

                public override string keywordId => "Emotion_BlueStar_SoundBuf";

                public int BDmg => RandomUtil.Range(2, 4);

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }

                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    base.BeforeRollDice(behavior);
                    if (behavior == null)
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        breakDmg = this.BDmg
                    });
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }
            }

            public class BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool : BattleUnitBuf
            {
                public const int _coolTimeMax = 3;

                public override string keywordId => "Emotion_BlueStar_SoundBuf_Cool";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 3;
                }

                public override void OnRoundEndTheLast()
                {
                    base.OnRoundEndTheLast();
                    --this.stack;
                    if (this.stack > 0)
                        return;
                    this.stack = 3;
                    this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf());
                }
            }
        }
    }

    public class PickUpModel_Butterfly : PickUpModelBase
    {
        public PickUpModel_Butterfly()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Butterfly_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Butterfly_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370321));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370322));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370323));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 4
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Butterfly0 : CreaturePickUpModel
    {
        public PickUpModel_Butterfly0()
        {
            this.level = 4;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370321),
      new LorId(LogLikeMod.ModId, 15370322),
      new LorId(LogLikeMod.ModId, 15370323)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_Butterfly_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Butterfly"];
    }

    public class PickUpModel_Butterfly1 : CreaturePickUpModel
    {
        public PickUpModel_Butterfly1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Butterfly1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Butterfly1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Butterfly1_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370321), (EmotionCardAbilityBase)new PickUpModel_Butterfly1.LogEmotionCardAbility_Butterfly1(), model);
        }

        public class LogEmotionCardAbility_Butterfly1 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 2;
            public const int _dmgMax = 7;
            public const int _bDmgMin = 2;
            public const int _bDmgMax = 5;
            public bool trigger;

            public static int Dmg => UnityEngine.Random.Range(2, 7);

            public static int BDmg => UnityEngine.Random.Range(2, 5);

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                this.trigger = false;
                if (!this.IsAttackDice(behavior.Detail))
                    return;
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || !this.IsAttackDice(behavior.Detail))
                    return;
                float num = -1f;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                {
                    if ((double)alive.hp > 0.0 && ((double)num < 0.0 || (double)alive.hp < (double)num))
                        num = alive.hp;
                }
                if ((double)target.hp <= (double)num)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = PickUpModel_Butterfly1.LogEmotionCardAbility_Butterfly1.Dmg,
                        breakDmg = PickUpModel_Butterfly1.LogEmotionCardAbility_Butterfly1.BDmg
                    });
                    this.trigger = true;
                }
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                if (!this.trigger)
                    return;
                behavior?.card?.target?.battleCardResultLog?.SetCreatureAbilityEffect("2/ButterflyEffect_Black", 1f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Atk_Black");
                this.trigger = false;
            }
        }
    }

    public class PickUpModel_Butterfly2 : CreaturePickUpModel
    {
        public PickUpModel_Butterfly2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Butterfly2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Butterfly2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Butterfly2_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370322), (EmotionCardAbilityBase)new PickUpModel_Butterfly2.LogEmotionCardAbility_Butterfly2(), model);
        }

        public class LogEmotionCardAbility_Butterfly2 : EmotionCardAbilityBase
        {
            public const int _strMin = 1;
            public const int _strMax = 2;

            public int Str => RandomUtil.Range(1, 2);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                    alive.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_Butterfly2.LogEmotionCardAbility_Butterfly2.BattleUnitBuf_Emotion_Butterfly_DmgByDebuf());
                if (this._owner.bufListDetail.GetNegativeBufTypeCount() <= 0 && this._owner.bufListDetail.GetReadyBufList().Find((Predicate<BattleUnitBuf>)(x => x.positiveType == BufPositiveType.Negative)) == null)
                    return;
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Fly", 1f, this._owner.view, this._owner.view, 2f);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this.Str, this._owner);
            }

            public class BattleUnitBuf_Emotion_Butterfly_DmgByDebuf : BattleUnitBuf
            {
                public const int _dmgAddMin = 2;
                public const int _dmgAddMax = 4;

                public int DmgAdd => RandomUtil.Range(2, 4);

                public override bool Hide => true;

                public override int GetDamageReduction(BattleDiceBehavior behavior)
                {
                    return this._owner.bufListDetail.GetNegativeBufTypeCount() > 0 ? -this.DmgAdd : base.GetDamageReduction(behavior);
                }

                public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
                {
                    base.BeforeTakeDamage(attacker, dmg);
                    if (this._owner.bufListDetail.GetNegativeBufTypeCount() <= 0)
                        return;
                    if (Singleton<StageController>.Instance.IsLogState())
                    {
                        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("2/ButterflyEffect_White", 1f);
                        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Atk_White");
                    }
                    else
                    {
                        SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("2/ButterflyEffect_White", 1f, this._owner.view, this._owner.view, 1f);
                        SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/ButterFlyMan_Atk_White");
                    }
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_Butterfly3 : CreaturePickUpModel
    {
        public PickUpModel_Butterfly3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Butterfly3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Butterfly3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Butterfly3_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370323), (EmotionCardAbilityBase)new PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3(), model);
        }

        public class LogEmotionCardAbility_Butterfly3 : EmotionCardAbilityBase
        {
            public const float _hpRate = 0.5f;
            public const int _cntMax = 2;
            public int cnt;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.cnt = 0;
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || (double)target.hp > (double)target.MaxHp * 0.5)
                    return;
                if (!behavior.card.card.XmlData.IsFloorEgo())
                {
                    if (behavior.card.card.XmlData.Spec.Ranged == CardRange.Near)
                        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("2/Butterfly_Emotion_Effect_Spread_Near", 1f);
                    else
                        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("2/Butterfly_Emotion_Effect_Spread", 1f);
                }
                if (this.cnt < 2)
                {
                    ++this.cnt;
                    BattleUnitBuf battleUnitBuf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3.BattleUnitBuf_Butterfly_Emotion_Seal));
                    if (battleUnitBuf == null)
                    {
                        PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3.BattleUnitBuf_Butterfly_Emotion_Seal buf = new PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3.BattleUnitBuf_Butterfly_Emotion_Seal();
                        target.bufListDetail.AddBuf((BattleUnitBuf)buf);
                        buf.Add();
                    }
                    else
                    {
                        if (!(battleUnitBuf is PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3.BattleUnitBuf_Butterfly_Emotion_Seal butterflyEmotionSeal))
                            return;
                        butterflyEmotionSeal.Add();
                    }
                }
            }

            public class BattleUnitBuf_Butterfly_Emotion_Seal : BattleUnitBuf
            {
                public int addedThisTurn;
                public int deleteThisTurn;

                public override string keywordId => "Butterfly_Seal";

                public BattleUnitBuf_Butterfly_Emotion_Seal() => this.stack = 0;

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.stack -= this.deleteThisTurn;
                    if (this.stack > 0)
                        return;
                    this.Destroy();
                }

                public override int SpeedDiceBreakedAdder() => this.stack;

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    this.deleteThisTurn = this.addedThisTurn;
                    this.addedThisTurn = 0;
                }

                public void Add()
                {
                    ++this.stack;
                    ++this.addedThisTurn;
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Lock");
                    int index = this._owner.passiveDetail.SpeedDiceNumAdder() - this._owner.passiveDetail.SpeedDiceBreakAdder() - this._owner.bufListDetail.SpeedDiceBreakAdder() + this.addedThisTurn - 1;
                    if (index < 0 || index >= this._owner.speedDiceResult.Count || this._owner.cardSlotDetail.cardAry[index].GetRemainingAbilityCount() <= 0)
                        return;
                    this._owner.cardSlotDetail.cardAry[index].DestroyDice(DiceMatch.AllDice);
                }
            }
        }
    }

    public class PickUpModel_ChildofGalaxy : PickUpModelBase
    {
        public PickUpModel_ChildofGalaxy()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ChildofGalaxy_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ChildofGalaxy_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370141));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370142));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370143));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_ChildofGalaxy0 : CreaturePickUpModel
    {
        public PickUpModel_ChildofGalaxy0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370141),
      new LorId(LogLikeMod.ModId, 15370142),
      new LorId(LogLikeMod.ModId, 15370143)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_ChildofGalaxy_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_ChildofGalaxy"];
    }

    public class PickUpModel_ChildofGalaxy1 : CreaturePickUpModel
    {
        public PickUpModel_ChildofGalaxy1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ChildofGalaxy1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ChildofGalaxy1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ChildofGalaxy1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370141), (EmotionCardAbilityBase)new PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1(), model);
        }

        public class LogEmotionCardAbility_ChildofGalaxy1 : EmotionCardAbilityBase
        {
            public const int _targetNum = 3;
            public const int _recoverMin = 3;
            public const int _recoverMax = 7;
            public const int _loseMin = 1;
            public const int _loseMax = 3;
            public const string _icon = "GalaxyBoy_Stone";

            public static int Recover => RandomUtil.Range(3, 7);

            public static int Lose => RandomUtil.Range(1, 3);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
                List<BattleDiceCardModel> hand = this._owner.allyCardDetail.GetHand();
                hand.RemoveAll((Predicate<BattleDiceCardModel>)(x => x.GetSpec().Ranged == CardRange.Instance));
                if (hand.Count <= 3)
                {
                    battleDiceCardModelList.AddRange((IEnumerable<BattleDiceCardModel>)hand);
                }
                else
                {
                    for (int index = 0; index < 3 && hand.Count > 0; ++index)
                    {
                        BattleDiceCardModel battleDiceCardModel = RandomUtil.SelectOne<BattleDiceCardModel>(hand);
                        hand.Remove(battleDiceCardModel);
                        battleDiceCardModelList.Add(battleDiceCardModel);
                    }
                }
                if (battleDiceCardModelList.Count <= 0)
                    return;
                foreach (BattleDiceCardModel battleDiceCardModel in battleDiceCardModelList)
                {
                    battleDiceCardModel.AddBuf((BattleDiceCardBuf)new PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.GalaxyChildPebbleBuf());
                    battleDiceCardModel.SetAddedIcon("GalaxyBoy_Stone");
                }
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
                {
                    BattleDiceCardBuf buf = battleDiceCardModel.GetBufList().Find((Predicate<BattleDiceCardBuf>)(x => x is PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.GalaxyChildPebbleBuf));
                    if (buf != null)
                    {
                        battleDiceCardModel.RemoveBuf(buf);
                        battleDiceCardModel.RemoveAddedIcon("GalaxyBoy_Stone");
                    }
                }
            }

            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnUseCard(curCard);
                if (curCard.card.GetBufList().FindAll((Predicate<BattleDiceCardBuf>)(x => x is PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.GalaxyChildPebbleBuf)).Count > 0)
                {
                    this._owner.RecoverHP(PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.Recover);
                    if (!Singleton<StageController>.Instance.IsLogState())
                    {
                        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_O", 1f, this._owner.view, this._owner.view, 2f);
                        SoundEffectPlayer.PlaySound("Creature/GalaxyBoy_Heal");
                    }
                    else
                    {
                        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_O", 2f);
                        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Heal");
                    }
                }
                else
                {
                    this._owner.LoseHp(PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.Lose);
                    if (!Singleton<StageController>.Instance.IsLogState())
                    {
                        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_X", 1f, this._owner.view, this._owner.view, 2f);
                        SoundEffectPlayer.PlaySound("Creature/GalaxyBoy_Deal");
                    }
                    else
                    {
                        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_X", 2f);
                        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Deal");
                    }
                }
            }

            public class GalaxyChildPebbleBuf : BattleDiceCardBuf
            {
                public override string keywordIconId => "GalaxyBoy_Stone";
            }
        }
    }

    public class PickUpModel_ChildofGalaxy2 : CreaturePickUpModel
    {
        public PickUpModel_ChildofGalaxy2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ChildofGalaxy2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ChildofGalaxy2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ChildofGalaxy2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370142), (EmotionCardAbilityBase)new PickUpModel_ChildofGalaxy2.LogEmotionCardAbility_ChildofGalaxy2(), model);
        }

        public class LogEmotionCardAbility_ChildofGalaxy2 : EmotionCardAbilityBase
        {
            public Battle.CreatureEffect.CreatureEffect _effect;
            public List<Battle.CreatureEffect.CreatureEffect> _damagedEffects = new List<Battle.CreatureEffect.CreatureEffect>();

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = 1
                });
            }

            public override void OnDie(BattleUnitModel killer)
            {
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive != this._owner)
                    {
                        float hp = alive.hp;
                        int v = Mathf.Min(this._owner.MaxHp / 2, 60);
                        int damage = alive.TakeDamage(v, DamageType.Emotion, this._owner);
                        alive.view.PrintBloodSprites(damage, hp);
                        Battle.CreatureEffect.CreatureEffect creatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("4/GalaxyBoy_Damaged", 1f, alive.view, (BattleUnitView)null, 2f);
                        creatureEffect.SetLayer("Character");
                        this._damagedEffects.Add(creatureEffect);
                        creatureEffect.gameObject.SetActive(false);
                        creatureEffect.SetLayer("Effect");
                        alive.view.Damaged(damage, BehaviourDetail.None, 0, (BattleUnitModel)null);
                    }
                }
                this._effect = this.MakeEffect("4/GalaxyBoy_Dust", destroyTime: 3f);
                Battle.CreatureEffect.CreatureEffect effect1 = this._effect;
                if (effect1 != null)
                    effect1.gameObject.SetActive(false);
                Battle.CreatureEffect.CreatureEffect effect2 = this._effect;
                if (effect2 != null)
                    effect2.AttachEffectLayer();
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Cry");
            }

            public override void OnPrintEffect(BattleDiceBehavior behavior)
            {
                if (!(bool)this._effect)
                    return;
                this._effect.gameObject.SetActive(true);
                this._effect = (Battle.CreatureEffect.CreatureEffect)null;
                foreach (Component damagedEffect in this._damagedEffects)
                    damagedEffect.gameObject.SetActive(true);
                this._damagedEffects.Clear();
            }
        }
    }

    public class PickUpModel_ChildofGalaxy3 : CreaturePickUpModel
    {
        public PickUpModel_ChildofGalaxy3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ChildofGalaxy3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ChildofGalaxy3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ChildofGalaxy3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370143), (EmotionCardAbilityBase)new PickUpModel_ChildofGalaxy3.LogEmotionCardAbility_ChildofGalaxy3(), model);
        }

        public class LogEmotionCardAbility_ChildofGalaxy3 : EmotionCardAbilityBase
        {
            public int _roundCount;

            public override void OnRoundStart()
            {
                if (this._roundCount >= 3)
                    return;
                ++this._roundCount;
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_galaxyChild3.BattleUnitBuf_galaxyChild_Friend());
                int v = Mathf.Min(this._owner.MaxHp / 10, 12);
                this._owner.RecoverHP(v);
                this._owner.ShowTypoTemporary(this._emotionCard, 0, ResultOption.Default, v);
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_O", 1f, this._owner.view, this._owner.view, 2f);
            }

            public override void OnSelectEmotion() => this._owner.view.unitBottomStatUI.SetBufs();

            public class BattleUnitBuf_galaxyChild_Friend : BattleUnitBuf
            {
                public override string keywordId => "GalaxyBoy_Stone";

                public override string keywordIconId => "GalaxyBoy_Stone";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_Clock : PickUpModelBase
    {
        public PickUpModel_Clock()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Clock_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Clock_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370191));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370192));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370193));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Clock0 : CreaturePickUpModel
    {
        public PickUpModel_Clock0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370191),
      new LorId(LogLikeMod.ModId, 15370192),
      new LorId(LogLikeMod.ModId, 15370193)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Clock_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Clock"];
    }

    public class PickUpModel_Clock1 : CreaturePickUpModel
    {
        public PickUpModel_Clock1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Clock1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Clock1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Clock1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370191), (EmotionCardAbilityBase)new PickUpModel_Clock1.LogEmotionCardAbility_Clock1(), model);
        }

        public class LogEmotionCardAbility_Clock1 : EmotionCardAbilityBase
        {
            public const float _START_BATTLE_TIME = 30f;
            public const int _powMin = 1;
            public const int _powMax = 2;
            public bool rolled;
            public float _elapsed;
            public bool _bTimeLimitOvered;
            public Silence_Emotion_Clock _clock;

            public static int Pow => RandomUtil.Range(1, 2);

            public Silence_Emotion_Clock Clock
            {
                get
                {
                    if (this._clock == null)
                        this._clock = SingletonBehavior<BattleManagerUI>.Instance.EffectLayer.GetComponentInChildren<Silence_Emotion_Clock>();
                    if (this._clock == null)
                    {
                        Silence_Emotion_Clock original = Resources.Load<Silence_Emotion_Clock>("Prefabs/Battle/CreatureEffect/8/Silence_Emotion_Clock");
                        if (original != null)
                        {
                            Silence_Emotion_Clock silenceEmotionClock = GameObject.Instantiate<Silence_Emotion_Clock>(original);
                            silenceEmotionClock.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                            silenceEmotionClock.gameObject.transform.localPosition = new Vector3(0.0f, 800f, 0.0f);
                            silenceEmotionClock.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                        this._clock = original;
                    }
                    return this._clock;
                }
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this.Init();
            }

            public void Init()
            {
                this._elapsed = 0.0f;
                this._bTimeLimitOvered = false;
                this.rolled = false;
            }

            public override void OnFixedUpdateInWaitPhase(float delta)
            {
                base.OnFixedUpdateInWaitPhase(delta);
                if (!this.rolled || this._bTimeLimitOvered)
                    return;
                Silence_Emotion_Clock clock = this.Clock;
                if (clock != null)
                    clock.Run(this._elapsed);
                this._elapsed += delta;
                if ((double)this._elapsed >= 30.0 && !SingletonBehavior<BattleCamManager>.Instance.IsCamIsReturning)
                {
                    Singleton<StageController>.Instance.CompleteApplyingLibrarianCardPhase(true);
                    this._bTimeLimitOvered = true;
                    this._elapsed = 0.0f;
                }
            }

            public override void OnAfterRollSpeedDice()
            {
                base.OnAfterRollSpeedDice();
                this.Init();
                this.rolled = true;
                Silence_Emotion_Clock clock = this.Clock;
                if (clock == null)
                    return;
                clock.OnStartRollSpeedDice();
            }

            public override void OnStartBattle()
            {
                base.OnStartBattle();
                Silence_Emotion_Clock clock = this.Clock;
                if (clock == null)
                    return;
                clock.OnStartUnitMoving();
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.rolled = false;
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = PickUpModel_Clock1.LogEmotionCardAbility_Clock1.Pow
                });
            }
        }
    }

    public class PickUpModel_Clock2 : CreaturePickUpModel
    {
        public PickUpModel_Clock2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Clock2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Clock2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Clock2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370192), (EmotionCardAbilityBase)new PickUpModel_Clock2.LogEmotionCardAbility_Clock2(), model);
        }

        public class LogEmotionCardAbility_Clock2 : EmotionCardAbilityBase
        {
            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this.GiveBuf();
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this.GiveBuf();
            }

            public void GiveBuf()
            {
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_Clock2.LogEmotionCardAbility_Clock2.BattleUnitBuf_Emotion_Silence_Bell)) != null)
                    return;
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_Clock2.LogEmotionCardAbility_Clock2.BattleUnitBuf_Emotion_Silence_Bell());
            }

            public class BattleUnitBuf_Emotion_Silence_Bell : BattleUnitBuf
            {
                public const int _stackMax = 13;
                public const int _powMin = 1;
                public const int _powMax = 2;
                public bool triggered;

                public override string keywordId => "Clock_Thirteen";

                public override string keywordIconId => "Silence_Emotion_Bell";

                public static int Pow => RandomUtil.Range(1, 2);

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }

                public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
                {
                    base.OnUseCard(card);
                    this.triggered = false;
                    ++this.stack;
                    if (this.stack < 13)
                        return;
                    this.stack = 0;
                    this.triggered = true;
                    this._owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.ResetTrigger));
                }

                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    base.BeforeRollDice(behavior);
                    if (!this.triggered || behavior == null)
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        power = PickUpModel_Clock2.LogEmotionCardAbility_Clock2.BattleUnitBuf_Emotion_Silence_Bell.Pow
                    });
                }

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    base.OnSuccessAttack(behavior);
                    if (!this.triggered)
                        return;
                    BattleUnitModel target = behavior?.card?.target;
                    if (target != null)
                    {
                        int v = Mathf.RoundToInt((float)(target.MaxHp * behavior.DiceResultValue) * 0.01f);
                        target.TakeDamage(v, DamageType.Card_Ability, this._owner);
                        this._owner.battleCardResultLog?.SetSucceedAtkEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Groggy));
                    }
                }

                public void ResetTrigger() => this.triggered = false;

                public void Groggy()
                {
                    BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
                    Camera effectCam = instance != null ? instance.EffectCam : (Camera)null;
                    if (effectCam.GetComponent<CameraFilterPack_Broken_Screen>() == null)
                    {
                        CameraFilterPack_Broken_Screen r = effectCam.gameObject.AddComponent<CameraFilterPack_Broken_Screen>();
                        AutoScriptDestruct autoScriptDestruct = effectCam.gameObject.AddComponent<AutoScriptDestruct>();
                        autoScriptDestruct.targetScript = (MonoBehaviour)r;
                        autoScriptDestruct.time = 1f;
                        autoScriptDestruct.StartCoroutine(this.BrokenRoutine(r));
                    }
                    SoundEffectPlayer.PlaySound("Creature/Clock_NoCreate");
                }

                public IEnumerator BrokenRoutine(CameraFilterPack_Broken_Screen r)
                {
                    float elapsed = 0.0f;
                    while ((double)elapsed < 1.0)
                    {
                        elapsed += Time.deltaTime * 2f;
                        if (r != null)
                            r.Fade = (float)(0.75 - (double)elapsed * 0.75);
                        yield return YieldCache.waitFixedUpdate;
                    }
                }
            }
        }
    }

    public class PickUpModel_Clock3 : CreaturePickUpModel
    {
        public PickUpModel_Clock3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Clock3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Clock3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Clock3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370193), (EmotionCardAbilityBase)new PickUpModel_Clock3.LogEmotionCardAbility_Clock3(), model);
        }

        public class LogEmotionCardAbility_Clock3 : EmotionCardAbilityBase
        {
            public const int _id1 = 1100012;
            public const int _id2 = 1100013;

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this.GiveCardsToDeck();
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this.GiveCards();
            }

            public void GiveCards()
            {
                this._owner.allyCardDetail.AddNewCard(1100012);
                this._owner.allyCardDetail.AddNewCard(1100013);
            }

            public void GiveCardsToDeck()
            {
                this._owner.allyCardDetail.AddNewCardToDeck(1100012);
                this._owner.allyCardDetail.AddNewCardToDeck(1100013);
            }
        }
    }

    public class PickUpModel_FairyCarnival : PickUpModelBase
    {
        public PickUpModel_FairyCarnival()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_FairyCarnival_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_FairyCarnival_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370211));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370212));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370213));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_FairyCarnival0 : CreaturePickUpModel
    {
        public PickUpModel_FairyCarnival0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370211),
      new LorId(LogLikeMod.ModId, 15370212),
      new LorId(LogLikeMod.ModId, 15370213)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_FairyCarnival_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_FairyCarnival"];
    }

    public class PickUpModel_FairyCarnival1 : CreaturePickUpModel
    {
        public PickUpModel_FairyCarnival1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_FairyCarnival1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_FairyCarnival1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_FairyCarnival1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370211), (EmotionCardAbilityBase)new PickUpModel_FairyCarnival1.LogEmotionCardAbility_FairyCarnival1(), model);
        }

        public class LogEmotionCardAbility_FairyCarnival1 : EmotionCardAbilityBase
        {
            public const int _maxHeal = 18;
            public const float _healRate = 0.15f;
            public int _count;
            public int _takeDamageCount;
            public CreatureEffect_Anim _effect;
            public bool _hit;
            public bool _destroy;
            public int _hitCount;

            public override void OnSelectEmotion()
            {
                try
                {
                    this._effect = this.MakeEffect("1/Fairy_Gluttony") as CreatureEffect_Anim;
                    CreatureEffect_Anim effect = this._effect;
                    if (effect != null)
                        effect.SetLayer("Character");
                    SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Fariy_Special");
                    if (!(soundEffectPlayer != null))
                        return;
                    soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

            public override void OnRoundEnd()
            {
                if (this._count < 3)
                {
                    int num = Mathf.Min((int)((double)this._owner.MaxHp * 0.15000000596046448), 18);
                    this._owner.RecoverHP(num);
                    ++this._count;
                    if (this._effect != null)
                        this._effect.SetTrigger("Recover");
                    this._owner.view.RecoverHp(num);
                }
                if (this._count < 3)
                    return;
                if (this._effect != null)
                {
                    this._effect.SetTrigger("Disappear");
                    this._effect.gameObject.AddComponent<AutoDestruct>().time = 2f;
                }
                this._effect = (CreatureEffect_Anim)null;
            }

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                if (this._takeDamageCount >= 3 || this._count >= 3)
                    return;
                ++this._takeDamageCount;
                if (this._takeDamageCount >= 3)
                {
                    this._count = 5;
                    int dmg1 = this._owner.LoseHp(Mathf.Min((int)this._owner.hp * 25 / 100, 30));
                    this._owner.battleCardResultLog?.SetDamageTaken(dmg1, atkDice.behaviourInCard.Dice, atkDice.Detail);
                    this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 1, ResultOption.Default, dmg1);
                    this._destroy = true;
                    if ((bool)this._effect)
                        this.ApplyCreatureEffect((Battle.CreatureEffect.CreatureEffect)this._effect);
                }
                else
                {
                    this._hit = true;
                    ++this._hitCount;
                    if ((bool)this._effect)
                        this.ApplyCreatureEffect((Battle.CreatureEffect.CreatureEffect)this._effect);
                }
                this._owner?.battleCardResultLog?.SetCreatureEffectSound("Creature/Fairy_Dead");
            }

            public override void OnPrintEffect(BattleDiceBehavior behavior)
            {
                if (this._hit)
                {
                    if (this._effect != null)
                        this._effect.SetTrigger("Hit");
                    --this._hitCount;
                    if (this._hitCount == 0)
                        this._hit = false;
                }
                if (!this._destroy)
                    return;
                this._destroy = false;
                if (this._effect != null)
                    this._effect.SetTrigger("Disappear");
                this._effect = (CreatureEffect_Anim)null;
            }

            public override void OnLayerChanged(string layerName)
            {
                if (!(this._effect == null))
                    return;
                this._effect.SetLayer(layerName);
            }
        }
    }

    public class PickUpModel_FairyCarnival2 : CreaturePickUpModel
    {
        public PickUpModel_FairyCarnival2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_FairyCarnival2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_FairyCarnival2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_FairyCarnival2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370212), (EmotionCardAbilityBase)new PickUpModel_FairyCarnival2.LogEmotionCardAbility_FairyCarnival2(), model);
        }

        public class LogEmotionCardAbility_FairyCarnival2 : EmotionCardAbilityBase
        {
            public const int _addMin = 1;
            public const int _addMax = 3;
            public const int _recoverMin = 2;
            public const int _recoverMax = 5;

            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                BattleUnitModel target = behavior.card.target;
                if (target == null || target.history.takeDamageAtOneRound <= 0)
                    return;
                int num = RandomUtil.Range(1, 3);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = num
                });
                target.battleCardResultLog?.SetCreatureAbilityEffect("1/Fairy_Gluttony2");
                this._owner.RecoverHP(RandomUtil.Range(2, 5));
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Fairy_Dead");
            }
        }
    }

    public class PickUpModel_FairyCarnival3 : CreaturePickUpModel
    {
        public PickUpModel_FairyCarnival3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_FairyCarnival3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_FairyCarnival3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_FairyCarnival3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370213), (EmotionCardAbilityBase)new PickUpModel_FairyCarnival3.LogEmotionCardAbility_FairyCarnival3(), model);
        }

        public class LogEmotionCardAbility_FairyCarnival3 : EmotionCardAbilityBase
        {
            public const int _loseHp = 10;
            public const int _pow = 3;
            public const int _haste = 3;
            public int lostHp;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this.lostHp = 0;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                {
                    if (alive != this._owner)
                    {
                        int num = alive.LoseHp(10);
                        if (num > 0)
                            this.lostHp += num;
                    }
                }
                if (this.lostHp > 0)
                    this._owner.RecoverHP(this.lostHp);
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 3, this._owner);
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 3, this._owner);
                SoundEffectPlayer.PlaySound("Creature/Fairy_QueenEat");
                SoundEffectPlayer.PlaySound("Creature/Fairy_QueenChange");
                this.SetFilter("1/Fairy_Filter");
            }
        }
    }

    public class PickUpModel_ForsakenMurderer : PickUpModelBase
    {
        public PickUpModel_ForsakenMurderer()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ForsakenMurderer_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ForsakenMurderer_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370021));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370022));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370023));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_ForsakenMurderer0 : CreaturePickUpModel
    {
        public PickUpModel_ForsakenMurderer0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370021),
      new LorId(LogLikeMod.ModId, 15370022),
      new LorId(LogLikeMod.ModId, 15370023)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_ForsakenMurderer_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_ForsakenMurderer"];
    }

    public class PickUpModel_ForsakenMurderer1 : CreaturePickUpModel
    {
        public PickUpModel_ForsakenMurderer1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ForsakenMurderer1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ForsakenMurderer1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ForsakenMurderer1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370021), (EmotionCardAbilityBase)new PickUpModel_ForsakenMurderer1.LogEmotionCardAbility_ForsakenMurderer1(), model);
        }

        public class LogEmotionCardAbility_ForsakenMurderer1 : EmotionCardAbilityBase
        {
            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                if (behavior.Detail != BehaviourDetail.Hit)
                    return;
                BattleUnitModel target = behavior.card?.target;
                if (target == null)
                    return;
                target.bufListDetail?.AddKeywordBufByEtc(KeywordBuf.Binding, 1, this._owner);
                target.bufListDetail?.AddKeywordBufByEtc(KeywordBuf.Paralysis, 1, this._owner);
                target.battleCardResultLog?.SetCreatureAbilityEffect("2/AbandonedMurder_Hit", 1f);
                target.battleCardResultLog?.SetCreatureEffectSound("Creature/Abandoned_Atk");
            }
        }
    }

    public class PickUpModel_ForsakenMurderer2 : CreaturePickUpModel
    {
        public PickUpModel_ForsakenMurderer2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ForsakenMurderer2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ForsakenMurderer2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ForsakenMurderer2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370022), (EmotionCardAbilityBase)new PickUpModel_ForsakenMurderer2.LogEmotionCardAbility_ForsakenMurderer2(), model);
        }

        public class LogEmotionCardAbility_ForsakenMurderer2 : EmotionCardAbilityBase
        {
            public override int GetSpeedDiceAdder() => -100;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (behavior.Detail != BehaviourDetail.Hit)
                    return;
                int num = RandomUtil.Range(1, 3);
                this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, num);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num
                });
            }

            public override void OnSelectEmotion()
            {
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Abandoned_Breathe");
                if (soundEffectPlayer != null)
                    soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                this._owner.view.charAppearance.SetTemporaryGift("Gift_AbandonedMurder", GiftPosition.Mouth);
            }
        }
    }

    public class PickUpModel_ForsakenMurderer3 : CreaturePickUpModel
    {
        public PickUpModel_ForsakenMurderer3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ForsakenMurderer3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ForsakenMurderer3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ForsakenMurderer3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370023), (EmotionCardAbilityBase)new PickUpModel_ForsakenMurderer3.LogEmotionCardAbility_ForsakenMurderer3(), model);
        }

        public class LogEmotionCardAbility_ForsakenMurderer3 : EmotionCardAbilityBase
        {
            public const int _min = -1;
            public const int _max = 3;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    min = -1,
                    max = 3
                });
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Murderer_Filter", false, 2f);
                SoundEffectPlayer.PlaySound("Creature/Abandoned_Angry");
            }
        }
    }

    public class PickUpModel_Greed : PickUpModelBase
    {
        public PickUpModel_Greed()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Greed_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Greed_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370251));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370252));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370253));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Greed0 : CreaturePickUpModel
    {
        public PickUpModel_Greed0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370251),
      new LorId(LogLikeMod.ModId, 15370252),
      new LorId(LogLikeMod.ModId, 15370253)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Greed_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Greed"];
    }

    public class PickUpModel_Greed1 : CreaturePickUpModel
    {
        public PickUpModel_Greed1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Greed1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Greed1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Greed1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370251), (EmotionCardAbilityBase)new PickUpModel_Greed1.LogEmotionCardAbility_Greed1(), model);
        }

        public class LogEmotionCardAbility_Greed1 : EmotionCardAbilityBase
        {
            public const float _prob = 0.25f;

            public static bool Prob => (double)RandomUtil.valueForProb < 0.25;

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || behavior == null || behavior.Index != 0 || !PickUpModel_Greed1.LogEmotionCardAbility_Greed1.Prob)
                    return;
                target.currentDiceAction?.DestroyDice(DiceMatch.AllDice);
                this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
            }

            public void Filter()
            {
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false);
                SoundEffectPlayer.PlaySound("Creature/Greed_StrongAtk_Defensed");
            }
        }
    }

    public class PickUpModel_Greed2 : CreaturePickUpModel
    {
        public PickUpModel_Greed2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Greed2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Greed2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Greed2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370252), (EmotionCardAbilityBase)new PickUpModel_Greed2.LogEmotionCardAbility_Greed2(), model);
        }

        public class LogEmotionCardAbility_Greed2 : EmotionCardAbilityBase
        {
            public const int _stackMax = 3;
            public int count;
            public Battle.CreatureEffect.CreatureEffect aura;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false, 2f);
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.DestroyAura();
                if (this.count > 0)
                {
                    this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Happiness", 1f, this._owner.view, this._owner.view);
                    SoundEffectPlayer.PlaySound("Creature/Greed_MakeDiamond");
                }
                this.count = 0;
            }

            public override void OnDie(BattleUnitModel killer)
            {
                base.OnDie(killer);
                this.DestroyAura();
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.DestroyAura();
                if (this.count <= 0)
                    return;
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, this.GetStack(this.count), this._owner);
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                ++this.count;
            }

            public void DestroyAura()
            {
                if (!(this.aura != null))
                    return;
                GameObject.Destroy(this.aura.gameObject);
                this.aura = (Battle.CreatureEffect.CreatureEffect)null;
            }

            public int GetStack(int cnt) => Mathf.Min(3, cnt);
        }
    }

    public class PickUpModel_Greed3 : CreaturePickUpModel
    {
        public PickUpModel_Greed3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Greed3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Greed3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Greed3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370253), (EmotionCardAbilityBase)new PickUpModel_Greed3.LogEmotionCardAbility_Greed3(), model);
        }

        public class LogEmotionCardAbility_Greed3 : EmotionCardAbilityBase
        {
            public const int _condition = 4;
            public const int _dmgMin = 3;
            public const int _dmgMax = 7;
            public const int _healMin = 2;
            public const int _healMax = 5;

            public static int Dmg => RandomUtil.Range(3, 7);

            public static int Heal => RandomUtil.Range(2, 5);

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null)
                    return;
                try
                {
                    int diceResultValue1 = behavior.DiceResultValue;
                    BattleDiceBehavior targetDice = behavior.TargetDice;
                    int num1 = diceResultValue1;
                    int? diceResultValue2 = targetDice?.DiceResultValue;
                    int? nullable = diceResultValue2.HasValue ? new int?(num1 - diceResultValue2.GetValueOrDefault()) : new int?();
                    int num2 = 4;
                    if (!(nullable.GetValueOrDefault() >= num2 & nullable.HasValue))
                        return;
                    target.TakeDamage(PickUpModel_Greed3.LogEmotionCardAbility_Greed3.Dmg, DamageType.Emotion, this._owner);
                    this._owner.RecoverHP(PickUpModel_Greed3.LogEmotionCardAbility_Greed3.Heal);
                    this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
                    target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
                    target.battleCardResultLog?.SetCreatureEffectSound("Creature/Greed_Vert_Change");
                }
                catch
                {
                }
            }

            public override void OnLoseParrying(BattleDiceBehavior behavior)
            {
                base.OnLoseParrying(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null)
                    return;
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null)
                    return;
                try
                {
                    int? diceResultValue1 = behavior.TargetDice?.DiceResultValue;
                    int diceResultValue2 = behavior.DiceResultValue;
                    int? nullable = diceResultValue1.HasValue ? new int?(diceResultValue1.GetValueOrDefault() - diceResultValue2) : new int?();
                    int num = 4;
                    if (!(nullable.GetValueOrDefault() >= num & nullable.HasValue))
                        return;
                    this._owner.TakeDamage(PickUpModel_Greed3.LogEmotionCardAbility_Greed3.Dmg, DamageType.Emotion, this._owner);
                    target.RecoverHP(PickUpModel_Greed3.LogEmotionCardAbility_Greed3.Heal);
                    this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
                    target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Greed_Vert_Change");
                }
                catch
                {
                }
            }
        }
    }

    public class PickUpModel_HappyTeddyBear : PickUpModelBase
    {
        public PickUpModel_HappyTeddyBear()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_HappyTeddyBear_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_HappyTeddyBear_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370111));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370112));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370113));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_HappyTeddyBear0 : CreaturePickUpModel
    {
        public PickUpModel_HappyTeddyBear0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370111),
      new LorId(LogLikeMod.ModId, 15370112),
      new LorId(LogLikeMod.ModId, 15370113)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_HappyTeddyBear_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_HappyTeddyBear"];
    }

    public class PickUpModel_HappyTeddyBear1 : CreaturePickUpModel
    {
        public PickUpModel_HappyTeddyBear1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_HappyTeddyBear1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_HappyTeddyBear1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_HappyTeddyBear1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370111), (EmotionCardAbilityBase)new PickUpModel_HappyTeddyBear1.LogEmotionCardAbility_HappyTeddyBear1(), model);
        }

        public class LogEmotionCardAbility_HappyTeddyBear1 : EmotionCardAbilityBase
        {
            public const float _defaultProb = 0.2f;
            public const float _probPerCnt = 0.1f;
            public BattleUnitModel _lastTarget;
            public int _parryingCount;

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                if ((double)RandomUtil.valueForProb >= 0.20000000298023224 + (double)this._parryingCount * 0.10000000149011612)
                    return;
                BattleUnitModel target = behavior?.card?.target;
                if (target != null)
                {
                    int diceResultValue = behavior.DiceResultValue;
                    this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Default, diceResultValue);
                    target.TakeBreakDamage(diceResultValue, DamageType.Emotion, this._owner);
                    target.battleCardResultLog?.SetCreatureEffectSound("Creature/Teddy_Atk");
                    target.battleCardResultLog?.SetCreatureAbilityEffect("1/HappyTeddy_Hug");
                }
            }

            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                if (!behavior.IsParrying())
                    return;
                if (behavior.card.target == this._lastTarget)
                {
                    ++this._parryingCount;
                }
                else
                {
                    this._parryingCount = 0;
                    this._lastTarget = behavior.card?.target;
                }
            }

            public override void OnSelectEmotion()
            {
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_HappyTeddyBear1.LogEmotionCardAbility_HappyTeddyBear1.BattleUnitBuf_teddy_hug());
                this._owner.view.unitBottomStatUI.SetBufs();
            }

            public class BattleUnitBuf_teddy_hug : BattleUnitBuf
            {
                public override string keywordId => "Teddy_Head";

                public override BufPositiveType positiveType => BufPositiveType.Positive;

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }

                public override void OnDie() => this.Destroy();
            }
        }
    }

    public class PickUpModel_HappyTeddyBear2 : CreaturePickUpModel
    {
        public PickUpModel_HappyTeddyBear2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_HappyTeddyBear2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_HappyTeddyBear2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_HappyTeddyBear2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370112), (EmotionCardAbilityBase)new PickUpModel_HappyTeddyBear2.LogEmotionCardAbility_HappyTeddyBear2(), model);
        }

        public class LogEmotionCardAbility_HappyTeddyBear2 : EmotionCardAbilityBase
        {
            public LorId _id = LorId.None;

            public override void OnRoundStart()
            {
                if (this._id == LorId.None)
                {
                    List<BattleDiceCardModel> hand = this._owner.allyCardDetail.GetHand();
                    hand.RemoveAll((Predicate<BattleDiceCardModel>)(x => x.GetSpec().Ranged == CardRange.Instance));
                    int highest = 0;
                    foreach (BattleDiceCardModel battleDiceCardModel in hand)
                    {
                        int cost = battleDiceCardModel.GetCost();
                        if (highest < cost)
                            highest = cost;
                    }
                    List<BattleDiceCardModel> all = hand.FindAll((Predicate<BattleDiceCardModel>)(x => x.GetCost() == highest));
                    this._id = all.Count <= 0 ? LorId.None : RandomUtil.SelectOne<BattleDiceCardModel>(all).GetID();
                }
                this.Ability();
            }

            public override void OnSelectEmotion() => SoundEffectPlayer.PlaySound("Creature/Teddy_On");

            public void Ability()
            {
                if (this._id == LorId.None)
                    return;
                foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetID() == this._id)))
                {
                    battleDiceCardModel.AddBufWithoutDuplication((BattleDiceCardBuf)new PickUpModel_HappyTeddyBear2.LogEmotionCardAbility_HappyTeddyBear2.TeddyCardBuf());
                    battleDiceCardModel.SetAddedIcon("TeddyIcon");
                }
            }

            public class TeddyCardBuf : BattleDiceCardBuf
            {
                public override DiceCardBufType bufType => DiceCardBufType.Teddy;

                public override string keywordIconId => "TeddyIcon";

                public override void OnUseCard(BattleUnitModel owner)
                {
                    List<BattleDiceCardModel> allDeck = owner.allyCardDetail.GetAllDeck();
                    if (this._card.GetOriginCost() == 4 && this._card.GetCost() <= 0)
                        PlatformManager.Instance.UnlockAchievement(AchievementEnum.ONCE_FLOOR1);
                    foreach (BattleDiceCardModel battleDiceCardModel in allDeck.FindAll((Predicate<BattleDiceCardModel>)(x => x.GetID() == this._card.GetID())))
                        battleDiceCardModel.AddCost(-1);
                }
            }
        }
    }

    public class PickUpModel_HappyTeddyBear3 : CreaturePickUpModel
    {
        public PickUpModel_HappyTeddyBear3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_HappyTeddyBear3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_HappyTeddyBear3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_HappyTeddyBear3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370113), (EmotionCardAbilityBase)new PickUpModel_HappyTeddyBear3.LogEmotionCardAbility_HappyTeddyBear3(), model);
        }

        public class LogEmotionCardAbility_HappyTeddyBear3 : EmotionCardAbilityBase
        {
            public const int _addMin = 1;
            public const int _addMax = 2;
            public const int _redMin = 1;
            public const int _redMax = 2;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!behavior.IsParrying())
                {
                    int num = RandomUtil.Range(1, 2);
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        power = -num
                    });
                }
                else
                {
                    int num = RandomUtil.Range(1, 2);
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        power = num
                    });
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("1/Teddy_Heart");
                    behavior.card.target?.battleCardResultLog?.SetCreatureEffectSound("Creature/Teddy_Guard");
                }
            }
        }
    }

    public class PickUpModel_HeartofAspiration : PickUpModelBase
    {
        public PickUpModel_HeartofAspiration()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_HeartofAspiration_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_HeartofAspiration_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370101));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370102));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370103));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_HeartofAspiration0 : CreaturePickUpModel
    {
        public PickUpModel_HeartofAspiration0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370101),
      new LorId(LogLikeMod.ModId, 15370102),
      new LorId(LogLikeMod.ModId, 15370103)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_HeartofAspiration_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_HeartofAspiration"];
    }

    public class PickUpModel_HeartofAspiration1 : CreaturePickUpModel
    {
        public PickUpModel_HeartofAspiration1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_HeartofAspiration1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_HeartofAspiration1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_HeartofAspiration1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp()
        {
            base.OnPickUp();
            SoundEffectPlayer.PlaySound("Creature/Heartbeat");
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370101), (EmotionCardAbilityBase)new PickUpModel_HeartofAspiration1.LogEmotionCardAbility_HeartofAspiration1(), model);
        }

        public class LogEmotionCardAbility_HeartofAspiration1 : EmotionCardAbilityBase
        {
            public Battle.CreatureEffect.CreatureEffect _heartBeatEffect;

            public override void OnSelectEmotion()
            {
                BattleCamManager instance1 = SingletonBehavior<BattleCamManager>.Instance;
                CameraFilterPack_Blur_Radial r = instance1 != null ? instance1.AddCameraFilter<CameraFilterPack_Blur_Radial>() : (CameraFilterPack_Blur_Radial)null;
                BattleCamManager instance2 = SingletonBehavior<BattleCamManager>.Instance;
                if (instance2 == null)
                    return;
                instance2.StartCoroutine(this.Pinpong(r));
            }

            public override void OnSelectEmotionOnce()
            {
                base.OnSelectEmotionOnce();
                SoundEffectPlayer.PlaySound("Creature/Heartbeat");
            }

            public IEnumerator Pinpong(CameraFilterPack_Blur_Radial r)
            {
                float elapsedTime = 0.0f;
                while ((double)elapsedTime < 1.0)
                {
                    elapsedTime += Time.deltaTime;
                    r.Intensity = Mathf.PingPong(Time.time, 0.05f);
                    yield return new WaitForEndOfFrame();
                }
                BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
                if (instance != null)
                    instance.RemoveCameraFilter<CameraFilterPack_Blur_Radial>();
            }

            public override void OnRoundEnd()
            {
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, RandomUtil.Range(1, 2), this._owner);
                if (this._owner.history.damageAtOneRound > 0)
                    return;
                this._owner.LoseHp(Mathf.Min(this._owner.MaxHp / 4, 30));
            }
        }
    }

    public class PickUpModel_HeartofAspiration2 : CreaturePickUpModel
    {
        public PickUpModel_HeartofAspiration2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_HeartofAspiration2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_HeartofAspiration2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_HeartofAspiration2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370102), (EmotionCardAbilityBase)new PickUpModel_HeartofAspiration2.LogEmotionCardAbility_HeartofAspiration2(), model);
        }

        public class LogEmotionCardAbility_HeartofAspiration2 : EmotionCardAbilityBase
        {
            public Battle.CreatureEffect.CreatureEffect _heartBeatEffect;

            public override void OnSelectEmotion()
            {
                Singleton<StageController>.Instance.onChangePhase += new StageController.OnChangePhaseDelegate(((EmotionCardAbilityBase)this).OnChangeStagePhase);
                SoundEffectPlayer.PlaySound("Creature/Heartbeat");
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_Heart", 1f, this._owner.view, this._owner.view, 3f);
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_heart.BattleUnitBuf_Emotion_Heart_Eager());
                this._owner.view.unitBottomStatUI.SetBufs();
            }

            public override StatBonus GetStatBonus()
            {
                return new StatBonus() { hpRate = 15 };
            }

            public override int GetSpeedDiceAdder()
            {
                int speedDiceAdder = RandomUtil.Range(1, 2);
                this._owner.ShowTypoTemporary(this._emotionCard, 0, ResultOption.Sign, speedDiceAdder);
                return speedDiceAdder;
            }

            public class BattleUnitBuf_Emotion_Heart_Eager : BattleUnitBuf
            {
                public override string keywordId => "HeartofAspiration_Heart";

                public override BufPositiveType positiveType => BufPositiveType.Positive;

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }

                public override void OnDie() => this.Destroy();
            }
        }
    }

    public class PickUpModel_HeartofAspiration3 : CreaturePickUpModel
    {
        public PickUpModel_HeartofAspiration3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_HeartofAspiration3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_HeartofAspiration3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_HeartofAspiration3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370103), (EmotionCardAbilityBase)new PickUpModel_HeartofAspiration3.LogEmotionCardAbility_HeartofAspiration3(), model);
        }

        public class LogEmotionCardAbility_HeartofAspiration3 : EmotionCardAbilityBase
        {
            public const int _maxTurn = 3;
            public const int _str = 4;
            public const int _endur = 4;
            public const int _quick = 4;
            public const int _prot = 4;
            public int count;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 4, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 4, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 4, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 4, this._owner);
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                ++this.count;
                if (this.count < 3)
                    return;
                this._owner.Die();
            }

            public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
            {
                return this.count < 3 ? AtkResist.Resist : base.GetResistBP(origin, detail);
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this.count = 0;
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_FastBeat", 1f, this._owner.view, this._owner.view);
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Heart_Fast");
                if (soundEffectPlayer == null)
                    return;
                soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_FastBeat", 1f, this._owner.view, this._owner.view);
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Heart_Fast");
                if (soundEffectPlayer == null)
                    return;
                soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
            }

            public override void OnKill(BattleUnitModel target)
            {
                if (target.faction != Faction.Enemy)
                    return;
                Singleton<StageController>.Instance.GetStageModel().AddHeartKillCount();
            }
        }
    }

    public class PickUpModel_House : PickUpModelBase
    {
        public PickUpModel_House()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_House_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_House_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370271));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370272));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370273));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_House0 : CreaturePickUpModel
    {
        public PickUpModel_House0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370271),
      new LorId(LogLikeMod.ModId, 15370272),
      new LorId(LogLikeMod.ModId, 15370273)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_House_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_House"];
    }

    public class PickUpModel_House1 : CreaturePickUpModel
    {
        public PickUpModel_House1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_House1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_House1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_House1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370271), (EmotionCardAbilityBase)new PickUpModel_House1.LogEmotionCardAbility_House1(), model);
        }

        public class LogEmotionCardAbility_House1 : EmotionCardAbilityBase
        {
            public const int _penalty = -2;
            public GameObject _aura;
            public bool _sound;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                SoundEffectPlayer.PlaySound("Creature/House_Lion_Poison");
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                int stack = BattleObjectManager.instance.GetAliveList(this._owner.faction).Count - 2;
                if (stack > 0)
                {
                    this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack, this._owner);
                    this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, stack, this._owner);
                    Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Lion", 1f, this._owner.view, this._owner.view);
                    this._aura = fxCreatureEffect != null ? fxCreatureEffect.gameObject : (GameObject)null;
                }
                else if (stack < 0)
                {
                    if (!this._sound)
                    {
                        this._sound = true;
                        SoundEffectPlayer.PlaySound("Creature/House_Lion_Change");
                    }
                    this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, -stack, this._owner);
                    this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, -stack, this._owner);
                    Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Cat", 1f, this._owner.view, this._owner.view);
                    this._aura = fxCreatureEffect != null ? fxCreatureEffect.gameObject : (GameObject)null;
                }
                else
                {
                    Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Lion", 1f, this._owner.view, this._owner.view);
                    this._aura = fxCreatureEffect != null ? fxCreatureEffect.gameObject : (GameObject)null;
                }
            }

            public override void OnDie(BattleUnitModel killer)
            {
                base.OnDie(killer);
                this.DestroyAura();
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.DestroyAura();
            }

            public void DestroyAura()
            {
                if (!(this._aura != null))
                    return;
                GameObject.Destroy(this._aura);
                this._aura = (GameObject)null;
            }
        }
    }

    public class PickUpModel_House2 : CreaturePickUpModel
    {
        public PickUpModel_House2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_House2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_House2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_House2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370272), (EmotionCardAbilityBase)new PickUpModel_House2.LogEmotionCardAbility_House2(), model);
        }

        public class LogEmotionCardAbility_House2 : EmotionCardAbilityBase
        {
            public const int _dmgPerStack = 3;
            public int stack = 1;

            public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
            {
                base.OnParryingStart(card);
                BattleUnitModel target = card?.target;
                if (target == null)
                    return;
                if (!this.CheckAbility(target))
                {
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                    {
                        foreach (BattleEmotionCardModel passive in alive.emotionDetail.PassiveList)
                        {
                            if (passive.AbilityList.Find((Predicate<EmotionCardAbilityBase>)(x => x is PickUpModel_House2.LogEmotionCardAbility_House2)) is PickUpModel_House2.LogEmotionCardAbility_House2 cardAbilityHouse2)
                                cardAbilityHouse2.StackToZero();
                        }
                    }
                }
                else
                {
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                        dmg = 3 * this.stack
                    });
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                    {
                        foreach (BattleEmotionCardModel passive in alive.emotionDetail.PassiveList)
                        {
                            if (passive.AbilityList.Find((Predicate<EmotionCardAbilityBase>)(x => x is PickUpModel_House2.LogEmotionCardAbility_House2)) is PickUpModel_House2.LogEmotionCardAbility_House2 cardAbilityHouse2)
                                cardAbilityHouse2.StackAdd();
                        }
                    }
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("7/WayBeckHome_Emotion_Atk", 1f);
                }
            }

            public override void OnStartOneSideAction(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnStartOneSideAction(curCard);
                BattleUnitModel target = curCard?.target;
                if (target == null)
                    return;
                if (!this.CheckAbility(target))
                {
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                    {
                        foreach (BattleEmotionCardModel passive in alive.emotionDetail.PassiveList)
                        {
                            if (passive.AbilityList.Find((Predicate<EmotionCardAbilityBase>)(x => x is PickUpModel_House2.LogEmotionCardAbility_House2)) is PickUpModel_House2.LogEmotionCardAbility_House2 cardAbilityHouse2)
                                cardAbilityHouse2.StackToZero();
                        }
                    }
                }
                else
                {
                    curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                        dmg = 3 * this.stack
                    });
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                    {
                        foreach (BattleEmotionCardModel passive in alive.emotionDetail.PassiveList)
                        {
                            if (passive.AbilityList.Find((Predicate<EmotionCardAbilityBase>)(x => x is PickUpModel_House2.LogEmotionCardAbility_House2)) is PickUpModel_House2.LogEmotionCardAbility_House2 cardAbilityHouse2)
                                cardAbilityHouse2.StackAdd();
                        }
                    }
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("7/WayBeckHome_Emotion_Atk", 1f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_NormalAtk");
                }
            }

            public override void OnRoundStartOnce()
            {
                base.OnRoundStartOnce();
                this.stack = 1;
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
                double num1 = Math.Ceiling((double)aliveList.Count * 0.5);
                int num2 = 0;
                while (aliveList.Count > 0 && num1 > 0.0)
                {
                    BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
                    if (battleUnitModel != null)
                    {
                        ++num2;
                        PickUpModel_House2.LogEmotionCardAbility_House2.BattleUnitBuf_Emotion_WayBackHome_Target buf = new PickUpModel_House2.LogEmotionCardAbility_House2.BattleUnitBuf_Emotion_WayBackHome_Target(num2);
                        battleUnitModel.bufListDetail.AddBuf((BattleUnitBuf)buf);
                        --num1;
                    }
                    aliveList.Remove(battleUnitModel);
                }
            }

            public bool CheckAbility(BattleUnitModel target)
            {
                BattleUnitBuf battleUnitBuf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_House2.LogEmotionCardAbility_House2.BattleUnitBuf_Emotion_WayBackHome_Target));
                return battleUnitBuf != null && battleUnitBuf.stack == this.stack;
            }

            public void StackToZero() => this.stack = 0;

            public void StackAdd() => ++this.stack;

            public class BattleUnitBuf_Emotion_WayBackHome_Target : BattleUnitBuf
            {
                public GameObject aura;

                public override string keywordId => "WayBackHome_Emotion_Target";

                public override string keywordIconId => "WayBackHome_Target";

                public BattleUnitBuf_Emotion_WayBackHome_Target(int value) => this.stack = value;

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    Battle.CreatureEffect.CreatureEffect creatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("7/WayBeckHome_Emotion_Way", 1f, owner.view, owner.view);
                    this.aura = creatureEffect != null ? creatureEffect.gameObject : (GameObject)null;
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }

                public override void OnDie()
                {
                    base.OnDie();
                    this.Destroy();
                }

                public override void Destroy()
                {
                    base.Destroy();
                    this.DestroyAura();
                }

                public void DestroyAura()
                {
                    if (!(this.aura != null))
                        return;
                    UnityEngine.Object.Destroy(this.aura);
                    this.aura = (GameObject)null;
                }
            }
        }
    }

    public class PickUpModel_House3 : CreaturePickUpModel
    {
        public PickUpModel_House3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_House3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_House3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_House3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370273), (EmotionCardAbilityBase)new PickUpModel_House3.LogEmotionCardAbility_House3(), model);
        }

        public class LogEmotionCardAbility_House3 : EmotionCardAbilityBase
        {
            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this.GiveBuf();
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this.GiveBuf();
            }

            public override void OnRoundStart() => this.GiveBuf();

            public void GiveBuf()
            {
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                {
                    if (alive == this._owner)
                    {
                        if (alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protected)) == null)
                        {
                            BattleUnitBuf buf = (BattleUnitBuf)new PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protected();
                            alive.bufListDetail.AddBuf(buf);
                        }
                    }
                    else if (alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protect)) == null)
                    {
                        BattleUnitBuf buf = (BattleUnitBuf)new PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protect();
                        alive.bufListDetail.AddBuf(buf);
                    }
                }
            }

            public class BattleUnitBuf_Emotion_WayBackHome_Protected : BattleUnitBuf
            {
                public override string keywordId => "WayBackHome_Emotion_Protected";
            }

            public class BattleUnitBuf_Emotion_WayBackHome_Protect : BattleUnitBuf
            {
                public const int _powMin = 1;
                public const int _powMax = 2;

                public int Pow => RandomUtil.Range(1, 2);

                public override bool Hide => true;

                public override string keywordId => "WayBackHome_Emotion_Protected";

                public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
                {
                    base.OnStartParrying(card);
                    BattlePlayingCardDataInUnitModel currentDiceAction = card?.target.currentDiceAction;
                    bool flag;
                    if (currentDiceAction == null)
                    {
                        flag = false;
                    }
                    else
                    {
                        BattleUnitModel earlyTarget = currentDiceAction.earlyTarget;
                        flag = earlyTarget != null && earlyTarget.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protected)) != null;
                    }
                    if (!flag)
                        return;
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                        power = this.Pow
                    });
                    this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Together", 2f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_MakeRoad");
                }
            }
        }
    }

    public class PickUpModel_KnightOfDespair : PickUpModelBase
    {
        public PickUpModel_KnightOfDespair()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_KnightOfDespair_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_KnightOfDespair_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370151));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370152));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370153));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_KnightOfDespair0 : CreaturePickUpModel
    {
        public PickUpModel_KnightOfDespair0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370151),
      new LorId(LogLikeMod.ModId, 15370152),
      new LorId(LogLikeMod.ModId, 15370153)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_KnightOfDespair_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_KnightOfDespair"];
    }

    public class PickUpModel_KnightOfDespair1 : CreaturePickUpModel
    {
        public PickUpModel_KnightOfDespair1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_KnightOfDespair1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_KnightOfDespair1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_KnightOfDespair1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370151), (EmotionCardAbilityBase)new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1(), model);
        }

        public class LogEmotionCardAbility_KnightOfDespair1 : EmotionCardAbilityBase
        {
            public List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo> _dmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
            public List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo> _breakdmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._dmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
                this._breakdmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
                this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Slash
                });
                this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Penetrate
                });
                this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Hit
                });
                this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Slash
                });
                this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Penetrate
                });
                this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Hit
                });
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._dmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
                this._breakdmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
                this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Slash
                });
                this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Penetrate
                });
                this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Hit
                });
                this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Slash
                });
                this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Penetrate
                });
                this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
                {
                    type = BehaviourDetail.Hit
                });
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.BattleUnitBuf_Gaho());
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KnightOfDespair_Gaho", false, 2f);
                try
                {
                    SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/KnightOfDespair_Gaho");
                    if (!(soundEffectPlayer != null))
                        return;
                    soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                }
                catch
                {
                }
            }

            public override void OnRoundStart()
            {
                this._dmgInfos.Sort((Comparison<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>)((x, y) => y.dmg - x.dmg));
                this._breakdmgInfos.Sort((Comparison<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>)((x, y) => y.dmg - x.dmg));
                BehaviourDetail behaviourDetail1 = BehaviourDetail.None;
                BehaviourDetail behaviourDetail2 = BehaviourDetail.None;
                if (this._dmgInfos[0].dmg > 0)
                    behaviourDetail1 = this._dmgInfos[0].type;
                if (this._breakdmgInfos[0].dmg > 0)
                    behaviourDetail2 = this._breakdmgInfos[0].type;
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.BattleUnitBuf_resists()
                {
                    hpTarget = behaviourDetail1,
                    bpTarget = behaviourDetail2
                });
                this._dmgInfos.ForEach((Action<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>)(x => x.dmg = 0));
                this._breakdmgInfos.ForEach((Action<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>)(x => x.dmg = 0));
            }

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo dmgInfo = this._dmgInfos.Find((Predicate<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>)(x => x.type == atkDice.Detail));
                if (dmgInfo == null)
                    return;
                dmgInfo.dmg += dmg;
            }

            public override void OnTakeBreakDamageByAttack(BattleDiceBehavior atkDice, int breakdmg)
            {
                PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo dmgInfo = this._breakdmgInfos.Find((Predicate<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>)(x => x.type == atkDice.Detail));
                if (dmgInfo == null)
                    return;
                dmgInfo.dmg += breakdmg;
            }

            public class BattleUnitBuf_resists : BattleUnitBuf
            {
                public BehaviourDetail hpTarget = BehaviourDetail.None;
                public BehaviourDetail bpTarget = BehaviourDetail.None;

                public override bool Hide => true;

                public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
                {
                    if (this.hpTarget == BehaviourDetail.None)
                        return base.GetResistHP(origin, detail);
                    return this.hpTarget == detail ? AtkResist.Endure : base.GetResistHP(origin, detail);
                }

                public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
                {
                    if (this.bpTarget == BehaviourDetail.None)
                        return base.GetResistBP(origin, detail);
                    return this.bpTarget == detail ? AtkResist.Endure : base.GetResistBP(origin, detail);
                }

                public override void OnRoundEnd() => this.Destroy();
            }

            public class DmgInfo
            {
                public BehaviourDetail type;
                public int dmg;
            }

            public class BattleUnitBuf_Gaho : BattleUnitBuf
            {
                public override string keywordId => "Gaho";
            }
        }
    }

    public class PickUpModel_KnightOfDespair2 : CreaturePickUpModel
    {
        public PickUpModel_KnightOfDespair2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_KnightOfDespair2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_KnightOfDespair2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_KnightOfDespair2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370152), (EmotionCardAbilityBase)new PickUpModel_KnightOfDespair2.LogEmotionCardAbility_KnightOfDespair2(), model);
        }

        public class LogEmotionCardAbility_KnightOfDespair2 : EmotionCardAbilityBase
        {
            public const int _powerMin = 1;
            public const int _powerMax = 3;
            public const int _quickMin = 1;
            public const int _quickMax = 3;
            public const int _vulne = 2;
            public int stack;
            public int tempStack;
            public SpriteFilter_Despair _filter;

            public static int Power => RandomUtil.Range(1, 3);

            public static int Quick => RandomUtil.Range(1, 3);

            public override void OnDieOtherUnit(BattleUnitModel unit)
            {
                base.OnDieOtherUnit(unit);
                if (unit.faction != Faction.Player)
                    return;
                ++this.stack;
            }

            public override void OnRoundStartOnce()
            {
                base.OnRoundStartOnce();
                if (this.tempStack > 0)
                {
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                    {
                        for (int index = 0; index < this.tempStack; ++index)
                            alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, 2);
                    }
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                    {
                        for (int index = 0; index < this.tempStack; ++index)
                        {
                            alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, PickUpModel_KnightOfDespair2.LogEmotionCardAbility_KnightOfDespair2.Power, alive);
                            alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, PickUpModel_KnightOfDespair2.LogEmotionCardAbility_KnightOfDespair2.Quick, alive);
                        }
                    }
                    if (this._filter == null)
                    {
                        this._filter = new GameObject().AddComponent<SpriteFilter_Despair>();
                        this._filter.Init("EmotionCardFilter/KnightOfDespair_Gaho", true, 1f);
                    }
                }
                this.tempStack = 0;
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (this._filter != null)
                {
                    this._filter.ManualDestroy();
                    this._filter = (SpriteFilter_Despair)null;
                }
                this.tempStack = this.stack;
                this.stack = 0;
            }

            public override void OnBattleEnd()
            {
                base.OnBattleEnd();
                if (!(this._filter != null))
                    return;
                this._filter.ManualDestroy();
                this._filter = (SpriteFilter_Despair)null;
            }
        }
    }

    public class PickUpModel_KnightOfDespair3 : CreaturePickUpModel
    {
        public PickUpModel_KnightOfDespair3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_KnightOfDespair3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_KnightOfDespair3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_KnightOfDespair3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370153), (EmotionCardAbilityBase)new PickUpModel_KnightOfDespair3.LogEmotionCardAbility_KnightOfDespair3(), model);
        }

        public class LogEmotionCardAbility_KnightOfDespair3 : EmotionCardAbilityBase
        {
            public const int _dmgRateMin = 10;
            public const int _dmgRateMax = 10;
            public const int _dmgMax = 12;
            public bool _bMaxDiceValue;

            public static int DmgRate => RandomUtil.Range(10, 10);

            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                this._bMaxDiceValue = false;
                if (behavior.Detail != BehaviourDetail.Penetrate || behavior.DiceVanillaValue != behavior.GetDiceMax())
                    return;
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/KnightOfDespair_Atk_Strong");
                this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Despair));
                this._bMaxDiceValue = true;
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                if (!this._bMaxDiceValue)
                    return;
                BattleUnitModel target = behavior.card?.target;
                if (target == null)
                    return;
                int v = Mathf.Min(12, target.MaxHp * PickUpModel_KnightOfDespair3.LogEmotionCardAbility_KnightOfDespair3.DmgRate / 100);
                target.TakeDamage(v, DamageType.Emotion, this._owner);
            }
        }
    }

    public class PickUpModel_Laetitia : PickUpModelBase
    {
        public PickUpModel_Laetitia()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Laetitia_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Laetitia_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370331));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370332));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370333));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 4
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Laetitia0 : CreaturePickUpModel
    {
        public PickUpModel_Laetitia0()
        {
            this.level = 4;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370331),
      new LorId(LogLikeMod.ModId, 15370332),
      new LorId(LogLikeMod.ModId, 15370333)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Laetitia_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Laetitia"];
    }

    public class PickUpModel_Laetitia1 : CreaturePickUpModel
    {
        public PickUpModel_Laetitia1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Laetitia1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Laetitia1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Laetitia1_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370331), (EmotionCardAbilityBase)new PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1(), model);
        }

        public class LogEmotionCardAbility_Laetitia1 : EmotionCardAbilityBase
        {
            public BattleUnitModel _target;

            public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
            {
                base.OnParryingStart(card);
                BattleUnitModel target = card?.target;
                if (target == null || this._target != null)
                    return;
                PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift buf = new PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift(this._owner);
                target.bufListDetail.AddBuf((BattleUnitBuf)buf);
                this._target = target;
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._target = (BattleUnitModel)null;
            }

            public override void OnDieOtherUnit(BattleUnitModel unit)
            {
                base.OnDieOtherUnit(unit);
                if (unit != this._target)
                    return;
                this._target = (BattleUnitModel)null;
            }

            public class BattleUnitBuf_Emotion_Latitia_Gift : BattleUnitBuf
            {
                public const float _prob = 0.4f;
                public const int _dmgMin = 2;
                public const int _dmgMax = 7;
                public const int _bleedMin = 2;
                public const int _bleedMax = 2;
                public BattleUnitModel _giver;

                public static bool Prob => (double)RandomUtil.valueForProb < 0.40000000596046448;

                public static int Dmg => RandomUtil.Range(2, 7);

                public static int Bleed => RandomUtil.Range(2, 2);

                public override string keywordId => "Latitia_Gift";

                public BattleUnitBuf_Emotion_Latitia_Gift(BattleUnitModel giver) => this._giver = giver;

                public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
                {
                    base.OnStartParrying(card);
                    BattleUnitModel target = card?.target;
                    if (target == null || this._giver == null || this._giver == target || !PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift.Prob)
                        return;
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Latitia_Boom", 1.5f);
                    this._owner.TakeDamage(PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift.Dmg, DamageType.Emotion, this._giver);
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift.Bleed, this._giver);
                }
            }
        }
    }

    public class PickUpModel_Laetitia2 : CreaturePickUpModel
    {
        public PickUpModel_Laetitia2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Laetitia2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Laetitia2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Laetitia2_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370332), (EmotionCardAbilityBase)new PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2(), model);
        }

        public class LogEmotionCardAbility_Laetitia2 : EmotionCardAbilityBase
        {
            public const int _powMin = 2;
            public const int _powMax = 4;

            public static int Pow => RandomUtil.Range(2, 4);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                List<BattleDiceCardModel> hand = this._owner.allyCardDetail.GetHand();
                hand.RemoveAll((Predicate<BattleDiceCardModel>)(x => x.GetSpec().Ranged == CardRange.Instance));
                if (hand.Count <= 0 || hand.Find((Predicate<BattleDiceCardModel>)(x => x.GetBufList().Find((Predicate<BattleDiceCardBuf>)(y => y is PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.BattleDiceCardBuf_Emotion_Heart)) != null)) != null)
                    return;
                BattleDiceCardModel battleDiceCardModel = RandomUtil.SelectOne<BattleDiceCardModel>(hand);
                battleDiceCardModel.AddBuf((BattleDiceCardBuf)new PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.BattleDiceCardBuf_Emotion_Heart());
                battleDiceCardModel.SetAddedIcon("Latitia_Heart");
            }

            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnUseCard(curCard);
                BattleDiceCardBuf battleDiceCardBuf = curCard?.card?.GetBufList().Find((Predicate<BattleDiceCardBuf>)(y => y is PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.BattleDiceCardBuf_Emotion_Heart));
                if (battleDiceCardBuf == null)
                    return;
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                {
                    power = PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.Pow
                });
                battleDiceCardBuf.Destroy();
            }

            public class BattleDiceCardBuf_Emotion_Heart : BattleDiceCardBuf
            {
                public const int _dmgMin = 2;
                public const int _dmgMax = 7;
                public const int _turn = 2;
                public int turn;
                public bool used;

                public static int Dmg => RandomUtil.Range(2, 7);

                public override string keywordIconId => "Latitia_Heart";

                public override void OnUseCard(BattleUnitModel owner)
                {
                    base.OnUseCard(owner);
                    this.used = true;
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    if (this.used)
                    {
                        this.Destroy();
                    }
                    else
                    {
                        ++this.turn;
                        if (this.turn < 2)
                            return;
                        BattleUnitModel owner = this._card?.owner;
                        owner?.TakeDamage(PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.BattleDiceCardBuf_Emotion_Heart.Dmg, DamageType.Emotion, owner);
                        this._card.temporary = true;
                        new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Latitia_Filter_Grey", false, 2f);
                        this.Destroy();
                    }
                }
            }
        }
    }

    public class PickUpModel_Laetitia3 : CreaturePickUpModel
    {
        public PickUpModel_Laetitia3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Laetitia3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Laetitia3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Laetitia3_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370333), (EmotionCardAbilityBase)new PickUpModel_Laetitia3.LogEmotionCardAbility_Laetitia3(), model);
        }

        public class LogEmotionCardAbility_Laetitia3 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 2;
            public const int _dmgMax = 4;
            public const float _prob = 0.5f;

            public static int Dmg => RandomUtil.Range(2, 4);

            public static bool Prob => (double)RandomUtil.valueForProb < 0.5;

            public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
            {
                base.ChangeDiceResult(behavior, ref diceResult);
                if (behavior.Index != 0)
                    return;
                if (PickUpModel_Laetitia3.LogEmotionCardAbility_Laetitia3.Prob)
                {
                    diceResult = behavior.GetDiceMax();
                }
                else
                {
                    diceResult = 1;
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Latitia_Boom", 1.5f);
                    this._owner.TakeDamage(PickUpModel_Laetitia3.LogEmotionCardAbility_Laetitia3.Dmg, DamageType.Emotion, this._owner);
                }
            }
        }
    }

    public class PickUpModel_LittleHelper : PickUpModelBase
    {
        public PickUpModel_LittleHelper()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LittleHelper_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LittleHelper_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370121));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370122));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370123));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_LittleHelper0 : CreaturePickUpModel
    {
        public PickUpModel_LittleHelper0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370121),
      new LorId(LogLikeMod.ModId, 15370122),
      new LorId(LogLikeMod.ModId, 15370123)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_LittleHelper_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_LittleHelper"];
    }

    public class PickUpModel_LittleHelper1 : CreaturePickUpModel
    {
        public PickUpModel_LittleHelper1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LittleHelper1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LittleHelper1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LittleHelper1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370121), (EmotionCardAbilityBase)new PickUpModel_LittleHelper1.LogEmotionCardAbility_LittleHelper1(), model);
        }

        public class LogEmotionCardAbility_LittleHelper1 : EmotionCardAbilityBase
        {
            public GameObject _aura;

            public override void OnRoundStart_after()
            {
                base.OnRoundStart_after();
                if (this._owner.cardSlotDetail.PlayPoint < this._owner.cardSlotDetail.GetMaxPlayPoint())
                    return;
                if (this._aura != null)
                    this.DestroyAura();
                Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Charge", 1f, this._owner.view, this._owner.view);
                this._aura = fxCreatureEffect != null ? fxCreatureEffect.gameObject : (GameObject)null;
                SoundEffectPlayer.PlaySound("Creature/Helper_FullCharge");
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_LittleHelper1.LogEmotionCardAbility_LittleHelper1.BattleUnitBuf_Emotion_Helper_Charge());
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.DestroyAura();
            }

            public override void OnDie(BattleUnitModel killer)
            {
                base.OnDie(killer);
                this.DestroyAura();
            }

            public void DestroyAura()
            {
                if (!(this._aura != null))
                    return;
                GameObject.Destroy(this._aura);
                this._aura = (GameObject)null;
            }

            public class BattleUnitBuf_Emotion_Helper_Charge : BattleUnitBuf
            {
                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, RandomUtil.Range(1, 2), owner);
                }

                public override void BeforeGiveDamage(BattleDiceBehavior behavior)
                {
                    base.BeforeGiveDamage(behavior);
                    if (behavior == null)
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = RandomUtil.Range(2, 7)
                    });
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_LittleHelper2 : CreaturePickUpModel
    {
        public PickUpModel_LittleHelper2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LittleHelper2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LittleHelper2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LittleHelper2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370122), (EmotionCardAbilityBase)new PickUpModel_LittleHelper2.LogEmotionCardAbility_LittleHelper2(), model);
        }

        public class LogEmotionCardAbility_LittleHelper2 : EmotionCardAbilityBase
        {
            public const int _recoverPP = 1;
            public const int _count = 3;
            public const int _quickMin = 1;
            public const int _quickMax = 2;
            public int cnt;
            public int quick;

            public static int Quick => RandomUtil.Range(1, 2);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this.quick <= 0)
                    return;
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Scan_Start", 1f, this._owner.view, this._owner.view, 3f);
                SoundEffectPlayer.PlaySound("Creature/Helper_On");
                this.quick = 0;
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                ++this.cnt;
                if (this.cnt < 3)
                    return;
                this.cnt %= 3;
                this._owner.cardSlotDetail.RecoverPlayPoint(1);
                ++this.quick;
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("2_Y/FX_IllusionCard_2_Y_Scan", 3f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Helper_On");
            }

            public override void OnRoundEnd()
            {
                if (this.quick <= 0)
                    return;
                for (int index = 0; index < this.quick; ++index)
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, PickUpModel_LittleHelper2.LogEmotionCardAbility_LittleHelper2.Quick, this._owner);
            }
        }
    }

    public class PickUpModel_LittleHelper3 : CreaturePickUpModel
    {
        public PickUpModel_LittleHelper3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LittleHelper3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LittleHelper3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LittleHelper3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370123), (EmotionCardAbilityBase)new PickUpModel_LittleHelper3.LogEmotionCardAbility_LittleHelper3(), model);
        }

        public class LogEmotionCardAbility_LittleHelper3 : EmotionCardAbilityBase
        {
            public const int _addMax = 3;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior == null || behavior.card?.target == null)
                    return;
                int a = behavior.card.speedDiceResultValue - behavior.card.target.speedDiceResult[behavior.card.targetSlotOrder].value;
                if (a > 0 && this.IsAttackDice(behavior.Detail))
                {
                    int num = Mathf.Min(a, 3);
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        power = num
                    });
                }
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                BattleUnitModel target = behavior.card?.target;
                int? speedDiceResultValue = behavior.card?.speedDiceResultValue;
                int? nullable = target?.speedDiceResult[behavior.card.targetSlotOrder].value;
                if (speedDiceResultValue.GetValueOrDefault() > nullable.GetValueOrDefault() & speedDiceResultValue.HasValue & nullable.HasValue)
                    target.battleCardResultLog?.SetCreatureAbilityEffect("2/Helper_Hit", 1.5f);
                if (behavior.Detail != BehaviourDetail.Slash || target == null)
                    return;
                target.battleCardResultLog?.SetCreatureEffectSound("Creature / Helper_Atk");
            }
        }
    }

    public class PickUpModel_LongBird : PickUpModelBase
    {
        public PickUpModel_LongBird()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LongBird_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LongBird_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370281));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370282));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370283));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_LongBird0 : CreaturePickUpModel
    {
        public PickUpModel_LongBird0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370281),
      new LorId(LogLikeMod.ModId, 15370282),
      new LorId(LogLikeMod.ModId, 15370283)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_LongBird_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_LongBird"];
    }

    public class PickUpModel_LongBird1 : CreaturePickUpModel
    {
        public PickUpModel_LongBird1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LongBird1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LongBird1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LongBird1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370281), (EmotionCardAbilityBase)new PickUpModel_LongBird1.LogEmotionCardAbility_LongBird1(), model);
        }

        public class LogEmotionCardAbility_LongBird1 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 2;
            public const int _dmgMax = 7;
            public const int _enduMin = 1;
            public const int _enduMax = 2;
            public const int _strMin = 1;
            public const int _strMax = 2;

            public int Dmg => RandomUtil.Range(2, 7);

            public int Endu => RandomUtil.Range(1, 2);

            public int Str => RandomUtil.Range(1, 2);

            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                base.OnRollDice(behavior);
                if (behavior.DiceVanillaValue > behavior.GetDiceMin())
                    return;
                this._owner.TakeDamage(this.Dmg, DamageType.Emotion, this._owner);
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, this.Endu, this._owner);
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, this.Str, this._owner);
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Judgement", 3f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/LongBird_Stun");
            }
        }
    }

    public class PickUpModel_LongBird2 : CreaturePickUpModel
    {
        public PickUpModel_LongBird2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LongBird2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LongBird2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LongBird2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370282), (EmotionCardAbilityBase)new PickUpModel_LongBird2.LogEmotionCardAbility_LongBird2(), model);
        }

        public class LogEmotionCardAbility_LongBird2 : EmotionCardAbilityBase
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if ((double)RandomUtil.valueForProb < 0.5)
                    {
                        BattleUnitBuf buf = alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_longbird2.BattleUnitBuf_LongBird_Emotion_Sin));
                        if (buf == null)
                        {
                            buf = (BattleUnitBuf)new EmotionCardAbility_longbird2.BattleUnitBuf_LongBird_Emotion_Sin();
                            alive.bufListDetail.AddBuf(buf);
                        }
                        ++buf.stack;
                    }
                }
            }

            public class BattleUnitBuf_LongBird_Emotion_Sin : BattleUnitBuf
            {
                public const int _stackMax = 5;
                public const float _hpRate = 0.1f;
                public bool triggered;

                public override KeywordBuf bufType => KeywordBuf.Emotion_Sin;

                public override string keywordId => "Sin_AbnormalityCard";

                public override string keywordIconId => "Sin_Abnormality";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }

                public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
                {
                    base.OnUseCard(card);
                    this.triggered = false;
                }

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    base.OnSuccessAttack(behavior);
                    BattleUnitModel target = behavior.card?.target;
                    if (target == null || this.triggered || this.stack <= 0)
                        return;
                    --this.stack;
                    this.triggered = true;
                    BattleUnitBuf buf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_LongBird2.LogEmotionCardAbility_LongBird2.BattleUnitBuf_LongBird_Emotion_Sin));
                    if (buf == null)
                    {
                        buf = (BattleUnitBuf)new PickUpModel_LongBird2.LogEmotionCardAbility_LongBird2.BattleUnitBuf_LongBird_Emotion_Sin();
                        target.bufListDetail.AddBuf(buf);
                    }
                    ++buf.stack;
                }

                public override void OnRoundEndTheLast()
                {
                    base.OnRoundEndTheLast();
                    if (this.stack < 5)
                        return;
                    this._owner.TakeDamage(Mathf.RoundToInt((float)this._owner.MaxHp * 0.1f), DamageType.Buf, this._owner);
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Judgement", 1f, this._owner.view, this._owner.view, 3f);
                    SoundEffectPlayer.PlaySound("Creature/LongBird_Down");
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_LongBird3 : CreaturePickUpModel
    {
        public PickUpModel_LongBird3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LongBird3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LongBird3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LongBird3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370283), (EmotionCardAbilityBase)new PickUpModel_LongBird3.LogEmotionCardAbility_LongBird3(), model);
        }

        public class LogEmotionCardAbility_LongBird3 : EmotionCardAbilityBase
        {
            public const int _healMin = 2;
            public const int _healMax = 8;
            public const int _cntMax = 3;
            public int cnt;

            public static int Heal => RandomUtil.Range(2, 8);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.cnt = 0;
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || this.cnt >= 3)
                    return;
                int maxHp = this.GetMaxHP();
                if ((int)target.hp < maxHp)
                    return;
                ++this.cnt;
                target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Scale", 2f);
                target.battleCardResultLog?.SetCreatureEffectSound("Creature/LongBird_On");
                this.GetHealTarget()?.RecoverHP(PickUpModel_LongBird3.LogEmotionCardAbility_LongBird3.Heal);
            }

            public BattleUnitModel GetHealTarget()
            {
                BattleUnitModel healTarget = (BattleUnitModel)null;
                List<BattleUnitModel> list = new List<BattleUnitModel>();
                int num = -100;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                {
                    if (num == -100)
                    {
                        list.Add(alive);
                        num = (int)alive.hp;
                    }
                    else if ((int)alive.hp < num)
                    {
                        list.Clear();
                        list.Add(alive);
                        num = (int)alive.hp;
                    }
                    else if ((int)alive.hp == num)
                        list.Add(alive);
                }
                if (list.Count > 0)
                    healTarget = RandomUtil.SelectOne<BattleUnitModel>(list);
                return healTarget;
            }

            public int GetMaxHP()
            {
                float maxHp = 0.0f;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                {
                    if ((double)alive.hp > (double)maxHp)
                        maxHp = alive.hp;
                }
                return (int)maxHp;
            }
        }
    }

    public class PickUpModel_LumberJack : PickUpModelBase
    {
        public PickUpModel_LumberJack()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LumberJack_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LumberJack_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370171));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370172));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370173));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_LumberJack0 : CreaturePickUpModel
    {
        public PickUpModel_LumberJack0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370171),
      new LorId(LogLikeMod.ModId, 15370172),
      new LorId(LogLikeMod.ModId, 15370173)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_LumberJack_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_LumberJack"];
    }

    public class PickUpModel_LumberJack1 : CreaturePickUpModel
    {
        public PickUpModel_LumberJack1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LumberJack1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LumberJack1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LumberJack1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370171), (EmotionCardAbilityBase)new PickUpModel_LumberJack1.LogEmotionCardAbility_LumberJack1(), model);
        }

        public class LogEmotionCardAbility_LumberJack1 : EmotionCardAbilityBase
        {
            public const int _stack = 3;
            public int pp;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this.pp >= 3)
                    this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_LumberJack1.LogEmotionCardAbility_LumberJack1.BattleUnitBuf_Lumberjack_emotion());
                this.pp = 0;
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.pp = this._owner.PlayPoint;
            }

            public class BattleUnitBuf_Lumberjack_emotion : BattleUnitBuf
            {
                public const int _powMin = 1;
                public const int _powMax = 2;
                public SoundEffectPlayer sound;
                public GameObject aura;

                public override string keywordId => "Lumberjack_Cut";

                public override string keywordIconId => "Lumberjack_Heart";

                public int Pow => RandomUtil.Range(1, 2);

                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    base.BeforeRollDice(behavior);
                    if (!this.IsAttackDice(behavior.Detail))
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        power = this.Pow
                    });
                }

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Mind", 1f, owner.view, owner.view);
                    this.aura = fxCreatureEffect != null ? fxCreatureEffect.gameObject : (GameObject)null;
                    this.sound = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/WoodMachine_Default", true, parent: owner.view.characterRotationCenter.transform);
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }

                public override void OnDie()
                {
                    base.OnDie();
                    this.Destroy();
                }

                public override void Destroy()
                {
                    base.Destroy();
                    this.DestroyAura();
                }

                public void DestroyAura()
                {
                    if (this.aura != null)
                    {
                        GameObject.Destroy(this.aura);
                        this.aura = (GameObject)null;
                    }
                    if (!(this.sound != null))
                        return;
                    this.sound.ManualDestroy();
                    this.sound = (SoundEffectPlayer)null;
                }
            }
        }
    }

    public class PickUpModel_LumberJack2 : CreaturePickUpModel
    {
        public PickUpModel_LumberJack2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LumberJack2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LumberJack2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LumberJack2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370172), (EmotionCardAbilityBase)new PickUpModel_LumberJack2.LogEmotionCardAbility_LumberJack2(), model);
        }

        public class LogEmotionCardAbility_LumberJack2 : EmotionCardAbilityBase
        {
            public const int _dmg = 15;
            public const int _targetNum = 2;
            public int accumulatedDmg;
            public bool dmged;
            public bool killed;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this.killed || this.dmged)
                    this.Ability();
                this.accumulatedDmg = 0;
                this.killed = false;
                this.dmged = false;
            }

            public override void AfterGiveDamage(BattleUnitModel target, int dmg)
            {
                base.AfterGiveDamage(target, dmg);
                if (this.dmged)
                    return;
                this.accumulatedDmg += dmg;
                if (this.accumulatedDmg >= 15)
                {
                    this.dmged = true;
                    this.Effect();
                }
            }

            public override void OnKill(BattleUnitModel target)
            {
                base.OnKill(target);
                this.killed = true;
                this.Effect();
            }

            public void Effect()
            {
                if (!Singleton<StageController>.Instance.IsLogState())
                {
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Heart", 1f, this._owner.view, this._owner.view, 1.5f);
                    SoundEffectPlayer.PlaySound("Creature/WoodMachine_AtkStrong");
                    SoundEffectPlayer.PlaySound("Creature/Heart_Guard");
                }
                else
                {
                    this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Heart", 1.5f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/WoodMachine_AtkStrong");
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Heart_Guard");
                }
            }

            public void Ability()
            {
                List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
                battleDiceCardModelList.AddRange((IEnumerable<BattleDiceCardModel>)this._owner.allyCardDetail.GetHand());
                if (battleDiceCardModelList.Count <= 0)
                    return;
                if (this.killed)
                {
                    int num = 2;
                    while (battleDiceCardModelList.Count > 0)
                    {
                        if (num <= 0)
                            break;
                        battleDiceCardModelList.Sort((Comparison<BattleDiceCardModel>)((x, y) => y.GetCost() - x.GetCost()));
                        int targetCost = battleDiceCardModelList[0].GetCost();
                        BattleDiceCardModel battleDiceCardModel = RandomUtil.SelectOne<BattleDiceCardModel>(battleDiceCardModelList.FindAll((Predicate<BattleDiceCardModel>)(x => x.GetCost() == targetCost)));
                        battleDiceCardModelList.Remove(battleDiceCardModel);
                        battleDiceCardModel.AddBuf((BattleDiceCardBuf)new PickUpModel_LumberJack2.LogEmotionCardAbility_LumberJack2.BattleDiceCardBuf_Lumberjack_Emotion_Killed());
                        --num;
                    }
                }
                else
                {
                    if (!this.dmged)
                        return;
                    battleDiceCardModelList.Sort((Comparison<BattleDiceCardModel>)((x, y) => y.GetCost() - x.GetCost()));
                    int targetCost = battleDiceCardModelList[0].GetCost();
                    RandomUtil.SelectOne<BattleDiceCardModel>(battleDiceCardModelList.FindAll((Predicate<BattleDiceCardModel>)(x => x.GetCost() == targetCost))).AddBuf((BattleDiceCardBuf)new PickUpModel_LumberJack2.LogEmotionCardAbility_LumberJack2.BattleDiceCardBuf_Lumberjack_Emotion());
                }
            }

            public class BattleDiceCardBuf_Lumberjack_Emotion : BattleDiceCardBuf
            {
                public override void OnUseCard(BattleUnitModel owner)
                {
                    base.OnUseCard(owner);
                    this.Destroy();
                }

                public override int GetCost(int oldCost) => oldCost - 1;
            }

            public class BattleDiceCardBuf_Lumberjack_Emotion_Killed :
              PickUpModel_LumberJack2.LogEmotionCardAbility_LumberJack2.BattleDiceCardBuf_Lumberjack_Emotion
            {
                public override int GetCost(int oldCost) => 0;
            }
        }
    }

    public class PickUpModel_LumberJack3 : CreaturePickUpModel
    {
        public PickUpModel_LumberJack3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_LumberJack3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_LumberJack3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_LumberJack3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370173), (EmotionCardAbilityBase)new PickUpModel_LumberJack3.LogEmotionCardAbility_LumberJack3(), model);
        }

        public class LogEmotionCardAbility_LumberJack3 : EmotionCardAbilityBase
        {
            public const int _powMax = 2;
            public bool trigger;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                this.trigger = false;
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || !this.IsAttackDice(behavior.Detail))
                    return;
                int num1 = target.cardSlotDetail.PlayPoint - target.cardSlotDetail.ReservedPlayPoint;
                int num2 = this._owner.cardSlotDetail.PlayPoint - this._owner.cardSlotDetail.ReservedPlayPoint;
                if (num1 > num2)
                {
                    int num3 = num1;
                    if (num3 > 0)
                    {
                        if (num3 > 2)
                            num3 = 2;
                        this.trigger = true;
                        behavior.ApplyDiceStatBonus(new DiceStatBonus()
                        {
                            power = num3
                        });
                    }
                }
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target != null && this.trigger)
                {
                    target.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Bloodmeet", 2f);
                    target.battleCardResultLog?.SetCreatureEffectSound("Creature/WoodMachine_Kill");
                }
                this.trigger = false;
            }
        }
    }

    public class PickUpModel_Mountain : PickUpModelBase
    {
        public PickUpModel_Mountain()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Mountain_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Mountain_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370261));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370262));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370263));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Mountain0 : CreaturePickUpModel
    {
        public PickUpModel_Mountain0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370261),
      new LorId(LogLikeMod.ModId, 15370262),
      new LorId(LogLikeMod.ModId, 15370263)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Mountain_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Mountain"];
    }

    public class PickUpModel_Mountain1 : CreaturePickUpModel
    {
        public PickUpModel_Mountain1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Mountain1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Mountain1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Mountain1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370261), (EmotionCardAbilityBase)new PickUpModel_Mountain1.LogEmotionCardAbility_Mountain1(), model);
        }

        public class LogEmotionCardAbility_Mountain1 : EmotionCardAbilityBase
        {
            public const float _healRate = 0.2f;
            public const int _healMax = 20;
            public const float _strRate = 0.1f;
            public bool _nextWave;

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._nextWave = true;
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_Mountain1.LogEmotionCardAbility_Mountain1.BattleUnitBuf_Emotion_DanggoCreature_Healed());
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._nextWave)
                    return;
                int stack = Math.Min(3, (int)((double)this._owner.UnitData.historyInWave.healed / (double)((float)this._owner.MaxHp * 0.1f)));
                if (stack <= 0)
                    return;
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack, this._owner);
            }

            public override void OnKill(BattleUnitModel target)
            {
                base.OnKill(target);
                if (this._nextWave || target.faction == this._owner.faction)
                    return;
                this._owner.RecoverHP((int)Math.Min(20f, 0.2f * (float)this._owner.MaxHp));
                target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Meet", 2f);
                this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.KillEffect));
            }

            public void KillEffect()
            {
                CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
                Battle.CreatureEffect.CreatureEffect original1 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
                if (original1 != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if ((creatureEffect != null ? creatureEffect.gameObject.GetComponent<AutoDestruct>() : null) == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect != null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                Battle.CreatureEffect.CreatureEffect original2 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
                if (!(original2 != null))
                    return;
                Battle.CreatureEffect.CreatureEffect creatureEffect1 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                if ((creatureEffect1 != null ? creatureEffect1.gameObject.GetComponent<AutoDestruct>() : null) == null)
                {
                    AutoDestruct autoDestruct = creatureEffect1 != null ? creatureEffect1.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }

            public class BattleUnitBuf_Emotion_DanggoCreature_Healed : BattleUnitBuf
            {
                public override string keywordId => "DangoCreature_Emotion_Healed";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.SetStack();
                }

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    this.SetStack();
                }

                public override void OnRoundStartAfter()
                {
                    base.OnRoundStartAfter();
                    this.SetStack();
                }

                public override void OnRoundEndTheLast()
                {
                    base.OnRoundEndTheLast();
                    this.SetStack();
                }

                public void SetStack() => this.stack = this._owner.UnitData.historyInWave.healed;
            }
        }
    }

    public class PickUpModel_Mountain2 : CreaturePickUpModel
    {
        public PickUpModel_Mountain2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Mountain2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Mountain2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Mountain2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370262), (EmotionCardAbilityBase)new PickUpModel_Mountain2.LogEmotionCardAbility_Mountain2(), model);
        }

        public class LogEmotionCardAbility_Mountain2 : EmotionCardAbilityBase
        {
            public const float _hpRateCond = 0.5f;
            public const int _maxCnt = 3;
            public const int _bDmgMin = 2;
            public const int _bDmgMax = 5;
            public const int _bufStack = 1;
            public int cnt;

            public static int BDmg => RandomUtil.Range(2, 5);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.cnt = 0;
            }

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                if (this.cnt >= 3 || !this.CheckHP())
                    return;
                ++this.cnt;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                {
                    if (!alive.IsExtinction())
                    {
                        alive.TakeBreakDamage(PickUpModel_Mountain2.LogEmotionCardAbility_Mountain2.BDmg, DamageType.Emotion, this._owner);
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, this._owner);
                    }
                }
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Shout", 3f);
                this._owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Damaged));
            }

            public void Damaged()
            {
                CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.3f);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Danggo_Lv2_Shout");
            }

            public bool CheckHP() => (double)this._owner.hp <= (double)this._owner.MaxHp * 0.5;
        }
    }

    public class PickUpModel_Mountain3 : CreaturePickUpModel
    {
        public PickUpModel_Mountain3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Mountain3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Mountain3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Mountain3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370263), (EmotionCardAbilityBase)new PickUpModel_Mountain3.LogEmotionCardAbility_Mountain3(), model);
        }

        public class LogEmotionCardAbility_Mountain3 : EmotionCardAbilityBase
        {
            public int stack;
            public bool _effect;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction);
                aliveList.Remove(this._owner);
                this.stack = aliveList.Count;
                if (this.stack > 0)
                {
                    foreach (BattleUnitModel battleUnitModel in aliveList)
                        battleUnitModel.Die();
                    this._owner.cardSlotDetail.SetRecoverPoint(this._owner.cardSlotDetail.GetRecoverPlayPoint() + this.stack);
                }
                Singleton<StageController>.Instance.GetStageModel().danggoUsed = true;
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._owner.cardSlotDetail.SetRecoverPoint(this._owner.cardSlotDetail.GetRecoverPlayPoint() + this.stack);
                this.MakeEffect("6/Dango_Emotion_Spread", target: this._owner);
            }

            public override int MaxPlayPointAdder() => this.stack;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (!this._effect)
                {
                    this._effect = true;
                    CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
                    Battle.CreatureEffect.CreatureEffect original1 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
                    if (original1 != null)
                    {
                        Battle.CreatureEffect.CreatureEffect creatureEffect1 = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                        Battle.CreatureEffect.CreatureEffect creatureEffect2 = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                        Battle.CreatureEffect.CreatureEffect creatureEffect3 = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                        if ((creatureEffect1 != null ? creatureEffect1.gameObject.GetComponent<AutoDestruct>() : null) == null)
                        {
                            AutoDestruct autoDestruct = creatureEffect1 != null ? creatureEffect1.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                            if (autoDestruct != null)
                            {
                                autoDestruct.time = 3f;
                                autoDestruct.DestroyWhenDisable();
                            }
                        }
                        if ((creatureEffect2 != null ? creatureEffect2.gameObject.GetComponent<AutoDestruct>() : null) == null)
                        {
                            AutoDestruct autoDestruct = creatureEffect2 != null ? creatureEffect2.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                            if (autoDestruct != null)
                            {
                                autoDestruct.time = 3f;
                                autoDestruct.DestroyWhenDisable();
                            }
                        }
                        if ((creatureEffect3 != null ? creatureEffect3.gameObject.GetComponent<AutoDestruct>() : null) == null)
                        {
                            AutoDestruct autoDestruct = creatureEffect3 != null ? creatureEffect3.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                            if (autoDestruct != null)
                            {
                                autoDestruct.time = 3f;
                                autoDestruct.DestroyWhenDisable();
                            }
                        }
                    }
                    Battle.CreatureEffect.CreatureEffect original2 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
                    if (original2 != null)
                    {
                        Battle.CreatureEffect.CreatureEffect creatureEffect4 = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                        Battle.CreatureEffect.CreatureEffect creatureEffect5 = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                        Battle.CreatureEffect.CreatureEffect creatureEffect6 = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                        if ((creatureEffect4 != null ? creatureEffect4.gameObject.GetComponent<AutoDestruct>() : null) == null)
                        {
                            AutoDestruct autoDestruct = creatureEffect4 != null ? creatureEffect4.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                            if (autoDestruct != null)
                            {
                                autoDestruct.time = 3f;
                                autoDestruct.DestroyWhenDisable();
                            }
                        }
                        if ((creatureEffect5 != null ? creatureEffect5.gameObject.GetComponent<AutoDestruct>() : null) == null)
                        {
                            AutoDestruct autoDestruct = creatureEffect5 != null ? creatureEffect5.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                            if (autoDestruct != null)
                            {
                                autoDestruct.time = 3f;
                                autoDestruct.DestroyWhenDisable();
                            }
                        }
                        if ((creatureEffect6 != null ? creatureEffect6.gameObject.GetComponent<AutoDestruct>() : null) == null)
                        {
                            AutoDestruct autoDestruct = creatureEffect6 != null ? creatureEffect6.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                            if (autoDestruct != null)
                            {
                                autoDestruct.time = 3f;
                                autoDestruct.DestroyWhenDisable();
                            }
                        }
                    }
                    this.MakeEffect("6/Dango_Emotion_Spread", target: this._owner);
                    SoundEffectPlayer.PlaySound("Creature/Danggo_LvUp");
                    SoundEffectPlayer.PlaySound("Creature/Danggo_Birth");
                }
                if (this.stack <= 0)
                    return;
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this.stack, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, this.stack, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, this.stack, this._owner);
            }
        }
    }

    public class PickUpModel_Nosferatu : PickUpModelBase
    {
        public PickUpModel_Nosferatu()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Nosferatu_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Nosferatu_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370361));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370362));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370363));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 4
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Nosferatu0 : CreaturePickUpModel
    {
        public PickUpModel_Nosferatu0()
        {
            this.level = 4;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370361),
      new LorId(LogLikeMod.ModId, 15370362),
      new LorId(LogLikeMod.ModId, 15370363)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_Nosferatu_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Nosferatu"];
    }

    public class PickUpModel_Nosferatu1 : CreaturePickUpModel
    {
        public PickUpModel_Nosferatu1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Nosferatu1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Nosferatu1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Nosferatu1_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370361), (EmotionCardAbilityBase)new PickUpModel_Nosferatu1.LogEmotionCardAbility_Nosferatu1(), model);
        }

        public class LogEmotionCardAbility_Nosferatu1 : EmotionCardAbilityBase
        {
            public const int _bleed = 1;
            public const int _blood = 1;

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null)
                    return;
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1, this._owner);
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Nosferatu_Emotion_Blood)) is BattleUnitBuf_Nosferatu_Emotion_Blood nosferatuEmotionBlood)
                    nosferatuEmotionBlood.Add();
                target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_BloodDrain");
                target.battleCardResultLog?.SetCreatureEffectSound("Nosferatu_Changed_BloodEat");
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new BattleUnitBuf_Nosferatu_Emotion_Blood());
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new BattleUnitBuf_Nosferatu_Emotion_Blood());
            }
        }
    }

    public class PickUpModel_Nosferatu2 : CreaturePickUpModel
    {
        public PickUpModel_Nosferatu2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Nosferatu2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Nosferatu2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Nosferatu2_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370362), (EmotionCardAbilityBase)new PickUpModel_Nosferatu2.LogEmotionCardAbility_Nosferatu2(), model);
        }

        public class LogEmotionCardAbility_Nosferatu2 : EmotionCardAbilityBase
        {
            public const int _heal = 10;
            public bool _trigger;

            public override void OnKill(BattleUnitModel target)
            {
                base.OnKill(target);
                if (target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding) == null)
                    return;
                target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_TeathATK");
                target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_BloodDrain");
                target.battleCardResultLog?.SetCreatureEffectSound("Creature/Nosferatu_Change");
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                    alive.RecoverHP(10);
                this._trigger = true;
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._trigger && this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Nosferatu_Emotion_Blood)) is BattleUnitBuf_Nosferatu_Emotion_Blood nosferatuEmotionBlood)
                    nosferatuEmotionBlood.StackToMax();
                this._trigger = false;
            }
        }
    }

    public class PickUpModel_Nosferatu3 : CreaturePickUpModel
    {
        public PickUpModel_Nosferatu3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Nosferatu3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Nosferatu3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Nosferatu3_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370363), (EmotionCardAbilityBase)new PickUpModel_Nosferatu3.LogEmotionCardAbility_Nosferatu3(), model);
        }

        public class LogEmotionCardAbility_Nosferatu3 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 2;
            public const int _dmgMax = 4;
            public const int _healMin = 2;
            public const int _healMax = 5;

            public int Dmg => RandomUtil.Range(2, 4);

            public int Heal => RandomUtil.Range(2, 5);

            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                base.OnRollDice(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) <= 0)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = this.Dmg
                });
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) <= 0)
                    return;
                target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_Bat", 3f);
                target.battleCardResultLog?.SetCreatureEffectSound("Nosferatu_Atk_Bat");
                this._owner.RecoverHP(this.Heal);
            }
        }
    }

    public class PickUpModel_Ozma : PickUpModelBase
    {
        public PickUpModel_Ozma()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Ozma_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Ozma_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370371));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370372));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370373));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 4
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Ozma0 : CreaturePickUpModel
    {
        public PickUpModel_Ozma0()
        {
            this.level = 4;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370371),
      new LorId(LogLikeMod.ModId, 15370372),
      new LorId(LogLikeMod.ModId, 15370373)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Ozma_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Ozma"];
    }

    public class PickUpModel_Ozma1 : CreaturePickUpModel
    {
        public PickUpModel_Ozma1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Ozma1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Ozma1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Ozma1_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370371), (EmotionCardAbilityBase)new PickUpModel_Ozma1.LogEmotionCardAbility_Ozma1(), model);
        }

        public class LogEmotionCardAbility_Ozma1 : EmotionCardAbilityBase
        {
            public bool _effect;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._effect)
                    return;
                this._effect = true;
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Jack_Start", 1f, this._owner.view, this._owner.view, 2f);
                SoundEffectPlayer.PlaySound("Creature/Ozma_RealPumkin_GetCard");
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                int num = RandomUtil.Range(3, 4);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num
                });
                if (!this.IsAttackDice(behavior.Detail))
                {
                    if (!this.IsDefenseDice(behavior.Detail))
                        return;
                    this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Jack_G", 2f);
                }
                else
                    this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Jack_P", 2f);
            }

            public override int GetCardCostAdder(BattleDiceCardModel card) => 1;
        }
    }

    public class PickUpModel_Ozma2 : CreaturePickUpModel
    {
        public PickUpModel_Ozma2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Ozma2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Ozma2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Ozma2_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370372), (EmotionCardAbilityBase)new PickUpModel_Ozma2.LogEmotionCardAbility_Ozma2(), model);
        }

        public class LogEmotionCardAbility_Ozma2 : EmotionCardAbilityBase
        {
            public const int _id = 1100014;

            public override void OnSelectEmotion() => this._owner.allyCardDetail.AddNewCard(1100014);

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this._owner.allyCardDetail.AddNewCardToDeck(1100014);
            }
        }
    }

    public class PickUpModel_Ozma3 : CreaturePickUpModel
    {
        public PickUpModel_Ozma3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Ozma3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Ozma3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Ozma3_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370373), (EmotionCardAbilityBase)new PickUpModel_Ozma3.LogEmotionCardAbility_Ozma3(), model);
        }

        public class LogEmotionCardAbility_Ozma3 : EmotionCardAbilityBase
        {
            public bool _activated;
            public bool _effect;

            public override void OnRoundStart()
            {
                if (!this._effect)
                {
                    this._effect = true;
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Particle", 1f, this._owner.view, this._owner.view, 3f);
                    SoundEffectPlayer.PlaySound("CreatureOzma_FarAtk");
                }
                if (this._activated)
                {
                    this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_Ozma3.LogEmotionCardAbility_Ozma3.BattleUnitBuf_ozmaReviveCheck))?.Destroy();
                }
                else
                {
                    if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_Ozma3.LogEmotionCardAbility_Ozma3.BattleUnitBuf_ozmaReviveCheck)) != null)
                        return;
                    this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_Ozma3.LogEmotionCardAbility_Ozma3.BattleUnitBuf_ozmaReviveCheck());
                }
            }

            public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
            {
                if (this._activated || (double)this._owner.hp > (double)dmg)
                    return false;
                this._activated = true;
                this._owner.RecoverHP(Mathf.Min(24, (int)((double)this._owner.MaxHp * 0.20000000298023224)));
                if (Singleton<StageController>.Instance.IsLogState())
                {
                    this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Particle", 3f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("CreatureOzma_FarAtk");
                }
                else
                {
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Particle", 1f, this._owner.view, this._owner.view, 3f);
                    SoundEffectPlayer.PlaySound("CreatureOzma_FarAtk");
                }
                return true;
            }

            public class BattleUnitBuf_ozmaReviveCheck : BattleUnitBuf
            {
                public override string keywordId => "Ozma_revive";

                public override string keywordIconId => "Ozma_AwakenPumpkin";

                public BattleUnitBuf_ozmaReviveCheck() => this.stack = 0;
            }
        }
    }

    public class PickUpModel_Pinocchio : PickUpModelBase
    {
        public PickUpModel_Pinocchio()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Pinocchio_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Pinocchio_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370201));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370202));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370203));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Pinocchio0 : CreaturePickUpModel
    {
        public PickUpModel_Pinocchio0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370201),
      new LorId(LogLikeMod.ModId, 15370202),
      new LorId(LogLikeMod.ModId, 15370203)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_Pinocchio_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Pinocchio"];
    }

    public class PickUpModel_Pinocchio1 : CreaturePickUpModel
    {
        public PickUpModel_Pinocchio1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Pinocchio1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Pinocchio1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Pinocchio1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370201), (EmotionCardAbilityBase)new PickUpModel_Pinocchio1.LogEmotionCardAbility_Pinocchio1(), model);
        }

        public class LogEmotionCardAbility_Pinocchio1 : EmotionCardAbilityBase
        {
            public override void OnWaveStart()
            {
                this._owner.allyCardDetail.AddNewCardToDeck(this.GetTargetCardId());
            }

            public override void OnSelectEmotion()
            {
                this._owner.allyCardDetail.AddNewCard(this.GetTargetCardId());
                SoundEffectPlayer.PlaySound("Creature/Pino_On");
            }

            public int GetTargetCardId()
            {
                int targetCardId = 1100001;
                if (this._owner.Book.ClassInfo.RangeType == EquipRangeType.Range)
                    targetCardId = 1100002;
                return targetCardId;
            }
        }
    }

    public class PickUpModel_Pinocchio2 : CreaturePickUpModel
    {
        public PickUpModel_Pinocchio2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Pinocchio2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Pinocchio2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Pinocchio2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370202), (EmotionCardAbilityBase)new PickUpModel_Pinocchio2.LogEmotionCardAbility_Pinocchio2(), model);
        }

        public class LogEmotionCardAbility_Pinocchio2 : EmotionCardAbilityBase
        {
            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                SoundEffectPlayer.PlaySound("Creature/Pino_Lie");
                this.SetFilter("0/Pinocchio_Emotion_Select");
            }

            public override void OnRoundStart()
            {
                foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
                {
                    if (battleDiceCardModel.GetOriginCost() <= 3)
                        battleDiceCardModel.AddBufWithoutDuplication((BattleDiceCardBuf)new EmotionCardAbility_pinocchio2.RandomCostBuf());
                }
            }

            public class RandomCostBuf : BattleDiceCardBuf
            {
                public int _cost;

                public override DiceCardBufType bufType => DiceCardBufType.Mirror;

                public RandomCostBuf() => this._cost = RandomUtil.Range(0, 3);

                public override int GetCost(int oldCost) => this._cost;
            }
        }
    }

    public class PickUpModel_Pinocchio3 : CreaturePickUpModel
    {
        public PickUpModel_Pinocchio3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Pinocchio3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Pinocchio3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Pinocchio3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370203), (EmotionCardAbilityBase)new PickUpModel_Pinocchio3.LogEmotionCardAbility_Pinocchio3(), model);
        }

        public class LogEmotionCardAbility_Pinocchio3 : EmotionCardAbilityBase
        {
            public const int _draw = 4;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Pino_Success");
                if (soundEffectPlayer == null)
                    return;
                soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this._owner.allyCardDetail.ReturnAllToDeck();
                this._owner.allyCardDetail.DrawCards(4);
                this.MakeEffect("0/Pinocchio_Curiosity", destroyTime: 3f);
            }
        }
    }

    public class PickUpModel_Porccubus : PickUpModelBase
    {
        public PickUpModel_Porccubus()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Porccubus_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Porccubus_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370241));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370242));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370243));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Porccubus0 : CreaturePickUpModel
    {
        public PickUpModel_Porccubus0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370241),
      new LorId(LogLikeMod.ModId, 15370242),
      new LorId(LogLikeMod.ModId, 15370243)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_Porccubus_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Porccubus"];
    }

    public class PickUpModel_Porccubus1 : CreaturePickUpModel
    {
        public PickUpModel_Porccubus1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Porccubus1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Porccubus1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Porccubus1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370241), (EmotionCardAbilityBase)new PickUpModel_Porccubus1.LogEmotionCardAbility_Porccubus1(), model);
        }

        public class LogEmotionCardAbility_Porccubus1 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 2;
            public const int _dmgMax = 7;
            public const int _brkDmgMin = 2;
            public const int _brkDmgMax = 7;

            public static int Dmg => RandomUtil.Range(2, 7);

            public static int BrkDmg => RandomUtil.Range(2, 7);

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                if (!this.IsAttackDice(behavior.Detail) || behavior.Detail != BehaviourDetail.Penetrate)
                    return;
                BattleUnitModel target = behavior.card?.target;
                if (target != null)
                {
                    BattleUnitBuf battleUnitBuf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_porccubus.BattleUnitBuf_Emotion_Porccubus_Happy));
                    if (battleUnitBuf != null)
                    {
                        if ((battleUnitBuf as EmotionCardAbility_porccubus.BattleUnitBuf_Emotion_Porccubus_Happy).Add())
                        {
                            target.TakeDamage(PickUpModel_Porccubus1.LogEmotionCardAbility_Porccubus1.Dmg, DamageType.Emotion, this._owner);
                            target.TakeBreakDamage(PickUpModel_Porccubus1.LogEmotionCardAbility_Porccubus1.BrkDmg, DamageType.Emotion, this._owner);
                            target.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.BloodFilter));
                        }
                        else
                        {
                            target.battleCardResultLog?.SetCreatureAbilityEffect("3/Porccubuss_Delight", 1f);
                            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Penetrate");
                        }
                    }
                    else
                    {
                        target.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_porccubus.BattleUnitBuf_Emotion_Porccubus_Happy());
                        target.battleCardResultLog?.SetCreatureAbilityEffect("3/Porccubuss_Delight", 1f);
                        target.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Penetrate");
                    }
                }
            }

            public void BloodFilter()
            {
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if ((creatureEffect != null ? creatureEffect.gameObject.GetComponent<AutoDestruct>() : null) == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect != null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                SoundEffectPlayer.PlaySound("Creature/Porccu_Special");
            }

            public void Filter()
            {
                new GameObject().AddComponent<SpriteFilter_Porccubus_Special>().Init("EmotionCardFilter/Porccubus_Filter", false, 1f);
            }

            public class BattleUnitBuf_Emotion_Porccubus_Happy : BattleUnitBuf
            {
                public const int _fullStack = 3;

                public override string keywordId => "Porccubus_Happy";

                public bool Add()
                {
                    ++this.stack;
                    if (this.stack < 3)
                        return false;
                    this.stack = 0;
                    return true;
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    if (this.stack > 0)
                        return;
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_Porccubus2 : CreaturePickUpModel
    {
        public PickUpModel_Porccubus2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Porccubus2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Porccubus2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Porccubus2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370242), (EmotionCardAbilityBase)new PickUpModel_Porccubus2.LogEmotionCardAbility_Porccubus2(), model);
        }

        public class LogEmotionCardAbility_Porccubus2 : EmotionCardAbilityBase
        {
            public override StatBonus GetStatBonus()
            {
                return new StatBonus() { breakRate = -50 };
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = 1
                });
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this.Filter();
            }

            public void Filter()
            {
                new GameObject().AddComponent<SpriteFilter_Porccubus>().Init("EmotionCardFilter/Porccubus_Filter", false);
                SoundEffectPlayer.PlaySound("Creature/Porccu_Nodmg");
            }
        }
    }

    public class PickUpModel_Porccubus3 : CreaturePickUpModel
    {
        public PickUpModel_Porccubus3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Porccubus3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Porccubus3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Porccubus3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370243), (EmotionCardAbilityBase)new PickUpModel_Porccubus3.LogEmotionCardAbility_Porccubus3(), model);
        }

        public class LogEmotionCardAbility_Porccubus3 : EmotionCardAbilityBase
        {
            public const int _recoverBpMin = 2;
            public const int _recoverBpMax = 4;

            public static int RecoverBP => RandomUtil.Range(2, 4);

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                this._owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
                if (this._owner.IsBreakLifeZero())
                    return;
                int recoverBp = PickUpModel_Porccubus3.LogEmotionCardAbility_Porccubus3.RecoverBP;
                this._owner.battleCardResultLog?.SetEmotionAbility(false, this._emotionCard, 0, ResultOption.Default);
                this._owner.breakDetail.RecoverBreak(recoverBp);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Hit");
            }

            public void Filter()
            {
                new GameObject().AddComponent<SpriteFilter_Porccubus>().Init("EmotionCardFilter/Porccubus_Filter", false);
            }
        }
    }

    public class PickUpModel_QueenBee : PickUpModelBase
    {
        public PickUpModel_QueenBee()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_QueenBee_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_QueenBee_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370311));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370312));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370313));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 4
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_QueenBee0 : CreaturePickUpModel
    {
        public PickUpModel_QueenBee0()
        {
            this.level = 4;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370311),
      new LorId(LogLikeMod.ModId, 15370312),
      new LorId(LogLikeMod.ModId, 15370313)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_QueenBee_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_QueenBee"];
    }

    public class PickUpModel_QueenBee1 : CreaturePickUpModel
    {
        public PickUpModel_QueenBee1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_QueenBee1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_QueenBee1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenBee1_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370311), (EmotionCardAbilityBase)new PickUpModel_QueenBee1.LogEmotionCardAbility_QueenBee1(), model);
        }

        public class LogEmotionCardAbility_QueenBee1 : EmotionCardAbilityBase
        {
            public const int _bleedMin = 1;
            public const int _bleedMax = 3;
            public const int _burnMin = 1;
            public const int _burnMax = 3;

            public static int Bleed => RandomUtil.Range(1, 3);

            public static int Burn => RandomUtil.Range(1, 3);

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel owner = atkDice.owner;
                if (owner == null)
                    return;
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, PickUpModel_QueenBee1.LogEmotionCardAbility_QueenBee1.Bleed, this._owner);
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, PickUpModel_QueenBee1.LogEmotionCardAbility_QueenBee1.Burn, this._owner);
                owner.battleCardResultLog?.SetCreatureEffectSound("Creature/QueenBee_Funga");
                this._owner.battleCardResultLog?.SetCreatureAbilityEffect("1/Queenbee_Spore", 2f);
            }
        }
    }

    public class PickUpModel_QueenBee2 : CreaturePickUpModel
    {
        public PickUpModel_QueenBee2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_QueenBee2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_QueenBee2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenBee2_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370312), (EmotionCardAbilityBase)new PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2(), model);
        }

        public class LogEmotionCardAbility_QueenBee2 : EmotionCardAbilityBase
        {
            public Dictionary<BattleUnitModel, int> dmgData = new Dictionary<BattleUnitModel, int>();

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel owner = atkDice.owner;
                if (owner == null || owner.faction != Faction.Enemy)
                    return;
                if (!this.dmgData.ContainsKey(owner))
                    this.dmgData.Add(owner, dmg);
                else
                    this.dmgData[owner] += dmg;
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this.dmgData.Count > 0)
                {
                    int num = 0;
                    BattleUnitModel battleUnitModel = (BattleUnitModel)null;
                    foreach (KeyValuePair<BattleUnitModel, int> keyValuePair in this.dmgData)
                    {
                        if (keyValuePair.Value > num && !keyValuePair.Key.IsDead())
                        {
                            num = keyValuePair.Value;
                            battleUnitModel = keyValuePair.Key;
                        }
                    }
                    if (battleUnitModel != null)
                    {
                        battleUnitModel.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2.BattleUnitBuf_queenbee_punish());
                        SoundEffectPlayer.PlaySound("Creature/QueenBee_AtkMode");
                    }
                }
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                {
                    if (alive != this._owner)
                        alive.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2.BattleUnitBuf_queenbee_attacker());
                }
                this.dmgData.Clear();
            }

            public class BattleUnitBuf_queenbee_punish : BattleUnitBuf
            {
                public Battle.CreatureEffect.CreatureEffect _aura;

                public override string keywordId => "Queenbee_Punish";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("1_M/FX_IllusionCard_1_M_BeeMark", 1f, owner.view, owner.view);
                }

                public override void OnDie()
                {
                    base.OnDie();
                    this.Destroy();
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }

                public override void Destroy()
                {
                    base.Destroy();
                    this.DestroyAura();
                }

                public void DestroyAura()
                {
                    if (!(this._aura != null))
                        return;
                    UnityEngine.Object.Destroy(this._aura.gameObject);
                    this._aura = (Battle.CreatureEffect.CreatureEffect)null;
                }
            }

            public class BattleUnitBuf_queenbee_attacker : BattleUnitBuf
            {
                public const int _dmgMin = 2;
                public const int _dmgMax = 4;

                public static int Dmg => RandomUtil.Range(2, 4);

                public override bool Hide => true;

                public override string keywordId => "Queenbee_Punish";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.hide = true;
                }

                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    base.BeforeRollDice(behavior);
                    BattleUnitModel target = behavior?.card?.target;
                    if (target == null || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2.BattleUnitBuf_queenbee_punish)) == null)
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2.BattleUnitBuf_queenbee_attacker.Dmg
                    });
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_QueenBee3 : CreaturePickUpModel
    {
        public PickUpModel_QueenBee3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_QueenBee3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_QueenBee3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenBee3_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370313), (EmotionCardAbilityBase)new PickUpModel_QueenBee3.LogEmotionCardAbility_QueenBee3(), model);
        }

        public class LogEmotionCardAbility_QueenBee3 : EmotionCardAbilityBase
        {
            public const int _rate = 5;
            public int _dmg;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                int stack = (int)Math.Round((double)this._dmg * 1.0 / 5.0);
                if (stack > 0)
                {
                    new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/QueenBee_Filter_Spore", false, 2f);
                    SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/QueenBee_Funga");
                    if (soundEffectPlayer != null)
                        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                        alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack);
                }
                this._dmg = 0;
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                int damageAtOneRound = this._owner.history.takeDamageAtOneRound;
                if (damageAtOneRound <= 0)
                    return;
                this._dmg = damageAtOneRound;
            }
        }
    }

    public class PickUpModel_QueenOfHatred : PickUpModelBase
    {
        public PickUpModel_QueenOfHatred()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_QueenOfHatred_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_QueenOfHatred_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370051));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370052));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370053));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_QueenOfHatred0 : CreaturePickUpModel
    {
        public PickUpModel_QueenOfHatred0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370051),
      new LorId(LogLikeMod.ModId, 15370052),
      new LorId(LogLikeMod.ModId, 15370053)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_QueenOfHatred_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_QueenOfHatred"];
    }

    public class PickUpModel_QueenOfHatred1 : CreaturePickUpModel
    {
        public PickUpModel_QueenOfHatred1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_QueenOfHatred1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_QueenOfHatred1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenOfHatred1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370051), (EmotionCardAbilityBase)new PickUpModel_QueenOfHatred1.LogEmotionCardAbility_QueenOfHatred1(), model);
        }

        public class LogEmotionCardAbility_QueenOfHatred1 : EmotionCardAbilityBase
        {
            public const int _hpMin = 3;
            public const int _hpMax = 5;
            public const int _hpTarget = 1;
            public const int _brMin = 2;
            public const int _brMax = 4;
            public const int _brTarget = 1;

            public static int RecoverHP => RandomUtil.Range(3, 5);

            public static int RecoverBreak => RandomUtil.Range(2, 4);

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MagicalGirl_kiss");
                this._owner.battleCardResultLog?.SetEmotionAbilityEffect("5/MagicalGirl_Heart");
                if (this.IsAttackDice(behavior.Detail))
                {
                    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_random(this._owner.faction, 1))
                        battleUnitModel.RecoverHP(PickUpModel_QueenOfHatred1.LogEmotionCardAbility_QueenOfHatred1.RecoverHP);
                }
                else
                {
                    if (!this.IsDefenseDice(behavior.Detail))
                        return;
                    int num = 1;
                    List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
                    List<BattleUnitModel> list = new List<BattleUnitModel>();
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                    {
                        if (!alive.IsBreakLifeZero())
                            list.Add(alive);
                    }
                    while (list.Count > 0 && num > 0)
                    {
                        BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(list);
                        list.Remove(battleUnitModel);
                        if (!battleUnitModel.IsBreakLifeZero())
                        {
                            battleUnitModelList.Add(battleUnitModel);
                            --num;
                        }
                    }
                    foreach (BattleUnitModel battleUnitModel in battleUnitModelList)
                        battleUnitModel.breakDetail.RecoverBreak(PickUpModel_QueenOfHatred1.LogEmotionCardAbility_QueenOfHatred1.RecoverBreak);
                }
            }

            public override void OnPrintEffect(BattleDiceBehavior behavior) => base.OnPrintEffect(behavior);
        }
    }

    public class PickUpModel_QueenOfHatred2 : CreaturePickUpModel
    {
        public PickUpModel_QueenOfHatred2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_QueenOfHatred2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_QueenOfHatred2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenOfHatred2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370052), (EmotionCardAbilityBase)new PickUpModel_QueenOfHatred2.LogEmotionCardAbility_QueenOfHatred2(), model);
        }

        public class LogEmotionCardAbility_QueenOfHatred2 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 3;
            public const int _dmgMax = 5;
            public BattleUnitModel target;
            public Battle.CreatureEffect.CreatureEffect effect;
            public int max;

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                BattleUnitModel battleUnitModel = (BattleUnitModel)null;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                {
                    int damageToEnemyAtRound = alive.history.damageToEnemyAtRound;
                    if (damageToEnemyAtRound > this.max)
                    {
                        battleUnitModel = alive;
                        this.max = damageToEnemyAtRound;
                    }
                }
                this.target = battleUnitModel;
                this.max = 0;
                if (!(this.effect != null))
                    return;
                GameObject.Destroy(this.effect.gameObject);
                this.effect = (Battle.CreatureEffect.CreatureEffect)null;
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this.target == null || this.target.IsDead())
                    return;
                this.effect = this.MakeEffect("5/MagicalGirl_Villain", target: this.target);
            }

            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                if (this.target != behavior.card?.target)
                    return;
                int num = RandomUtil.Range(3, 5);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = num
                });
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MagicalGirl_Gun");
                this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Pink));
            }
        }
    }

    public class PickUpModel_QueenOfHatred3 : CreaturePickUpModel
    {
        public PickUpModel_QueenOfHatred3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_QueenOfHatred3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_QueenOfHatred3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenOfHatred3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370053), (EmotionCardAbilityBase)new PickUpModel_QueenOfHatred3.LogEmotionCardAbility_QueenOfHatred3(), model);
        }

        public class LogEmotionCardAbility_QueenOfHatred3 : EmotionCardAbilityBase
        {
            public const int _strMax = 2;
            public const int _costRedCond = 3;
            public const int _bDmgMin = 2;
            public const int _bDmgMax = 4;
            public int cnt;

            public static int BDmg => RandomUtil.Range(2, 4);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.cnt = 0;
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.cnt = 0;
            }

            public override void OnLoseParrying(BattleDiceBehavior behavior)
            {
                base.OnLoseParrying(behavior);
                if (behavior == null || behavior.card?.target == null)
                    return;
                ++this.cnt;
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_HeartBroken", 2f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Oz_Atk_Boom");
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) == null)
                    this._owner.TakeBreakDamage(PickUpModel_QueenOfHatred3.LogEmotionCardAbility_QueenOfHatred3.BDmg, DamageType.Emotion, this._owner);
                if (this.cnt <= 2)
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
                if (this.cnt != 3)
                    return;
                this.ReduceCost();
            }

            public void ReduceCost()
            {
                List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
                battleDiceCardModelList.AddRange((IEnumerable<BattleDiceCardModel>)this._owner.allyCardDetail.GetHand());
                if (battleDiceCardModelList.Count <= 0)
                    return;
                battleDiceCardModelList.Sort((Comparison<BattleDiceCardModel>)((x, y) => y.GetCost() - x.GetCost()));
                int targetCost = battleDiceCardModelList[0].GetCost();
                List<BattleDiceCardModel> all = battleDiceCardModelList.FindAll((Predicate<BattleDiceCardModel>)(x => x.GetCost() == targetCost));
                if (all.Count <= 0)
                    Debug.LogError("???");
                else
                    RandomUtil.SelectOne<BattleDiceCardModel>(all).AddCost(-1);
            }
        }
    }

    public class PickUpModel_Redhood : PickUpModelBase
    {
        public PickUpModel_Redhood()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Redhood_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Redhood_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370061));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370062));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370063));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_Redhood0 : CreaturePickUpModel
    {
        public PickUpModel_Redhood0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370061),
      new LorId(LogLikeMod.ModId, 15370062),
      new LorId(LogLikeMod.ModId, 15370063)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_Redhood_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_Redhood"];
    }

    public class PickUpModel_Redhood1 : CreaturePickUpModel
    {
        public PickUpModel_Redhood1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Redhood1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Redhood1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Redhood1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370061), (EmotionCardAbilityBase)new PickUpModel_Redhood1.LogEmotionCardAbility_Redhood1(), model);
        }

        public class LogEmotionCardAbility_Redhood1 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 2;
            public const int _dmgMax = 6;
            public BattleUnitModel _target;

            public static int Dmg => RandomUtil.Range(2, 6);

            public override void OnWaveStart() => base.OnWaveStart();

            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnUseCard(curCard);
                if (this._target != null || curCard.GetDiceBehaviorList().Find((Predicate<BattleDiceBehavior>)(x => x.Type == BehaviourType.Atk)) == null)
                    return;
                this._target = curCard.target;
                this._target.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_Redhood1.LogEmotionCardAbility_Redhood1.BattleUnitBuf_redhood_prey());
                this._target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Hunted", 1.5f);
                this._target.battleCardResultLog?.SetCreatureEffectSound("Creature/RedHood_Gun");
            }

            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                BattleUnitModel target = behavior.card?.target;
                if (target == null || target != this._target)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = PickUpModel_Redhood1.LogEmotionCardAbility_Redhood1.Dmg
                });
            }

            public class BattleUnitBuf_redhood_prey : BattleUnitBuf
            {
                public override string keywordId => "RedHood_Hunt";

                public override string keywordIconId => "Redhood_Target";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }
            }
        }
    }

    public class PickUpModel_Redhood2 : CreaturePickUpModel
    {
        public PickUpModel_Redhood2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Redhood2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Redhood2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Redhood2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370062), (EmotionCardAbilityBase)new PickUpModel_Redhood2.LogEmotionCardAbility_Redhood2(), model);
        }

        public class LogEmotionCardAbility_Redhood2 : EmotionCardAbilityBase
        {
            public const float _strRate = 0.2f;
            public const int _stackMax = 2;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/RedHood_Filter", false, 2f);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedHood_Change_mad");
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (this._owner.history.takeDamageAtOneRound <= 0)
                    return;
                int x = (int)Math.Round((double)this._owner.history.takeDamageAtOneRound * 0.20000000298023224);
                if (x > 0)
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, Math.Min(x, 2), this._owner);
            }
        }
    }

    public class PickUpModel_Redhood3 : CreaturePickUpModel
    {
        public PickUpModel_Redhood3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_Redhood3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_Redhood3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_Redhood3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370063), (EmotionCardAbilityBase)new PickUpModel_Redhood3.LogEmotionCardAbility_Redhood3(), model);
        }

        public class LogEmotionCardAbility_Redhood3 : EmotionCardAbilityBase
        {
            public Battle.CreatureEffect.CreatureEffect aura;
            public string path = "6/RedHood_Emotion_Aura";

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                if (!(this.aura != null))
                    return;
                this.DestroyAura();
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.DestroyAura();
                if ((double)this._owner.hp > (double)this._owner.MaxHp * 0.5)
                    return;
                if ((double)this._owner.hp <= (double)this._owner.MaxHp * 0.34999999403953552)
                {
                    if ((double)this._owner.hp <= (double)this._owner.MaxHp * 0.25)
                        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 4, this._owner);
                    else
                        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2, this._owner);
                }
                else
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedHood_Change");
                if (this.aura == null)
                    this.aura = this.MakeEffect(this.path, target: this._owner, apply: false);
            }

            public override void OnDie(BattleUnitModel killer)
            {
                base.OnDie(killer);
                this.DestroyAura();
            }

            public void DestroyAura()
            {
                if (this.aura != null && this.aura.gameObject != null)
                    GameObject.Destroy(this.aura.gameObject);
                this.aura = (Battle.CreatureEffect.CreatureEffect)null;
            }
        }
    }

    public class PickUpModel_RedShoes : PickUpModelBase
    {
        public PickUpModel_RedShoes()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_RedShoes_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_RedShoes_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370131));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370132));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370133));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_RedShoes0 : CreaturePickUpModel
    {
        public PickUpModel_RedShoes0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370131),
      new LorId(LogLikeMod.ModId, 15370132),
      new LorId(LogLikeMod.ModId, 15370133)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName() => TextDataModel.GetText("PickUpCreature_RedShoes_Name");

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_RedShoes"];
    }

    public class PickUpModel_RedShoes1 : CreaturePickUpModel
    {
        public PickUpModel_RedShoes1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_RedShoes1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_RedShoes1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_RedShoes1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370131), (EmotionCardAbilityBase)new PickUpModel_RedShoes1.LogEmotionCardAbility_RedShoes1(), model);
        }

        public class LogEmotionCardAbility_RedShoes1 : EmotionCardAbilityBase
        {
            public int value;
            public const int _powMin = 1;
            public const int _powMax = 2;

            public override void OnRoundStart()
            {
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive.faction != this._owner.faction && (double)RandomUtil.valueForProb < 0.5)
                    {
                        alive.bufListDetail.AddBufWithoutDuplication((BattleUnitBuf)new PickUpModel_RedShoes1.LogEmotionCardAbility_RedShoes1.BattleUnitBuf_redshoes(this._emotionCard, alive, this._owner, this));
                        SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_On");
                        if (soundEffectPlayer != null)
                            soundEffectPlayer.SetGlobalPosition(alive.view.WorldPosition);
                    }
                }
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (behavior.card.target == null || behavior.card.target.bufListDetail.GetKewordBufStack(KeywordBuf.RedShoes) <= 0)
                    return;
                this.value = RandomUtil.Range(1, 2);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = this.value
                });
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                if (this.value > 0)
                    this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 1, ResultOption.Default, this.value);
                this.value = 0;
            }

            public CreatureEffect_FaceAttacher MakeFaceEffect(BattleUnitView target)
            {
                CreatureEffect_FaceAttacher effectFaceAttacher = this.MakeEffect("3/RedShoes_Attract", apply: false) as CreatureEffect_FaceAttacher;
                effectFaceAttacher.AttachTarget(target);
                return effectFaceAttacher;
            }

            public class BattleUnitBuf_redshoes : BattleUnitBuf
            {
                public BattleUnitModel _target;
                public BattleEmotionCardModel _emotionCard;
                public PickUpModel_RedShoes1.LogEmotionCardAbility_RedShoes1 _script;
                public List<CreatureEffect_FaceAttacher> _faceEffect = new List<CreatureEffect_FaceAttacher>();
                public int value;
                public const int _dmgMin = 2;
                public const int _dmgMax = 4;

                public override KeywordBuf bufType => KeywordBuf.RedShoes;

                public BattleUnitBuf_redshoes(
                  BattleEmotionCardModel emotionCard,
                  BattleUnitModel owner,
                  BattleUnitModel target,
                  PickUpModel_RedShoes1.LogEmotionCardAbility_RedShoes1 script)
                {
                    this._emotionCard = emotionCard;
                    this._target = target;
                    this._script = script;
                    try
                    {
                        Debug.Log($"Tagetting: {owner.view.charAppearance.gameObject.name} to {target.view.charAppearance.gameObject.name}");
                    }
                    catch
                    {
                        Debug.LogError("Failed to print targetting");
                    }
                }

                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    this.value = RandomUtil.Range(2, 4);
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = this.value
                    });
                }

                public override void OnRoundStart()
                {
                    CreatureEffect_FaceAttacher effectFaceAttacher = this._script.MakeFaceEffect(this._owner.view);
                    effectFaceAttacher.SetLayer("Character");
                    if (!(bool)effectFaceAttacher)
                        return;
                    this._faceEffect.Add(effectFaceAttacher);
                }

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 1, ResultOption.Default, this.value);
                }

                public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
                {
                    return this._target.IsDead() ? (BattleUnitModel)null : this._target;
                }

                public override void OnRoundEnd()
                {
                    foreach (CreatureEffect_FaceAttacher effectFaceAttacher in this._faceEffect)
                    {
                        if (effectFaceAttacher != null)
                            effectFaceAttacher.ManualDestroy();
                    }
                    this._faceEffect.Clear();
                    this.Destroy();
                }

                public override void Destroy()
                {
                    foreach (CreatureEffect_FaceAttacher effectFaceAttacher in this._faceEffect)
                    {
                        if (effectFaceAttacher != null)
                            effectFaceAttacher.ManualDestroy();
                    }
                    this._faceEffect.Clear();
                    base.Destroy();
                }

                public override void OnLayerChanged(string layerName)
                {
                    foreach (Battle.CreatureEffect.CreatureEffect creatureEffect in this._faceEffect)
                        creatureEffect.SetLayer(layerName);
                }
            }
        }
    }

    public class PickUpModel_RedShoes2 : CreaturePickUpModel
    {
        public PickUpModel_RedShoes2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_RedShoes2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_RedShoes2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_RedShoes2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp()
        {
            base.OnPickUp();
            SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370132), (EmotionCardAbilityBase)new PickUpModel_RedShoes2.LogEmotionCardAbility_RedShoes2(), model);
        }

        public class LogEmotionCardAbility_RedShoes2 : EmotionCardAbilityBase
        {
            public override void OnSelectEmotionOnce()
            {
                base.OnSelectEmotionOnce();
                SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
            }

            public override int OnGiveKeywordBufByCard(
              BattleUnitBuf buf,
              int stack,
              BattleUnitModel target)
            {
                return buf.bufType == KeywordBuf.Bleeding ? stack : 0;
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend));
            }
        }
    }

    public class PickUpModel_RedShoes3 : CreaturePickUpModel
    {
        public PickUpModel_RedShoes3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_RedShoes3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_RedShoes3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_RedShoes3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370133), (EmotionCardAbilityBase)new PickUpModel_RedShoes3.LogEmotionCardAbility_RedShoes3(), model);
        }

        public class LogEmotionCardAbility_RedShoes3 : EmotionCardAbilityBase
        {
            public const int _powMin = 1;
            public const int _powMax = 3;
            public const int _loseMin = 2;
            public const int _loseMax = 5;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.Detail != BehaviourDetail.Slash)
                    return;
                int num = RandomUtil.Range(1, 3);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num
                });
            }

            public override void OnLoseParrying(BattleDiceBehavior behavior)
            {
                base.OnLoseParrying(behavior);
                if (behavior.Detail != BehaviourDetail.Slash)
                    return;
                this._owner.breakDetail.TakeBreakDamage(RandomUtil.Range(2, 5), DamageType.Passive, this._owner);
            }

            public override void OnDrawParrying(BattleDiceBehavior behavior)
            {
                base.OnDrawParrying(behavior);
                if (behavior.Detail != BehaviourDetail.Slash)
                    return;
                this._owner.breakDetail.TakeBreakDamage(RandomUtil.Range(2, 5), DamageType.Emotion, this._owner);
            }

            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                base.OnRollDice(behavior);
                if (behavior.Detail != BehaviourDetail.Slash)
                    return;
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_SlashHit");
                if (soundEffectPlayer != null)
                    soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Red));
            }
        }
    }

    public class PickUpModel_ScareCrow : PickUpModelBase
    {
        public PickUpModel_ScareCrow()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ScareCrow_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ScareCrow_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370071));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370072));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370073));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_ScareCrow0 : CreaturePickUpModel
    {
        public PickUpModel_ScareCrow0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370071),
      new LorId(LogLikeMod.ModId, 15370072),
      new LorId(LogLikeMod.ModId, 15370073)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_ScareCrow_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_ScareCrow"];
    }

    public class PickUpModel_ScareCrow1 : CreaturePickUpModel
    {
        public PickUpModel_ScareCrow1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ScareCrow1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ScareCrow1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ScareCrow1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370071), (EmotionCardAbilityBase)new PickUpModel_ScareCrow1.LogEmotionCardAbility_ScareCrow1(), model);
        }

        public class LogEmotionCardAbility_ScareCrow1 : EmotionCardAbilityBase
        {
            public const int _discard = 1;
            public const int _ppMin = 1;
            public const int _ppMax = 1;
            public const float _filterTime = 1.5f;
            public const float _filterX = 0.5f;
            public const float _filterY = 0.5f;
            public const float _filterSpeed = 1.2f;

            public int RecoverPP => RandomUtil.Range(1, 1);

            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnUseCard(curCard);
                if (this._owner.allyCardDetail.GetHand().Count == 0)
                    return;
                this._owner.allyCardDetail.DiscardACardRandomlyByAbility(1);
                this._owner.cardSlotDetail.RecoverPlayPoint(this.RecoverPP);
                this._owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.PrintSound));
            }

            public void PrintSound()
            {
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Scarecrow_Special");
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this.SetFilter();
            }

            public void SetFilter()
            {
                GameObject gameObject = SingletonBehavior<BattleCamManager>.Instance.EffectCam.gameObject;
                if (!(gameObject != null))
                    return;
                CameraFilterPack_Distortion_ShockWave distortionShockWave = gameObject.AddComponent<CameraFilterPack_Distortion_ShockWave>();
                distortionShockWave.PosX = 0.5f;
                distortionShockWave.PosY = 0.5f;
                distortionShockWave.Speed = 1.2f;
                BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
                AutoScriptDestruct autoScriptDestruct = (instance != null ? instance.EffectCam.gameObject.AddComponent<AutoScriptDestruct>() : (AutoScriptDestruct)null) ?? (AutoScriptDestruct)null;
                if (autoScriptDestruct != null)
                {
                    autoScriptDestruct.targetScript = (MonoBehaviour)distortionShockWave;
                    autoScriptDestruct.time = 1.5f;
                }
            }
        }
    }

    public class PickUpModel_ScareCrow2 : CreaturePickUpModel
    {
        public PickUpModel_ScareCrow2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ScareCrow2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ScareCrow2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ScareCrow2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370072), (EmotionCardAbilityBase)new PickUpModel_ScareCrow2.LogEmotionCardAbility_ScareCrow2(), model);
        }

        public class LogEmotionCardAbility_ScareCrow2 : EmotionCardAbilityBase
        {
            public const int _bpMin = 2;
            public const int _bpMax = 5;
            public const int _bDmgMin = 2;
            public const int _bDmgMax = 4;
            public bool trigger;

            public int RecoverBP => RandomUtil.Range(2, 5);

            public int BDmg => RandomUtil.Range(2, 4);

            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                BattleUnitModel target = behavior.card?.target;
                if (target == null || target.breakDetail.breakGauge <= this._owner.breakDetail.breakGauge)
                    return;
                this.trigger = true;
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                if (!this.trigger)
                    return;
                this.trigger = false;
                this._owner.breakDetail.RecoverBreak(this.RecoverBP);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Scarecrow_Drink");
                behavior?.card?.target.TakeBreakDamage(this.BDmg, DamageType.Emotion, this._owner);
            }
        }
    }

    public class PickUpModel_ScareCrow3 : CreaturePickUpModel
    {
        public PickUpModel_ScareCrow3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ScareCrow3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ScareCrow3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ScareCrow3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370073), (EmotionCardAbilityBase)new PickUpModel_ScareCrow3.LogEmotionCardAbility_ScareCrow3(), model);
        }

        public class LogEmotionCardAbility_ScareCrow3 : EmotionCardAbilityBase
        {
            public override void OnMakeBreakState(BattleUnitModel target)
            {
                base.OnMakeBreakState(target);
                if (target == null || target == this._owner)
                    return;
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Scarecrow_Dead");
                List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
                switch (this._owner.Book.ClassInfo.RangeType)
                {
                    case EquipRangeType.Melee:
                        battleDiceCardModelList = target.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetSpec().Ranged == CardRange.Near));
                        break;
                    case EquipRangeType.Range:
                        battleDiceCardModelList = target.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetSpec().Ranged == CardRange.Far));
                        break;
                    case EquipRangeType.Hybrid:
                        battleDiceCardModelList = target.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetSpec().Ranged == CardRange.Near || x.GetSpec().Ranged == CardRange.Far));
                        break;
                }
                battleDiceCardModelList.RemoveAll((Predicate<BattleDiceCardModel>)(x => x.temporary));
                foreach (int index in MathUtil.Combination(2, battleDiceCardModelList.Count))
                {
                    BattleDiceCardModel battleDiceCardModel = this._owner.allyCardDetail.AddNewCard(battleDiceCardModelList[index].GetID());
                    battleDiceCardModel.SetCurrentCost(battleDiceCardModel.GetOriginCost() - 2);
                    battleDiceCardModel.XmlData.optionList.Add(CardOption.ExhaustOnUse);
                    battleDiceCardModel.AddBuf((BattleDiceCardBuf)new PickUpModel_ScareCrow3.LogEmotionCardAbility_ScareCrow3.BattleDiceCardBuf_scarecrowCreated());
                }
            }

            public class BattleDiceCardBuf_scarecrowCreated : BattleDiceCardBuf
            {
                public override void OnUseCard(BattleUnitModel owner)
                {
                    Singleton<StageController>.Instance.waveHistory.AddRakeCreatedUsed();
                }
            }
        }
    }

    public class PickUpModel_ScorchedGirl : PickUpModelBase
    {
        public PickUpModel_ScorchedGirl()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ScorchedGirl_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ScorchedGirl_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370011));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370012));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370013));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_ScorchedGirl0 : CreaturePickUpModel
    {
        public PickUpModel_ScorchedGirl0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370011),
      new LorId(LogLikeMod.ModId, 15370012),
      new LorId(LogLikeMod.ModId, 15370013)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_ScorchedGirl_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_ScorchedGirl"];
    }

    public class PickUpModel_ScorchedGirl1 : CreaturePickUpModel
    {
        public PickUpModel_ScorchedGirl1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ScorchedGirl1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ScorchedGirl1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ScorchedGirl1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370011), (EmotionCardAbilityBase)new PickUpModel_ScorchedGirl1.LogEmotionCardAbility_ScorchedGirl1(), model);
        }

        public class LogEmotionCardAbility_ScorchedGirl1 : EmotionCardAbilityBase
        {
            public const float _prob = 0.4f;
            public const int _burnMin = 1;
            public const int _burnMax = 3;
            public bool trigger;

            public static bool Prob => (double)RandomUtil.valueForProb < 0.40000000596046448;

            public static int Burn => RandomUtil.Range(1, 3);

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (!this.trigger)
                    return;
                this.trigger = false;
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_burnningGirl.BattleUnitBuf_Emotion_BurningGirl_Burn());
            }

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                int burn = PickUpModel_ScorchedGirl1.LogEmotionCardAbility_ScorchedGirl1.Burn;
                atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, burn, this._owner);
                this._owner.battleCardResultLog?.SetCreatureAbilityEffect("1/MatchGirl_Ash", 1f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MatchGirl_Barrier");
                if (!PickUpModel_ScorchedGirl1.LogEmotionCardAbility_ScorchedGirl1.Prob)
                    return;
                this.trigger = true;
            }

            public class BattleUnitBuf_Emotion_BurningGirl_Burn : BattleUnitBuf
            {
                public const int _burnMin = 1;
                public const int _burnMax = 1;

                public override string keywordId => "BurningGirl_Burn";

                public override string keywordIconId => "Burning_Match";

                public static int Burn => RandomUtil.Range(1, 1);

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    base.OnSuccessAttack(behavior);
                    behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, PickUpModel_ScorchedGirl1.LogEmotionCardAbility_ScorchedGirl1.BattleUnitBuf_Emotion_BurningGirl_Burn.Burn, this._owner);
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_ScorchedGirl2 : CreaturePickUpModel
    {
        public PickUpModel_ScorchedGirl2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ScorchedGirl2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ScorchedGirl2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ScorchedGirl2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370012), (EmotionCardAbilityBase)new PickUpModel_ScorchedGirl2.LogEmotionCardAbility_ScorchedGirl2(), model);
        }

        public class LogEmotionCardAbility_ScorchedGirl2 : EmotionCardAbilityBase
        {
            public const int _burnMin = 1;
            public const int _burnMax = 3;
            public const float _hpRate = 0.2f;
            public const float _dmgRate = 0.3f;
            public const int _maxDmg = 36;
            public Battle.CreatureEffect.CreatureEffect _effect;

            public static int Burn => RandomUtil.Range(1, 3);

            public override void OnStartCardAction(BattlePlayingCardDataInUnitModel curCard)
            {
                if ((double)this._owner.hp > (double)this._owner.MaxHp * 0.20000000298023224)
                    return;
                int v = Mathf.Min((int)((double)this._owner.MaxHp * 0.30000001192092896), 36);
                BattleUnitModel target = curCard?.target;
                target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, PickUpModel_ScorchedGirl2.LogEmotionCardAbility_ScorchedGirl2.Burn, this._owner);
                target?.TakeDamage(v, DamageType.Emotion);
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/MatchGirl_Explosion");
                if (soundEffectPlayer != null)
                    soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                this._effect = this.MakeEffect("1/MatchGirl_Footfall", destroyTime: 2f, apply: false);
                this._effect.AttachEffectLayer();
                this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.RemoveUI));
                try
                {
                    this._owner.view.StartDeadEffect(false);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
                this._owner.Die();
            }

            public void RemoveUI() => this._owner.view.unitBottomStatUI.EnableCanvas(false);

            public override void OnPrintEffect(BattleDiceBehavior behavior)
            {
                this._effect = (Battle.CreatureEffect.CreatureEffect)null;
            }

            public override void OnSelectEmotion() => SoundEffectPlayer.PlaySound("Creature/MatchGirl_Cry");
        }
    }

    public class PickUpModel_ScorchedGirl3 : CreaturePickUpModel
    {
        public PickUpModel_ScorchedGirl3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ScorchedGirl3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ScorchedGirl3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ScorchedGirl3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370013), (EmotionCardAbilityBase)new PickUpModel_ScorchedGirl3.LogEmotionCardAbility_ScorchedGirl3(), model);
        }

        public class LogEmotionCardAbility_ScorchedGirl3 : EmotionCardAbilityBase
        {
            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_burnningGirl3.BattleUnitBuf_burningGirl_Ember_New());
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                BattleUnitBuf battleUnitBuf = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_burnningGirl3.BattleUnitBuf_burningGirl_Ember_New));
                if (battleUnitBuf == null)
                {
                    this._owner.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_burnningGirl3.BattleUnitBuf_burningGirl_Ember_New());
                }
                else
                {
                    List<LorId> lorIdList = battleUnitBuf is EmotionCardAbility_burnningGirl3.BattleUnitBuf_burningGirl_Ember_New burningGirlEmberNew ? burningGirlEmberNew.TargetIDs() : (List<LorId>)null;
                    foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
                    {
                        if (lorIdList.Contains(battleDiceCardModel.GetID()))
                        {
                            battleDiceCardModel.AddBuf((BattleDiceCardBuf)new EmotionCardAbility_burnningGirl3.BattleDiceCardBuf_Emotion_BurningGirl());
                            battleDiceCardModel.SetAddedIcon("Burning_Match", -1);
                        }
                    }
                }
            }

            public class BattleUnitBuf_burningGirl_Ember_New : BattleUnitBuf
            {
                public const float _prob = 0.25f;
                public const int _triggerStack = 4;
                public bool _triggered;
                public int _max;
                public List<LorId> _targetIDs = new List<LorId>();

                public override string keywordId => "BurningGirl_Ember";

                public override string keywordIconId => "Burning_Match";

                public BattleUnitBuf_burningGirl_Ember_New() => this.stack = 0;

                public override void AfterDiceAction(BattleDiceBehavior behavior)
                {
                    base.AfterDiceAction(behavior);
                    if (!this.CheckCondition(behavior))
                        return;
                    if (this.stack >= 4 && (double)RandomUtil.valueForProb < 0.25)
                    {
                        this._triggered = true;
                        this._max = behavior.GetDiceMax();
                        this._owner.TakeDamage(this._max, DamageType.Buf);
                    }
                    else
                        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MatchGirl_NoBoom");
                }

                public override void BeforeRollDice(BattleDiceBehavior behavior)
                {
                    base.BeforeRollDice(behavior);
                    if (!this.CheckCondition(behavior))
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        max = this.stack
                    });
                }

                public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
                {
                    base.OnUseCard(card);
                    if (card == null || card.isKeepedCard)
                        return;
                    if (this._targetIDs.Count < 2)
                    {
                        this._targetIDs.Add(card.card.GetID());
                        foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
                        {
                            if (battleDiceCardModel.GetID() == card.card.GetID())
                            {
                                battleDiceCardModel.AddBuf((BattleDiceCardBuf)new EmotionCardAbility_burnningGirl3.BattleDiceCardBuf_Emotion_BurningGirl());
                                battleDiceCardModel.SetAddedIcon("Burning_Match");
                            }
                        }
                    }
                    if (!this._targetIDs.Contains(card.card.GetID()))
                        return;
                    ++this.stack;
                }

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    base.OnSuccessAttack(behavior);
                    if (!this.CheckCondition(behavior))
                        return;
                    int stack = this.stack;
                    if (stack > 0)
                        behavior.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, stack, this._owner);
                }

                public override void OnRollDiceInRecounter()
                {
                    base.OnRollDiceInRecounter();
                    if (!this._triggered)
                        return;
                    SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("1/MatchGirl_Footfall", 1f, this._owner.view, (BattleUnitView)null, 2f).AttachEffectLayer();
                    SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/MatchGirl_Explosion");
                    if (soundEffectPlayer != null)
                        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                    this._triggered = false;
                }

                public bool CheckCondition(BattleDiceBehavior behavior)
                {
                    BattlePlayingCardDataInUnitModel card = behavior?.card;
                    return card != null && this._targetIDs.Contains(card.card.GetID()) && this.IsFirstDice(behavior);
                }

                public bool IsFirstDice(BattleDiceBehavior behavior)
                {
                    return behavior != null && behavior.Index == 0;
                }

                public List<LorId> TargetIDs() => this._targetIDs;
            }

            public class BattleDiceCardBuf_Emotion_BurningGirl : BattleDiceCardBuf
            {
                public override string keywordIconId => "Burning_Match";
            }
        }
    }

    public class PickUpModel_ShyLookToday : PickUpModelBase
    {
        public PickUpModel_ShyLookToday()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ShyLookToday_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ShyLookToday_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370031));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370032));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370033));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_ShyLookToday0 : CreaturePickUpModel
    {
        public PickUpModel_ShyLookToday0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370031),
      new LorId(LogLikeMod.ModId, 15370032),
      new LorId(LogLikeMod.ModId, 15370033)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_ShyLookToday_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_ShyLookToday"];
    }

    public class PickUpModel_ShyLookToday1 : CreaturePickUpModel
    {
        public PickUpModel_ShyLookToday1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ShyLookToday1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ShyLookToday1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ShyLookToday1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370031), (EmotionCardAbilityBase)new PickUpModel_ShyLookToday1.LogEmotionCardAbility_ShyLookToday1(), model);
        }

        public class LogEmotionCardAbility_ShyLookToday1 : EmotionCardAbilityBase
        {
            public int _currentFace = 1;
            public float elap;
            public float freq = 1f;
            public CreatureEffect_Emotion_Face face;

            public int CurrentFace
            {
                get => this._currentFace;
                set
                {
                    this._currentFace = value;
                    this.SetFace();
                }
            }

            public void SetFace()
            {
                if (this.face == null)
                    return;
                this.face.SetFace(this.CurrentFace);
            }

            public void GenFace()
            {
                if (this.face != null)
                    GameObject.Destroy(this.face.gameObject);
                Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("3_H/FX_IllusionCard_3_H_Look", 1f, this._owner.view, this._owner.view);
                this.face = fxCreatureEffect != null ? fxCreatureEffect.GetComponent<CreatureEffect_Emotion_Face>() : (CreatureEffect_Emotion_Face)null;
                this.SetFace();
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this.CurrentFace = RandomUtil.Range(0, 4);
                this.GenFace();
            }

            public override void OnSelectEmotion()
            {
                this.CurrentFace = RandomUtil.Range(0, 4);
                this.GenFace();
                SoundEffectPlayer.PlaySound("Creature/Shy_Smile");
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                int num = 0;
                switch (this.CurrentFace)
                {
                    case 0:
                        num = -2;
                        break;
                    case 1:
                        num = -1;
                        break;
                    case 2:
                        num = 0;
                        break;
                    case 3:
                        num = 1;
                        break;
                    case 4:
                        num = 2;
                        break;
                }
                if (num == 0)
                    return;
                this._owner.battleCardResultLog?.SetEmotionAbility(false, this._emotionCard, 0, ResultOption.Sign, num);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num
                });
            }

            public override void OnFixedUpdateInWaitPhase(float delta)
            {
                this.elap += delta;
                if ((double)this.elap <= (double)this.freq)
                    return;
                this.CurrentFace = RandomUtil.Range(0, 4);
                this.elap = 0.0f;
            }
        }
    }

    public class PickUpModel_ShyLookToday2 : CreaturePickUpModel
    {
        public PickUpModel_ShyLookToday2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ShyLookToday2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ShyLookToday2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ShyLookToday2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370032), (EmotionCardAbilityBase)new PickUpModel_ShyLookToday2.LogEmotionCardAbility_ShyLookToday2(), model);
        }

        public class LogEmotionCardAbility_ShyLookToday2 : EmotionCardAbilityBase
        {
            public const int _addMin = 1;
            public const int _addMax = 2;
            public const int _dmgMin = 2;
            public const int _dmgMax = 7;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (behavior.Detail != BehaviourDetail.Guard)
                    return;
                int num1 = RandomUtil.Range(1, 2);
                int num2 = RandomUtil.Range(2, 7);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num1,
                    guardBreakAdder = num2
                });
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                if (behavior.Detail != BehaviourDetail.Guard)
                    return;
                this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/ShyLookToday_Guard", 0.8f);
            }
        }
    }

    public class PickUpModel_ShyLookToday3 : CreaturePickUpModel
    {
        public PickUpModel_ShyLookToday3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_ShyLookToday3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_ShyLookToday3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_ShyLookToday3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370033), (EmotionCardAbilityBase)new PickUpModel_ShyLookToday3.LogEmotionCardAbility_ShyLookToday3(), model);
        }

        public class LogEmotionCardAbility_ShyLookToday3 : EmotionCardAbilityBase
        {
            public Queue<bool> triggers = new Queue<bool>();
            public int count;
            public Coroutine effect;

            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnUseCard(curCard);
                this.count = 0;
                foreach (DiceBehaviour originalDiceBehavior in curCard.GetOriginalDiceBehaviorList())
                {
                    if (originalDiceBehavior.Type == BehaviourType.Def)
                        ++this.count;
                }
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.Detail != BehaviourDetail.Guard)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = this.count
                });
            }

            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                base.OnRollDice(behavior);
                if (!this.IsDefenseDice(behavior.Detail))
                    return;
                this._owner?.battleCardResultLog?.SetRolldiceEvent(new BattleCardBehaviourResult.BehaviourEvent(this.ChangeColor));
            }

            public void ChangeColorDefault()
            {
                if (this._owner.view.gameObject.activeInHierarchy && this.effect != null)
                    this._owner.view.charAppearance.StopCoroutine(this.effect);
                this.effect = this._owner.view.charAppearance.ChangeColor(Color.white, 0.75f);
            }

            public void ChangeColor()
            {
                if (this._owner.view.gameObject.activeInHierarchy && this.effect != null)
                    this._owner.view.charAppearance.StopCoroutine(this.effect);
                this.effect = this._owner.view.charAppearance.ChangeColor(new Color(1f, 0.0f, 0.0f, 1f), 0.75f, new Action(this.ChangeColorDefault));
            }

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.triggers.Clear();
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.triggers.Clear();
            }
        }
    }

    public class PickUpModel_SingingMachine : PickUpModelBase
    {
        public PickUpModel_SingingMachine()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SingingMachine_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SingingMachine_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370221));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370222));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370223));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_SingingMachine0 : CreaturePickUpModel
    {
        public PickUpModel_SingingMachine0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370221),
      new LorId(LogLikeMod.ModId, 15370222),
      new LorId(LogLikeMod.ModId, 15370223)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_SingingMachine_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_SingingMachine"];
    }

    public class PickUpModel_SingingMachine1 : CreaturePickUpModel
    {
        public PickUpModel_SingingMachine1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SingingMachine1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SingingMachine1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SingingMachine1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370221), (EmotionCardAbilityBase)new PickUpModel_SingingMachine1.LogEmotionCardAbility_SingingMachine1(), model);
        }

        public class LogEmotionCardAbility_SingingMachine1 : EmotionCardAbilityBase
        {
            public const int _dmgMin = 4;
            public const int _dmgMax = 8;
            public const int _brkDmgMin = 4;
            public const int _brkDmgMax = 8;
            public const int _heal = 15;
            public const int _bHeal = 15;

            public override void OnSelectEmotionOnce()
            {
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Singing_Atk");
                Util.LoadPrefab("Battle/CreatureCard/SingingMachineCard_play_particle", SingletonBehavior<BattleSceneRoot>.Instance.transform);
            }

            public override void OnKill(BattleUnitModel target)
            {
                base.OnKill(target);
                if (this._owner.faction != Faction.Player || target == null || target.faction != Faction.Enemy)
                    return;
                this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
                this._owner.RecoverHP(15);
                this._owner.breakDetail.RecoverBreak(15);
            }

            public void Filter()
            {
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/SingingMachine_Filter_Special", false, 2f);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/SingingMachine_Eat");
            }

            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                int num = RandomUtil.Range(4, 8);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = num
                });
            }

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                this._owner.breakDetail.TakeBreakDamage(RandomUtil.Range(4, 8), DamageType.Emotion);
            }
        }
    }

    public class PickUpModel_SingingMachine2 : CreaturePickUpModel
    {
        public PickUpModel_SingingMachine2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SingingMachine2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SingingMachine2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SingingMachine2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370222), (EmotionCardAbilityBase)new PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2(), model);
        }

        public class LogEmotionCardAbility_SingingMachine2 : EmotionCardAbilityBase
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.GetBuf();
            }

            public void GetBuf()
            {
                if (!(this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm)) is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm singingMachineRhythm))
                    this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm(1));
                else
                    ++singingMachineRhythm.stack;
            }

            public class BattleUnitBuf_Emotion_SingingMachine_Rhythm : BattleUnitBuf
            {
                public const int _brkDmgMin = 2;
                public const int _brkDmgMax = 5;
                public const int _str = 1;
                public const float _prob = 0.25f;
                public Battle.CreatureEffect.CreatureEffect _effect;
                public int reserve;

                public override string keywordId => "SingingMachine_Rhythm";

                public static int BrkDmg => RandomUtil.Range(2, 5);

                public static bool Prob => (double)RandomUtil.valueForProb < 0.25;

                public BattleUnitBuf_Emotion_SingingMachine_Rhythm(int value = 0)
                {
                    this.stack = value;
                    this.reserve = Mathf.Max(0, 1 - value);
                }

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    if (this.stack <= 0)
                        return;
                    this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, this._owner);
                }

                public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
                {
                    base.OnTakeDamageByAttack(atkDice, dmg);
                    if (this.stack <= 0)
                        return;
                    this._owner.TakeBreakDamage(PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm.BrkDmg, DamageType.Buf, this._owner);
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    if (this.stack > 0)
                        --this.stack;
                    this.stack += this.reserve;
                    this.reserve = 0;
                    if (this.stack <= 0)
                        this.Destroy();
                    else
                        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
                }

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    if (!(this._effect == null))
                        return;
                    this._effect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("4/SingingMachine_NoteAura", 1f, this._owner.view, (BattleUnitView)null);
                    Battle.CreatureEffect.CreatureEffect effect = this._effect;
                    if (effect != null)
                        effect.SetLayer("Character");
                    SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Singing_Rhythm");
                    if (soundEffectPlayer == null)
                        return;
                    soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                }

                public override void Destroy()
                {
                    if (this._effect != null)
                    {
                        UnityEngine.Object.Destroy(this._effect.gameObject);
                        this._effect = (Battle.CreatureEffect.CreatureEffect)null;
                    }
                    base.Destroy();
                }

                public override void OnLayerChanged(string layerName)
                {
                    if (!(this._effect != null))
                        return;
                    this._effect.SetLayer(layerName);
                }

                public void Reserve(int value = 1) => this.reserve += value;

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    base.OnSuccessAttack(behavior);
                    if (!PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm.Prob)
                        return;
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Singing_Rhythm");
                    this.Ability();
                }

                public void Ability()
                {
                    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Player);
                    if (aliveList.Count == 0)
                        return;
                    List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
                    foreach (BattleUnitModel battleUnitModel in aliveList)
                    {
                        if (battleUnitModel.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm)) == null)
                            battleUnitModelList.Add(battleUnitModel);
                    }
                    if (battleUnitModelList.Count > 0)
                    {
                        battleUnitModelList[UnityEngine.Random.Range(0, battleUnitModelList.Count)].bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm());
                    }
                    else
                    {
                        battleUnitModelList.AddRange((IEnumerable<BattleUnitModel>)aliveList);
                        if (battleUnitModelList.Count <= 0 || !(battleUnitModelList[UnityEngine.Random.Range(0, battleUnitModelList.Count)].bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm)) is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm singingMachineRhythm))
                            return;
                        singingMachineRhythm.Reserve();
                    }
                }
            }
        }
    }

    public class PickUpModel_SingingMachine3 : CreaturePickUpModel
    {
        public PickUpModel_SingingMachine3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SingingMachine3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SingingMachine3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SingingMachine3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370223), (EmotionCardAbilityBase)new PickUpModel_SingingMachine3.LogEmotionCardAbility_SingingMachine3(), model);
        }

        public class LogEmotionCardAbility_SingingMachine3 : EmotionCardAbilityBase
        {
            public const int _addMin = 1;
            public const int _addMax = 3;

            public static int Add => RandomUtil.Range(1, 3);

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/SingingMachine_Filter_Aura", false, 2f);
                SoundEffectPlayer.PlaySound("Creature/SingingMachine_Open");
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (this.IsAttackDice(behavior.Detail))
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        power = PickUpModel_SingingMachine3.LogEmotionCardAbility_SingingMachine3.Add
                    });
                }
                else
                {
                    if (!this.IsDefenseDice(behavior.Detail))
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        max = -300
                    });
                }
            }
        }
    }

    public class PickUpModel_SmallBird : PickUpModelBase
    {
        public PickUpModel_SmallBird()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SmallBird_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SmallBird_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370181));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370182));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370183));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 2
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_SmallBird0 : CreaturePickUpModel
    {
        public PickUpModel_SmallBird0()
        {
            this.level = 2;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370181),
      new LorId(LogLikeMod.ModId, 15370182),
      new LorId(LogLikeMod.ModId, 15370183)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_SmallBird_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_SmallBird"];
    }

    public class PickUpModel_SmallBird1 : CreaturePickUpModel
    {
        public PickUpModel_SmallBird1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SmallBird1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SmallBird1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SmallBird1_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370181), (EmotionCardAbilityBase)new PickUpModel_SmallBird1.LogEmotionCardAbility_SmallBird1(), model);
        }

        public class LogEmotionCardAbility_SmallBird1 : EmotionCardAbilityBase
        {
            public const int _powMin = 2;
            public const int _powMax = 4;
            public bool dmged;

            public static int Pow => RandomUtil.Range(2, 4);

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this.dmged = false;
            }

            public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
            {
                base.BeforeTakeDamage(attacker, dmg);
                if (this._owner.IsImmuneDmg() || this._owner.IsInvincibleHp((BattleUnitModel)null))
                    return false;
                this.dmged = true;
                return false;
            }

            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                if (this.dmged)
                    this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_SmallBird1.LogEmotionCardAbility_SmallBird1.BattleUnitBuf_Emotion_SmallBird_Punish());
                this.dmged = false;
            }

            public class BattleUnitBuf_Emotion_SmallBird_Punish : BattleUnitBuf
            {
                public bool powUp = true;
                public GameObject aura;

                public override string keywordId => "SmallBird_Punishment";

                public override string keywordIconId => "SmallBird_Emotion_Punish";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    if (this.aura == null)
                    {
                        Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Punising", 1f, this._owner.view, this._owner.view);
                        this.aura = fxCreatureEffect != null ? fxCreatureEffect.gameObject : (GameObject)null;
                    }
                    SoundEffectPlayer.PlaySound("Creature/SmallBird_StrongAtk");
                }

                public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
                {
                    base.OnUseCard(curCard);
                    if (!this.powUp)
                        return;
                    this.powUp = false;
                    curCard.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus()
                    {
                        power = PickUpModel_SmallBird1.LogEmotionCardAbility_SmallBird1.Pow
                    });
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }

                public override void OnDie()
                {
                    base.OnDie();
                    this.Destroy();
                }

                public override void Destroy()
                {
                    base.Destroy();
                    this.DestroyAura();
                }

                public void DestroyAura()
                {
                    if (!(this.aura != null))
                        return;
                    GameObject.Destroy(this.aura);
                    this.aura = (GameObject)null;
                }
            }
        }
    }

    public class PickUpModel_SmallBird2 : CreaturePickUpModel
    {
        public PickUpModel_SmallBird2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SmallBird2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SmallBird2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SmallBird2_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370182), (EmotionCardAbilityBase)new PickUpModel_SmallBird2.LogEmotionCardAbility_SmallBird2(), model);
        }

        public class LogEmotionCardAbility_SmallBird2 : EmotionCardAbilityBase
        {
            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                this.GiveBuf();
            }

            public override void OnWaveStart()
            {
                base.OnWaveStart();
                this.GiveBuf();
            }

            public void GiveBuf()
            {
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_SmallBird2.LogEmotionCardAbility_SmallBird2.BattleUnitBuf_Emotion_SmallBird_Buri)) != null)
                    return;
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_SmallBird2.LogEmotionCardAbility_SmallBird2.BattleUnitBuf_Emotion_SmallBird_Buri());
            }

            public class BattleUnitBuf_Emotion_SmallBird_Buri : BattleUnitBuf
            {
                public const int _stackMax = 10;
                public bool dmged;

                public override string keywordId => "Smallbird_Beak";

                public override string keywordIconId => "SmallBird_Emotion_Buri";

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    this.stack = 0;
                }

                public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
                {
                    base.OnTakeDamageByAttack(atkDice, dmg);
                    this.dmged = true;
                    this.stack = 0;
                }

                public override void BeforeGiveDamage(BattleDiceBehavior behavior)
                {
                    base.BeforeGiveDamage(behavior);
                    if (this.stack <= 0)
                        return;
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = this.stack
                    });
                }

                public override void OnSuccessAttack(BattleDiceBehavior behavior)
                {
                    base.OnSuccessAttack(behavior);
                    BattleUnitModel target = behavior?.card?.target;
                    if (target == null || this.stack <= 0)
                        return;
                    target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Attack", 2f);
                    target.battleCardResultLog?.SetCreatureEffectSound("Creature/SmallBird_Atk");
                }

                public override void OnRoundEndTheLast()
                {
                    base.OnRoundEndTheLast();
                    if (this.dmged)
                    {
                        this.dmged = false;
                        this.stack = 0;
                    }
                    else
                    {
                        ++this.stack;
                        if (this.stack <= 10)
                            return;
                        this.stack = 10;
                    }
                }
            }
        }
    }

    public class PickUpModel_SmallBird3 : CreaturePickUpModel
    {
        public PickUpModel_SmallBird3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SmallBird3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SmallBird3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SmallBird3_FlaverText");
            this.level = 2;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370183), (EmotionCardAbilityBase)new PickUpModel_SmallBird3.LogEmotionCardAbility_SmallBird3(), model);
        }

        public class LogEmotionCardAbility_SmallBird3 : EmotionCardAbilityBase
        {
            public const int _evadePowMin = 1;
            public const int _evadePowMax = 2;
            public const int _powByEvadeMin = 1;
            public const int _powByEvadeMax = 2;

            public static int EvadePow => RandomUtil.Range(1, 2);

            public static int PowByEvade => RandomUtil.Range(1, 2);

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.Detail != BehaviourDetail.Evasion)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = PickUpModel_SmallBird3.LogEmotionCardAbility_SmallBird3.EvadePow
                });
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                if (behavior.Detail != BehaviourDetail.Evasion)
                    return;
                BattlePlayingCardDataInUnitModel card = behavior.card;
                if (card != null)
                    card.ApplyDiceStatBonus(DiceMatch.NextAttackDice, new DiceStatBonus()
                    {
                        power = PickUpModel_SmallBird3.LogEmotionCardAbility_SmallBird3.PowByEvade
                    });
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Feather", 3f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Smallbird_Wing");
            }
        }
    }

    public class PickUpModel_SpiderBud : PickUpModelBase
    {
        public PickUpModel_SpiderBud()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SpiderBud_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SpiderBud_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370231));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370232));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370233));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 3
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_SpiderBud0 : CreaturePickUpModel
    {
        public PickUpModel_SpiderBud0()
        {
            this.level = 3;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370231),
      new LorId(LogLikeMod.ModId, 15370232),
      new LorId(LogLikeMod.ModId, 15370233)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_SpiderBud_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_SpiderBud"];
    }

    public class PickUpModel_SpiderBud1 : CreaturePickUpModel
    {
        public PickUpModel_SpiderBud1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SpiderBud1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SpiderBud1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SpiderBud1_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370231), (EmotionCardAbilityBase)new PickUpModel_SpiderBud1.LogEmotionCardAbility_SpiderBud1(), model);
        }

        public class LogEmotionCardAbility_SpiderBud1 : EmotionCardAbilityBase
        {
            public const int _valueMin = 1;
            public const int _valueMax = 2;

            public int Value => RandomUtil.Range(1, 2);

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                if (behavior.DiceVanillaValue != behavior.GetDiceMax())
                    return;
                BattleUnitModel target = behavior.card.target;
                if (target == null)
                    return;
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Paralysis, this.Value, this._owner);
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, this.Value, this._owner);
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, this.Value, this._owner);
                behavior.card.target?.battleCardResultLog?.SetCreatureAbilityEffect("3/Spider_Cocoon", 2f);
            }
        }
    }

    public class PickUpModel_SpiderBud2 : CreaturePickUpModel
    {
        public PickUpModel_SpiderBud2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SpiderBud2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SpiderBud2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SpiderBud2_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370232), (EmotionCardAbilityBase)new PickUpModel_SpiderBud2.LogEmotionCardAbility_SpiderBud2(), model);
        }

        public class LogEmotionCardAbility_SpiderBud2 : EmotionCardAbilityBase
        {
            public const int _healMin = 2;
            public const int _healMax = 4;
            public bool breaked;

            public static int Heal => RandomUtil.Range(2, 4);

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                this.breaked = false;
                if (behavior == null)
                    return;
                bool? nullable = behavior.card?.target?.IsBreakLifeZero();
                bool flag = false;
                if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
                    this.breaked = true;
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                if (behavior == null || behavior.card?.target == null || !behavior.card.target.IsBreakLifeZero() || this.breaked)
                    return;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                    alive.RecoverHP(PickUpModel_SpiderBud2.LogEmotionCardAbility_SpiderBud2.Heal);
                string resPath = "";
                switch (behavior.behaviourInCard.MotionDetail)
                {
                    case MotionDetail.H:
                        resPath = "3/SpiderBud_Spiders_H";
                        break;
                    case MotionDetail.J:
                        resPath = "3/SpiderBud_Spiders_J";
                        break;
                    case MotionDetail.Z:
                        resPath = "3/SpiderBud_Spiders_Z";
                        break;
                }
                if (!string.IsNullOrEmpty(resPath))
                    this._owner.battleCardResultLog.SetCreatureAbilityEffect(resPath, 1f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Spider_gochiDown");
            }
        }
    }

    public class PickUpModel_SpiderBud3 : CreaturePickUpModel
    {
        public PickUpModel_SpiderBud3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_SpiderBud3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_SpiderBud3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_SpiderBud3_FlaverText");
            this.level = 3;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370233), (EmotionCardAbilityBase)new PickUpModel_SpiderBud3.LogEmotionCardAbility_SpiderBud3(), model);
        }

        public class LogEmotionCardAbility_SpiderBud3 : EmotionCardAbilityBase
        {
            public const int _powRed = 1;
            public const float _prob = 0.5f;
            public const int _bind = 1;
            public const int _vuln = 1;

            public bool Prob => (double)RandomUtil.valueForProb < 0.5;

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Spidermom_Down");
                if (soundEffectPlayer != null)
                    soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                this.MakeEffect("3/Spider_RedEye", target: this._owner, apply: false);
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!behavior.IsParrying())
                    return;
                BattleDiceBehavior targetDice = behavior?.TargetDice;
                if (targetDice == null)
                    return;
                targetDice.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = -1
                });
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                if (!this.Prob)
                    return;
                behavior.card?.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, 1, this._owner);
                behavior.card?.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, this._owner);
            }
        }
    }

    public class PickUpModel_TheSnowQueen : PickUpModelBase
    {
        public PickUpModel_TheSnowQueen()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_TheSnowQueen_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_TheSnowQueen_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370301));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370302));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370303));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 4
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_TheSnowQueen0 : CreaturePickUpModel
    {
        public PickUpModel_TheSnowQueen0()
        {
            this.level = 4;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370301),
      new LorId(LogLikeMod.ModId, 15370302),
      new LorId(LogLikeMod.ModId, 15370303)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_TheSnowQueen_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_TheSnowQueen"];
    }

    public class PickUpModel_TheSnowQueen1 : CreaturePickUpModel
    {
        public PickUpModel_TheSnowQueen1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_TheSnowQueen1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_TheSnowQueen1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_TheSnowQueen1_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370301), (EmotionCardAbilityBase)new PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1(), model);
        }

        public class LogEmotionCardAbility_TheSnowQueen1 : EmotionCardAbilityBase
        {
            public const float _prob = 0.5f;
            public const int _bindMin = 1;
            public const int _bindMax = 3;
            public const int _dmgMin = 2;
            public const int _dmgMax = 5;

            public static bool Prob => (double)RandomUtil.valueForProb < 0.5;

            public static int Bind => RandomUtil.Range(1, 3);

            public static int Dmg => RandomUtil.Range(2, 5);

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || !PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.Prob)
                    return;
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.Bind, this._owner);
                if (target.bufListDetail.GetReadyBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.BattleUnitBuf_Emotion_Snowqueen_Aura)) == null)
                    target.bufListDetail.AddReadyBuf((BattleUnitBuf)new PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.BattleUnitBuf_Emotion_Snowqueen_Aura());
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x.bufType == KeywordBuf.Binding)) == null)
                    return;
                target.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_SnowUnATK", 2f);
                target.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowQueen_Atk");
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || !this.IsAttackDice(behavior.Detail) || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x.bufType == KeywordBuf.Binding)) == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    breakDmg = PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.Dmg
                });
            }

            public class BattleUnitBuf_Emotion_Snowqueen_Aura : BattleUnitBuf
            {
                public GameObject aura;

                public override bool Hide => true;

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    if (this._owner != null)
                    {
                        Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_SnowAura", 1f, this._owner.view, this._owner.view);
                        this.aura = fxCreatureEffect != null ? fxCreatureEffect.gameObject : (GameObject)null;
                    }
                    SoundEffectPlayer.PlaySound("Creature/SnowQueen_Freeze");
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    this.Destroy();
                }

                public override void OnDie()
                {
                    base.OnDie();
                    this.Destroy();
                }

                public override void Destroy()
                {
                    base.Destroy();
                    this.DestroyAura();
                }

                public void DestroyAura()
                {
                    if (!(this.aura != null))
                        return;
                    UnityEngine.Object.Destroy(this.aura.gameObject);
                    this.aura = (GameObject)null;
                }
            }
        }
    }

    public class PickUpModel_TheSnowQueen2 : CreaturePickUpModel
    {
        public PickUpModel_TheSnowQueen2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_TheSnowQueen2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_TheSnowQueen2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_TheSnowQueen2_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370302), (EmotionCardAbilityBase)new PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2(), model);
        }

        public class LogEmotionCardAbility_TheSnowQueen2 : EmotionCardAbilityBase
        {
            public int cnt;

            public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
            {
                base.OnParryingStart(card);
                this.cnt = 0;
            }

            public override void OnWinParrying(BattleDiceBehavior behavior)
            {
                base.OnWinParrying(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null)
                    return;
                ++this.cnt;
                target.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_IceUnATK", 2f);
                target.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowQueen_Guard");
                if (this.cnt != 2)
                    return;
                BattleUnitBuf buf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2.BattleUnitBuf_Emotion_SnowQueen_Shard));
                if (buf == null)
                {
                    buf = (BattleUnitBuf)new PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2.BattleUnitBuf_Emotion_SnowQueen_Shard(this._owner);
                    target.bufListDetail.AddBuf(buf);
                }
                if (!(buf is PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2.BattleUnitBuf_Emotion_SnowQueen_Shard emotionSnowQueenShard))
                    return;
                emotionSnowQueenShard.Add();
            }

            public override void OnEndParrying(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnEndParrying(curCard);
                this.cnt = 0;
            }

            public class BattleUnitBuf_Emotion_SnowQueen_Shard : BattleUnitBuf
            {
                public const int _stackMax = 3;
                public BattleUnitModel _attacker;

                public override string keywordId => "SnowQueen_Emotion_Shard";

                public override string keywordIconId => "SnowQueen_Debuf";

                public BattleUnitBuf_Emotion_SnowQueen_Shard(BattleUnitModel attacker)
                {
                    this._attacker = attacker;
                    this.stack = 0;
                }

                public void Add() => ++this.stack;

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    if (this.stack < 3)
                        return;
                    this._owner.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2.BattleUnitBuf_Emotion_SnowQueen_Stun2(this._attacker));
                    this.Destroy();
                }
            }

            public class BattleUnitBuf_Emotion_SnowQueen_Stun2 : BattleUnitBuf
            {
                public BattleUnitModel _attacker;
                public Battle.CreatureEffect.CreatureEffect _aura;

                public override bool Hide => true;

                public BattleUnitBuf_Emotion_SnowQueen_Stun2(BattleUnitModel attacker)
                {
                    this._attacker = attacker;
                }

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1, this._attacker);
                }

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    if (this._owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null || this._owner.IsImmune(KeywordBuf.Stun) || this._owner.bufListDetail.IsImmune(BufPositiveType.Negative))
                        return;
                    this._owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                    this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("0/SnowQueen_Emotion_Frozen", 1f, this._owner.view, this._owner.view);
                    SoundEffectPlayer.PlaySound("Creature/SnowQueen_Immune");
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    if (this._aura != null)
                    {
                        UnityEngine.Object.Destroy(this._aura.gameObject);
                        this._aura = (Battle.CreatureEffect.CreatureEffect)null;
                        this._owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                    }
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_TheSnowQueen3 : CreaturePickUpModel
    {
        public PickUpModel_TheSnowQueen3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_TheSnowQueen3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_TheSnowQueen3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_TheSnowQueen3_FlaverText");
            this.level = 4;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370303), (EmotionCardAbilityBase)new PickUpModel_TheSnowQueen3.LogEmotionCardAbility_TheSnowQueen3(), model);
        }

        public class LogEmotionCardAbility_TheSnowQueen3 : EmotionCardAbilityBase
        {
            public bool _effect;

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._effect)
                    return;
                this._effect = true;
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/0_K/FX_IllusionCard_0_K_Blizzard");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = GameObject.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if ((creatureEffect != null ? creatureEffect.gameObject.GetComponent<AutoDestruct>() : null) == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect != null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct)null;
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                SoundEffectPlayer.PlaySound("Creature/SnowQueen_StrongAtk2");
            }

            public override void OnSelectEmotion()
            {
                base.OnSelectEmotion();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive != this._owner)
                        alive.bufListDetail.AddBuf((BattleUnitBuf)new PickUpModel_TheSnowQueen3.LogEmotionCardAbility_TheSnowQueen3.BattleUnitBuf_Emotion_SnowQueen_Stun(this._owner));
                }
            }

            public class BattleUnitBuf_Emotion_SnowQueen_Stun : BattleUnitBuf
            {
                public const int _bindMin = 6;
                public const int _bindMax = 6;
                public BattleUnitModel _attacker;
                public Battle.CreatureEffect.CreatureEffect _aura;

                public static int Bind => RandomUtil.Range(6, 6);

                public override string keywordId => "SnowQueen_Emotion_Stun";

                public override string keywordIconId => "SnowQueen_Stun";

                public BattleUnitBuf_Emotion_SnowQueen_Stun(BattleUnitModel attacker)
                {
                    this._attacker = attacker;
                }

                public override void Init(BattleUnitModel owner)
                {
                    base.Init(owner);
                    owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1, this._attacker);
                }

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    if (this._owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null || this._owner.IsImmune(KeywordBuf.Stun) || this._owner.bufListDetail.IsImmune(BufPositiveType.Negative))
                        return;
                    this._owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                    this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("0/SnowQueen_Emotion_Frozen", 1f, this._owner.view, this._owner.view);
                }

                public override void OnRoundEnd()
                {
                    base.OnRoundEnd();
                    if (this._owner.faction != this._attacker.faction)
                        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, PickUpModel_TheSnowQueen3.LogEmotionCardAbility_TheSnowQueen3.BattleUnitBuf_Emotion_SnowQueen_Stun.Bind, this._attacker);
                    if (this._aura != null)
                    {
                        GameObject.Destroy(this._aura.gameObject);
                        this._aura = (Battle.CreatureEffect.CreatureEffect)null;
                        this._owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                    }
                    this.Destroy();
                }
            }
        }
    }

    public class PickUpModel_UniverseZogak : PickUpModelBase
    {
        public PickUpModel_UniverseZogak()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_UniverseZogak_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_UniverseZogak_Desc");
            this.FlaverText = "???";
        }

        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
            RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370041));
            collection.Add(passiveInfo1);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
            RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370042));
            collection.Add(passiveInfo2);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
            RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370043));
            collection.Add(passiveInfo3);
            emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
            MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
            {
                abnormal = emotionCardXmlInfoList,
                level = 1
            };
            LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)collection);
        }
    }

    public class PickUpModel_UniverseZogak0 : CreaturePickUpModel
    {
        public PickUpModel_UniverseZogak0()
        {
            this.level = 1;
            this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370041),
      new LorId(LogLikeMod.ModId, 15370042),
      new LorId(LogLikeMod.ModId, 15370043)
    };
        }

        public override List<EmotionCardXmlInfo> GetCreatureList()
        {
            return CreaturePickUpModel.GetEmotionListById(this.ids);
        }

        public override string GetCreatureName()
        {
            return TextDataModel.GetText("PickUpCreature_UniverseZogak_Name");
        }

        public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_UniverseZogak"];
    }

    public class PickUpModel_UniverseZogak1 : CreaturePickUpModel
    {
        public PickUpModel_UniverseZogak1()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_UniverseZogak1_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_UniverseZogak1_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_UniverseZogak1_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370041), (EmotionCardAbilityBase)new PickUpModel_UniverseZogak1.LogEmotionCardAbility_UniverseZogak1(), model, true);
        }

        public class LogEmotionCardAbility_UniverseZogak1 : EmotionCardAbilityBase
        {
            public const int _brkDmgMin = 5;
            public const int _brkDmgMax = 10;
            public const int _recoverBpMin = 5;
            public const int _recoverBpMax = 10;

            public static int BrkDmg => RandomUtil.Range(5, 10);

            public static int RecoverBP => RandomUtil.Range(5, 10);

            public override void OnSelectEmotionOnce()
            {
                base.OnSelectEmotionOnce();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive.faction != this._owner.faction)
                        alive.TakeBreakDamage(PickUpModel_UniverseZogak1.LogEmotionCardAbility_UniverseZogak1.BrkDmg, DamageType.Emotion, this._owner);
                    else
                        alive.breakDetail.RecoverBreak(PickUpModel_UniverseZogak1.LogEmotionCardAbility_UniverseZogak1.RecoverBP);
                }
                Camera effectCam = SingletonBehavior<BattleCamManager>.Instance.EffectCam;
                CameraFilterPack_Distortion_Dream2 r = effectCam.GetComponent<CameraFilterPack_Distortion_Dream2>();
                if (r == null)
                    r = effectCam.gameObject.AddComponent<CameraFilterPack_Distortion_Dream2>();
                BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
                if (instance != null)
                    instance.StartCoroutine(this.DistortionRoutine(r));
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Sing");
                if (soundEffectPlayer == null)
                    return;
                soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
            }

            public IEnumerator DistortionRoutine(CameraFilterPack_Distortion_Dream2 r)
            {
                float e = 0.0f;
                float amount = UnityEngine.Random.Range(20f, 30f);
                int speed = 15;
                while ((double)e < 1.0)
                {
                    e += Time.deltaTime * 2f;
                    r.Distortion = Mathf.Lerp(amount, 0.0f, e);
                    r.Speed = Mathf.Lerp((float)speed, 0.0f, e);
                    yield return null;
                }
                GameObject.Destroy(r);
            }
        }
    }

    public class PickUpModel_UniverseZogak2 : CreaturePickUpModel
    {
        public PickUpModel_UniverseZogak2()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_UniverseZogak2_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_UniverseZogak2_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_UniverseZogak2_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370042), (EmotionCardAbilityBase)new PickUpModel_UniverseZogak2.LogEmotionCardAbility_UniverseZogak2(), model);
        }

        public class LogEmotionCardAbility_UniverseZogak2 : EmotionCardAbilityBase
        {
            public Battle.CreatureEffect.CreatureEffect _hitEffect;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (behavior.Detail != BehaviourDetail.Penetrate)
                    return;
                this._hitEffect = this.MakeEffect("4/Fragment_Hit", destroyTime: 1f);
                Battle.CreatureEffect.CreatureEffect hitEffect = this._hitEffect;
                if (hitEffect != null)
                    hitEffect.gameObject.SetActive(false);
                if (behavior.card.target != null)
                {
                    SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Hit");
                    if (soundEffectPlayer == null)
                        return;
                    soundEffectPlayer.SetGlobalPosition(behavior.card.target.view.WorldPosition);
                }
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                BattleUnitModel target = behavior.card?.target;
                if (target == null)
                    return;
                float num = (float)target.breakDetail.breakGauge / (float)target.breakDetail.GetDefaultBreakGauge();
                if (behavior.Detail == BehaviourDetail.Penetrate)
                {
                    BattleUnitBuf activatedBuf = target.bufListDetail.GetActivatedBuf(KeywordBuf.UniverseCardBuf);
                    if (activatedBuf != null)
                        ++activatedBuf.stack;
                    else
                        target.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_fragmentSpace2.UniverseBuf());
                }
                if (!target.IsBreakLifeZero())
                {
                    target.breakDetail.breakGauge = (int)((double)target.breakDetail.GetDefaultBreakGauge() * (double)num);
                    if (target.breakDetail.breakGauge < 1)
                    {
                        target.breakDetail.breakLife = 0;
                        target.breakDetail.breakGauge = 0;
                        target.breakDetail.DestroyBreakPoint();
                    }
                }
            }

            public override void OnPrintEffect(BattleDiceBehavior behavior)
            {
                if (!(bool)this._hitEffect)
                    return;
                this._hitEffect.gameObject.SetActive(true);
                this._hitEffect = (Battle.CreatureEffect.CreatureEffect)null;
            }

            public class UniverseBuf : BattleUnitBuf
            {
                public override KeywordBuf bufType => KeywordBuf.UniverseCardBuf;

                public override StatBonus GetStatBonus()
                {
                    return new StatBonus()
                    {
                        breakRate = -Mathf.Min(50, this.stack * 5)
                    };
                }
            }
        }
    }

    public class PickUpModel_UniverseZogak3 : CreaturePickUpModel
    {
        public PickUpModel_UniverseZogak3()
        {
            this.Name = TextDataModel.GetText("PickUpCreature_UniverseZogak3_Name");
            this.Desc = TextDataModel.GetText("PickUpCreature_UniverseZogak3_Desc");
            this.FlaverText = TextDataModel.GetText("PickUpCreature_UniverseZogak3_FlaverText");
            this.level = 1;
        }

        public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370043), (EmotionCardAbilityBase)new PickUpModel_UniverseZogak3.LogEmotionCardAbility_UniverseZogak3(), model);
        }

        public class LogEmotionCardAbility_UniverseZogak3 : EmotionCardAbilityBase
        {
            public const float _prob = 0.5f;
            public const float _probPerHit = 0.1f;
            public const int _weak = 1;
            public const int _disarm = 1;
            public const int _maxCnt = 3;
            public int cnt;
            public int hitCnt;
            public bool activated;

            public bool Prob()
            {
                return (double)RandomUtil.valueForProb < 0.5 + (double)this.hitCnt * 0.10000000149011612;
            }

            public override void OnStartOneSideAction(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnStartOneSideAction(curCard);
                this.activated = false;
                this.cnt = 0;
            }

            public override void OnSucceedAttack(BattleDiceBehavior behavior)
            {
                base.OnSucceedAttack(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || behavior.IsParrying())
                    return;
                if (this.Prob() && this.cnt < 3)
                {
                    ++this.cnt;
                    target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1, this._owner);
                    target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Disarm, 1, this._owner);
                    if (!this.activated)
                    {
                        this._owner.battleCardResultLog.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Effect));
                        this.activated = true;
                    }
                }
                ++this.hitCnt;
            }

            public void Effect()
            {
                try
                {
                    SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Sing");
                    if (soundEffectPlayer != null)
                        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
                    BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
                    if (!(instance != null))
                        return;
                    instance.AddCameraFilter<CameraFilterCustom_universe>(true);
                }
                catch (Exception ex)
                {
                    Debug.LogError(("Camera Filter Adding Failed " + ex?.ToString()));
                }
            }
        }
    }
}