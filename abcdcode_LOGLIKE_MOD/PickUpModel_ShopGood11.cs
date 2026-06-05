// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood11
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood11 : ShopPickUpModel
    {
        public PickUpModel_ShopGood11()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570011));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90011);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return base.IsCanPickUp(target) && !target.IsDead();
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            Singleton<LogAssetBundleManager>.Instance.LoadEffectEachScale(model.view.transform, new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0.0f, 1f), "loglike_smokebullet");
            DiceCardSelfAbility_ozma_forgotten.BattleUnitBuf_ozma_forgotten buf = new DiceCardSelfAbility_ozma_forgotten.BattleUnitBuf_ozma_forgotten();
            model.bufListDetail.AddBuf(buf);
            List<BattleUnitModel> actionableEnemyList = Singleton<StageController>.Instance.GetActionableEnemyList();
            if (model.faction != Faction.Player)
                return;
            for (int index1 = 0; index1 < actionableEnemyList.Count; ++index1)
            {
                BattleUnitModel actor = actionableEnemyList[index1];
                if (actor.turnState != BattleUnitTurnState.BREAK)
                    actor.turnState = BattleUnitTurnState.WAIT_CARD;
                try
                {
                    for (int index2 = 0; index2 < actor.speedDiceResult.Count; ++index2)
                    {
                        if (!actor.speedDiceResult[index2].breaked && index2 < actor.cardSlotDetail.cardAry.Count)
                        {
                            BattlePlayingCardDataInUnitModel cardDataInUnitModel = actor.cardSlotDetail.cardAry[index2];
                            if (cardDataInUnitModel != null && cardDataInUnitModel.card != null)
                            {
                                if (cardDataInUnitModel.target == model)
                                {
                                    this.Log("Target!");
                                    BattleUnitModel targetByCard = BattleObjectManager.instance.GetTargetByCard(actor, cardDataInUnitModel.card, index2, actor.TeamKill());
                                    if (targetByCard != null)
                                    {
                                        int targetSlot = UnityEngine.Random.Range(0, targetByCard.speedDiceResult.Count);
                                        int num = actor.ChangeTargetSlot(cardDataInUnitModel.card, targetByCard, index2, targetSlot, actor.TeamKill());
                                        cardDataInUnitModel.target = targetByCard;
                                        cardDataInUnitModel.targetSlotOrder = num;
                                        cardDataInUnitModel.earlyTarget = targetByCard;
                                        cardDataInUnitModel.earlyTargetOrder = num;
                                    }
                                    else
                                    {
                                        actor.allyCardDetail.ReturnCardToHand(actor.cardSlotDetail.cardAry[index2].card);
                                        actor.cardSlotDetail.cardAry[index2] = (BattlePlayingCardDataInUnitModel)null;
                                    }
                                }
                                else if (cardDataInUnitModel.earlyTarget == model)
                                {
                                    this.Log("Target!2");
                                    BattleUnitModel targetByCard = BattleObjectManager.instance.GetTargetByCard(actor, cardDataInUnitModel.card, index2, actor.TeamKill());
                                    if (targetByCard != null)
                                    {
                                        int targetSlot = UnityEngine.Random.Range(0, targetByCard.speedDiceResult.Count);
                                        int num = actor.ChangeTargetSlot(cardDataInUnitModel.card, targetByCard, index2, targetSlot, actor.TeamKill());
                                        cardDataInUnitModel.earlyTarget = targetByCard;
                                        cardDataInUnitModel.earlyTargetOrder = num;
                                    }
                                    else
                                    {
                                        cardDataInUnitModel.earlyTarget = cardDataInUnitModel.target;
                                        cardDataInUnitModel.earlyTargetOrder = cardDataInUnitModel.targetSlotOrder;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    Debug.LogError("target change error");
                }
            }
            model.bufListDetail.RemoveBuf((BattleUnitBuf)buf);
            SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood11.Hiding());
        }

        public class Hiding : OnceEffect
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive11"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570011));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570011));
            }

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase || BattleObjectManager.instance.GetAliveList(Faction.Player).Count <= 1)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90011));
                this.Use();
            }
        }
    }
}
