// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CardDropValueList
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using System.Linq;


namespace abcdcode_LOGLIKE_MOD
{
    public class CardDropValueList : Singleton<CardDropValueList>
    {
        public List<CardDropValueXmlInfo> infos;
        private CardDropValueXmlInfo[] _allInfos;

        public void Init(List<CardDropValueXmlInfo> info)
        {
            this.infos = info;
            this._allInfos = new CardDropValueXmlInfo[info.Count];
            info.CopyTo(this._allInfos);
        }

        public CardDropValueXmlInfo GetData(LorId id, DropType droptype = DropType.Card)
        {
            return this.infos.Find(x => x.Id == id && x.droptype == DropType.Card);
        }

        public void RestoreToDefault()
        {
            this.infos.Clear();
            this.infos.AddRange(this._allInfos.ToList());
        }
    }
}
