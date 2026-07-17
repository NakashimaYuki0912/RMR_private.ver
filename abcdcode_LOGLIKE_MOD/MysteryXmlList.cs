// -----------------------------------------------------------------------------
// Library of Ruina mod script: MysteryXmlList
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryXmlList.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>MysteryXmlList</summary>

public class MysteryXmlList : Singleton<MysteryXmlList>
{
  public List<MysteryXmlInfo> info;
  private MysteryXmlInfo[] _allInfos;

  public void Init(List<MysteryXmlInfo> info)
  {
    this.info = info;
    this._allInfos = new MysteryXmlInfo[info.Count];
    info.CopyTo(this._allInfos);
  }

  public MysteryXmlInfo GetData(LorId stageid)
  {
    return this.info.Find((Predicate<MysteryXmlInfo>) (x => x.StageId == stageid));
  }

  public void RestoreToDefault()
  {
    this.info.Clear();
    this.info.AddRange((IEnumerable<MysteryXmlInfo>) ((IEnumerable<MysteryXmlInfo>) this._allInfos).ToList<MysteryXmlInfo>());
  }
}
}
