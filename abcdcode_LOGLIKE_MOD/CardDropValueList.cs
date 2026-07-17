// -----------------------------------------------------------------------------
// Library of Ruina mod script: CardDropValueList
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\CardDropValueList.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;


namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>CardDropValueList</summary>
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
