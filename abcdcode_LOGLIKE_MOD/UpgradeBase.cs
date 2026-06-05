// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.UpgradeBase
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System.Collections.Generic;
using LOR_Localize;
using System;

namespace abcdcode_LOGLIKE_MOD
{

    public class UpgradeBase
    {
        public UpgradeBase.UpgradeInfo upgradeinfo;
        public DiceCardXmlInfo baseinfo;
        public DiceCardXmlInfo upxmlinfo;
        public LorId baseid;
        public int index;

        public virtual void Init()
        {
        }

        public DiceCardXmlInfo CreateUpBase(int index = 0, int count = 1)
        {
            DiceCardXmlInfo upBase = this.baseinfo.Copy(true);
            upBase.workshopID = $"{this.CanRepeatUpgrade().ToString()}:{index.ToString()}:{count.ToString()}{LogCardUpgradeManager.UpgradeKeyword}{this.baseinfo.workshopID}";
            upBase.workshopName = $"{this.baseinfo.workshopName}+{(count > 1 ? count.ToString() : "")}";
            upBase.Spec = this.baseinfo.Spec.Copy();
            return upBase;
        }

        public virtual DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            DiceCardXmlInfo upxmlinfo;
            if (this.upxmlinfo == null || this.upxmlinfo.workshopID.StartsWith("True"))
            {
                this.upxmlinfo = this.CreateUpBase(index, count);
                if (this.upgradeinfo == null)
                {
                    int num = 4 - this.upxmlinfo.DiceBehaviourList.Count > 0 ? 4 - this.upxmlinfo.DiceBehaviourList.Count : 1;
                    foreach (DiceBehaviour diceBehaviour in this.upxmlinfo.DiceBehaviourList)
                        diceBehaviour.Dice += num;
                    upxmlinfo = this.upxmlinfo;
                }
                else
                {
                    if (this.upgradeinfo.selfabilitydata != string.Empty)
                        this.upxmlinfo.Script = this.upgradeinfo.selfabilitydata;
                    foreach (KeyValuePair<int, string> keyValuePair in this.upgradeinfo.abilitydata)
                    {
                        if (keyValuePair.Value != string.Empty)
                            this.upxmlinfo.DiceBehaviourList[keyValuePair.Key].Script = keyValuePair.Value;
                    }
                    if (this.upgradeinfo.costdata != -100)
                    {
                        int num = this.DoesCostReductionStack() ? this.upgradeinfo.costdata * count : this.upgradeinfo.costdata;
                        this.upxmlinfo.Spec.Cost = Math.Max(0, this.baseinfo.Spec.Cost - num);
                    }
                    foreach (KeyValuePair<int, UpgradeBase.UpgradeInfo.DiceChangeData> keyValuePair in this.upgradeinfo.diceChangeData)
                    {
                        this.upxmlinfo.DiceBehaviourList[keyValuePair.Key].Detail = keyValuePair.Value.detail;
                        this.upxmlinfo.DiceBehaviourList[keyValuePair.Key].Type = keyValuePair.Value.type;
                        this.upxmlinfo.DiceBehaviourList[keyValuePair.Key].MotionDetail = keyValuePair.Value.motion;
                    }
                    if (!this.DoesAddedDiceStack())
                    {
                        foreach (DiceBehaviour addedDie in this.upgradeinfo.addedDice)
                            this.upxmlinfo.DiceBehaviourList.Add(addedDie);
                    }
                    for (int index1 = 0; index1 < count; index1++)
                    {
                        foreach (KeyValuePair<int, int> keyValuePair in this.upgradeinfo.mindata)
                            this.upxmlinfo.DiceBehaviourList[keyValuePair.Key].Min += keyValuePair.Value;
                        foreach (KeyValuePair<int, int> keyValuePair in this.upgradeinfo.maxdata)
                            this.upxmlinfo.DiceBehaviourList[keyValuePair.Key].Dice += keyValuePair.Value;
                        if (this.DoesAddedDiceStack())
                        {
                            foreach (DiceBehaviour addedDie in this.upgradeinfo.addedDice)
                                this.upxmlinfo.DiceBehaviourList.Add(addedDie);
                        }
                    }
                    upxmlinfo = this.upxmlinfo;
                }
            }
            else
            {
                this.EditUpInfo(index, count);
                upxmlinfo = this.upxmlinfo;
            }
            return upxmlinfo;
        }

        public virtual bool CanRepeatUpgrade() => false;

        public virtual bool DoesCostReductionStack() => false;

        public virtual bool DoesAddedDiceStack() => false;

        public void EditUpInfo(int index = 0, int count = 1)
        {
            this.upxmlinfo.workshopID = $"{this.CanRepeatUpgrade().ToString()}:{index.ToString()}:{count.ToString()}{LogCardUpgradeManager.UpgradeKeyword}{this.baseinfo.workshopID}";
            this.upxmlinfo.workshopName = $"{this.baseinfo.workshopName}+{(count > 1 ? count.ToString() : "")}";
        }

        public class UpgradeInfo
        {
            public List<DiceBehaviour> addedDice;
            public string selfabilitydata;
            public int costdata;
            public Dictionary<int, int> mindata;
            public Dictionary<int, int> maxdata;
            public Dictionary<int, string> abilitydata;
            public Dictionary<int, UpgradeBase.UpgradeInfo.DiceChangeData> diceChangeData;

            public UpgradeInfo()
            {
                this.selfabilitydata = string.Empty;
                this.costdata = -100;
                this.mindata = new Dictionary<int, int>();
                this.maxdata = new Dictionary<int, int>();
                this.abilitydata = new Dictionary<int, string>();
                this.addedDice = new List<DiceBehaviour>();
                this.diceChangeData = new Dictionary<int, UpgradeBase.UpgradeInfo.DiceChangeData>();
            }

            public void SetAll(string selfscript, List<int> min, List<int> max, List<string> ability)
            {
                this.selfabilitydata = selfscript;
                int key1 = 0;
                foreach (int num in min)
                {
                    this.mindata[key1] = num;
                    ++key1;
                }
                int key2 = 0;
                foreach (int num in max)
                {
                    this.maxdata[key2] = num;
                    ++key2;
                }
                int key3 = 0;
                foreach (string str in ability)
                {
                    this.abilitydata[key3] = str;
                    ++key3;
                }
            }

            public void SetSelfAbility(string script) => this.selfabilitydata = script;

            public void SetCost(int value) => this.costdata = value;

            public void SetDice(int index, int min, int max)
            {
                this.mindata[index] = min;
                this.maxdata[index] = max;
            }

            public void SetMin(int index, int value) => this.mindata[index] = value;

            public void SetMax(int index, int value) => this.maxdata[index] = value;

            public void SetAbility(int index, string script) => this.abilitydata[index] = script;

            public void AddDice(DiceBehaviour behaviour) => this.addedDice.Add(behaviour);

            public void ChangeDiceType(int index, BehaviourDetail detail)
            {
                MotionDetail motionDetail = MotionDetail.G;
                BehaviourType typeShit = BehaviourType.Atk;
                switch (detail)
                {
                    case BehaviourDetail.Slash:
                        motionDetail = MotionDetail.J;
                        break;
                    case BehaviourDetail.Penetrate:
                        motionDetail = MotionDetail.Z;
                        break;
                    case BehaviourDetail.Hit:
                        motionDetail = MotionDetail.H;
                        break;
                    case BehaviourDetail.Guard:
                        motionDetail = MotionDetail.G;
                        break;
                    case BehaviourDetail.Evasion:
                        motionDetail = MotionDetail.E;
                        break;
                }
                switch (detail)
                {
                    case BehaviourDetail.Slash:
                    case BehaviourDetail.Penetrate:
                    case BehaviourDetail.Hit:
                        typeShit = BehaviourType.Atk;
                        break;
                    case BehaviourDetail.Guard:
                    case BehaviourDetail.Evasion:
                        typeShit = BehaviourType.Atk;
                        break;
                }
                this.diceChangeData[index] = new UpgradeBase.UpgradeInfo.DiceChangeData()
                {
                    detail = detail,
                    type = typeShit,
                    motion = motionDetail
                };
            }

            public void AddDice(
              int min,
              int max,
              BehaviourDetail detail,
              string script,
              MotionDetail mdetail,
              BehaviourType btype,
              string effectres,
              string actionscript)
            {
                this.AddDice(new DiceBehaviour()
                {
                    Min = min,
                    Dice = max,
                    Script = script,
                    Detail = detail,
                    MotionDetail = mdetail,
                    Type = btype,
                    EffectRes = effectres,
                    ActionScript = actionscript
                });
            }

            public void AddDice(
              int min,
              int max,
              BehaviourDetail detail,
              string script,
              BehaviourType btype)
            {
                MotionDetail mdetail = MotionDetail.G;
                switch (detail)
                {
                    case BehaviourDetail.Slash:
                        mdetail = MotionDetail.J;
                        break;
                    case BehaviourDetail.Penetrate:
                        mdetail = MotionDetail.Z;
                        break;
                    case BehaviourDetail.Hit:
                        mdetail = MotionDetail.H;
                        break;
                    case BehaviourDetail.Guard:
                        mdetail = MotionDetail.G;
                        break;
                    case BehaviourDetail.Evasion:
                        mdetail = MotionDetail.E;
                        break;
                }
                this.AddDice(min, max, detail, script, mdetail, btype, "", "");
            }

            public class DiceChangeData
            {
                public BehaviourDetail detail;
                public BehaviourType type;
                public MotionDetail motion;
            }
        }
    }
}
