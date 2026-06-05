using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;
using Sound;
using UnityEngine;
using LOR_DiceSystem;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// A BattleUnitBuf with some additional overrides that interact with RMR mechanics.
    /// </summary>
    public class RMRBufBase : BattleUnitBuf
    {
        public virtual void OnSpendSmoke(int amount)
        {
        }

        public virtual bool SmokeBoostsOutgoingDamage()
        {
            return true;
        }

        public virtual bool SmokeBoostsIncomingDamage()
        {
            return true;
        }

        public virtual void OnSpendSmokeByCombatPage(int amount)
        {
        }
    }
    public static class RoguelikeBufs
    {
        public static KeywordBuf CritChance;
        public static KeywordBuf RMRShield;
        public static KeywordBuf RMRStaggerShield;
        public static KeywordBuf BurnProtection;
        public static KeywordBuf BleedProtection;
        public static KeywordBuf ClashPower;
        public static KeywordBuf SlashClashPower;
        public static KeywordBuf PierceClashPower;
        public static KeywordBuf BluntClashPower;
        public static KeywordBuf RMRSmoke;
        public static KeywordBuf RMRPersistence;
        public static KeywordBuf RMRLuck;
    }

    public class SweeperBuf : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.RMRPersistence;
            }
        }

        public override string keywordId => "RMR_Persistence";
        public override string keywordIconId => "RMR_ReworkPersistence"; // unsure if the persistence icon name is this or not
        public override BufPositiveType positiveType => BufPositiveType.Positive;

        public override void OnHpZero()
        {
            if (base.IsDestroyed())
            {
                return;
            }
            this._owner.SetHp(30);
            this._owner.UnitData.historyInStage.sweeperSkill++;
            this.Destroy();
        }
        public override void OnRoundEnd()
        {
            this.stack--;
            if (this.stack <= 0)
            {
                // _owner.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 504001));
                // this can be uncommented if we want to make the page reappear in deck if the buf does not revive you,
                // probably unnecessary but an optional buff to persistence
                this.Destroy();
            }
        }
    }

    public class BattleUnitBuf_RMR_Luck : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.RMRLuck;
            }
        }

        public override string keywordId => "RMR_Luck";
        public override string keywordIconId => "RMRBuf_Luck";
        public override BufPositiveType positiveType => BufPositiveType.Positive;

        public static void ChangeDiceResult(BattleDiceBehavior behavior, int level, ref int diceResult)
        {
            int diceMin = behavior.GetDiceMin();
            int diceMax = behavior.GetDiceMax();
            int num1 = diceResult;
            for (int index = 0; index < level; ++index)
            {
                int num2 = DiceStatCalculator.MakeDiceResult(diceMin, diceMax, 0);
                if (num2 > num1)
                    num1 = num2;
            }
            diceResult = num1;
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }

        public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
        {
            BattleUnitBuf_RMR_Luck.ChangeDiceResult(behavior, this.stack, ref diceResult);
            this.stack--;
            if (this.stack < 0)
            {
                this.Destroy();
            }
        }
    }

    public class BattleUnitBuf_RMR_Smoke : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.RMRSmoke;
            }
        }
        public override string keywordId => "RMR_Smoke";
        public override string keywordIconId => "RMR_ReworkSmoke"; // not sure if just "Smoke" is the bufname


        public override int paramInBufDesc
        {
            get
            {
                return this.stack * 3;
            }
        }

        public override void OnAddBuf(int addedStack)
        {
            if (stack > 10)
            {
                stack = 10;
            }
            if (_owner.IsImmune(bufType))
            {
                stack = 0;
                Destroy();
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (!_owner.IsImmune(bufType) && stack >= 9)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    power = 1
                });
            }
        }

        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (_owner.IsImmune(bufType))
            {
                return;
            }
            bool boostdamage = true;
            foreach (PassiveAbilityBase passiveAbilityBase in _owner.passiveDetail.PassiveList)
            {
                if (passiveAbilityBase != null && passiveAbilityBase is RMRPassiveBase && passiveAbilityBase.isActiavted)
                {
                    try
                    {
                        RMRPassiveBase basepassive = passiveAbilityBase as RMRPassiveBase;
                        if (!basepassive.SmokeBoostsOutgoingDamage())
                        {
                            boostdamage = false;
                        }
                    }
                    catch (Exception message)
                    {
                        Debug.Log(message);
                    }
                }
            }
            foreach (BattleUnitBuf battleUnitBuf in _owner.bufListDetail.GetActivatedBufList().ToArray())
            {
                if (battleUnitBuf != null && battleUnitBuf is RMRBufBase && !battleUnitBuf.IsDestroyed())
                {
                    try
                    {
                        RMRBufBase rmrbuffbase = battleUnitBuf as RMRBufBase;
                        if (!rmrbuffbase.SmokeBoostsOutgoingDamage())
                        {
                            boostdamage = false;
                        }
                    }
                    catch (Exception message2)
                    {
                        Debug.Log(message2);
                    }
                }
            }
            if (stack > 0 && boostdamage)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    dmgRate = stack * 3
                });
            }
        }

        public override int GetDamageIncreaseRate()
        {
            if (_owner.IsImmune(bufType))
            {
                return base.GetDamageIncreaseRate();
            }
            bool boostdamage = true;
            foreach (PassiveAbilityBase passiveAbilityBase in _owner.passiveDetail.PassiveList)
            {
                if (passiveAbilityBase != null && passiveAbilityBase is RMRPassiveBase && passiveAbilityBase.isActiavted)
                {
                    try
                    {
                        RMRPassiveBase basepassive = passiveAbilityBase as RMRPassiveBase;
                        if (!basepassive.SmokeBoostsIncomingDamage())
                        {
                            boostdamage = false;
                        }
                    }
                    catch (Exception message)
                    {
                        Debug.Log(message);
                    }
                }
            }
            foreach (BattleUnitBuf battleUnitBuf in _owner.bufListDetail.GetActivatedBufList().ToArray())
            {
                if (battleUnitBuf != null && battleUnitBuf is RMRBufBase && !battleUnitBuf.IsDestroyed())
                {
                    try
                    {
                        RMRBufBase rmrbuffbase = battleUnitBuf as RMRBufBase;
                        if (!rmrbuffbase.SmokeBoostsIncomingDamage())
                        {
                            boostdamage = false;
                        }
                    }
                    catch (Exception message2)
                    {
                        Debug.Log(message2);
                    }
                }
            }
            if (stack > 0 && boostdamage)
            {
                return stack * 3;
            }
            return base.GetDamageIncreaseRate();
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.stack -= this.stack / 2;
            if (this.stack <= 0)
            {
                this.Destroy();
            }
        }

        public bool Spend(int amount, bool isCard = false)
        {
            if (amount <= this.stack)
            {
                this.stack -= amount;
                if (this.stack <= 0)
                {
                    this.Destroy();
                }
                if (isCard)
                {
                    foreach (PassiveAbilityBase passiveAbilityBase in _owner.passiveDetail.PassiveList)
                    {
                        if (passiveAbilityBase != null && passiveAbilityBase is RMRPassiveBase && passiveAbilityBase.isActiavted)
                        {
                            try
                            {
                                RMRPassiveBase basepassive = passiveAbilityBase as RMRPassiveBase;
                                basepassive.OnSpendSmokeByCombatPage(amount);
                            }
                            catch (Exception message)
                            {
                                Debug.Log(message);
                            }
                        }
                    }
                    foreach (BattleUnitBuf battleUnitBuf in _owner.bufListDetail.GetActivatedBufList().ToArray())
                    {
                        if (battleUnitBuf != null && battleUnitBuf is RMRBufBase && !battleUnitBuf.IsDestroyed())
                        {
                            try
                            {
                                RMRBufBase rmrbuffbase = battleUnitBuf as RMRBufBase;
                                rmrbuffbase.OnSpendSmokeByCombatPage(amount);
                            }
                            catch (Exception message2)
                            {
                                Debug.Log(message2);
                            }
                        }
                    }
                }
                else
                {
                    foreach (PassiveAbilityBase passiveAbilityBase in _owner.passiveDetail.PassiveList)
                    {
                        if (passiveAbilityBase != null && passiveAbilityBase is RMRPassiveBase && passiveAbilityBase.isActiavted)
                        {
                            try
                            {
                                RMRPassiveBase basepassive = passiveAbilityBase as RMRPassiveBase;
                                basepassive.OnSpendSmoke(amount);
                            }
                            catch (Exception message)
                            {
                                Debug.Log(message);
                            }
                        }
                    }
                    foreach (BattleUnitBuf battleUnitBuf in _owner.bufListDetail.GetActivatedBufList().ToArray())
                    {
                        if (battleUnitBuf != null && battleUnitBuf is RMRBufBase && !battleUnitBuf.IsDestroyed())
                        {
                            try
                            {
                                RMRBufBase rmrbuffbase = battleUnitBuf as RMRBufBase;
                                rmrbuffbase.OnSpendSmoke(amount);
                            }
                            catch (Exception message2)
                            {
                                Debug.Log(message2);
                            }
                        }
                    }
                }
               
                return true;
            }
            return false;
        }
    }

    public class BattleUnitBuf_RMR_BluntClashPower : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.BluntClashPower;
            }
        }
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override string keywordId
        {
            get
            {
                if (this.stack >= 0)
                {
                    return "RMRBuf_BluntClashPower";
                }
                else if (this.stack < 0)
                {
                    return "RMRBuf_BluntClashPowerDown";
                }
                return "RMRBuf_BluntClashPower";
            }
        }

        public override string keywordIconId
        {
            get
            {
                if (this.stack >= 0)
                {
                    return "BluntClashUp";
                }
                else if (this.stack < 0)
                {
                    return "BluntClashDown";
                }
                return "BluntClashUp";
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.Detail == BehaviourDetail.Hit)
            {
                if (behavior.IsParrying() && !this._owner.IsImmune(this.bufType) && !this._owner.IsNullifyPower() && !behavior.card.ignorePower)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        power = stack,
                        dmg = -stack,
                        breakDmg = -stack,
                        guardBreakAdder = -stack
                    });
                }
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }

    }

    public class BattleUnitBuf_RMR_PierceClashPower : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.PierceClashPower;
            }
        }
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override string keywordId
        {
            get
            {
                if (this.stack >= 0)
                {
                    return "RMRBuf_PierceClashPower";
                }
                else if (this.stack < 0)
                {
                    return "RMRBuf_PierceClashPowerDown";
                }
                return "RMRBuf_PierceClashPower";
            }
        }

        public override string keywordIconId
        {
            get
            {
                if (this.stack >= 0)
                {
                    return "PierceClashUp";
                }
                else if (this.stack < 0)
                {
                    return "PierceClashDown";
                }
                return "PierceClashUp";
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.Detail == BehaviourDetail.Penetrate)
            {
                if (behavior.IsParrying() && !this._owner.IsImmune(this.bufType) && !this._owner.IsNullifyPower() && !behavior.card.ignorePower)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        power = stack,
                        dmg = -stack,
                        breakDmg = -stack,
                        guardBreakAdder = -stack
                    });
                }
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }

    }
    public class BattleUnitBuf_RMR_SlashClashPower : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.SlashClashPower;
            }
        }
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override string keywordId
        {
            get
            {
                if (this.stack >= 0)
                {
                    return "RMRBuf_SlashClashPower";
                }
                else if (this.stack < 0)
                {
                    return "RMRBuf_SlashClashPowerDown";
                }
                return "RMRBuf_SlashClashPower";
            }
        }

        public override string keywordIconId
        {
            get
            {
                if (this.stack >= 0)
                {
                    return "SlashClashUp";
                }
                else if (this.stack < 0)
                {
                    return "SlashClashDown";
                }
                return "SlashClashUp";
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.Detail == BehaviourDetail.Slash)
            {
                if (behavior.IsParrying() && !this._owner.IsImmune(this.bufType) && !this._owner.IsNullifyPower() && !behavior.card.ignorePower)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        power = stack,
                        dmg = -stack,
                        breakDmg = -stack,
                        guardBreakAdder = -stack
                    });
                }
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }

    }
    public class BattleUnitBuf_RMR_ClashPower : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.ClashPower;
            }
        }
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override string keywordId
        {
            get
            {
                if (this.stack >= 0)
                {
                    return "RMRBuf_ClashPower";
                }
                else if (this.stack < 0)
                {
                    return "RMRBuf_ClashPowerDown";
                }
                return "RMRBuf_ClashPower";
            }
        }

        public override string keywordIconId
        {
            get
            {
                if (this.stack >= 0)
                {
                    return "ClashPowerUp";
                }
                else if (this.stack < 0)
                {
                    return "ClashPowerDown";
                }
                return "ClashPowerUp";
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.IsParrying() && !this._owner.IsImmune(this.bufType) && !this._owner.IsNullifyPower() && !behavior.card.ignorePower)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    power = stack,
                    dmg = -stack,
                    breakDmg = -stack,
                    guardBreakAdder = -stack
                });
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }
        
    }
    public class BattleUnitBuf_RMR_BurnProtection : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.BurnProtection;
            }
        }
        public override string keywordId => "RMR_BurnProtection";
        public override string keywordIconId => "RMRBuf_BurnProtection";
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override float DmgFactor(int dmg, DamageType type, KeywordBuf keyword)
        {
            float result;
            if (keyword == KeywordBuf.Burn)
            {
                float num = dmg;
                if (num > this.stack)
                {
                    result = (num - this.stack) / num;
                }
                else
                {
                    result = 0f;
                }
            }
            else
            {
                result = base.DmgFactor(dmg, type, keyword);
            }
            return result;
        }

        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            this.Destroy();
        }
    }

    public class BattleUnitBuf_RMR_BleedProtection : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.BleedProtection;
            }
        }
        public override string keywordId => "RMR_BleedProtection";
        public override string keywordIconId => "RMRBuf_BleedProtection";
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override float DmgFactor(int dmg, DamageType type, KeywordBuf keyword)
        {
            float result;
            if (keyword == KeywordBuf.Bleeding)
            {
                float num = dmg;
                if (num > this.stack)
                {
                    result = (num - this.stack) / num;
                }
                else
                {
                    result = 0f;
                }
            }
            else
            {
                result = base.DmgFactor(dmg, type, keyword);
            }
            return result;
        }

        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            this.Destroy();
        }
    }

    public class BattleUnitBuf_RMR_Shield : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.RMRShield;
            }
        }
        public override string keywordId => "RMR_Shield";
        public override string keywordIconId => "RMRBuf_Shield";
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (!this._owner.IsImmune(this.bufType) && !base.IsDestroyed())
            {
                if (dmg > this.stack)
                {
                    float num = (float)this.stack;
                    this.Destroy();
                    return ((float)dmg - num) / (float)dmg;
                }
                if (this.stack >= dmg)
                {
                    this.stack -= dmg;
                    return 0f;
                }
            }
            return base.DmgFactor(dmg, type, keyword);
        }

        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            this.Destroy();
        }
    }

    public class BattleUnitBuf_RMR_StaggerShield : BattleUnitBuf
    {
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.RMRStaggerShield;
            }
        }
        public override string keywordId => "RMR_StaggerShield";
        public override string keywordIconId => "RMRBuf_StaggerShield";
        public override BufPositiveType positiveType => BufPositiveType.Positive;
        public override float BreakDmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            if (!this._owner.IsImmune(this.bufType) && !base.IsDestroyed())
            {
                if (dmg > this.stack)
                {
                    float num = (float)this.stack;
                    this.Destroy();
                    return ((float)dmg - num) / (float)dmg;
                }
                if (this.stack >= dmg)
                {
                    this.stack -= dmg;
                    return 0f;
                }
            }
            return base.BreakDmgFactor(dmg, type, keyword);
        }

        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            this.Destroy();
        }
    }

    public class BattleUnitBuf_RMR_CritChance : BattleUnitBuf
    {
        bool initResources;
        AudioClip critSfx;
        public override string keywordId => "RMR_CriticalStrike";
        public override string keywordIconId => "RMRBuf_CriticalStrike";
        public bool onCrit;

        Sprite sprite;
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.CritChance;
            }
        }
        public override BufPositiveType positiveType => BufPositiveType.Positive;

        private void OnCritEffect()
        {
            critSfx.PlaySound(_owner.view.transform);
            var effect = new GameObject();
            effect.transform.localScale = new Vector3(2f, 2f);
            effect.transform.parent = _owner.view.transform;
            effect.layer = LayerMask.NameToLayer("Effect");
            effect.transform.localPosition = new Vector3(0f, 0f);
            SpriteRenderer spriteRenderer = effect.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
            effect.AddComponent<CritVfx>();
            spriteRenderer.enabled = true;
            effect.SetActive(true);
        }

        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            onCrit = false;
            if (!initResources) // cache resources into memory to prevent slowdowns/stutters
            {
                critSfx = RMRCore.RMRMapHandler.GetAudioClip("critical.mp3");
                sprite = LogLikeMod.ArtWorks["OnCritEffect"];
                initResources = true;
            }
            if (behavior.owner?.currentDiceAction?.target != null)
            {
                var target = behavior.owner?.currentDiceAction?.target;
                var critRoll = RandomUtil.Range(0, 100);
                if (critRoll <= this.stack)
                {
                    onCrit = true;
                }
                if (onCrit)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        dmgRate = 50,
                        breakRate = 50
                    });
                    behavior.owner.emotionDetail.GiveEmotionCoin(EmotionCoinType.Positive);
                    behavior.owner.battleCardResultLog.SetPrintEffectEvent(OnCritEffect);
                    GlobalLogueEffectManager.Instance.OnCrit(_owner, target);
                }
            }
        }

        public override void AfterDiceAction(BattleDiceBehavior behavior)
        {
            onCrit = false;
        }

        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            this.Destroy();
        }

        public class CritVfx : MonoBehaviour
        {
            SpriteRenderer renderer;

            public void Start()
            {
                renderer = base.gameObject.GetComponent<SpriteRenderer>();
                Color color = renderer.color;
                color.b = 0f;
                renderer.color = color;
            }

            public void Update()
            {
                this.timer += Time.deltaTime;
                base.gameObject.transform.localPosition = new Vector3(base.gameObject.transform.localPosition.x, base.gameObject.transform.localPosition.y + timer / 9f);
                Color color = renderer.color;
                color.r = Math.Min(1f, 0f + timer / 1.5f);
                color.g = Math.Max(0f, 1f - timer / 1.5f);
                color.a = Math.Max(0f, 1f - timer / 1.75f);
                renderer.color = color;
                if (this.timer >= this.deathtimer)
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }

            public float timer;

            public float deathtimer = 3f;
        }
    }

}
