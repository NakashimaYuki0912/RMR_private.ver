// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PuppeteerBuf
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;


namespace abcdcode_LOGLIKE_MOD
{

    public class PuppeteerBuf : BattleUnitBuf
    {
        public BattleUnitModel owner;

        public override string keywordId => "LogueLikeMod_PuppeteerBuf";

        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, LogLikeMod.ArtWorks["buff_Puppeteer"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.owner = owner;
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            base.OnUseCard(card);
            if (PickUpModel_ShopGood25.Shop25Effect.curpuppet == null || !_owner.cardSlotDetail.cardQueue.Contains(card))
                return;
            BattleUnitModel owner = PickUpModel_ShopGood25.Shop25Effect.curpuppet.owner;
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(card.card.GetID()));
            playingCard.costSpended = true;
            Singleton<StageController>.Instance._allCardList.Insert(0, new BattlePlayingCardDataInUnitModel
            {
                owner = owner,
                card = playingCard,
                target = card.target,
                speedDiceResultValue = 99
            });
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }
    }
}
