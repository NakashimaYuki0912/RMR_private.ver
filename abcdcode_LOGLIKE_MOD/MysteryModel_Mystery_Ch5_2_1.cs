// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch5_2_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch5_2_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch5_2_1</summary>

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
