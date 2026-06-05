// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood46
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModel_ShopGood46 : ShopPickUpModel
    {
        public PickUpModel_ShopGood46()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570046));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90046);
        }

        public override List<BattleUnitModel> GetPickupTarget()
        {
            return BattleObjectManager.instance.GetAliveList(Faction.Enemy);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            if (model == null)
                return;
            List<BattlePlayingCardDataInUnitModel> all = model.cardSlotDetail.cardAry.FindAll((Predicate<BattlePlayingCardDataInUnitModel>)(x => x != null && x.card != null));
            if (all.Count > 0)
            {
                BattlePlayingCardDataInUnitModel selected = RandomUtil.SelectOne<BattlePlayingCardDataInUnitModel>(all);
                model.allyCardDetail.ReturnCardToHand(selected.card);
                int index = model.cardSlotDetail.cardAry.FindIndex((Predicate<BattlePlayingCardDataInUnitModel>)(x => x == selected));
                model.cardSlotDetail.cardAry[index] = (BattlePlayingCardDataInUnitModel)null;
                model.view.speedDiceSetterUI.GetSpeedDiceByIndex(index);
                all.Remove(selected);
            }
            SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood46.SupriseBox());
        }

        public class SupriseBox : OnceEffect
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive46"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570046));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570046));
            }

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90046));
                this.Use();
            }
        }
    }
}
