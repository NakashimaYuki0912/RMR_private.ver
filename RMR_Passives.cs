using System;
using System.Collections.Generic;
using System.Linq;
using abcdcode_LOGLIKE_MOD;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using LOR_XML;
using UnityEngine;
using static RogueLike_Mod_Reborn.RMREffect_Prescript;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// A PassiveAbilityBase with some additional overrides that interact with RMR mechanics.
    /// </summary>
    public class RMRPassiveBase : PassiveAbilityBase
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

	public class PassiveAbility_RMR_Posthaste : PassiveAbilityBase
    {
        public override int SpeedDiceNumAdder()
        {
			if (owner.emotionDetail.EmotionLevel >= 2)
            {
				return 1;
            }
            return base.SpeedDiceNumAdder();
        }
    }

	public class PassiveAbility_RMR_LoneSpeed : PassiveAbilityBase
    {
		bool speed;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
			speed = false;
			foreach (BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(owner.faction).FindAll((BattleUnitModel x) => x != owner))
            {
				foreach (PassiveAbilityBase passive in ally.passiveDetail.PassiveList)
                {
					if (passive.InnerTypeId == 1)
                    {
						speed = true;
                    }
                }
            }
        }

        public override int SpeedDiceNumAdder()
        {
			if (!speed)
            {
				return 1;
            }
            return base.SpeedDiceNumAdder();
        }
    }
    public class PassiveAbility_RMR_CopleyPassive : PassiveAbilityBase
    {
        BattleUnitModel target;
        int count;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            base.OnUseCard(card);
            if (count >= 3)
            {
                card.ForeachQueue(DiceMatch.AllAttackDice, behavior => behavior.AddAbility(new DiceCardAbility_bleeding1atk()));
            }
            if (target != null)
            {
                if (target != card.target)
                {
					target = card.target;
                    count = 0;
                }
                else if (target == card.target)
                {
                    count++;
                    DmgIncrease(card);
                }
            }
            else
            {
                count++;
                target = card.target;
                DmgIncrease(card);
            }
        }

        private void DmgIncrease(BattlePlayingCardDataInUnitModel card)
        {
            if (count > 5)
            {
                count = 5;
            }
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                dmgRate = 10 * count,
                breakRate = 10 * count
            });
        }
    }

    public class PassiveAbility_RMR_CopleyShimmering : PassiveAbilityBase
    {
		private int _patternCount;

		private bool pattern1;

		private List<int> _usedCardType = new List<int>();

		private int _onlyCardCooltime;

		BattleUnitModel indextarget;

        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
			if (unit == indextarget)
            {
				ObtainTarget();
            }
        }

        private void ObtainTarget()
        {
			List<BattleUnitModel> list = BattleObjectManager.instance.GetAliveList(Faction.Player);
			int highesthp = 0;
			if (list.Count > 0)
			{
				foreach (BattleUnitModel guy in list)
				{
					if (guy.hp > highesthp)
					{
						highesthp = (int)guy.hp;
					}
				}
			}
			List<BattleUnitModel> list2 = new List<BattleUnitModel>();
			foreach (BattleUnitModel goober in list)
			{
				if (goober.hp == highesthp)
				{
					list2.Add(goober);
				}
			}
			if (list2.Count > 0)
			{
				indextarget = RandomUtil.SelectOne<BattleUnitModel>(list2);
			}
		}
		public override void OnWaveStart()
		{
			if (indextarget == null)
            {
				ObtainTarget();
            }
			pattern1 = false;
			_usedCardType = new List<int>();
			int stack = MysteryModel_RMR_CopleyIndex2.LosePrescript();
            for (int i = 0; i < stack; i++)
            {
                List<BattleUnitModel> list = BattleObjectManager.instance.GetAliveList(Faction.Enemy).FindAll((BattleUnitModel x) => x.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf y) => y is BattleUnitBuf_RMRPrescriptbuf) == null);
                if (list.Count > 0)
                {
                    RandomUtil.SelectOne<BattleUnitModel>(list).bufListDetail.AddBuf(new BattleUnitBuf_RMRPrescriptbuf());
                }
            }
        }

		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			int item = curCard.card.GetID().id;
			if (!_usedCardType.Contains(item))
			{
				_usedCardType.Add(item);
			}
		}

		public int GetReleaseCard()
		{
			if (_usedCardType.Contains(605009))
			{
				return 605010;
			}
			if (_usedCardType.Contains(605008))
			{
				return 605009;
			}
			return 605008;
		}

		public override void OnRoundStartAfter()
		{
			if (!owner.IsBreakLifeZero())
			{
				SetCards();
				_patternCount++;
				if (_patternCount >= 4)
				{
					_patternCount = 0;
				}
			}
		}


		// this technically is susceptible to surprise box/page cancellation into being locked out of unlock while unlikely, might be necessary to adjust in the future
		public void SetCards()
		{
			owner.allyCardDetail.ExhaustAllCards();
			if (TheIndexCardUtil.IsActivatedRelease(owner))
			{
				_onlyCardCooltime--;
				if (_onlyCardCooltime <= 0)
				{
					AddNewCard(595003); // amputation
					_onlyCardCooltime = 3;
				}
			}
			if (!TheIndexCardUtil.IsActivatedRelease(owner))
			{
				if (_patternCount == 0)
				{
					AddNewCard(605022); // swift trace
					AddNewCard(GetReleaseCard()); // unlock
				}
				if (_patternCount == 1)
				{
					if (!pattern1)
					{
						AddNewCard(605006); // binding chains
						if (owner.bufListDetail.GetKewordBufStack(KeywordBuf.SlashPowerUp) > 0)
                        {
							AddNewCard(605005); // eliminate
						}
						else
                        {
							AddNewCard(595002); // skewer
						}
						pattern1 = true;
					}
					else
					{
						_patternCount++;
					}
				}
				if (_patternCount == 2)
				{
					AddNewCard(GetReleaseCard()); // unlock
					AddNewCard(TheIndexCardUtil.GetCardIdByBuf(owner)); // picks between wotp, execute and undertake prescript depending on bufs
				}
				if (_patternCount == 3)
				{
					AddNewCard(605007);  // multislash
					AddNewCard(TheIndexCardUtil.GetCardIdByBuf(owner)); // picks between wotp, execute and undertake prescript depending on bufs
				}
			}
			else
			{
				AddNewCard(605022); // swift trace
				AddNewCard(TheIndexCardUtil.GetCardIdByBuf(owner)); // picks between wotp, execute and undertake prescript depending on bufs
			}
			int num = 0;
			List<SpeedDice> list = owner.Book.GetSpeedDiceRule(owner).Roll(owner);
			if (list.Count >= 3)
			{
				num = list.Count - 2;
			}
			if (num > 0)
			{
				if (owner.bufListDetail.GetKewordBufStack(KeywordBuf.SlashPowerUp) > 0)
				{
					AddNewCard(605005); // eliminate
				}
				else
				{
					AddNewCard(595002); // skewer
				}
			}
		}

		public override void OnRoundEndTheLast()
		{
		}

		private void AddNewCard(int id)
		{
			owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, id))?.SetCostToZero();
		}
	}

    public class PassiveAbility_RMR_CopleyShimmering2 : PassiveAbilityBase
    {
		private int _patternCount;

		private bool pattern1;

		private List<int> _usedCardType = new List<int>();

		private int _onlyCardCooltime;

		bool amputate;

		BattleUnitModel indextarget;

		public override void OnDieOtherUnit(BattleUnitModel unit)
		{
			base.OnDieOtherUnit(unit);
			if (unit == indextarget)
			{
				ObtainTarget();
			}
		}

		private void ObtainTarget()
		{
			List<BattleUnitModel> list = BattleObjectManager.instance.GetAliveList(Faction.Player);
			int highesthp = 0;
			if (list.Count > 0)
			{
				foreach (BattleUnitModel guy in list)
				{
					if (guy.hp > highesthp)
					{
						highesthp = (int)guy.hp;
					}
				}
			}
			List<BattleUnitModel> list2 = new List<BattleUnitModel>();
			foreach (BattleUnitModel goober in list)
			{
				if (goober.hp == highesthp)
				{
					list2.Add(goober);
				}
			}
			if (list2.Count > 0)
			{
				indextarget = RandomUtil.SelectOne<BattleUnitModel>(list2);
			}
		}

		public override void OnWaveStart()
		{
			if (indextarget == null)
			{
				ObtainTarget();
			}
			pattern1 = false;
			_usedCardType = new List<int>();
			amputate = false;
			owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(1, SephirahType.None));
		}

		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			int item = curCard.card.GetID().id;
			if (!_usedCardType.Contains(item))
			{
				_usedCardType.Add(item);
			}
		}

		public int GetReleaseCard()
		{
			if (_usedCardType.Contains(605009))
			{
				return 605010;
			}
			if (_usedCardType.Contains(605008))
			{
				return 605009;
			}
			return 605008;
		}

		public override void OnRoundStartAfter()
		{
			if (!owner.IsBreakLifeZero())
			{
				SetCards();
				_patternCount++;
				if (_patternCount >= 4)
				{
					_patternCount = 0;
				}
			}
		}


		// this technically is susceptible to surprise box/page cancellation into being locked out of unlock while unlikely, might be necessary to adjust in the future
		public void SetCards()
		{
			owner.allyCardDetail.ExhaustAllCards();
			if (!amputate)
            {
				AddNewCard(595003);
				// removes condition for card so it can be played immediately
				foreach (BattleDiceCardModel card in owner.allyCardDetail.GetHand().FindAll((BattleDiceCardModel x) => x.GetID() == new LorId(LogLikeMod.ModId, 595003)))
                {
					card.XmlData.Script = "";
				}
				amputate = true;
            }
			if (TheIndexCardUtil.IsActivatedRelease(owner))
			{
				_onlyCardCooltime--;
				if (_onlyCardCooltime <= 0)
				{
					AddNewCard(595003); // amputation
					_onlyCardCooltime = 3;
				}
			}
			if (!TheIndexCardUtil.IsActivatedRelease(owner))
			{
				if (_patternCount == 0)
				{
					AddNewCard(605022); // swift trace
					AddNewCard(GetReleaseCard()); // unlock
				}
				if (_patternCount == 1)
				{
					if (!pattern1)
					{
						AddNewCard(605006); // binding chains
						if (owner.bufListDetail.GetKewordBufStack(KeywordBuf.SlashPowerUp) > 0)
						{
							AddNewCard(605005); // eliminate
						}
						else
						{
							AddNewCard(595002); // skewer
						}
						pattern1 = true;
					}
					else
					{
						_patternCount++;
					}
				}
				if (_patternCount == 2)
				{
					AddNewCard(GetReleaseCard()); // unlock
					AddNewCard(TheIndexCardUtil.GetCardIdByBuf(owner)); // picks between wotp, execute and undertake prescript depending on bufs
				}
				if (_patternCount == 3)
				{
					AddNewCard(605007);  // multislash
					AddNewCard(TheIndexCardUtil.GetCardIdByBuf(owner)); // picks between wotp, execute and undertake prescript depending on bufs
				}
			}
			else
			{
				AddNewCard(605022); // swift trace
				AddNewCard(TheIndexCardUtil.GetCardIdByBuf(owner)); // picks between wotp, execute and undertake prescript depending on bufs
			}
			int num = 0;
			List<SpeedDice> list = owner.Book.GetSpeedDiceRule(owner).Roll(owner);
			if (list.Count >= 3)
			{
				num = list.Count - 2;
			}
			if (num > 0)
			{
				if (owner.bufListDetail.GetKewordBufStack(KeywordBuf.SlashPowerUp) > 0)
				{
					AddNewCard(605005); // eliminate
				}
				else
				{
					AddNewCard(595002); // skewer
				}
			}
		}

		public override void OnRoundEndTheLast()
		{
		}

		private void AddNewCard(int id)
		{
			owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, id))?.SetCostToZero();
		}
	}

    public class PassiveAbility_RMR_PrescriptPassive : PassiveAbilityBase
    {
        BattleUnitModel target;
        int count;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            base.OnUseCard(card);
            if (target != null)
            {
                if (target != card.target)
                {
                    count = 0;
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                    {
                        dmgRate = -50,
                        breakRate = -50
                    });
                }
                else if (target == card.target)
                {
                    count++;
                    DmgIncrease(card);
                }
            }
            else
            {
                count++;
                target = card.target;
                DmgIncrease(card);
            }
        }

        private void DmgIncrease(BattlePlayingCardDataInUnitModel card)
        {
            if (count > 3)
            {
                count = 3;
            }
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                dmgRate = 10 * count,
                breakRate = 10 * count
            });
        }
    }

    public class PassiveAbility_RMR_Injured : PassiveAbilityBase
    {
        public override float GetStartHp(float hp)
        {
            return hp / 2;
        }
    }
}
