// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch5_2_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_Ch5_2_1 : MysteryBase
    {
        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (PassiveAbility_MoneyCheck.GetMoney() >= 60 || id != 0)
                return;
            this.FrameObj["choice_btn1"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc1"].GetComponent<TextMeshProUGUI>().text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc1"].GetComponent<TextMeshProUGUI>().text;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (choiceid == 0)
            {
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new MysteryModel_Mystery_Ch5_2_1.PlayerSetting5_2_1());
                MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 2500021), StageType.Normal);
            }
            if (choiceid == 1 && PassiveAbility_MoneyCheck.GetMoney() >= 60)
            {
                if (PassiveAbility_MoneyCheck.GetMoney() < 60)
                    return;
                LogueBookModels.SubMoney(60);
            }
            base.OnClickChoice(choiceid);
        }

        [HideFromItemCatalog]
        public class PlayerSetting5_2_1 : GlobalLogueEffectBase
        {
            public override void OnStartBattleAfter()
            {
                base.OnStartBattleAfter();
                foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Player))
                {
                    FormationModel formationModel = new FormationModel(Singleton<FormationXmlList>.Instance.GetData(230001));
                    try
                    {
                        battleUnitModel.formation = formationModel.PostionList[battleUnitModel.index];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }
                }
                this.Destroy();
            }
        }
    }
}
