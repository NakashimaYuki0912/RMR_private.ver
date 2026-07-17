// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch6_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch6_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch6_2</summary>

    public class MysteryModel_Mystery_Ch6_2 : MysteryBase
    {
        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (PassiveAbility_MoneyCheck.GetMoney() >= 10 || id != 0)
                return;
            this.FrameObj["choice_btn3"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc3"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc3"].GetComponent<TextMeshProUGUI>().text;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                if (choiceid != 3)
                {
                    if ((double)UnityEngine.Random.value > 0.5)
                    {
                        if (choiceid == 0)
                        {
                            LogueBookModels.AddMoney(LogueBookModels.GetMoney() / 2);
                            this.SwapFrame(1);
                        }
                        if (choiceid == 1)
                        {
                            foreach (BookModel bookModel in LogueBookModels.booklist.FindAll((Predicate<BookModel>)(x => x.owner != null)))
                                LogueBookModels.AddBook(bookModel.ClassInfo.id);
                            this.SwapFrame(3);
                        }
                        if (choiceid != 2)
                            return;
                        foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Player))
                        {
                            battleUnitModel.RecoverHP(battleUnitModel.MaxHp);
                            if (battleUnitModel.IsDead())
                                battleUnitModel.Revive(battleUnitModel.MaxHp);
                        }
                        this.SwapFrame(5);
                        return;
                    }
                    if (choiceid == 0)
                    {
                        LogueBookModels.SubMoney(LogueBookModels.GetMoney() / 2);
                        this.SwapFrame(2);
                    }
                    if (choiceid == 1)
                    {
                        List<BookModel> all = LogueBookModels.booklist.FindAll((Predicate<BookModel>)(x => x.owner != null));
                        foreach (UnitDataModel model in LogueBookModels.playerModel)
                            LogueBookModels.EquipNewPage(model, LogueBookModels.BaseXmlInfo);
                        foreach (BookModel bookModel in all)
                            LogueBookModels.booklist.Remove(bookModel);
                        this.SwapFrame(4);
                    }
                    if (choiceid == 2)
                        this.SwapFrame(6);
                    return;
                }
                if (LogueBookModels.GetMoney() < 10)
                    return;
                LogueBookModels.SubMoney(10);
            }
            base.OnClickChoice(choiceid);
        }
    }
}
