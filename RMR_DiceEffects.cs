// -----------------------------------------------------------------------------
// RogueLike Mod Reborn (RMR): RMR_DiceEffects
// Namespace/file: ruina-roguelike-reborn-main\RMR_DiceEffects.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;
using LOR_DiceSystem;

namespace RogueLike_Mod_Reborn
{
    #region --- DiceCardAbility_RMR_RecycleOnClashLose ---

    /// <summary>Dice ability: DiceCardAbility_RMR_RecycleOnClashLose</summary>
    public class DiceCardAbility_RMR_RecycleOnClashLose : DiceCardAbilityBase
    {
        int cap = 0;
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            if (cap < 10 && !owner.breakDetail.IsBreakLifeZero())
            {
                cap++;
                ActivateBonusAttackDice();
            }
        }

        // public static string Desc = "[On Clash Lose] Recycle this die";
    }
    #endregion

    #region --- DiceCardAbility_RMR_HitAndRunBlockonEvade ---


    /// <summary>Dice ability: DiceCardAbility_RMR_HitAndRunBlockonEvade</summary>

    public class DiceCardAbility_RMR_HitAndRunBlockonEvade : DiceCardAbilityBase
    {
        public override void OnRollDice()
        {
            base.OnRollDice();
            owner.bufListDetail.AddBuf(new BattleUnitBuf_doNOTfuckingdareDie() { block = behavior.DiceVanillaValue});
        }

        public override void AfterAction()
        {
            base.AfterAction();
            owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_doNOTfuckingdareDie).Destroy();
        }

        /// <summary>BattleUnitBuf_doNOTfuckingdareDie</summary>

        public class BattleUnitBuf_doNOTfuckingdareDie : BattleUnitBuf
        {
            public int block = 0;
            public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
            {
                return base.GetBreakDamageReduction(behaviourDetail) + block;
            }
            public override int GetDamageReduction(BattleDiceBehavior behavior)
            {
                return base.GetDamageReduction(behavior) + block;
            }
            public override void OnEndBattle(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnEndBattle(curCard);
                this.Destroy();
            }
        }

        // public static string Desc = "[On Clash Lose] Reduce incoming damage and stagger damage by natural value";
    }
    #endregion

    #region --- DiceCardAbility_RMR_Recover2HPSRatk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Recover2HPSRatk</summary>

    public class DiceCardAbility_RMR_Recover2HPSRatk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Recover_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            owner.RecoverHP(2);
            owner.breakDetail.RecoverBreak(2);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_BackstreetsDashDie ---


    /// <summary>Dice ability: DiceCardAbility_RMR_BackstreetsDashDie</summary>

    public class DiceCardAbility_RMR_BackstreetsDashDie : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                breakDmg = behavior.TargetDice.DiceResultValue
            });
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Add2PowerAllpw ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Add2PowerAllpw</summary>

    public class DiceCardAbility_RMR_Add2PowerAllpw : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                power = 2
            });
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_whythefuckistherenoendurance1atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_whythefuckistherenoendurance1atk</summary>

    public class DiceCardAbility_RMR_whythefuckistherenoendurance1atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Endurance_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 1, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_whythefuckistherenobleeding1pw ---


    /// <summary>Dice ability: DiceCardAbility_RMR_whythefuckistherenobleeding1pw</summary>

    public class DiceCardAbility_RMR_whythefuckistherenobleeding1pw : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_CrueltyDie ---


    /// <summary>Dice ability: DiceCardAbility_RMR_CrueltyDie</summary>

    public class DiceCardAbility_RMR_CrueltyDie : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[2] { "Bleeding_Keyword", "Recover_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            int? num = base.card.target?.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding);
            if (num > 0)
            {
                if (num > 30)
                {
                    num = 30;
                }
                base.owner.RecoverHP(num ?? 0);
                base.owner.breakDetail.RecoverBreak(num ?? 0);
            }
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_CrueltyUpgradeDie ---


    /// <summary>Dice ability: DiceCardAbility_RMR_CrueltyUpgradeDie</summary>

    public class DiceCardAbility_RMR_CrueltyUpgradeDie : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[2] { "Bleeding_Keyword", "Recover_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            int? num = base.card.target?.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding);
            if (num > 0)
            {
                num = num * 2;
                if (num > 40)
                {
                    num = 40;
                }
                base.owner.RecoverHP(num ?? 0);
                base.owner.breakDetail.RecoverBreak(num ?? 0);
            }
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_SetFireUpgradeDie ---


    /// <summary>Dice ability: DiceCardAbility_RMR_SetFireUpgradeDie</summary>

    public class DiceCardAbility_RMR_SetFireUpgradeDie : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "Burn_Keyword"};
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 5, owner);
            owner.TakeDamage(3, DamageType.Card_Ability);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_breakvuln1atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_breakvuln1atk</summary>

    public class DiceCardAbility_RMR_breakvuln1atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "Vulnerable_break" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Vulnerable_break, 1, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_critchance2pw ---


    /// <summary>Dice ability: DiceCardAbility_RMR_critchance2pw</summary>

    public class DiceCardAbility_RMR_critchance2pw : DiceCardAbilityBase
    {
         public override string[] Keywords => new string[1] { "RMR_CriticalStrike_Keyword" };

        public override void OnWinParrying()
        {
            base.OnWinParrying();
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 2, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_critchance5pw ---


    /// <summary>Dice ability: DiceCardAbility_RMR_critchance5pw</summary>

    public class DiceCardAbility_RMR_critchance5pw : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_CriticalStrike_Keyword" };

        public override void OnWinParrying()
        {
            base.OnWinParrying();
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 5, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_critchance12pw ---


    /// <summary>Dice ability: DiceCardAbility_RMR_critchance12pw</summary>

    public class DiceCardAbility_RMR_critchance12pw : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_CriticalStrike_Keyword" };

        public override void OnWinParrying()
        {
            base.OnWinParrying();
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 12, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_bleed1twiceatk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_bleed1twiceatk</summary>

    public class DiceCardAbility_RMR_bleed1twiceatk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "Bleeding_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_2protpwnow ---


    /// <summary>Dice ability: DiceCardAbility_RMR_2protpwnow</summary>

    public class DiceCardAbility_RMR_2protpwnow : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "Protection_Keyword" };

        public override void OnWinParrying()
        {
            base.OnWinParrying();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 2, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_break5pw ---


    /// <summary>Dice ability: DiceCardAbility_RMR_break5pw</summary>

    public class DiceCardAbility_RMR_break5pw : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            card?.target?.breakDetail.TakeBreakDamage(5, DamageType.Card_Ability);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_GainSmoke4Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_GainSmoke4Atk</summary>

    public class DiceCardAbility_RMR_GainSmoke4Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 4, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke1Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke1Atk</summary>

    public class DiceCardAbility_RMR_Smoke1Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 1, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke2Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke2Atk</summary>

    public class DiceCardAbility_RMR_Smoke2Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 2, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke3Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke3Atk</summary>

    public class DiceCardAbility_RMR_Smoke3Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 3, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke4Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke4Atk</summary>

    public class DiceCardAbility_RMR_Smoke4Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 4, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke5Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke5Atk</summary>

    public class DiceCardAbility_RMR_Smoke5Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 5, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke6Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke6Atk</summary>

    public class DiceCardAbility_RMR_Smoke6Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 6, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke7Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke7Atk</summary>

    public class DiceCardAbility_RMR_Smoke7Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 7, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke8Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke8Atk</summary>

    public class DiceCardAbility_RMR_Smoke8Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 8, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_Smoke9Atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_Smoke9Atk</summary>

    public class DiceCardAbility_RMR_Smoke9Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Smoke_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRSmoke, 9, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_shield5atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_shield5atk</summary>

    public class DiceCardAbility_RMR_shield5atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Shield_Keyword" };

        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.RMRShield, 5, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_shield5pw ---

    /// <summary>Dice ability: DiceCardAbility_RMR_shield5pw</summary>
    public class DiceCardAbility_RMR_shield5pw : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "RMR_Shield_Keyword" };

        public override void OnWinParrying()
        {
            base.OnWinParrying();
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.RMRShield, 5, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_draw3pw ---



    /// <summary>Dice ability: DiceCardAbility_RMR_draw3pw</summary>


    public class DiceCardAbility_RMR_draw3pw : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "DrawCard_Keyword" };
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            owner.allyCardDetail.DrawCards(3);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_1vulnbreakvulnpw ---


    /// <summary>Dice ability: DiceCardAbility_RMR_1vulnbreakvulnpw</summary>

    public class DiceCardAbility_RMR_1vulnbreakvulnpw : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[2] { "Vulnerabke_Keyword", "Vulnerable_break" };
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Vulnerable, 1, owner);
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Vulnerable_break, 1, owner);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_recover1pl ---


    /// <summary>Dice ability: DiceCardAbility_RMR_recover1pl</summary>

    public class DiceCardAbility_RMR_recover1pl : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Recover_Keyword" };
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            owner.RecoverHP(1);
        }
    }
    #endregion

    #region --- DiceCardAbility_RMR_recover5atk ---


    /// <summary>Dice ability: DiceCardAbility_RMR_recover5atk</summary>

    public class DiceCardAbility_RMR_recover5atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Recover_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            owner.RecoverHP(5);
        }
    }
    #endregion

}
